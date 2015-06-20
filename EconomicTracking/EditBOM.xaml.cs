using EconomicTracking.Dal;
using System;
using System.Collections.Generic;
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
using System.Data.Entity;

namespace EconomicTracking
{
    /// <summary>
    /// Interaction logic for EditBOM.xaml
    /// </summary>
    public partial class EditBOM : UserControl
    {
        int id;
        EconomicsTrackingDbContext context = new EconomicsTrackingDbContext();
        public EditBOM()
        {
            InitializeComponent();
        }

        private async void CusCombo_Loaded(object sender, RoutedEventArgs e)
        {
            CusCombo.ItemsSource = await context.CustomerAssembly.Select(x => x.Customer).Distinct().ToListAsync();
        }

        private async void CusCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            empty();
            var r = Visibility.Hidden;
            addchildbtn.Visibility = r; CustomerPartNamelbl.Visibility = r; CustomerPartNametxt.Visibility = r; Deletebtn.Visibility = r;
            if (CusCombo.SelectedIndex != -1)
            {
                CusAssidCombo.SelectedIndex=-1;
                CuspartnoCombo.SelectedIndex = -1;
                CusAssidCombo.ItemsSource  = await context.CustomerAssembly.Where(x=>x.Customer==CusCombo.SelectedItem.ToString()).Select(x => x.CustAssyName).Distinct().ToListAsync();
            }
        }

