using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysAdmin.ActiveDirectory.Models
{
    public interface IADEntry
    {
        string CN { get; set; }

        List<string> ObjectClass { get; set; }

    }
}
