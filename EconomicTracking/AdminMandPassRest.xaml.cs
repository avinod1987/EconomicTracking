using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace EconomicTracking
{
    /// <summary>
    /// Interaction logic for AdminMandPassRest.xaml
    /// </summary>
    public partial class AdminMandPassRest : Window
    {
        public AdminMandPassRest()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtPassword.Password) && !string.IsNullOrEmpty(txtPassword.Password))
            {
                this.Close();
                var frm = new MainWindow();
                App.Current.MainWindow = frm;
                frm.Show();
            }
            else
            {
                MessageBox.Show("Need To Create Password");
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
