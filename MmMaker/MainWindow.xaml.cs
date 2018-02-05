using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MmMaker.Service;
using MmMaker.Model;

namespace MmMaker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<ExcelContent> _rows;

        public List<ExcelFile> ExcelFiles { get; set; }
        public List<ExcelContent> rows
        {
            get
            {
                if (_rows == null)
                {
                    return new List<ExcelContent>();
                }
                else
                {
                    List<ExcelContent> cont = new List<ExcelContent>();

                    cont = ExcelFiles.SelectMany(x => x.Content).ToList();

                    return cont;
                }
            }

            set
            {
                _rows = value;
            }
        }

        public MainWindow()
        {
            InitializeComponent();

            ExcelFiles = new List<ExcelFile>();
            rows = new List<ExcelContent>();


            //bindowanie danych
            FileList.ItemsSource = ExcelFiles;
        }

        /// <summary>
        /// Otwarcie okna dialogowego do wybrania pliku XLS
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            //otwarcie okna dialogowego
            Service.FileDialog dialog = new Service.FileDialog();
            string FileName = dialog.OpenDialog();

            //otwarcie pliku XLS
            ExcelFile excelFile = new ExcelFile(FileName);


            //parsowanie pliku xls
            List<ExcelContent> fileContent = new List<ExcelContent>();
            fileContent = excelFile.ParseExcel();

            //dodanie wyniku parsowania do kolekcji
            ExcelFiles.Add(new ExcelFile()
            {
                FileName = FileName,
                Content = fileContent
            });

            excelContent.ItemsSource = rows;

            FileList.Items.Refresh();
        }


        private void ExportToExcel_click(object sender, RoutedEventArgs e)
        {
            //czy lista zawieta dane
            if (rows == null || rows.Count == 0)
            {
                MessageBox.Show("Brak danych do zapisania", "Zapis", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            //posortuj liste
            List<ExcelContent> OrderedRows = rows.OrderBy(x => x.ProductName).ToList();

            //wywołaj eksport
            ExcelFile xlsFile = new ExcelFile();
            bool writeStatus = xlsFile.ExportToExcel(OrderedRows);

            //pokaż komunikat
            if (writeStatus)
            {
                MessageBox.Show("Zapis zakończony!", "Zapis", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Niepowodzenie eksportu", "Zapis", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }

        private void SaveToFirebase_click(object sender, RoutedEventArgs e)
        {
            FirebaseConnector firebase = new FirebaseConnector();
            firebase.Connect().ContinueWith(x =>
                firebase.GetData()

            );




        }

    }

}
