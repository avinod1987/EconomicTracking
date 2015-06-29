using EconomicTracking.Dal;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace EconomicTracking
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {

            //Database.SetInitializer(new DropCreateDatabaseIfModelChanges<EconomicsTrackingDbContext>());
            AppDomain.CurrentDomain.SetData("DataDirectory", System.IO.Directory.GetCurrentDirectory());
            if (!Directory.Exists(@"c:\App_Data"))
                Directory.CreateDirectory(@"c:\App_Data");
            var di = new DirectoryInfo(@"c:\App_Data");
            di.Attributes &= ~FileAttributes.ReadOnly;
           // ClearReadOnly(di);
            Database.SetInitializer<EconomicsTrackingDbContext>(null);

                
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            LoginScreen win = new LoginScreen();
            win.ShowDialog();
                if (!win.DialogResult.GetValueOrDefault())
                  {
                      if (win.txtUserName.Text != "Admin" && win.txtPassd.Text!="Admin") {
                          Environment.Exit(0);
                      }
                  }
                else
                {
                    var frm = new MainWindow();
                    Current.MainWindow = frm;
                    frm.Show();
                }

            
        }

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show("An unhandled error just occurred: " + e.Exception.Message, "Exception Sample", MessageBoxButton.OK, MessageBoxImage.Warning);
            e.Handled = true;
        }
        private void ClearReadOnly(DirectoryInfo parentDirectory)
        {
            if (parentDirectory != null)
            {
                parentDirectory.Attributes = FileAttributes.Normal;
                foreach (FileInfo fi in parentDirectory.GetFiles())
                {
                    fi.Attributes = FileAttributes.Normal;
                }
                foreach (DirectoryInfo di in parentDirectory.GetDirectories())
                {
                    ClearReadOnly(di);
                }
            }
        }

        
    }
}
