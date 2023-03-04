using DemoToiletPaper2023.db;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DemoToiletPaper2023.Pages
{
    public partial class AddListWindow : Window
    {
        public static ToiletPaper_dbEntities db = new ToiletPaper_dbEntities();
        OpenFileDialog ofdImage = new OpenFileDialog();
        public AddListWindow()
        {
            InitializeComponent();
            foreach (var serv in ProductListWindow.db.TypeProd)
            {
                TypeCB.ItemsSource = db.TypeProd.ToList();
            }
            foreach (var serv in ProductListWindow.db.Material)
            {
                MaterialCB.ItemsSource = db.Material.ToList();
            }
        }

        private void btn_Image_Click(object sender, RoutedEventArgs e)
        {
            ofdImage.Filter = "Image files|*.bmp;*.jpg;*.png|All files|*.*";
            ofdImage.FilterIndex = 1;
            if (ofdImage.ShowDialog() == true)
            {
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.UriSource = new Uri(ofdImage.FileName);
                image.EndInit();
                playim.Source = image;
            }
        }

        private void PriceTB_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void btn_ImageDel_Click(object sender, RoutedEventArgs e)
        {
            BitmapImage image = new BitmapImage();
            image.Freeze();
            playim.Source = image;
        }

        private void btn_Create_Click(object sender, RoutedEventArgs e)
        {
            if (NameTB.Text == "" || PriceTB.Text == "" || TypeCB.Text == "" || ArticleTB.Text == "")
            {
                MessageBox.Show("Введите ваши данные!");
            }
            else
            {
                try
                {
                    Product prod = new Product();

                    prod.Name = NameTB.Text;
                    prod.MinCostForAgent = Convert.ToInt32(PriceTB.Text);
                    var TypeName1 = MaterialCB.SelectedItem;
                    var temp1 = ((Material)TypeName1).id;
                    prod.Id_Material = temp1;
                    prod.Id_Prod = Convert.ToInt32(ArticleTB.Text);
                    var TypeName = TypeCB.SelectedItem;
                    var temp = ((TypeProd)TypeName).Id;
                    prod.Id_Type = temp;
                    prod.Count = Convert.ToInt32(CountTB.Text);

                    prod.Picture = File.ReadAllBytes(ofdImage.FileName);
                    db.Product.Add(prod);
                    db.SaveChanges();
                    MessageBox.Show("Complete");
                }
                catch
                {
                    MessageBox.Show("Не корректные данные!");
                }

            }
        }

        private void btn_Clear_Click(object sender, RoutedEventArgs e)
        {
            NameTB.Clear();
            PriceTB.Clear();
            ArticleTB.Clear();
            BitmapImage image = new BitmapImage();
            image.Freeze();
            playim.Source = image;
        }

        private void TypeCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var typeName = ((TypeProd)TypeCB.SelectedItem).Id;
            var type = ProductListWindow.db.TypeProd.Where(x => x.Id == typeName).FirstOrDefault();
        }

        private void MaterialCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var typeName = ((Material)MaterialCB.SelectedItem).id;
            var type = ProductListWindow.db.Material.Where(x => x.id == typeName).FirstOrDefault();
        }

        private void BackBTN_Click(object sender, RoutedEventArgs e)
        {
            ProductListWindow productList = new ProductListWindow();
            productList.Show();
            this.Close();
        }

        private void CountTB_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

        }
    }
}
