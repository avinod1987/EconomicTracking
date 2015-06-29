using EconomicTracking.Dal;
using Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using EntityFramework.Utilities;
using EntityFramework.BulkInsert.Extensions;
using System.Transactions;
namespace EconomicTracking
{
    /// <summary>
    /// Interaction logic for SettlementControl.xaml
    /// </summary>
    public partial class SettlementControl : System.Windows.Controls.UserControl
    {
        private BackgroundWorker worker = null;
        public  SettlementControl()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (worker == null)
            {
                worker = new BackgroundWorker();
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Excel Files|*.xls;*.xlsx;*.xlsm|All files (*.*)|*.*";
                DialogResult res = openFileDialog.ShowDialog();
                if (res == System.Windows.Forms.DialogResult.OK)
                {
                    worker.DoWork += (a, b) =>
                  {
                      Dispatcher.BeginInvoke(new Action(() =>
                      {
                          lblFileName.Text = "Please wait data is processing.......";
                      }));

                      var finfo = new FileInfo(openFileDialog.FileName);
                      FileStream stream = File.Open(openFileDialog.FileName, FileMode.Open, FileAccess.Read);

                      IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                      excelReader.IsFirstRowAsColumnNames = true;
                      DataSet result = excelReader.AsDataSet();
                      excelReader.Close();
                      DataTable dt = result.Tables["SettlementRef"];
                      dt = RemoveUnusedColumnsAndRows(dt);
                      int index = 0;
                      var cols = new List<string>();
                      foreach (DataColumn col in dt.Columns)
                      {
                          if (!string.IsNullOrEmpty(dt.Rows[0][index].ToString()))
                              col.ColumnName = dt.Rows[0][index].ToString();
                          else
                              cols.Add(col.ColumnName);
                          index++;
                      }
                      cols.ForEach(x => { dt.Columns.Remove(x); });
                      dt.Rows[0].Delete();
                      dt.AcceptChanges();
                      int count = 0;
                      using (var context = new EconomicsTrackingDbContext())
                      {
                          context.Configuration.AutoDetectChangesEnabled = false;
                          context.Configuration.ValidateOnSaveEnabled = false;
                          var curlist = context.Currency.ToList();
                          var comlist = context.Materials.ToList();
                          var scplist = context.Scraps.ToList();
                          var settlementList = new List<Settlement>();
                          var currencyList = new List<SettlementCurrency>();
                          var commodityList = new List<SettlementCommodity>();
                          var scrapList = new List<SettlementScarp>();
                          //foreach (DataRow dr in dt.Rows)
                          //{
                          //    IQueryable<Settlement> t = context.Settlements.Where(x => x.SettlementRef == dr["Settlement Ref"].ToString() && x.CustomerName == dr["Customer Name"].ToString());
                          //    context.Settlements.RemoveRange(t);
                          //}
                          foreach (DataRow row in dt.Rows)
                          {
                              Dispatcher.BeginInvoke(new Action(() =>
                                         {
                                             lblFileName.Text = "Uploading records to database(" + count + "/" + (dt.Rows.Count) + ")......";
                                         }));
                              var settlement = new Settlement();
                              settlement.SettlementRef = row["Settlement Ref"].ToString();
                              settlement.CustomerId = row["Customer Reference"].ToString();
                              settlement.CustomerName = row["Customer Name"].ToString();
                              settlement.SettlementFrom = Convert.ToDateTime(row["SettlmentFrom"].ToString());
                              settlement.SettlementTo = Convert.ToDateTime(row["SettlmentTo"].ToString());
                              settlement.Currency = new List<SettlementCurrency>();
                              settlement.Commodity = new List<SettlementCommodity>();
                              settlement.Scarp = new List<SettlementScarp>();
                              curlist.ForEach(x =>
                              {
                                  string mname = x.CurrencyName;
                                  if (row[mname] != null && !string.IsNullOrEmpty(row[mname].ToString()))
                                  {
                                      var curr = new SettlementCurrency();
                                      curr.CurrencyCode = x.CurrencyName;
                                      curr.Rate = Convert.ToDecimal(row[mname].ToString());
                                      //curr.Settlement_Id = settlement.Id;
                                      settlement.Currency.Add(curr);
                                      currencyList.Add(curr);
                                  }
                              });
                              comlist.ForEach(x =>
                              {
                                  string mname = x.MaterialName;
                                      //.Replace("\r\n", "");
                                  if (row[mname] != null && !string.IsNullOrEmpty(row[mname].ToString()))
                                  {
                                      var comm = new SettlementCommodity();
                                      comm.MaterialName = x.MaterialName;
                                      comm.Rate = Convert.ToDecimal(row[mname].ToString());
                                      settlement.Commodity.Add(comm);
                                      commodityList.Add(comm);
                                  }
                              });
                              scplist.ForEach(x =>
                              {
                                  string mname = x.ScrapName;
                                      //.Replace("\r\n", "");
                                  if (row[mname] != null && !string.IsNullOrEmpty(row[mname].ToString()))
                                  {
                                      var comm = new SettlementScarp();
                                      comm.ScrapName = x.ScrapName;
                                      comm.Rate = Convert.ToDecimal(row[mname].ToString());
                                      settlement.Scarp.Add(comm);
                                      scrapList.Add(comm);
                                  }
                              });
                              settlementList.Add(settlement);
                              //context.Settlements.Add(settlement);
                              //context.SaveChanges();
                                  //EFBatchOperation.For<context,Settlement>(.InsertAll(settlementList);
                              count++;
                              //EFBatchOperation(context, settlementList).InsertAll(settlementList);
                              
                          }
                          using (var transactionScope = new TransactionScope())
                          {
                              // some stuff in dbcontext
                              context.Settlements.AddRange(settlementList);
                              context.SaveChanges();
                              transactionScope.Complete();
                          }
                      }
                  };
                    worker.RunWorkerCompleted += worker_RunWorkerCompleted;
                    worker.RunWorkerAsync();
                }
                else
                {
                    worker.Dispose();
                    worker = null;
                }
            }
            else
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("Please wait save process is running.....!", "Settlement Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        private void worker_RunWorkerCompleted(object sender,
                                      RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                lblFileName.Text = "Unable to Upload Data.......";
                Xceed.Wpf.Toolkit.MessageBox.Show("Unable to Upload Data"+e.Error.Message.ToString(), "Settlement Info", MessageBoxButton.OK, MessageBoxImage.Information);
                worker.Dispose();
                worker = null;
                GC.Collect();
            }
            else { 
            lblFileName.Text = "Data uploaded successfully.......";
            Xceed.Wpf.Toolkit.MessageBox.Show("Data uploaded successfully.......", "Settlement Info", MessageBoxButton.OK, MessageBoxImage.Information);
            worker.Dispose();
            worker = null;
            GC.Collect();
            }
        }
        private static DataTable RemoveUnusedColumnsAndRows(DataTable table)
        {
            foreach (var column in table.Columns.Cast<DataColumn>().ToArray())
            {
                if (table.AsEnumerable().All(dr => dr.IsNull(column)))
                    table.Columns.Remove(column);
            }
            table.AcceptChanges();
            for (int h = 0; h < table.Rows.Count; h++)
            {
                if (table.Rows[h].IsNull(0) == true)
                {
                    table.Rows[h].Delete();
                }
            }
            table.AcceptChanges();

            return table;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
