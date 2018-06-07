using System.Collections.Generic;
using System;


namespace MmMaker.Model
{
    public class ExcelFile
    {
        public string FileName { get; set; }
        public List<ExcelContent> Content { get; set; }
    }
}
