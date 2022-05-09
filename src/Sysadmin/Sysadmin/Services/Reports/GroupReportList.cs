using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
