using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Infrastructure.Helpers
{
    internal static class NameHelpers
    {
        private static readonly Dictionary<string, string> CharacterReplacements = new()
        {
            {"\"", ""}, {"!", ""}, {"'", ""}, {"^", ""}, {"+", ""}, {"%", ""},
            {"&", ""}, {"/", ""}, {"(", ""}, {")", ""}, {"=", ""}, {"?", ""},
            {"_", ""}, {" ", "-"}, {"@", ""}, {"€", ""}, {"¨", ""}, {"~", ""},
            {",", ""}, {";", ""}, {":", ""}, {".", "-"}, {"Ö", "o"}, {"ö", "o"},
            {"Ü", "u"}, {"ü", "u"}, {"ı", "i"}, {"İ", "i"}, {"ğ", "g"}, {"Ğ", "g"},
            {"æ", ""}, {"ß", ""}, {"â", "a"}, {"î", "i"}, {"ş", "s"}, {"Ş", "s"},
            {"Ç", "c"}, {"ç", "c"}, {"<", ""}, {">", ""}, {"|", ""}
        };

        public static string CharacterRegulatory(string name)
        {
            foreach (var replacement in CharacterReplacements)
            {
                name = name.Replace(replacement.Key, replacement.Value);
            }
            return name;
        }
    }
}
