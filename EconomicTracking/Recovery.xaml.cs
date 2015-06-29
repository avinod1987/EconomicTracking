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
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.Entity;
using System.ComponentModel;
using System.Threading;
namespace EconomicTracking
{
    /// <summary>
    /// Interaction logic for Recovery.xaml
    /// </summary>
    public partial class Recovery : UserControl
    {
        DataTable dt = new DataTable();
        List<string> li = new List<string>();
        List<string> litemp=new List<string>();
        BackgroundWorker b; DataColumn dc;

        public Recovery()
        {
            InitializeComponent();
            dc= new DataColumn("ID", typeof(System.Int32));
            DataColumn dc1 = new DataColumn("TotalRecovery");
            //dc.AutoIncrement = true;
            //dc.AutoIncrementSeed = 1;
            //dc.ReadOnly = false;
            //dc.Unique = true; 
            dt.Columns.Add(dc);
            dt.Columns.Add("CustomerAssemblyNo");
            dt.Columns.Add("ScrapRecovery");
            dt.Columns.Add("CurrencyRecovery");
            dt.Columns.Add("CommodityRecovery");
            dt.Columns.Add("OverHeadRecovery");
            dt.Columns.Add("TotalRecovery", typeof(decimal));
            
        }

        private async void comdchk_Checked(object sender, RoutedEventArgs e)
        {
            var context = new EconomicsTrackingDbContext();
            if (Custcombo.SelectedIndex != -1 && !string.IsNullOrEmpty(setreflbl.Content.ToString()) && !string.IsNullOrEmpty(setreflb2.Content.ToString()))
            {
                var ssList =await context.BillOfMaterial.Include("Material").Where(s => li.Contains(s.CustAssyNo)).ToListAsync();
                var sList = ssList.Where(s => s.LocalPartName != string.Empty && s.LocalPartNo != string.Empty && s.Quantity != 0 && s.UOM != "").ToList();
                int i = 0;
                var comfromsettbl =await context.Settlements.Where(x => x.SettlementRef == setreflbl.Content.ToString() && x.CustomerName == Custcombo.SelectedItem.ToString()).Select(x => new { x.Commodity }).ToListAsync();
                var comfromsettbl2 =await context.Settlements.Where(x => x.SettlementRef == setreflb2.Content.ToString() && x.CustomerName == Custcombo.SelectedItem.ToString()).Select(x => new { x.Commodity }).ToListAsync();
                foreach (var r in li.Distinct().ToList())
                {
                    var comtemp = (from s in sList
                                   where s.CustAssyNo == r
                                   select new
                                   {
                                       Commodity = s.Material.MaterialName,
                                       CustomerAssemblyNo = s.CustAssyNo,
                                       Totalrmqty = (s.RMUOM.ToLower() == "g" || s.RMUOM == "gm") ? s.TotalRMqty / 100 : (s.RMUOM.ToLower() == "ml" || s.RMUOM == "m") ? s.TotalRMqty / 1000 : s.TotalRMqty
                                   }).ToList();
                    var comdity = (from s in comtemp
                                   group s by new { Commodity = s.Commodity } into d
                                   select new { Commodity = d.Key.Commodity, TotalRmqtybyComm = d.Sum(x => x.Totalrmqty), CusAss = r }).OrderBy(x => x.Commodity).ToList();
                    IEnumerable<SettlementCommodity> comtt1 = comfromsettbl.SelectMany(x => x.Commodity).ToList();
                    IEnumerable<SettlementCommodity> comtt2 = comfromsettbl2.SelectMany(x => x.Commodity).ToList();
                    var cc = (from g in comdity
                              join h in comtt1
                              on g.Commodity.ToLower() equals h.MaterialName.ToLower()
                              join d in comtt2
                              on g.Commodity.ToLower() equals d.MaterialName.ToLower()
                             select new
                            {
                                Recoverye = (h.Rate - d.Rate) * g.TotalRmqtybyComm
                            }).ToList();
                    if (dt.Rows[i]["CommodityRecovery"].ToString() == null || dt.Rows[i]["CommodityRecovery"].ToString() == string.Empty)
                    {
                        dt.Rows[i]["CommodityRecovery"] =Math.Round(cc.Sum(x => x.Recoverye));
                    }
                    i++;
                }
                Thread t = new Thread(TotalColumnSum);
                t.Start();
                //TotalColumnSum();
                salesqtydatagrid.ItemsSource = dt.DefaultView;
            }
        }

