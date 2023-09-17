namespace SysAdmin.ActiveDirectory.Models
{
    public class ContactEntry : IADEntry
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
        public List<string> ObjectClass { get; set; } = new List<string> { "Contact" };

        [AD("objectguid", true)]
        public Guid ObjectGUID { get; set; }

        [AD("objectsid", true)]
        public ADSID SID { get; set; }

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

        [AD("wwwhomepage")]
        public string HomePage { get; set; }

        [AD("homephone")]
        public string HomePhone { get; set; }

        [AD("initials")]
        public string Initials { get; set; }

        //[ADAttribute("manager")]
        //public string Manager { get; set; }

        //[ADAttribute("memberof", true)]
        //public List<String> MemberOf { get; set; }

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

        [AD("postofficebox")]
        public string POBox { get; set; }

        [AD("postalcode")]
        public string PostalCode { get; set; }

        [AD("st")]
        public string State { get; set; }

        [AD("streetaddress")]
        public string StreetAddress { get; set; }

        [AD("sn")]
        public string LastName { get; set; }

        [AD("title")]
        public string Title { get; set; }

        [AD("countrycode")]
        public int CountryCode { get; set; }

        [AD("adspath")]
        public string AdsPath { get; set; }

        [AD("name")]
        public string Name { get; set; }

        [AD("instancetype")]
        public int InstanceType { get; set; }
    }
}