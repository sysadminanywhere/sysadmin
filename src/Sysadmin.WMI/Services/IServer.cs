using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sysadmin.WMI.Services
{
    public interface IServer
    {
        string ServerName { get; set; }
        int Port { get; set; }
        bool IsSSL { get; set; }
    }
}