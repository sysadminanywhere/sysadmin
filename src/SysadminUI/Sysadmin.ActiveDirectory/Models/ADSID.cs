using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysAdmin.ActiveDirectory.Models
{
    public class ADSID
    {

        public byte[] Value { get; private set; }

        public ADSID(byte[] value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return ConvertByteToStringSid(Value);
        }

        private static string ConvertByteToStringSid(byte[] sidBytes)
        {
            short sSubAuthorityCount = 0;
            StringBuilder strSid = new StringBuilder();
            strSid.Append("S-");

            try
            {
                // Add SID revision.
                strSid.Append(sidBytes[0].ToString());

                sSubAuthorityCount = Convert.ToInt16(sidBytes[1]);

                // Next six bytes are SID authority value.
                if (sidBytes[2] != 0 || sidBytes[3] != 0)
                {
                    string strAuth = string.Format("0x{0:2x}{1:2x}{2:2x}{3:2x}{4:2x}{5:2x}",
                                   (short)sidBytes[2],
                                   (short)sidBytes[3],
                                   (short)sidBytes[4],
                                   (short)sidBytes[5],
                                   (short)sidBytes[6],
                                   (short)sidBytes[7]);
                    strSid.Append("-");
                    strSid.Append(strAuth);
                }
                else
                {
                    long iVal = sidBytes[7] +
                         (sidBytes[6] << 8) +
                         (sidBytes[5] << 16) +
                         (sidBytes[4] << 24);
                    strSid.Append("-");
                    strSid.Append(iVal.ToString());
                }

                // Get sub authority count...
                int idxAuth = 0;
                for (int i = 0; i < sSubAuthorityCount; i++)
                {
                    idxAuth = 8 + i * 4;
                    uint iSubAuth = BitConverter.ToUInt32(sidBytes, idxAuth);
                    strSid.Append("-");
                    strSid.Append(iSubAuth.ToString());
                }
            }
            catch (Exception ex)
            {

            }

            return strSid.ToString();
        }

    }
}
