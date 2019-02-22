namespace QGate.Eaf.Domain.Globalization
{
    public static class CultureCodes
    {
        public static readonly string cs = LanguageCodes.cs;
        public static readonly string csCz = GetCultureCode(cs, "CZ");
        public static readonly string en = LanguageCodes.en;
        public static readonly string enUS = GetCultureCode(cs, "US");
        public static readonly string enGG = GetCultureCode(cs, "GB");


        private static string GetCultureCode(string languageCode, string countryCode)
        {
            return string.Concat(languageCode, "-", countryCode);
        }
    }
}
