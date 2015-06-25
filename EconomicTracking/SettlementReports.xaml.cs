using EconomicTracking.Dal;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WPFReportTest;
using System.Data.Entity;
using System;
using System.Data;
using System.Reflection;
using System.ComponentModel;
namespace EconomicTracking
{
    /// <summary>
    /// Interaction logic for SettlementReports.xaml
    /// </summary>
    public partial class SettlementReports : UserControl
    {
        public SettlementReports()
        {
            InitializeComponent();
            CalendarDateRange dr = new CalendarDateRange();
            dr.Start = DateTime.Now.AddDays(1);
            dtSettleTo.BlackoutDates.Add(dr);
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (cbmCustomer.SelectedItem != null && cbmSettlement.SelectedItem != null)
            {
                var sList = new List<Settlement>();
                using (var context = new EconomicsTrackingDbContext())
                {
                    sList = await context.Settlements.Include("Commodity")
                                               .Include("Scarp").Include("Currency").Where(x => x.SettlementRef == cbmSettlement.SelectedValue.ToString() && x.CustomerName == cbmCustomer.SelectedItem.ToString()).ToListAsync();
                }
                if (sList.Any())
                {
                    grpSettlement.Visibility = Visibility.Visible;
                    gridSettlementcommodity.Visibility = Visibility.Visible;
                    gridSettlementcurrency.Visibility = Visibility.Visible;
                    gridSettlementscrap.Visibility = Visibility.Visible;
                    Showreportbutton.Visibility = Visibility.Visible;
                    //var sslist = sList.Select(x => new { x.Id, x.CustomerId, x.CustomerName, x.SettlementFrom, x.SettlementTo,x.SettlementRef }).ToList();
                    gridSettlement.ItemsSource = sList.Select(x => new { x.Id, x.CustomerId, x.CustomerName, x.SettlementFrom, x.SettlementTo, x.SettlementRef }).Take(1).ToList();
                    gridScrap.ItemsSource = sList.FirstOrDefault().Scarp.Select(x => new { x.ScrapName, x.Rate }).ToList();
                    gridCurrency.ItemsSource = sList.FirstOrDefault().Currency.Select(x => new { x.CurrencyCode, x.Rate }).ToList();
                    gridCommodity.ItemsSource = sList.FirstOrDefault().Commodity.Select(x => new { x.MaterialName, x.Rate }).ToList();
                }
            }
            else
            {

                Xceed.Wpf.Toolkit.MessageBox.Show("Please Select Customer and Settlement Ref Id", "Settlement Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (cbmSettlement.SelectedValue != null && cbmCustomer.SelectedValue!=null)
            {

                if (string.IsNullOrEmpty(cbmSettlement.SelectedValue.ToString()) || string.IsNullOrEmpty(cbmCustomer.SelectedValue.ToString()))
                {
                    await Dispatcher.BeginInvoke(new Action(() =>
                    {
                        Xceed.Wpf.Toolkit.MessageBox.Show("Please Select Customer and Settlement Ref Id.!", "Settlement Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    }));
                }
                else
                {
                    
                    var sListTemp = new List<Settlement>();
                    var sList = new List<Settlement>();
                    using (var context = new EconomicsTrackingDbContext())
                    {
                        //cbmSettlement.SelectedValue.ToString()

                        sList = await context.Settlements.Include("Commodity")
                                                       .Include("Scarp").Include("Currency").Where(x => x.SettlementRef == cbmSettlement.SelectedValue.ToString()  &&x.CustomerName==cbmCustomer.SelectedItem.ToString()).ToListAsync();
                        //sList = sListTemp.Where(x => x.SettlementFrom >= dtSettleFrom.SelectedDate.Value && x.SettlementTo <= dtSettleTo.SelectedDate.Value).ToList();

                        var setle = new List<SettlementModel>();
                        foreach (var item in sList)
                        {
                            var map = new SettlementModel
                            {
                                SettlementRef = item.SettlementRef,
                                SettlementFrom = item.SettlementFrom,
                                SettlementTo = item.SettlementTo,
                                CustomerName = item.CustomerName,
                                CommodityTotal = Sum(item.Commodity.Select(x => x.Rate).ToList()),
                                ScrapTotal = Sum(item.Scarp.Select(x => x.Rate).ToList())
                            };
                            map.Currency = new List<SettlementCurrencyModel>();
                            item.Currency.ToList().ForEach(x =>
                            {
                                map.Currency.Add(new SettlementCurrencyModel
                                {
                                    Id = x.Id,
                                    CurrencyCode = x.CurrencyCode,
                                    Rate = x.Rate,
                                    Settlement_Id = x.Settlement_Id
                                });
                            });
                            map.Commodity = new List<SettlementCommodityModel>();
                            item.Commodity.ToList().ForEach(x =>
                            {
                                map.Commodity.Add(new SettlementCommodityModel
                                {
                                    Id = x.Id,
                                    MaterialName = x.MaterialName,
                                    Rate = x.Rate,
                                    Settlement_Id = x.Settlement_Id
                                });
                            });
                            map.Scarp = new List<SettlementScarpModel>();
                            item.Scarp.ToList().ForEach(x =>
                            {
                                map.Scarp.Add(new SettlementScarpModel
                                {
                                    Id = x.Id,
                                    ScrapName = x.ScrapName,
                                    Rate = x.Rate,
                                    Settlement_Id = x.Settlement_Id
                                });
                            });
                            setle.Add(map);
                            
                        }
                        if (setle.Count > 0)
                        {
                            EmployeeInfoCrystalReport employeeInfoCrystalReport = new EmployeeInfoCrystalReport();
                            var settlement = setle.Take(1).ToList();
                            var currency = setle.Take(1).FirstOrDefault().Currency.ToList();
                            ReportUtility.DisplaySettlementreport(employeeInfoCrystalReport, settlement, null, currency, setle.Take(1).FirstOrDefault().Commodity, setle.Take(1).FirstOrDefault().Scarp);
                            employeeInfoCrystalReport.Dispose();
                        }
                        else
                        {
                            Xceed.Wpf.Toolkit.MessageBox.Show("Don't have any records.", "Settlement Info", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                }
            }
            else
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("Please Select Customer And SetttlementRefId", "Settlement Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }

        }

        public decimal Sum(List<decimal> list)
        {
            decimal sum = 0;
            list.ForEach(x =>
            {
                sum += x;
            });
            return sum;
        }



        private void cbmCustomer_Loaded(object sender, RoutedEventArgs e)
        {
            cbmCustomer.ItemsSource = new EconomicsTrackingDbContext().Settlements.Select(x => x.CustomerName).Distinct().OrderBy(x => x).ToList();

        }

        private void cbmSettlement_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            cbmSettlement.ItemsSource = new EconomicsTrackingDbContext().Settlements.Select(x => x.SettlementRef).ToList();
            if (cbmSettlement.SelectedValue != null)
            {
                Showdatabysetmt.Visibility = Visibility.Visible;
            }
            else
            {
                Showdatabysetmt.Visibility = Visibility.Hidden;
            }
        }

        private async void cbmCustomer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cbmSettlement.ItemsSource = await new EconomicsTrackingDbContext().Settlements.Where(y => y.CustomerName == cbmCustomer.SelectedValue.ToString()).Select(x => x.SettlementRef).Distinct().ToListAsync();

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (cbmCustomer.SelectedIndex!=-1)
            {
                
                if (!string.IsNullOrEmpty(cbmCustomer.SelectedValue.ToString()) && dtSettleFrom.SelectedDate.HasValue && dtSettleTo.SelectedDate.HasValue)
                {
                    var sList = new List<Settlement>();
                    using (var context = new EconomicsTrackingDbContext())
                    {
                        sList = context.Settlements.Where(x => x.CustomerName == cbmCustomer.SelectedValue.ToString()).ToList();
                         
                    }
                    if (sList.Any())
                    {
                        grpSettlement.Visibility = Visibility.Visible;
                        gridSettlementcommodity.Visibility = Visibility.Hidden;
                        gridSettlementcurrency.Visibility = Visibility.Hidden;
                        gridSettlementscrap.Visibility = Visibility.Hidden;
                        Showreportbutton.Visibility = Visibility.Hidden;
                        var sslisttemp = sList.Select(x => new { x.Id, x.CustomerId, x.CustomerName, x.SettlementFrom, x.SettlementTo, x.SettlementRef }).ToList();
                        var sslistactual = sslisttemp.Where(x => x.SettlementFrom >= dtSettleFrom.SelectedDate.Value && x.SettlementTo <= dtSettleTo.SelectedDate.Value).ToList();
                        gridSettlement.ItemsSource = sslistactual;
                        if (sslistactual.Count()==0)
                        {
                            Xceed.Wpf.Toolkit.MessageBox.Show("Don't have any records.", "Settlement Info", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        //gridScrap.ItemsSource = sList.FirstOrDefault().Scarp.Select(x => new { x.ScrapName, x.Rate }).ToList();
                        //gridCurrency.ItemsSource = sList.FirstOrDefault().Currency.Select(x => new { x.CurrencyCode, x.Rate }).ToList();
                        //gridCommodity.ItemsSource = sList.FirstOrDefault().Commodity.Select(x => new { x.MaterialName, x.Rate }).ToList();
                    }
                    else
                    {
                        Xceed.Wpf.Toolkit.MessageBox.Show("Don't have any records.", "Settlement Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                {
                    Xceed.Wpf.Toolkit.MessageBox.Show("Please Select Customer And from-To date", "Settlement Info", MessageBoxButton.OK, MessageBoxImage.Information);
                }


            }
            else
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("Please Select Customer And from-To date", "Settlement Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }

        }


    }
}
