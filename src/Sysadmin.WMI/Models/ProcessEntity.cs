using System;
using System.Collections.Generic;
using System.Text;

namespace Sysadmin.WMI.Models
{
    public class ProcessEntity
    {
        [WMIAttribute("Caption")]
        public string Caption { get; set; }

        [WMIAttribute("ExecutablePath")]
        public string ExecutablePath { get; set; }

        [WMIAttribute("Description")]
        public string Description { get; set; }

        [WMIAttribute("Handle")]
        public string Handle { get; set; }

        [WMIAttribute("WorkingSetSize")]
        public string WorkingSetSize { get; set; }

    }
}
