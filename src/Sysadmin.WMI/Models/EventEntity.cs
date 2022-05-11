using System;
using System.Collections.Generic;
using System.Text;

namespace Sysadmin.WMI.Models
{
    public class EventEntity
    {

        [WMIAttribute("RecordNumber")]
        public string RecordNumber { get; set; }

        [WMIAttribute("EventType")]
        public int EventType { get; set; }

        [WMIAttribute("EventCode")]
        public string EventCode { get; set; }

        [WMIAttribute("Type")]
        public string Type { get; set; }

        [WMIAttribute("TimeGenerated")]
        public DateTime TimeGenerated { get; set; }

        [WMIAttribute("SourceName")]
        public string SourceName { get; set; }

        [WMIAttribute("Category")]
        public string Category { get; set; }

        [WMIAttribute("User")]
        public string User { get; set; }

        [WMIAttribute("Message")]
        public string Message { get; set; }

        [WMIAttribute("Logfile")]
        public string Logfile { get; set; }

    }
}
