using System;
using System.Collections.Generic;
using System.Text;

namespace Sysadmin.WMI.Models.Hardware
{
    public class LogicalDiskEntity : IHardware
    {

        [WMIAttribute("Name")]
        public string Name { get; set; }

        [WMIAttribute("Description")]
        public string Description { get; set; }

        [WMIAttribute("FileSystem")]
        public string FileSystem { get; set; }

        [WMIAttribute("Size")]
        public string Size { get; set; }

        [WMIAttribute("ProviderName")]
        public string ProviderName { get; set; }

        [WMIAttribute("SupportsFileCompression")]
        public string SupportsFileCompression { get; set; }

        [WMIAttribute("SupportsDiskQuotas")]
        public string SupportsDiskQuotas { get; set; }

        [WMIAttribute("FreeSpace")]
        public string FreeSpace { get; set; }

        [WMIAttribute("Compressed")]
        public string Compressed { get; set; }

        [WMIAttribute("VolumeSerialNumber")]
        public string VolumeSerialNumber { get; set; }

        [WMIAttribute("VolumeName")]
        public string VolumeName { get; set; }

    }
}