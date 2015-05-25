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
            if (cbmBom.SelectedItem == null)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("Please the CustomerAssemblyId", "Bill of Materials Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                //bomreportbtn.Visibility = Visibility.Visible;
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

        private async void ComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            using (var context = new EconomicsTrackingDbContext())
            {
                cbmBom.ItemsSource = await context.CustomerAssembly.Select(x => x.CustAssyNo).ToListAsync();
            }
        }
    }
}
