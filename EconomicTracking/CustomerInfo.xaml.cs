
using EconomicTracking.Dal;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
namespace EconomicTracking
{
    /// <summary>
    /// Interaction logic for CustomerInfo.xaml
    /// </summary>
    public partial class CustomerInfo : UserControl
    {
        public CustomerInfo()
        {
            
            InitializeComponent();
            txtName.Focus();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var context = ContextHelper.GetContext())
                {
                    var customer = new Customer();
                    int i = context.Customers.Count()+1;
                    customer.Id = "CusRef_" + i;
                    customer.Name = txtName.Text;
                    customer.Location = txtLocation.Text;
                    customer.Phone = txtMobile.Text;
                    customer.Pincode = txtPincode.Text;
                    customer.Company = txtCompany.Text;
                   // customer.Address = txtAddress.Text;
                    customer.Email = txtEmail.Text;


                    context.Customers.Add(customer);
                    await context.SaveChangesAsync();
                    Xceed.Wpf.Toolkit.MessageBox.Show("Sucessfully Added the Customer");
                }
            }
            catch (DbEntityValidationException ex)
            {
                List<string> errorMessages = new List<string>();
                foreach (DbEntityValidationResult validationResult in ex.EntityValidationErrors)
                {
                    string entityName = validationResult.Entry.Entity.GetType().Name;
                    foreach (DbValidationError error in validationResult.ValidationErrors)
                    {
                        errorMessages.Add(entityName + "." + error.PropertyName + ": " + error.ErrorMessage);
                    }
                }
            }

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            txtAddress.Text = ""; txtCompany.Text = ""; txtEmail.Text = ""; txtLocation.Text = ""; txtMobile.Text = ""; txtName.Text = ""; txtPincode.Text = "";
        }
    }
}
