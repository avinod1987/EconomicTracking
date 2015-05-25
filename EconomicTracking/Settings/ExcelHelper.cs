
namespace EconomicTracking
{
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml;


    public class ExcelHelper
    {
        private static readonly ExcelHelper _instance = new ExcelHelper();

        public static ExcelHelper Instance
        {
            get { return _instance; }
        }
        /// 
        /// Create one Excel-XML-Document with SpreadsheetML from a DataTable
        /// 
        /// Datasource which would be exported in Excel
        /// Name of exported file
        public void Create(DataTable dtSource, string strFileName)
        {
            // Create XMLWriter
            using (XmlTextWriter xtwWriter = new XmlTextWriter(strFileName, Encoding.UTF8))
            {

                //Format the output file for reading easier
                xtwWriter.Formatting = Formatting.Indented;

                // 
                xtwWriter.WriteStartDocument();

                // Adding processing information 
                xtwWriter.WriteProcessingInstruction("mso-application", "progid='Excel.Sheet'");

                // Adding root element
                xtwWriter.WriteStartElement("Workbook");
                xtwWriter.WriteAttributeString("xmlns", "urn:schemas-microsoft-com:office:spreadsheet");
                xtwWriter.WriteAttributeString("xmlns", "o", null, "urn:schemas-microsoft-com:office:office");
                xtwWriter.WriteAttributeString("xmlns", "x", null, "urn:schemas-microsoft-com:office:excel");
                xtwWriter.WriteAttributeString("xmlns", "ss", null, "urn:schemas-microsoft-com:office:spreadsheet");
                xtwWriter.WriteAttributeString("xmlns", "html", null, "http://www.w3.org/TR/REC-html40");

                // 
                xtwWriter.WriteProcessingInstruction("mso-application", "progid=\"Excel.Sheet\"");

                // 
                xtwWriter.WriteStartElement("DocumentProperties", "urn:schemas-microsoft-com:office:office");

                // Write document properties
                xtwWriter.WriteElementString("Author", "Economic Tracking");
                xtwWriter.WriteElementString("LastAuthor", "Economic Tracking");
                xtwWriter.WriteElementString("Created", DateTime.Now.ToString("u") + "Z");
                xtwWriter.WriteElementString("Company", "XXXXXXXXXX");
                xtwWriter.WriteElementString("Version", "12");

                // 
                xtwWriter.WriteEndElement();

                // 
                xtwWriter.WriteStartElement("ExcelWorkbook", "urn:schemas-microsoft-com:office:excel");

                // Write settings of workbook
                xtwWriter.WriteElementString("WindowHeight", "8010");
                xtwWriter.WriteElementString("WindowWidth", "14805");
                xtwWriter.WriteElementString("WindowTopX", "240");
                xtwWriter.WriteElementString("WindowTopY", "105");
                xtwWriter.WriteElementString("ProtectStructure", "False");
                xtwWriter.WriteElementString("ProtectWindows", "False");

                // 
                xtwWriter.WriteEndElement();

                // 
                xtwWriter.WriteStartElement("Styles");

                // 
                xtwWriter.WriteStartElement("Style");
                xtwWriter.WriteAttributeString("ss", "ID", null, "Default");
                xtwWriter.WriteAttributeString("ss", "Name", null, "Normal");

                // 
                xtwWriter.WriteStartElement("Alignment");
                xtwWriter.WriteAttributeString("ss", "Vertical", null, "Bottom");
                xtwWriter.WriteEndElement();

                // Write null on the other properties
                xtwWriter.WriteElementString("Borders", null);
                xtwWriter.WriteElementString("Font", null);
                xtwWriter.WriteElementString("Interior", null);
                xtwWriter.WriteElementString("NumberFormat", null);
                xtwWriter.WriteElementString("Protection", null);


                // 
                xtwWriter.WriteEndElement();

                //
                xtwWriter.WriteStartElement("Style");
                xtwWriter.WriteAttributeString("ss", "ID", null, "s16");
                xtwWriter.WriteStartElement("Font");
                xtwWriter.WriteAttributeString("ss", "Bold", null, "1");
                xtwWriter.WriteAttributeString("ss", "Size", null, "11");
                xtwWriter.WriteAttributeString("ss", "Underline", null, "Single");
                xtwWriter.WriteEndElement();

                // 
                xtwWriter.WriteEndElement();


                // 
                xtwWriter.WriteEndElement();

                // 
                xtwWriter.WriteStartElement("Worksheet");
                xtwWriter.WriteAttributeString("ss", "Name", null, dtSource.TableName);

                // 
                xtwWriter.WriteStartElement("Table");
                xtwWriter.WriteAttributeString("ss", "ExpandedColumnCount", null, dtSource.Columns.Count.ToString());
                xtwWriter.WriteAttributeString("ss", "ExpandedRowCount", null, (dtSource.Rows.Count + 1).ToString());
                xtwWriter.WriteAttributeString("x", "FullColumns", null, "1");
                xtwWriter.WriteAttributeString("x", "FullRows", null, "1");
                //xtwWriter.WriteAttributeString("ss", "DefaultColumnWidth", null, "60");

                // Run through all rows of data source


                // 
                xtwWriter.WriteStartElement("Row");
                foreach (DataColumn Header in dtSource.Columns)
                {
                    // 
                    xtwWriter.WriteStartElement("Cell");
                    xtwWriter.WriteAttributeString("ss", "StyleID", null, "s16");

                    // xxx
                    xtwWriter.WriteStartElement("Data");
                    xtwWriter.WriteAttributeString("ss", "Type", null, "String");
                    // Write content of cell
                    xtwWriter.WriteValue(Header.ColumnName);

                    // 
                    xtwWriter.WriteEndElement();

                    // 
                    xtwWriter.WriteEndElement();
                }

                xtwWriter.WriteEndElement();


                foreach (DataRow row in dtSource.Rows)
                {
                    // 
                    xtwWriter.WriteStartElement("Row");

                    // Run through all cell of current rows
                    foreach (object cellValue in row.ItemArray)
                    {
                        // 
                        xtwWriter.WriteStartElement("Cell");
                        //if (cnt == 0)
                        //    xtwWriter.WriteAttributeString("ss", "StyleID", null, "s16");

                        // 
                        xtwWriter.WriteStartElement("Data");
                        xtwWriter.WriteAttributeString("ss", "Type", null, "String");
                        // Write content of cell
                        string strcellValue = (cellValue == System.DBNull.Value ? string.Empty : (string)cellValue);
                        xtwWriter.WriteValue(strcellValue);

                        // 
                        xtwWriter.WriteEndElement();

                        // 
                        xtwWriter.WriteEndElement();

                        xtwWriter.WriteEndElement();
                    }
                    // 
                    xtwWriter.WriteEndElement();
                }

                xtwWriter.WriteEndElement();

                // 
                xtwWriter.WriteStartElement("WorksheetOptions", "urn:schemas-microsoft-com:office:excel");

                // Write settings of page
                xtwWriter.WriteStartElement("PageSetup");
                xtwWriter.WriteStartElement("Header");
                xtwWriter.WriteAttributeString("x", "Margin", null, "0.4921259845");
                xtwWriter.WriteEndElement();
                xtwWriter.WriteStartElement("Footer");
                xtwWriter.WriteAttributeString("x", "Margin", null, "0.4921259845");
                xtwWriter.WriteEndElement();
                xtwWriter.WriteStartElement("PageMargins");
                xtwWriter.WriteAttributeString("x", "Bottom", null, "0.984251969");
                xtwWriter.WriteAttributeString("x", "Left", null, "0.78740157499999996");
                xtwWriter.WriteAttributeString("x", "Right", null, "0.78740157499999996");
                xtwWriter.WriteAttributeString("x", "Top", null, "0.984251969");
                xtwWriter.WriteEndElement();
                xtwWriter.WriteEndElement();

                // 
                xtwWriter.WriteElementString("Selected", null);

                // 
                xtwWriter.WriteStartElement("Panes");

                // 
                xtwWriter.WriteStartElement("Pane");

                // Write settings of active field
                xtwWriter.WriteElementString("Number", "1");
                xtwWriter.WriteElementString("ActiveRow", "1");
                xtwWriter.WriteElementString("ActiveCol", "1");

                // 
                xtwWriter.WriteEndElement();

                // 
                xtwWriter.WriteEndElement();

                // False
                xtwWriter.WriteElementString("ProtectObjects", "False");

                // False
                xtwWriter.WriteElementString("ProtectScenarios", "False");

                // 
                xtwWriter.WriteEndElement();

                // 
                xtwWriter.WriteEndElement();

                // 
                xtwWriter.WriteEndElement();

                // Write file on hard disk
                xtwWriter.Flush();
                xtwWriter.Close();
            }
        }
    }

