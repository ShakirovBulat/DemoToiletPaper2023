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
using System.Windows.Shapes;
using DemoToiletPaper2023.db;

namespace DemoToiletPaper2023.Pages
{
    /// <summary>
    /// Логика взаимодействия для InsertListWindow.xaml
    /// </summary>
    public partial class InsertListWindow : Window
    {
        public static ToiletPaper_dbEntities dBEntities = new ToiletPaper_dbEntities();
        Product prod;
        public InsertListWindow(Product prod)
        {
            InitializeComponent();
            this.prod = prod;
            DataFill();
        }

        void DataFill()
        {
            NameTB.Text = Convert.ToString(prod.Name);
            TypeTB.Text = Convert.ToString(prod.Id_Type);
            ArtcileTB.Text = Convert.ToString(prod.Id_Prod);
            CostTB.Text = Convert.ToString(prod.MinCostForAgent);
            MaterialsTB.Text = Convert.ToString(prod.Id_Material);

        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            Product prod2 = dBEntities.Product.FirstOrDefault();
            prod2 = prod;
            prod2.Name = NameTB.Text;
            prod2.Id_Type = Convert.ToInt32(TypeTB.Text);
            prod2.Id_Prod = Convert.ToInt32(ArtcileTB.Text);
            prod2.MinCostForAgent = Convert.ToInt32(CostTB.Text);
            prod2.Id_Material = Convert.ToInt32(MaterialsTB.Text);

            dBEntities.SaveChanges();
            MessageBox.Show("Выполнено!");

            ProductListWindow product = new ProductListWindow();
            product.PaperLst.ItemsSource = dBEntities.Product.ToList();
        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            ProductListWindow productListWindow = new ProductListWindow();
            productListWindow.Show();
            this.Close();
        }
    }
}
