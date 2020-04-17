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
    public partial class Editor : Form
    {
        public Editor()
        {
            InitializeComponent();

            this.FormClosing += new FormClosingEventHandler((sender, e) => { e.Cancel = true; this.Hide(); });
        }

        public void SetDesign(string design)
        {
            this.design.LoadFile(design, RichTextBoxStreamType.PlainText);
        }

        public string GetDesign()
        {
            return design.Text;
        }
    }
}
