using System.Collections.Generic;

namespace SysAdmin.Services.Reports
{
    public class GroupReportList : List<object>
    {
        public GroupReportList(IEnumerable<object> items) : base(items)
        {
        }
        public object Key { get; set; }
    }
}
