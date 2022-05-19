using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SysAdmin.ActiveDirectory.Repositories;
using SysAdmin.ActiveDirectory.Services.Ldap;
using SysAdmin.Models;
using SysAdmin.Services;
using SysAdmin.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SysAdmin.Controls
{
    public sealed partial class MemberOfControl : UserControl
    {

        public delegate void MemberOfHandler();
        public event MemberOfHandler Changed;

        INotificationService notification = App.Current.Services.GetService<INotificationService>();

        public ObservableCollection<MemberItem> Items { get; private set; } = new ObservableCollection<MemberItem>();

        private MemberItem selected;
        public MemberItem Selected
        {
            get { return selected; }
            set
            {
                selected = value;
                if (value != null && !string.IsNullOrEmpty(value.DistinguishedName))
                    deleteButton.IsEnabled = true;
                else
                    deleteButton.IsEnabled = false;
            }
        }

        public MemberOfControl()
        {
            this.InitializeComponent();
        }

        public List<string> MemberOf
        {
            get
            {
                return (List<string>)GetValue(MemberOfProperty);
            }
            set
            {
                SetValue(MemberOfProperty, value);
                Update(value);
            }
        }

        public static readonly DependencyProperty MemberOfProperty = DependencyProperty.Register("MemberOf", typeof(string), typeof(MemberOfControl), new PropertyMetadata(null, valueChanged));

        private static void valueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MemberOfControl x = (MemberOfControl)d;
            if ((List<string>)e.NewValue != x.MemberOf)
            {
                d.SetValue(e.Property, x.MemberOf);
            }
        }

        private void Update(List<string> value)
        {
            Items.Clear();
            if (value != null)
                foreach (string item in value)
                    Items.Add(new MemberItem() { Name = ActiveDirectory.ADHelper.ExtractCN(item), DistinguishedName = item });

            if (PrimaryGroupId != 0)
            {
                string group = ActiveDirectory.ADHelper.GetPrimaryGroup(PrimaryGroupId);
                if (!String.IsNullOrEmpty(group))
                    Items.Add(new MemberItem() { Name = group, DistinguishedName = String.Empty });
            }
        }


        public string DistinguishedName
        {
            get
            {
                return (string)GetValue(DistinguishedNameProperty);
            }
            set
            {
                SetValue(DistinguishedNameProperty, value);
            }
        }

        public static readonly DependencyProperty DistinguishedNameProperty = DependencyProperty.Register("DistinguishedName", typeof(string), typeof(MemberOfControl), null);


        public int PrimaryGroupId
        {
            get
            {
                return (int)GetValue(PrimaryGroupIdProperty);
            }
            set
            {
                SetValue(PrimaryGroupIdProperty, value);
                if (value != 0)
                {
                    string group = ActiveDirectory.ADHelper.GetPrimaryGroup(value);
                    if (!string.IsNullOrEmpty(group))
                        if (Items.FirstOrDefault(c => c.Name == group) == null)
                            Items.Add(new MemberItem() { Name = group, DistinguishedName = string.Empty });
                }
            }
        }

        public static readonly DependencyProperty PrimaryGroupIdProperty = DependencyProperty.Register("PrimaryGroupId", typeof(int), typeof(MemberOfControl), null);

        private async void addButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new SelectGroupDialog();
            dialog.XamlRoot = this.XamlRoot;

            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                try
                {
                    await AddMember(dialog.Selected.CN, DistinguishedName);
                    if (Changed != null) Changed();
                }
                catch (LdapException le)
                {
                    notification.ShowErrorMessage(ActiveDirectory.LdapResult.GetErrorMessageFromResult(le.ResultCode));
                }
                catch (Exception ex)
                {
                    notification.ShowErrorMessage(ex.Message);
                }
            }
        }

        private async void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await DeleteMember(Selected.Name, DistinguishedName);
                if (Changed != null) Changed();
            }
            catch (LdapException le)
            {
                notification.ShowErrorMessage(ActiveDirectory.LdapResult.GetErrorMessageFromResult(le.ResultCode));
            }
            catch (Exception ex)
            {
                notification.ShowErrorMessage(ex.Message);
            }
        }

        private async Task DeleteMember(string groupCN, string distinguishedName)
        {
            await Task.Run(async () =>
            {
                using (var ldap = new LdapService(App.SERVER, App.CREDENTIAL))
                {
                    using (var groupsRepository = new GroupsRepository(ldap))
                    {
                        var group = await groupsRepository.GetByCNAsync(groupCN);

                        if (group != null)
                        {
                            await groupsRepository.DeleteMemberAsync(group, distinguishedName);
                        }

                    }
                }
            });
        }

        private async Task AddMember(string groupCN, string distinguishedName)
        {
            await Task.Run(async () =>
            {
                using (var ldap = new LdapService(App.SERVER, App.CREDENTIAL))
                {
                    using (var groupsRepository = new GroupsRepository(ldap))
                    {
                        var group = await groupsRepository.GetByCNAsync(groupCN);

                        if (group != null)
                        {
                            await groupsRepository.AddMemberAsync(group, distinguishedName);
                        }

                    }
                }
            });
        }

    }

}