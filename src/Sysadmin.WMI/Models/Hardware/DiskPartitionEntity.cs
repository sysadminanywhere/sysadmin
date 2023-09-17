using System;
using System.Collections.Generic;
using System.Text;

namespace Sysadmin.WMI.Models.Hardware
{
    public class DiskPartitionEntity : IHardware
    {

        [WMIAttribute("Name")]
        public string Name { get; set; }

        [WMIAttribute("Description")]
        public string Description { get; set; }

        [WMIAttribute("DiskIndex")]
        public string DiskIndex { get; set; }

        [WMIAttribute("Bootable")]
        public string Bootable { get; set; }

        [WMIAttribute("BootPartition")]
        public string BootPartition { get; set; }

        [WMIAttribute("Size")]
        public string Size { get; set; }

        [WMIAttribute("StartingOffset")]
        public string StartingOffset { get; set; }

    }
}
