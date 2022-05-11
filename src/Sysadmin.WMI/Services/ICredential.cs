using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sysadmin.WMI.Services
{
    public interface ICredential
    {
        string UserName { get; set; }
        string Password { get; set; }

    }
}
