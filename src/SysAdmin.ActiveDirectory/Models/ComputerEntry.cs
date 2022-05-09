namespace SysAdmin.ActiveDirectory.Models
{

    /*

    http://social.technet.microsoft.com/wiki/contents/articles/12056.active-directory-get-adcomputer-default-and-extended-properties.aspx
    
    Property	                        Syntax	        R/RW	lDAPDisplayName
    
    AccountExpirationDate	            DateTime	    RW	    accountExpires, local time
    AccountLockoutTime	                DateTime	    RW	    lockoutTime, local time
    AccountNotDelegated	                Boolean	        RW	    userAccountControl (bit mask 1048576)
    AllowReversiblePasswordEncryption	Boolean	        RW	    userAccountControl (bit mask 128)
    BadLogonCount	                    Int32	        R	    badPwdCount
    CannotChangePassword	            Boolean	        RW	    nTSecurityDescriptor
    CanonicalName	                    String	        R	    canonicalName
    Certificates	                    ADCollection	RW	    userCertificate
    CN	                                String	        R	    cn
    Created	                            DateTime	    R	    whenCreated
    Deleted	                            Boolean	        R	    isDeleted
    Description	                        String	        RW	    description
    DisplayName	                        String	        RW	    displayName
    DistinguishedName	                String (DN)	    R	    distinguishedName
    DNSHostName	                        String	        RW	    dNSHostName
    DoesNotRequirePreAuth	            Boolean	        RW	    userAccountControl (bit mask 4194304)
    Enabled	                            Boolean	        RW	    userAccountControl (bit mask not 2)
    HomedirRequired	                    Boolean	        RW	    userAccountControl (bit mask 8)
    HomePage	                        String	        RW	    wWWHomePage
    IPv4Address	                        String	        R	    
    IPv6Address	                        String	        R	    
    LastBadPasswordAttempt	            DateTime	    R	    badPasswordTime, local time
    LastKnownParent	                    String (DN)	    R	    lastKnownParent
    LastLogonDate	                    DateTime	    R	    lastLogonTimeStamp, local time
    Location	                        String	        RW	    location
    LockedOut	                        Boolean	        RW	    msDS-User-Account-Control-Computed (bit mask 16)
    ManagedBy	                        String (DN)	    RW	    managedBy
    MemberOf	                        ADCollection	R	    memberOf
    MNSLogonAccount	                    Boolean	        RW	    userAccountControl (bit mask 131072)
    Modified	                        DateTime	    R	    whenChanged
    Name	                            String	        R	    cn (Relative Distinguished Name)
    ObjectCategory	                    String	        R	    objectCategory
    ObjectClass	                        String	        R	    objectClass, most specific value
    ObjectGUID	                        Guid	        R	    objectGUID converted to string
    OperatingSystem	                    String	        RW	    operatingSystem
    OperatingSystemHotfix	            String	        RW	    operatingSystemHotFix
    OperatingSystemServicePack	        String	        RW	    operatingSystemServicePack
    OperatingSystemVersion	            String	        RW	    operatingSystemVersion
    PasswordExpired	                    Boolean	        RW	    msDS-User-Account-Control-Computed (bit mask 8388608)
    PasswordLastSet	                    DateTime	    RW	    pwdLastSet, local time
    PasswordNeverExpires	            Boolean	        RW	    userAccountControl (bit mask 64)
    PasswordNotRequired	                Boolean	        RW	    userAccountControl (bit maks 32)
    PrimaryGroup	                    String	        R	    Group with primaryGroupToken
    ProtectedFromAccidentalDeletion	    Boolean	        RW	    nTSecurityDescriptor
    SamAccountName	                    String	        RW	    sAMAccountName
    ServiceAccount	                    ADCollection	RW	    msDS-HostServiceAccount
    ServicePrincipalNames	            ADCollection	RW	    servicePrincipalName
    SID	                                Sid	            R	    objectSID converted to string
    SIDHistory	                        ADCollection	R	    sIDHistory
    TrustedForDelegation	            Boolean	        RW	    userAccountControl (bit mask 524288)
    TrustedToAuthForDelegation	        Boolean	        RW	    userAccountControl (bit mask 16777216)
    UseDESKeyOnly	                    Boolean	        RW	    userAccountControl (bit mask 2097152)
    UserPrincipalName	                String	        RW	    userPrincipalName
    
    */

    public class ComputerEntry : IADEntry
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
        public List<string> ObjectClass { get; set; } = new List<string> { "Computer" };

        [AD("objectguid", true)]
        public Guid ObjectGUID { get; set; }

        [AD("objectsid", true)]
        public ADSID SID { get; set; }

        [AD("samaccountname")]
        public string SamAccountName { get; set; }

        [AD("accountexpires", ADAttribute.DateTypes.TimeStamp)]
        public DateTime? AccountExpirationDate { get; set; }

        [AD("badpwdcount", true)]
        public int BadLogonCount { get; set; }

        [AD("badpasswordtime", ADAttribute.DateTypes.TimeStamp, true)]
        public DateTime? LastBadPasswordAttempt { get; set; }

        [AD("lastlogon", ADAttribute.DateTypes.TimeStamp, true)]
        public DateTime? LastLogon { get; set; }

        [AD("location")]
        public string Location { get; set; }

        [AD("managedby")]
        public string ManagedBy { get; set; }

        [AD("memberof", true)]
        public List<string> MemberOf { get; set; }

        [AD("operatingsystem")]
        public string OperatingSystem { get; set; }

        [AD("operatingsystemhotfix")]
        public string OperatingSystemHotfix { get; set; }

        [AD("operatingsystemservicepack")]
        public string OperatingSystemServicePack { get; set; }

        [AD("operatingsystemversion")]
        public string OperatingSystemVersion { get; set; }

        [AD("pwdlastset", ADAttribute.DateTypes.TimeStamp)]
        public DateTime? PasswordLastSet { get; set; }

        [AD("serviceprincipalname")]
        public List<string> ServicePrincipalNames { get; set; }

        [AD("primarygroupid")]
        public int PrimaryGroupId { get; set; }

        [AD("useraccountcontrol")]
        public int UserAccountControl { get; set; }

        public UserAccountControls UserControl
        {
            get
            {
                return (UserAccountControls)UserAccountControl;
            }
            set
            {
                UserAccountControl = (int)value;
            }
        }

        [AD("msds-supportedencryptiontypes")]
        public int MsdsSupportedEncryptionTypes { get; set; }

        [AD("iscriticalsystemobject")]
        public bool IsCriticalSystemObject { get; set; }

        [AD("dnshostname")]
        public string DnsHostName { get; set; }

        [AD("samaccounttype")]
        public int SamAccountType { get; set; }

        [AD("countrycode")]
        public int CountryCode { get; set; }

        [AD("localpolicyflags")]
        public int LocalPolicyFlags { get; set; }

        [AD("logoncount")]
        public int LogonCount { get; set; }

        [AD("adspath")]
        public string AdsPath { get; set; }

        [AD("name")]
        public string Name { get; set; }

        [AD("lastlogoff")]
        public int LastLogoff { get; set; }

        [AD("instancetype")]
        public int InstanceType { get; set; }

        [AD("codepage")]
        public int Codepage { get; set; }

    }
}