using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace SysAdmin.Services
{
    public class SettingsService : ISettingsService
    {

        private Dictionary<string, string> settings = new Dictionary<string, string>();

        public string ThemeSetting
        {
            get
            {
                return GetStringValue("ThemeSetting", "theme_dark");
            }
            set { SetValue("ThemeSetting", value); }
        }

        public string UserDisplayNameFormat
        {
            get
            {
                return GetStringValue("UserDisplayNameFormat", "(?<FirstName>\\S+) (?<LastName>\\S+)");
            }
            set { SetValue("UserDisplayNameFormat", value); }
        }

        public string UserLoginPattern
        {
            get
            {
                return GetStringValue("UserLoginPattern", "(?<FirstName>\\S)\\S+ (?<LastName>\\S+)");
            }
            set { SetValue("UserLoginPattern", value); }
        }

        public string UserLoginFormat
        {
            get
            {
                return GetStringValue("UserLoginFormat", "${FirstName}${LastName}");
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

        public string UserName
        {
            get
            {
                return GetStringValue("UserName");
            }
            set { SetValue("UserName", value); }
        }

        public int ServerPort
        {
            get
            {
                return GetIntValue("ServerPort", 389);
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

        public int LoginSelectedIndex 
        {
            get
            {
                return GetIntValue("LoginSelectedIndex", 0);
            }
            set { SetValue("LoginSelectedIndex", value); }
        }
        public bool LoginUseCredentials 
        {
            get
            {
                return GetBooleanValue("LoginUseCredentials", false);
            }
            set { SetValue("LoginUseCredentials", value); }
        }

        public void LoadSettings()
        {
            try
            {
                string json = System.IO.File.ReadAllText("settings.json");
                settings = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            }
            catch { }
        }

        public void SaveSettings()
        {
            string json = JsonConvert.SerializeObject(settings);
            System.IO.File.WriteAllText("settings.json", json);
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
                return int.Parse(value.ToString());
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
                return Boolean.Parse(value.ToString());
        }

    }

}