using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Escher
{
    public class PDF995Helper
    {
        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        static extern long WritePrivateProfileString(string Section, string Key, string Value, string FilePath);

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        static extern int GetPrivateProfileString(string Section, string Key, string Default, StringBuilder RetVal, int Size, string FilePath);

        private const string cWaitForCompletionFlag = "wait.for.completion.flag";
        private const string cDeleteWaitForCompletionFlag = "delete.wait.for.completion.flag.bat";

        private string PDF995iniFile;
        private string PDF995synciniFile;
        private string PDF995bookmarksFile;

        public PDF995Helper(string pdf, string pdfName, string bookmarksInXml, string bookmarksInHtm)
        {
            string pdfPath = string.Format("{0}\\{1}.pdf", App.GetSetting("DocumentsFolder"), pdfName.ToLower());

            string flagPath = string.Format("{0}\\{1}", App.GetSetting("DocumentsFolder"), cWaitForCompletionFlag);
            string deleteFlagPath = string.Format("{0}\\{1}", App.GetSetting("DocumentsFolder"), cDeleteWaitForCompletionFlag);

            File.Delete(pdfPath);

            RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\PDF995");

            PDF995iniFile = string.Format("{0}PDF995\\res\\PDF995.ini", (string)registryKey.GetValue("Path", ""));

            WritePrivateProfileString("Parameters", "Output File", pdfPath, PDF995iniFile);
            WritePrivateProfileString("Parameters", "ProcessPDF", deleteFlagPath, PDF995iniFile);
            WritePrivateProfileString("Parameters", "TS Enabled", "0", PDF995iniFile); // See hotmail for this fix/hint

            string pdfEdit = string.Format("{0}PDF995\\res\\utilities\\PDFEdit995.exe", (string)registryKey.GetValue("Path", ""));

            if (bookmarksInXml != null)
            {
                WritePrivateProfileString("Parameters", "ProcessPS", string.Format("{0} InsertBookMarks", pdfEdit), PDF995iniFile);

                string bookmarksFolder = "C:\\Documents and Settings\\All Users\\PDF995\\res\\utilities";

                if (!Directory.Exists(bookmarksFolder))
                {
                    Directory.CreateDirectory(bookmarksFolder);
                }

                PDF995bookmarksFile = string.Format("{0}\\bookmarks.xml", bookmarksFolder);

                File.WriteAllText(PDF995bookmarksFile, bookmarksInXml, Encoding.GetEncoding("iso-8859-1"));
            }
            else
            {
                WritePrivateProfileString("Parameters", "ProcessPS", "", PDF995iniFile);
            }

            if (bookmarksInHtm != null)
            {
                string htmPath = string.Format("{0}\\{1}.html", App.GetSetting("DocumentsFolder"), pdf.ToLower());

                File.WriteAllText(htmPath, bookmarksInHtm, Encoding.GetEncoding("iso-8859-1"));
            }

            // Paste this in the address bar of the file explorer: C:\Documents and Settings\All Users\pdf995

            PDF995synciniFile = string.Format("{0}PDF995\\res\\pdfsync.ini", (string)registryKey.GetValue("Path", ""));

            if (!File.Exists(PDF995synciniFile))
            {
                PDF995synciniFile = "C:\\Documents and Settings\\All Users\\PDF995\\pdfsync.ini";
            }

            if (!File.Exists(PDF995synciniFile))
            {
                PDF995synciniFile = null;
            }
            else
            {
                WritePrivateProfileString("Parameters", "PS Creation Complete", "0", PDF995synciniFile);
            }

            File.WriteAllText(flagPath, flagPath);
            File.WriteAllText(deleteFlagPath, string.Format("del \"{0}\"\n", flagPath));
        }

        public void WaitForCompletion()
        {
            string flagPath = string.Format("{0}\\{1}.pdf", App.GetSetting("DocumentsFolder"), cWaitForCompletionFlag);

            while (File.Exists(flagPath))
            {
                Thread.Sleep(100);

                Application.DoEvents();
            }

            if (PDF995synciniFile != null)
            {
                string psCreationComplete = "0";

                while (psCreationComplete == "0")
                {
                    Thread.Sleep(100);

                    Application.DoEvents();

                    StringBuilder setting = new StringBuilder(255);
                    GetPrivateProfileString("Parameters", "PS Creation Complete", "", setting, setting.Capacity, PDF995synciniFile);

                    psCreationComplete = setting.ToString();
                }
            }
        }
    }
}
