using System;
using System.Collections.Generic;
using System.Text;

namespace Sysadmin.WMI.Models.Hardware
{
    public class BIOSEntity
    {

        [WMIAttribute("Manufacturer")]
        public string Manufacturer { get; set; }

        [WMIAttribute("Version")]
        public string Version { get; set; }

        [WMIAttribute("SystemBIOSMajorVersion")]
        public string SystemBIOSMajorVersion { get; set; }

        [WMIAttribute("SystemBIOSMinorVersion")]
        public string SystemBIOSMinorVersion { get; set; }

        [WMIAttribute("SMBIOSBIOSVersion")]
        public string SMBIOSBIOSVersion { get; set; }

        [WMIAttribute("SMBIOSMajorVersion")]
        public string SMBIOSMajorVersion { get; set; }

        [WMIAttribute("SMBIOSMinorVersion")]
        public string SMBIOSMinorVersion { get; set; }

        [WMIAttribute("ReleaseDate")]
        public DateTime? ReleaseDate { get; set; }

        [WMIAttribute("SerialNumber")]
        public string SerialNumber { get; set; }

    }
}
