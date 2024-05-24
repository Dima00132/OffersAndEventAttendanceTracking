//using Bytescout.Spreadsheet;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Vml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Cell = DocumentFormat.OpenXml.Spreadsheet.Cell;


namespace ScannerAndDistributionOfQRCodes.Model.Parser
{
    public interface IParser
    {
        public List<Guest> Pars(string filePath);
    }
    public class XlsxParser : IParser
    {

        private readonly List<Guest> _guests = [];

        public List<Guest> Pars(string filePath)
        {
            using SpreadsheetDocument document = SpreadsheetDocument.Open(filePath, false);
            SharedStringTable sharedStringTable = document.WorkbookPart.SharedStringTablePart.SharedStringTable;
            
            foreach (WorksheetPart worksheetPart in document.WorkbookPart.WorksheetParts)
                IterateThroughSheetData(worksheetPart, sharedStringTable);

            return _guests;
        }

        private Dictionary<string, Action<Guest, string>> _columnCommand = new()
        {
            ["C"] = (x, y) => x.SetSurname(y),
            ["D"] = (x, y) => x.SetName(y),
            ["E"] = (x, y) => x.SetPatronymic(y),
            ["F"] = (x, y) => x.SetMail(y)
        };


        private bool DeterminingCorrectColumn(string callReference, out string currentReference)
        {
            currentReference = Regex.Replace(callReference, "[0-9]", "", RegexOptions.IgnoreCase);
            if (_columnCommand.ContainsKey(currentReference))
                return true;
            return false;
        }

        private void IterateThroughCell(Row row, SharedStringTable sharedStringTable)
        {
            Guest guest = new Guest();
            foreach (Cell cell in row.Elements<Cell>())
                if (DeterminingCorrectColumn(cell.CellReference.Value, out string  currentReference))
                {
                    var text = sharedStringTable.ElementAt(Int32.Parse(cell.InnerText)).InnerText;
                    _columnCommand[currentReference]?.Invoke(guest, text);
                }
            _guests.Add(guest);
        }

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

