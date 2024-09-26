using SysAdmin.ActiveDirectory.Models;
using SysAdmin.ActiveDirectory.Repositories;
using SysAdmin.ActiveDirectory.Services.Ldap;
using SysAdmin.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Wpf.Ui.Controls;

namespace Sysadmin.Controls
{
    /// <summary>
    /// Interaction logic for SelectControl.xaml
    /// </summary>
    public partial class SelectControl : UserControl, INotifyPropertyChanged
    {

        public delegate void SelectControHandler(MemberItem item);
        public event SelectControHandler SelectedItem;

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public enum Show
        {
            All,
            Computers,
            Users,
            Groups
        }

        public ObservableCollection<MemberItem> Items { get; private set; } = new ObservableCollection<MemberItem>();

        private List<MemberItem> cache = new List<MemberItem>();

        private MemberItem selected;
        public MemberItem Selected
        {
            get { return selected; }
            set
            {
                selected = value;
                if (SelectedItem != null && value != null)
                {
                    SelectedItem(value);
                }
            }
        }

        public SelectControl()
        {
            InitializeComponent();
        }

        public async Task Load(Show show)
        {
            progressRing.Visibility = Visibility.Visible;

            Items.Clear();

            cache = await ListAsync(show);

            if (cache == null)
                cache = new List<MemberItem>();

            Items = new ObservableCollection<MemberItem>(cache);

            OnPropertyChanged("Items");

            progressRing.Visibility = Visibility.Collapsed;
        }


        private void AutoSuggestBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = (sender as AutoSuggestBox).Text;
            if (string.IsNullOrEmpty(searchText))
                Items = new ObservableCollection<MemberItem>(cache);
            else
                Items = new ObservableCollection<MemberItem>(cache.Where(c => c.Name.ToUpper().StartsWith(searchText.ToUpper())));

            OnPropertyChanged("Items");
        }

        private async Task<List<MemberItem>> ListAsync(Show show)
        {
            List<MemberItem> members = new List<MemberItem>();

            await Task.Run(async () =>
            {
                using (var ldap = new LdapService(App.SERVER, App.CREDENTIAL))
                {

                    switch (show)
                    {
                        case Show.Computers:
                            members = await Computers(ldap);
                            break;

                        case Show.Users:
                            members = await Users(ldap);
                            break;

                        case Show.Groups:
                            members = await Groups(ldap);
                            break;

                        case Show.All:
                            members = await Computers(ldap);
                            members.AddRange(await Users(ldap));
                            members.AddRange(await Groups(ldap));
                            break;
                    }

                }
            });

            return members.OrderBy(c => c.Name).ToList();
        }

        private async Task<List<MemberItem>> Computers(LdapService ldap)
        {
            List<MemberItem> members = new List<MemberItem>();

            List<ComputerEntry> computers = await new ComputersRepository(ldap).ListAsync();
            foreach (ComputerEntry computer in computers)
                members.Add(new MemberItem() { Name = computer.CN, DistinguishedName = computer.DistinguishedName });

            return members;
        }

        private async Task<List<MemberItem>> Users(LdapService ldap)
        {
            List<MemberItem> members = new List<MemberItem>();

            List<UserEntry> users = await new UsersRepository(ldap).ListAsync();
            foreach (UserEntry user in users)
                members.Add(new MemberItem() { Name = user.CN, DistinguishedName = user.DistinguishedName });

            return members;
        }

        private async Task<List<MemberItem>> Groups(LdapService ldap)
        {
            List<MemberItem> members = new List<MemberItem>();

            List<GroupEntry> groups = await new GroupsRepository(ldap).ListAsync();
            foreach (GroupEntry group in groups)
                members.Add(new MemberItem() { Name = group.CN, DistinguishedName = group.DistinguishedName });

            return members;
        }

    }
}