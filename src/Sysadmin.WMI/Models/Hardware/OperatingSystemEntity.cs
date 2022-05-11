using System;
using System.Collections.Generic;
using System.Text;

namespace Sysadmin.WMI.Models.Hardware
{
    public class OperatingSystemEntity
    {

        [WMIAttribute("OSType")]
        public string OSType { get; set; }

        [WMIAttribute("Caption")]
        public string Caption { get; set; }

        [WMIAttribute("Manufacturer")]
        public string Manufacturer { get; set; }

        [WMIAttribute("Version")]
        public string Version { get; set; }

        [WMIAttribute("CSDVersion")]
        public string CSDVersion { get; set; }

        [WMIAttribute("SerialNumber")]
        public string SerialNumber { get; set; }

        [WMIAttribute("OSArchitecture")]
        public string OSArchitecture { get; set; }

        [WMIAttribute("OperatingSystemSKU")]
        public string OperatingSystemSKU { get; set; }

        [WMIAttribute("Locale")]
        public string Locale { get; set; }

        [WMIAttribute("CountryCode")]
        public string CountryCode { get; set; }

        [WMIAttribute("OSLanguage")]
        public string OSLanguage { get; set; }

        [WMIAttribute("Organization")]
        public string Organization { get; set; }

        [WMIAttribute("SystemDirectory")]
        public string SystemDirectory { get; set; }

    }
}
