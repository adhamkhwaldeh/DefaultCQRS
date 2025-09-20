using System;
using System.Collections.Generic;
using System.Text;

namespace AlJawad.DefaultCQRS.Extensions
{
    public static class EnumerableExtensions
    {

  

        public static bool HasValue(this string[] input)
        {
            return input != null && input.Length > 0;
        }

        public static bool nullOrEmpty<T>(this IEnumerable<T>? values)
        {
            return values == null || !values.Any();
        }
        public static string ToDelimitedString<T>(this IEnumerable<T> values)
        {
            return ToDelimitedString<T>(values, ",");
        }

        public static string ToDelimitedString<T>(this IEnumerable<T> values, string delimiter)
        {
            var sb = new StringBuilder();
            foreach (var i in values)
            {
                if (sb.Length > 0)
                    sb.Append(delimiter ?? ",");
                sb.Append(i.ToString());
            }

            return sb.ToString();
        }

        public static string ToDelimitedString(this IEnumerable<string> values)
        {
            return ToDelimitedString(values, ",");
        }


        public static string ToDelimitedString(this IEnumerable<string> values, string delimiter)
        {
            return ToDelimitedString(values, delimiter);
        }

        public static IEnumerable<T> Append<T>(this IEnumerable<T> source, params T[] tail)
        {
            return source.Concat(tail);
        }
        public static IEnumerable<T> Suffix<T>(this IEnumerable<T> source, params T[] head)
        {
            return head.Concat(source);
        }
    }
}