        private void comdchk_Unchecked(object sender, RoutedEventArgs e)
        {
            //srpchk.IsChecked = false; Currchk.IsChecked = false; Ohchk.IsChecked = false;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["CommodityRecovery"] = string.Empty;
            }
            TotalColumnSum();
        }

        private async void Custcombo_Loaded(object sender, RoutedEventArgs e)
        {
            var context = new EconomicsTrackingDbContext();
            Custcombo.ItemsSource =await context.CustomerAssembly.Select(x => x.Customer).Distinct().ToListAsync();
        }

        private void setref1_Loaded(object sender, RoutedEventArgs e)
        {
            if (Custcombo.SelectedIndex != -1)
            {
                //setreflbl.Content = setref1.SelectedItem.ToString();
            }
        }

        private void setref2_Loaded(object sender, RoutedEventArgs e)
        {
            if (Custcombo.SelectedIndex != -1)
            {
            }
        }

        private void setrefdt1_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {

            if (setrefdt1.SelectedDate.HasValue && setref1.SelectedIndex == -1 && Custcombo.SelectedIndex != -1)
            {
                var context = new EconomicsTrackingDbContext();
                var cus = context.Settlements.Where(x => x.CustomerName == Custcombo.SelectedItem.ToString()).ToList();
                int i = cus.Where(x => x.SettlementFrom <= setrefdt1.SelectedDate && x.SettlementTo >= setrefdt1.SelectedDate).Select(x => x.SettlementRef).Count();
                if (i > 0)
                {
                    setreflbl.Content = cus.Where(x => x.SettlementFrom <= setrefdt1.SelectedDate && x.SettlementTo >= setrefdt1.SelectedDate).Select(x => x.SettlementRef).FirstOrDefault();
                }
            }
        }

