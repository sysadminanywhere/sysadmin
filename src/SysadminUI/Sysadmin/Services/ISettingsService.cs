using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysAdmin.Services
{
    public interface ISettingsService
    {

        void LoadSettings();

        void SaveSettings();

        string ThemeSetting { get; set; }

        string UserDisplayNameFormat { get; set; }

        string UserLoginPattern { get; set; }

        string UserLoginFormat { get; set; }
        
        string UserDefaultPassword { get; set; }


        string ServerName { get; set; }

        string UserName { get; set; }

        int ServerPort { get; set; }

        bool IsSSL { get; set; }

        int LoginSelectedIndex { get; set; }

        bool LoginUseCredentials { get; set; }

    }
}