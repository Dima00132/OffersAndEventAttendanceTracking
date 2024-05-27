using System.Text;

namespace ScannerAndDistributionOfQRCodes.Model
{
    public static class GeneratorUniqueQRHashCode
    {
        public static string Generate(params string[] strings)
        {
            StringBuilder builder = new StringBuilder();
            unchecked
            {
                int hash = 0;
                foreach (var item in strings)
                {
                    var itemHash = item.GetHashCode();
                    //hash += itemHash;
                    //var newHash = hash + itemHash;
                    builder.Append(itemHash);
                }
                builder.Append(hash);
            }
            return builder.ToString();
        }
    }
}
