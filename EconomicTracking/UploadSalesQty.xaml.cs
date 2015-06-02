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
using EntityFramework.BulkInsert.Extensions;
using EntityFramework.Utilities;

namespace EconomicTracking
{
    /// <summary>
    /// Interaction logic for UploadSalesQty.xaml
    /// </summary>
    public partial class UploadSalesQty : System.Windows.Controls.UserControl
    {
        private BackgroundWorker worker = null;
        public UploadSalesQty()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (worker==null)
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
                       
                        
                        DataTable dt = result.Tables["Volumes"];
                        dt = RemoveUnusedColumnsAndRows(dt);
                        int index = 0;
                        foreach (DataColumn col in dt.Columns)
                        {
                            col.ColumnName = dt.Rows[0][index].ToString();
                            index++;
                        }
                        dt.Rows[0].Delete();
                        dt.AcceptChanges();
                        using (var context = new EconomicsTrackingDbContext())
                        {
                            context.Configuration.AutoDetectChangesEnabled = false;
                            context.Configuration.ValidateOnSaveEnabled = false;
                            int count = 0;
                            var entities = new List<SalesQty>();
                            //foreach (DataRow row in dt.Rows)
                            //{
                            //    string cusass1 = row["Part No"].ToString();
                            //    DateTime dat1 = Convert.ToDateTime(row["Date"].ToString());
                            //    int i = context.SalesQty.Where(x => x.CustomerAssemblyId == cusass1 && x.Date == dat1.Date).Count();
                            //    if (i > 0)
                            //    {
                            //        context.SalesQty.RemoveRange(context.SalesQty.Where(x => x.CustomerAssemblyId == cusass1 && x.Date == dat1.Date));
                            //    }
                            //}
                                foreach (DataRow row in dt.Rows)
                                {
                                    try
                                    {
                                        Dispatcher.BeginInvoke(new Action(() =>
                                        {
                                            lblFileName.Text = "Uploading records to database(" + count + "/" + (dt.Rows.Count ) + ")......";
                                        }));
                                        var saleQty = new SalesQty();
                                        string cusass = row["Part No"].ToString();
                                        DateTime dat = Convert.ToDateTime(row["Date"].ToString());  
                                        saleQty.CustomerAssemblyId = row["Part No"].ToString();
                                        saleQty.Date = Convert.ToDateTime(row["Date"].ToString());
                                        saleQty.CustomerName = row["Customer"].ToString();
                                        saleQty.Quantity =string.IsNullOrEmpty(row["Qty"].ToString()) ? 0 : Convert.ToDecimal(row["Qty"].ToString());
                                        context.SalesQty.Add(saleQty);
                                        //entities.Add(saleQty);
                                        
                                    }
                                    catch (Exception ex)
                                    {

                                        Dispatcher.BeginInvoke(new Action(() =>
                                        {
                                            Xceed.Wpf.Toolkit.MessageBox.Show("Failed to upload data.", "BOM Info", MessageBoxButton.OK, MessageBoxImage.Information);
                                        }));
                                    }
                                    
                                    count++;
                                    
                                    
                                }
                            //    var options = new BulkInsertOptions
                            //    {
                            //        EnableStreaming = true
                            //    };
                            ////context.BulkInsert(entities, options);
                            //EFBatchOperation.For(context, context.SalesQty).InsertAll(entities); 
                                //context.SalesQty.AddRange(entities);
                                context.SaveChanges();
                            
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
                Xceed.Wpf.Toolkit.MessageBox.Show("Please wait save process is running.....!", "Sales Quatity", MessageBoxButton.OK, MessageBoxImage.Information);
            }

        }
        private void worker_RunWorkerCompleted(object sender,
                                       RunWorkerCompletedEventArgs e)
        {
            lblFileName.Text = "Data uploaded successfully.......";
            Xceed.Wpf.Toolkit.MessageBox.Show("Data uploaded successfully.......", "SalesQty Info", MessageBoxButton.OK, MessageBoxImage.Information);
            worker.Dispose();
            worker = null;
            GC.Collect();
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
