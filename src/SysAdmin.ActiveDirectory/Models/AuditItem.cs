using System;

namespace SysAdmin.ActiveDirectory.Models
{
    public class AuditItem
    {

        public string Name { get; set; }
        public string Action { get; set; }
        public DateTime Date { get; set; }
        public string DistinguishedName { get; set; }
        public string Type { get; set; }

    }
}
