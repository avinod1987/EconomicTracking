using EconomicTracking.Dal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
using WPF.MDI;
using WPFReportTest;

namespace EconomicTracking
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            AppDomain.CurrentDomain.SetData("DataDirectory", System.IO.Directory.GetCurrentDirectory());
            if (!Directory.Exists(@"c:\App_Data"))
                Directory.CreateDirectory(@"c:\App_Data");
            var di = new DirectoryInfo(@"c:\App_Data");
            di.Attributes &= ~FileAttributes.ReadOnly;
            //ClearReadOnly(di);
            Database.SetInitializer<EconomicsTrackingDbContext>(null);
            
        }
        private void ClearReadOnly(DirectoryInfo parentDirectory)
        {
            if (parentDirectory != null)
            {
                parentDirectory.Attributes = FileAttributes.Normal;
                foreach (FileInfo fi in parentDirectory.GetFiles())
                {
                    fi.Attributes = FileAttributes.Normal;
                }
                foreach (DirectoryInfo di in parentDirectory.GetDirectories())
                {
                    ClearReadOnly(di);
                }
            }
        }

        private void AddWindow_Click(object sender, RoutedEventArgs e)
        {

            Container.Children.Add(new MdiChild()
            {
                Title = "Customer Details",
                Content = new CustomerInfo(),
                Height = 600,
                Width = 800
            });
        }

        private void AddSettlement_Click(object sender, RoutedEventArgs e)
        {

            Container.Children.Add(new MdiChild()
            {
                Title = "Upload Settlement Details",
                Content = new SettlementControl(),
                Height = 600,
                Width = 800
            });

        }


        private void SettlementReports_Click(object sender, RoutedEventArgs e)
        {

    Container.Children.Add(new MdiChild()
    {
        Title = "Settlement Reports",
        Content = new SettlementReports(),
        Height = 600,
        Width = 950
    });
          
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Container.Children.Add(new MdiChild()
            {
                Title = "Upload Sales Quantity Details",
                Content = new UploadSalesQty(),
                Height = 600,
                Width = 800
            });
        }

        private void SalesQtyReports_Click(object sender, RoutedEventArgs e)
        {
            Container.Children.Add(new MdiChild()
            {
                Title = "Sales Quantity Reports",
                Content = new SalesQtyReports(),
                Height = 600,
                Width = 800
            });
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            Container.Children.Add(new MdiChild()
            {
                Title = "Upload Bill Of Material Details",
                Content = new UploadBom(),
                Height = 600,
                Width = 800
            });
        }

        private void BOMReports_Click(object sender, RoutedEventArgs e)
        {
            Container.Children.Add(new MdiChild()
            {
                Title = "Bill Of Material Report",
                Content = new BOMReports(),
                Height = 600,
                Width = 800
            });
        }

        private void FinalReports_Click(object sender, RoutedEventArgs e)
        {
            Container.Children.Add(new MdiChild()
            {
                Title = "Upload Commodity Final Reports",
                Content = new UploadFinalReport(),
                Height = 600,
                Width = 800
            });
        }

        private void FinalSReports_Click(object sender, RoutedEventArgs e)
        {
            Container.Children.Add(new MdiChild()
            {
                Title = "Commodity Final Reports",
                Content = new FinalSReports(),
                Height = 600,
                Width = 800
            });
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        private void Receipe_Click(object sender, RoutedEventArgs e)
        {
            Container.Children.Add(new MdiChild()
            {
                Title = "Receipe Reports",
                Content = new ReciepyReports(),
                Height = 600,
                Width = 800
            });

        }

        private void Recovery_Click(object sender, RoutedEventArgs e)
        {
            Container.Children.Add(new MdiChild()
            {
                Title = "Receipe Reports",
                Content = new Recovery(),
                Height = 600,
                Width = 800
            });

        }

        //Recovery



    }
}
