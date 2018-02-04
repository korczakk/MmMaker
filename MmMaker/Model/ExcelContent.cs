using System;

namespace MmMaker.Model
{
    public class ExcelContent
    {
        public int Lp { get; set; }
        public double BarCode { get; set; }
        public string ProductName { get; set; }
        public double NumberOfItems { get; set; }
        public double ItemGrosValue { get; set; }
        public double TotalGrosValue { get; set; }
        public Guid Id { get; set; }

        
    }
}