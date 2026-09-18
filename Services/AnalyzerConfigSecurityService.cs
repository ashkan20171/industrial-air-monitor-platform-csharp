using System;
using System.Security.Cryptography;
using System.Text;

namespace AshkanAQMS.Services
{
    public static class AnalyzerConfigSecurityService
    {
        private static readonly byte[] Entropy = Encoding.UTF8.GetBytes("AshkanAQMS.AnalyzerConfig.v1");

        public static string Protect(string value)
        {
            if (string.IsNullOrEmpty(value)) return string.Empty;
            byte[] data = Encoding.UTF8.GetBytes(value);
            byte[] protectedData = ProtectedData.Protect(data, Entropy, DataProtectionScope.CurrentUser);
            return Convert.ToBase64String(protectedData);
        }

        public static string Unprotect(string value)
        {
            if (string.IsNullOrEmpty(value)) return string.Empty;
            try
            {
                byte[] protectedData = Convert.FromBase64String(value);
                byte[] data = ProtectedData.Unprotect(protectedData, Entropy, DataProtectionScope.CurrentUser);
                return Encoding.UTF8.GetString(data);
            }
            catch (CryptographicException) { return string.Empty; }
            catch (FormatException) { return string.Empty; }
        }
    }
}
