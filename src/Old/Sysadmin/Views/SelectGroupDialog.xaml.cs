using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SysAdmin.ActiveDirectory.Models;
using SysAdmin.ActiveDirectory.Repositories;
using SysAdmin.ActiveDirectory.Services.Ldap;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SysAdmin.Views
{
    public sealed partial class SelectGroupDialog : ContentDialog
    {

        public ObservableCollection<GroupEntry> Items { get; set; } = new ObservableCollection<GroupEntry>();

        private GroupEntry selected;
        public GroupEntry Selected
        {
            get { return selected; }
            set
            {
                selected = value;
                if (value != null)
                    IsPrimaryButtonEnabled = true;
                else
                    IsPrimaryButtonEnabled = false;
            }
        }

        private List<GroupEntry> cache = new List<GroupEntry>();


        public SelectGroupDialog()
        {
            this.InitializeComponent();

            IsPrimaryButtonEnabled = false;

            this.Loaded += SelectGroupDialog_Loaded;
        }

        private async void SelectGroupDialog_Loaded(object sender, RoutedEventArgs e)
        {
            Items.Clear();

            cache = await ListAsync();

            foreach (var item in cache)
            {
                Items.Add(item);
            }

        }

        public async Task<List<GroupEntry>> ListAsync()
        {
            List<GroupEntry> List = new List<GroupEntry>();

            await Task.Run(async () =>
            {
                using (var ldap = new LdapService(App.SERVER, App.CREDENTIAL))
                {
                    using (var groupsRepository = new GroupsRepository(ldap))
                    {
                        List = await groupsRepository.ListAsync();
                    }
                }
            });

            return List;
        }

        private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            Items.Clear();

            if (string.IsNullOrEmpty(sender.Text))
            {
                Items = new ObservableCollection<GroupEntry>(cache);
            }
            else
            {
                Items = new ObservableCollection<GroupEntry>(cache.Where(c => c.CN.ToUpper().StartsWith(sender.Text.ToUpper())));
            }
        }

    }
}