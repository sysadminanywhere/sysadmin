namespace SysAdmin.ActiveDirectory.Models
{

    /*

    http://social.technet.microsoft.com/wiki/contents/articles/12079.active-directory-get-adgroup-default-and-extended-properties.aspx
    
    Property	                        Syntax	        R/RW	lDAPDisplayName
    
    CanonicalName	                    String	        R	    canonicalName
    CN	                                String	        R	    cn
    Created	                            DateTime	    R	    whenCreated
    Deleted	                            Boolean	        R	    isDeleted
    Description	                        String	        RW	    description
    DisplayName	                        String	        RW	    displayName
    DistinguishedName	                String (DN)	    RW	    distinguishedName
    GroupCategory	                    String	        RW	    groupType (bit mask 2147483648)
    GroupScope	                        String	        RW	    groupType (bit mask 1, 2, 4, or 8)
    HomePage	                        String	        RW	    wWWHomePage
    LastKnownParent	                    String (DN)	    R	    lastKnownParent
    ManagedBy	                        String (DN)	    RW	    managedBy
    MemberOf	                        ADCollection	R	    memberOf
    Members	                            ADCollection	R	    member
    Modified	                        DateTime	    R	    whenChanged
    Name	                            String	        R	    cn (Relative Distinguished Name)
    ObjectCategory	                    String	        R	    objectCategory
    ObjectClass	                        String	        RW	    objectClass, most specific value
    ObjectGUID	                        Guid	        RW	    objectGUID converted to string
    ProtectedFromAccidentalDeletion	    Boolean	        RW	    nTSecurityDescriptor
    SamAccountName	                    String	        RW	    sAMAccountName
    SID	                                Sid	            RW	    objectSID converted to string
    SIDHistory	                        ADCollection	R	    sidHistory
    
    */

    public class GroupEntry : IADEntry
    {
        [AD("cn", true)]
        public string CN { get; set; }

        [AD("whencreated", ADAttribute.DateTypes.Date, true)]
        public DateTime? Created { get; set; }

        [AD("description")]
        public string Description { get; set; }

        [AD("distinguishedname", true)]
        public string DistinguishedName { get; set; }

        [AD("whenchanged", ADAttribute.DateTypes.Date, true)]
        public DateTime? Modified { get; set; }

        [AD("objectcategory", true)]
        public string ObjectCategory { get; set; }

        [AD("objectclass", true)]
        public List<string> ObjectClass { get; set; } = new List<string> { "Group" };

        [AD("objectguid", true)]
        public Guid ObjectGUID { get; set; }

        [AD("objectsid", true)]
        public ADSID SID { get; set; }

        [AD("samaccountname")]
        public string SamAccountName { get; set; }

        [AD("managedby")]
        public string ManagedBy { get; set; }

        [AD("memberof", true)]
        public List<string> MemberOf { get; set; }

        [AD("member", true)]
        public List<string> Members { get; set; }

        [AD("grouptype")]
        public long GroupType { get; set; }

        [AD("iscriticalsystemobject")]
        public bool IsCriticalSystemObject { get; set; }

        [AD("samaccounttype")]
        public int SamAccountType { get; set; }

        [AD("systemflags")]
        public int SystemFlags { get; set; }

        [AD("adspath")]
        public string AdsPath { get; set; }

        [AD("name")]
        public string Name { get; set; }

        [AD("instancetype")]
        public int InstanceType { get; set; }

        [AD("admincount")]
        public int AdminCount { get; set; }

        [AD("primarygroupid")]
        public int PrimaryGroupId { get; set; }

        public string ADGroupType
        {
            get
            {
                return ADHelper.GetGroupType(GroupType);
            }
        }
    }

}