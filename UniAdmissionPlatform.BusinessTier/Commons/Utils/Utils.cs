using System.Text.RegularExpressions;

namespace UniAdmissionPlatform.BusinessTier.Commons.Utils
{
    public static class Utils
    {
        public static string ToSnakeCase(this string o) =>
            Regex.Replace(o, @"(\w)([A-Z])", "$1-$2").ToLower();
    }
}