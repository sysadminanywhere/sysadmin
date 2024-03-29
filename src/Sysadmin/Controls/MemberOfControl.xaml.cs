﻿using SysAdmin.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using SysAdmin.ActiveDirectory.Repositories;
using SysAdmin.ActiveDirectory.Services.Ldap;
using System.ComponentModel;
using SysAdmin.ActiveDirectory;
using LdapForNet;

namespace Sysadmin.Controls
{
    /// <summary>
    /// Interaction logic for MemberOfControl.xaml
    /// </summary>
    public partial class MemberOfControl : UserControl
    {

        public delegate void MemberOfHandler();
        public event MemberOfHandler Changed;

        public delegate void MemberOfErrorHandler(string ErrorMessage);
        public event MemberOfErrorHandler Error;

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

        public static readonly DependencyProperty MemberOfProperty =
        DependencyProperty.Register(
            name: "MemberOf",
            propertyType: typeof(List<string>),
            ownerType: typeof(MemberOfControl));

        public List<string> MemberOf
        {
            get => (List<string>)GetValue(MemberOfProperty);
            set
            {
                SetValue(MemberOfProperty, value);
            }
        }

        public static readonly DependencyProperty DistinguishedNameProperty =
            DependencyProperty.Register(
                name: "DistinguishedName",
                propertyType: typeof(string),
                ownerType: typeof(MemberOfControl),
                typeMetadata: new FrameworkPropertyMetadata(defaultValue: string.Empty));

        public string DistinguishedName
        {
            get => (string)GetValue(DistinguishedNameProperty);
            set => SetValue(DistinguishedNameProperty, value);
        }

        public static readonly DependencyProperty CNProperty =
            DependencyProperty.Register(
                name: "CN",
                propertyType: typeof(string),
                ownerType: typeof(MemberOfControl),
                typeMetadata: new FrameworkPropertyMetadata(defaultValue: string.Empty));

        public string CN
        {
            get => (string)GetValue(CNProperty);
            set => SetValue(CNProperty, value);
        }

        public static readonly DependencyProperty PrimaryGroupIdProperty =
        DependencyProperty.Register(
            name: "PrimaryGroupId",
            propertyType: typeof(int),
            ownerType: typeof(MemberOfControl),
            typeMetadata: new FrameworkPropertyMetadata(defaultValue: 0));

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
                    string group = ADHelper.GetPrimaryGroup(value);
                    if (!string.IsNullOrEmpty(group))
                        if (Items.FirstOrDefault(c => c.Name == group) == null)
                            Items.Add(new MemberItem() { Name = group, DistinguishedName = string.Empty });
                }
            }
        }

        private void Update(List<string> value)
        {
            Items.Clear();
            if (value != null)
                foreach (string item in value)
                    Items.Add(new MemberItem() { Name = ADHelper.ExtractCN(item), DistinguishedName = item });

            if (PrimaryGroupId != 0)
            {
                string group = ADHelper.GetPrimaryGroup(PrimaryGroupId);
                if (!string.IsNullOrEmpty(group))
                    Items.Add(new MemberItem() { Name = group, DistinguishedName = string.Empty });
            }
        }

        public MemberOfControl()
        {
            InitializeComponent();

            DependencyPropertyDescriptor dpd = DependencyPropertyDescriptor.FromProperty(MemberOfProperty, typeof(MemberOfControl));

            if (dpd != null)
            {
                dpd.AddValueChanged(this, OnChanged);
            }
        }

        private void OnChanged(object? sender, EventArgs e)
        {
            Update(MemberOf);
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            flyout.Show();
            selectControl.Load(SelectControl.Show.Groups);
        }

        private async void deleteButton_Click(object sender, RoutedEventArgs e) //NOSONAR
        {
            try
            {
                await DeleteMember(Selected.Name, DistinguishedName);
                if (Changed != null) Changed();
            }
            catch (LdapException le)
            {
                if (Error != null)
                    Error(LdapResult.GetErrorMessageFromResult(le.ResultCode));
            }
            catch (Exception ex)
            {
                if (Error != null)
                    Error(ex.Message);
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

        private async void selectControl_SelectedItem(MemberItem item) //NOSONAR
        {
            flyout.Hide();

            try
            {
                await AddMember(item.Name, DistinguishedName);
                if (Changed != null) Changed();
            }
            catch (LdapException le)
            {
                if (Error != null)
                    Error(LdapResult.GetErrorMessageFromResult(le.ResultCode));
            }
            catch (Exception ex)
            {
                if (Error != null)
                    Error(ex.Message);
            }
        }

    }
}