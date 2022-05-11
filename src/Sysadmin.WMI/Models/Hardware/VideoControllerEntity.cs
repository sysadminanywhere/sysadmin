using System;
using System.Collections.Generic;
using System.Text;

namespace Sysadmin.WMI.Models.Hardware
{
    public class VideoControllerEntity : IHardware
    {

        [WMIAttribute("AcceleratorCapabilities")]
        public string AcceleratorCapabilities { get; set; }

        [WMIAttribute("AdapterCompatibility")]
        public string AdapterCompatibility { get; set; }

        [WMIAttribute("AdapterDACType")]
        public string AdapterDACType { get; set; }

        [WMIAttribute("AdapterRAM")]
        public long AdapterRAM { get; set; }

        [WMIAttribute("Availability")]
        public int Availability { get; set; }

        [WMIAttribute("CapabilityDescriptions")]
        public string CapabilityDescriptions { get; set; }

        [WMIAttribute("Caption")]
        public string Caption { get; set; }

        [WMIAttribute("ColorTableEntries")]
        public string ColorTableEntries { get; set; }

        [WMIAttribute("ConfigManagerErrorCode")]
        public int ConfigManagerErrorCode { get; set; }

        [WMIAttribute("ConfigManagerUserConfig")]
        public bool ConfigManagerUserConfig { get; set; }

        [WMIAttribute("CreationClassName")]
        public string CreationClassName { get; set; }

        [WMIAttribute("CurrentBitsPerPixel")]
        public int CurrentBitsPerPixel { get; set; }

        [WMIAttribute("CurrentHorizontalResolution")]
        public int CurrentHorizontalResolution { get; set; }

        [WMIAttribute("CurrentNumberOfColors")]
        public long CurrentNumberOfColors { get; set; }

        [WMIAttribute("CurrentNumberOfColumns")]
        public int CurrentNumberOfColumns { get; set; }

        [WMIAttribute("CurrentNumberOfRows")]
        public int CurrentNumberOfRows { get; set; }

        [WMIAttribute("CurrentRefreshRate")]
        public int CurrentRefreshRate { get; set; }

        [WMIAttribute("CurrentScanMode")]
        public int CurrentScanMode { get; set; }

        [WMIAttribute("CurrentVerticalResolution")]
        public int CurrentVerticalResolution { get; set; }

        [WMIAttribute("Description")]
        public string Description { get; set; }

        [WMIAttribute("DeviceID")]
        public string DeviceID { get; set; }

        [WMIAttribute("DeviceSpecificPens")]
        public string DeviceSpecificPens { get; set; }

        [WMIAttribute("DitherType")]
        public int DitherType { get; set; }

        [WMIAttribute("DriverDate")]
        public DateTime DriverDate { get; set; }

        [WMIAttribute("DriverVersion")]
        public string DriverVersion { get; set; }

        [WMIAttribute("ErrorCleared")]
        public string ErrorCleared { get; set; }

        [WMIAttribute("ErrorDescription")]
        public string ErrorDescription { get; set; }

        [WMIAttribute("ICMIntent")]
        public string ICMIntent { get; set; }

        [WMIAttribute("ICMMethod")]
        public string ICMMethod { get; set; }

        [WMIAttribute("InfFilename")]
        public string InfFilename { get; set; }

        [WMIAttribute("InfSection")]
        public string InfSection { get; set; }

        [WMIAttribute("InstallDate")]
        public string InstallDate { get; set; }

        [WMIAttribute("InstalledDisplayDrivers")]
        public string InstalledDisplayDrivers { get; set; }

        [WMIAttribute("LastErrorCode")]
        public string LastErrorCode { get; set; }

        [WMIAttribute("MaxMemorySupported")]
        public string MaxMemorySupported { get; set; }

        [WMIAttribute("MaxNumberControlled")]
        public string MaxNumberControlled { get; set; }

        [WMIAttribute("MaxRefreshRate")]
        public int MaxRefreshRate { get; set; }

        [WMIAttribute("MinRefreshRate")]
        public int MinRefreshRate { get; set; }

        [WMIAttribute("Monochrome")]
        public bool Monochrome { get; set; }

        [WMIAttribute("Name")]
        public string Name { get; set; }

        [WMIAttribute("NumberOfColorPlanes")]
        public string NumberOfColorPlanes { get; set; }

        [WMIAttribute("NumberOfVideoPages")]
        public string NumberOfVideoPages { get; set; }

        [WMIAttribute("PNPDeviceID")]
        public string PNPDeviceID { get; set; }

        [WMIAttribute("PowerManagementCapabilities")]
        public string PowerManagementCapabilities { get; set; }

        [WMIAttribute("PowerManagementSupported")]
        public string PowerManagementSupported { get; set; }

        [WMIAttribute("ProtocolSupported")]
        public string ProtocolSupported { get; set; }

        [WMIAttribute("ReservedSystemPaletteEntries")]
        public string ReservedSystemPaletteEntries { get; set; }

        [WMIAttribute("SpecificationVersion")]
        public string SpecificationVersion { get; set; }

        [WMIAttribute("Status")]
        public string Status { get; set; }

        [WMIAttribute("StatusInfo")]
        public string StatusInfo { get; set; }

        [WMIAttribute("SystemCreationClassName")]
        public string SystemCreationClassName { get; set; }

        [WMIAttribute("SystemName")]
        public string SystemName { get; set; }

        [WMIAttribute("SystemPaletteEntries")]
        public string SystemPaletteEntries { get; set; }

        [WMIAttribute("TimeOfLastReset")]
        public string TimeOfLastReset { get; set; }

        [WMIAttribute("VideoArchitecture")]
        public int VideoArchitecture { get; set; }

        [WMIAttribute("VideoMemoryType")]
        public int VideoMemoryType { get; set; }

        [WMIAttribute("VideoMode")]
        public string VideoMode { get; set; }

        [WMIAttribute("VideoModeDescription")]
        public string VideoModeDescription { get; set; }

        [WMIAttribute("VideoProcessor")]
        public string VideoProcessor { get; set; }

    }
}