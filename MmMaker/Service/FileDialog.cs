using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MmMaker.Service
{
    public class FileDialog
    {
        public  string OpenDialog()
        {
            string FileName = string.Empty;

            OpenFileDialog dialog = new OpenFileDialog();

            dialog.Filter = "Plik Excel 2003|*.xls|Plik Excel 2007|*.xlsx";

            if (dialog.ShowDialog() == true)
            {
                FileName = dialog.FileName;
            }

            return FileName;
        }
    }
}
