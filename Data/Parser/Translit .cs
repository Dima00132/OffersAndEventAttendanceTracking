using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScannerAndDistributionOfQRCodes.Data.Parser
{
    public static  class Translit
    {
         private static readonly  Dictionary<string, string> _dictionaryChar = new()
            {
                {"Й","Q"},
                {"Ц","W"},
                {"У","E"},
                {"К","R"},
                {"Е","T"},
                {"Н","Y"},
                {"Г","U"},
                {"Ш","I"},
                {"Щ","O"},
                {"З","P"},
                {"Ф","A"},
                {"Ы","S"},
                {"В","D"},
                {"А","F"},
                {"П","G"},
                {"Р","H"},
                {"О","J"},
                {"Л","K"},
                {"Д","L"},
                {"Я","Z"},
                {"Ч","X"},
                {"С","C"},
                {"М","V"},
                {"И","B"},
                {"Т","N"},
                {"Ь","M"}
            };

        public static string TranslitColumnName(string source)
        {
            var upperValue = source?.ToUpper();
            return string.IsNullOrEmpty(upperValue) ? string.Empty 
                : source.Length > 1 ? TranslitLongColumnName(upperValue)
                : _dictionaryChar.TryGetValue(upperValue, out string value) ? value : upperValue;
        }

        private static string TranslitLongColumnName(string source)
        {
            var columnName =new StringBuilder();
            foreach (var item in source)
            {
                if (_dictionaryChar.TryGetValue(item.ToString(),out string value))
                {
                    columnName.Append(value);
                    continue;
                }
                columnName.Append(item);
            }
            return columnName.ToString();
        }
    }
}
