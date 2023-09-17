using System;
using System.Collections.Generic;
using System.Text;

namespace Sysadmin.WMI.Models.Hardware
{
    public class ProcessorEntity : IHardware
    {

        [WMIAttribute("Name")]
        public string Name { get; set; }

        [WMIAttribute("Manufacturer")]
        public string Manufacturer { get; set; }

        [WMIAttribute("Model")]
        public string Model { get; set; }

        [WMIAttribute("Description")]
        public string Description { get; set; }

        [WMIAttribute("ThreadCount")]
        public string ThreadCount { get; set; }

        [WMIAttribute("NumberOfCores")]
        public string NumberOfCores { get; set; }

        [WMIAttribute("NumberOfLogicalProcessors")]
        public string NumberOfLogicalProcessors { get; set; }

        [WMIAttribute("ProcessorId")]
        public string ProcessorId { get; set; }

        [WMIAttribute("SocketDesignation")]
        public string SocketDesignation { get; set; }

        [WMIAttribute("MaxClockSpeed")]
        public string MaxClockSpeed { get; set; }

        [WMIAttribute("Voltage")]
        public string Voltage { get; set; }

        [WMIAttribute("AddressWidth")]
        public string AddressWidth { get; set; }

        [WMIAttribute("Device")]
        public string Device { get; set; }

        [WMIAttribute("L2CacheSize")]
        public string L2CacheSize { get; set; }

        [WMIAttribute("L3CacheSize")]
        public string L3CacheSize { get; set; }

        [WMIAttribute("NumberOfEnabledCore")]
        public string NumberOfEnabledCore { get; set; }

        [WMIAttribute("CurrentClockSpeed")]
        public string CurrentClockSpeed { get; set; }

        [WMIAttribute("SerialNumber")]
        public string SerialNumber { get; set; }

        [WMIAttribute("VirtualizationFirmwareEnabled")]
        public string VirtualizationFirmwareEnabled { get; set; }

    }
}
