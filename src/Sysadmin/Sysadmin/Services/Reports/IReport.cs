using FastReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysAdmin.Services.Reports
{
    public interface IReport
    {

        string Name { get; }
        string Description { get; }
        string Group { get; }

        Task<Report> Report();

    }
}
