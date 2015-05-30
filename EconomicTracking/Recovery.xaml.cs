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

namespace EconomicTracking
{
    /// <summary>
    /// Interaction logic for Recovery.xaml
    /// </summary>
    public partial class Recovery : UserControl
    {
        DataTable dt = new DataTable();
        List<string> li = new List<string>();
        DataRow dr = null;

        public Recovery()
        {
            InitializeComponent();
            //, typeof(System.Decimal)
            DataColumn dc = new DataColumn("ID", typeof(System.Int32));
            DataColumn dc1 = new DataColumn("TotalRecovery");
            dc.AutoIncrement = true;
            dc.AutoIncrementSeed = 1; dc.ReadOnly = false;
            dc.Unique = true; dt.Columns.Add(dc);
            dt.Columns.Add("CustomerAssemblyNo");
            dt.Columns.Add("ScrapRecovery");
            dt.Columns.Add("CurrencyRecovery");
            dt.Columns.Add("CommodityRecovery");
            dt.Columns.Add("OverHeadRecovery");
            dt.Columns.Add("TotalRecovery", typeof(decimal));
            //dt.PrimaryKey =new[] {dc};
            //dt.Columns.Add(dc1);
            li.Add("CusAssy_498");
            li.Add("CusAssy_499");
            li.Add("CusAssy_500");
            foreach (var r in li)
            {
                DataRow dr = dt.NewRow();
                dr["CustomerAssemblyNo"] = r;
                dt.Rows.Add(dr);
            }
        }

        private void comdchk_Checked(object sender, RoutedEventArgs e)
        {
            var context = new EconomicsTrackingDbContext();
            if (Custcombo.SelectedIndex != -1 && !string.IsNullOrEmpty(setreflbl.Content.ToString()) && !string.IsNullOrEmpty(setreflb2.Content.ToString()))
            {
                var ssList = context.BillOfMaterial.Where(s => li.Contains(s.CustAssyNo)).ToList();
                var sList = ssList.Where(s => s.LocalPartName != string.Empty && s.LocalPartNo != string.Empty && s.Quantity != 0 && s.UOM != "").ToList();
                int i = 0;
                li = li.Distinct().ToList();
                foreach (var r in li)
                {
                    var comtemp = (from s in sList
                                   where s.CustAssyNo == r
                                   select new
                                   {
                                       Commodity = s.Commodity,
                                       CustomerAssemblyNo = s.CustAssyNo,
                                       Totalrmqty = (s.RMUOM.ToLower() == "g" || s.RMUOM == "gm") ? s.TotalRMqty / 100 : (s.RMUOM.ToLower() == "ml" || s.RMUOM == "m") ? s.TotalRMqty / 1000 : s.TotalRMqty
                                   }).ToList();
                    var comdity = (from s in comtemp
                                   group s by new { Commodity = s.Commodity } into d
                                   select new { Commodity = d.Key.Commodity, TotalRmqtybyComm = d.Sum(x => x.Totalrmqty), CusAss = r }).OrderBy(x => x.Commodity).ToList();
                    var comfromsettbl = context.Settlements.Where(x => x.SettlementRef == setreflbl.Content.ToString() && x.CustomerName == Custcombo.SelectedItem.ToString()).Select(x => new { x.Commodity }).ToList();
                    var comfromsettbl2 = context.Settlements.Where(x => x.SettlementRef == setreflb2.Content.ToString() && x.CustomerName == Custcombo.SelectedItem.ToString()).Select(x => new { x.Commodity }).ToList();
                    var comtt1 = new List<SettlementCommodity>();
                    var comtt2 = new List<SettlementCommodity>();
                    foreach (var r1 in comfromsettbl)
                    {
                        foreach (var f in r1.Commodity)
                        {
                            var comm = new SettlementCommodity();
                            comm.MaterialName = f.MaterialName;
                            comm.Rate = f.Rate;
                            comtt1.Add(comm);
                        }
                    }
                    foreach (var r2 in comfromsettbl2)
                    {
                        foreach (var f in r2.Commodity)
                        {
                            var comm = new SettlementCommodity();
                            comm.MaterialName = f.MaterialName;
                            comm.Rate = f.Rate;
                            comtt2.Add(comm);
                        }
                    }
                    var cc = from g in comdity
                             join h in comtt1
                                on g.Commodity.ToLower() equals h.MaterialName.ToLower()
                             join d in comtt2
                            on g.Commodity.ToLower() equals d.MaterialName.ToLower()
                             select new
                            {
                                CommodityName = g.Commodity,
                                CommodityWtKG = g.TotalRmqtybyComm,
                                SetPrice1 = h.Rate,
                                SetPrice2 = d.Rate,
                                Recoverye = (h.Rate - d.Rate) * g.TotalRmqtybyComm
                            };
                    if (dt.Rows[i]["CommodityRecovery"].ToString() == null || dt.Rows[i]["CommodityRecovery"].ToString() == string.Empty)
                    {
                        dt.Rows[i]["CommodityRecovery"] = Math.Round(cc.Sum(x => x.Recoverye));

                    }
                    i++;
                }
                TotalColumnSum();
                salesqtydatagrid.ItemsSource = dt.DefaultView;
                //salesqtydatagrid.ItemsSource = dt.AsEnumerable().Select(x => new { CustomerAssemblyNo = x["CustomerAssemblyNo"].ToString(), CommodityRecovery = x["CommodityRecovery"].ToString() });
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

        private void Custcombo_Loaded(object sender, RoutedEventArgs e)
        {
            var context = new EconomicsTrackingDbContext();
            Custcombo.ItemsSource = context.Settlements.Select(x => x.CustomerName).Distinct().ToList();
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
            }
        }

        private void setref2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (setref2.SelectedIndex != -1)
            {
                setreflb2.Content = setref2.SelectedItem.ToString();
            }
        }

