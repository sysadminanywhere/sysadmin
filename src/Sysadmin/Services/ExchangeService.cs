using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sysadmin.Services
{
    public class ExchangeService : IExchangeService
    {
        private object parameter;

        public object GetParameter()
        {
            return parameter;
        }

        public void SetParameter(object parameter)
        {
            this.parameter = parameter;
        }
    }
}
