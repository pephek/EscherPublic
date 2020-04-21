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

            labelProgress.Visible = false;
            labelWaiting.Visible = false;
        }

        public void SetProgress(int pageNumber)
        {
            labelProgress.ForeColor = Color.Black;
            labelProgress.Text = string.Format("1 · Printing page {0} of {1}", pageNumber, numberOfPages);

            labelWaiting.ForeColor = Color.Silver;

            labelProgress.Visible = true;
            labelWaiting.Visible = true;

            this.Refresh();

            Application.DoEvents();
        }

        public void SetWaiting()
        {
            labelProgress.ForeColor = Color.Silver;

            labelWaiting.ForeColor = Color.Black;

            this.Refresh();

            Application.DoEvents();
        }
    }
}
