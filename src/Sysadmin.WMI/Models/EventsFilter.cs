using System;
using System.Collections.Generic;
using System.Text;

namespace Sysadmin.WMI.Models
{
    public enum EventsFilter
    {
        TodayErrors,
        TodayWarnings,
        TodayInformations,
        TodaySecurityAuditSuccess,
        TodaySecurityAuditFailure,
        TodayAll
    }
}
