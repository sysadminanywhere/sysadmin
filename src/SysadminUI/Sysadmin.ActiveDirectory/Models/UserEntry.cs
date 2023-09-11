using SysAdmin.ActiveDirectory;

namespace SysAdmin.ActiveDirectory.Models
{
    /*

http://social.technet.microsoft.com/wiki/contents/articles/12037.active-directory-get-aduser-default-and-extended-properties.aspx

Property	                        Syntax	        R/RW	lDAPDisplayName

AccountExpirationDate	            DateTime	    RW	    accountExpires, converted to local time
AccountLockoutTime	                DateTime	    RW	    lockoutTime, converted to local time
AccountNotDelegated	                Boolean	        RW	    userAccountControl (bit mask 1048576)
AllowReversiblePasswordEncryption	Boolean	        RW	    userAccountControl (bit mask 128)
BadLogonCount	                    Int32	        R	    badPwdCount
CannotChangePassword	            Boolean	        RW	    nTSecurityDescriptor
CanonicalName	                    String	        R	    canonicalName
Certificates	                    ADCollection	RW	    userCertificate
ChangePasswordAtLogon	            Boolean	        W	    If pwdLastSet = 0
City	                            String	        RW	    l
CN	                                String	        R	    cn
Company	                            String	        RW	    company
Country	                            String	        RW	    c (2 character abbreviation)
Created	                            DateTime	    R	    whenCreated
Deleted	                            Boolean	        R	    isDeleted
Department	                        String	        RW	    department
Description	                        String	        RW	    description
DisplayName	                        String	        RW	    displayName
DistinguishedName	                String (DN)	    R	    distinguishedName
Division	                        String	        RW	    division
DoesNotRequirePreAuth	            Boolean	        RW	    userAccountControl (bit mask 4194304)
EmailAddress	                    String	        RW	    mail
EmployeeID	                        String	        RW	    employeeID
EmployeeNumber	                    String	        RW	    employeeNumber
Enabled	                            Boolean	        RW	    userAccountControl (bit mask not 2)
Fax	                                String	        RW	    facsimileTelephoneNumber
GivenName	                        String	        RW	    givenName
HomeDirectory	                    String	        RW	    homeDirectory
HomedirRequired	                    Boolean	        RW	    userAccountControl (bit mask 8)
HomeDrive	                        String	        RW	    homeDrive
HomePage	                        String	        RW	    wWWHomePage
HomePhone	                        String	        RW	    homePhone
Initials	                        String	        RW	    initials
LastBadPasswordAttempt	            DateTime	    R	    badPasswordTime, converted to local time
LastKnownParent	                    String (DN)	    R	    lastKnownParent
LastLogonDate	                    DateTime	    R	    lastLogonTimeStamp, converted to local time
LockedOut	                        Boolean	        RW	    msDS-User-Account-Control-Computed (bit mask 16)
LogonWorkstations	                String	        RW	    userWorkstations
Manager	                            String (DN)	    RW	    manager
MemberOf	                        ADCollection	R	    memberOf
MNSLogonAccount	                    Boolean	        RW	    userAccountControl (bit mask 131072)
MobilePhone	                        String	        RW	    mobile
Modified	                        DateTime	    R	    whenChanged
Name	                            String	        R	    cn (Relative Distinguished Name)
ObjectCategory	                    String	        R	    objectCategory
ObjectClass	                        String	        R	    objectClass, most specific value
ObjectGUID	                        Guid	        R	    objectGUID converted to string
Office	                            String	        RW	    physicalDeliveryOfficeName
OfficePhone	                        String	        RW	    telephoneNumber
Organization	                    String	        RW	    o
OtherName	                        String	        RW	    middleName
PasswordExpired	                    Boolean	        RW	    msDS-User-Account-Control-Computed (bit mask 8388608)
PasswordLastSet	                    DateTime	    RW	    pwdLastSet, local time
PasswordNeverExpires	            Boolean	        RW	    userAccountControl (bit mask 64)
PasswordNotRequired	                Boolean	        RW	    userAccountControl (bit mask 32)
POBox	                            String	        RW	    postOfficeBox
PostalCode	                        String	        RW	    postalCode
PrimaryGroup	                    String	        R	    Group with primaryGroupToken
ProfilePath	                        String	        RW	    profilePath
ProtectedFromAccidentalDeletion	    Boolean	        RW	    nTSecurityDescriptor
SamAccountName	                    String	        RW	    sAMAccountName
ScriptPath	                        String	        RW	    scriptPath
ServicePrincipalNames	            ADCollection	RW	    servicePrincipalName
SID	                                Sid	            R	    objectSID converted to string
SIDHistory	                        ADCollection	R	    sIDHistory
SmartcardLogonRequired	            Boolean	        RW	    userAccountControl (bit mask 262144)
State	                            String	        RW	    st
StreetAddress	                    String	        RW	    streetAddress
Surname	                            String	        RW	    sn
Title	                            String	        RW	    title
TrustedForDelegation	            Boolean	        RW	    userAccountControl (bit mask 524288)
TrustedToAuthForDelegation	        Boolean	        RW	    userAccountControl (bit mask 16777216)
UseDESKeyOnly	                    Boolean	        RW	    userAccountControl (bit mask 2097152)
UserPrincipalName	                String	        RW	    userPrincipalName

*/

