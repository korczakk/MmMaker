using MmMaker.Model;
using NLog;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MmMakerExcelManager
{
    public class ExcelParser
    {
        /// <summary>
        /// Otwarty skoroszyt Excela
        /// </summary>
        HSSFWorkbook _workBook;

        /// <summary>
        /// logger
        /// </summary>
        static Logger logger = LogManager.GetCurrentClassLogger();
        private List<ExcelContent> Content;
        private Stream _stream;

        public ExcelParser(Stream stream)
        {
            Content = new List<ExcelContent>();

            _stream = stream;

            //otwiera plik i przechowuje w polu
            _workBook = ReadExcelFile();
        }

        public List<ExcelContent> ParseExcel()
        {
            ISheet ws = _workBook.GetSheetAt(0);
            List<ExcelContent> ExcelContent = new List<ExcelContent>();

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
                    ExcelContent content = new ExcelContent()
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
        
        #region PrivateMethods

        /// <summary>
        /// Otwiera wskazany plik i zwraca jako skoroszyt Excel
        /// </summary>
        /// <param name="fileName">Scieżka do pliku</param>
        /// <returns>Skoroszyt Excel</returns>
        private HSSFWorkbook ReadExcelFile()
        {
            HSSFWorkbook wb;

            using (_stream)
            {
                wb = new HSSFWorkbook(_stream);
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
                logger.Error("Wystąpił błąd w trakcie oznaczania wiersza danych o nr: {0}. Original error message: {1}", row.RowNum, ex.Message);

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

