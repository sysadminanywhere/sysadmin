using LdapForNet;
using SysAdmin.ActiveDirectory.Models;
using System.Reflection;
using static LdapForNet.Native.Native;

namespace SysAdmin.ActiveDirectory
{
    public static class LdapResolver
    {

        public static List<DirectoryModificationAttribute> GetDirectoryModificationAttributes(IADEntry entry, IADEntry entryOld, List<string> attributes)
        {
            List<DirectoryModificationAttribute> directoryModificationAttributes = new List<DirectoryModificationAttribute>();

            List<PropertyInfo> properties = entry.GetType().GetRuntimeProperties().ToList();
            List<PropertyInfo> propertiesOld = entryOld.GetType().GetRuntimeProperties().ToList();

            foreach (PropertyInfo property in properties)
            {
                var adAttributes = (ADAttribute[])property.GetCustomAttributes(typeof(ADAttribute), true);

                if (adAttributes != null && adAttributes.Length > 0)
                {
                    var attribute = attributes.FirstOrDefault(c => c.ToLower() == adAttributes[0].Name.ToLower());

                    if (attribute != null)
                    {
                        object? value = property.GetValue(entry);
                        var propertyOld = propertiesOld.Where(c => c.Name == property.Name).First();
                        object? valueOld = propertyOld.GetValue(entryOld);

                        if (value is string && string.IsNullOrEmpty(value.ToString()))
                            value = null;

                        if (value != null)
                        {
                            LdapModOperation ldapModOperation = LdapModOperation.LDAP_MOD_REPLACE;

                            if (valueOld == null)
                                ldapModOperation = LdapModOperation.LDAP_MOD_ADD;

                            var modificationAttribute = new DirectoryModificationAttribute()
                            {
                                Name = attribute,
                                LdapModOperation = ldapModOperation
                            };
                            if (property.PropertyType == typeof(string))
                                modificationAttribute.Add<string>(value.ToString());

                            if (property.PropertyType == typeof(List<string>))
                                modificationAttribute.Add(new List<string> { value.ToString() });

                            directoryModificationAttributes.Add(modificationAttribute);
                        }
                        else
                        {
                            if (valueOld != null)
                            {
                                var modificationAttribute = new DirectoryModificationAttribute()
                                {
                                    Name = attribute,
                                    LdapModOperation = LdapModOperation.LDAP_MOD_DELETE
                                };
                                directoryModificationAttributes.Add(modificationAttribute);
                            }
                        }

                    }
                }
            }

            return directoryModificationAttributes;
        }

        public static LdapEntry GetLdapEntry(string dn, IADEntry entry, List<string> attributes)
        {
            LdapEntry ldapEntry = new LdapEntry();

            ldapEntry.Dn = dn;

            Dictionary<string, List<string>> keyValues = new Dictionary<string, List<string>>();

            List<PropertyInfo> properties = entry.GetType().GetRuntimeProperties().ToList();

            foreach (PropertyInfo property in properties)
            {
                var adAttributes = (ADAttribute[])property.GetCustomAttributes(typeof(ADAttribute), true);

                if (adAttributes != null && adAttributes.Length > 0)
                {
                    var attribute = attributes.FirstOrDefault(c => c.ToLower() == adAttributes[0].Name.ToLower());

                    if (attribute != null)
                    {
                        object? value = property.GetValue(entry);

                        if (value != null)
                        {
                            keyValues.Add(attribute, new List<string> { value.ToString() });
                        }
                    }
                }
            }

            keyValues.Add("objectclass", entry.ObjectClass);

            ldapEntry.Attributes = keyValues;

            return ldapEntry;
        }

    }
}