using EconomicTracking.Dal;
using Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Data.Entity;
using System.ComponentModel;
using System.Threading;
using EntityFramework.BulkInsert.Extensions;
namespace EconomicTracking
{
    /// <summary>
    /// Interaction logic for UploadBom.xaml
    /// </summary>
    public partial class UploadBom : System.Windows.Controls.UserControl
    {
        private BackgroundWorker worker = null;
        CustomerAssembly custAssm = null;
        public UploadBom()
        {
            InitializeComponent();
        }
        //[STAThread]
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
                    try
                    {
                        worker.DoWork += (a, b) =>
                        {
                            Dispatcher.BeginInvoke(new Action(() =>
                            {
                                imgLoading.Visibility = Visibility.Visible;
                                lblFileName.Text = "Please wait data is processing.......";
                            }));

                            var finfo = new FileInfo(openFileDialog.FileName);
                            //try { 
                            //FileStream stream = File.Open(openFileDialog.FileName, FileMode.Open, FileAccess.Read);
                            //    }

                            //catch (Exception ex1) { System.Windows.MessageBox.Show("Either file is open or Unable to read the excel");  err=1; }

                            FileStream stream1 = File.Open(openFileDialog.FileName, FileMode.Open, FileAccess.Read);

                            IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream1);
                            excelReader.IsFirstRowAsColumnNames = true;
                            Thread.Sleep(500);
                            DataSet result = excelReader.AsDataSet();
                            excelReader.Close();
                            using (var context = new EconomicsTrackingDbContext())
                            {
                                context.Configuration.AutoDetectChangesEnabled = false;
                                context.Configuration.ValidateOnSaveEnabled = false;
                                int count = 0;
                                var entities = new List<CustomerAssembly>();
                                var bomEntities = new List<BillOfMaterial>();
                                foreach (DataTable table in result.Tables)
                                {
                                    try
                                    {
                                        Dispatcher.BeginInvoke(new Action(() =>
                                    {
                                        lblFileName.Text = "Uploading records to database(" + count + "/" + (result.Tables.Count - 1) + ")......";
                                    }));
                                        DataTable dt = table;
                                        DataTable custDt = dt.AsEnumerable().Take(2).CopyToDataTable();

                                        foreach (var column in custDt.Columns.Cast<DataColumn>().ToArray())
                                        {
                                            if (custDt.AsEnumerable().All(dr => dr.IsNull(column)))
                                                custDt.Columns.Remove(column);
                                        }
                                        custDt.AcceptChanges();
                                        int index = 0;
                                        var removeColumns = new List<string>();
                                        foreach (DataColumn col in custDt.Columns)
                                        {
                                            if (!custDt.Columns.Contains(custDt.Rows[0][index].ToString()))
                                            {
                                                col.ColumnName = custDt.Rows[0][index].ToString();
                                            }
                                            else
                                            {
                                                removeColumns.Add(col.ColumnName);
                                            }
                                            index++;
                                        }
                                        custDt.Rows[0].Delete();
                                        removeColumns.ForEach(x => { custDt.Columns.Remove(x); });
                                        custDt.AcceptChanges();
                                        dt = RemoveUnusedColumnsAndRows(dt);
                                        index = 0;
                                        int colum = 1;
                                        removeColumns = new List<string>();
                                        //dt.Columns.RemoveAt(dt.Columns.Count - 1);
                                        dt = dt.AsEnumerable().Skip(3).CopyToDataTable();
                                        dt.AcceptChanges();
                                        foreach (DataColumn col in dt.Columns)
                                        {
                                            if (!dt.Columns.Contains(dt.Rows[0][index].ToString()))
                                            {
                                                col.ColumnName = dt.Rows[0][index].ToString().Trim();
                                            }
                                            else
                                            {
                                                col.ColumnName = dt.Rows[0][index].ToString().Trim() + "_" + colum;
                                                colum++;
                                            }
                                            index++;
                                        }
                                        dt.Rows[0].Delete();

                                        //removeColumns.ForEach(x => { dt.Columns.Remove(x); });
                                        dt.AcceptChanges();
                                        RemoveNullColumnFromDataTable(dt);
                                        custAssm = new CustomerAssembly();
                                        var custRow = custDt.Rows[0];
                                        string cusass = custRow["Cust Assy No"].ToString().Trim();
                                        string cus=custRow["Customer"].ToString().Trim();
                                      int k=  (from c in context.CustomerAssembly
                                            where (c.CustAssyNo==cusass && c.Customer==cus)
                                                   select c).Count();
                                        //int k = context.CustomerAssembly.Select(x => x.CustAssyNo == cusass && x.Customer == cus).Count();
                                        MessageBoxResult messboxreslt = MessageBoxResult.No;
                                        if (k > 0) {

                                            messboxreslt= System.Windows.MessageBox.Show(cusass + "   " + cus + " Alredy Exists Do You Want to Continue Click No to be skipped from Upload", "Confirmation", System.Windows.MessageBoxButton.YesNo);
                                            if (messboxreslt == MessageBoxResult.Yes) { 
                                            context.CustomerAssembly.RemoveRange(context.CustomerAssembly.Where(x => x.CustAssyNo == cus && x.Customer == cus));
                                            context.BillOfMaterial.RemoveRange(context.BillOfMaterial.Where(x => x.CustAssyNo == cus));
                                            }
                                        }
                                        if (messboxreslt == MessageBoxResult.Yes||k<=0) {
                                        custAssm.Customer = custRow["Customer"].ToString().Trim();
                                        custAssm.CustAssyNo = custRow["Cust Assy No"].ToString().Trim();
                                        custAssm.CustAssyName = custRow["Cust Assy Name"].ToString().Trim();
                                        custAssm.LocalAssyNo = custRow["Local Assy No"].ToString().Trim();
                                        custAssm.LocalAssyName = custRow["Local Assy Name"].ToString().Trim();
                                        custAssm.Quantity = !string.IsNullOrEmpty(custRow["QtyCparts"].ToString().Trim()) ? Convert.ToDecimal(custRow["QtyCparts"].ToString().Trim()) : 0;
                                        custAssm.SettlementRef = custRow["Settlement Ref"].ToString().Trim();
                                        custAssm.TotalCost = !string.IsNullOrEmpty(custRow["Total Cost"].ToString().Trim()) ? Convert.ToDecimal(custRow["Total Cost"].ToString().Trim()) : 0;
                                        custAssm.Family = custRow["Family"].ToString().Trim();
                                        custAssm.Category = custRow["Category"].ToString().Trim();
                                        //custAssm.Quantity = custRow["Settlement Ref"].ToString().Trim());
                                        custAssm.BOM = new List<BillOfMaterial>();
                                        foreach (DataRow row in dt.Rows)
                                        {
                                            var bom = new BillOfMaterial();
                                            bom.CustAssyNo = custAssm.CustAssyNo;
                                            bom.CustomerPartNo = string.IsNullOrEmpty(row["Cust Part No"].ToString().Trim()) ? "" : row["Cust Part Name"].ToString().Trim();
                                            bom.CustomerPartName = string.IsNullOrEmpty(row["Cust Part Name"].ToString().Trim()) ? "" : row["Cust Part Name"].ToString().Trim();
                                            bom.LocalPartNo = string.IsNullOrEmpty(row["Local Part No"].ToString().Trim()) ? "" : row["Local Part No"].ToString().Trim();
                                            //bom.SettlementRef = string.IsNullOrEmpty(row["Settlement Ref"].ToString().Trim()) ? "" : row["Local Part No"].ToString().Trim();
                                            bom.LocalPartName = string.IsNullOrEmpty(row["Local Part Name"].ToString().Trim()) ? "" : row["Local Part Name"].ToString().Trim();
                                            bom.Quantity = string.IsNullOrEmpty(row["Qty In Assy"].ToString().Trim()) ? 0 : Convert.ToInt32(row["Qty In Assy"].ToString().Trim());
                                            bom.UOM = string.IsNullOrEmpty(row["UOM"].ToString().Trim()) ? "" : row["UOM"].ToString().Trim();
                                            bom.RMUOM = string.IsNullOrEmpty(row["RM_UOM"].ToString().Trim()) ? "" : row["RM_UOM"].ToString().Trim();
                                            bom.RawMaterial = string.IsNullOrEmpty(row["RM Code"].ToString().Trim()) ? "" : row["RM Code"].ToString().Trim();
                                            bom.Commodity = string.IsNullOrEmpty(row["RMCommodity"].ToString().Trim()) ? "" : row["RMCommodity"].ToString().Trim();
                                            bom.BOMQuantity = string.IsNullOrEmpty(row["RM Qty/part"].ToString().Trim()) ? 0 : Convert.ToDecimal(row["RM Qty/part"].ToString().Trim());
                                            bom.TotalRMqty = string.IsNullOrEmpty(row["Total RM Qty/Assy"].ToString().Trim()) ? 0 : Convert.ToDecimal(row["Total RM Qty/Assy"].ToString().Trim());
                                            bom.Scarp = string.IsNullOrEmpty(row["Scrap Commodity"].ToString().Trim()) ? "" : row["Scrap Commodity"].ToString().Trim();
                                            bom.ScrapQuantity = string.IsNullOrEmpty(row["Scrap Qty/part"].ToString().Trim()) ? 0 : Convert.ToDecimal(row["Scrap Qty/part"].ToString().Trim());
                                            bom.ChildPartRate = string.IsNullOrEmpty(row["Cost/child part"].ToString().Trim()) ? 0 : Convert.ToDecimal(row["Cost/child part"].ToString().Trim());
                                            //bom.ToalCost = Convert.ToDecimal(row["Total cost"].ToString());
                                            bom.ToalCost = string.IsNullOrEmpty(row["Total cost of child parts(INR)"].ToString().Trim()) ? 0 : Convert.ToDecimal(row["Total cost of child parts(INR)"].ToString().Trim());
                                            bom.TotalcostinPurCurr = string.IsNullOrEmpty(row["Total Cost in Purchasing Currency"].ToString().Trim()) ? 0 : Convert.ToDecimal(row["Total Cost in Purchasing Currency"].ToString().Trim());
                                            bom.Exchangerate = string.IsNullOrEmpty(row["Exchange rate"].ToString().Trim()) ? 0 : Convert.ToDecimal(row["Exchange rate"].ToString().Trim());
                                            bom.ChildpartCost = string.IsNullOrEmpty(row["Cost/child part"].ToString().Trim()) ? 0 : Convert.ToDecimal(row["Cost/child part"].ToString().Trim());
                                            //bom.CurrencyCode = row["Purchasing Currency"].ToString();
                                            bom.CurrencyCode = string.IsNullOrEmpty(row["Purchasing Currency"].ToString().Trim()) ? "" : row["Purchasing Currency"].ToString().Trim();
                                            bom.Scraptotalqty = string.IsNullOrEmpty(row["Total Scrap Qty/Assy"].ToString().Trim()) ? 0 : Convert.ToDecimal(row["Total Scrap Qty/Assy"].ToString().Trim());
                                            //bom.TotalScrapQty = string.IsNullOrEmpty(row["Total Scrap Qty/Assy"].ToString().Trim()) ? 0 : Convert.ToDecimal(row["Total Scrap Qty/Assy"].ToString().Trim());
                                            

                                            custAssm.BOM.Add(bom);

                                        }

                                        //entities.Add(custAssm);
                                        //custAssm.BOM = bomEntities;

                                        context.CustomerAssembly.Add(custAssm);
                                        context.SaveChanges();
                                        }

                                    }
                                    catch (Exception ex)
                                    {
                                        Xceed.Wpf.Toolkit.MessageBox.Show("Failed to upload data.", "BOM Info", MessageBoxButton.OK, MessageBoxImage.Information);

                                        //  Dispatcher.BeginInvoke(new Action(() =>
                                        //{
                                        //    Xceed.Wpf.Toolkit.MessageBox.Show("Failed to upload data.", "BOM Info", MessageBoxButton.OK, MessageBoxImage.Information);
                                        //}));
                                        //   err = 1;
                                    }
                                    count++;
                                }
                                //var options = new BulkInsertOptions
                                //{
                                //    EnableStreaming = true
                                //};

                                //context.BulkInsert(entities, options);
                                //context.BulkInsert(bomEntities, options);

                            }
                        };



