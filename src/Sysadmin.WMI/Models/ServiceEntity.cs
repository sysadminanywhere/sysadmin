using System;
using System.Collections.Generic;
using System.Text;

namespace Sysadmin.WMI.Models
{
    public class ServiceEntity
    {

        [WMIAttribute("Caption")]
        public string Caption { get; set; }

        [WMIAttribute("Description")]
        public string Description { get; set; }

        [WMIAttribute("Name")]
        public string Name { get; set; }

        [WMIAttribute("State")]
        public string State { get; set; }

        [WMIAttribute("PathName")]
        public string PathName { get; set; }

        [WMIAttribute("DisplayName")]
        public string DisplayName { get; set; }

        [WMIAttribute("StartMode")]
        public string StartMode { get; set; }

        [WMIAttribute("ProcessId")]
        public string ProcessId { get; set; }

    }
}