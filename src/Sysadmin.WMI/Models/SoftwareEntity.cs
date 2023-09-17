using System;
using System.Collections.Generic;
using System.Text;

namespace Sysadmin.WMI.Models
{
    public class SoftwareEntity
    {

        [WMIAttribute("Name")]
        public string Name { get; set; }

        [WMIAttribute("Vendor")]
        public string Vendor { get; set; }

        [WMIAttribute("Version")]
        public string Version { get; set; }

    }
}
