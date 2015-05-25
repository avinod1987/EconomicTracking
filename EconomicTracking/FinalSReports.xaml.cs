using EconomicTracking.Dal;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Data.Entity;
using System;
using System.Collections.Generic;
namespace EconomicTracking
{
    /// <summary>
    /// Interaction logic for FinalSReports.xaml
    /// </summary>
    public partial class FinalSReports : UserControl
    {
        public FinalSReports()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            BackgroundWorkerHelper.Run(
         (s, ea) =>
         {
             var context = new EconomicsTrackingDbContext();
             Dispatcher.BeginInvoke(new Action(() =>
          {
              gridFinalReports.ItemsSource = context.FinalReport.Where(x => x.CustomerAssyId == cbmReports.SelectedValue).ToList();
              gridFinalReports.Visibility = Visibility.Visible;
          }));
         });
        }

        private void cbmReports_Loaded(object sender, RoutedEventArgs e)
        {
            BackgroundWorkerHelper.Run(
          (s, ea) =>
          {
              var context = new EconomicsTrackingDbContext();
              Dispatcher.BeginInvoke(new Action(() =>
           {
               cbmReports.ItemsSource = context.FinalReport.Select(x => x.CustomerAssyId).ToList();
           }));
          });

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var context = new EconomicsTrackingDbContext();
            
            var report = context.FinalReport.Where(x => x.CustomerAssyId == cbmReports.SelectedValue).FirstOrDefault();
            Settlement currentSettlement = context.Settlements.Include("Commodity")
                                           .Include("Scarp").Include("Currency").Where(x => x.SettlementRef == report.CurrentSettlementRef).FirstOrDefault();
            Settlement lastSettlement = context.Settlements.Include("Commodity")
                                           .Include("Scarp").Include("Currency").Where(x => x.SettlementRef == report.LastSettlementRef).FirstOrDefault();
            var bom = context.CustomerAssembly.Include("BOM").Where(x => x.CustAssyNo == report.CustomerAssyId).FirstOrDefault();
            var listCurr = new List<SettlementCurrency>();
            var listComm = new List<SettlementCommodity>();
            var listScrap = new List<SettlementScarp>();
            var listContribCurr = new List<SettlementCurrency>();
            var listContribComm = new List<SettlementCommodity>();
            var listContribScrap = new List<SettlementScarp>();
            var listRecoveryCurr = new List<SettlementCurrency>();
            var listRecoveryComm = new List<SettlementCommodity>();
            var listRecoveryScrap = new List<SettlementScarp>();

            for (int i = 0; i < currentSettlement.Currency.Count; i++)
            {

                var currency = new SettlementCurrency();
                currency.CurrencyCode = currentSettlement.Currency[i].CurrencyCode;
                decimal rate = currentSettlement.Currency[i].Rate - lastSettlement.Currency[i].Rate;
                currency.Rate = rate;
                listCurr.Add(currency);
                if (currentSettlement.Currency[i].CurrencyCode.ToUpper() != "INR")
                {
                    currency = new SettlementCurrency();
                    currency.CurrencyCode = currentSettlement.Currency[i].CurrencyCode;
                    var lastCurr = lastSettlement.Currency.Where(x => x.CurrencyCode == currentSettlement.Currency[i].CurrencyCode).FirstOrDefault().Rate;
                    var sum = bom.BOM.Where(x => x.CurrencyCode == currentSettlement.Currency[i].CurrencyCode).Select(x => x.ToalCost).Sum();
                    currency.Rate = sum / lastCurr;
                    listContribCurr.Add(currency);
                    rate = rate * (sum / lastCurr);
                    currency = new SettlementCurrency();
                    currency.CurrencyCode = currentSettlement.Currency[i].CurrencyCode;
                    currency.Rate = rate;
                    listRecoveryCurr.Add(currency);
                }
            }

            for (int i = 0; i < currentSettlement.Commodity.Count; i++)
            {
                var comm = new SettlementCommodity();
                comm.MaterialName = currentSettlement.Commodity[i].MaterialName;
                comm.Rate = currentSettlement.Commodity[i].Rate - lastSettlement.Commodity[i].Rate;
                listComm.Add(comm);
                comm = new SettlementCommodity();
                comm.MaterialName = currentSettlement.Commodity[i].MaterialName;
                decimal sum = bom.BOM.Where(x => x.Commodity == currentSettlement.Commodity[i].MaterialName).Select(x => x.BOMQuantity).Sum();
                comm.Rate = sum;
                listContribComm.Add(comm);
                comm = new SettlementCommodity();
                comm.MaterialName = currentSettlement.Commodity[i].MaterialName;
                comm.Rate = (currentSettlement.Commodity[i].Rate - lastSettlement.Commodity[i].Rate) * sum;
                listRecoveryComm.Add(comm);
            }

            for (int i = 0; i < currentSettlement.Scarp.Count; i++)
            {
                var scrap = new SettlementScarp();
                scrap.ScrapName = currentSettlement.Scarp[i].ScrapName;
                scrap.Rate = currentSettlement.Scarp[i].Rate - lastSettlement.Scarp[i].Rate;
                listScrap.Add(scrap);
                scrap = new SettlementScarp();
                scrap.ScrapName = currentSettlement.Scarp[i].ScrapName;
                decimal sum = bom.BOM.Where(x => x.Scarp == currentSettlement.Scarp[i].ScrapName).Select(x => x.ScrapQuantity).Sum();
                scrap.Rate = sum;
                listContribScrap.Add(scrap);
                scrap = new SettlementScarp();
                scrap.ScrapName = currentSettlement.Scarp[i].ScrapName;
                scrap.Rate = (currentSettlement.Scarp[i].Rate - lastSettlement.Scarp[i].Rate) * sum;
                listRecoveryScrap.Add(scrap);
            }
            report.Recovery = listRecoveryCurr.Select(x => x.Rate).Sum();
        }
    }
}
