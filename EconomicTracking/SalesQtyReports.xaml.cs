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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.Entity;
using System.ComponentModel;
using EntityFramework.Utilities;
using System.Threading;
using System.Collections.ObjectModel;
namespace EconomicTracking
{
    /// <summary>
    /// Interaction logic for SalesQtyReports.xaml
    /// </summary>
    public partial class SalesQtyReports : System.Windows.Controls.UserControl
    {
        List<string> li = new List<string>();
        DataTable table = new DataTable();
        private BackgroundWorker worker = null;
         
        public SalesQtyReports()
        {
            InitializeComponent();
            var context = new EconomicsTrackingDbContext();
            lstItems.ItemsSource = context.SalesQty.ToList();
        }



        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Excel file (*.xls)|*.xls|Excel Files (*.xlsx)|*.xlsx";
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                worker = new BackgroundWorker();
                worker.DoWork += (a, b) =>
                {

                    table.ExportToExcel(saveFileDialog.FileName);
                };
                worker.RunWorkerCompleted += worker_RunWorkerCompleted;
                worker.RunWorkerAsync();
            }

        }

        private void gridSalesQty_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (!dtSalesFrom.SelectedDate.HasValue || !dtSalesTo.SelectedDate.HasValue)
            {
                await Dispatcher.BeginInvoke(new Action(() =>
                 {
                     Xceed.Wpf.Toolkit.MessageBox.Show("From and To Dates are mandatory to get results.!", "BOM Info", MessageBoxButton.OK, MessageBoxImage.Information);
                 }));
            }
            else
            {
                var sList = new List<SalesQty>();
                var context = new EconomicsTrackingDbContext();
                sList = await context.SalesQty.Where(x => x.Date >= dtSalesFrom.SelectedDate.Value && x.Date <= dtSalesTo.SelectedDate.Value).ToListAsync();
                if (sList.Any())
                {
                    worker = new BackgroundWorker();
                    worker.DoWork += (a, b) =>
                    {
                        var qtyList = new List<SalesQtyModel>();
                        sList = sList.OrderByDescending(x => x.Date).ToList();
                        table.Columns.Add("Customer Assy No");
                        sList.ForEach(x =>
                        {
                            if (!table.Columns.Contains(x.Date.ToString("dd-MMM-yy")))
                                table.Columns.Add(new DataColumn(x.Date.ToString("dd-MMM-yy"), System.Type.GetType("System.String")));
                        });
                        table.AcceptChanges();
                        var custAssy = sList.Select(x => x.CustomerAssemblyId).Distinct().ToList();
                        custAssy.ForEach(x =>
                        {
                            if (x != "0")
                            {
                                DataRow row = table.NewRow();
                                row["Customer Assy No"] = x;
                                foreach (var item in sList.Where(y => y.CustomerAssemblyId == x).ToList())
                                {
                                    row[item.Date.ToString("dd-MMM-yy")] = item.Quantity;
                                    //var salqty = new SalesQtyModel();
                                    //salqty.CustomerAssemblyId = item.CustomerAssemblyId;
                                    //salqty.Date = item.Date;
                                    //salqty.Quantity = item.Quantity;
                                    //qtyList.Add(salqty);
                                }
                                table.Rows.Add(row);
                                table.AcceptChanges();
                            }
                        });
                        Dispatcher.BeginInvoke(new Action(() =>
               {
                   salesqtygroup.Visibility = Visibility.Visible;
                   salesqtydatagrid.ItemsSource = table.DefaultView;
                   //gridSalesQty.ItemsSource = table.DefaultView;
               }));
                    };
                    worker.RunWorkerCompleted += worker_RunWorkerCompleted;
                    worker.RunWorkerAsync();
                }
                else
                {
                    Xceed.Wpf.Toolkit.MessageBox.Show("No Records.!", "BOM Info", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }

        }
        private void worker_RunWorkerCompleted(object sender,
                                      RunWorkerCompletedEventArgs e)
        {
            worker.Dispose();
            worker = null;
            GC.Collect();
        }

        private void Liet_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Xceed.Wpf.Toolkit.MessageBox.Show(Liet.SelectedItem.ToString());
        }

        private void salesqtybymonthbtn_Click(object sender, RoutedEventArgs e)
        {
            if (dtsalesfrommonth.SelectedDate.HasValue && dtsalesfromYear.SelectedDate.HasValue)
            {
                //var sList = new List<SalesQty>();
                var context = new EconomicsTrackingDbContext();
                //var sList = (from s in context.SalesQty
                //          group s by s.Date.ToString("yyyy/MM") into g1
                //          select new
                //          {
                //              CustomerAssemblyId=g1.Select(x=>x.CustomerAssemblyId),
                //              read = g1.Key,
                //              t1 = g1.Sum(x => x.Quantity)
                //          }).OrderBy(x => x.read).ToList();

                var todatemonthend = DateTime.DaysInMonth(dtsalesfromYear.SelectedDate.Value.Year, dtsalesfromYear.SelectedDate.Value.Month);

                //24-apr-2014
               //string s1= lstItems.SelectedItem.ToString();
               //Xceed.Wpf.Toolkit.MessageBox.Show("dsudsd"+s1);
                List<string> l = new List<string> { "CusAssy_627", "CusAssy_018" };
                var fromDate = new DateTime(dtsalesfrommonth.SelectedDate.Value.Year, dtsalesfrommonth.SelectedDate.Value.Month, 1);
                var toDate = new DateTime(dtsalesfromYear.SelectedDate.Value.Year, dtsalesfromYear.SelectedDate.Value.Month, todatemonthend);
                var list = context.SalesQty.Where(x => x.Date >= fromDate && x.Date <= toDate).ToList();
                var sList = (from s in list
                             group s by new { month = s.Date.Month, year = s.Date.Year, cus = s.CustomerAssemblyId } into d
                             select new
                             {
                                 MonthYear = d.Key.month + "/" + d.Key.year,
                                 Sum = Math.Truncate(d.Sum(x => x.Quantity)),
                                 CustomerAssemblyId = d.Key.cus
                             })
                    //.Where(l.Contains()
                    //.Where(x => x.CustomerAssemblyId == cuscombo.SelectedItem.ToString())
                    .Where(x => l.Contains(x.CustomerAssemblyId))
                    //.Where(x => x.CustomerAssemblyId == "CusAssy_627" )
                             .OrderBy(x => DateTime.Parse(x.MonthYear)).ToList();

                //|| x.CustomerAssemblyId == "CusAssy_018"

                //foreach(var e in Slider)



                //var sList1 = sList.Where(x=>x.year >= dtsalesfrommonth.SelectedDate.Value.Year 
                //    && x.year <= dtsalesfromYear.SelectedDate.Value.Year

                //    ).TakeWhile(x=>x.year==dtsalesfrommonth.SelectedDate.Value.Year?x.month>=dtsalesfrommonth.SelectedDate.Value.Month:false).ToList();

                if (sList != null)
                {
                    //worker = new BackgroundWorker();
                    //worker.DoWork += (a, b) =>
                    //{
                    System.Windows.Controls.CheckBox box; ;


                    table.Columns.Add("Customer Assy No");
                    sList.ForEach(x =>
                    {
                        if (!table.Columns.Contains(x.MonthYear.ToString()))
                            table.Columns.Add(new DataColumn(x.MonthYear.ToString(), System.Type.GetType("System.String")));
                    });
                    table.AcceptChanges();

                    int j = sList.Select(x => x.CustomerAssemblyId).Distinct().Count();
                    var tep = sList.Select(x => x.CustomerAssemblyId).Distinct().ToList();
                    //for (int i = 0; i < j; i++)
                    //{
                    if (tep.Any())
                    {
                        foreach (var cus in tep)
                        {
                            DataRow dr = table.NewRow();
                            foreach (var r in sList.Where(x => x.CustomerAssemblyId == cus))
                            {
                                //string s= (string)dr["Customer Assy No"];
                                if (dr["Customer Assy No"] == null || dr["Customer Assy No"].ToString() == "")
                                {
                                    dr["Customer Assy No"] = r.CustomerAssemblyId.ToString();
                                    dr[r.MonthYear.ToString()] = r.Sum;
                                    table.Rows.Add(dr);
                                }
                                else
                                {
                                    dr[r.MonthYear.ToString()] = r.Sum;
                                }
                            }
                        }
                    }
                    //}
                    table.AcceptChanges();
                    for (int i = 0; i < 3; i++)
                    {
                        box = new System.Windows.Controls.CheckBox();
                        box.Tag = "sddsds";


                    }

                    salesqtygroup.Visibility = Visibility.Visible;
                    //table = (DataTable)sList.AsEnumerable();
                    salesqtydatagrid.ItemsSource = sList;
                    //};
                    //worker.RunWorkerCompleted += worker_RunWorkerCompleted;
                    //worker.RunWorkerAsync();
                }
                else
                {
                    Xceed.Wpf.Toolkit.MessageBox.Show("No Records.!", "BOM Info", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("Please Select The Month And Year.!", "BOM Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }

        }

        private void cuscombo_Loaded(object sender, RoutedEventArgs e)
        {
            var context = new EconomicsTrackingDbContext();
            cuscombo.ItemsSource = context.SalesQty.Select(x => x.CustomerAssemblyId).Distinct().ToList();
        }

        private void listcus_Loaded(object sender, RoutedEventArgs e)
        {
            //var context = new EconomicsTrackingDbContext();
            //listcus.ItemsSource = context.CustomerAssembly.Select(x => x.CustAssyName).ToList();

        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (chkSelectAll.IsChecked == true)
            {
                lstItems.SelectAll();
            }
            else
            {
                //lstItems.UnselectAll();
            }
        }

        private void lstItems_Loaded(object sender, RoutedEventArgs e)
        {
           
        }

        private void CheckBox_Loaded(object sender, RoutedEventArgs e)
        {
            var context = new EconomicsTrackingDbContext();
            this.DataContext = context.SalesQty.Select(x => x.CustomerAssemblyId).ToList();
            //lstItems.ItemsSource = context.SalesQty.Select(x => x.CustomerAssemblyId).ToList();
        }

        private void chkcus_Checked(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.CheckBox p = (System.Windows.Controls.CheckBox)sender;
            li.Add(p.Content.ToString());

            Xceed.Wpf.Toolkit.MessageBox.Show(p.Content.ToString());
            
        }

        private void chkcus_Unchecked(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.CheckBox p = (System.Windows.Controls.CheckBox)sender;
            li.Remove(p.Content.ToString());

            Xceed.Wpf.Toolkit.MessageBox.Show(p.Content.ToString());
        }

    }
}
