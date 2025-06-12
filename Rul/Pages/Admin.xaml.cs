 
using Rul.Entities;
using Rul.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace Rul.Pages
{
    public partial class Admin : Page
    {
        User user = new User();
        List<Product> orderProducts = new List<Product>();

        public Admin(User Currentuser)
        {
            InitializeComponent();
            user = Currentuser;
            DataContext = this;

            var product = mssql_script_tradeEntities.GetContext().Product.ToList();
            LViewProduct.ItemsSource = product;
            txtAllAmount.Text = product.Count().ToString();
            UpdateData();
            User();
        }

        private void User()
        {
            if (user != null)
            {
                txtFullname.Text = user.UserSurname.ToString() + user.UserName.ToString() + " " + user.UserPatronymic.ToString();
            }
            else
            {
                txtFullname.Text = "Гость";
            }
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

            if (LViewProduct == null)
            {
                return;
            }

            // Поиск
            string searchText = txtSearch.Text.ToLower();
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                result = result
                    .Where(p => p.ProductName.ToLower().Contains(searchText) ||
                                p.ProductDescription.ToLower().Contains(searchText) ||
                                p.ProductManufacturer.ToLower().Contains(searchText))
                    .ToList();
            }

            // Сортировка
            switch (cmbSorting.SelectedIndex)
            {
                case 1:
                    result = result.OrderBy(p => p.ProductCost).ToList();
                    break;
                case 2:
                    result = result.OrderByDescending(p => p.ProductCost).ToList();
                    break;
            }

            // Фильтрация по скидке
            switch (cmbFilter.SelectedIndex)
            {
                case 1: // 0%-9,99%
                    result = result.Where(p => p.ProductDiscountAmount >= 0 && p.ProductDiscountAmount < 10).ToList();
                    break;
                case 2: // 10%-14,99%
                    result = result.Where(p => p.ProductDiscountAmount >= 10 && p.ProductDiscountAmount < 15).ToList();
                    break;
                case 3: // 15% и более
                    result = result.Where(p => p.ProductDiscountAmount >= 15).ToList();
                    break;
            }

            // Обновляем интерфейс
            LViewProduct.ItemsSource = result;
            txtResultAmount.Text = result.Count.ToString();
            txtAllAmount.Text = mssql_script_tradeEntities.GetContext().Product.Count().ToString();
        }

      

        private void LViewProduct_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateData();
        }

        private void txtSearch_SelectionChanged(object sender, RoutedEventArgs e)
        {
            UpdateData();
        }

        private void cmbSorting_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateData();
        }

        private void cmbFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateData();
        }

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

        private void btnOrder_Click(object sender, RoutedEventArgs e)
        {
            if (orderProducts.Count == 0)
            {
                MessageBox.Show("Корзина пуста!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            OrderWindow order = new OrderWindow(orderProducts, user);
            order.Show();
        }

        private void btnAddNewProduct_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddEditProductPage(null));
        }

        private void LViewProduct_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new AddEditProductPage(LViewProduct.SelectedItem as Product));
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

           
            if (Visibility == Visibility.Visible)
            {
                mssql_script_tradeEntities.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());

                //UpdateData();
            }
        }
    }
}
