using System.Windows;
using System.Windows.Controls.Primitives;
namespace WPFReportTest
{
    public partial class ReportViewerUI : Window
    {
        public ReportViewerUI()
        {
            InitializeComponent();
        }
        public void setReportSource(CrystalDecisions.CrystalReports.Engine.ReportDocument aReport)
        {
            this.crystalReportsViewer.ViewerCore.ReportSource = aReport;
        }
    }
}
