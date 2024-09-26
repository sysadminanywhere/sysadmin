using LdapForNet;
using SysAdmin.ActiveDirectory.Services.Ldap;
using SysAdmin.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;
using static LdapForNet.Native.Native;

namespace Sysadmin.Controls
{
    /// <summary>
    /// Interaction logic for DirectoryTreeControl.xaml
    /// </summary>
    public partial class DirectoryTreeControl : UserControl
    {
        public delegate void DirectoryTreeHandler(string DistinguishedName);
        public event DirectoryTreeHandler SelectedItem;

        public DirectoryTreeControl()
        {
            this.InitializeComponent();
        }

        public async Task Load()
        {

            progressRing.Visibility = Visibility.Visible;

            treeView.Items.Clear();

            TreeViewItem root = new TreeViewItem() { Header = "Root", IsExpanded = true };

            var list = await ListAsync();

            foreach (TreeItem item in list)
            {
                TreeViewItem node = new TreeViewItem() { Header = item.Name };
                node.Tag = item;
                node.Items.Add(new TreeViewItem());
                node.Expanded += Node_Expanded;
                node.Selected += Node_Selected;
                root.Items.Add(node);
            }

            treeView.Items.Add(root);

            progressRing.Visibility = Visibility.Collapsed;
        }

        private void Node_Selected(object sender, RoutedEventArgs e)
        {
            TreeViewItem node = (TreeViewItem)e.Source;

            if (SelectedItem != null)
            {
                TreeItem item = node.Tag as TreeItem;
                SelectedItem(item.DistinguishedName);
            }
        }

        private async void Node_Expanded(object sender, RoutedEventArgs e)
        {
            TreeViewItem node = (TreeViewItem)e.Source;

            if (node.Items.Count == 1)
            {
                node.Items.RemoveAt(0);

                if (node.Tag is TreeItem item)
                {
                    var children = await ListAsync(item.DistinguishedName);

                    if (children != null && children.Count > 0)
                    {
                        foreach (TreeItem child in children)
                        {
                            TreeViewItem treeViewItem = new TreeViewItem();
                            treeViewItem.Tag = child;
                            treeViewItem.Items.Add(new TreeViewItem());
                            treeViewItem.Header = child.Name;
                            treeViewItem.Expanded += Node_Expanded;
                            treeViewItem.Selected += Node_Selected;
                            node.Items.Add(treeViewItem);
                        }
                    }
                }
            }

        }

        private async Task<List<TreeItem>> ListAsync(string path = "")
        {
            List<TreeItem> list = new List<TreeItem>();

            await Task.Run(async () =>
            {
                using (var ldap = new LdapService(App.SERVER, App.CREDENTIAL))
                {
                    if (string.IsNullOrEmpty(path))
                        path = ldap.DefaultNamingContext;

                    var searchEntries = await ldap.SearchAsync(path, "(objectclass=*)", scope: LdapSearchScope.LDAP_SCOPE_ONELEVEL);

                    foreach (LdapEntry entry in searchEntries)
                    {
                        if (entry.DirectoryAttributes.Contains("CN")
                        && (entry.DirectoryAttributes["objectClass"].GetValues<string>().Contains("builtinDomain") || entry.DirectoryAttributes["objectClass"].GetValues<string>().Contains("container")))
                        {
                            TreeItem item = new TreeItem();
                            item.Name = entry.DirectoryAttributes["CN"].GetValue<string>();
                            item.DistinguishedName = entry.DirectoryAttributes["DistinguishedName"].GetValue<string>();
                            list.Add(item);
                        }
                        if (entry.DirectoryAttributes.Contains("OU"))
                        {
                            TreeItem item = new TreeItem();
                            item.Name = entry.DirectoryAttributes["OU"].GetValue<string>();
                            item.DistinguishedName = entry.DirectoryAttributes["DistinguishedName"].GetValue<string>();
                            list.Add(item);
                        }
                    }
                }
            });

            return list;
        }

    }
}