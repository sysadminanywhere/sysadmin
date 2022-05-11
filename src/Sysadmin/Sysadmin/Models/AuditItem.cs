using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysAdmin.Models
{
    public class AuditItem
    {

        public string CN { get; set; }
        public string Action { get; set; }
        public DateTime Date { get; set; }
        public string DistinguishedName { get; set; }

    }
}
