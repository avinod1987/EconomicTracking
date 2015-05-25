using EconomicTracking.Dal;
using Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EconomicTracking
{
    /// <summary>
    /// Interaction logic for UploadFinalReport.xaml
    /// </summary>
    public partial class UploadFinalReport : System.Windows.Controls.UserControl
    {
        public UploadFinalReport()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            DataTable dataTable = new DataTable();
            try
            {
                dataTable = ExcelReader.GetDataFromExcelFile("Report_01");

            }
            catch (Exception ex)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("Error");
            }
            if (dataTable.Rows.Count > 0)
            {
                using (var context = new EconomicsTrackingDbContext())
                {
                    foreach (DataRow row in dataTable.Rows)
                    {
                        var finalReport = new FinalReport();
                        finalReport.CustomerAssyId = row["Cust Part No"].ToString();
                        finalReport.QuoteReference = row["Quote Reference"].ToString();
                        finalReport.InitialQuoteDate = Convert.ToDateTime(row["Initial Quote date"].ToString());
                        finalReport.LastQuoteDate = Convert.ToDateTime(row["Last Settled Quote Date"].ToString());
                        finalReport.LastSettlementRef = row["Last Settlement Reference"].ToString();
                        finalReport.CurrentSettlementRef = row["Current Settlement Reference"].ToString();
                        finalReport.FromPeriod = row["From Period"].ToString();
                        finalReport.ToPeriod = row["To Period"].ToString();
                        finalReport.EffectiveSalesQuantity = row["Effective Sales Qty"].ToString().ToCharArray().Any(char.IsNumber) ? Convert.ToDecimal(row["Effective Sales Qty"].ToString()) : 0;
                        finalReport.Recovery = row["Recovery/part"].ToString().ToCharArray().Any(char.IsNumber) ? Convert.ToDecimal(row["Recovery/part"].ToString()) : 0;
                        finalReport.TotalRecovery = row["Total Recovery"].ToString().ToCharArray().Any(char.IsNumber) ? Convert.ToDecimal(row["Total Recovery"].ToString()) : 0;
                        context.FinalReport.Add(finalReport);
                    }
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}