using FastColoredTextBoxNS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Escher
{
    public partial class Editor : Form
    {
        private readonly TextStyle keywordStyle = new TextStyle(Brushes.Blue, null, FontStyle.Regular);
        private readonly TextStyle separatorStyle = new TextStyle(Brushes.Silver, null, FontStyle.Regular);
        private readonly TextStyle commentStyle = new TextStyle(Brushes.Green, null, FontStyle.Italic);
        private readonly TextStyle feedStyle = new TextStyle(Brushes.Maroon, null, FontStyle.Bold);
        private readonly TextStyle importantStyle = new TextStyle(Brushes.Red, null, FontStyle.Regular);
        private readonly TextStyle enumStyle = new TextStyle(Brushes.SteelBlue, null, FontStyle.Regular);
        private readonly TextStyle htmlStyle = new TextStyle(Brushes.Magenta, null, FontStyle.Regular);

        private Dictionary<string, string> specials = new Dictionary<string, string>();

        private Validator validator;
        private Preview preview;

        private Action reload;

        private string designName;

        public bool IsDirty;

        public Editor(Validator validator, Preview preview)
        {
            InitializeComponent();

            this.Load += new EventHandler((sender, e) => Initialize());

            this.menuFind.Click += new EventHandler((sender, e) => design.ShowFindDialog());
            this.menuReplace.Click += new EventHandler((sender, e) => design.ShowReplaceDialog());
            this.menuPreview.Click += new EventHandler((sender, e) => App.TryRun(PreviewDesign));
            this.menuBeautify.Click += new EventHandler((sender, e) => App.TryRun(BeautifyDesign));
            this.menuValidate.Click += new EventHandler((sender, e) => App.TryRun(ValidateDesign));
            this.menuSave.Click += new EventHandler((sender, e) => App.TryRun(SaveDesign));
            this.menuExit.Click += new EventHandler((sender, e) => App.TryRun(ExitDesign));
            this.menuKeywordAssignment.Click += new EventHandler((sender, e) => App.TryRun(KeywordAssignment));

            this.validator = validator;
            this.preview = preview;

            design.TextChanged += new EventHandler<TextChangedEventArgs>((sender, e) => Recolor(e));
            design.KeyUp += new KeyEventHandler((sender, e) => Parse(e));
            design.MouseUp += new MouseEventHandler((sender, e) => Parse(null));
            design.KeyPressed += new KeyPressEventHandler((sender, e) => Replace(e));
        }

        private void Initialize()
        {
            #region Restore Window State
            if (Properties.Settings.Default.EditorSize.Width == 0)
            {
                Properties.Settings.Default.Upgrade();
            }
            if (Properties.Settings.Default.EditorSize.Width == 0 || Properties.Settings.Default.EditorSize.Height == 0)
            {
                this.Location = new Point(10, 10);
                this.Size = new Size(512, 512);
            }
            else
            {
                this.WindowState = Properties.Settings.Default.EditorState;

                if (this.WindowState == FormWindowState.Minimized)
                {
                    this.WindowState = FormWindowState.Normal;
                }

                this.Location = Properties.Settings.Default.EditorLocation;
                this.Size = Properties.Settings.Default.EditorSize;
            }
            #endregion

            this.specials = new Dictionary<string, string>();

            this.specials.Add("`a", "à");
            this.specials.Add("'a", "á");
            this.specials.Add("^a", "â");
            this.specials.Add("\"a", "ä");
            this.specials.Add("`e", "è");
            this.specials.Add("'e", "é");
            this.specials.Add("^e", "ê");
            this.specials.Add("\"e", "ë");
            this.specials.Add("`i", "ì");
            this.specials.Add("'i", "í");
            this.specials.Add("^i", "î");
            this.specials.Add("\"i", "ï");
            this.specials.Add("`o", "ò");
            this.specials.Add("'o", "ó");
            this.specials.Add("^o", "ô");
            this.specials.Add("\"o", "ö");
            this.specials.Add("`u", "ù");
            this.specials.Add("'u", "ú");
            this.specials.Add("^u", "û");
            this.specials.Add("\"u", "ü");
            this.specials.Add("~a", "ã");
            this.specials.Add("~o", "õ");
            this.specials.Add("~n", "ñ");
            this.specials.Add(",c", "ç");
            this.specials.Add("/4", "¼");
            this.specials.Add("/2", "½");
            this.specials.Add("/3", "¾");
            this.specials.Add("<<", "«");
            this.specials.Add(">>", "»");
            this.specials.Add("SS", "ß");
            this.specials.Add("..", "·");
            this.specials.Add("xx", "×");
            this.specials.Add("^0", "°");
            this.specials.Add("^1", "¹");
            this.specials.Add("^2", "²");
            this.specials.Add("^3", "³");
            this.specials.Add("||", "¦");
            this.specials.Add("==", "≡");
            this.specials.Add(",,", "¸");

            foreach (string special in specials.Values)
            {
                ToolStripMenuItem menuSpecial = new ToolStripMenuItem(special);

                menuInsert.DropDownItems.Add(menuSpecial);

                menuSpecial.Click += (s, e) => Insert(special);
            }
        }

        public void SetDesign(string designName, Action reload)
        {
            this.designName = designName;

            if (reload != null)
            {
                this.reload = reload;
            }

            string designPath = string.Format("{0}\\{1}.album", App.GetSetting("AlbumsFolder"), this.designName);

            design.Text = File.ReadAllText(designPath, Encoding.GetEncoding("iso-8859-1"));

            this.IsDirty = false;

            menuSave.Enabled = false;

            this.Text = string.Format("Escher ̣̤̤· Editing {0}", designName);
        }

        public void SetError(string error = null)
        {
            if (!string.IsNullOrEmpty(error))
            {
                design.SelectionStart = validator.SelectionStart();
                design.SelectionLength = validator.SelectionLength();
                design.DoSelectionVisible();
                SetStatus(error, failure: true);
            }
            else
            {
                SetStatus();
            }
        }

        public string GetDesignName()
        {
            return this.designName;
        }

        public string GetDesignText()
        {
            return design.Text;
        }

        private void SetStatus(string text = "", bool success = false, bool failure = false)
        {
            status.ForeColor = (success ? Color.Green : (failure ? Color.Red : Color.Black));
            status.Text = text;
            status.Refresh();
        }

        private void Insert(string special)
        {
            design.InsertText(special);
        }

        private void Replace(KeyPressEventArgs e)
        {
            if (design.SelectionStart > 1)
            {
                string one = design.Text[design.SelectionStart - 2].ToString();
                string two = e.KeyChar.ToString();

                if (this.specials.TryGetValue(one + two, out string special))
                {
                    design.SelectionStart -= 2;
                    design.SelectionLength = 2;
                    design.ClearSelected();
                    design.InsertText(special);
                }
            }
        }

        private void Parse(KeyEventArgs e)
        {
            if (e != null)
            {
                if (e.Modifiers == Keys.Alt)
                {
                    if (e.KeyCode == Keys.F || e.KeyCode == Keys.H || e.KeyCode == Keys.P || e.KeyCode == Keys.V || e.KeyCode == Keys.S || e.KeyCode == Keys.X)
                    {
                        return;
                    }
                }
            }

            if (design.SelectionStart <= 0)
            {
                return;
            }

            string line = design.GetLineText(design.PositionToPlace(design.SelectionStart).iLine);

            if (!validator.Parse(line, out string error))
            {
                SetStatus(error, failure: true);
            }
            else
            {
                SetStatus();
            }
        }

        private void Recolor(TextChangedEventArgs e)
        {
            e.ChangedRange.ClearStyle(StyleIndex.All);

            e.ChangedRange.SetStyle(separatorStyle, @"=|\|");

            e.ChangedRange.SetStyle(htmlStyle, @"!|<br>|%|&nbsp;|&#8470;|<b>|</b>|<i>|</i>|<s>|</s>");

            //e.ChangedRange.SetStyle(CommentStyle, @"//.*$", RegexOptions.Multiline);

            e.ChangedRange.SetStyle(commentStyle, @"'.*$", RegexOptions.Multiline);

            e.ChangedRange.SetStyle(keywordStyle, @"\b(Alignment|Appearance|Catalogue|Color|Combine|Comment|Copyright|Description|Design|FontOfDescription|FontOfType|FrameColor|Height|Html|Horizontal|Issued|LineFeed|Menu|Overprint|PageNumber|PageTitle|PageSubTitle|Pdf|Perforation|Perfs|Picture|Positions|Printed|Private|Sample|Separate|Series|Settings|Size|Skip|Stamp|Type|Unlisted|Value|Varieties|Variety|Version|Vertical|Width)\b");

            e.ChangedRange.SetStyle(enumStyle, @"\b(Black|Centered|False|HexagonVertical|Left|Rectangle|RectangleRotated|Right|Rotated|Triangle45|Triangle45Inverted|Triangle60|Triangle60Inverted|True|White)\b");

            e.ChangedRange.SetStyle(feedStyle, @"\b(Album|Country|Part|PageFeed|End)\b");

            e.ChangedRange.SetStyle(importantStyle, @"\b(ApplyTo|ApplyToFrameStyle|Thin|Thick)\b");
            e.ChangedRange.SetStyle(importantStyle, @"=VB|=C#");

            this.IsDirty = true;

            menuSave.Enabled = true;

            if (!this.Text.EndsWith("*"))
            {
                this.Text += "*";
            }
        }

        private void PreviewDesign()
        {
            if (design.SelectionStart <= 0)
            {
                return;
            }

            DesignParser designParser = new DesignParser();

            Design d = designParser.Parse(design.Text, null, out string error);

            if (!string.IsNullOrEmpty(error))
            {
                SetStatus(error, failure: true);

                preview.Hide();
            }    
            else
            {
                int pageNumber = 1;

                int pageFeedIndex = design.Text.LastIndexOf("PageFeed", design.SelectionStart);

                if (pageFeedIndex >= 0)
                {
                    string pageFeed = design.GetLineText(design.PositionToPlace(pageFeedIndex).iLine);

                    string[] split = pageFeed.Split("PageNumber=");

                    if (split.Length > 1)
                    {
                        string albumNumber = split[1].Split(' ')[0];

                        DesignEntry entry = d.FindAlbumNumber(albumNumber);

                        if (entry != null)
                        {
                            pageNumber = entry.PageNumber;
                        }
                    }
                }

                preview.Show();
                preview.ShowPreview(d, pageNumber: pageNumber, printMode: PrintMode.ToScreen, screenMode: ScreenMode.MatchScreenHeight);

                this.Focus();
            }
        }

        private void BeautifyDesign()
        {
            if (!design.SelectedText.Contains("\r\n"))
            {
                return;
            }

            Dictionary<int, int> maxima = new Dictionary<int, int>();

            string selectedText = design.SelectedText.Replace(" " + Validator.cKeywordSeparator + " Separate=False", "");

            if (design.SelectedText.EndsWith("\r\n"))
            {
                selectedText = selectedText.Substring(0, selectedText.Length - 2);
            }

            string[] lines = selectedText.Split("\r\n");

            for (int line = 0; line < lines.Length; line++)
            {
                string trim = lines[line].Trim();

                if (trim.StartsWith("Variety=") || trim.StartsWith("Stamp="))
                {
                    string[] pairs = trim.Split(Validator.cKeywordSeparator);

                    for (int pair = 0; pair < pairs.Length; pair++)
                    {
                        if (!maxima.ContainsKey(pair))
                        {
                            maxima.Add(pair, 0);
                        }

                        maxima[pair] = Math.Max(maxima[pair], pairs[pair].Trim().Length);
                    }
                }
            }

            StringBuilder replacement = new StringBuilder();

            for (int line = 0; line < lines.Length; line++)
            {
                string trim = lines[line].Trim();

                if (trim.StartsWith("Variety=") || trim.StartsWith("Stamp="))
                {
                    replacement.Append("    ");

                    string[] pairs = trim.Split(Validator.cKeywordSeparator);

                    for (int pair = 0; pair < pairs.Length; pair++)
                    {
                        replacement.Append(pairs[pair].Trim()).Append(new string(' ', maxima[pair] - pairs[pair].Trim().Length));

                        if (pair < pairs.Length - 1)
                        {
                            replacement.Append(' ').Append(Validator.cKeywordSeparator).Append(' ');
                        }
                    }

                }
                else
                {
                    replacement.Append(lines[line]);
                }

                if (line < lines.Length - 1)
                {
                    replacement.Append("\r\n");
                }
            }

            if (design.SelectedText.EndsWith("\r\n"))
            {
                replacement.Append("\r\n");
            }

            design.SelectedText = replacement.ToString();
        }

        private void ValidateDesign()
        {
            SetStatus("Validating design...");

            Thread.Sleep(100);

            if (!validator.Parse(design.Text, null, out string error))
            {
                SetError(error);
            }
            else
            {
                SetStatus("Valid design", success: true);
            }
        }

        private void SaveDesign()
        {
            SetStatus("Validating design...");

            Thread.Sleep(100);

            if (!validator.Parse(design.Text, null, out string error))
            {
                SetError(error);
            }
            else
            {
                SetStatus();

                string designPath = string.Format("{0}\\{1}.album", App.GetSetting("AlbumsFolder"), this.designName);
                string archivePath = string.Format("{0}\\{1}.album", App.GetSetting("ArchiveFolder"), this.designName);

                for (int archive = 4; archive >= 0; archive--)
                {
                    if (File.Exists(archivePath + "." + archive.ToString()))
                    {
                        File.Copy(archivePath + "." + archive.ToString(), archivePath + "." + (archive + 1).ToString(), overwrite: true);
                    }
                }

                if (File.Exists(designPath))
                {
                    File.Copy(designPath, archivePath + ".0", overwrite: true);
                }

                File.WriteAllText(designPath, design.Text, Encoding.GetEncoding("iso-8859-1"));

                this.IsDirty = false;

                menuSave.Enabled = false;

                this.Text = string.Format("Escher ̣̤̤· Editing {0}", designName);

                App.TryRun(this.reload);
            }
        }

        private void ExitDesign()
        {
            if (this.IsDirty)
            {
                if (MessageBox.Show("The design has unsaved changed, do you want to discard the changes?", App.GetName() + " · Discard changes", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
                {
                    return;
                }
            }

            SetDesign(this.designName, null);

            preview.Hide();

            #region Save Window State
            Properties.Settings.Default.EditorState = this.WindowState;
            if (this.WindowState == FormWindowState.Normal)
            {
                Properties.Settings.Default.EditorLocation = this.Location;
                Properties.Settings.Default.EditorSize = this.Size;
            }
            else
            {
                Properties.Settings.Default.EditorLocation = this.RestoreBounds.Location;
                Properties.Settings.Default.EditorSize = this.RestoreBounds.Size;
            }
            Properties.Settings.Default.Save();
            #endregion

            this.Hide();
        }

        private void KeywordAssignment()
        {
            design.Text = design.Text.Replace(":=", Validator.cKeywordAssignment.ToString());
        }
    }
}
