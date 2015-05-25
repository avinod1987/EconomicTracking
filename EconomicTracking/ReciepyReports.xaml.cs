using EconomicTracking.Dal;
using System;
using System.Collections.Generic;
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

namespace EconomicTracking
{
    /// <summary>
    /// Interaction logic for ReciepyReports.xaml
    /// </summary>
    public partial class ReciepyReports : UserControl
    {
        public ReciepyReports()
        {
            InitializeComponent();


        }

        private void ComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            var db = new EconomicsTrackingDbContext();
            CusAssIdCombo.ItemsSource = db.CustomerAssembly.Select(x => x.CustAssyNo).Distinct().ToList();
        }

        private  void receipeshowbutton_Click(object sender, RoutedEventArgs e)
        {
            var sList = new List<BillOfMaterial>();
            EconomicsTrackingDbContext context = new EconomicsTrackingDbContext();
            sList = context.BillOfMaterial.Select(x => x).Where(x=>x.CustAssyNo==CusAssIdCombo.SelectedItem.ToString()).ToList();
            //QuantityInAssembly = d.Sum(x => x.Quantity)

            var comditylist = (from s in sList
                               where s.LocalPartName != string.Empty && s.LocalPartNo != string.Empty && s.Quantity != 0 && s.UOM != ""
                               group s by new { commodity = s.Commodity, uom = s.UOM } into d
                               select new { CommodityName = d.Key.commodity, UOM = d.Key.uom, RamMaterialWeigth= d.Sum(x => x.BOMQuantity), TotalQty = d.Sum(x => x.BOMQuantity) * d.Sum(x => x.Quantity) }).OrderBy(x => x.CommodityName);
            
            var scraplist = (from s in sList
                             where s.LocalPartName != string.Empty && s.LocalPartNo != string.Empty && s.Quantity != 0 &&s.UOM!=""
                             group s by new { commodity = s.Commodity, uom = s.UOM } into d
                             select new { CommodityName = d.Key.commodity, UOM = d.Key.uom, ScrapQuanity = d.Sum(x => x.ScrapQuantity), TotalQty = d.Sum(x => x.ScrapQuantity) * d.Sum(x => x.Quantity) }).OrderBy(x => x.CommodityName);
            var currencylist = (from s in sList
                             group s by new { Currency = s.CurrencyCode } into d
                                select new { CurrencyType = d.Key.Currency, ChildPartRate = d.Sum(x => x.ChildPartRate), TotlInPurCurrINR = d.Sum(x => x.ChildPartRate) * d.Sum(x => x.Quantity) }).Where(x => x.TotlInPurCurrINR != 0).OrderBy(x => x.CurrencyType);

            var overhead = context.BillOfMaterial.Where(x => x.CustAssyNo == CusAssIdCombo.SelectedItem.ToString() && x.LocalPartNo == "" && x.LocalPartName == "" && x.Quantity == 0&&x.UOM=="").Select(t => new { OverHeadType = t.CustomerPartName, OHSetmtCurency = t.ToalCost, CurrencyCode = t.CurrencyCode }).ToList();
            //d.Sum(x => x.ToalCost)
            
            if (comditylist != null) {
                commoditydatagrid.Visibility = Visibility.Visible;
                commoditydatagrid.ItemsSource = comditylist;
                
                
                recepiereportbtn.Visibility = Visibility.Visible;
                //OverHeadgrid.Visibility = Visibility.Visible;
                //OverHeadgrid.ItemsSource = li;
            }
            if (scraplist != null) {
                Scrapdatagrid.Visibility = Visibility.Visible;
                Scrapdatagrid.ItemsSource = scraplist;
            }
            if (currencylist != null) {
                Currencygrid.Visibility = Visibility.Visible;
                Currencygrid.ItemsSource = currencylist;
            }
            if (overhead != null)
            {
               OverHeadgrid.Visibility = Visibility.Visible;
               OverHeadgrid.ItemsSource = overhead;
            }

        }

        private void CusAssIdCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CusAssIdCombo.SelectedItem != null) {
                receipeshowbutton.Visibility = Visibility.Visible;
            }
            else
            {
                receipeshowbutton.Visibility = Visibility.Hidden;
            }

        }

    }

    //public class overhd
    //{
    //    public string TypeofOH { get; set; }

    //    public float  OHinSetldCurr { get; set; }

    //    public string currency { get; set; }

    //    public float OHInINR { get; set; }

    //}
}