        private void srpchk_Checked(object sender, RoutedEventArgs e)
        {
            var context = new EconomicsTrackingDbContext();
            if (Custcombo.SelectedIndex != -1 && !string.IsNullOrEmpty(setreflbl.Content.ToString()) && !string.IsNullOrEmpty(setreflb2.Content.ToString()))
            {
                int i = 0;
                var sList = context.BillOfMaterial.Where(s => li.Contains(s.CustAssyNo) && s.LocalPartName != string.Empty && s.LocalPartNo != string.Empty && s.Quantity != 0 && s.UOM != "").ToList();
                foreach (var r in li)
                {
                    var comtemp = (from s in sList
                                   where s.CustAssyNo == r
                                   select new
                                   {
                                       ScrapCommodity = s.Scarp,
                                       CustomerAssemblyNo = s.CustAssyNo,
                                       Totalrmqty = (s.RMUOM.ToLower() == "g" || s.RMUOM == "gm") ? s.Scraptotalqty / 100 : (s.RMUOM.ToLower() == "ml" || s.RMUOM == "m") ? s.Scraptotalqty / 1000 : s.Scraptotalqty
                                   }).ToList();
                    var comdity = (from s in comtemp
                                   group s by new { ScrapCommodity = s.ScrapCommodity } into d
                                   select new { ScrapCommodity = d.Key.ScrapCommodity, TotalRmqtybyComm = d.Sum(x => x.Totalrmqty), CusAss = r }).OrderBy(x => x.ScrapCommodity).ToList();
                    var comfromsettbl = context.Settlements.Where(x => x.SettlementRef == setreflbl.Content.ToString() && x.CustomerName == Custcombo.SelectedItem.ToString()).Select(x => new { x.Scarp }).ToList();
                    var comfromsettbl2 = context.Settlements.Where(x => x.SettlementRef == setreflb2.Content.ToString() && x.CustomerName == Custcombo.SelectedItem.ToString()).Select(x => new { x.Scarp }).ToList();
                    var comtt1 = new List<SettlementScarp>();
                    var comtt2 = new List<SettlementScarp>();
                    foreach (var r1 in comfromsettbl)
                    {
                        foreach (var f in r1.Scarp)
                        {
                            var comm = new SettlementScarp();
                            comm.ScrapName = f.ScrapName;
                            comm.Rate = f.Rate;
                            comtt1.Add(comm);
                        }
                    }
                    foreach (var r2 in comfromsettbl2)
                    {
                        foreach (var f in r2.Scarp)
                        {
                            var comm = new SettlementScarp();
                            comm.ScrapName = f.ScrapName;
                            comm.Rate = f.Rate;
                            comtt2.Add(comm);
                        }
                    }
                    var cc = from g in comdity
                             join h in comtt1
                                on g.ScrapCommodity.ToLower() equals h.ScrapName.ToLower()
                             join d in comtt2
                            on g.ScrapCommodity.ToLower() equals d.ScrapName.ToLower()
                             select new
                            {
                                ScrapCommodityName = g.ScrapCommodity,
                                ScrapWtKG = g.TotalRmqtybyComm,
                                SetPrice1 = h.Rate,
                                SetPrice2 = d.Rate,
                                Recoverye = (h.Rate - d.Rate) * g.TotalRmqtybyComm
                            };
                    if (dt.Rows[i]["ScrapRecovery"].ToString() == null || dt.Rows[i]["ScrapRecovery"].ToString() == string.Empty)
                    {

                        dt.Rows[i]["ScrapRecovery"] = Math.Round(cc.Sum(x => x.Recoverye));

                    }
                    i++;
                }
                TotalColumnSum();
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

        private void Currchk_Checked(object sender, RoutedEventArgs e)
        {
            //comdchk.IsChecked = false; srpchk.IsChecked = false; Ohchk.IsChecked = false; Currchk.IsChecked = true;

            var context = new EconomicsTrackingDbContext();
            if (Custcombo.SelectedIndex != -1 && !string.IsNullOrEmpty(setreflbl.Content.ToString()) && !string.IsNullOrEmpty(setreflb2.Content.ToString()))
            {
                int i = 0;
                var sList = context.BillOfMaterial.Where(s => li.Contains(s.CustAssyNo)).ToList();
                foreach (var r in li)
                {
                    var comtemp = (from s in sList
                                   where s.CustAssyNo == r
                                   select new
                                   {
                                       //ScrapCommodity = s.Scarp,
                                       //Commodity = s.Commodity,
                                       CurrencyCode = s.CurrencyCode,
                                       CustomerAssemblyNo = s.CustAssyNo,
                                       CurrencySumValue = s.TotalcostinPurCurr
                                   }).ToList();
                    var comdity = (from s in comtemp
                                   group s by new { CurrencyCode = s.CurrencyCode } into d
                                   select new { CurrencyCode = d.Key.CurrencyCode, CurrencySumValue = d.Sum(x => x.CurrencySumValue), CusAss = r }).OrderBy(x => x.CurrencyCode).ToList();
                    var comfromsettbl = context.Settlements.Where(x => x.SettlementRef == setreflbl.Content.ToString() && x.CustomerName == Custcombo.SelectedItem.ToString()).Select(x => new { x.Currency }).ToList();
                    var comfromsettbl2 = context.Settlements.Where(x => x.SettlementRef == setreflb2.Content.ToString() && x.CustomerName == Custcombo.SelectedItem.ToString()).Select(x => new { x.Currency }).ToList();
                    var comtt1 = new List<SettlementCurrency>();
                    var comtt2 = new List<SettlementCurrency>();
                    foreach (var r1 in comfromsettbl)
                    {
                        foreach (var f in r1.Currency)
                        {
                            var comm = new SettlementCurrency();
                            comm.CurrencyCode = f.CurrencyCode;
                            comm.Rate = f.Rate;
                            comtt1.Add(comm);
                        }
                    }
                    foreach (var r2 in comfromsettbl2)
                    {
                        foreach (var f in r2.Currency)
                        {
                            var comm = new SettlementCurrency();
                            comm.CurrencyCode = f.CurrencyCode;
                            comm.Rate = f.Rate;
                            comtt2.Add(comm);
                        }
                    }
                    var cc = from g in comdity
                             join h in comtt1
                                on g.CurrencyCode.ToLower() equals h.CurrencyCode.ToLower()
                             join d in comtt2
                            on g.CurrencyCode.ToLower() equals d.CurrencyCode.ToLower()
                             select new
                            {
                                CurrencyCode = g.CurrencyCode,
                                CurrenctTotal = g.CurrencySumValue,
                                SetPrice1 = h.Rate,
                                SetPrice2 = d.Rate,
                                Recoverye = (h.Rate - d.Rate) * g.CurrencySumValue
                            };
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

        private void chkcus_Loaded(object sender, RoutedEventArgs e)
        {
            var context = new EconomicsTrackingDbContext();
            this.DataContext = context.BillOfMaterial.Select(x => x.CustAssyNo).ToList();
        }

        private void chkcus_Checked(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.CheckBox p = (System.Windows.Controls.CheckBox)sender;
            li.Add(p.Content.ToString());
        }

        private void chkcus_Unchecked(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.CheckBox p = (System.Windows.Controls.CheckBox)sender;
            li.Remove(p.Content.ToString());
        }

        private void Custcombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Custcombo.SelectedIndex != -1)
            {
                var context = new EconomicsTrackingDbContext();
                setref1.ItemsSource = context.Settlements.Where(x => x.CustomerName == Custcombo.SelectedItem.ToString()).Select(x => x.SettlementRef).Distinct().ToList();
                setref2.ItemsSource = context.Settlements.Where(x => x.CustomerName == Custcombo.SelectedItem.ToString()).Select(x => x.SettlementRef).Distinct().ToList();
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
            //dt.Rows.Cast<DataRow>().Where(
            //r => r.ItemArray[0] == someValue).ToList().ForEach(r => r.Delete());

            int i = 0;
            while (i < dt.Rows.Count)
            {
                if (dt.Rows[i]["CustomerAssemblyNo"].ToString() == "CusAssy_499")
                {
                    dt.Rows.RemoveAt(i);
                }
                else
                    i++;
            }
            li.Remove("CusAssy_499");


            //dr.AcceptChanges();
            dt.AcceptChanges();
            //comdchk_Checked(null, null);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            var context = new EconomicsTrackingDbContext();
            if (Custcombo.SelectedIndex != -1 && !string.IsNullOrEmpty(setreflbl.Content.ToString()) && !string.IsNullOrEmpty(setreflb2.Content.ToString()))
            {
                var ssList = context.BillOfMaterial.Where(s => li.Contains(s.CustAssyNo)).ToList();
                var sList = ssList.Where(s => s.LocalPartName != string.Empty && s.LocalPartNo != string.Empty && s.Quantity != 0 && s.UOM != "").ToList();
                li = li.Distinct().ToList();
                int i = 0;
                foreach (var r in li)
                {
                    var comtemp = (from s in sList
                                   where s.CustAssyNo == r
                                   select new
                                   {
                                       Commodity = s.Commodity,
                                       Scarp = s.Scarp,
                                       CustomerAssemblyNo = s.CustAssyNo,
                                       Totalrmqty = (s.RMUOM.ToLower() == "g" || s.RMUOM == "gm") ? s.TotalRMqty / 100 : (s.RMUOM.ToLower() == "ml" || s.RMUOM == "m") ? s.TotalRMqty / 1000 : s.TotalRMqty,
                                       TotalScraprmqty = (s.RMUOM.ToLower() == "g" || s.RMUOM == "gm") ? s.Scraptotalqty / 100 : (s.RMUOM.ToLower() == "ml" || s.RMUOM == "m") ? s.Scraptotalqty / 1000 : s.Scraptotalqty
                                   }).ToList();
                    var comdity = (from s in comtemp
                                   group s by new { Commodity = s.Commodity } into d
                                   select new { Commodity = d.Key.Commodity, TotalRmqtybyComm = d.Sum(x => x.Totalrmqty), CusAss = r }).OrderBy(x => x.Commodity).ToList();
                    var Scrap = (from s in comtemp
                                 group s by new { Commodity = s.Scarp } into d
                                 select new { Commodity = d.Key.Commodity, TotalScraprmqty = d.Sum(x => x.TotalScraprmqty), CusAss = r }).OrderBy(x => x.Commodity).ToList();
                    var Currency = (from s in ssList
                                    group s by new { Curr = s.CurrencyCode } into d
                                    select new { CurrencyCode = d.Key.Curr, TotalinINR = d.Sum(x => x.ToalCost), TotalinPurCurrency = d.Sum(x => x.TotalcostinPurCurr), CusAss = r }).OrderBy(x => x.CurrencyCode).ToList();
                    var comfromsettbl = context.Settlements.Where(x => x.SettlementRef == setreflbl.Content.ToString() && x.CustomerName == Custcombo.SelectedItem.ToString()).Select(x => new { x.Commodity, x.Scarp, x.Currency }).ToList();
                    var comfromsettbl2 = context.Settlements.Where(x => x.SettlementRef == setreflb2.Content.ToString() && x.CustomerName == Custcombo.SelectedItem.ToString()).Select(x => new { x.Commodity, x.Scarp, x.Currency }).ToList();
                    var comtt1 = new List<SettlementCommodity>();
                    var comtt2 = new List<SettlementCommodity>();
                    var scrapcomtt1 = new List<SettlementScarp>();
                    var scrapcomtt2 = new List<SettlementScarp>();
                    var currt1 = new List<SettlementCurrency>();
                    var currt2 = new List<SettlementCurrency>();
                    foreach (var r1 in comfromsettbl)
                    {
                        foreach (var f in r1.Commodity)
                        {
                            var comm = new SettlementCommodity();
                            comm.MaterialName = f.MaterialName;
                            comm.Rate = f.Rate;
                            comtt1.Add(comm);
                        }
                    }
                    foreach (var r2 in comfromsettbl2)
                    {
                        foreach (var f in r2.Commodity)
                        {
                            var comm = new SettlementCommodity();
                            comm.MaterialName = f.MaterialName;
                            comm.Rate = f.Rate;
                            comtt2.Add(comm);
                        }
                    }
                    foreach (var r1 in comfromsettbl)
                    {
                        foreach (var f in r1.Scarp)
                        {
                            var comm = new SettlementScarp();
                            comm.ScrapName = f.ScrapName;
                            comm.Rate = f.Rate;
                            scrapcomtt1.Add(comm);
                        }
                    }
                    foreach (var r2 in comfromsettbl2)
                    {
                        foreach (var f in r2.Scarp)
                        {
                            var comm = new SettlementScarp();
                            comm.ScrapName = f.ScrapName;
                            comm.Rate = f.Rate;
                            scrapcomtt1.Add(comm);
                        }
                    }
                    foreach (var r1 in comfromsettbl)
                    {
                        foreach (var f in r1.Currency)
                        {
                            var comm = new SettlementCurrency();
                            comm.CurrencyCode = f.CurrencyCode;
                            comm.Rate = f.Rate;
                            currt1.Add(comm);
                        }
                    }
                    foreach (var r2 in comfromsettbl2)
                    {
                        foreach (var f in r2.Currency)
                        {
                            var comm = new SettlementCurrency();
                            comm.CurrencyCode = f.CurrencyCode;
                            comm.Rate = f.Rate;
                            currt2.Add(comm);
                        }
                    }
                    var cc = from g in comdity
                             join h in comtt1
                                on g.Commodity.ToLower() equals h.MaterialName.ToLower()
                             join d in comtt2
                            on g.Commodity.ToLower() equals d.MaterialName.ToLower()
                             select new
                            {
                                CommodityName = g.Commodity,
                                CommodityWtKG = g.TotalRmqtybyComm,
                                SetPrice1 = h.Rate,
                                SetPrice2 = d.Rate,
                                Recovery = (h.Rate - d.Rate) * g.TotalRmqtybyComm
                            };
                    var sc = from g in Scrap
                             join h in scrapcomtt1
                                on g.Commodity.ToLower() equals h.ScrapName.ToLower()
                             join d in scrapcomtt2
                            on g.Commodity.ToLower() equals d.ScrapName.ToLower()
                             select new
                             {
                                 CommodityName = g.Commodity,
                                 CommodityWtKG = g.TotalScraprmqty,
                                 SetPrice1 = h.Rate,
                                 SetPrice2 = d.Rate,
                                 Recovery = (h.Rate - d.Rate) * g.TotalScraprmqty
                             };
                    //var curr = from g in Currency
                    //           join h in currt1
                    //            on g.CurrencyCode.ToLower() equals h.CurrencyCode.ToLower()
                    //           join d in currt2
                    //        on g.CurrencyCode.ToLower() equals d.CurrencyCode.ToLower()
                    //         select new
                    //         {
                    //             ScarpCommodityName = g.TotalScraprmqty,
                    //             CommodityWtKG = g.TotalScraprmqty,
                    //             SetPrice1 = h.Rate,
                    //             SetPrice2 = d.Rate,
                    //             Recovery = (h.Rate - d.Rate) * g.TotalScraprmqty
                    //         };



                }

            }
        }

    }
}
