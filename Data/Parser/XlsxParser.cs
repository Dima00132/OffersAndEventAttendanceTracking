//using Bytescout.Spreadsheet;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Vml.Spreadsheet;
using ScannerAndDistributionOfQRCodes.Data.Parser.Interface;
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
    public sealed class XlsxParser : IParser
    {
        private readonly List<Dictionary<string, string>> _rowsXlsxDate = [];

        public List<Dictionary<string, string>> Pars(Stream stream)
        {
            using SpreadsheetDocument document = SpreadsheetDocument.Open(stream, false);
            StartPars(document);
            return _rowsXlsxDate;
        }

        public List<Dictionary<string, string>> Pars(string filePath)
        {
            using SpreadsheetDocument document = SpreadsheetDocument.Open(filePath, false);
            StartPars(document);
            return _rowsXlsxDate;
        }

        private void StartPars(SpreadsheetDocument document)
        {
            SharedStringTable sharedStringTable = document.WorkbookPart.SharedStringTablePart.SharedStringTable;
            foreach (WorksheetPart worksheetPart in document.WorkbookPart.WorksheetParts)
                IterateThroughSheetData(worksheetPart, sharedStringTable);
        }

        private string DeterminingCorrectColumn(string callReference)
            =>Regex.Replace(callReference, "[0-9]", "", RegexOptions.IgnoreCase);


        private bool ParseInnerText(Cell cell,out int inner, out string text)
        {
            text = string.Empty;
            if (!Int32.TryParse(cell.InnerText, out inner))
            {
                text = cell.InnerText;
                return false;
            }
            return true;
        }

        private string GetInnerText(SharedStringTable sharedStringTable,int inner, Cell cell)
        {
            try
            {
                return sharedStringTable.ElementAt(inner).InnerText;
            }
            catch (ArgumentOutOfRangeException)
            {
                return cell.InnerText;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void IterateThroughCell(Row row, SharedStringTable sharedStringTable)
        {
            var currentRow = new Dictionary<string, string>();

            foreach (Cell cell in row.Elements<Cell>())
            {
                var currentReference = DeterminingCorrectColumn(cell.CellReference.Value);
                var isInnerText = ParseInnerText(cell, out int inner, out string text);
                if(isInnerText)
                    text = GetInnerText(sharedStringTable, inner, cell);
                currentRow.Add(currentReference, text);
            }
            _rowsXlsxDate.Add(currentRow);
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

