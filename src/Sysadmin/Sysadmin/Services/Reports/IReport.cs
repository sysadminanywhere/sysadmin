using FastReport;
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
