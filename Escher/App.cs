using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Escher
{
    public static class StringExtensions
    {
        public static string[] Split(this string s, char separator, bool joinAgainExceptFirstOne = false)
        {
            return s.Split(separator.ToString(), joinAgainExceptFirstOne);
        }

        public static string[] Split(this string s, string separator, bool joinAgainExceptFirstOne = false)
        {
            string[] split = s.Split(new[] { separator }, StringSplitOptions.None);

            if (joinAgainExceptFirstOne)
            {
                return new string[] { split[0], string.Join(separator, split.Skip(1)) };
            }
            else
            {
                return split;
            }
        }

        public static string CapitalizeFirstLetters(this string s)
        {
            s = s.Replace("  ", " ").ToLower();

            string[] split = s.Split(' ');

            for (int i = 0; i < split.Length; i++)
            {
                if (split[i].Length > 1)
                {
                    split[i] = Char.ToUpper(split[i][0]) + split[i].Substring(1);
                }
            }

            s = string.Join(" ", split);

            return s;
        }
    }

    public class App
    {
        public static string GetName()
        {
            return System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
        }

        public static string GetSetting(string setting)
        {
            return ConfigurationManager.AppSettings[setting];
        }

        public static void SetException(string exception)
        {
            MessageBox.Show(exception, App.GetName() + " · Exception", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            System.Environment.Exit(1);
        }

        public static void TryRun(Action code)
        {
            TryRun(code: code, codeWithArgument: null, argument: null, editor: null);
        }

        public static void TryRun(Action code, Editor editor)
        {
            TryRun(code: code, codeWithArgument: null, argument: null, editor: editor);
        }

        public static void TryRun(Action<string> code, string argument)
        {
            TryRun(code: null, codeWithArgument: code, argument: argument, editor: null);
        }

        public static void TryRun(Action<string> code, string argument, Editor editor)
        {
            TryRun(code: null, codeWithArgument: code, argument: argument, editor: editor);
        }

        private static void TryRun(Action code, Action<string> codeWithArgument, string argument, Editor editor)
        {
            if (editor != null && editor.IsDirty)
            {
                MessageBox.Show("The editor has unsaved changes; cannot continue with this operation!", App.GetName() + " · Unsaved Changes", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                bool retry = true;
                bool retried = false;

                while (retry)
                {
                    try
                    {
                        if (argument == null)
                        {
                            code();
                        }
                        else
                        {
                            codeWithArgument(argument);
                        }

                        retry = false;
                    }
                    catch (Exception e)
                    {
                        string message = string.Format("The following exception occurred:\n\n{0}", e.Message);

                        if (retried)
                        {
                            message += string.Format("\n\n{0}", e.StackTrace.ToString());
                        }

                        if (MessageBox.Show(message, App.GetName() + " · Exception", MessageBoxButtons.RetryCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1) == DialogResult.Cancel)
                        {
                            retry = false;
                        }
                        else
                        {
                            retried = true;
                        }
                    }
                }
            }
        }
    }
}
