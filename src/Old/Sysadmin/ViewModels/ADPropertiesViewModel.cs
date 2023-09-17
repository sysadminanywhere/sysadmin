using LdapForNet;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using SysAdmin.ActiveDirectory.Models;
using SysAdmin.ActiveDirectory.Services.Ldap;
using SysAdmin.Models;
using SysAdmin.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace SysAdmin.ViewModels
{
    public class ADPropertiesViewModel : ObservableObject
    {

        public string DistinguishedName { get; set; }

        public ObservableCollection<NameValueItem> Items { get; private set; } = new ObservableCollection<NameValueItem>();


        IBusyService busyService = App.Current.Services.GetService<IBusyService>();

        public async Task LoadAsync()
        {
            busyService.Busy();

            List<LdapEntry> entries = new List<LdapEntry>();

            await Task.Run(async () =>
            {
                using (var ldap = new LdapService(App.SERVER, App.CREDENTIAL))
                {
                    entries = await ldap.SearchAsync("(distinguishedName=" + DistinguishedName + ")");
                }
            });

            Items.Clear();

            if (entries.Count == 1)
            {
                foreach (var attribute in entries[0].DirectoryAttributes)
                {
                    NameValueItem item = new NameValueItem();

                    item.Name = attribute.Name;

                    switch (item.Name.ToLower())
                    {
                        case "objectsid":
                            item.Value = new ADSID(attribute.GetValue<byte[]>()).ToString();
                            break;

                        case "objectguid":
                            item.Value = new Guid(attribute.GetValue<byte[]>()).ToString();
                            break;

                        case "usercertificate":
                            item.Value = "[binary]";
                            break;

                        default:
                            item.Value = string.Join(", ", attribute.GetValues<string>());
                            break;
                    }


                    Items.Add(item);
                }
            }

            OnPropertyChanged(nameof(Items));

            busyService.Idle();
        }

    }
}