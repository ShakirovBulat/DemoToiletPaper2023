﻿using DemoToiletPaper2023.db;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace DemoToiletPaper2023.Pages
{
    /// <summary>
    /// Логика взаимодействия для ProductListWindow.xaml
    /// </summary>
    public partial class ProductListWindow : Window
    {
        public static ToiletPaper_dbEntities db = new ToiletPaper_dbEntities();
        Product prod1;

        public ProductListWindow()
        {
            InitializeComponent();
            prod1 = new Product();
            PaperLst.ItemsSource = db.Product.ToList();
            RefreshComboBox();
            RefreshButtons();
            foreach (var serv in ProductListWindow.db.TypeProd)
            {
                FilterCB.ItemsSource = db.TypeProd.ToList();
            }
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(PaperLst.ItemsSource);
            view.Filter = UserFilter;
        }

        private void BLeft_Click(object sender, RoutedEventArgs e)
        {
            if (pageNumber == 0)
                return;
            pageNumber--;
            RefreshPagination();
        }

        private void BRight_Click(object sender, RoutedEventArgs e)
        {
            if (prod.Count % pageSize == 0)
            {
                if (pageNumber == (prod.Count / pageSize) - 1)
                    return;
            }
            else
            {

                if (pageNumber == (prod.Count / pageSize))
                    return;
            }
            pageNumber++;
            RefreshPagination();
        }

        private void CBNumberWrite_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            pageSize = Convert.ToInt32(CBNumberWrite.SelectedItem.ToString());
            RefreshPagination();
            RefreshButtons();
        }

        int pageSize;
        int pageNumber;
        List<Product> prod = db.Product.ToList();

        private void RefreshPagination()
        {
            PaperLst.ItemsSource = null;
            PaperLst.ItemsSource = prod.Skip(pageNumber * pageSize).Take(pageSize).ToList();
        }

        private void RefreshComboBox()
        {
            CBNumberWrite.Items.Add("15");
        }

        private void RefreshButtons()
        {
            WPButtons.Children.Clear();
            if (prod.Count % pageSize == 0)
            {
                for (int i = 0; i < prod.Count / pageSize; i++)
                {
                    Button button = new Button();
                    button.Content = i + 1;
                    button.Click += Button_Click;
                    button.Margin = new Thickness(0, 5, 0, 5);
                    button.Width = 25;
                    button.Height = 25;
                    button.BorderBrush = Brushes.White;
                    button.Background = Brushes.White;
                    button.FontSize = 8;
                    WPButtons.Children.Add(button);
                }
            }
            else
            {
                for (int i = 0; i < prod.Count / pageSize; i++)
                {
                    Button button = new Button();
                    button.Content = i + 1;
                    button.Click += Button_Click;
                    button.Margin = new Thickness(0, 5, 0, 5);
                    button.Width = 25;
                    button.Height = 25;
                    button.BorderBrush = Brushes.White;
                    button.Background = Brushes.White;
                    button.FontSize = 8;
                    WPButtons.Children.Add(button);
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            pageNumber = Convert.ToInt32(button.Content) - 1;
            RefreshPagination();
        }

        private bool UserFilter(object item)
        {
            if (String.IsNullOrEmpty(SearchTB.Text))
                return true;
            else
                return ((item as Product).Name.IndexOf(SearchTB.Text, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        private void txtName_TextChanged(object sender, TextChangedEventArgs e)
        {
            var TBSQ = db.Product.OrderBy(a => a.Name).ToList();
            TBSQ = TBSQ.Where(a => a.Name.ToLower().Contains(SearchTB.Text.ToLower())).ToList();
            PaperLst.ItemsSource = TBSQ;
        }

        private void FilterCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var typeName = ((TypeProd)FilterCB.SelectedItem).NameType;
            var type = ProductListWindow.db.TypeProd.Where(x => x.NameType == typeName).FirstOrDefault();
            PaperLst.ItemsSource = db.Product.Where(x => x.TypeProd.NameType == typeName).ToList();
        }

        private void PaperLst_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var insert = PaperLst.SelectedItem as Product;
            InsertListWindow insertwin = new InsertListWindow(insert);
            this.Close();
            insertwin.Show();
        }

        private void btn_add_Click(object sender, RoutedEventArgs e)
        {
            AddListWindow add = new AddListWindow();
            this.Close();
            add.Show();
        }

        private void SortCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SortCB.SelectedIndex == 0)
            {
                PaperLst.ItemsSource = db.Product.ToList();
                CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(PaperLst.ItemsSource);
                view.SortDescriptions.Add(new SortDescription("Product.MinCostForAgent", ListSortDirection.Ascending));


            }
            else if (SortCB.SelectedIndex == 1)
            {
                PaperLst.ItemsSource = db.Product.ToList();
                CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(PaperLst.ItemsSource);
                view.SortDescriptions.Add(new SortDescription("Product.MinCostForAgent", ListSortDirection.Descending));


            }
        }

        private void DelBTN_Click(object sender, RoutedEventArgs e)
        {
            var q = PaperLst.SelectedItem as Product;
            if (q == null)
            {
                MessageBox.Show("Ничего не выбрано!");
                return;
            }
            MessageBoxResult result = MessageBox.Show("Вы действительно хотите удалить строку?", "Удалить?", MessageBoxButton.YesNoCancel);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    db.Product.Remove(q);
                    db.SaveChanges();
                    PaperLst.ItemsSource = db.Product.ToList();
                    MessageBox.Show("Выполнено!");
                }
                catch
                {
                    MessageBox.Show("Ошибка!");
                }
            }
        }
    }
}