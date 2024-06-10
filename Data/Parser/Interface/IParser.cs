//using Bytescout.Spreadsheet;

namespace ScannerAndDistributionOfQRCodes.Data.Parser.Interface
{
    public interface IParser
    {
        public List<Dictionary<string, string>> Pars(string filePath);

        public List<Dictionary<string, string>> Pars(Stream stream);
    }

}

