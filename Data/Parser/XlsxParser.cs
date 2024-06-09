//using Bytescout.Spreadsheet;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Vml.Spreadsheet;
using ScannerAndDistributionOfQRCodes.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Cell = DocumentFormat.OpenXml.Spreadsheet.Cell;


namespace ScannerAndDistributionOfQRCodes.Data.Parser
{
    public interface IParser
    {
        public List<Dictionary<string, string>> Pars(string filePath);

        public List<Dictionary<string, string>> Pars(Stream stream);
    }
    public class XlsxParser : IParser
    {


        private readonly List<Dictionary<string, string>> RowsXlsxDate = new();

        public List<Dictionary<string, string>> Pars(Stream stream)
        {
            using SpreadsheetDocument document = SpreadsheetDocument.Open(stream, false);
            StartPars(document);
            return RowsXlsxDate;
        }

        public List<Dictionary<string, string>> Pars(string filePath)
        {
            using SpreadsheetDocument document = SpreadsheetDocument.Open(filePath, false);
            StartPars(document);
            //SharedStringTable sharedStringTable = document.WorkbookPart.SharedStringTablePart.SharedStringTable;

            //foreach (WorksheetPart worksheetPart in document.WorkbookPart.WorksheetParts)
            //    IterateThroughSheetData(worksheetPart, sharedStringTable);

            return RowsXlsxDate;
        }

        private void StartPars(SpreadsheetDocument document)
        {
            SharedStringTable sharedStringTable = document.WorkbookPart.SharedStringTablePart.SharedStringTable;
            foreach (WorksheetPart worksheetPart in document.WorkbookPart.WorksheetParts)
                IterateThroughSheetData(worksheetPart, sharedStringTable);
        }

        //private Dictionary<string, Action<Guest, string>> _columnCommand = new()
        //{
        //    ["C"] = (x, y) => x.SetSurname(y),
        //    ["D"] = (x, y) => x.SetName(y),
        //    ["E"] = (x, y) => x.SetPatronymic(y),
        //    ["F"] = (x, y) => x.SetMail(y)
        //};


        private string DeterminingCorrectColumn(string callReference)
            =>Regex.Replace(callReference, "[0-9]", "", RegexOptions.IgnoreCase);

        private void IterateThroughCell(Row row, SharedStringTable sharedStringTable)
        {
            Guest guest = new Guest();
            var currentRow = new Dictionary<string, string>();

            foreach (Cell cell in row.Elements<Cell>())
            {
                var currentReference = DeterminingCorrectColumn(cell.CellReference.Value);
                int inner;
                var text = string.Empty;
                if (!Int32.TryParse(cell.InnerText, out inner))
                {
                    text = cell.InnerText;
                    currentRow.Add(currentReference, text);
                    continue;
                }

                try
                {
                    text = sharedStringTable.ElementAt(inner).InnerText;
                }
                catch (ArgumentOutOfRangeException)
                {
                    text = cell.InnerText;
                }
                catch(Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                currentRow.Add(currentReference, text);
            }
            RowsXlsxDate.Add(currentRow);
        }

            //private bool DeterminingCorrectColumn(string callReference, out string currentReference)
            //{
            //    currentReference = Regex.Replace(callReference, "[0-9]", "", RegexOptions.IgnoreCase);
            //    if (_columnCommand.ContainsKey(currentReference))
            //        return true;
            //    return false;
            //}

            //private void IterateThroughCell(Row row, SharedStringTable sharedStringTable)
            //{
            //    Guest guest = new Guest();
            //    foreach (Cell cell in row.Elements<Cell>())
            //        if (DeterminingCorrectColumn(cell.CellReference.Value, out string currentReference))
            //        {
            //            var text = sharedStringTable.ElementAt(int.Parse(cell.InnerText)).InnerText;
            //            _columnCommand[currentReference]?.Invoke(guest, text);
            //        }
            //    _guests.Add(guest);
            //}

            private void IterateThroughRow(SheetData sheetData, SharedStringTable sharedStringTable)
            {
                foreach (Row row in sheetData.Elements<Row>().Skip(1))
                    IterateThroughCell(row, sharedStringTable);
            }

            private void IterateThroughSheetData(WorksheetPart worksheetPart, SharedStringTable sharedStringTable)
            {
                foreach (SheetData sheetData in worksheetPart.Worksheet.Elements<SheetData>())
                    if (sheetData.HasChildren)
                        IterateThroughRow(sheetData, sharedStringTable);
            }
        }

}

