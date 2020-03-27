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
    public partial class Preview : Form
    {
        private Page page;

        public Preview()
        {
            InitializeComponent();
        }

        private void Preview_Load(object sender, EventArgs e)
        {
            ShowPage();
        }

        private void Preview_KeyPress(object sender, KeyPressEventArgs e)
        {
            switch (e.KeyChar)
            {
                case (char)Keys.Escape:
                    this.Close();
                    break;
                default:
                    switch (e.KeyChar.ToString().ToLower())
                    {
                        case "p":
                            ShowPageSetup();
                            break;
                    }
                    break;
            }
        }

        public void SetPage(Page page)
        {
            this.page = page;
        }

        private void ShowPageSetup()
        {
            Print print = new Print();
            print.printMode = PrintMode.ToScreen;
            DialogResult result = print.ShowDialog();

            if (result == DialogResult.OK)
            {
                PageSetup.Load();

                ShowPage();
            }
        }

        private void ShowPage()
        {
        }
    }
}
