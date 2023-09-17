using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysAdmin.Services
{
    public interface ISettingsService
    {

        int ThemeSetting { get; set; }

        string UserDisplayNameFormat { get; set; }

        string UserLoginPattern { get; set; }

        string UserLoginFormat { get; set; }
        
        string UserDefaultPassword { get; set; }


        string ServerName { get; set; }

        string UserNameOther { get; set; }

        string UserNameCredentials { get; set; }

        int? ServerPort { get; set; }

        bool? IsSSL { get; set; }

    }
}