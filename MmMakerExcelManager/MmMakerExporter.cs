using MmMaker.Model;
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
    public class MmMakerExporter
    {
        private Stream _stream;

        public MmMakerExporter(Stream stream)
        {
            _stream = stream;
        }

        public bool ExportToExcel(List<ExcelContent> dataToExport)
        {
            using (_stream)
            {
                IWorkbook wb = new HSSFWorkbook();
                ISheet ws = wb.CreateSheet("Scalone.xls");

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

                wb.Write(_stream);
            }

            return true;
        }

    }
}
