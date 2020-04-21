using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Escher
{
    public class PDF995Helper
    {
        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        static extern long WritePrivateProfileString(string Section, string Key, string Value, string FilePath);

        private const string cWaitForCompletionFlag = "wait.for.completion.flag";
        private const string cDeleteWaitForCompletionFlag = "delete.wait.for.completion.flag.bat";

        public PDF995Helper(string pdfName)
        {
            string pdfPath = string.Format("{0}\\{1}.pdf", App.GetSetting("DocumentsFolder"), pdfName.ToLower());

            string flagPath = string.Format("{0}\\{1}", App.GetSetting("DocumentsFolder"), cWaitForCompletionFlag);
            string deleteFlagPath = string.Format("{0}\\{1}", App.GetSetting("DocumentsFolder"), cDeleteWaitForCompletionFlag);

            File.Delete(pdfPath);

            RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\PDF995");

            string ini = string.Format("{0}PDF995\\res\\PDF995.ini", (string)registryKey.GetValue("Path", ""));

            WritePrivateProfileString("Parameters", "Output File", pdfPath, ini);
            WritePrivateProfileString("Parameters", "ProcessPDF", deleteFlagPath, ini);

            File.WriteAllText(flagPath, flagPath);
            File.WriteAllText(deleteFlagPath, string.Format("del \"{0}\"\n", flagPath));
        }

        public void WaitForCompletion()
        {
            string flagPath = string.Format("{0}\\{1}.pdf", App.GetSetting("DocumentsFolder"), cWaitForCompletionFlag);

            while (File.Exists(flagPath))
            {
                Thread.Sleep(100);
            }
        }
    }
}
