using EconomicTracking.Dal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using WPFTextBoxAutoComplete;
namespace EconomicTracking
{
    /// <summary>
    /// Interaction logic for EditSettlement.xaml
    /// </summary>
    public partial class EditSettlement : UserControl
    {
        DataTable dt = new DataTable();
        DataTable dt1 = new DataTable();
        DataTable dt2 = new DataTable();
        List<SettlementCommodity> li = new List<SettlementCommodity>();
        int settid;
        string customerFilter = "";
        EconomicsTrackingDbContext context = new EconomicsTrackingDbContext();
        public EditSettlement()
        {
            InitializeComponent();
            dt.Columns.Add("MaterialName"); dt.Columns.Add("Rate"); dt.Columns.Add("Id");
            dt1.Columns.Add("ScrapName"); dt1.Columns.Add("Rate"); dt1.Columns.Add("Id");
            dt2.Columns.Add("Id"); dt2.Columns.Add("CurrencyCode"); dt2.Columns.Add("Rate");
        }

        private void cbmCustomer_Loaded(object sender, RoutedEventArgs e)
        {
            var cus1 = context.Settlements.Select(x => x.CustomerName).Distinct().ToList();
            {
               
                cbmCustomer.ItemsSource = cus1;
            }
        }

        private void cbmCustomer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbmCustomer.SelectedIndex != -1)
            {
                var cus = context.Settlements.Where(x => x.CustomerName == cbmCustomer.SelectedItem.ToString()).Select(x => x.SettlementRef).Distinct().ToList();
                {
                    cbmSettlement.ItemsSource = cus.ToList();
                    var s = Visibility.Hidden;
                    Createbtn.Visibility = s; Savebtn.Visibility = s; SettmentGrid.Visibility = s; Commoditygrid.Visibility = s; Scarpgrid.Visibility = s; CurrencyGrid.Visibility = s;
                }
            }
        }
        private void SettmentGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Settlement objsettoedit = SettmentGrid.SelectedItem as Settlement;
        }

        private void SettmentGrid_RowEditEnding(object sender, Microsoft.Windows.Controls.DataGridRowEditEndingEventArgs e)
        {
            EconomicsTrackingDbContext con=new EconomicsTrackingDbContext();
            var ss = con.Settlements.Where(x => x.Id == settid).FirstOrDefault();
            Settlement sett = e.Row.DataContext as Settlement;
            ss.CustomerId = sett.CustomerId;
            ss.CustomerName = sett.CustomerName;
            ss.SettlementFrom = sett.SettlementFrom;
            ss.SettlementRef = sett.SettlementRef;
            ss.SettlementTo = sett.SettlementTo;
            //li.Add(ss);
             MessageBoxResult res = MessageBox.Show("Do u want to save the changes made to settlement?", "Confirmation!", MessageBoxButton.YesNo, MessageBoxImage.Question);
             if (res == MessageBoxResult.Yes)
             {
                 con.SaveChanges();
             }
        }

        private void Commoditygrid_RowEditEnding(object sender, Microsoft.Windows.Controls.DataGridRowEditEndingEventArgs e)
        {
            var c = context.Settlements.Include("Commodity").Where(x => x.CustomerName == cbmCustomer.SelectedItem.ToString() && x.SettlementRef == cbmSettlement.SelectedItem.ToString()).ToList();
            SettlementCommodity sett = e.Row.DataContext as SettlementCommodity;
            if (c.Count > 0) {
             var ra = context.SettlementCommodities.Where(x => x.Id == sett.Id).FirstOrDefault();
            //var temp = c.Select(x => x.Commodity).ToList();
            //var ss = temp.Select(x => x.Where(y=>y.Id==sett.Id)).FirstOrDefault();
            //var r=ss.Select(x=>x).FirstOrDefault();
            ra.MaterialName = sett.MaterialName;
            ra.Rate = sett.Rate;
            li.Add(ra);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

            bool HasValidationError = false;
            for (int i = 0; i < Commoditygrid.Items.Count; i++)
            {
                Microsoft.Windows.Controls.DataGridRow row = EditSettlement.GetRow(Commoditygrid, i);
                if (row != null && Validation.GetHasError(row))
                {
                    HasValidationError = true;
                    break;
                }
            }
            for (int i = 0; i < Scarpgrid.Items.Count; i++)
            {
                Microsoft.Windows.Controls.DataGridRow row = EditSettlement.GetRow(Scarpgrid, i);
                if (row != null && Validation.GetHasError(row))
                {
                    HasValidationError = true;
                    break;
                }
            }
            if (HasValidationError)
            {
                MessageBox.Show("Enter Valid data before Saving");
            }
            else
            {
                //var con = context.Settlements.Where(x => x.Id == settid).ToList();
                context.SaveChanges();
                MessageBox.Show("Changes Saved Sucessfully");
            }
        }
        public static Microsoft.Windows.Controls.DataGridRow GetRow(Microsoft.Windows.Controls.DataGrid grid, int index)
        {
            Microsoft.Windows.Controls.DataGridRow row = (Microsoft.Windows.Controls.DataGridRow)grid.ItemContainerGenerator.ContainerFromIndex(index);
            if (row == null)
            {
                // May be virtualized, bring into view and try again.
                grid.UpdateLayout();
                grid.ScrollIntoView(grid.Items[index]);
                row = (Microsoft.Windows.Controls.DataGridRow)grid.ItemContainerGenerator.ContainerFromIndex(index);
            }
            return row;
        }

        private void Commoditygrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            
            if (e.Device.Target.GetType().Name == "DataGridCell")
            {
                if (e.Key == Key.Delete)
                {
                    SettlementCommodity com = this.Commoditygrid.SelectedItem as SettlementCommodity;
                    var r = context.SettlementCommodities.FirstOrDefault(x => x.Id == com.Id);
                    SettlementCommodity f = r as SettlementCommodity;
                    MessageBoxResult res = MessageBox.Show("Are you sure want to delete?", "Confirmation!", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    e.Handled = (res == MessageBoxResult.No);
                    if (res == MessageBoxResult.Yes)
                    {
                        context.SettlementCommodities.Remove(f);
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            if (Convert.ToInt32(dt.Rows[i]["Id"]) == com.Id)
                            {
                                dt.Rows[i].Delete(); dt.AcceptChanges();
                            }
                        }
                        context.SaveChanges();
                        Commoditygrid.ItemsSource = dt.DataTableToList<SettlementCommodity>();
                    }
                }
            }
            
        }
        private void Scarpgrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Device.Target.GetType().Name == "DataGridCell")
            {
                if (e.Key == Key.Delete)
                {
                    SettlementScarp com = this.Scarpgrid.SelectedItem as SettlementScarp;
                    var r = context.SettlementScraps.FirstOrDefault(x => x.Id == com.Id);
                    SettlementScarp f = r as SettlementScarp;
                    MessageBoxResult res = MessageBox.Show("Are you sure want to delete?", "Confirmation!", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    e.Handled = (res == MessageBoxResult.No);
                    if (res == MessageBoxResult.Yes)
                    {
                        context.SettlementScraps.Remove(f);
                        for (int i = 0; i < dt1.Rows.Count; i++)
                        {
                            if (Convert.ToInt32(dt1.Rows[i]["Id"]) == com.Id)
                            {
                                dt1.Rows[i].Delete(); dt1.AcceptChanges();
                            }
                        }
                        context.SaveChanges();
                        Scarpgrid.ItemsSource = dt1.DataTableToList<SettlementScarp>();
                    }
                }
            }
            
        }

        private void Scarpgrid_RowEditEnding(object sender, Microsoft.Windows.Controls.DataGridRowEditEndingEventArgs e)
        {

            var c = context.Settlements.Include("Scarp").Where(x => x.CustomerName == cbmCustomer.SelectedItem.ToString() && x.SettlementRef == cbmSettlement.SelectedItem.ToString()).ToList();
            SettlementScarp sett = e.Row.DataContext as SettlementScarp;
            List<SettlementScarp> li = new List<SettlementScarp>();
            var ss = context.SettlementScraps.Where(x => x.Id == sett.Id).FirstOrDefault();
            ss.ScrapName = sett.ScrapName;
            ss.Rate = sett.Rate;
            li.Add(ss);
        }



        private void CurrencyGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        { 
            if (e.Device.Target.GetType().Name == "DataGridCell")
            {
                if (e.Key == Key.Delete)
                {
                    SettlementCurrency com = this.CurrencyGrid.SelectedItem as SettlementCurrency;
                    var r = context.SettlementCurrencies.FirstOrDefault(x => x.Id == com.Id);
                    SettlementCurrency f = r as SettlementCurrency;
                    MessageBoxResult res = MessageBox.Show("Are you sure want to delete?", "Confirmation!", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    e.Handled = (res == MessageBoxResult.No);
                    if (res == MessageBoxResult.Yes)
                    {
                        context.SettlementCurrencies.Remove(f);
                        for (int i = 0; i < dt2.Rows.Count; i++)
                        {
                            if (Convert.ToInt32(dt2.Rows[i]["Id"]) == com.Id)
                            {
                                dt2.Rows[i].Delete(); dt2.AcceptChanges();
                            }
                        }
                        context.SaveChanges();
                        CurrencyGrid.ItemsSource = dt2.DataTableToList<SettlementCurrency>();
                    }
                }
            }
        }

        private void CurrencyGrid_RowEditEnding(object sender, Microsoft.Windows.Controls.DataGridRowEditEndingEventArgs e)
        {
            var c = context.Settlements.Include("Currency").Where(x => x.CustomerName == cbmCustomer.SelectedItem.ToString() && x.SettlementRef == cbmSettlement.SelectedItem.ToString()).ToList();
            SettlementCurrency sett = e.Row.DataContext as SettlementCurrency;
            List<SettlementCurrency> li = new List<SettlementCurrency>();
            var ss = context.SettlementCurrencies.Where(x => x.Id == sett.Id).FirstOrDefault();
            ss.CurrencyCode = sett.CurrencyCode;
            ss.Rate = sett.Rate;
            li.Add(ss);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var r = Visibility.Visible;
            Commoditynametxt.Visibility = r; Commoditypricetxt.Visibility = r; Scarpnametxt.Visibility = r; Scarppricetxt.Visibility = r; currtxt.Visibility = r; currpricetxt.Visibility = r; Addnewitembtn.Visibility = r;
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            var r = Visibility.Hidden;
            addcomm();            
            Commoditynametxt.Visibility = r; Commoditypricetxt.Visibility = r; Scarpnametxt.Visibility = r; Scarppricetxt.Visibility = r; currtxt.Visibility = r; currpricetxt.Visibility = r; Addnewitembtn.Visibility = r;
        }

        private void addcomm()
        {
            if (!string.IsNullOrEmpty(Commoditynametxt.Text) && !string.IsNullOrEmpty(Commoditypricetxt.Text))
            {
                var c = context.Settlements.Include("Commodity").Where(x => x.CustomerName == cbmCustomer.SelectedItem.ToString() && x.SettlementRef == cbmSettlement.SelectedItem.ToString()).ToList();
                List<List<SettlementCommodity>> Comm = c.Select(x => x.Commodity).ToList();
                Settlement se = new Settlement();
                //se.Id = settid;
                se.SettlementFrom = c.Select(x => x.SettlementFrom).FirstOrDefault();
                se.SettlementTo = c.Select(x => x.SettlementTo).FirstOrDefault();
                se.SettlementRef = c.Select(x => x.SettlementRef).FirstOrDefault();
                se.CustomerId = c.Select(x => x.CustomerId).FirstOrDefault();
                se.CustomerName = c.Select(x => x.CustomerName).FirstOrDefault();
                SettlementCommodity ss = new SettlementCommodity();
                se.Commodity = new List<SettlementCommodity>();
                for (int i = 0; i < 1; i++)
                {
                    ss.MaterialName = Commoditynametxt.Text;
                    ss.Rate = Convert.ToDecimal(Commoditypricetxt.Text);
                    se.Commodity.Add(ss);
                }
                context.Settlements.Add(se);
                context.SaveChanges();
                DataRow dr = dt.NewRow();
                dr["MaterialName"] = Commoditynametxt.Text;
                dr["Id"] = ss.Id;
                dr["Rate"] = Commoditypricetxt.Text; dt.Rows.Add(dr);
                Commoditygrid.ItemsSource = dt.DataTableToList<SettlementCommodity>();
                Commoditynametxt.Text = string.Empty; Commoditypricetxt.Text = string.Empty;
            }
            if (!string.IsNullOrEmpty(Scarpnametxt.Text) && !string.IsNullOrEmpty(Scarppricetxt.Text))
            {
                var c = context.Settlements.Include("Scarp").Where(x => x.CustomerName == cbmCustomer.SelectedItem.ToString() && x.SettlementRef == cbmSettlement.SelectedItem.ToString()).ToList();
                List<List<SettlementScarp>> Comm = c.Select(x => x.Scarp).ToList();
                Settlement se = new Settlement();
                se.Id = settid;
                se.SettlementFrom = c.Select(x => x.SettlementFrom).FirstOrDefault();
                se.SettlementTo = c.Select(x => x.SettlementTo).FirstOrDefault();
                se.SettlementRef = c.Select(x => x.SettlementRef).FirstOrDefault();
                se.CustomerId = c.Select(x => x.CustomerId).FirstOrDefault();
                se.CustomerName = c.Select(x => x.CustomerName).FirstOrDefault();
                SettlementScarp ss = new SettlementScarp();
                se.Scarp = new List<SettlementScarp>();
                for (int i = 0; i < 1; i++)
                {
                    ss.ScrapName = Scarpnametxt.Text;
                    ss.Rate = Convert.ToDecimal(Scarppricetxt.Text);
                    se.Scarp.Add(ss);
                }
                context.Settlements.Add(se);
                context.SaveChanges();

                DataRow dr = dt1.NewRow();
                dr["ScrapName"] = Scarpnametxt.Text;
                dr["Id"] = ss.Id;
                dr["Rate"] = Scarppricetxt.Text; dt1.Rows.Add(dr);
                Scarpgrid.ItemsSource = dt1.DataTableToList<SettlementScarp>();
                Scarpnametxt.Text = string.Empty; Scarppricetxt.Text = string.Empty;
            }
            if (!string.IsNullOrEmpty(currtxt.Text) && !string.IsNullOrEmpty(currpricetxt.Text))
            {
                var c = context.Settlements.Include("Currency").Where(x => x.CustomerName == cbmCustomer.SelectedItem.ToString() && x.SettlementRef == cbmSettlement.SelectedItem.ToString()).ToList();
                List<List<SettlementCurrency>> Comm = c.Select(x => x.Currency).ToList();
                Settlement se = new Settlement();
                se.Id = settid;
                se.SettlementFrom = c.Select(x => x.SettlementFrom).FirstOrDefault();
                se.SettlementTo = c.Select(x => x.SettlementTo).FirstOrDefault();
                se.SettlementRef = c.Select(x => x.SettlementRef).FirstOrDefault();
                se.CustomerId = c.Select(x => x.CustomerId).FirstOrDefault();
                se.CustomerName = c.Select(x => x.CustomerName).FirstOrDefault();
                SettlementCurrency ss = new SettlementCurrency();
                se.Currency = new List<SettlementCurrency>();
                for (int i = 0; i < 1; i++)
                {
                    ss.CurrencyCode = currtxt.Text;
                    ss.Rate = Convert.ToDecimal(currpricetxt.Text);
                    se.Currency.Add(ss);
                }
                context.Settlements.Add(se);
                context.SaveChanges();

                DataRow dr = dt2.NewRow();
                dr["CurrencyCode"] = currtxt.Text;
                dr["Id"] = ss.Id;
                dr["Rate"] = currpricetxt.Text; dt2.Rows.Add(dr);
                CurrencyGrid.ItemsSource = dt2.DataTableToList<SettlementCurrency>();
                currtxt.Text = string.Empty; currpricetxt.Text = string.Empty;
            }
        }
        
            private void Showcomm(){
                foreach (var enrt in context.ChangeTracker.Entries())
                {
                    switch (enrt.State)
                    {
                        case System.Data.Entity.EntityState.Modified:
                            enrt.State = System.Data.Entity.EntityState.Unchanged;
                            break;
                    }
                }
                dt.Clear(); dt1.Clear(); dt2.Clear();
                var c = context.Settlements.Include("Currency").Include("Scarp").Include("Commodity").Where(x => x.CustomerName == cbmCustomer.SelectedItem.ToString() && x.SettlementRef == cbmSettlement.SelectedItem.ToString()).ToList();
                settid = c.Select(x => x.Id).FirstOrDefault();
                IEnumerable<Settlement> cus = context.Settlements.Where(x => x.CustomerName == cbmCustomer.SelectedItem.ToString() && x.SettlementRef == cbmSettlement.SelectedItem.ToString()).Take(1).ToList();
                SettmentGrid.ItemsSource = cus;
                var Comm = c.Select(x => x.Commodity).ToList();
                var scr = c.Select(x => x.Scarp).ToList();
                var curr = c.Select(x => x.Currency).ToList();

                foreach (var r in Comm)
                {
                    foreach (var s in r)
                    {
                        DataRow dr = dt.NewRow();
                        dr["MaterialName"] = s.MaterialName;
                        dr["Id"] = s.Id;
                        dr["Rate"] = s.Rate;
                        dt.Rows.Add(dr);
                    }
                }
                foreach (var r1 in scr)
                {
                    foreach (var s in r1)
                    {
                        DataRow dr1 = dt1.NewRow();
                        dr1["ScrapName"] = s.ScrapName;
                        dr1["Id"] = s.Id;
                        dr1["Rate"] = s.Rate; dt1.Rows.Add(dr1);
                    }
                }
                foreach (var r1 in curr)
                {
                    foreach (var s in r1)
                    {
                        DataRow dr1 = dt2.NewRow();
                        dr1["CurrencyCode"] = s.CurrencyCode;
                        dr1["Id"] = s.Id;
                        dr1["Rate"] = s.Rate; dt2.Rows.Add(dr1);
                    }
                }

                Commoditygrid.ItemsSource = dt.DataTableToList<SettlementCommodity>();
                Scarpgrid.ItemsSource = dt1.DataTableToList<SettlementScarp>();
                CurrencyGrid.ItemsSource = dt2.DataTableToList<SettlementCurrency>();
            }

        private void cbmSettlement_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbmSettlement.SelectedIndex != -1)
            {
                Showcomm();
                var r = Visibility.Hidden;var s = Visibility.Visible;
                Createbtn.Visibility = s;Savebtn.Visibility = s;SettmentGrid.Visibility = s;Commoditygrid.Visibility = s;Scarpgrid.Visibility = s;CurrencyGrid.Visibility = s;
                Commoditynametxt.Visibility = r; Commoditypricetxt.Visibility = r; Scarpnametxt.Visibility = r; Scarppricetxt.Visibility = r; currtxt.Visibility = r; currpricetxt.Visibility = r; Addnewitembtn.Visibility = r;
            }
        }

        private void cbmCustomer_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            //if (!string.IsNullOrEmpty(e.Text))
            //    customerFilter += e.Text;
            if (customerFilter.Length > 2)
            {
                var cus1 = context.Settlements.Where(x => x.CustomerName.StartsWith(e.Text)).Select(x => x.CustomerName).Distinct().ToList();
                {

                    cbmCustomer.ItemsSource = cus1;
                }
            }
        }


    }
    public class CommodityValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            SettlementCommodity com = (value as BindingGroup).Items[0] as SettlementCommodity;
            if (string.IsNullOrEmpty(com.MaterialName))
            {
                return new ValidationResult(false, "Commodity Name Cant be Null");
            }
            else
                return ValidationResult.ValidResult;
        }
    }
    public class ScrapValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            SettlementScarp com = (value as BindingGroup).Items[0] as SettlementScarp;
            if (string.IsNullOrEmpty(com.ScrapName))
            {
                return new ValidationResult(false, "Scarp Cant be Null");
            }
            else
                return ValidationResult.ValidResult;
        }
    }

    public class SalesValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            SalesQty com = (value as BindingGroup).Items[0] as SalesQty;
            if (com.Quantity == 0 || string.IsNullOrEmpty(com.Quantity.ToString()))
            {
                return new ValidationResult(false, "Sales Quantity Cant be Null Or Zero ");
            }
            else
                return ValidationResult.ValidResult;
        }
    }


    
    
    

}
