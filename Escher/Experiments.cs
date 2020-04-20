using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Escher
{
    public class Experiments
    {
        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        static extern long WritePrivateProfileString(string Section, string Key, string Value, string FilePath);

        private static Font printFont = new Font("Arial", 10);

        public static void ExperimentWithSomething()
        {
            string printerName = "PDF995x";

            bool exist = PrinterSettings.InstalledPrinters.Cast<string>().Any(name => printerName.ToUpper().Trim() == name.ToUpper().Trim());

            RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\PDF995");

            string ini = string.Format("{0}PDF995\\res\\PDF995.ini", (string)registryKey.GetValue("Path", ""));

            File.Delete("C:\\PDF995\\experiment.pdf");

            WritePrivateProfileString("Parameters", "Output File", "C:\\PDF995\\experiment.pdf", ini);

            PrintDocument doc = new PrintDocument();
            PrinterSettings settings = new PrinterSettings();

            settings.PrinterName = "PDF995";
            //settings.PrintToFile = true;
            //settings.PrintFileName = "c:\\pdf995\\test.pdf";

            doc.PrintPage += new PrintPageEventHandler(PrintPage);

            doc.Print();
        }

        private static void PrintPage(object sender, PrintPageEventArgs ev)
        {
            ev.Graphics.DrawString("ExperimentWithSomething()", printFont, Brushes.Black, 100, 100, new StringFormat());

            ev.HasMorePages = false;
        }
    }
}
