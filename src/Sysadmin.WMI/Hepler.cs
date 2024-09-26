using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Sysadmin.WMI
{
    public static class Helper
    {

        public static DateTime GetWMIDate(object date)
        {
            //                0000000000111111111122222
            //                0123456789012345678901234
            //LastBootUpTime: 20111212080531.125599+240

            string sDate = Convert.ToString(date);

            int year = Convert.ToInt32(sDate.Substring(0, 4));
            int month = Convert.ToInt32(sDate.Substring(4, 2));
            int day = Convert.ToInt32(sDate.Substring(6, 2));

            int hour = 0;
            int minute = 0;
            int second = 0;

            if (sDate.Length > 8)
            {
                hour = Convert.ToInt32(sDate.Substring(8, 2));
                minute = Convert.ToInt32(sDate.Substring(10, 2));
                second = Convert.ToInt32(sDate.Substring(12, 2));
            }

            return new DateTime(year, month, day, hour, minute, second);
        }

        public static List<string> GetCollection(object obj)
        {
            List<string> collection = new List<string>();

            string[] arr = null;

            if (obj is string)
            {
                arr = new string[1] { obj.ToString() };
            }
            else
            {
                //arr = (string[])(obj as JArray).ToObject(typeof(string[]));
                var col = obj as System.Collections.IEnumerable;
                if (col != null)
                {
                    arr = col.Cast<object>().Select(x => x.ToString()).ToArray();
                }
                else
                {
                    arr = new string[] { };
                }
            }

            foreach (string item in arr)
            {
                collection.Add(item);
            }

            return collection;
        }

    }
}