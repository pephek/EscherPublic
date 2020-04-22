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
    public partial class Progress : Form
    {
        private readonly int numberOfPages;

        public Progress(int numberOfPages)
        {
            InitializeComponent();

            this.numberOfPages = numberOfPages;

            labelPrinting.Text = string.Format("1 · Printing page {0} of {1}", 0, numberOfPages);
            labelCreating.Text = string.Format("2 · Creating image {0} of {1}", 0, numberOfPages);

            labelPrinting.ForeColor = Color.Silver;
            labelCreating.ForeColor = Color.Silver;
            labelWaiting.ForeColor = Color.Silver;
        }

        public void SetPrintingProgress(int pageNumber)
        {
            labelPrinting.Text = string.Format("1 · Printing page {0} of {1}", pageNumber, numberOfPages);

            labelPrinting.ForeColor = Color.Black;
            labelCreating.ForeColor = Color.Silver;
            labelWaiting.ForeColor = Color.Silver;

            Refresh();

            Application.DoEvents();
        }

        public void SetCreatingProgress(int pageNumber)
        {
            labelCreating.Text = string.Format("2 · Creating image {0} of {1}", pageNumber, numberOfPages);

            labelPrinting.ForeColor = Color.Silver;
            labelCreating.ForeColor = Color.Black;
            labelWaiting.ForeColor = Color.Silver;

            Refresh();

            Application.DoEvents();
        }

        public void SetWaiting()
        {
            labelPrinting.ForeColor = Color.Silver;
            labelCreating.ForeColor = Color.Silver;
            labelWaiting.ForeColor = Color.Black;

            Refresh();

            Application.DoEvents();
        }
    }
}
