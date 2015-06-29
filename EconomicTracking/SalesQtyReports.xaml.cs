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
using System.Collections;
namespace EconomicTracking
{
    /// <summary>
    /// Interaction logic for SalesQtyReports.xaml
    /// </summary>
    public partial class SalesQtyReports : System.Windows.Controls.UserControl
    {
        List<string> li = new List<string>();
        List<string> li1 = new List<string>();
        DataTable table = new DataTable();
        Task t;
        EconomicsTrackingDbContext context = new EconomicsTrackingDbContext();
        private BackgroundWorker worker = null; short s = 0;

        public SalesQtyReports()
        {
            InitializeComponent();
            CalendarDateRange cd = new CalendarDateRange();
            cd.Start = DateTime.Now.AddDays((DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month) - DateTime.Now.Day) + 1);
            dtsalesfromYear.BlackoutDates.Add(cd);
            var IsWaiting = Visibility.Hidden;
            gg.Visibility = IsWaiting; txt.Text = "";
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (t != null)
            {
                if (!t.IsCompleted)
                {
                    Xceed.Wpf.Toolkit.MessageBox.Show("Data is Still Processing");
                }
                else
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
            }
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
                table.Clear(); txt.Text = "Please Wait..................";
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

        private async void salesqtybymonthbtn_Click(object sender, RoutedEventArgs e)
        {
            if (dtsalesfrommonth.SelectedDate.HasValue && dtsalesfromYear.SelectedDate.HasValue && cuscombo.SelectedIndex!=-1)
            {
                var IsWaiting = Visibility.Visible;
                gg.Visibility = IsWaiting;
                table.Clear(); txt.Text = "Please Wait..................";
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

                System.Globalization.DateTimeFormatInfo mfi = new System.Globalization.DateTimeFormatInfo();
                var fromDate = new DateTime(dtsalesfrommonth.SelectedDate.Value.Year, dtsalesfrommonth.SelectedDate.Value.Month, 1);
                var toDate = new DateTime(dtsalesfromYear.SelectedDate.Value.Year, dtsalesfromYear.SelectedDate.Value.Month, todatemonthend);
                var list = await context.SalesQty.Where(x => x.Date >= fromDate && x.Date <= toDate).ToListAsync();
                var sList = (from s in list
                             where s.CustomerName==cuscombo.SelectedItem.ToString()
                             group s by new { month = s.Date.Month, year = s.Date.Year, cus = s.CustomerAssemblyId } into d
                             select new
                             {
                                 montyear = mfi.GetAbbreviatedMonthName(d.Key.month) + "," + d.Key.year,
                                 MonthYear = d.Key.month + "/" + d.Key.year,
                                 Sum = d.Sum(x => x.Quantity),
                                 CustomerAssemblyId = d.Key.cus
                             })
                    //.Where(l.Contains()
                    //.Where(x => x.CustomerAssemblyId == cuscombo.SelectedItem.ToString())
                    .Where(x => li.Contains(x.CustomerAssemblyId))
                    //.Where(x => x.CustomerAssemblyId == "CusAssy_627" )
                             .OrderBy(x => DateTime.Parse(x.MonthYear)).ToList();

                //|| x.CustomerAssemblyId == "CusAssy_018"

                //foreach(var e in Slider)



                //var sList1 = sList.Where(x=>x.year >= dtsalesfrommonth.SelectedDate.Value.Year 
                //    && x.year <= dtsalesfromYear.SelectedDate.Value.Year

                //    ).TakeWhile(x=>x.year==dtsalesfrommonth.SelectedDate.Value.Year?x.month>=dtsalesfrommonth.SelectedDate.Value.Month:false).ToList();

                if (sList != null)
                {
                    salesqtygroup.Visibility = Visibility.Visible;
                    //table = (DataTable)sList.AsEnumerable();
                    salesqtydatagrid.ItemsSource = sList;
                    IsWaiting = Visibility.Hidden;
                    gg.Visibility = IsWaiting;
                    txt.Text = "";
                }

                if (sList != null)
                {
                    //worker = new BackgroundWorker();
                    //worker.DoWork += (a, b) =>
                    //{
                    t = Task.Run(() => { 
                    if (s == 0)
                    {
                        table.Columns.Add("Customer Assy No"); s++;
                    };
                    sList.ForEach(x =>
                    {
                        if (!table.Columns.Contains(x.MonthYear.ToString()))
                            table.Columns.Add(new DataColumn(x.MonthYear.ToString(), System.Type.GetType("System.String")));
                    });
                    table.AcceptChanges();
                    //int j = sList.Select(x => x.CustomerAssemblyId).Distinct().Count();
                    var tep = sList.Select(x => x.CustomerAssemblyId).Distinct().ToList();
                    
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
                    }); 
                    
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

        private async void cuscombo_Loaded(object sender, RoutedEventArgs e)
        {
            cuscombo.ItemsSource = await context.SalesQty.Select(x => x.CustomerName).Distinct().ToListAsync();
        }
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {

            if (chkSelectAll.IsChecked == true)
            {
                li.Clear();
                li = li1;
                li.Distinct().ToList();
                trt.SelectAll();
            }
        }

        private void CheckBox_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = context.SalesQty.Where(x => x.CustomerName == cuscombo.SelectedItem.ToString()).Distinct().ToList();
        }

        private void chkcus_Checked(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.CheckBox p = (System.Windows.Controls.CheckBox)sender;
            if (!li.Contains(p.Content.ToString()))
            {
                li.Add(p.Content.ToString()); Xceed.Wpf.Toolkit.MessageBox.Show(p.Content.ToString());
            }

        }

        private void chkcus_Unchecked(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.CheckBox p = (System.Windows.Controls.CheckBox)sender;
            if (li.Contains(p.Content.ToString()))
            {
                li.Add(p.Content.ToString()); Xceed.Wpf.Toolkit.MessageBox.Show(p.Content.ToString());
            }
        }
        private async void cuscombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var r = await context.SalesQty.Where(x => x.CustomerName == cuscombo.SelectedItem.ToString()).Select(x=>new {x.CustomerAssemblyId}).Distinct().ToListAsync();
            foreach (var r1 in r.Select(x => new {x.CustomerAssemblyId}).Distinct().ToList())
            {
                li1.Add(r1.CustomerAssemblyId);
            }
            li.Clear();
            trt.ItemsSource = r;
            trt.UnselectAll();

        }

        private void chkSelectAll_Unchecked(object sender, RoutedEventArgs e)
        {
            if (chkSelectAll.IsChecked == false)
            {
                li.Clear();
                trt.UnselectAll();
            }
        }
    }
    }
    

