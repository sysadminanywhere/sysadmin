using LdapForNet;
using SysAdmin.ActiveDirectory.Models;
using SysAdmin.ActiveDirectory.Services.Ldap;
using static LdapForNet.Native.Native;

namespace SysAdmin.ActiveDirectory.Repositories
{
    public class GroupsRepository : IDisposable
    {

        private LdapService ldapService;

        public GroupsRepository(LdapService ldapService)
        {
            if (ldapService == null)
                throw new ArgumentNullException(nameof(ldapService));

            this.ldapService = ldapService;
        }

        public async Task<List<GroupEntry>> ListAsync()
        {
            List<GroupEntry> groups = new List<GroupEntry>();

            List<LdapEntry> list = await ldapService.SearchAsync("(objectClass=group)");

            foreach (LdapEntry entry in list)
            {
                groups.Add(ADResolver<GroupEntry>.GetValues(entry));
            }

            return groups;
        }

        public async Task<GroupEntry?> GetByCNAsync(string cn)
        {
            if (string.IsNullOrEmpty(cn))
                throw new ArgumentNullException(nameof(cn));

            var result = await ldapService.SearchAsync("(&(objectClass=group)(cn=" + cn + "))");
            var entry = result.FirstOrDefault();

            if (entry != null)
                return ADResolver<GroupEntry>.GetValues(entry);
            else
                return null;
        }

        public async Task<GroupEntry?> AddAsync(GroupEntry group)
        {
            return await AddAsync(string.Empty, group, GroupScopes.Global, true);
        }

        public async Task<GroupEntry?> AddAsync(GroupEntry group, GroupScopes groupScope, bool isSecurity)
        {
            return await AddAsync(string.Empty, group, groupScope, isSecurity);
        }

        public async Task<GroupEntry?> AddAsync(string distinguishedName, GroupEntry group, GroupScopes groupScope, bool isSecurity)
        {
            if (group == null)
                throw new ArgumentNullException(nameof(group));

            if (string.IsNullOrEmpty(group.CN))
                throw new ArgumentNullException(nameof(group.CN));

            List<string> attributes = new List<string>
            {
                "description",
                "groupType",
                "sAMAccountName"
            };

            if (string.IsNullOrEmpty(group.SamAccountName))
                group.SamAccountName = group.CN;

            group.GroupType = GroupTypeExtensions.GetGroupType(groupScope, isSecurity);

            if (string.IsNullOrEmpty(distinguishedName))
            {
                string cn = "cn=" + group.CN + "," + new ADContainers(ldapService).GetUsersContainer();
                await ldapService.AddAsync(LdapResolver.GetLdapEntry(cn, group, attributes));
            }
            else
            {
                string cn = "cn=" + group.CN + "," + distinguishedName;
                await ldapService.AddAsync(LdapResolver.GetLdapEntry(cn, group, attributes));
            }

            var result = await ldapService.SearchAsync("(&(objectClass=group)(cn=" + group.CN + "))");
            var entry = result.FirstOrDefault();

            if (entry != null)
                return ADResolver<GroupEntry>.GetValues(entry);
            else
                return null;
        }

        public async Task<GroupEntry?> ModifyAsync(GroupEntry group)
        {
            if (group == null)
                throw new ArgumentNullException(nameof(group));

            if (string.IsNullOrEmpty(group.CN))
                throw new ArgumentNullException(nameof(group.CN));

            if (string.IsNullOrEmpty(group.DistinguishedName))
                throw new ArgumentNullException(nameof(group.DistinguishedName));

            List<string> attributes = new List<string>
            {
                "description"
            };

            var result = await ldapService.SearchAsync("(&(objectClass=group)(cn=" + group.CN + "))");
            var entry = result.FirstOrDefault();

            if (entry != null)
            {
                GroupEntry oldGroup = ADResolver<GroupEntry>.GetValues(entry);
                await ldapService.SendRequestAsync(new ModifyRequest(group.DistinguishedName, LdapResolver.GetDirectoryModificationAttributes(group, oldGroup, attributes).ToArray()));

                var newGroup = await GetByCNAsync(group.CN);
                if (newGroup != null)
                    return newGroup;
            }

            return null;
        }

        public async Task DeleteAsync(GroupEntry group)
        {
            if (group == null)
                throw new ArgumentNullException(nameof(group));

            if (string.IsNullOrEmpty(group.DistinguishedName))
                throw new ArgumentNullException(nameof(group.DistinguishedName));

            await ldapService.DeleteAsync(group.DistinguishedName);
        }

        public async Task AddMemberAsync(GroupEntry group, string distinguishedName)
        {
            if (group == null)
                throw new ArgumentNullException(nameof(group));

            if (string.IsNullOrEmpty(group.CN))
                throw new ArgumentNullException(nameof(group.CN));

            if (string.IsNullOrEmpty(group.DistinguishedName))
                throw new ArgumentNullException(nameof(group.DistinguishedName));

            if (string.IsNullOrEmpty(distinguishedName))
                throw new ArgumentNullException(nameof(distinguishedName));

            var result = await ldapService.SearchAsync("(&(objectClass=group)(cn=" + group.CN + "))");
            var entry = result.FirstOrDefault();

            if (entry != null)
            {
                var attribute = new DirectoryModificationAttribute()
                {
                    Name = "member",
                    LdapModOperation = LdapModOperation.LDAP_MOD_ADD
                };

                attribute.Add(distinguishedName);

                await ldapService.SendRequestAsync(new ModifyRequest(group.DistinguishedName, attribute));
            }
        }

        public async Task DeleteMemberAsync(GroupEntry group, string distinguishedName)
        {
            if (group == null)
                throw new ArgumentNullException(nameof(group));

            if (string.IsNullOrEmpty(group.CN))
                throw new ArgumentNullException(nameof(group.CN));

            if (string.IsNullOrEmpty(group.DistinguishedName))
                throw new ArgumentNullException(nameof(group.DistinguishedName));

            if (string.IsNullOrEmpty(distinguishedName))
                throw new ArgumentNullException(nameof(distinguishedName));

            var result = await ldapService.SearchAsync("(&(objectClass=group)(cn=" + group.CN + "))");
            var entry = result.FirstOrDefault();

            if (entry != null)
            {
                var attribute = new DirectoryModificationAttribute()
                {
                    Name = "member",
                    LdapModOperation = LdapModOperation.LDAP_MOD_DELETE
                };

                attribute.Add(distinguishedName);

                await ldapService.SendRequestAsync(new ModifyRequest(group.DistinguishedName, attribute));
            }
        }

        public void Dispose()
        {

        }

    }
}