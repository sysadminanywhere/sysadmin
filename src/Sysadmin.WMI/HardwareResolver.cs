using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sysadmin.WMI
{
    public static class HardwareResolver<T>
    {

        public async static Task<List<T>> GetValues(List<Dictionary<string, object>> result)
        {
            List<T> entities = new List<T>();

            foreach (var item in result)
                entities.Add(WMIResolver<T>.GetValues(item));

            return entities;
        }

        public async static Task<T> GetValue(List<Dictionary<string, object>> result)
        {
            return WMIResolver<T>.GetValues(result.FirstOrDefault());
        }

    }
}