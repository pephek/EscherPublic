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
        public PrintMode printMode;

        public Print()
        {
            InitializeComponent();

            this.Load += new EventHandler((sender, e) => PrintLoad());
            this.Shown += new EventHandler((sender, e) => buttonOk.Focus());

            buttonOk.Click += new EventHandler((sender, e) => SaveAndClose());
        }

        private void PrintLoad()
        {
            this.Text = App.GetName() + " · Print " + (printMode == PrintMode.ToScreen ? "Preview" : "Document");

            buttonOk.Text = (printMode == PrintMode.ToScreen ? "Preview" : "Print");

            groupBoxAlbumOptions.Enabled = (printMode == PrintMode.ToDocument);

            PageSetup pageSetup = PageSetup.Get();

            // Format

            comboBoxFormat.Items.AddRange(PageFormats.Get().Select(format => format.FormatName).ToArray());
            comboBoxFormat.Text = pageSetup.PageFormat.FormatName;
            checkBoxIncludeMarginForPunchHoles.Checked = pageSetup.IncludeMarginForPunchHoles;

            // Stamps Options

            checkBoxIncludeImage.Checked = pageSetup.IncludeImage;
            checkBoxIncludeNumber.Checked = pageSetup.IncludeNumber;
            checkBoxIncludeValue.Checked = pageSetup.IncludeValueAndColor;

            // Frame Options

            switch (pageSetup.FrameStyle)
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

            switch (pageSetup.ColorStyle)
            {
                case ColorStyle.Greyscale:
                    radioButtonGreyscale.Checked = true;
                    break;
                case ColorStyle.Color:
                    radioButtonColor.Checked = true;
                    break;
            }

            // Font Options

            switch (pageSetup.FontSize)
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

            checkBoxIncludeBorder.Checked = pageSetup.IncludeBorder;
            checkBoxIncludeTitle.Checked = pageSetup.IncludeTitle;

            // Catalog Options

            comboBoxCatalog.Items.AddRange(Catalogs.Get());
            comboBoxCatalog.Text = Catalogs.Convert(pageSetup.Catalog);
            checkBoxAppendCatalog.Checked = pageSetup.AppendCatalog;

            // Album Options

            checkBoxIncludeSamplePagesOnly.Checked = pageSetup.IncludeSamplePagesOnly;
            checkBoxIncludeHtmlScans.Checked = pageSetup.IncludeHtmlScans;
            checkBoxIncludePdfBookmarks.Checked = pageSetup.IncludePdfBookmarks;

            // Real life page scale

            numericRealLifePageScale.Value = pageSetup.RealLifePageScale;
        }

        private void SaveAndClose()
        {
            // Format

            Properties.Settings.Default.Paper = comboBoxFormat.Text;
            Properties.Settings.Default.IncludeMarginForPunchHoles = checkBoxIncludeMarginForPunchHoles.Checked;

            // Stamps Options

            Properties.Settings.Default.IncludeImage = checkBoxIncludeImage.Checked;
            Properties.Settings.Default.IncludeNumber = checkBoxIncludeNumber.Checked;
            Properties.Settings.Default.IncludeValueAndColor = checkBoxIncludeValue.Checked;

            // Frame Options

            Properties.Settings.Default.FrameStyle = (radioButtonThinSolid.Checked ? FrameStyle.ThinSolid : (radioButtonThick.Checked ? FrameStyle.Thick : FrameStyle.ThinDotted));

            // Color Options

            Properties.Settings.Default.ColorStyle = (radioButtonGreyscale.Checked ? ColorStyle.Greyscale : ColorStyle.Color);

            // Font Options

            Properties.Settings.Default.FontSize = (radioButtonSmall.Checked ? FontSize.Small : (radioButtonMedium.Checked ? FontSize.Medium : FontSize.Large));

            // Page Options

            Properties.Settings.Default.IncludeBorder = checkBoxIncludeBorder.Checked;
            Properties.Settings.Default.IncludeTitle = checkBoxIncludeTitle.Checked;

            // Catalog Options

            Properties.Settings.Default.Catalog = Catalogs.Convert(comboBoxCatalog.Text);
            Properties.Settings.Default.AppendCatalog = checkBoxAppendCatalog.Checked;

            // Album Options

            Properties.Settings.Default.IncludeSamplePagesOnly = checkBoxIncludeSamplePagesOnly.Checked;
            Properties.Settings.Default.IncludeHtmlScans = checkBoxIncludeHtmlScans.Checked;
            Properties.Settings.Default.IncludePdfBookmarks = checkBoxIncludePdfBookmarks.Checked;

            // Real life page scale

            Properties.Settings.Default.RealLifePageScale = numericRealLifePageScale.Value;

            // Save settings

            Properties.Settings.Default.Save();
            Properties.Settings.Default.Reload();

            // Reload

            PageSetup.Load();

            // Result

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
