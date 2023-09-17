using System;
using System.Collections.Generic;
using System.Text;

namespace Sysadmin.WMI.Models.Hardware
{
    public class BaseboardEntity
    {

        [WMIAttribute("Product")]
        public string Product { get; set; }

        [WMIAttribute("Manufacturer")]
        public string Manufacturer { get; set; }

        [WMIAttribute("HotSwappable")]
        public string HotSwappable { get; set; }

        [WMIAttribute("HostingBoard")]
        public string HostingBoard { get; set; }

        [WMIAttribute("Removable")]
        public string Removable { get; set; }

        [WMIAttribute("Replaceable")]
        public string Replaceable { get; set; }

        [WMIAttribute("RequiresDaughterBoard")]
        public string RequiresDaughterBoard { get; set; }

        [WMIAttribute("Version")]
        public string Version { get; set; }

        [WMIAttribute("SerialNumber")]
        public string SerialNumber { get; set; }

        [WMIAttribute("TotalCpuSockets")]
        public string TotalCpuSockets { get; set; }

    }
}
