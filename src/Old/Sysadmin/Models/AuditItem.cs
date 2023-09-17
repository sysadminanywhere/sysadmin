using System;

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
