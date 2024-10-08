﻿using System.Management;
using System.Runtime.InteropServices;

namespace Sysadmin.WMI.Services
{
    public class WMIService : IDisposable
    {

        ManagementScope? managementScope = null;

        ICredential? credential;

        public ICredential? Credential { get { return credential; } }

        public WMIService(string computerAddress, ICredential? credential, bool inDomain)
        {
            if (computerAddress == null)
                throw new ArgumentNullException(nameof(computerAddress));

            if (!inDomain && computerAddress.Contains('.'))
                computerAddress = computerAddress.Split(".")[0];


            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                ConnectionOptions connectionOptions = new ConnectionOptions();

                if (credential != null)
                {
                    connectionOptions.Username = credential.UserName;
                    connectionOptions.Password = credential.Password;
                    connectionOptions.EnablePrivileges = true;
                }

                managementScope = new ManagementScope("\\\\" + computerAddress + "\\root\\cimv2", connectionOptions);
            }
            else
            {
                throw new PlatformNotSupportedException();
            }
        }

        public List<Dictionary<string, object>> Query(string queryString)
        {
            if (queryString == null)
                throw new ArgumentNullException(nameof(queryString));

            if (managementScope == null)
                throw new ArgumentNullException(nameof(managementScope));

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                List<Dictionary<string, object>> lst = new List<Dictionary<string, object>>();

                using (ManagementObjectSearcher query = new ManagementObjectSearcher(managementScope, new SelectQuery(queryString)))
                {
                    foreach (ManagementObject service in query.Get().OfType<ManagementObject>())
                    {
                        Dictionary<string, object> keyValues = new Dictionary<string, object>();

                        foreach (PropertyData data in service.Properties)
                        {
                            keyValues.Add(data.Name, data.Value);
                        }

                        lst.Add(keyValues);
                    }
                }

                return lst;
            }
            else
            {
                throw new PlatformNotSupportedException();
            }
        }

        public void Invoke(string queryString, string methodName, List<object>? args = null)
        {
            if (queryString == null)
                throw new ArgumentNullException(nameof(queryString));

            if (methodName == null)
                throw new ArgumentNullException(nameof(methodName));

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(managementScope, new SelectQuery(queryString));

                object obj = null;

                foreach (ManagementObject item in searcher.Get().OfType<ManagementObject>())
                {
                    if (args != null && args.Count > 0)
                        obj = item.InvokeMethod(methodName, args.ToArray());
                    else
                        obj = item.InvokeMethod(methodName, null);
                }
            }
            else
            {
                throw new PlatformNotSupportedException();
            }
        }

        public void Dispose()
        {
            if (managementScope != null)
                managementScope = null;
        }

    }
}
