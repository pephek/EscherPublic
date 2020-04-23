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

        public static void SetException(string exception)
        {
            MessageBox.Show(exception, App.GetName() + " · Exception", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            System.Environment.Exit(1);
        }

        public static string GetSetting(string setting)
        {
            return ConfigurationManager.AppSettings[setting];
        }
    }
}
