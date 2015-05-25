using Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace EconomicTracking
{
    public class ExcelReader
    {
        public static DataTable GetDataFromExcelFile(string tableName)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Excel Files|*.xls;*.xlsx;*.xlsm|All files (*.*)|*.*";
            DialogResult res = openFileDialog.ShowDialog();
            if (res == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    var finfo = new FileInfo(openFileDialog.FileName);
                    FileStream stream = File.Open(openFileDialog.FileName, FileMode.Open, FileAccess.Read);

                    IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                    excelReader.IsFirstRowAsColumnNames = true;
                    DataSet result = excelReader.AsDataSet();
                    excelReader.Close();
                    DataTable dt = result.Tables[tableName];
                    dt.Columns.Remove("Column0");
                    dt.Columns.Remove("Column1");
                    dt = RemoveUnusedColumnsAndRows(dt);
                    var removeColumns = new List<string>();
                    int index = 0;
                    bool stopAdding = false;
                    foreach (DataColumn col in dt.Columns)
                    {
                        if (dt.Rows[0][index].ToString().ToLower() == "usd")
                            stopAdding = true;
                        if (!stopAdding)
                            col.ColumnName = dt.Rows[0][index].ToString();
                        else
                            removeColumns.Add(col.ColumnName);
                        index++;
                    }
                    removeColumns.ForEach(x => { dt.Columns.Remove(x); });
                    dt.Rows[0].Delete();
                    dt.AcceptChanges();
                    return dt;
                }
                catch (Exception ex)
                {
                    MessageBoxResult messageBoxResult = Xceed.Wpf.Toolkit.MessageBox.Show("Are sure you closed the file?", "Please close the excel file before uploading", System.Windows.MessageBoxButton.YesNo);
                    if (messageBoxResult == MessageBoxResult.Yes)
                    {
                        GetDataFromExcelFile(tableName);
                    }
                    else
                    {
                        return new DataTable();
                    }
                }
            }
            return new DataTable();
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
