using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Escher
{
    public partial class About : Form
    {
        public About()
        {
            InitializeComponent();

            FileVersionInfo info = FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location);

            labelVersion.Text = string.Format("{0} V{1} · {2}", info.ProductName, info.ProductVersion, info.LegalCopyright);
            //·
        }

        private void buttonClose_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
