namespace SysAdmin.ActiveDirectory.Models
{

    public enum GroupScopes
    {
        Local = 0,
        Global = 1,
        Universal = 2
    }

    public enum GroupType : uint
    {
        GLOBAL = 0x2,
        DOMAIN_LOCAL = 0x4,
        UNIVERSAL = 0x8,
        SECURITY = 0x80000000
    }

    public static class GroupTypeExtensions
    {

        public static long GetGroupType(GroupScopes groupScope, bool isSecurity)
        {
            long groupType = 0;

            if (isSecurity)
            {
                if (groupScope == GroupScopes.Global)
                    groupType = unchecked((int)(GroupType.GLOBAL | GroupType.SECURITY));

                if (groupScope == GroupScopes.Local)
                    groupType = unchecked((int)(GroupType.DOMAIN_LOCAL | GroupType.SECURITY));

                if (groupScope == GroupScopes.Universal)
                    groupType = unchecked((int)(GroupType.UNIVERSAL | GroupType.SECURITY));
            }
            else
            {
                if (groupScope == GroupScopes.Global)
                    groupType = unchecked((int)GroupType.GLOBAL);

                if (groupScope == GroupScopes.Local)
                    groupType = unchecked((int)GroupType.DOMAIN_LOCAL);

                if (groupScope == GroupScopes.Universal)
                    groupType = unchecked((int)GroupType.UNIVERSAL);
            }

            return groupType;
        }
    }

}