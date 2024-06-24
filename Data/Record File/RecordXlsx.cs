using ScannerAndDistributionOfQRCodes.Data.Parser.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aspose.Cells;
using System.Text.RegularExpressions;
using System.IO;

namespace ScannerAndDistributionOfQRCodes.Data.Record_File
{
    public sealed class RecordXlsx
    {
        public  void Record(List<string[]> strings , Stream stream)
        {
            using Workbook workbook = new Workbook(stream);
            RecordXlsxFile(strings, workbook);
            workbook.Save(stream, SaveFormat.Xlsx);
        }
        public void Record(List<string[]> strings, string filePath)
        {
            using Workbook workbook = new Workbook(filePath);
            RecordXlsxFile(strings, workbook);
            workbook.Save(filePath, SaveFormat.Xlsx);
        }

        private void RecordXlsxFile(List<string[]> strings, Workbook workbook )
        {
            for (int i = 0; i < strings.Count; i++)
            {
                Worksheet sheet = workbook.Worksheets[i];
                for (int y = 0; y < strings[i].Length; y++)
                {
                    Aspose.Cells.Cell cell = sheet.Cells[i, y];
                    cell.PutValue(strings[i][y]);
                }
            }
        }

     
    }
}
