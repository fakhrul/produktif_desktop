using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;

namespace Produktif
{
    static class Extensions
    {
        public static string ToReadableString(TimeSpan span)
        {
            string formatted = string.Format("{0}{1}{2}{3}",
                span.Duration().Days > 0 ? string.Format("{0:0} d, ", span.Days) : string.Empty,
                span.Duration().Hours > 0 ? string.Format("{0:0} h, ", span.Hours) : string.Empty,
                span.Duration().Minutes > 0 ? string.Format("{0:0} m, ", span.Minutes) : string.Empty,
                span.Duration().Seconds > 0 ? string.Format("{0:0} s", span.Seconds) : string.Empty);

            if (formatted.EndsWith(", ")) formatted = formatted.Substring(0, formatted.Length - 2);

            if (string.IsNullOrEmpty(formatted)) formatted = "0 s";

            return formatted;
        }


        public static IList<T> Clone<T>(this IList<T> listToClone) where T : ICloneable
        {
            return listToClone.Select(item => (T)item.Clone()).ToList();
        }

        internal static string IsValidUrl(string urlString)
        {
            try
            {
                var url = urlString.Substring(urlString.LastIndexOf('.', urlString.LastIndexOf('.') - 1) + 1);
                var urls = url.Split('.');

                if (string.IsNullOrEmpty(urls[urls.Length - 1]))
                    return "";
            if (!urlString.StartsWith("https://") && !urlString.StartsWith("http://"))
                {
                    urlString = "https://" + urlString;
                }
                if (!isValidURL(urlString))
                    return "";
                return urlString;
            }
            catch (Exception ex)
            {
            }
            return "";
        }

        static bool isValidURL(string url)
        {

            string pattern = @"^(ht|f)tp(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&amp;%\$#_]*)?$";

            // Regex to check valid URL
            Regex r = new Regex(pattern);

            if (r.IsMatch(url))
                return true;
            return false;

        }

    }
}
