using System;
using System.Collections.Generic;
using System.Text;

namespace Sysadmin.WMI.Models.Hardware
{
    public class DiskDriveEntity : IHardware
    {

        [WMIAttribute("Name")]
        public string Name { get; set; }

        [WMIAttribute("Model")]
        public string Model { get; set; }

        [WMIAttribute("Manufacturer")]
        public string Manufacturer { get; set; }

        [WMIAttribute("InterfaceType")]
        public string InterfaceType { get; set; }

        [WMIAttribute("Size")]
        public string Size { get; set; }

        [WMIAttribute("MediaType")]
        public string MediaType { get; set; }

        [WMIAttribute("DiskDrive")]
        public string DiskDrive { get; set; }

        [WMIAttribute("FirmwareRevisions")]
        public string FirmwareRevisions { get; set; }

        [WMIAttribute("Partitions")]
        public string Partitions { get; set; }

        [WMIAttribute("SerialNumber")]
        public string SerialNumber { get; set; }

    }
}
