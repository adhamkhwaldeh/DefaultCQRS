using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace AlJawad.DefaultCQRS.Extensions
{
    public static class StringExtentions
    {

        public static string Truncate(this string text, int keep, string ellipsis = "...")
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;

            if (string.IsNullOrEmpty(ellipsis))
                ellipsis = string.Empty;

            string buffer = NormalizeLineEndings(text);
            if (buffer.Length <= keep)
                return buffer;

            if (buffer.Length <= keep + ellipsis.Length || keep < ellipsis.Length)
                return buffer.Substring(0, keep);

            return string.Concat(buffer.Substring(0, keep - ellipsis.Length), ellipsis);
        }
        public static bool NullOrEmpty(this string input)
        {

            return input == null || input.IsEmpty();
        }

        public static string CalculateHash(this string input)
        {
            using var algorithm = SHA512.Create(); //or MD5 SHA256 etc.
            var hashedBytes = algorithm.ComputeHash(Encoding.UTF8.GetBytes(input));
            return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
        }
        public static bool IsMixedCase(this string s)
        {
            if (s.IsEmpty())
                return false;

            var containsUpper = s.Any(char.IsUpper);
            var containsLower = s.Any(char.IsLower);

            return containsLower && containsUpper;
        }
        public static string NormalizeLineEndings(this string text)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;

            return text
                .Replace("\r\n", "\n")
                .Replace("\n", Environment.NewLine);
        }

        public static string RemoveInvisible(this string s)
        {
            if (string.IsNullOrEmpty(s))
                return s;

            return s
                .Replace("\r\n", " ")
                .Replace('\n', ' ')
                .Replace('\t', ' ');
        }

        public static bool IsEmail(string email)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email)) return false;
                return new System.Net.Mail.MailAddress(email) != null;
            }
            catch
            {
                return false;
            }
        }
        public static bool HasValue(this string me) => !me.IsEmpty();

        public static bool IsEmpty(this string me)
        {
            return string.IsNullOrWhiteSpace(me);
        }
        public static string ApplyFormat(this string input, params string[] format)
        {
            if (format is null || format.Length == 0)
            {
                format = new string[] { " " };
            }
            return string.Format(input, format).Trim();
        }
    }
}
