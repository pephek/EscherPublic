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
        private Editor editor = new Editor();

        private Preview preview = new Preview();

        private Design design;

        public Main()
        {
            InitializeComponent();

            this.FormClosing += new FormClosingEventHandler((sender, e) => MainClosing());

            this.editToolStripMenuItem.Click += new EventHandler((sender, e) => Edit());
        }

        private void Main_Load(object sender, EventArgs e)
        {
            Main_Load();
        }

        private void Main_Load()
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

                refreshPageFormatsToolStripMenuItem_Click(null, null);

                SetMenus(enabled: false);

                designsToolStripMenuItem.Visible = false;

                refreshDesignsToolStripMenuItem_Click(null, null);

                webBrowser.Navigating += WebBrowser_Navigating;
                webBrowser.IsWebBrowserContextMenuEnabled = false;
                webBrowser.AllowWebBrowserDrop = false;

                PageSetup.Load();

                LoadDesign("_ Test");
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

        private void Edit()
        {
            editor.Show();
            editor.Invalidate();
            editor.Activate();
        }

        private void refreshDesignsToolStripMenuItem_Click(object sender, EventArgs eventArgs)
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

            designsToolStripMenuItem.DropDownItems.Clear();

            string letter = "";

            foreach (string design in designs)
            {
                string firstLetter = design.Substring(0, 1).ToUpper();

                if (letter != firstLetter)
                {
                    designsToolStripMenuItem.DropDownItems.Add(new ToolStripMenuItem(firstLetter));
                    letter = firstLetter;
                }
            }

            foreach (ToolStripDropDownItem item in designsToolStripMenuItem.DropDownItems)
            {
                foreach (string design in designs)
                {
                    if (item.Text.Substring(0, 1).ToUpper() == design.Substring(0, 1).ToUpper())
                    {
                        ToolStripMenuItem itemDesign = new ToolStripMenuItem(design);

                        item.DropDownItems.Add(itemDesign);

                        itemDesign.Click += (s, e) => LoadDesign(design);
                    }
                }
            }

            designsToolStripMenuItem.Visible = (designsToolStripMenuItem.DropDownItems.Count != 0);
        }

        private void refreshPageFormatsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PageFormats.Load(App.GetSetting("PageFormats"));
        }

        private void findStampNumberToolStripMenuItem_Click(object sender, EventArgs e)
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

        private void findPageNumberToolStripMenuItem_Click(object sender, EventArgs e)
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

        private void findAlbumNumberToolStripMenuItem_Click(object sender, EventArgs e)
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

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(string.Format("Are you sure you want to exit from the {0} application?", App.GetName()), App.GetName() + " · Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                System.Environment.Exit(1);
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About about = new About();
            about.ShowDialog();
        }

        private void SetMenus(bool enabled)
        {
            editToolStripMenuItem.Enabled = enabled;
            reportToolStripMenuItem.Enabled = enabled;
            printDocumentToolStripMenuItem.Enabled = enabled;
            publishToolStripMenuItem.Enabled = enabled;
            findStampNumberToolStripMenuItem.Enabled = enabled;
            findPageNumberToolStripMenuItem.Enabled = enabled;
            findAlbumNumberToolStripMenuItem.Enabled = enabled;
        }

        private void LoadDesign(string designFile)
        {
            string error;

            SetMenus(enabled: false);

            designFile = App.GetSetting("DesignsFolder") + "\\" + designFile + ".cdb";

            this.editor.SetDesign(designFile);

            DesignValidator designValidator = new DesignValidator();

            if (!designValidator.Parse(this.editor.GetDesign(), SetProgress, out error))
            {
                MessageBox.Show(string.Format("Invalid design: {0}", error), App.GetName(), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                DesignParser designParser = new DesignParser();

                design = designParser.Parse(this.editor.GetDesign(), SetProgress, out error);

                if (!string.IsNullOrEmpty(error))
                {
                    MessageBox.Show(string.Format("Invalid design: {0}", error), App.GetName(), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    webBrowser.DocumentText = HtmlHelper.GetDesignInHtml(design);

                    SetMenus(enabled: true);
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
