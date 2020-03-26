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
        }

        public void SetDesign(string design)
        {
            rtfEditor.LoadFile(design, RichTextBoxStreamType.PlainText);
        }

        public string GetDesign()
        {
            return rtfEditor.Text;
        }
    }
}
