using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysAdmin.Services
{
    public class SettingsService : ISettingsService
    {

        private readonly Dictionary<string, string> settings = new Dictionary<string, string>();

        public int ThemeSetting
        {
            get
            {
                return GetIntValue("ThemeSetting", 0);
            }
            set { SetValue("ThemeSetting", value); }
        }

        public string UserDisplayNameFormat
        {
            get
            {
                return GetStringValue("UserDisplayNameFormat", @"(?<FirstName>\S+) (?<LastName>\S+)");
            }
            set { SetValue("UserDisplayNameFormat", value); }
        }

        public string UserLoginPattern
        {
            get
            {
                return GetStringValue("UserLoginPattern", @"(?<FirstName>\S+) (?<LastName>\S+)");
            }
            set { SetValue("UserLoginPattern", value); }
        }

        public string UserLoginFormat
        {
            get
            {
                return GetStringValue("UserLoginFormat", @"${FirstName}.${LastName}");
            }
            set { SetValue("UserLoginFormat", value); }
        }

        public string UserDefaultPassword
        {
            get
            {
                return GetStringValue("UserDefaultPassword");
            }
            set { SetValue("UserDefaultPassword", value); }
        }


        public string ServerName
        {
            get
            {
                return GetStringValue("ServerName");
            }
            set { SetValue("ServerName", value); }
        }

        public string UserNameOther
        {
            get
            {
                return GetStringValue("UserNameOther");
            }
            set { SetValue("UserNameOther", value); }
        }

        public string UserNameCredentials
        {
            get
            {
                return GetStringValue("UserNameCredentials");
            }
            set { SetValue("UserNameCredentials", value); }
        }

        public int ServerPort
        {
            get
            {
                return GetIntValue("ServerPort");
            }
            set { SetValue("ServerPort", value); }
        }

        public bool IsSSL
        {
            get
            {
                return GetBooleanValue("IsSSL");
            }
            set { SetValue("IsSSL", value); }
        }


        public void LoadSettings()
        {
        }

        public void SaveSettings()
        {
        }

        private void SetValue(string key, object value)
        {
            string str = value == null ? string.Empty : value.ToString();

            if (!settings.ContainsKey(key))
                settings.Add(key, str);
            else
                settings[key] = str;

        }

        private object? GetValue(string key)
        {
            if (!settings.ContainsKey(key))
                return null;
            else
                return settings[key];
        }


        private int GetIntValue(string key, int defaultvalue = 0)
        {
            object value = GetValue(key);

            if (value == null)
                return defaultvalue;
            else
                return (int)value;
        }

        private string GetStringValue(string key, string defaultvalue = "")
        {
            object value = GetValue(key);

            if (value == null)
                return defaultvalue;
            else
                return value.ToString();
        }

        private bool GetBooleanValue(string key, bool defaultvalue = false)
        {
            object value = GetValue(key);

            if (value == null)
                return defaultvalue;
            else
                return (bool)value;
        }

    }

}