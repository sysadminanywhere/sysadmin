using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Sysadmin.WMI
{
    public static class WmiResolver<T>
    {

        public static T GetValues(Dictionary<string, object> properties)
        {
            T result = (T)Activator.CreateInstance(typeof(T));

            if (properties != null)
            {

                foreach (PropertyInfo property in result.GetType().GetRuntimeProperties())
                {

                    string propertyName = property.Name;

                    var attributes = (WMIAttribute[])property.GetCustomAttributes(typeof(WMIAttribute), true);

                    if (attributes.Any())
                    {
                        propertyName = attributes[0].Name;
                    }

                    if (properties.ContainsKey(propertyName))
                    {

                        if (property.PropertyType == typeof(string))
                        {
                            if (properties[propertyName] != null)
                                property.SetValue(result, Convert.ToString(properties[propertyName]));
                            else
                                property.SetValue(result, string.Empty);
                        }

                        if (property.PropertyType == typeof(DateTime) || property.PropertyType == typeof(DateTime?))
                            property.SetValue(result, Helper.GetWMIDate(properties[propertyName]));

                        if (property.PropertyType == typeof(bool))
                            property.SetValue(result, Convert.ToBoolean(properties[propertyName]));

                        if (property.PropertyType == typeof(int))
                        {
                            if (properties[propertyName] != null)
                            {
                                if(properties[propertyName] is int)
                                    property.SetValue(result, Convert.ToInt32(properties[propertyName]));
                                else
                                    property.SetValue(result, -111);
                            }
                            else
                            {
                                property.SetValue(result, 0);
                            }
                        }

                        if (property.PropertyType == typeof(long))
                            property.SetValue(result, Convert.ToInt64(properties[propertyName]));

                        if (property.PropertyType == typeof(byte[]))
                            property.SetValue(result, Encoding.ASCII.GetBytes(properties[propertyName].ToString()));

                        if (property.PropertyType == typeof(List<String>))
                            property.SetValue(result, Helper.GetCollection(properties[propertyName]));

                    }
                }

            }

            return result;
        }

    }
}