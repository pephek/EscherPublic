using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace Escher
{
    public partial class Main : Form
    {
        private readonly static Validator validator = new Validator();
        private readonly static Preview preview = new Preview();
        private readonly Editor editor = new Editor(validator, preview);

        private Design design;

        public Main()
        {
            InitializeComponent();

            this.Load += new EventHandler((sender, e) => Initialize());
            this.FormClosing += new FormClosingEventHandler((sender, e) => MainClosing());

            this.menuEdit.Click += new EventHandler((sender, e) => EditDesign());
            this.menuPrint.Click += new EventHandler((sender, e) => PrintDesign());
            this.menuRefreshDesigns.Click += new EventHandler((sender, e) => RefreshDesigns());
            this.menuRefreshFormats.Click += new EventHandler((sender, e) => RefreshFormats());
            this.menuFindStampNumber.Click += new EventHandler((sender, e) => FindStampNumber());
            this.menuFindPageNumber.Click += new EventHandler((sender, e) => FindPageNumber());
            this.menuFindAlbumNumber.Click += new EventHandler((sender, e) => FindAlbumNumber());
            this.menuExit.Click += new EventHandler((sender, e) => Exit());
            this.menuAbout.Click += new EventHandler((sender, e) => About());
        }

        private void Initialize()
        {
            try
            {
                #region Restore Window State
                if (Properties.Settings.Default.MainSize.Width == 0)
                {
                    Properties.Settings.Default.Upgrade();
                }
                if (Properties.Settings.Default.MainSize.Width == 0 || Properties.Settings.Default.MainSize.Height == 0)
                {
                    this.Location = new Point(10, 10);
                    this.Size = new Size(512, 512);
                }
                else
                {
                    this.WindowState = Properties.Settings.Default.MainState;

                    if (this.WindowState == FormWindowState.Minimized)
                    {
                        this.WindowState = FormWindowState.Normal;
                    }

                    this.Location = Properties.Settings.Default.MainLocation;
                    this.Size = Properties.Settings.Default.MainSize;
                }
                #endregion

                bool exists = System.Diagnostics.Process.GetProcessesByName(System.IO.Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetEntryAssembly().Location)).Count() > 1;

                if (exists)
                {
                    App.SetException(string.Format("The {0} application is already running and can not run more than once at the same time.", App.GetName()));
                }

                string documentsFolder = App.GetSetting("DocumentsFolder");

                if (!Directory.Exists(documentsFolder))
                {
                    Directory.CreateDirectory(documentsFolder);
                }

                string designsFolder = App.GetSetting("DesignsFolder");

                if (!Directory.Exists(designsFolder))
                {
                    Directory.CreateDirectory(designsFolder);
                }

                string designsRollbackFolder = App.GetSetting("DesignsRollbackFolder");

                if (!Directory.Exists(designsRollbackFolder))
                {
                    Directory.CreateDirectory(designsRollbackFolder);
                }

                RefreshFormats();

                SetMenus(enabled: false);

                menuOpen.Visible = false;

                RefreshDesigns();

                webBrowser.Navigating += WebBrowser_Navigating;
                webBrowser.IsWebBrowserContextMenuEnabled = false;
                webBrowser.AllowWebBrowserDrop = false;

                PageSetup.Load();

                OpenDesign("_ Test");
            }
            catch (Exception exception)
            {
                App.SetException(exception.Message);
            }
        }

        private void MainClosing()
        {
            #region Save Window State
            Properties.Settings.Default.MainState = this.WindowState;
            if (this.WindowState == FormWindowState.Normal)
            {
                Properties.Settings.Default.MainLocation = this.Location;
                Properties.Settings.Default.MainSize = this.Size;
            }
            else
            {
                Properties.Settings.Default.MainLocation = this.RestoreBounds.Location;
                Properties.Settings.Default.MainSize = this.RestoreBounds.Size;
            }
            Properties.Settings.Default.Save();
            #endregion
        }

        private void PrintDesign()
        {
            DesignEntry country = this.design.GetCountry(1);

            bool excludeNumber = country.Settings.ToLower().Contains("!includenumber");
            bool excludeValueAndColor = country.Settings.ToLower().Contains("!includevalueandcolor");

            Print print = new Print(PrintMode.ToDocument, excludeNumber, excludeValueAndColor);

            DialogResult result = print.ShowDialog();

            if (result == DialogResult.OK)
            {
                bool retry = true;

                while (retry)
                {
                    try
                    {
                        PageSetup setup = PageSetup.Load();

                        string pdfName = design.GetPdf();

                        if (setup.Catalog != Catalog.None)
                        {
                            pdfName += "_" + setup.Catalog.ToString().ToLower();
                        }

                        pdfName += "_" + setup.PageFormat.FormatName;

                        if (setup.IncludeMarginForPunchHoles)
                        {
                            pdfName += "_offcenter";
                        }

                        if (setup.IncludeSamplePagesOnly)
                        {
                            pdfName += "_sample";
                        }

                        if (setup.FontSize == FontSize.Medium)
                        {
                            pdfName += "_font6";
                        }
                        else if (setup.FontSize == FontSize.Large)
                        {
                            pdfName += "_font7";
                        }

                        PDF995Helper pdfHelper = new PDF995Helper(pdfName);

                        Progress progress = new Progress(this.design.NumberOfPages());
                        progress.Show();
                        progress.Refresh();

                        preview.PrintDocument(App.GetSetting("PDFPrinter"), setup.PageFormat.PageWidth, setup.PageFormat.PageHeight, this.design, progress.SetProgress);

                        progress.SetWaiting();

                        pdfHelper.WaitForCompletion();

                        progress.Close();

                        retry = false;
                    }
                    catch (Exception e)
                    {
                        if (MessageBox.Show(string.Format("Exception printing design '{0}':\n\n{1}", editor.GetDesignName(), e.Message), App.GetName() + " · Exception", MessageBoxButtons.RetryCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1) == DialogResult.Cancel)
                        {
                            retry = false;
                        }
                    }
                }
            }
        }

        private void EditDesign()
        {
            editor.Show();
            editor.Invalidate();
            editor.Activate();
            editor.SetError();
        }

        private void RefreshDesigns()
        {
            string[] designFiles = Directory.GetFiles(App.GetSetting("DesignsFolder"), "*.cdb", SearchOption.TopDirectoryOnly);

            List<string> designs = new List<string>();

            foreach (string designFile in designFiles)
            {
                string[] split = designFile.Split('\\');
                string design = split[split.Count() - 1];
                split = design.Split('.');
                designs.Add(split[0]);
            }

            designs.Sort();

            menuOpen.DropDownItems.Clear();

            string letter = "";

            foreach (string design in designs)
            {
                string firstLetter = design.Substring(0, 1).ToUpper();

                if (letter != firstLetter)
                {
                    menuOpen.DropDownItems.Add(new ToolStripMenuItem(firstLetter));
                    letter = firstLetter;
                }
            }

            foreach (ToolStripDropDownItem item in menuOpen.DropDownItems)
            {
                foreach (string design in designs)
                {
                    if (item.Text.Substring(0, 1).ToUpper() == design.Substring(0, 1).ToUpper())
                    {
                        ToolStripMenuItem itemDesign = new ToolStripMenuItem(design);

                        item.DropDownItems.Add(itemDesign);

                        itemDesign.Click += (s, e) => OpenDesign(design);
                    }
                }
            }

            menuOpen.Visible = (menuOpen.DropDownItems.Count != 0);
        }

        private void RefreshFormats()
        {
            PageFormats.Load(App.GetSetting("PageFormats"));
        }

        private void FindStampNumber()
        {
            bool found = true;

            string input = Interaction.InputBox("Stamp number:", App.GetName() + " · Find stamp number");

            if (input != "")
            {
                DesignEntry entry = design.FindStampNumber(input);

                if (entry == null)
                {
                    found = false;
                }
                else
                {
                    entry = design.FindPageNumber(entry.PageNumber);

                    if (entry == null)
                    {
                        found = false;
                    }
                    else
                    {
                        HtmlElement element = webBrowser.Document.GetElementById(string.Format("page({0},{1})", entry.PageNumber, entry.AlbumNumber));

                        if (element == null)
                        {
                            found = false;
                        }
                        else
                        {
                            element.ScrollIntoView(true);
                        }
                    }
                }

                if (!found)
                {
                    MessageBox.Show(string.Format("Stamp number '{0}' does not exist.", input), App.GetName() + " · Find stamp number", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void FindPageNumber()
        {
            bool found = true;

            string input = Interaction.InputBox("Page number:", App.GetName() + " · Find page number");

            int page;

            if (input != "")
            {
                if (!Int32.TryParse(input, out page))
                {
                    found = false;
                }
                else
                {
                    DesignEntry entry = design.FindPageNumber(page);

                    if (entry == null)
                    {
                        found = false;
                    }
                    else
                    {
                        HtmlElement element = webBrowser.Document.GetElementById(string.Format("page({0},{1})", entry.PageNumber, entry.AlbumNumber));

                        if (element == null)
                        {
                            found = false;
                        }
                        else
                        {
                            element.ScrollIntoView(true);
                        }
                    }
                }

                if (!found)
                {
                    MessageBox.Show(string.Format("Page number '{0}' does not exist.", page), App.GetName() + " · Find page number", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void FindAlbumNumber()
        {
            bool found = true;

            string input = Interaction.InputBox("Album number:", App.GetName() + " · Find album number");

            if (input != "")
            {
                DesignEntry entry = design.FindAlbumNumber(input);

                if (entry == null)
                {
                    found = false;
                }
                else
                {
                    HtmlElement element = webBrowser.Document.GetElementById(string.Format("page({0},{1})", entry.PageNumber, entry.AlbumNumber));

                    if (element == null)
                    {
                        found = false;
                    }
                    else
                    {
                        element.ScrollIntoView(true);
                    }
                }

                if (!found)
                {
                    MessageBox.Show(string.Format("Album number '{0}' does not exist.", input), App.GetName() + " · Find album number", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void Exit()
        {
            if (MessageBox.Show(string.Format("Are you sure you want to exit from the {0} application?", App.GetName()), App.GetName() + " · Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                System.Environment.Exit(1);
            }
        }

        private void About()
        {
            About about = new About();
            about.ShowDialog();
        }

        private void SetMenus(bool enabled)
        {
            menuEdit.Enabled = enabled;
            menuReport.Enabled = enabled;
            menuPrint.Enabled = enabled;
            menuPublish.Enabled = enabled;
            menuFindStampNumber.Enabled = enabled;
            menuFindPageNumber.Enabled = enabled;
            menuFindAlbumNumber.Enabled = enabled;
        }

        private void Reopen(string designText)
        {
            DesignParser designParser = new DesignParser();

            this.design = designParser.Parse(designText, SetProgress, out string error);

            webBrowser.DocumentText = HtmlHelper.GetDesignInHtml(design);

            SetMenus(enabled: true);
        }

        private void OpenDesign(string designName)
        {
            bool retry = true;

            while (retry)
            {
                try
                {
                    string error = null;

                    SetMenus(enabled: false);

                    // webBrowser.Navigate("about:blank");

                    editor.SetDesign(designName, Reopen);

                    if (validator.Parse(editor.GetDesign(), SetProgress, out error))
                    {
                        DesignParser designParser = new DesignParser();

                        this.design = designParser.Parse(editor.GetDesign(), SetProgress, out error);
                    }

                    if (!string.IsNullOrEmpty(error))
                    {
                        if (MessageBox.Show(string.Format("Invalid design '{0}': {1}\n\nDo you want to edit the design?", designName, error), App.GetName() + " · Validating Design", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                        {
                            editor.Show();
                            editor.SetError(error);
                        }
                    }
                    else
                    {
                        webBrowser.DocumentText = HtmlHelper.GetDesignInHtml(design);

                        SetMenus(enabled: true);
                    }

                    retry = false;
                }
                catch (Exception e)
                {
                    if (MessageBox.Show(string.Format("Exception opening design '{0}':\n\n{1}", designName, e.Message), App.GetName() + " · Exception", MessageBoxButtons.RetryCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1) == DialogResult.Cancel)
                    {
                        retry = false;
                    }
                }
            }
        }

        private void webBrowser_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                e.IsInputKey = true;
            }
        }

        private void WebBrowser_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            if (e.Url.AbsolutePath.StartsWith("page(") && e.Url.AbsolutePath.EndsWith(""))
            {
                e.Cancel = true;

                PageSetup.Load();

                int pageNumber = Int32.Parse(e.Url.AbsolutePath.Replace("page(", "").Replace(")", ""));

                preview.SetPreview(design, pageNumber: pageNumber, printMode: PrintMode.ToScreen, screenMode: ScreenMode.MatchScreenHeight);
                preview.Invalidate();
                preview.Show();
                preview.Activate();
            }
            else if (e.Url.AbsolutePath.StartsWith("stamp("))
            {
                int index = Int32.Parse(e.Url.AbsolutePath.Replace("stamp(", "").Replace(")", ""));

                DesignEntry stamp = design[index];

                Design stamps = design.GetStampsFromSeries(pageNumber: stamp.PageNumber, number: stamp.Number);

                Imaging imaging = new Imaging();

                imaging.SetImage(
                    series: stamps,
                    stampNumber: stamp.Number,
                    folder: App.GetSetting("ImagesFolder"),
                    country: design.GetCountry(stamp.PageNumber).Text,
                    section: design.GetSection(stamp.PageNumber).Text
                );

                if (imaging.ShowDialog() == DialogResult.OK)
                {
                    webBrowser.Refresh();
                }

                e.Cancel = true;
            }
        }

        private void SetProgress(int progress)
        {

        }

        private void experimentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Experiments.ExperimentWithSomething();
        }
    }
}
