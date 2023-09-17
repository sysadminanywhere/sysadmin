using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LdapForNet.Native.Native;

namespace SysAdmin.ActiveDirectory
{
    public static class LdapResult
    {

        public static string GetErrorMessageFromResult(ResultCode? resultCode)
        {
            string result = string.Empty;

            switch (resultCode)
            {
                case ResultCode.LDAP_NOT_SUPPORTED:
                    result = "LDAP not supported";
                    break;

                case ResultCode.LDAP_PARAM_ERROR:
                    result = "LDAP param erroe";
                    break;

                case ResultCode.Success:
                    result = "Success";
                    break;

                case ResultCode.OperationsError:
                    result = "Operations error";
                    break;

                case ResultCode.ProtocolError:
                    result = "Protocol error";
                    break;

                case ResultCode.TimeLimitExceeded:
                    result = "Time limit exceeded";
                    break;

                case ResultCode.SizeLimitExceeded:
                    result = "Size limit exceeded";
                    break;

                case ResultCode.CompareFalse:
                    result = "Compare false";
                    break;

                case ResultCode.CompareTrue:
                    result = "Compare true";
                    break;

                case ResultCode.AuthMethodNotSupported:
                    result = "Auth method not supported";
                    break;

                case ResultCode.StrongAuthRequired:
                    result = "Strong auth required";
                    break;

                case ResultCode.ReferralV2:
                    result = "Referral V2";
                    break;

                case ResultCode.Referral:
                    result = "Referral";
                    break;

                case ResultCode.AdminLimitExceeded:
                    result = "Admin limit exceeded";
                    break;

                case ResultCode.UnavailableCriticalExtension:
                    result = "Unavailable critical extension";
                    break;

                case ResultCode.ConfidentialityRequired:
                    result = "Confidentiality required";
                    break;

                case ResultCode.SaslBindInProgress:
                    result = "Sasl bind in progress";
                    break;

                case ResultCode.NoSuchAttribute:
                    result = "No such attribute";
                    break;

                case ResultCode.UndefinedAttributeType:
                    result = "Undefined attribute type";
                    break;

                case ResultCode.InappropriateMatching:
                    result = "Inappropriate matching";
                    break;

                case ResultCode.ConstraintViolation:
                    result = "Constraint violation";
                    break;

                case ResultCode.AttributeOrValueExists:
                    result = "Attribute or value exists";
                    break;

                case ResultCode.InvalidAttributeSyntax:
                    result = "Invalid attribute syntax";
                    break;

                case ResultCode.NoSuchObject:
                    result = "No such object";
                    break;

                case ResultCode.AliasProblem:
                    result = "Alias problem";
                    break;

                case ResultCode.InvalidDNSyntax:
                    result = "Invalid DN syntax";
                    break;

                case ResultCode.AliasDereferencingProblem:
                    result = "Alias dereferencing problem";
                    break;

                case ResultCode.InappropriateAuthentication:
                    result = "Inappropriate authentication";
                    break;

                case ResultCode.InvalidCredentials:
                    result = "Invalid credentials";
                    break;

                case ResultCode.InsufficientAccessRights:
                    result = "Insufficient access rights";
                    break;

                case ResultCode.Busy:
                    result = "Busy";
                    break;

                case ResultCode.Unavailable:
                    result = "Unavailable";
                    break;

                case ResultCode.UnwillingToPerform:
                    result = "Unwilling to perform";
                    break;

                case ResultCode.LoopDetect:
                    result = "Loop detect";
                    break;

                case ResultCode.SortControlMissing:
                    result = "Sort control missing";
                    break;

                case ResultCode.OffsetRangeError:
                    result = "Offset range error";
                    break;

                case ResultCode.NamingViolation:
                    result = "Naming violation";
                    break;

                case ResultCode.ObjectClassViolation:
                    result = "Object class violation";
                    break;

                case ResultCode.NotAllowedOnNonLeaf:
                    result = "Not allowed on non leaf";
                    break;

                case ResultCode.NotAllowedOnRdn:
                    result = "Not allowed on Rdn";
                    break;

                case ResultCode.EntryAlreadyExists:
                    result = "Entry already exists";
                    break;

                case ResultCode.ObjectClassModificationsProhibited:
                    result = "Object class modifications prohibited";
                    break;

                case ResultCode.ResultsTooLarge:
                    result = "Results too large";
                    break;

                case ResultCode.AffectsMultipleDsas:
                    result = "Affects multiple Dsas";
                    break;

                case ResultCode.VirtualListViewError:
                    result = "Virtual list view error";
                    break;

                case ResultCode.Other:
                    result = "Other";
                    break;
            }

            return result;
        }

    }
}