                        worker.RunWorkerCompleted += worker_RunWorkerCompleted;
                        worker.RunWorkerAsync();


                    }
                    catch (Exception exc)
                    {
                        imgLoading.Visibility = Visibility.Hidden;
                        lblFileName.Text = "Unable uploaded successfully.......";
                        Xceed.Wpf.Toolkit.MessageBox.Show("Failed To uploaded .......", "BOM Info", MessageBoxButton.OK, MessageBoxImage.Information);

                    }


                }

                else
                {
                    worker.Dispose();
                    worker = null;
                }
            }
            else
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("Please wait background save process is running.....!", "BOM Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }

        }


        private void worker_RunWorkerCompleted(object sender,
                                       RunWorkerCompletedEventArgs e)
        {

            imgLoading.Visibility = Visibility.Hidden;
            
            if (custAssm != null)
            {
                lblFileName.Text = "Data uploaded successfully.......";
                Xceed.Wpf.Toolkit.MessageBox.Show("Data uploaded successfully.......", "BOM Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                lblFileName.Text = "Failed uploaded successfully.......";
                Xceed.Wpf.Toolkit.MessageBox.Show("Failed uploaded successfully Either File is open/invaild data/File is corrupted.......", "BOM Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            worker.Dispose();
            worker = null;
            GC.Collect();
        }
        public static void RemoveNullColumnFromDataTable(DataTable dt)
        {
            for (int i = dt.Rows.Count - 1; i >= 0; i--)
            {
                if (dt.Rows[i][1] == DBNull.Value)
                    dt.Rows[i].Delete();
            }
            dt.AcceptChanges();
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
    }
}