        private void setrefdt2_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (setrefdt2.SelectedDate.HasValue && setref2.SelectedIndex == -1 && Custcombo.SelectedIndex != -1)
            {
                var context = new EconomicsTrackingDbContext();
                var cus = context.Settlements.Where(x => x.CustomerName == Custcombo.SelectedItem.ToString()).ToList();
                int i = cus.Where(x => x.SettlementFrom <= setrefdt2.SelectedDate && x.SettlementTo >= setrefdt2.SelectedDate).Select(x => x.SettlementRef).Count();
                if (i > 0)
                {
                    setreflb2.Content = cus.Where(x => x.SettlementFrom <= setrefdt2.SelectedDate && x.SettlementTo >= setrefdt2.SelectedDate).Select(x => x.SettlementRef).FirstOrDefault();
                }
            }
        }

        private void setref1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (setref1.SelectedIndex != -1)
            {
                setreflbl.Content = setref1.SelectedItem.ToString();
                comdchk.IsChecked = false; Currchk.IsChecked = false; srpchk.IsChecked = false; 
            }
        }

        private void setref2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (setref2.SelectedIndex != -1)
            {
                setreflb2.Content = setref2.SelectedItem.ToString();
                comdchk.IsChecked = false; Currchk.IsChecked = false; srpchk.IsChecked = false; 
            }
        }

        private async void srpchk_Checked(object sender, RoutedEventArgs e)
        {
            var context = new EconomicsTrackingDbContext();
            if (Custcombo.SelectedIndex != -1 && !string.IsNullOrEmpty(setreflbl.Content.ToString()) && !string.IsNullOrEmpty(setreflb2.Content.ToString()))
            {
                
                int i = 0;
                var sList = await context.BillOfMaterial.Include("Scrap").Where(s => li.Contains(s.CustAssyNo) && s.LocalPartName != string.Empty && s.LocalPartNo != string.Empty && s.Quantity != 0 && s.UOM != "").ToListAsync();
                string customer = Custcombo.SelectedItem.ToString();
                string Sett1 = setreflbl.Content.ToString();
                string Sett2 = setreflb2.Content.ToString();
                var comfromsettbl = context.Settlements.Where(x => x.SettlementRef == Sett1 && x.CustomerName == customer).Select(x => new { x.Scarp }).ToList();
                var comfromsettbl2 = context.Settlements.Where(x => x.SettlementRef == Sett2 && x.CustomerName == customer).Select(x => new { x.Scarp }).ToList();
                b = new BackgroundWorker();
                    b.DoWork+=((a,p) => { 
                        foreach (var r in li)
                        {
                            var comtemp = (from s in sList.Where(x=>x.CustAssyNo==r)
                                           select new
                                           {
                                               ScrapCommodity = s.Scarp,
                                               CustomerAssemblyNo = s.CustAssyNo,
                                               Totalrmqty = (s.RMUOM.ToLower() == "g" || s.RMUOM.ToLower() == "gm") ? s.Scraptotalqty / 100 : (s.RMUOM.ToLower() == "ml" || s.RMUOM == "m") ? s.Scraptotalqty / 1000 : s.Scraptotalqty
                                           }).ToList();
                            var comdity = (from s in comtemp
                                           group s by new { ScrapCommodity = s.ScrapCommodity } into d
                                           select new { ScrapCommodity = d.Key.ScrapCommodity, TotalRmqtybyComm = d.Sum(x => x.Totalrmqty), CusAss = r }).OrderBy(x => x.ScrapCommodity).ToList();
                            IEnumerable<SettlementScarp> Scarsett11 = comfromsettbl.SelectMany(x=>x.Scarp).ToList();
                            IEnumerable<SettlementScarp> Scarsett12 = comfromsettbl2.SelectMany(x => x.Scarp).ToList();
                            var fg = (from g in comdity
                                      join h in Scarsett11
                                     on g.ScrapCommodity.ToLower() equals h.ScrapName.ToLower()
                                      join k in Scarsett12
                                     on g.ScrapCommodity.ToLower() equals k.ScrapName.ToLower()
                                     select new { Recov=(h.Rate-k.Rate)*g.TotalRmqtybyComm }).ToList();
                            if (dt.Rows[i]["ScrapRecovery"].ToString() == null || dt.Rows[i]["ScrapRecovery"].ToString() == string.Empty)
                            {
                                dt.Rows[i]["ScrapRecovery"] = Math.Round(fg.Sum(x => x.Recov));
                            }
                            i++;
                        }
                        TotalColumnSum();b.Dispose();
                    });
                    if (b != null && b.IsBusy != true) { 
                      b.RunWorkerAsync();
                    } salesqtydatagrid.ItemsSource = dt.DefaultView;
            }
        }
        private void srpchk_Unchecked(object sender, RoutedEventArgs e)
        {
            //comdchk.IsChecked = false; Currchk.IsChecked = false; Ohchk.IsChecked = false;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["ScrapRecovery"] = string.Empty;
            }
            TotalColumnSum();
        }

        private async void Currchk_Checked(object sender, RoutedEventArgs e)
        {
            
            //comdchk.IsChecked = false; srpchk.IsChecked = false; Ohchk.IsChecked = false; Currchk.IsChecked = true;

            var context = new EconomicsTrackingDbContext();
            if (Custcombo.SelectedIndex != -1 && !string.IsNullOrEmpty(setreflbl.Content.ToString()) && !string.IsNullOrEmpty(setreflb2.Content.ToString()))
            {
                int i = 0;
                var sList =await context.BillOfMaterial.Include("Currency").Where(s => li.Contains(s.CustAssyNo)).ToListAsync();
                string Sett1 = setreflbl.Content.ToString();
                string Sett2 = setreflb2.Content.ToString();
                var comfromsettbl = await context.Settlements.Where(x => x.SettlementRef == Sett1 && x.CustomerName == Custcombo.SelectedItem.ToString()).Select(x => new { x.Currency }).ToListAsync();
                var comfromsettbl2 = await context.Settlements.Where(x => x.SettlementRef == Sett2 && x.CustomerName == Custcombo.SelectedItem.ToString()).Select(x => new { x.Currency }).ToListAsync();               
                foreach (var r in li.Distinct().ToList())
                {
                    var comtemp = (from s in sList.Where(x => x.CustAssyNo == r)
                                   select new
                                   {
                                       CurrencyCode = s.Currency.CurrencyName,
                                       CustomerAssemblyNo = s.CustAssyNo,
                                       CurrencySumValue = s.TotalcostinPurCurr
                                   }).ToList();
                    var comdity = (from s in comtemp
                                   group s by new { CurrencyCode = s.CurrencyCode } into d
                                   select new { CurrencyCode = d.Key.CurrencyCode, CurrencySumValue = d.Sum(x => x.CurrencySumValue), CusAss = r }).OrderBy(x => x.CurrencyCode).ToList();
                    IEnumerable<SettlementCurrency> h1 = comfromsettbl.SelectMany(x => x.Currency).ToList();
                    IEnumerable<SettlementCurrency> h2 = comfromsettbl2.SelectMany(x => x.Currency).ToList();
                    var cc = (from g in comdity
                              join h in h1
                              on g.CurrencyCode.ToLower() equals h.CurrencyCode.ToLower()
                              join d in h2
                              on g.CurrencyCode.ToLower() equals d.CurrencyCode.ToLower()
                              select new
                              {
                                  Recoverye = (h.Rate - d.Rate) * g.CurrencySumValue
                              }).ToList();
                    if (dt.Rows[i]["CurrencyRecovery"].ToString() == null || dt.Rows[i]["CurrencyRecovery"].ToString() == string.Empty)
                    {
                        dt.Rows[i]["CurrencyRecovery"] = Math.Round(cc.Sum(x => x.Recoverye));
                    }
                    i++;
                }
                Thread th=new Thread(TotalColumnSum);
                th.Start(); salesqtydatagrid.ItemsSource = dt.DefaultView;
            }
        }
        private void Curr()
        {
            var context = new EconomicsTrackingDbContext();
            if (Custcombo.SelectedIndex != -1 && !string.IsNullOrEmpty(setreflbl.Content.ToString()) && !string.IsNullOrEmpty(setreflb2.Content.ToString()))
            {
                int i = 0;
                var sList = context.BillOfMaterial.Include("Currency").Where(s => li.Contains(s.CustAssyNo)).ToList();
                string Sett1 = setreflbl.Content.ToString();
                string Sett2 = setreflb2.Content.ToString();

                var comfromsettbl = context.Settlements.Where(x => x.SettlementRef == Sett1 && x.CustomerName == Custcombo.SelectedItem.ToString()).Select(x => new { x.Currency }).ToList();
                var comfromsettbl2 = context.Settlements.Where(x => x.SettlementRef == Sett2 && x.CustomerName == Custcombo.SelectedItem.ToString()).Select(x => new { x.Currency }).ToList();
                foreach (var r in li.Distinct().ToList())
                {
                    var comtemp = (from s in sList.Where(x => x.CustAssyNo == r)
                                   select new
                                   {
                                       CurrencyCode = s.Currency.CurrencyName,
                                       CustomerAssemblyNo = s.CustAssyNo,
                                       CurrencySumValue = s.TotalcostinPurCurr
                                   }).ToList();
                    var comdity = (from s in comtemp
                                   group s by new { CurrencyCode = s.CurrencyCode } into d
                                   select new { CurrencyCode = d.Key.CurrencyCode, CurrencySumValue = d.Sum(x => x.CurrencySumValue), CusAss = r }).OrderBy(x => x.CurrencyCode).ToList();
                    IEnumerable<SettlementCurrency> h1 = comfromsettbl.SelectMany(x => x.Currency).ToList();
                    IEnumerable<SettlementCurrency> h2 = comfromsettbl2.SelectMany(x => x.Currency).ToList();
                    var cc = (from g in comdity
                              join h in h1
                              on g.CurrencyCode.ToLower() equals h.CurrencyCode.ToLower()
                              join d in h2
                              on g.CurrencyCode.ToLower() equals d.CurrencyCode.ToLower()
                              select new
                              {
                                  Recoverye = (h.Rate - d.Rate) * g.CurrencySumValue
                              }).ToList();
                    if (dt.Rows[i]["CurrencyRecovery"].ToString() == null || dt.Rows[i]["CurrencyRecovery"].ToString() == string.Empty)
                    {
                        dt.Rows[i]["CurrencyRecovery"] = Math.Round(cc.Sum(x => x.Recoverye));
                    }
                    i++;
                }
                TotalColumnSum();
            }
        }

        private void Currchk_Unchecked(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["CurrencyRecovery"] = string.Empty;
            }
            TotalColumnSum();
        }

        private void Ohchk_Checked(object sender, RoutedEventArgs e)
        {
            comdchk.IsChecked = false; Currchk.IsChecked = false; srpchk.IsChecked = false; Ohchk.IsChecked = true;
        }

        private void Ohchk_Unchecked(object sender, RoutedEventArgs e)
        {
            comdchk.IsChecked = false; Currchk.IsChecked = false; srpchk.IsChecked = false;
        }

        //private void chkcus_Loaded(object sender, RoutedEventArgs e)
        //{
        //    var context = new EconomicsTrackingDbContext();
        //    this.DataContext = context.CustomerAssembly.Where(x=>x.Customer==Custcombo.SelectedItem.ToString()).Select(x=>x.CustAssyNo).Distinct().ToList();
        //}

        private void chkcus_Checked(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.CheckBox p = (System.Windows.Controls.CheckBox)sender;
            if (!li.Contains(p.Content.ToString())) { 
            li.Add(p.Content.ToString());
            DataRow dr = dt.NewRow();
            dr["CustomerAssemblyNo"] = p.Content.ToString();
            dt.Rows.Add(dr);
            } dt.AcceptChanges();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["ID"] = i + 1;
            }
        }

        private void chkcus_Unchecked(object sender, RoutedEventArgs e)
        {
           System.Windows.Controls.CheckBox p = (System.Windows.Controls.CheckBox)sender;
           string s = p.Content.ToString();
           List<int> dtcount = new List<int>();
           if(li.Contains(s)){
               li.Remove(s);
               short j = 0;
                   foreach (DataRow dr in dt.Rows)
                   {
                       if (dr["CustomerAssemblyNo"] != null)
                       {
                           if (dr["CustomerAssemblyNo"].ToString() == s)
                           {
                               dtcount.Add(j);
                           }
                       } j++;
                   }
               foreach(var i in dtcount)
               {
                   dt.Rows[i].Delete();
               } dt.AcceptChanges();
           }
           for (int i = 0; i < dt.Rows.Count; i++)
           {
               dt.Rows[i]["ID"] = i + 1;
           } 
        }

        private async void Custcombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            li.Clear();
            comdchk.IsChecked = false; Currchk.IsChecked = false; srpchk.IsChecked = false; CusAssSelectAllchk.IsChecked = false;
            if (Custcombo.SelectedIndex != -1)
            {
                var context = new EconomicsTrackingDbContext();
                setref1.ItemsSource = await context.Settlements.Where(x => x.CustomerName == Custcombo.SelectedItem.ToString()).Select(x => x.SettlementRef).Distinct().ToListAsync();
                setref2.ItemsSource = await context.Settlements.Where(x => x.CustomerName == Custcombo.SelectedItem.ToString()).Select(x => x.SettlementRef).Distinct().ToListAsync();
                var cusass = await context.CustomerAssembly.Where(x => x.Customer == Custcombo.SelectedItem.ToString()).Select(x => new { x.CustAssyNo }).ToListAsync();
                lstcusass.ItemsSource = cusass;
                lstcusass.UnselectAll();;
                cusass.ToList().ForEach(x => litemp.Add(x.CustAssyNo)); dt.Clear();
                
            }
        }
        private void TotalColumnSum()
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["TotalRecovery"] = 0;
            }

            foreach (DataRow row in dt.Rows)
            {
                decimal rowSum = 0;
                foreach (DataColumn col in dt.Columns)
                {
                    if (col.ColumnName != "ID")
                    {
                        if (!row.IsNull(col))
                        {
                            string stringValue = row[col].ToString();
                            decimal d;
                            if (decimal.TryParse(stringValue, out d))
                                rowSum += d;
                        }
                    }
                }
                row.SetField("TotalRecovery", rowSum);
            }
        }

        private void CusAssSelectAllchk_Checked(object sender, RoutedEventArgs e)
        {
            li.Clear(); lstcusass.SelectAll();
            li = litemp.ToList(); dt.Clear();
            foreach(var r in li.Distinct().ToList()){
            DataRow dr = dt.NewRow();
            dr["CustomerAssemblyNo"] = r;
            dt.Rows.Add(dr);
            } dt.AcceptChanges();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["ID"] = i + 1;
            }
            StringBuilder d = new StringBuilder();
            li.ForEach(x => d.Append(x));
            Xceed.Wpf.Toolkit.MessageBox.Show(d.ToString());
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            var context = new EconomicsTrackingDbContext();
            if (Custcombo.SelectedIndex != -1 && !string.IsNullOrEmpty(setreflbl.Content.ToString()) && !string.IsNullOrEmpty(setreflb2.Content.ToString()))
            {
                //var ssList = context.BillOfMaterial.Where(s => li.Contains(s.CustAssyNo)).ToList();
                //var sList = ssList.Where(s => s.LocalPartName != string.Empty && s.LocalPartNo != string.Empty && s.Quantity != 0 && s.UOM != "").ToList();
                //li = li.Distinct().ToList();
                //int i = 0;
                //foreach (var r in li)
                //{
                //    var comtemp = (from s in sList
                //                   where s.CustAssyNo == r
                //                   select new
                //                   {
                //                       Commodity = s.Material.MaterialName,
                //                       Scarp = s.Scrap.ScrapName,
                //                       CustomerAssemblyNo = s.CustAssyNo,
                //                       Totalrmqty = (s.RMUOM.ToLower() == "g" || s.RMUOM == "gm") ? s.TotalRMqty / 100 : (s.RMUOM.ToLower() == "ml" || s.RMUOM == "m") ? s.TotalRMqty / 1000 : s.TotalRMqty,
                //                       TotalScraprmqty = (s.RMUOM.ToLower() == "g" || s.RMUOM == "gm") ? s.Scraptotalqty / 100 : (s.RMUOM.ToLower() == "ml" || s.RMUOM == "m") ? s.Scraptotalqty / 1000 : s.Scraptotalqty
                //                   }).ToList();
                //    var comdity = (from s in comtemp
                //                   group s by new { Commodity = s.Commodity } into d
                //                   select new { Commodity = d.Key.Commodity, TotalRmqtybyComm = d.Sum(x => x.Totalrmqty), CusAss = r }).OrderBy(x => x.Commodity).ToList();
                //    var Scrap = (from s in comtemp
                //                 group s by new { Commodity = s.Scarp } into d
                //                 select new { Commodity = d.Key.Commodity, TotalScraprmqty = d.Sum(x => x.TotalScraprmqty), CusAss = r }).OrderBy(x => x.Commodity).ToList();
                //    var Currency = (from s in ssList
                //                    group s by new { Curr = s.Currency.CurrencyName } into d
                //                    select new { CurrencyCode = d.Key.Curr, TotalinINR = d.Sum(x => x.ToalCost), TotalinPurCurrency = d.Sum(x => x.TotalcostinPurCurr), CusAss = r }).OrderBy(x => x.CurrencyCode).ToList();
                //    var comfromsettbl = context.Settlements.Where(x => x.SettlementRef == setreflbl.Content.ToString() && x.CustomerName == Custcombo.SelectedItem.ToString()).Select(x => new { x.Commodity, x.Scarp, x.Currency }).ToList();
                //    var comfromsettbl2 = context.Settlements.Where(x => x.SettlementRef == setreflb2.Content.ToString() && x.CustomerName == Custcombo.SelectedItem.ToString()).Select(x => new { x.Commodity, x.Scarp, x.Currency }).ToList();
                //    var comtt1 = new List<SettlementCommodity>();
                //    var comtt2 = new List<SettlementCommodity>();
                //    var scrapcomtt1 = new List<SettlementScarp>();
                //    var scrapcomtt2 = new List<SettlementScarp>();
                //    var currt1 = new List<SettlementCurrency>();
                //    var currt2 = new List<SettlementCurrency>();
                //    foreach (var r1 in comfromsettbl)
                //    {
                //        foreach (var f in r1.Commodity)
                //        {
                //            var comm = new SettlementCommodity();
                //            comm.MaterialName = f.MaterialName;
                //            comm.Rate = f.Rate;
                //            comtt1.Add(comm);
                //        }
                //    }
                //    foreach (var r2 in comfromsettbl2)
                //    {
                //        foreach (var f in r2.Commodity)
                //        {
                //            var comm = new SettlementCommodity();
                //            comm.MaterialName = f.MaterialName;
                //            comm.Rate = f.Rate;
                //            comtt2.Add(comm);
                //        }
                //    }
                //    foreach (var r1 in comfromsettbl)
                //    {
                //        foreach (var f in r1.Scarp)
                //        {
                //            var comm = new SettlementScarp();
                //            comm.ScrapName = f.ScrapName;
                //            comm.Rate = f.Rate;
                //            scrapcomtt1.Add(comm);
                //        }
                //    }
                //    foreach (var r2 in comfromsettbl2)
                //    {
                //        foreach (var f in r2.Scarp)
                //        {
                //            var comm = new SettlementScarp();
                //            comm.ScrapName = f.ScrapName;
                //            comm.Rate = f.Rate;
                //            scrapcomtt1.Add(comm);
                //        }
                //    }
                //    foreach (var r1 in comfromsettbl)
                //    {
                //        foreach (var f in r1.Currency)
                //        {
                //            var comm = new SettlementCurrency();
                //            comm.CurrencyCode = f.CurrencyCode;
                //            comm.Rate = f.Rate;
                //            currt1.Add(comm);
                //        }
                //    }
                //    foreach (var r2 in comfromsettbl2)
                //    {
                //        foreach (var f in r2.Currency)
                //        {
                //            var comm = new SettlementCurrency();
                //            comm.CurrencyCode = f.CurrencyCode;
                //            comm.Rate = f.Rate;
                //            currt2.Add(comm);
                //        }
                //    }
                //    var cc = from g in comdity
                //             join h in comtt1
                //                on g.Commodity.ToLower() equals h.MaterialName.ToLower()
                //             join d in comtt2
                //            on g.Commodity.ToLower() equals d.MaterialName.ToLower()
                //             select new
                //            {
                //                CommodityName = g.Commodity,
                //                CommodityWtKG = g.TotalRmqtybyComm,
                //                SetPrice1 = h.Rate,
                //                SetPrice2 = d.Rate,
                //                Recovery = (h.Rate - d.Rate) * g.TotalRmqtybyComm
                //            };
                //    var sc = from g in Scrap
                //             join h in scrapcomtt1
                //                on g.Commodity.ToLower() equals h.ScrapName.ToLower()
                //             join d in scrapcomtt2
                //            on g.Commodity.ToLower() equals d.ScrapName.ToLower()
                //             select new
                //             {
                //                 CommodityName = g.Commodity,
                //                 CommodityWtKG = g.TotalScraprmqty,
                //                 SetPrice1 = h.Rate,
                //                 SetPrice2 = d.Rate,
                //                 Recovery = (h.Rate - d.Rate) * g.TotalScraprmqty
                //             };
                //    //var curr = from g in Currency
                //    //           join h in currt1
                //    //            on g.CurrencyCode.ToLower() equals h.CurrencyCode.ToLower()
                //    //           join d in currt2
                //    //        on g.CurrencyCode.ToLower() equals d.CurrencyCode.ToLower()
                //    //         select new
                //    //         {
                //    //             ScarpCommodityName = g.TotalScraprmqty,
                //    //             CommodityWtKG = g.TotalScraprmqty,
                //    //             SetPrice1 = h.Rate,
                //    //             SetPrice2 = d.Rate,
                //    //             Recovery = (h.Rate - d.Rate) * g.TotalScraprmqty
                //    //         };



                //}
                BackgroundWorker worker;
                System.Windows.Forms.SaveFileDialog saveFileDialog = new System.Windows.Forms.SaveFileDialog();
                saveFileDialog.Filter = "Excel file (*.xls)|*.xls|Excel Files (*.xlsx)|*.xlsx";
                saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    worker = new BackgroundWorker();
                    worker.DoWork += (a, b) =>
                    {
                        dt.ExportToExcel(saveFileDialog.FileName);
                    };
                    worker.RunWorkerAsync();
                }
            }
        }

        private void CusAssSelectAllchk_Unchecked(object sender, RoutedEventArgs e)
        {
            li.Clear(); lstcusass.UnselectAll(); dt.Clear();
        }

    }
}
