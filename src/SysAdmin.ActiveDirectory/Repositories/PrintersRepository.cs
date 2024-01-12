using LdapForNet;
using SysAdmin.ActiveDirectory.Models;
using SysAdmin.ActiveDirectory.Services.Ldap;

namespace SysAdmin.ActiveDirectory.Repositories
{
    public class PrintersRepository : IDisposable
    {

        private LdapService ldapService;

        public PrintersRepository(LdapService ldapService)
        {
            if (ldapService == null)
                throw new ArgumentNullException(nameof(ldapService));

            this.ldapService = ldapService;
        }

        public async Task<List<PrinterEntry>> ListAsync()
        {
            List<PrinterEntry> printers = new List<PrinterEntry>();

            List<LdapEntry> list = await ldapService.SearchAsync("(objectClass=printQueue)");

            foreach (LdapEntry entry in list)
            {
                printers.Add(ADResolver<PrinterEntry>.GetValues(entry));
            }

            return printers;
        }

        public async Task<PrinterEntry?> GetByCNAsync(string cn)
        {
            if (string.IsNullOrEmpty(cn))
                throw new ArgumentNullException(nameof(cn));

            var result = await ldapService.SearchAsync("(&(objectClass=printQueue)(cn=" + cn + "))");
            var entry = result.FirstOrDefault();

            if (entry != null)
                return ADResolver<PrinterEntry>.GetValues(entry);
            else
                return null;
        }

        //public void Add(PrinterEntry printer)
        //{
        //    Add(string.Empty, printer);
        //}

        //public void Add(string distinguishedName, PrinterEntry printer)
        //{
        //    if (string.IsNullOrEmpty(printer.CN))
        //    {
        //        throw new ArgumentException("Property CN is empty!");
        //    }

        //    List<string> attributes = new List<string>
        //    {
        //        "description"
        //    };

        //    if (string.IsNullOrEmpty(distinguishedName))
        //    {
        //        var item = ldapService.WellKnownObjects().Where(c => c.StartsWith(ADContainers.ContainerUsers)).First();
        //        string cn = "cn=" + printer.CN + "," + item.Replace(ADContainers.ContainerUsers, string.Empty);
        //        ldapService.Add(LdapResolver.GetLdapEntry(cn, printer, attributes));
        //    }
        //    else
        //    {
        //        string cn = "cn=" + printer.CN + "," + distinguishedName;
        //        ldapService.Add(LdapResolver.GetLdapEntry(cn, printer, attributes));
        //    }

        //    var entry = ldapService.Search("(&(objectClass=printQueue)(cn=" + printer.CN + "))").FirstOrDefault();

        //    if (entry != null)
        //    {
        //    }
        //}

        //public void Modify(PrinterEntry printer)
        //{
        //    List<string> attributes = new List<string>
        //    {
        //        "description"
        //    };

        //    var entry = ldapService.Search("(&(objectClass=printQueue)(cn=" + printer.CN + "))").FirstOrDefault();

        //    if (entry != null)
        //    {
        //        PrinterEntry oldPrinter = ADResolver<PrinterEntry>.GetValues(entry);
        //        var response = ldapService.SendRequest(new ModifyRequest(printer.DistinguishedName, LdapResolver.GetDirectoryModificationAttributes(printer, oldPrinter, attributes).ToArray()));
        //    }

        //}

        public async Task DeleteAsync(PrinterEntry printer)
        {
            if (printer == null)
                throw new ArgumentNullException(nameof(printer));

            if (string.IsNullOrEmpty(printer.DistinguishedName))
                throw new ArgumentNullException(nameof(printer.DistinguishedName));

            await ldapService.DeleteAsync(printer.DistinguishedName);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            ldapService?.Dispose();
        }

    }
}