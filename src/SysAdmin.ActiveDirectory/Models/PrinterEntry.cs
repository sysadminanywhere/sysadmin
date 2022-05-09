namespace SysAdmin.ActiveDirectory.Models
{

    /*

    http://social.technet.microsoft.com/wiki/contents/articles/12103.active-directory-get-adobject-default-and-extended-properties.aspx
    
    Property	                        Syntax	        R/RW	lDAPDisplayName
    
    CanonicalName	                    String	        R	    canonicalName
    CN	                                String	        R	    cn
    Created	                            DateTime	    R	    whenCreated
    Deleted	                            Boolean	        R	    isDeleted
    Description	                        String	        RW	    description
    DisplayName	                        String	        RW	    displayName
    DistinguishedName	                String (DN)	    R	    distinguishedName
    LastKnownParent	                    String (DN)	    R	    lastKnownParent
    Modified	                        DateTime	    R	    modifyTimeStamp
    Name	                            String	        R	    Name (Relative Distinguished Name)
    ObjectCategory	                    String	        R	    objectCategory
    ObjectClass	                        String	        R	    objectClass, most specific value
    ObjectGUID	                        Guid	        R	    objectGUID converted to string
    ProtectedFromAccidentalDeletion	    Boolean	        RW	    nTSecurityDescriptor
    
    */

    public class PrinterEntry : IADEntry
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
        public List<string> ObjectClass { get; set; } = new List<string> { "printQueue" };

        [AD("objectguid", true)]
        public Guid ObjectGUID { get; set; }

        [AD("objectsid", true)]
        public ADSID SID { get; set; }

        [AD("samaccountname")]
        public string SamAccountName { get; set; }

        [AD("adspath")]
        public string AdsPath { get; set; }

        [AD("name")]
        public string Name { get; set; }

        [AD("instancetype")]
        public int InstanceType { get; set; }

        [AD("printspooling")]
        public string PrintSpooling { get; set; }

        [AD("url")]
        public string Url { get; set; }

        [AD("shortservername")]
        public string ShortServerName { get; set; }

        [AD("drivername")]
        public string DriverName { get; set; }

        [AD("printlanguage")]
        public string PrintLanguage { get; set; }

        [AD("servername")]
        public string ServerName { get; set; }

        [AD("printorientationssupported")]
        public List<string> PrintOrientationsSupported { get; set; }

        [AD("printrateunit")]
        public string PrintRateUnit { get; set; }

        [AD("printmediaready")]
        public string PrintMediaReady { get; set; }

        [AD("printmediasupported")]
        public List<string> PrintMediaSupported { get; set; }

        [AD("printername")]
        public string PrinterName { get; set; }

        [AD("printbinnames")]
        public List<string> PrintBinNames { get; set; }

        [AD("uncname")]
        public string UncName { get; set; }

        [AD("printmaxyextent")]
        public int PrintMaxyExtent { get; set; }

        [AD("printkeepprintedjobs")]
        public bool PrintKeepPrintedJobs { get; set; }

        [AD("printminyextent")]
        public int PrintMinyExtent { get; set; }

        [AD("printstaplingsupported")]
        public bool PrintStaplingSupported { get; set; }

        [AD("printnumberup")]
        public int PrintNumberUp { get; set; }

        [AD("driverversion")]
        public int DriverVersion { get; set; }

        [AD("printmemory")]
        public int PrintMemory { get; set; }

        [AD("printmaxxextent")]
        public int PrintMaxxExtent { get; set; }

        [AD("printcollate")]
        public bool PrintCollate { get; set; }

        [AD("versionnumber")]
        public int VersionNumber { get; set; }

        [AD("printrate")]
        public int PrintRate { get; set; }

        [AD("portname")]
        public string PortName { get; set; }

        [AD("printsharename")]
        public string PrintShareName { get; set; }

        [AD("printpagesperminute")]
        public int PrintPagesPerMinute { get; set; }

        [AD("printminxextent")]
        public int PrintMinxExtent { get; set; }

        [AD("flags")]
        public int Flags { get; set; }

        [AD("printmaxresolutionsupported")]
        public int PrintMaxResolutionSupported { get; set; }

        [AD("priority")]
        public int Priority { get; set; }

        [AD("rintduplexsupported")]
        public bool PrintDuplexSupported { get; set; }

        [AD("printcolor")]
        public bool PrintColor { get; set; }

        [AD("printendtime", ADAttribute.DateTypes.TimeStamp)]
        public DateTime? PrintendTime { get; set; }

        [AD("printstarttime", ADAttribute.DateTypes.TimeStamp)]
        public DateTime? PrintStartTime { get; set; }

    }
}