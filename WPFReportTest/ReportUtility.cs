using System;
using System.Windows;
using System.Windows.Forms.VisualStyles;
using CrystalDecisions.CrystalReports.Engine;
using EconomicTracking.Dal;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Data;
namespace WPFReportTest
{
    public class ReportUtility
    {
        public static void DisplaySettlementreport(ReportClass rc, object objDataSource, Window parentWindow, object ObjSource1 = null, object ObjSource2 = null, object ObjSource3 = null, object ObjSource4 = null)
        {
            ReportViewerUI Viewer = new ReportViewerUI();
            try
            {
                rc.Database.Tables["EconomicTracking_Dal_SettlementModel"].SetDataSource(objDataSource);

                if (ObjSource1 != null)
                    rc.Database.Tables["EconomicTracking_Dal_SettlementCurrencyModel"].SetDataSource(ObjSource1);
                if (ObjSource2 != null)
                    rc.Database.Tables["EconomicTracking_Dal_SettlementCommodityModel"].SetDataSource(ObjSource2);
                if (ObjSource3 != null)
                    rc.Database.Tables["EconomicTracking_Dal_SettlementScarpModel"].SetDataSource(ObjSource3);

                Viewer.setReportSource(rc);

                Viewer.ShowDialog();
            }
            catch (Exception ex)
            {
                Viewer.Close();
                Xceed.Wpf.Toolkit.MessageBox.Show("Loading failed......!", "Error in Reports", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public static void DisplaySalesQtyReports(ReportClass rc, object objDataSource)
        {
            ReportViewerUI Viewer = new ReportViewerUI();
            try
            {

                rc.Database.Tables["EconomicTracking_Dal_SalesQtyModel"].SetDataSource(objDataSource);
                Viewer.setReportSource(rc);
                Viewer.ShowDialog();
            }
            catch (Exception ex)
            {
                Viewer.Close();
                Xceed.Wpf.Toolkit.MessageBox.Show("Loading failed......!", "Error in Reports", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public static void DisplayBOMReports(ReportClass rc, List<CustomerAssemblyModel> objDataSource)
        {
            ReportViewerUI Viewer = new ReportViewerUI();
            try
            {
                rc.Database.Tables["EconomicTracking_Dal_BillOfMaterialModel"].SetDataSource(objDataSource[0].BOM);
                rc.Database.Tables["EconomicTracking_Dal_CustomerAssemblyModel"].SetDataSource(objDataSource);
                Viewer.setReportSource(rc);
                Viewer.ShowDialog();
            }
            catch (Exception ex)
            {
                Viewer.Close();
                Xceed.Wpf.Toolkit.MessageBox.Show("Loading failed......!", "Error in Reports", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public static void DisplayCommReports(ReportClass rc, DataSet ds)
        {
            ReportViewerUI Viewer = new ReportViewerUI();
            try
            {
                rc.Database.Tables["CommdityRecepie"].SetDataSource(ds.Tables[0]);
                Viewer.setReportSource(rc);
                Viewer.ShowDialog();
            }
            catch (Exception ex)
            {
                Viewer.Close();
                Xceed.Wpf.Toolkit.MessageBox.Show("Loading failed......!", "Error in Reports", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
