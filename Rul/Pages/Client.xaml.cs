using Rul.Entities;
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
using System.Xaml;

using Rul.Entities;
using Rul.Windows;

namespace Rul.Pages
{
    /// <summary>
    /// Логика взаимодействия для Client.xaml
    /// </summary>
    public partial class Client : Page
    {
        User user = new User();
        public Client(User Currentuser)
        {
            InitializeComponent();

            var product = mssql_script_tradeEntities.GetContext().Product.ToList();

            LViewProduct.ItemsSource = product;
            user=Currentuser;
            DataContext = this;

            txtAllAmount.Text =product.Count().ToString();   
            UpdateData();

            User();

        }

        List<Product> orderProducts=new List<Product>();

        
        private void User() {

            if (user!=null)
            {
                txtFullname.Text=user.UserSurname.ToString()+user.UserName.ToString()+" "+user.UserPatronymic.ToString();
            }
            else
            {
                txtFullname.Text = "Гость";
            }
                  
        }

        private void LViewProduct_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateData();


        }

        public string[] SortingList { get; set; } = {
            "Без сортировки",
            "стоимость по возрастанию",
            "стоимость по убыванию",
        };


        public string[] FilterList { get; set; } = {
            "Все диапазоны",
            "0%-9,99%",
            "10%-14,99%",
            "15% и более",
        };

        private void UpdateData()
        {
            var result = mssql_script_tradeEntities.GetContext().Product.ToList();
            var search = txtSearch.Text.ToLower();
            if (cmbSorting.SelectedIndex == 1)
                result = result.OrderBy(p => p.ProductCost).ToList();
            if (cmbSorting.SelectedIndex == 2)
                result = result.OrderByDescending(p => p.ProductCost).ToList();


            if (cmbFilter.SelectedIndex == 1)
                result = result.Where(p => p.ProductDiscountAmount >= 0 && p.ProductDiscountAmount < 10).ToList();
            if (cmbFilter.SelectedIndex==2)
                result = result.Where(p => p.ProductDiscountAmount >= 10 && p.ProductDiscountAmount < 15).ToList();
            if (cmbFilter.SelectedIndex==3)
                result = result.Where(p => p.ProductDiscountAmount >= 15).ToList();


            result = result.Where(p => p.ProductName.ToLower().Contains(txtSearch.Text.ToLower())).ToList();
            LViewProduct.ItemsSource=result;

            txtResultAmount.Text=result.Count().ToString();


        }


      

        private void cmbFilter_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            UpdateData();

        }

        private void cmbSorting_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateData();

        }

        private void txtSearch_SelectionChanged(object sender, RoutedEventArgs e)
        {
            UpdateData();


        }

        //private void btnAddProduct_Click(object sender, RoutedEventArgs e)
        //{

        //    orderProducts.Add(LViewProduct.SelectedItem as Product);
        //    if (orderProducts.Count>0)
        //    {
        //        btnOrder.Visibility=Visibility .Visible;

        //    }

        //}

        private void btnAddProduct_Click(object sender, RoutedEventArgs e)
        {
            if (LViewProduct.SelectedItem is Product selectedProduct)
            {
                if (!orderProducts.Any(p => p.ProductArticleNumber == selectedProduct.ProductArticleNumber))
                {
                    orderProducts.Add(selectedProduct);
                    MessageBox.Show($"Товар {selectedProduct.ProductName} добавлен в корзину!",
                                  "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);

                    btnOrder.Visibility = Visibility.Visible;
                    btnOrder.Content = $"Оформить заказ ({orderProducts.Count})";
                }
                else
                {
                    MessageBox.Show("Этот товар уже в корзине!",
                                  "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        //private void btnOrder_Click(object sender, RoutedEventArgs e)
        //{

        //    OrderWindow order = new OrderWindow(orderProducts, user);
        //    order.Show();
        //}
        private void btnOrder_Click(object sender, RoutedEventArgs e)
        {
            if (orderProducts.Count == 0)
            {
                MessageBox.Show("Корзина пуста!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            //var orderWindow = new OrderWindow(orderProducts, user);
            //if (orderWindow.ShowDialog() == true)

            OrderWindow order = new OrderWindow(orderProducts, user);
            order.Show();

            
            //{
            //    // Если заказ успешно оформлен
            //    orderProducts.Clear();
            //    btnOrder.Visibility = Visibility.Collapsed;
            //    //txtCartCount.Text = "0";
            //}


            
        }
    }
}