    public class UserEntry : IADEntry
    {
        [AD("cn", true)]
        public string CN { get; set; }

        [AD("whencreated", ADAttribute.DateTypes.Date, true)]
        public DateTime? Created { get; set; }

        [AD("description")]
        public string Description { get; set; }

        [AD("distinguishedname", true)]
        public string DistinguishedName { get; set; }

        [AD("displayname", true)]
        public string DisplayName { get; set; }

        [AD("whenchanged", ADAttribute.DateTypes.Date, true)]
        public DateTime? Modified { get; set; }

        [AD("objectcategory", true)]
        public string ObjectCategory { get; set; }

        [AD("objectclass", true)]
        public List<string> ObjectClass { get; set; } = new List<string> { "User" };

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

        [AD("lockouttime", ADAttribute.DateTypes.TimeStamp)]
        public DateTime? AccountLockoutTime { get; set; }

        //[ADAttribute("usercertificate")]
        //public List<String> Certificates { get; set; }

        [AD("l")]
        public string City { get; set; }

        [AD("company")]
        public string Company { get; set; }

        [AD("c")]
        public string Country { get; set; }

        [AD("department")]
        public string Department { get; set; }

        [AD("division")]
        public string Division { get; set; }

        [AD("mail")]
        public string EmailAddress { get; set; }

        [AD("employeeid")]
        public string EmployeeID { get; set; }

        [AD("employeenumber")]
        public string EmployeeNumber { get; set; }

        [AD("facsimiletelephonenumber")]
        public string Fax { get; set; }

        [AD("givenName")]
        public string FirstName { get; set; }

        [AD("homedirectory")]
        public string HomeDirectory { get; set; }

        [AD("homedrive")]
        public string HomeDrive { get; set; }

        [AD("wwwhomepage")]
        public string HomePage { get; set; }

        [AD("homephone")]
        public string HomePhone { get; set; }

        [AD("initials")]
        public string Initials { get; set; }

        [AD("userworkstations")]
        public string LogonWorkstations { get; set; }

        [AD("manager")]
        public string Manager { get; set; }

        [AD("memberof", true)]
        public List<string> MemberOf { get; set; }

        [AD("mobile")]
        public string MobilePhone { get; set; }

        [AD("physicaldeliveryofficename")]
        public string Office { get; set; }

        [AD("telephonenumber")]
        public string OfficePhone { get; set; }

        [AD("o")]
        public string Organization { get; set; }

        [AD("middlename")]
        public string OtherName { get; set; }

        [AD("pwdlastset", ADAttribute.DateTypes.TimeStamp)]
        public DateTime? PasswordLastSet { get; set; }

        [AD("postofficebox")]
        public string POBox { get; set; }

        [AD("postalcode")]
        public string PostalCode { get; set; }

        [AD("profilepath")]
        public string ProfilePath { get; set; }

        [AD("scriptpath")]
        public string ScriptPath { get; set; }

        [AD("st")]
        public string State { get; set; }

        [AD("streetaddress")]
        public string StreetAddress { get; set; }

        [AD("sn")]
        public string LastName { get; set; }

        [AD("title")]
        public string Title { get; set; }

        [AD("userprincipalname")]
        public string UserPrincipalName { get; set; }

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

        [AD("iscriticalsystemobject")]
        public bool IsCriticalSystemObject { get; set; }

        [AD("samaccounttype")]
        public int SamAccountType { get; set; }

        [AD("countrycode")]
        public int CountryCode { get; set; }

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

        [AD("admincount")]
        public int AdminCount { get; set; }

        [AD("managedobjects")]
        public List<string> ManagedObjects { get; set; }

        [AD("serviceprincipalname")]
        public string ServicePrincipalName { get; set; }

        [AD("logonhours")]
        public byte[] LogonHours { get; set; }

        [AD("jpegphoto")]
        public byte[] JpegPhoto { get; set; }

    }

}