using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.Collections.Generic;
using System.IO;
using MmMaker.Service;
using System;
using NLog;
using System.Collections.ObjectModel;

namespace MmMaker.Model
{
    public class ExcelFile
    {
        public string FileName { get; set; }
        public List<ExcelContent> Content { get; set; }

        /// <summary>
        /// Otwarty skoroszyt Excela
        /// </summary>
        HSSFWorkbook _workBook;

        /// <summary>
        /// logger
        /// </summary>
        static Logger logger = LogManager.GetCurrentClassLogger();

        public ExcelFile()
        {
            Content = new List<ExcelContent>();
        }

        /// <summary>
        /// Ten konstruktor otwiera plik Excel, którego nazwa została przekazana w parametrze
        /// </summary>
        /// <param name="fileName"></param>
        public ExcelFile(string fileName)
        {
            FileName = fileName;

            Content = new List<ExcelContent>();

            //otwiera plik i przechowuje w polu
            _workBook = ReadExcelFile();
        }


        public List<ExcelContent> ParseExcel()
        {
            ISheet ws = _workBook.GetSheetAt(0);
            List<ExcelContent> ExcelContent = new List<Model.ExcelContent>();

            //iteracja przez wiersze
            for (int i = 0; i < ws.LastRowNum; i++)
            {
                IRow row = ws.GetRow(i);

                //czy wiersz jest pusty
                if (row == null || IsRowEmpty(row))
                {
                    continue;
                }


                //jeśli wiersz ma dane to parsuj dane
                if (IsDataRow(row))
                {
                    //Dane z wiersza zapisywane są do obiektu
                    ExcelContent content = new Model.ExcelContent()
                    {
                        Id = Guid.NewGuid(),
                        BarCode = row.Cells[3].NumericCellValue,
                        ProductName = row.Cells[8].StringCellValue,
                        NumberOfItems = double.Parse(row.Cells[24].ToString()),
                        ItemGrosValue = double.Parse(row.Cells[27].ToString()),
                        TotalGrosValue = double.Parse(row.Cells[35].ToString())
                    };

                    ExcelContent.Add(content);
                }

            }

            return ExcelContent;
        }


        public bool ExportToExcel(List<ExcelContent> dataToExport)
        {
            using (FileStream stream = new FileStream("Scalone.xls", FileMode.Create, FileAccess.Write))
            {
                IWorkbook wb = new HSSFWorkbook();
                ISheet ws = wb.CreateSheet("Scalone");

                //nagłówek
                IRow header = ws.CreateRow(0);

                header.CreateCell(0).SetCellValue("Lp");
                header.CreateCell(1).SetCellValue("Kod");
                header.CreateCell(2).SetCellValue("Nazwa");
                header.CreateCell(3).SetCellValue("Liczba sztuk");
                header.CreateCell(4).SetCellValue("Cena");
                header.CreateCell(5).SetCellValue("Wartość brutto");

                int rowNum = 1;

                foreach (ExcelContent item in dataToExport)
                {
                    IRow row = ws.CreateRow(rowNum);

                    row.CreateCell(0).SetCellValue(rowNum);
                    row.CreateCell(1).SetCellValue(item.BarCode);
                    row.CreateCell(2).SetCellValue(item.ProductName);
                    row.CreateCell(3).SetCellValue(item.NumberOfItems);
                    row.CreateCell(4).SetCellValue(item.ItemGrosValue);
                    row.CreateCell(5).SetCellValue(item.TotalGrosValue);

                    rowNum += 1;
                }

                wb.Write(stream);
            }

            return true;
        }

        #region PrivateMethods

        /// <summary>
        /// Otwiera wskazany plik i zwraca jako skoroszyt Excel
        /// </summary>
        /// <param name="fileName">Scieżka do pliku</param>
        /// <returns>Skoroszyt Excel</returns>
        private HSSFWorkbook ReadExcelFile()
        {
            HSSFWorkbook wb;

            using (FileStream stream = new FileStream(FileName, FileMode.Open, FileAccess.Read))
            {
                wb = new HSSFWorkbook(stream);
            }

            return wb;
        }


        /// <summary>
        /// Sprawdza czy wiersz zawiera dane; sprawdzenie na podstawie kolejności kolumn w pliku XLS
        /// </summary>
        /// <param name="row">Sprawdzany wiersz</param>
        /// <returns>Jeśli wiersz zawiera dane zwraca true</returns>
        private bool IsDataRow(IRow row)
        {
            try
            {
                if (row.Cells.Count < 28) return false;

                int TestResult = 0;

                if (row.Cells[1].CellType == CellType.Numeric) TestResult += 1;
                if (row.Cells[3].CellType == CellType.Numeric) TestResult += 1;
                if (row.Cells[8].CellType == CellType.String) TestResult += 1;

                double val;
                bool tryResult1 = double.TryParse(row.Cells[24].ToString(), out val);
                if (tryResult1) TestResult += 1;

                bool tryResult2 = double.TryParse(row.Cells[27].ToString(), out val);
                if (tryResult2) TestResult += 1;

                bool tryResult3 = double.TryParse(row.Cells[35].ToString(), out val);
                if (tryResult3) TestResult += 1;

                return TestResult == 6;

            }
            catch (System.Exception ex)
            {
                logger.Error("Wystąpił błąd w trakcie oznaczania wiersza danych o nr: {0} w pliku: {1}. Original error message: {2}", row.RowNum, FileName, ex.Message);

                return false;
            }
        }


        /// <summary>
        /// Sprawdza czy wiersz zawiera komórki z wartościami.
        /// </summary>
        /// <param name="row">Sprawdzany wiersz</param>
        /// <returns>True jeśli wiersz zawiera tylko puste komórki</returns>
        private bool IsRowEmpty(IRow row)
        {
            foreach (ICell cell in row.Cells)
            {
                if (cell != null && cell.CellType != CellType.Blank)
                {
                    return false;
                }
            }

            return true;
        }

        #endregion  
    }
}
