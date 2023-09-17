using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sysadmin.Services
{
    public interface IStateService
    {

        bool IsLoggedIn { get; set; }

    }
}
