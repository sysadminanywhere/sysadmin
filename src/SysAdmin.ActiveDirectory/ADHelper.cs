using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysAdmin.ActiveDirectory
{
    public static class ADHelper
    {
        public static string ExtractCN(string dn)
        {
            string[] parts = dn.Split(new char[] { ',' });

            for (int i = 0; i < parts.Length; i++)
            {
                var p = parts[i];
                var elems = p.Split(new char[] { '=' });
                var t = elems[0].Trim().ToUpper();
                var v = elems[1].Trim();
                if (t == "CN")
                {
                    return v;
                }
            }
            return dn;
        }

        public static string GetPrimaryGroup(int id)
        {
            /*
                512   Domain Admins
                513   Domain Users
                514   Domain Guests
                515   Domain Computers
                516   Domain Controllers
            */

            switch (id)
            {
                case 512:
                    return "Domain Admins";

                case 513:
                    return "Domain Users";

                case 514:
                    return "Domain Guests";

                case 515:
                    return "Domain Computers";

                case 516:
                    return "Domain Controllers";

                default:
                    return string.Empty;
            }
        }

        public static string GetGroupType(long groupType)
        {
            switch (groupType)
            {
                case 2:
                    return "Global distribution group";

                case 4:
                    return "Domain local distribution group";

                case 8:
                    return "Universal distribution group";

                case -2147483646:
                    return "Global security group";

                case -2147483644:
                    return "Domain local security group";

                case -2147483640:
                    return "Universal security group";

                case -2147483643:
                    return "BuiltIn Group";

                default:
                    return string.Empty;
            }
        }

    }
}