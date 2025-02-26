﻿using FastColoredTextBoxNS;
using Microsoft.VisualBasic;
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
        private readonly TextStyle pageStyle = new TextStyle(Brushes.Black, null, FontStyle.Bold);

        private Dictionary<string, string> specials = new Dictionary<string, string>();

        private Validator validator;
        private Preview preview;

        private Action reload;

        private string designName;

        private Dictionary<string, string> comments = new Dictionary<string, string>();
        private Dictionary<string, string> sizes = new Dictionary<string, string>();
        private HashSet<string> sizesToSkip = new HashSet<string>();

        public bool IsDirty;

        public Editor(Validator validator, Preview preview)
        {
            InitializeComponent();

            this.Load += new EventHandler((sender, e) => Initialize());

            this.menuFindStampNumber.Click += new EventHandler((sender, e) => App.TryRun(FindStampNumber));
            this.menuFindAlbumNumber.Click += new EventHandler((sender, e) => App.TryRun(FindAlbumNumber));
            this.menuReplace.Click += new EventHandler((sender, e) => designMaster.ShowReplaceDialog());
            this.menuPreview.Click += new EventHandler((sender, e) => App.TryRun(PreviewDesign));
            this.menuBeautify.Click += new EventHandler((sender, e) => App.TryRun(BeautifyDesign));
            this.menuSplit.Click += new EventHandler((sender, e) => App.TryRun(SplitDesign));
            this.menuValidate.Click += new EventHandler((sender, e) => App.TryRun(ValidateDesign));
            this.menuSave.Click += new EventHandler((sender, e) => App.TryRun(SaveDesign));
            this.menuExit.Click += new EventHandler((sender, e) => App.TryRun(ExitDesign));
            this.menuKeywordAssignment.Click += new EventHandler((sender, e) => App.TryRun(KeywordAssignment));
            this.menuRefresh.Click += new EventHandler((sender, e) => menuRefresh.Checked = !menuRefresh.Checked);

            this.validator = validator;
            this.preview = preview;

            designMaster.TextChanged += new EventHandler<TextChangedEventArgs>((sender, e) => Recolor(e));
            designMaster.KeyUp += new KeyEventHandler((sender, e) => Parse(e));
            designMaster.MouseUp += new MouseEventHandler((sender, e) => Parse(null));
            designMaster.KeyPressed += new KeyPressEventHandler((sender, e) => Replace(e));
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
            this.specials.Add("\"O", "Ö");
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

            designMaster.Text = File.ReadAllText(designPath, Encoding.GetEncoding("iso-8859-1"));

            this.IsDirty = false;

            menuSave.Enabled = false;

            this.Text = string.Format("Escher ̣̤̤· Editing {0}", designName);

            this.comments.Clear();
            this.sizes.Clear();
            this.sizesToSkip.Clear();
        }

        public void SetError(string error = null)
        {
            if (!string.IsNullOrEmpty(error))
            {
                designMaster.SelectionStart = validator.SelectionStart();
                designMaster.SelectionLength = validator.SelectionLength();
                designMaster.DoSelectionVisible();
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
            return designMaster.Text;
        }

        private void SetStatus(string text = "", bool success = false, bool failure = false)
        {
            status.ForeColor = (success ? Color.Green : (failure ? Color.Red : Color.Black));
            status.Text = text;
            status.Refresh();
        }

        private void Insert(string special)
        {
            designMaster.InsertText(special);
        }

        private void Replace(KeyPressEventArgs e)
        {
            if (designMaster.SelectionStart > 1)
            {
                string one = designMaster.Text[designMaster.SelectionStart - 2].ToString();
                string two = e.KeyChar.ToString();

                if (this.specials.TryGetValue(one + two, out string special))
                {
                    designMaster.SelectionStart -= 2;
                    designMaster.SelectionLength = 2;
                    designMaster.ClearSelected();
                    designMaster.InsertText(special);
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

            if (designMaster.SelectionStart <= 0)
            {
                return;
            }

            string line = designMaster.GetLineText(designMaster.PositionToPlace(designMaster.SelectionStart).iLine);

            if (!validator.Parse(line, out string error))
            {
                SetStatus(error, failure: true);
            }
            else
            {
                SetStatus();

                if (e == null)
                {
                    ImmediatePreviewDesign();
                }
            }
        }

        private void Recolor(TextChangedEventArgs e)
        {
            e.ChangedRange.ClearStyle(StyleIndex.All);

            e.ChangedRange.SetStyle(separatorStyle, @"=|\|");

            e.ChangedRange.SetStyle(htmlStyle, @"!|<br>|%|&nbsp;|&#8470;|<b>|</b>|<i>|</i>|<s>|</s>|<f>|</f>");

            //e.ChangedRange.SetStyle(CommentStyle, @"//.*$", RegexOptions.Multiline);

            e.ChangedRange.SetStyle(commentStyle, @"'.*$", RegexOptions.Multiline);

            e.ChangedRange.SetStyle(pageStyle, @"PageFeed.*$", RegexOptions.Multiline);

            e.ChangedRange.SetStyle(keywordStyle, @"\b(Alignment|Appearance|Catalogue|Color|Combine|Comment|Copyright|Description|Design|FontOfDescription|FontOfType|FrameColor|Height|Html|Horizontal|Image|Issued|LineFeed|Menu|Overprint|PageNumber|PageTitle|PageSubTitle|Pdf|Perforation|Perfs|Picture|Positions|Printed|Private|RoundedCorners|Sample|Separate|Series|Settings|Size|Skip|Stamp|Type|Unlisted|Value|Varieties|Variety|Version|Vertical|Width)\b");

            e.ChangedRange.SetStyle(enumStyle, @"\b(Black|Centered|False|HexagonVertical|Left|Rectangle|RectangleRotated|Right|Rotated|Triangle45|Triangle45Inverted|Triangle60|Triangle60Inverted|True|White)\b");

            e.ChangedRange.SetStyle(feedStyle, @"\b(Album|Country|Part|End)\b");

            e.ChangedRange.SetStyle(importantStyle, @"\b(ApplyTo|ApplyToFrameStyle|Thin|Thick)\b");
            e.ChangedRange.SetStyle(importantStyle, @"=VB|=C#");

            this.IsDirty = true;

            menuSave.Enabled = true;

            if (!this.Text.EndsWith("*"))
            {
                this.Text += "*";
            }

            ImmediatePreviewDesign();
        }

        private string FindLineBefore(string text, ref int index)
        {
            string line = null;

            index--;

            do
            {
                index = designMaster.Text.LastIndexOf(text, index, StringComparison.Ordinal);

                if (index >= 0)
                {
                    line = designMaster.GetLineText(designMaster.PositionToPlace(index).iLine);

                    if (line.Trim().StartsWith("'"))
                    {
                        line = null;

                        index--;
                    }
                }

            } while (line == null && index >= 0);

            return line;
        }

        private int FindLineAfter(string text, int startIndex)
        {
            string line = null;

            int index = startIndex;

            do
            {
                index = designMaster.Text.IndexOf(text, index, StringComparison.Ordinal);

                if (index >= 0)
                {
                    line = designMaster.GetLineText(designMaster.PositionToPlace(index).iLine);

                    if (line.Trim().StartsWith("'"))
                    {
                        line = null;

                        index++;
                    }
                }

            } while (line == null && index >= 0);

            if (index == -1)
            {
                index = designMaster.Text.Length - 1;
            }

            return index;
        }

        private void ImmediatePreviewDesign()
        {
            if (!menuRefresh.Checked)
            {
                return;
            }

            try
            {
                int lineNumber = designMaster.PositionToPlace(designMaster.SelectionStart).iLine;

                if (lineNumber == designMaster.LinesCount - 1)
                {
                    return;
                }

                string end;
                string country = null;
                string series = null;
                string section = null;

                int lineNumberPageFeedThis;
                int lineNumberPageFeedNext;

                int index;

                //StopwatchHelper.Start("Looking for End");
                index = designMaster.SelectionStart;
                if (!string.IsNullOrEmpty(end = FindLineBefore("End", ref index)))
                {
                    return;
                }
                //StopwatchHelper.Stop();

                //StopwatchHelper.Start("Looking for PageFeed");
                index = designMaster.SelectionStart;
                if (string.IsNullOrEmpty(FindLineBefore("PageFeed", ref index)))
                {
                    return;
                }
                //StopwatchHelper.Stop();

                lineNumberPageFeedThis = designMaster.PositionToPlace(index).iLine;

                //StopwatchHelper.Start("Looking for Series");
                if (string.IsNullOrEmpty(series = FindLineBefore("Series=", ref index)))
                {
                    return;
                }
                //StopwatchHelper.Stop();

                //StopwatchHelper.Start("Looking for Part");
                if (string.IsNullOrEmpty(section = FindLineBefore("Part=", ref index)))
                {
                    return;
                }
                //StopwatchHelper.Stop();

                //StopwatchHelper.Start("Looking for Country");
                if (string.IsNullOrEmpty(country = FindLineBefore("Country=", ref index)))
                {
                    return;
                }
                //StopwatchHelper.Stop();

                //StopwatchHelper.Start("Looking for End or PageFeed");
                index = Math.Min(FindLineAfter("PageFeed", designMaster.SelectionStart), FindLineAfter("End", designMaster.SelectionStart));
                //StopwatchHelper.Stop();

                lineNumberPageFeedNext = designMaster.PositionToPlace(index).iLine;

                StringBuilder pageLines = new StringBuilder();

                pageLines.Append("Album | Pdf=Pdf | Version=Version\r\n");
                pageLines.Append(country).Append("\r\n");
                pageLines.Append(section).Append("\r\n");
                pageLines.Append(series).Append("\r\n");

                //StopwatchHelper.Start("Looking for Comments and Sizes");
                for (int line = lineNumberPageFeedThis; line < lineNumberPageFeedNext; line++)
                {
                    string lineText = designMaster.GetLineText(line).Trim();

                    if (!lineText.StartsWith("'"))
                    {
                        if (lineText.Contains("Comment:"))
                        {
                            string commentValue = lineText.Split("Comment:")[1].Split('|')[0].Replace("!", "").Replace("%", "");

                            if (this.comments.ContainsKey(commentValue))
                            {
                                lineText = lineText.Replace("Comment:" + commentValue, this.comments[commentValue]);
                            }
                            else
                            {
                                index = designMaster.SelectionStart;
                                string commentOrigin = FindLineBefore("Comment=" + commentValue, ref index);

                                if (commentOrigin != null)
                                {
                                    string commentContents = commentOrigin.Split("=")[1].Split('|')[0].Trim();

                                    while (commentContents.StartsWith("!") || commentContents.StartsWith("%"))
                                    {
                                        commentContents = commentContents.Substring(1);
                                    }

                                    this.comments.Add(commentValue, commentContents);

                                    lineText = lineText.Replace("Comment:" + commentValue, commentContents);
                                }
                            }
                        }

                        if (lineText.Contains("Size="))
                        {
                            string sizeValue = lineText.Split("Size=")[1].Split('|')[0].Trim();

                            if (!sizesToSkip.Contains(sizeValue))
                            {
                                if (this.sizes.ContainsKey(sizeValue))
                                {
                                    lineText = lineText.Replace("Size=" + sizeValue, this.sizes[sizeValue]);
                                }
                                else
                                {
                                    index = designMaster.SelectionStart;
                                    string sizeOrigin = FindLineBefore("Design=" + sizeValue, ref index);

                                    if (sizeOrigin == null)
                                    {
                                        this.sizesToSkip.Add(sizeValue);
                                    }
                                    else
                                    {
                                        string sizeContents = sizeOrigin.Split("|", joinAgainExceptFirstOne: true)[1].Trim();

                                        this.sizes.Add(sizeValue, sizeContents + " | Width=+4 | Height=+4");

                                        lineText = lineText.Replace("Size=" + sizeValue, this.sizes[sizeValue]);
                                    }
                                }
                            }
                        }
                    }

                    pageLines.Append(lineText).Append("\r\n");
                }
                //StopwatchHelper.Stop();

                pageLines.Append("End\r\n");

                string pageText = pageLines.ToString();

                //StopwatchHelper.Start("Parsing design");
                Design design = (new DesignParser()).Parse(pageText, null, out string error);
                //StopwatchHelper.Stop();

                if (string.IsNullOrEmpty(error))
                {
                    preview.ShowPreview(design, pageNumber: 1, albumNumber: design.GetPagefeed(1).AlbumNumber, printMode: PrintMode.ToScreen, screenMode: ScreenMode.MatchScreenHeight);
                    preview.Show();
                    preview.Activate();

                    this.Focus();
                }
                else
                {
                    //SetError(error);
                }
            }
            catch (Exception e)
            {
                SetError(e.Message);
            }
        }

        private void PreviewDesign()
        {
            if (designMaster.SelectionStart <= 0)
            {
                return;
            }

            DesignParser designParser = new DesignParser();

            Design design = designParser.Parse(designMaster.Text, null, out string error);

            if (!string.IsNullOrEmpty(error))
            {
                SetStatus(error, failure: true);
            }    
            else
            {
                int pageNumber = 1;

                int pageFeedIndex = designMaster.Text.LastIndexOf("PageFeed", designMaster.SelectionStart);

                if (pageFeedIndex >= 0)
                {
                    string pageFeed = designMaster.GetLineText(designMaster.PositionToPlace(pageFeedIndex).iLine);

                    string[] split = pageFeed.Split("PageNumber=");

                    if (split.Length > 1)
                    {
                        string albumNumber = split[1].Split(' ')[0];

                        DesignEntry entry = design.FindAlbumNumber(albumNumber);

                        if (entry != null)
                        {
                            pageNumber = entry.PageNumber;
                        }
                    }
                }

                preview.ShowPreview(design, pageNumber: pageNumber, albumNumber: design.GetPagefeed(pageNumber).AlbumNumber, printMode: PrintMode.ToScreen, screenMode: ScreenMode.MatchScreenHeight);
                preview.Show();

                this.Focus();
            }
        }

        private void FindStampNumber()
        {
            const string cKeyword = "Variety=";

            string input = Interaction.InputBox("Stamp number:", App.GetName() + " · Find stamp number");

            if (input != "")
            {
                int index = designMaster.Text.IndexOf(cKeyword + input + " ");

                if (index < 0)
                {
                    MessageBox.Show(string.Format("Stamp number '{0}' does not exist.", input), App.GetName() + " · Find stamp number", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    designMaster.SelectionStart = index + cKeyword.Length;
                    designMaster.SelectionLength = input.Length;
                    designMaster.DoSelectionVisible();
                }
            }
        }

        private void FindAlbumNumber()
        {
            const string cKeyword = "PageNumber=";

            string input = Interaction.InputBox("Album number:", App.GetName() + " · Find album number");

            if (input != "")
            {
                int index = designMaster.Text.IndexOf(cKeyword + input);

                if (index < 0)
                {
                    MessageBox.Show(string.Format("Album number '{0}' does not exist.", input), App.GetName() + " · Find album number", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    designMaster.SelectionStart = index + cKeyword.Length;
                    designMaster.SelectionLength = input.Length;
                    designMaster.DoSelectionVisible();
                }
            }
        }

        private void BeautifyDesign()
        {
            if (!designMaster.SelectedText.Contains("\r\n"))
            {
                return;
            }

            Dictionary<int, int> maxima = new Dictionary<int, int>();

            string selectedText = designMaster.SelectedText
                .Replace(" " + Validator.cKeywordSeparator + " Separate=False", "")
                .Replace(" " + Validator.cKeywordSeparator + " Alignment=Centered", "");

            if (designMaster.SelectedText.EndsWith("\r\n"))
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
                    string[] pairs = trim.Split(Validator.cKeywordSeparator);

                    for (int pair = 0; pair < pairs.Length; pair++)
                    {
                        replacement.Append(pairs[pair].Trim());

                        if (pair < pairs.Length - 1)
                        {
                            replacement.Append(new string(' ', maxima[pair] - pairs[pair].Trim().Length)).Append(' ').Append(Validator.cKeywordSeparator).Append(' ');
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

            if (designMaster.SelectedText.EndsWith("\r\n"))
            {
                replacement.Append("\r\n");
            }

            designMaster.SelectedText = replacement.ToString();
        }

        private void SplitDesign()
        {
            splitter.Visible = !splitter.Visible;
            designSlave.Visible = !designSlave.Visible;
        }

        private void ValidateDesign()
        {
            SetStatus("Validating design...");

            Thread.Sleep(100);

            if (!validator.Parse(designMaster.Text, null, out string error))
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

            if (!validator.Parse(designMaster.Text, null, out string error))
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

                File.WriteAllText(designPath, designMaster.Text, Encoding.GetEncoding("iso-8859-1"));

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
            designMaster.Text = designMaster.Text.Replace(":=", Validator.cKeywordAssignment.ToString());
        }
    }
}
