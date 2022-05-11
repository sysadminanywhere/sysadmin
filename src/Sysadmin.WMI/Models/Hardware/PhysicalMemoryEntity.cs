using System;
using System.Collections.Generic;
using System.Text;

namespace Sysadmin.WMI.Models.Hardware
{
    public class PhysicalMemoryEntity : IHardware
    {

        [WMIAttribute("Attributes")]
        public int Attributes { get; set; }

        [WMIAttribute("BankLabel")]
        public string BankLabel { get; set; }

        [WMIAttribute("Capacity")]
        public long Capacity { get; set; }

        [WMIAttribute("Caption")]
        public string Caption { get; set; }

        [WMIAttribute("ConfiguredClockSpeed")]
        public int ConfiguredClockSpeed { get; set; }

        [WMIAttribute("ConfiguredVoltage")]
        public string ConfiguredVoltage { get; set; }

        [WMIAttribute("CreationClassName")]
        public string CreationClassName { get; set; }

        [WMIAttribute("DataWidth")]
        public int DataWidth { get; set; }

        [WMIAttribute("Description")]
        public string Description { get; set; }

        [WMIAttribute("DeviceLocator")]
        public string DeviceLocator { get; set; }

        [WMIAttribute("FormFactor")]
        public int FormFactor { get; set; }

        [WMIAttribute("HotSwappable")]
        public string HotSwappable { get; set; }

        [WMIAttribute("InstallDate")]
        public string InstallDate { get; set; }

        [WMIAttribute("InterleaveDataDepth")]
        public int InterleaveDataDepth { get; set; }

        [WMIAttribute("InterleavePosition")]
        public int InterleavePosition { get; set; }

        [WMIAttribute("Manufacturer")]
        public string Manufacturer { get; set; }

        [WMIAttribute("MaxVoltage")]
        public string MaxVoltage { get; set; }

        [WMIAttribute("MemoryType")]
        public int MemoryType { get; set; }

        [WMIAttribute("MinVoltage")]
        public string MinVoltage { get; set; }

        [WMIAttribute("Model")]
        public string Model { get; set; }

        [WMIAttribute("Name")]
        public string Name { get; set; }

        [WMIAttribute("OtherIdentifyingInfo")]
        public string OtherIdentifyingInfo { get; set; }

        [WMIAttribute("PartNumber")]
        public string PartNumber { get; set; }

        [WMIAttribute("PositionInRow")]
        public string PositionInRow { get; set; }

        [WMIAttribute("PoweredOn")]
        public string PoweredOn { get; set; }

        [WMIAttribute("Removable")]
        public string Removable { get; set; }

        [WMIAttribute("Replaceable")]
        public string Replaceable { get; set; }

        [WMIAttribute("SerialNumber")]
        public string SerialNumber { get; set; }

        [WMIAttribute("SKU")]
        public string SKU { get; set; }

        [WMIAttribute("SMBIOSMemoryType")]
        public int SMBIOSMemoryType { get; set; }

        [WMIAttribute("Speed")]
        public int Speed { get; set; }

        [WMIAttribute("Status")]
        public string Status { get; set; }

        [WMIAttribute("Tag")]
        public string Tag { get; set; }

        [WMIAttribute("TotalWidth")]
        public int TotalWidth { get; set; }

        [WMIAttribute("TypeDetail")]
        public int TypeDetail { get; set; }

        [WMIAttribute("Version")]
        public string Version { get; set; }

    }
}