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
        public static string[] Split(this string s, string separator, bool joinAgainExceptFirstOne = false)
        {
            string[] split = s.Split(new[] { separator }, StringSplitOptions.None);

            if (joinAgainExceptFirstOne)
            {
                return new string[] { split[0], String.Join(separator, split.Skip(1)) };
            }
            else
            {
                return split;
            }
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
            MessageBox.Show(exception, App.GetName(), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            System.Environment.Exit(1);
        }

        public static string GetSetting(string setting)
        {
            return ConfigurationManager.AppSettings[setting];
        }

        public static string GetSetting(string setting, string defaultSetting)
        {
            string appSetting = ConfigurationManager.AppSettings[setting];

            if (string.IsNullOrEmpty(appSetting))
            {
                appSetting = defaultSetting;
            }

            return appSetting;
        }

        public static bool GetSetting(string setting, bool defaultSetting)
        {
            bool boolSetting = defaultSetting;

            string appSetting = ConfigurationManager.AppSettings[setting];

            if (!string.IsNullOrEmpty(appSetting))
            {
                if (!Boolean.TryParse(appSetting, out boolSetting))
                {
                    MessageBox.Show(string.Format("Invalid value '{0}' for the '{1}' setting", appSetting, setting), App.GetName() + " · Settings", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }

            return boolSetting;
        }

        public static FrameStyle GetSetting(string setting, FrameStyle defaultSetting)
        {
            FrameStyle frameStyle = defaultSetting;

            string appSetting = ConfigurationManager.AppSettings[setting];

            if (!string.IsNullOrEmpty(appSetting))
            {
                if (!Enum.TryParse(appSetting, out frameStyle))
                {
                    MessageBox.Show(string.Format("Invalid value '{0}' for the '{1}' setting", appSetting, setting), App.GetName() + " · Settings", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }

            return frameStyle;
        }

        public static ColorStyle GetSetting(string setting, ColorStyle defaultSetting)
        {
            ColorStyle colorStyle = defaultSetting;

            string appSetting = ConfigurationManager.AppSettings[setting];

            if (!string.IsNullOrEmpty(appSetting))
            {
                if (!Enum.TryParse(appSetting, out colorStyle))
                {
                    MessageBox.Show(string.Format("Invalid value '{0}' for the '{1}' setting", appSetting, setting), App.GetName() + " · Settings", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }

            return colorStyle;
        }

        public static FontSize GetSetting(string setting, FontSize defaultSetting)
        {
            FontSize fontSize = defaultSetting;

            string appSetting = ConfigurationManager.AppSettings[setting];

            if (!string.IsNullOrEmpty(appSetting))
            {
                if (!Enum.TryParse(appSetting, out fontSize))
                {
                    MessageBox.Show(string.Format("Invalid value '{0}' for the '{1}' setting", appSetting, setting), App.GetName() + " · Settings", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }

            return fontSize;
        }

        public static void SetSetting(string setting, string value)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath);

            config.AppSettings.Settings.Remove(setting);
            config.AppSettings.Settings.Add(setting, value);

            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }

        public static void SetSetting(string setting, bool value)
        {
            SetSetting(setting, value.ToString());
        }

        public static string GetImage(string path, string number, float width, float height, ColorStyle colorStyle, bool thumbnail)
        {
            string image = null;

            if (number.Contains('('))
            {
                number = number.Substring(0, number.IndexOf('(') - 1).Trim();
            }

            // Remove any spaces in the number
            number = number.Replace(" ", "");

            // If we don't want the thumbnail image then we want the album page image in either grey scale or full color
            if (!thumbnail)
            {
                path = string.Format("{0}\\{1}", path, colorStyle == ColorStyle.Color ? "xlcolor" : "xlprint");
            }

            // Finalize the path with the number
            path = string.Format("{0}\\{1}.jpg", path, number);

            // If we want the thumbnail but don't have one then we are going to create one
            if (thumbnail)
            {
                throw new Exception("toto");
            }

            if (File.Exists(path))
            {
                image = path;
            }

            return image;
        }
    }
}
