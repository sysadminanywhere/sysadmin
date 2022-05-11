﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Sysadmin.WMI.Models.Hardware
{
    public class ComputerSystemEntity
    {

        [WMIAttribute("AdminPasswordStatus")]
        public int AdminPasswordStatus { get; set; }

        [WMIAttribute("AutomaticManagedPagefile")]
        public bool AutomaticManagedPagefile { get; set; }

        [WMIAttribute("AutomaticResetBootOption")]
        public bool AutomaticResetBootOption { get; set; }

        [WMIAttribute("AutomaticResetCapability")]
        public bool AutomaticResetCapability { get; set; }

        [WMIAttribute("BootOptionOnLimit")]
        public string BootOptionOnLimit { get; set; }

        [WMIAttribute("BootOptionOnWatchDog")]
        public string BootOptionOnWatchDog { get; set; }

        [WMIAttribute("BootROMSupported")]
        public bool BootROMSupported { get; set; }

        [WMIAttribute("BootStatus")]
        public List<string> BootStatus { get; set; }

        [WMIAttribute("BootupState")]
        public string BootupState { get; set; }

        [WMIAttribute("Caption")]
        public string Caption { get; set; }

        [WMIAttribute("ChassisBootupState")]
        public int ChassisBootupState { get; set; }

        [WMIAttribute("ChassisSKUNumber")]
        public string ChassisSKUNumber { get; set; }

        [WMIAttribute("CreationClassName")]
        public string CreationClassName { get; set; }

        [WMIAttribute("CurrentTimeZone")]
        public int CurrentTimeZone { get; set; }

        [WMIAttribute("DaylightInEffect")]
        public string DaylightInEffect { get; set; }

        [WMIAttribute("description")]
        public string Description { get; set; }

        [WMIAttribute("DNSHostName")]
        public string DNSHostName { get; set; }

        [WMIAttribute("Domain")]
        public string Domain { get; set; }

        [WMIAttribute("DomainRole")]
        public int DomainRole { get; set; }

        [WMIAttribute("EnableDaylightSavingsTime")]
        public bool EnableDaylightSavingsTime { get; set; }

        [WMIAttribute("FrontPanelResetStatus")]
        public int FrontPanelResetStatus { get; set; }

        [WMIAttribute("HypervisorPresent")]
        public bool HypervisorPresent { get; set; }

        [WMIAttribute("InfraredSupported")]
        public bool InfraredSupported { get; set; }

        [WMIAttribute("InitialLoadInfo")]
        public string InitialLoadInfo { get; set; }

        [WMIAttribute("InstallDate")]
        public string InstallDate { get; set; }

        [WMIAttribute("KeyboardPasswordStatus")]
        public int KeyboardPasswordStatus { get; set; }

        [WMIAttribute("LastLoadInfo")]
        public int LastLoadInfo { get; set; }

        [WMIAttribute("Manufacturer")]
        public string Manufacturer { get; set; }

        [WMIAttribute("Model")]
        public string Model { get; set; }

        [WMIAttribute("Name")]
        public string Name { get; set; }

        [WMIAttribute("NameFormat")]
        public string NameFormat { get; set; }

        [WMIAttribute("NetworkServerModeEnabled")]
        public bool NetworkServerModeEnabled { get; set; }

        [WMIAttribute("NumberOfLogicalProcessors")]
        public int NumberOfLogicalProcessors { get; set; }

        [WMIAttribute("NumberOfProcessors")]
        public int NumberOfProcessors { get; set; }

        [WMIAttribute("OEMLogoBitmap")]
        public string OEMLogoBitmap { get; set; }

        [WMIAttribute("OEMStringArray")]
        public List<string> OEMStringArray { get; set; }

        [WMIAttribute("PartOfDomain")]
        public bool PartOfDomain { get; set; }

        [WMIAttribute("PauseAfterReset")]
        public int PauseAfterReset { get; set; }

        [WMIAttribute("PCSystemType")]
        public int PCSystemType { get; set; }

        [WMIAttribute("PCSystemTypeEx")]
        public int PCSystemTypeEx { get; set; }

        [WMIAttribute("PowerManagementCapabilities")]
        public string PowerManagementCapabilities { get; set; }

        [WMIAttribute("PowerManagementSupported")]
        public string PowerManagementSupported { get; set; }

        [WMIAttribute("PowerOnPasswordStatus")]
        public int PowerOnPasswordStatus { get; set; }

        [WMIAttribute("PowerState")]
        public int PowerState { get; set; }

        [WMIAttribute("PowerSupplyState")]
        public int PowerSupplyState { get; set; }

        [WMIAttribute("PrimaryOwnerContact")]
        public string PrimaryOwnerContact { get; set; }

        [WMIAttribute("PrimaryOwnerName")]
        public string PrimaryOwnerName { get; set; }

        [WMIAttribute("ResetCapability")]
        public int ResetCapability { get; set; }

        [WMIAttribute("ResetCount")]
        public int ResetCount { get; set; }

        [WMIAttribute("ResetLimit")]
        public int ResetLimit { get; set; }

        [WMIAttribute("Roles")]
        public List<string> Roles { get; set; }

        [WMIAttribute("Status")]
        public string Status { get; set; }

        [WMIAttribute("SupportContactDescription")]
        public string SupportContactDescription { get; set; }

        [WMIAttribute("SystemFamily")]
        public string SystemFamily { get; set; }

        [WMIAttribute("SystemSKUNumber")]
        public string SystemSKUNumber { get; set; }

        [WMIAttribute("SystemStartupDelay")]
        public string SystemStartupDelay { get; set; }

        [WMIAttribute("SystemStartupOptions")]
        public string SystemStartupOptions { get; set; }

        [WMIAttribute("SystemStartupSetting")]
        public string SystemStartupSetting { get; set; }

        [WMIAttribute("SystemType")]
        public string SystemType { get; set; }

        [WMIAttribute("ThermalState")]
        public int ThermalState { get; set; }

        [WMIAttribute("TotalPhysicalMemory")]
        public long TotalPhysicalMemory { get; set; }

        [WMIAttribute("UserName")]
        public string UserName { get; set; }

        [WMIAttribute("WakeUpType")]
        public int WakeUpType { get; set; }

        [WMIAttribute("Workgroup")]
        public string Workgroup { get; set; }

    }
}