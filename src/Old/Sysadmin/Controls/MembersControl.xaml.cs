using LdapForNet;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SysAdmin.ActiveDirectory.Repositories;
using SysAdmin.ActiveDirectory.Services.Ldap;
using SysAdmin.Models;
using SysAdmin.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SysAdmin.Controls
{
    public sealed partial class MembersControl : UserControl
    {

        public delegate void MembersHandler();
        public event MembersHandler Changed;

        public ObservableCollection<MemberItem> Items { get; private set; } = new ObservableCollection<MemberItem>();

        INotificationService notification = App.Current.Services.GetService<INotificationService>();

        private MemberItem selected;
        public MemberItem Selected
        {
            get { return selected; }
            set
            {
                selected = value;
                if (value != null)
                    deleteButton.IsEnabled = true;
                else
                    deleteButton.IsEnabled = false;
            }
        }

        public MembersControl()
        {
            this.InitializeComponent();
        }

        public List<string> Members
        {
            get
            {
                return (List<string>)GetValue(MembersProperty);
            }
            set
            {
                SetValue(MembersProperty, value);
                Update(value);
            }
        }

        public static readonly DependencyProperty MembersProperty = DependencyProperty.Register("MemberOf", typeof(string), typeof(MembersControl), new PropertyMetadata(null, valueChanged));

        private static void valueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MembersControl x = (MembersControl)d;
            if ((List<string>)e.NewValue != x.Members)
            {
                d.SetValue(e.Property, x.Members);
            }
        }

        private void Update(List<string> value)
        {
            Items.Clear();
            if (value != null)
                foreach (string item in value)
                    Items.Add(new MemberItem() { Name = ActiveDirectory.ADHelper.ExtractCN(item), DistinguishedName = item });
        }

        public string CN
        {
            get
            {
                return (string)GetValue(CNProperty);
            }
            set
            {
                SetValue(CNProperty, value);
            }
        }

        public static readonly DependencyProperty CNProperty = DependencyProperty.Register("CN", typeof(string), typeof(MembersControl), null);

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
        }

        private async void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await DeleteMember(CN, Selected.DistinguishedName);
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

    }

}