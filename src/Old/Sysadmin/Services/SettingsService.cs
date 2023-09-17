using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace SysAdmin.Services
{
    public class SettingsService : ISettingsService
    {

        public int ThemeSetting
        {
            get
            {
                if (ApplicationData.Current.LocalSettings.Values["ThemeSetting"] != null)
                    return (int)ApplicationData.Current.LocalSettings.Values["ThemeSetting"];
                else
                    return 0;
            }
            set { ApplicationData.Current.LocalSettings.Values["ThemeSetting"] = value; }
        }

        public string UserDisplayNameFormat
        {
            get
            {
                if (ApplicationData.Current.LocalSettings.Values["UserDisplayNameFormat"] != null)
                    return ApplicationData.Current.LocalSettings.Values["UserDisplayNameFormat"].ToString();
                else
                    return null;
            }
            set { ApplicationData.Current.LocalSettings.Values["UserDisplayNameFormat"] = value; }
        }

        public string UserLoginPattern
        {
            get
            {
                if (ApplicationData.Current.LocalSettings.Values["UserLoginPattern"] != null)
                    return ApplicationData.Current.LocalSettings.Values["UserLoginPattern"].ToString();
                else
                    return null;
            }
            set { ApplicationData.Current.LocalSettings.Values["UserLoginPattern"] = value; }
        }

        public string UserLoginFormat
        {
            get
            {
                if (ApplicationData.Current.LocalSettings.Values["UserLoginFormat"] != null)
                    return ApplicationData.Current.LocalSettings.Values["UserLoginFormat"].ToString();
                else
                    return null;
            }
            set { ApplicationData.Current.LocalSettings.Values["UserLoginFormat"] = value; }
        }

        public string UserDefaultPassword
        {
            get
            {
                if (ApplicationData.Current.LocalSettings.Values["UserDefaultPassword"] != null)
                    return ApplicationData.Current.LocalSettings.Values["UserDefaultPassword"].ToString();
                else
                    return string.Empty;
            }
            set { ApplicationData.Current.LocalSettings.Values["UserDefaultPassword"] = value; }
        }


        public string ServerName
        {
            get
            {
                if (ApplicationData.Current.LocalSettings.Values["ServerName"] != null)
                    return ApplicationData.Current.LocalSettings.Values["ServerName"].ToString();
                else
                    return null;
            }
            set { ApplicationData.Current.LocalSettings.Values["ServerName"] = value; }
        }

        public string UserNameOther
        {
            get
            {
                if (ApplicationData.Current.LocalSettings.Values["UserNameOther"] != null)
                    return ApplicationData.Current.LocalSettings.Values["UserNameOther"].ToString();
                else
                    return null;
            }
            set { ApplicationData.Current.LocalSettings.Values["UserNameOther"] = value; }
        }

        public string UserNameCredentials
        {
            get
            {
                if (ApplicationData.Current.LocalSettings.Values["UserNameCredentials"] != null)
                    return ApplicationData.Current.LocalSettings.Values["UserNameCredentials"].ToString();
                else
                    return null;
            }
            set { ApplicationData.Current.LocalSettings.Values["UserNameCredentials"] = value; }
        }

        public int? ServerPort
        {
            get
            {
                if (ApplicationData.Current.LocalSettings.Values["ServerPort"] != null)
                    return int.Parse(ApplicationData.Current.LocalSettings.Values["ServerPort"].ToString());
                else
                    return null;
            }
            set { ApplicationData.Current.LocalSettings.Values["ServerPort"] = value; }
        }

        public bool? IsSSL
        {
            get
            {
                if (ApplicationData.Current.LocalSettings.Values["IsSSL"] != null)
                    return (bool)ApplicationData.Current.LocalSettings.Values["IsSSL"];
                else
                    return null;
            }
            set { ApplicationData.Current.LocalSettings.Values["IsSSL"] = value; }
        }

    }
}