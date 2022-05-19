using LdapForNet;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SysAdmin.ActiveDirectory.Services.Ldap;
using SysAdmin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static LdapForNet.Native.Native;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SysAdmin.Controls
{
    public sealed partial class DirectoryTreeControl : UserControl
    {

        public delegate void DirectoryTreeHandler(string DistinguishedName);
        public event DirectoryTreeHandler SelectedItem;

        private TreeViewNode selected;
        public TreeViewNode Selected
        {
            get { return selected; }
            set
            {
                selected = value;
                if (value != null && selected.Content is TreeItem)
                {
                    if (SelectedItem != null)
                    {
                        SelectedItem((selected.Content as TreeItem).DistinguishedName);
                    }
                }
            }
        }

        public DirectoryTreeControl()
        {
            this.InitializeComponent();

            this.Loaded += DirectoryTreeControl_Loaded;
        }

        private async void DirectoryTreeControl_Loaded(object sender, RoutedEventArgs e)
        {
            treeView.RootNodes.Clear();

            TreeViewNode root = new TreeViewNode() { Content = "Root", IsExpanded = true };

            var list = await ListAsync();

            foreach (TreeItem item in list)
            {
                TreeViewNode node = new TreeViewNode() { Content = item };
                node.Children.Add(new TreeViewNode() { Content = new TreeItem() });
                root.Children.Add(node);
            }

            treeView.RootNodes.Add(root);
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

        private async void treeView_Expanding(TreeView sender, TreeViewExpandingEventArgs args)
        {
            TreeViewNode node = args.Node;

            if (node.Children.Count == 1)
            {
                if (node.Content is TreeItem)
                {
                    TreeItem item = (TreeItem)node.Content;

                    TreeItem item0 = (TreeItem)node.Children[0].Content;

                    if (string.IsNullOrEmpty(item0.Name))
                    {
                        var children = await ListAsync(item.DistinguishedName);

                        if (children != null && children.Count > 0)
                        {
                            foreach (TreeItem child in children)
                            {
                                node.Children.Add(new TreeViewNode() { Content = child });
                            }
                        }

                        node.Children.RemoveAt(0);
                    }
                }
            }

        }
    }
}