using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sysadmin.Services
{
    public interface  IExchangeService
    {

        void SetParameter(object parameter);

        object GetParameter();

    }
}
