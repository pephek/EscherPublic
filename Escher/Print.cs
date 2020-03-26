using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Escher
{
    public partial class Print : Form
    {
        public PrintMode PrintMode;

        public Print()
        {
            InitializeComponent();
        }

        private void Print_Load(object sender, EventArgs e)
        {
            Print_Load();
        }

        private void Print_Shown(object sender, EventArgs e)
        {
            buttonOk.Focus();
        }

        private void Print_Load()
        {
            this.Text = App.GetName() + " · Print " + (PrintMode == PrintMode.ToScreen ? "Preview" : "Document");

            buttonOk.Text = (PrintMode == PrintMode.ToScreen ? "Preview" : "Print");

            groupBoxAlbumOptions.Enabled = (PrintMode == PrintMode.ToDocument);

            // Format

            comboBoxFormat.Items.AddRange(PageFormat.GetPageFormats().Select(format => format.FormatName).ToArray());
            comboBoxFormat.Text = App.GetSetting("Print.Format", "A4");
            checkBoxIncludeMarginForPunchHoles.Checked = App.GetSetting("Print.IncludeMarginForPunchHoles", false);

            // Stamps Options

            checkBoxIncludeImage.Checked = App.GetSetting("Print.IncludeImage", true);
            checkBoxIncludeNumber.Checked = App.GetSetting("Print.IncludeNumber", true);
            checkBoxIncludeValue.Checked = App.GetSetting("Print.IncludeValue", true);

            // Frame Options

            switch (App.GetSetting("Print.FrameStyle", FrameStyle.Thick))
            {
                case FrameStyle.ThinSolid:
                    radioButtonThinSolid.Checked = true;
                    break;
                case FrameStyle.Thick:
                    radioButtonThick.Checked = true;
                    break;
                case FrameStyle.ThinDotted:
                    radioButtonThinDotted.Checked = true;
                    break;
            }

            // Color Options

            switch (App.GetSetting("Print.ColorStyle", ColorStyle.Color))
            {
                case ColorStyle.Greyscale:
                    radioButtonGreyscale.Checked = true;
                    break;
                case ColorStyle.Color:
                    radioButtonColor.Checked = true;
                    break;
            }

            // Font Options

            switch (App.GetSetting("Print.FontSize", FontSize.Small))
            {
                case FontSize.Small:
                    radioButtonSmall.Checked = true;
                    break;
                case FontSize.Medium:
                    radioButtonMedium.Checked = true;
                    break;
                case FontSize.Large:
                    radioButtonLarge.Checked = true;
                    break;
            }

            // Page Options

            checkBoxIncludeBorder.Checked = App.GetSetting("Print.IncludeBorder", true);
            checkBoxIncludeTitle.Checked = App.GetSetting("Print.IncludeTitle", true);

            // Catalog Options

            comboBoxCatalog.Items.AddRange(Catalogs.Get());
            comboBoxCatalog.Text = App.GetSetting("Print.Catalog", Catalogs.Get()[0]);
            checkBoxAppendCatalog.Checked = App.GetSetting("Print.AppendCatalog", false);

            // Album Options

            checkBoxIncludeSamplePagesOnly.Checked = App.GetSetting("Print.IncludeSamplePagesOnly", false);
            checkBoxIncludeHtmlScans.Checked = App.GetSetting("Print.IncludeHtmlScans", false);
            checkBoxIncludePdfBookmarks.Checked = App.GetSetting("Print.IncludePdfBookmarks", true);
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            // Format

            App.SetSetting("Print.Format", comboBoxFormat.Text);
            App.SetSetting("Print.IncludeMarginForPunchHoles", checkBoxIncludeMarginForPunchHoles.Checked);

            // Stamps Options

            App.SetSetting("Print.IncludeImage", checkBoxIncludeImage.Checked);
            App.SetSetting("Print.IncludeNumber", checkBoxIncludeNumber.Checked);
            App.SetSetting("Print.IncludeValue", checkBoxIncludeValue.Checked);

            // Frame Options

            App.SetSetting("Print.FrameStyle", (radioButtonThinSolid.Checked ? FrameStyle.ThinSolid : (radioButtonThick.Checked ? FrameStyle.Thick : FrameStyle.ThinDotted)).ToString());

            // Color Options

            App.SetSetting("Print.ColorStyle", (radioButtonGreyscale.Checked ? ColorStyle.Greyscale  : ColorStyle.Color).ToString());

            // Font Options

            App.SetSetting("Print.FontSize", (radioButtonSmall.Checked ? FontSize.Small : (radioButtonMedium.Checked ? FontSize.Medium : FontSize.Large)).ToString());

            // Page Options

            App.SetSetting("Print.IncludeBorder", checkBoxIncludeBorder.Checked);
            App.SetSetting("Print.IncludeTitle", checkBoxIncludeTitle.Checked);

            // Catalog Options

            App.SetSetting("Print.Catalog", comboBoxCatalog.Text);
            App.SetSetting("Print.AppendCatalog", checkBoxAppendCatalog.Checked);

            // Album Options

            App.SetSetting("Print.IncludeSamplePagesOnly", checkBoxIncludeSamplePagesOnly.Checked);
            App.SetSetting("Print.IncludeHtmlScans", checkBoxIncludeHtmlScans.Checked);
            App.SetSetting("Print.IncludePdfBookmarks", checkBoxIncludePdfBookmarks.Checked);

            // Result

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
