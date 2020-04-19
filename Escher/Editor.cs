using FastColoredTextBoxNS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
        private readonly TextStyle separatorStyle = new TextStyle(Brushes.Gray, null, FontStyle.Regular);
        private readonly TextStyle commentStyle = new TextStyle(Brushes.Green, null, FontStyle.Italic);
        private readonly TextStyle feedStyle = new TextStyle(Brushes.Maroon, null, FontStyle.Bold);
        private readonly TextStyle veryImportantStyle = new TextStyle(Brushes.Red, null, FontStyle.Bold);
        private readonly TextStyle enumStyle = new TextStyle(Brushes.SteelBlue, null, FontStyle.Regular);

        private Validator validator;
        private Preview preview;

        private Action<string> reopen;

        private string designName;

        private bool isDirty;

        public Editor(Validator validator, Preview preview)
        {
            InitializeComponent();

            this.Load += new EventHandler((sender, e) => Initialize());

            this.menuPreview.Click += new EventHandler((sender, e) => PreviewDesign());
            this.menuValidate.Click += new EventHandler((sender, e) => ValidateDesign());
            this.menuSave.Click += new EventHandler((sender, e) => SaveDesign());
            this.menuExit.Click += new EventHandler((sender, e) => ExitDesign());

            this.validator = validator;
            this.preview = preview;

            design.TextChanged += new EventHandler<TextChangedEventArgs>((sender, e) => Recolor(e));
            design.KeyUp += new KeyEventHandler((sender, e) => Parse());
            design.MouseUp += new MouseEventHandler((sender, e) => Parse());
        }

        public void SetDesign(string designName, Action<string> reopen)
        {
            this.designName = designName;

            this.reopen = reopen;

            string designPath = string.Format("{0}\\{1}.cdb", App.GetSetting("DesignsFolder"), this.designName);

            design.Text = File.ReadAllText(designPath, Encoding.GetEncoding("iso-8859-1"));

            this.isDirty = false;

            menuSave.Enabled = false;

            this.Text = string.Format("Editing {0}", designName);
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

        public string GetDesign()
        {
            return design.Text;
        }

        private void SetStatus(string text = "", bool success = false, bool failure = false)
        {
            status.ForeColor = (success ? Color.Green : (failure ? Color.Red : Color.Black));
            status.Text = text;
            status.Refresh();
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
        }

        private void Parse()
        {
            if (design.SelectionStart != 0)
            {
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
        }

        private void Recolor(TextChangedEventArgs e)
        {
            e.ChangedRange.ClearStyle(keywordStyle, separatorStyle, commentStyle, veryImportantStyle, enumStyle);

            e.ChangedRange.SetStyle(separatorStyle, @":=|\|");

            //e.ChangedRange.SetStyle(CommentStyle, @"//.*$", RegexOptions.Multiline);
            e.ChangedRange.SetStyle(commentStyle, @"'.*$", RegexOptions.Multiline);

            e.ChangedRange.SetStyle(keywordStyle, @"\b(Alignment|Appearance|Catalogue|Color|Combine|Comment|Copyright|Description|Design|FontOfDescription|FontOfType|FrameColor|Height|Html|Horizontal|Issued|LineFeed|Menu|Overprint|PageNumber|Pdf|Perforation|Perfs|Picture|Positions|Printed|Private|Sample|Separate|Series|Settings|Size|Skip|Stamp|Type|Unlisted|Value|Varieties|Variety|Version|Vertical|Width)\b");

            e.ChangedRange.SetStyle(enumStyle, @"\b(Black|Centered|False|HexagonVertical|Left|Rectangle|RectangleRotated|Right|Rotated|Triangle45|Triangle45Inverted|Triangle60|Triangle60Inverted|True|White)\b");

            e.ChangedRange.SetStyle(feedStyle, @"\b(Country|Part|PageFeed|End)\b");

            e.ChangedRange.SetStyle(veryImportantStyle, @"\b(ApplyTo|ApplyToFrameStyle|Thin|Thick)\b");
            e.ChangedRange.SetStyle(veryImportantStyle, @":=VB|:=C#");

            this.isDirty = true;

            menuSave.Enabled = true;

            if (!this.Text.EndsWith("*"))
            {
                this.Text += "*";
            }
        }

        private void PreviewDesign()
        {

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

                string designPath = string.Format("{0}\\{1}.cdb", App.GetSetting("DesignsFolder"), this.designName);
                string desigRollbackPath = string.Format("{0}\\{1}.cdb", App.GetSetting("DesignsRollbackFolder"), this.designName);

                File.Copy(designPath, desigRollbackPath, overwrite: true);

                File.WriteAllText(designPath, design.Text, Encoding.GetEncoding("iso-8859-1"));

                this.isDirty = false;

                menuSave.Enabled = false;

                this.Text = string.Format("Editing {0}", designName);

                this.reopen(design.Text);
            }
        }

        private void ExitDesign()
        {
            if (this.isDirty)
            {
                if (MessageBox.Show("The design has unsaved changed, do you want to discard the changes?", App.GetName() + " · Discard changes", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
                {
                    return;
                }
            }

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
    }
}
