using EconomicTracking.Dal;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WPFReportTest;
using System.Data.Entity;

namespace EconomicTracking
{
    /// <summary>
    /// Interaction logic for BOMReports.xaml
    /// </summary>
    public partial class BOMReports : UserControl
    {
        public BOMReports()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var sList = new List<CustomerAssembly>();
            //using (var context = ContextHelper.GetContext())
            //{
            //    sList = await context.CustomerAssembly.Include("BOM").Where(x => x.CustAssyNo == cbmBom.SelectedItem.ToString()).Take(1).ToListAsync();
            //}
            if (cbmBom.SelectedIndex == -1 || cuscombo.SelectedIndex == -1)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("Please the CustomerAssemblyId", "Bill of Materials Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                //bomreportbtn.Visibility = Visibility.Visible;
                //x.Customer==cuscombo.SelectedItem.ToString()
                var context = new EconomicsTrackingDbContext();
                sList = await context.CustomerAssembly.Include("BOM").Where(x => x.CustAssyNo == cbmBom.SelectedItem.ToString()).Take(1).ToListAsync();

                //sList.ForEach(x => x.BOM.RemoveAll(y => y.LocalPartName == "" && y.LocalPartNo == ""));

                if (sList.Count() == 0)
                {
                    Xceed.Wpf.Toolkit.MessageBox.Show("No Matching Result Found.", "Bill of Materials Info", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    var qtyList = new List<CustomerAssemblyModel>();
                    foreach (var item in sList)
                    {
                        
                        var cust = new CustomerAssemblyModel();
                        cust.CustAssyNo = item.CustAssyNo;
                        cust.CustAssyName = item.CustAssyName;
                        cust.LocalAssyNo = item.LocalAssyNo;
                        cust.LocalAssyName = item.LocalAssyName;
                        //cust.Customer = item.Customer;
                        cust.Quantity = item.Quantity;
                        cust.BOM = new List<BillOfMaterialModel>();
                        item.BOM.ForEach(x =>
                        {
                            var bom = new BillOfMaterialModel();
                            bom.BOMQuantity = x.BOMQuantity;
                            bom.ChildPartRate = x.ChildPartRate;
                            //if (x.Commodity != "") { 
                            bom.Commodity = x.Commodity;
                            //}
                            bom.CurrencyCode = x.CurrencyCode;
                            bom.CustomerPartName = x.CustomerPartName;
                            bom.CustomerPartNo = x.CustomerPartNo;
                            bom.LocalPartName = x.LocalPartName;
                            bom.LocalPartNo = x.LocalPartNo;
                            bom.Quantity = x.Quantity;
                            bom.RawMaterial = x.RawMaterial;
                            bom.Scarp = x.Scarp;
                            bom.ScrapQuantity = x.ScrapQuantity;
                            bom.ToalCost = x.ToalCost;
                            bom.UOM = x.UOM;
                            //bom.to
                            cust.BOM.Add(bom);
                        });
                        qtyList.Add(cust);
                        
                    }
                    qtyList.ForEach(x => x.BOM.RemoveAll(y => y.LocalPartName == "" && y.LocalPartNo==""));
                    if (qtyList.Count > 0)
                    {
                       
                        BOMCrystalReport crystalReport = new BOMCrystalReport();

                        ReportUtility.DisplayBOMReports(crystalReport, qtyList);
                    }
                    else
                    {
                        Xceed.Wpf.Toolkit.MessageBox.Show("Don't have any records.", "Bill of Materials Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
        }

        //private async void ComboBox_Loaded(object sender, RoutedEventArgs e)
        //{
        //    using (var context = new EconomicsTrackingDbContext())
        //    {
        //        cbmBom.ItemsSource = await context.CustomerAssembly.Select(x => x.CustAssyNo).ToListAsync();
        //    }
        //}


        private async void cuscombo_Loaded(object sender, RoutedEventArgs e)
        {
            using (var context = new EconomicsTrackingDbContext())
            {
                cuscombo.ItemsSource = await context.CustomerAssembly.Select(x => x.Customer).Distinct().ToListAsync();
            }

        }

        private async void cuscombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            using (var context = new EconomicsTrackingDbContext())
            {
                if (cuscombo.SelectedIndex != -1)
                { 
                var custname = await context.CustomerAssembly.Select(x => new { x.Customer, x.CustAssyNo }).Where(x => x.Customer == cuscombo.SelectedItem.ToString()).ToListAsync();

                var cusass = (from d in custname
                             select d.CustAssyNo).Distinct().ToList();
                cbmBom.ItemsSource = cusass.ToList();
                }
                else { cbmBom.ItemsSource = null; }
                
            }

        }

        private async void cbmBom_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            using (var context = new EconomicsTrackingDbContext())
            {

                if (cbmBom.SelectedIndex != -1 && cuscombo.SelectedIndex!=-1)
                { 
                var custass = await context.CustomerAssembly.Select(x => x).Where(x => x.Customer == cuscombo.SelectedItem.ToString()).Distinct().ToListAsync();
                var cut = cbmBom.SelectedItem.ToString();
                var custtemp = from d in context.CustomerAssembly
                               where d.CustAssyNo == cut
                               select d;
                cbmbomsetref.ItemsSource =  custtemp.Select(x=>x.SettlementRef).ToList();
                }
                else { cbmbomsetref.ItemsSource = null; }
            }

        }
    }
}
