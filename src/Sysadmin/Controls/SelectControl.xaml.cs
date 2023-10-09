using LdapForNet;
using System.Windows;
using SysAdmin.ActiveDirectory.Services.Ldap;
using SysAdmin.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Controls;
using Wpf.Ui.Controls;
using System.Linq;
using System.Collections.Generic;

namespace Sysadmin.Controls
{
    /// <summary>
    /// Interaction logic for SelectControl.xaml
    /// </summary>
    public partial class SelectControl : UserControl
    {

        public delegate void SelectControHandler(string DistinguishedName);
        public event SelectControHandler SelectedItem;

        public ObservableCollection<LdapEntry> Items { get; private set; } = new ObservableCollection<LdapEntry>();

        private LdapEntry selected;
        public LdapEntry Selected
        {
            get { return selected; }
            set
            {
                selected = value;
                if (SelectedItem != null)
                {
                    SelectedItem(value.Dn);
                }
            }
        }

        public SelectControl()
        {
            InitializeComponent();
        }

        public async Task Load(string path, string filter)
        {
            progressRing.Visibility = Visibility.Visible;

            Items.Clear();

            List<LdapEntry> entries = await ListAsync(path, filter);

            if (entries.Count > 0)
            {
                foreach (LdapEntry entry in entries.OrderBy(c => c.Dn))
                {
                    Items.Add(entry);
                }
            }

            progressRing.Visibility = Visibility.Collapsed;
        }


        private void AutoSuggestBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private async Task<List<LdapEntry>> ListAsync(string path, string filter)
        {

            List<LdapEntry> entries = new List<LdapEntry>();

            await Task.Run(async () =>
            {
                using (var ldap = new LdapService(App.SERVER, App.CREDENTIAL))
                {
                    if (string.IsNullOrEmpty(path))
                        path = ldap.DefaultNamingContext;

                    entries = await ldap.SearchAsync(path, filter);
                }
            });

            return entries;
        }

    }
}