        private async void CusAssidCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            empty();
            if (CusAssidCombo.SelectedIndex != -1 && CusCombo.SelectedIndex != -1)
            {
                var cusass = await context.CustomerAssembly.Include("BOM").Where(x => x.CustAssyName == CusAssidCombo.SelectedItem.ToString()).ToListAsync();
                CuspartnoCombo.ItemsSource = (from c in cusass.Select(x => x.BOM)
                           from f in c
                           where f.LocalPartName != string.Empty && f.LocalPartNo != string.Empty && f.Quantity != 0 && f.UOM != string.Empty
                           select f.CustomerPartName ).ToList();
                var r = Visibility.Visible;
                addchildbtn.Visibility = r; CustomerPartNamelbl.Visibility = r; CustomerPartNametxt.Visibility = r;
                Deletebtn.Visibility = Visibility.Hidden;
                addchildbtn.Content = "Add Child Item";

            }
        }

        private async void addchildbtn_Click(object sender, RoutedEventArgs e)
        {
            string mess = addchildbtn.Content.ToString();
            if (mess == "Update Child Item")
            {
                var cusass = await context.CustomerAssembly.Include("BOM").Where(x => x.CustAssyName == CusAssidCombo.SelectedItem.ToString()).ToListAsync();
                var bomtemp = (from c in cusass.Select(x => x.BOM)
                               from f in c
                               where f.LocalPartName != string.Empty && f.LocalPartNo != string.Empty && f.Quantity != 0 && f.UOM != string.Empty
                               select f).Where(x => x.CustomerPartName == CuspartnoCombo.SelectedItem.ToString()).ToList();
                CustomerAssembly cus = new CustomerAssembly();
                cus.Category = cusass.Select(x => x.Category).FirstOrDefault();
                cus.CustAssyName = cusass.Select(x => x.CustAssyName).FirstOrDefault();
                cus.CustAssyNo = cusass.Select(x => x.CustAssyNo).FirstOrDefault();
                cus.Family = cusass.Select(x => x.Family).FirstOrDefault();
                cus.LocalAssyName = cusass.Select(x => x.LocalAssyName).FirstOrDefault();
                cus.LocalAssyNo = cusass.Select(x => x.LocalAssyNo).FirstOrDefault();
                cus.Category = cusass.Select(x => x.Category).FirstOrDefault();
                cus.BOM = new List<BillOfMaterial>();
                var bom = new BillOfMaterial();
                bom.CustAssyNo = cus.CustAssyNo;
                bom.CustomerPartNo = Custpartnotxt.Text;
                bom.CustomerPartName = CuspartnoCombo.SelectedItem.ToString();
                bom.LocalPartNo = localpartnotxt.Text;
                bom.LocalPartName = localpartnametxt.Text;
                bom.Quantity = Convert.ToInt32(qtyinasstxt.Text);
                bom.UOM ="";
                bom.RMUOM = "";
                bom.RawMaterial = "";
                bom.Commodity = Rmcommtxt.Text;
                bom.BOMQuantity = Convert.ToDecimal(rmcommqtytxt.Text);
                bom.TotalRMqty = Convert.ToDecimal(rmcommqtytxt.Text) * Convert.ToInt32(qtyinasstxt.Text);
                bom.Scarp = scrpcommtxt.Text;
                bom.ScrapQuantity = Convert.ToDecimal(Scrapqtytxt.Text);
                bom.Scraptotalqty = Convert.ToDecimal(rmcommqtytxt.Text) * Convert.ToDecimal(Scrapqtytxt.Text);
                bom.ChildPartRate = Convert.ToDecimal(costperchildtxt.Text);
                bom.ToalCost = Convert.ToDecimal(costperchildtxt.Text) * Convert.ToInt32(qtyinasstxt.Text);
                bom.TotalcostinPurCurr = Convert.ToDecimal(costperchildtxt.Text) / Convert.ToDecimal(Exctxt.Text);
                bom.Exchangerate = Convert.ToDecimal(Exctxt.Text);
                cus.BOM.Add(bom); context.SaveChanges();
            }
        }

        private void CuspartnoCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            empty();
            
            if (CusAssidCombo.SelectedIndex != -1 && CusCombo.SelectedIndex != -1 && CuspartnoCombo.SelectedIndex != -1)
            {
            var r = Visibility.Hidden;
            CustomerPartNamelbl.Visibility = r; CustomerPartNametxt.Visibility = r;
            Deletebtn.Visibility = Visibility.Visible;
            displaybom();
            addchildbtn.Content = "Update Child Item";
            }
        }

        private async void displaybom()
        {
            List<string> li = new List<string> { 
            "Gram","KiloGram"
            };
                var cusass = await context.CustomerAssembly.Include("BOM").Where(x => x.CustAssyName == CusAssidCombo.SelectedItem.ToString()).ToListAsync();
                var bomtemp = (from c in cusass.Select(x => x.BOM)
                               from f in c
                               where f.LocalPartName != string.Empty && f.LocalPartNo != string.Empty && f.Quantity != 0 && f.UOM != string.Empty
                               select f).Where(x => x.CustomerPartName == CuspartnoCombo.SelectedItem.ToString()).ToList();
                id = bomtemp.Select(x => x.Id).FirstOrDefault();
                Custpartnotxt.Text = bomtemp.Select(x => x.CustomerPartNo).FirstOrDefault();
                localpartnotxt.Text = bomtemp.Select(x => x.LocalPartNo).FirstOrDefault();
                localpartnametxt.Text = bomtemp.Select(x => x.LocalPartName).FirstOrDefault();
                qtyinasstxt.Text = bomtemp.Select(x => x.BOMQuantity).FirstOrDefault().ToString();
                string s = bomtemp.Select(x => x.RawMaterial).FirstOrDefault();
                string rmuom = bomtemp.Select(x => x.RMUOM).FirstOrDefault();
                string scuom = bomtemp.Select(x => x.UOM).FirstOrDefault();
                scrapUOMcombo.ItemsSource = li; RmUOMcombo.ItemsSource = li;
                if (scuom.ToLower() == "g" || scuom.ToLower() == "gm")
                {
                    scrapUOMcombo.SelectedItem = "Gram";
                }
                if (rmuom.ToLower() == "kg" || rmuom.ToLower() == "kilogram")
                {
                    RmUOMcombo.SelectedItem = "KiloGram";
                }
                Rmcommtxt.Text = context.RMCode.Where(x => x.RMCodeId == s).Select(x => x.RMName).FirstOrDefault();
                rmcommqtytxt.Text = bomtemp.Select(x => x.TotalRMqty).FirstOrDefault().ToString();
                scrpcommtxt.Text = bomtemp.Select(x => x.Scarp).FirstOrDefault();
                Scrapqtytxt.Text = bomtemp.Select(x => x.ScrapQuantity).FirstOrDefault().ToString();
                costperchildtxt.Text = bomtemp.Select(x => x.ChildpartCost).FirstOrDefault().ToString();
                purrcurrtxt.Text = bomtemp.Select(x => x.CurrencyCode).FirstOrDefault().ToString();
                Exctxt.Text = bomtemp.Select(x => x.Exchangerate).FirstOrDefault().ToString();
        }

        private void empty()
        {
            var r = string.Empty;
            scrapUOMcombo.SelectedIndex = -1; RmUOMcombo.SelectedIndex = -1;
            Custpartnotxt.Text = r; localpartnotxt.Text = r; localpartnametxt.Text = r; Rmcommtxt.Text = r; qtyinasstxt.Text = r;
            rmcommqtytxt.Text = r; scrpcommtxt.Text = r; Scrapqtytxt.Text = r; costperchildtxt.Text = r; Exctxt.Text = r; purrcurrtxt.Text = r;
        }

        private void Deletebtn_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Customer Name " + CusCombo.SelectedItem.ToString() + "  ");
            sb.Append("CustomerAssemblyId " + CusAssidCombo.SelectedItem.ToString() + "  ");
            sb.Append("CustomerPartName " + CuspartnoCombo.SelectedItem.ToString() + "  ");
            var b = context.BillOfMaterial.FirstOrDefault(x => x.Id == id) as BillOfMaterial;
            context.BillOfMaterial.Remove(b);
            //context.BillOfMaterial.Remove(b as BillOfMaterial); 
            context.SaveChanges(); empty();
            CusAssidCombo.SelectedIndex = -1; CusCombo.SelectedIndex = -1; CuspartnoCombo.SelectedIndex = -1;          
            MessageBox.Show(sb.ToString());

        }

    }
}
