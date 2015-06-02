using EconomicTracking.Dal;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPFReportTest;

namespace EconomicTracking
{
    /// <summary>
    /// Interaction logic for ReciepyReports.xaml
    /// </summary>
    public partial class ReciepyReports : System.Windows.Controls.UserControl
    {
        DataTable dt1 = null; DataTable dt3 = null; DataTable dt4 = null;
        DataTable dt2 = null; Commodity ds = null;

        public ReciepyReports()
        {
            InitializeComponent();
        }

        private void ComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            var db = new EconomicsTrackingDbContext();
            CusAssIdCombo.ItemsSource = db.CustomerAssembly.Select(x => x.CustAssyNo).Distinct().ToList();

        }

        private void receipeshowbutton_Click(object sender, RoutedEventArgs e)
        {
            var sList = new List<BillOfMaterial>();
            EconomicsTrackingDbContext context = new EconomicsTrackingDbContext();
            sList = context.BillOfMaterial.Select(x => x).Where(x => x.CustAssyNo == CusAssIdCombo.SelectedItem.ToString()).ToList();
            var comditylist = (from s in sList
                               where s.LocalPartName != string.Empty && s.LocalPartNo != string.Empty && s.Quantity != 0 && s.UOM != ""
                               group s by new { commodity = s.Commodity, uom = s.RMUOM.ToLower() == "g" || s.RMUOM.ToLower() == "gm" ? "KG" : s.RMUOM.ToLower() == "ml" || s.RMUOM.ToLower() == "m" ? "LITER" : s.RMUOM.ToUpper() } into d
                               select new { CommodityName = d.Key.commodity, UOM = d.Key.uom, RamMaterialWeigth = d.Sum(x => x.RMUOM.ToLower() == "g" || x.RMUOM == "gm" ? (x.TotalRMqty) / 100 : x.RMUOM.ToLower() == "m" || x.RMUOM == "ml" ? (x.TotalRMqty) / 1000 : x.TotalRMqty) }).OrderBy(x => x.CommodityName);
            var scraplist = (from s in sList
                             where s.LocalPartName != string.Empty && s.LocalPartNo != string.Empty && s.Quantity != 0 && s.UOM != ""
                             group s by new { commodity = s.Commodity, uom = s.RMUOM.ToLower() == "g" || s.RMUOM.ToLower() == "gm" ? "KG" : s.RMUOM.ToLower() == "ml" || s.RMUOM.ToLower() == "m" ? "LITER" : s.RMUOM.ToUpper() } into d
                             select new { ScarpCommodityName = d.Key.commodity, UOM = d.Key.uom, ScrapQuanity = d.Sum(x => x.RMUOM.ToLower() == "g" || x.RMUOM == "gm" ? (x.Scraptotalqty) / 100 : x.RMUOM.ToLower() == "m" || x.RMUOM == "ml" ? (x.Scraptotalqty) / 1000 : x.Scraptotalqty) }).OrderBy(x => x.ScarpCommodityName);
            var currencylist = (from s in sList
                                where s.ToalCost != 0
                                group s by new { Currency = s.CurrencyCode } into d
                                select new { CurrencyType = d.Key.Currency, CurrencyInINR = d.Sum(x => x.ToalCost), TotlInPurCurr = d.Sum(x => x.TotalcostinPurCurr) }).Where(x => x.TotlInPurCurr != 0).OrderBy(x => x.CurrencyType);

            var overhead = context.BillOfMaterial.Where(x => x.CustAssyNo == CusAssIdCombo.SelectedItem.ToString() && x.LocalPartNo == "" && x.LocalPartName == "" && x.Quantity == 0 && x.UOM == "").Select(t => new { OverHeadType = t.CustomerPartName, OHSetmtCurencyINR = t.ToalCost, OHSetmtCurency = t.TotalcostinPurCurr, CurrencyCode = t.CurrencyCode }).ToList();
            //d.Sum(x => x.ToalCost)

            ds = new Commodity();
            DataTable dt1 = ds.CommdityRecepie;
            DataRow dr;
            foreach (var r in comditylist)
            {
                dr = dt1.NewRow();
                dr["Commodity"] = r.CommodityName;
                dr["UOM"] = r.UOM; dr["RMWeight"] = r.RamMaterialWeigth;
                dt1.Rows.Add(dr);
            }
            dt1.AcceptChanges();
            DataTable dt2 = ds.Currency;
            DataRow dr1;
            foreach (var r in currencylist)
            {
                dr1 = dt2.NewRow();
                dr1["CurrencyCode"] = r.CurrencyType;
                dr1["CurrencyinINR"] = r.CurrencyInINR; dr1["PurrCurrency"] = r.TotlInPurCurr;
                dt2.Rows.Add(dr1);
            }
            dt2.AcceptChanges();

            DataTable dt3 = ds.Scrap;
            DataRow dr2;
            foreach (var r in scraplist)
            {
                dr2 = dt3.NewRow();
                dr2["ScrapName"] = r.ScarpCommodityName;
                dr2["UOM"] = r.UOM; dr2["ScrapWeight"] = r.ScrapQuanity;
                dt3.Rows.Add(dr2);
            }
            dt3.AcceptChanges();
            DataTable dt4 = ds.OverHead;
            DataRow dr3;
            foreach (var r in overhead)
            {
                dr3 = dt4.NewRow();
                dr3["OHType"] = r.OverHeadType;
                dr3["OHinINR"] = r.OHSetmtCurencyINR; dr3["OHinSettCurr"] = r.OHSetmtCurency;
                dt4.Rows.Add(dr3);
            }
            dt4.AcceptChanges();

            if (comditylist != null)
            {
                commoditydatagrid.Visibility = Visibility.Visible;
                commoditydatagrid.ItemsSource = comditylist;


                recepiereportbtn.Visibility = Visibility.Visible;
                //OverHeadgrid.Visibility = Visibility.Visible;
                //OverHeadgrid.ItemsSource = li;
            }
            if (scraplist != null)
            {
                Scrapdatagrid.Visibility = Visibility.Visible;
                Scrapdatagrid.ItemsSource = scraplist;
            }
            if (currencylist != null)
            {
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
            if (CusAssIdCombo.SelectedItem != null)
            {
                receipeshowbutton.Visibility = Visibility.Visible;
            }
            else
            {
                receipeshowbutton.Visibility = Visibility.Hidden;
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            getdetails();
            recepie crp = new recepie();
            crp.SetDataSource(ds);
            ReportViewerUI rp = new ReportViewerUI();
            rp.setReportSource(crp);
            rp.ShowDialog();


        }

        private void getdetails()
        {
            if (ds == null)
            {
                var sList = new List<BillOfMaterial>();
                EconomicsTrackingDbContext context = new EconomicsTrackingDbContext();
                sList = context.BillOfMaterial.Select(x => x).Where(x => x.CustAssyNo == CusAssIdCombo.SelectedItem.ToString()).ToList();
                //QuantityInAssembly = d.Sum(x => x.Quantity)
                //var comditylisttemp=sList.Select(x=>new{x.Commodity,x.BOMQuantity,x.Quantity,})
                //, TotalQty = d.Sum(x => x.BOMQuantity) * d.Sum(x => x.Quantity)
                //, TotalQty = d.Sum(x => x.ScrapQuantity) * d.Sum(x => x.Quantity)
                var comditylist = (from s in sList
                                   where s.LocalPartName != string.Empty && s.LocalPartNo != string.Empty && s.Quantity != 0 && s.UOM != ""
                                   group s by new { commodity = s.Commodity, uom = s.RMUOM.ToLower() == "g" || s.RMUOM.ToLower() == "gm" ? "KG" : s.RMUOM.ToLower() == "ml" || s.RMUOM.ToLower() == "m" ? "LITER" : s.RMUOM.ToUpper() } into d
                                   select new { CommodityName = d.Key.commodity, UOM = d.Key.uom, RamMaterialWeigth = d.Sum(x => x.RMUOM.ToLower() == "g" || x.RMUOM == "gm" ? (x.TotalRMqty) / 100 : x.RMUOM.ToLower() == "m" || x.RMUOM == "ml" ? (x.TotalRMqty) / 1000 : x.TotalRMqty) }).OrderBy(x => x.CommodityName);
                var scraplist = (from s in sList
                                 where s.LocalPartName != string.Empty && s.LocalPartNo != string.Empty && s.Quantity != 0 && s.UOM != ""
                                 group s by new { commodity = s.Commodity, uom = s.RMUOM.ToLower() == "g" || s.RMUOM.ToLower() == "gm" ? "KG" : s.RMUOM.ToLower() == "ml" || s.RMUOM.ToLower() == "m" ? "LITER" : s.RMUOM.ToUpper() } into d
                                 select new { CommodityName = d.Key.commodity, UOM = d.Key.uom, ScrapQuanity = d.Sum(x => x.RMUOM.ToLower() == "g" || x.RMUOM == "gm" ? (x.Scraptotalqty) / 100 : x.RMUOM.ToLower() == "m" || x.RMUOM == "ml" ? (x.Scraptotalqty) / 1000 : x.Scraptotalqty) }).OrderBy(x => x.CommodityName);
                var currencylist = (from s in sList
                                    where s.ToalCost != 0
                                    group s by new { Currency = s.CurrencyCode } into d
                                    select new { CurrencyType = d.Key.Currency, CurrencyInINR = d.Sum(x => x.ToalCost), TotlInPurCurr = d.Sum(x => x.TotalcostinPurCurr) }).Where(x => x.TotlInPurCurr != 0).OrderBy(x => x.CurrencyType);

                var overhead = context.BillOfMaterial.Where(x => x.CustAssyNo == CusAssIdCombo.SelectedItem.ToString() && x.LocalPartNo == "" && x.LocalPartName == "" && x.Quantity == 0 && x.UOM == "").Select(t => new { OverHeadType = t.CustomerPartName, OHSetmtCurencyINR = t.ToalCost, OHSetmtCurency = t.TotalcostinPurCurr, CurrencyCode = t.CurrencyCode }).ToList();
                //d.Sum(x => x.ToalCost)

                ds = new Commodity();
                DataTable dt1 = ds.CommdityRecepie;
                DataRow dr;
                foreach (var r in comditylist)
                {
                    dr = dt1.NewRow();
                    dr["Commodity"] = r.CommodityName;
                    dr["UOM"] = r.UOM; dr["RMWeight"] = r.RamMaterialWeigth;
                    dt1.Rows.Add(dr);
                }
                dt1.AcceptChanges();
                DataTable dt2 = ds.Currency;
                DataRow dr1;
                foreach (var r in currencylist)
                {
                    dr1 = dt2.NewRow();
                    dr1["CurrencyCode"] = r.CurrencyType;
                    dr1["CurrencyinINR"] = r.CurrencyInINR; dr1["PurrCurrency"] = r.TotlInPurCurr;
                    dt2.Rows.Add(dr1);
                }
                dt2.AcceptChanges();

                DataTable dt3 = ds.Scrap;
                DataRow dr2;
                foreach (var r in scraplist)
                {
                    dr2 = dt2.NewRow();
                    dr2["ScrapName"] = r.CommodityName;
                    dr2["UOM"] = r.UOM; dr2["ScrapWeight"] = r.ScrapQuanity;
                    dt3.Rows.Add(dr2);
                }
                dt3.AcceptChanges();
                DataTable dt4 = ds.OverHead;
                DataRow dr3;
                foreach (var r in overhead)
                {
                    dr3 = dt3.NewRow();
                    dr3["OHType"] = r.OverHeadType;
                    dr3["OHinINR"] = r.OHSetmtCurencyINR; dr3["OHinSettCurr"] = r.OHSetmtCurency;
                    dt4.Rows.Add(dr3);
                }
                dt4.AcceptChanges();
            }

        }

        private void recepiereportbtn_Click(object sender, RoutedEventArgs e)
        {
            //Instance reference for Excel Application
            Microsoft.Office.Interop.Excel.Application objXL = null;
            //Workbook refrence
            Microsoft.Office.Interop.Excel.Workbook objWB = null;
            try
            {
                //Instancing Excel using COM services
                objXL = new Microsoft.Office.Interop.Excel.Application();
                //Adding WorkBook
                objWB = objXL.Workbooks.Add(ds.Tables.Count);
                //Variable to keep sheet count
                int sheetcount = 1;
                //Do I need to explain this ??? If yes please close my website and learn abc of .net .
                foreach (DataTable dt in ds.Tables)
                {
                    //Adding sheet to workbook for each datatable
                    Microsoft.Office.Interop.Excel.Worksheet objSHT = (Microsoft.Office.Interop.Excel.Worksheet)objWB.Sheets.Add();
                    //Naming sheet as SheetData1.SheetData2 etc....
                    objSHT.Name = "SheetData" + sheetcount.ToString();
                    for (int j = 0; j < dt.Rows.Count; j++)
                    {
                        for (int i = 0; i < dt.Columns.Count; i++)
                        {
                            //Condition to put column names in 1st row
                            //Excel work book indexes start from 1,1 and not 0,0
                            if (j == 0)
                            {
                                objSHT.Cells[1, i + 1] = dt.Columns[i].ColumnName.ToString();
                            }
                            //Writing down data
                            objSHT.Cells[j + 2, i + 1] = dt.Rows[j][i].ToString();
                        }
                    }
                    //Incrementing sheet count
                    sheetcount++;
                }
                //Saving the work book
                objWB.Saved = true;
                SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Excel file (*.xls)|*.xls|Excel Files (*.xlsx)|*.xlsx";
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                objWB.SaveCopyAs(saveFileDialog.FileName);
            
            }
                
                //Closing work book
                objWB.Close();
                //Closing excel application
                objXL.Quit();

            }
            catch (Exception ex)
            {
                objWB.Saved = true;
                //Closing work book
                objWB.Close();
                //Closing excel application
                objXL.Quit();
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