    public static class My_DataTable_Extensions
    {
        /// <summary>
        /// Export DataTable to Excel file
        /// </summary>
        /// <param name="DataTable">Source DataTable</param>
        /// <param name="ExcelFilePath">Path to result file name</param>
        public static void ExportToExcel(this System.Data.DataTable DataTable, string ExcelFilePath = null)
        {
            try
            {
                int ColumnsCount;

                if (DataTable == null || (ColumnsCount = DataTable.Columns.Count) == 0)
                    throw new Exception("ExportToExcel: Null or empty input table!\n");

                // load excel, and create a new workbook
                Microsoft.Office.Interop.Excel.Application Excel = new Microsoft.Office.Interop.Excel.Application();
                Excel.Workbooks.Add();

                // single worksheet
                Microsoft.Office.Interop.Excel._Worksheet Worksheet = Excel.ActiveSheet;

                object[] Header = new object[ColumnsCount];

                // column headings               
                for (int i = 0; i < ColumnsCount; i++)
                    Header[i] = DataTable.Columns[i].ColumnName;

                Microsoft.Office.Interop.Excel.Range HeaderRange = Worksheet.get_Range((Microsoft.Office.Interop.Excel.Range)(Worksheet.Cells[1, 1]), (Microsoft.Office.Interop.Excel.Range)(Worksheet.Cells[1, ColumnsCount]));
                HeaderRange.Value = Header;
                HeaderRange.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray);
                HeaderRange.Font.Bold = true;
               // HeaderRange.EntireColumn.NumberFormat = "mmm-yy";
                // DataCells
                int RowsCount = DataTable.Rows.Count;
                object[,] Cells = new object[RowsCount, ColumnsCount];

                for (int j = 0; j < RowsCount; j++)
                    for (int i = 0; i < ColumnsCount; i++)
                        Cells[j, i] = DataTable.Rows[j][i];
              
                Worksheet.get_Range((Microsoft.Office.Interop.Excel.Range)(Worksheet.Cells[2, 1]), (Microsoft.Office.Interop.Excel.Range)(Worksheet.Cells[RowsCount + 1, ColumnsCount])).Value = Cells;
                
                // check fielpath
                if (ExcelFilePath != null && ExcelFilePath != "")
                {
                    try
                    {
                        Worksheet.SaveAs(ExcelFilePath);
                        Excel.Quit();
                        System.Windows.MessageBox.Show("Excel file saved!");
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("ExportToExcel: Excel file could not be saved! Check filepath.\n"
                            + ex.Message);
                    }
                }
                else    // no filepath is given
                {
                    Excel.Visible = true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("ExportToExcel: \n" + ex.Message);
            }
        }
    }
}
