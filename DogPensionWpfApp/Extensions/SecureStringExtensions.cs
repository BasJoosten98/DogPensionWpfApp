using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace DogPensionWpfApp.Extensions
{
    public static class SecureStringExtensions
    {
        public static String ToUnsecureString(this SecureString value)
        {
            IntPtr valuePtr = IntPtr.Zero;
            try
            {
                valuePtr = Marshal.SecureStringToGlobalAllocUnicode(value);
                return Marshal.PtrToStringUni(valuePtr);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(valuePtr);
            }
        }
        public static SecureString ToSecureString(this String value)
        {
            SecureString result = new SecureString();
            foreach(char c in value) { result.AppendChar(c); }
            return result;
        }
    }
}
