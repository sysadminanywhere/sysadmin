using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Sysadmin.WMI
{
    public static class WMIResolver<T>
    {

        public static T GetValues(Dictionary<string, object> properties)
        {
            T result = (T)Activator.CreateInstance(typeof(T));

            foreach (PropertyInfo property in result.GetType().GetRuntimeProperties())
            {

                string propertyName = property.Name;

                var attributes = (WMIAttribute[])property.GetCustomAttributes(typeof(WMIAttribute), true);

                if (attributes.Count() > 0)
                {
                    propertyName = attributes[0].Name;
                }

                if (properties.ContainsKey(propertyName))
                {

                    if (property.PropertyType == typeof(string))
                    {
                        if (properties[propertyName] != null)
                            property.SetValue(result, properties[propertyName].ToString());
                        else
                            property.SetValue(result, string.Empty);
                    }

                    if (property.PropertyType == typeof(DateTime) || property.PropertyType == typeof(DateTime?))
                        property.SetValue(result, Helper.GetWMIDate(properties[propertyName].ToString()));

                    if (property.PropertyType == typeof(bool))
                        property.SetValue(result, bool.Parse(properties[propertyName].ToString()));

                    if (property.PropertyType == typeof(int))
                    {
                        if (properties[propertyName] != null)
                            property.SetValue(result, int.Parse(properties[propertyName].ToString()));
                        else
                            property.SetValue(result, 0);
                    }

                    if (property.PropertyType == typeof(long))
                        property.SetValue(result, long.Parse(properties[propertyName].ToString()));

                    if (property.PropertyType == typeof(byte[]))
                        property.SetValue(result, Encoding.ASCII.GetBytes(properties[propertyName].ToString()));

                    if (property.PropertyType == typeof(List<String>))
                        property.SetValue(result, Helper.GetCollection(properties[propertyName]));

                }
            }

            return result;
        }

    }
}