using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysAdmin.Services
{
    public interface IBusyService
    {

        void Busy();
        void Idle();

    }
}
