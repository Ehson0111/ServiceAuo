using Rul.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

namespace Rul.Pages
{
    /// <summary>
    /// Логика взаимодействия для OrderPage.xaml
    /// </summary>
    public partial class OrderPage : Page
    {
        List<Product> productList = new List<Product>();
        int userid;
        public OrderPage(List<Product> products, User user)
        {
            InitializeComponent();
            //DataContext = this;
            //lViewOrder.ItemsSource = productList;       
            //DataC
            //ontext = this;

            userid=user.UserID;
            DataContext = this;
            productList=products;

            lViewOrder.ItemsSource = productList;


            cmbPickupPoint.ItemsSource = mssql_script_tradeEntities.GetContext().PickupPoint.ToList();

            if (user != null)
                txtUser.Text = user.UserSurname.ToString() + user.UserName.ToString() + " " + user.UserSurname.ToString();
        }
        public string Total
        {
            get
            {
                var total = productList.Sum(p => Convert.ToDouble(p.ProductCost) - Convert.ToDouble(p.ProductCost) * Convert.ToDouble(p.ProductDiscountAmount / 100.00));
                return total.ToString();
            }
        }

        private void btnDeleteProduct_Click(object sender, RoutedEventArgs e)
        {

            if (MessageBox.Show("Вы уверены, что хотите удалить этот элемент?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)

            {
                productList.Remove(lViewOrder.SelectedItem as Product);

            }
        }


        //public string Total
        //{
        //    get
        //    {
        //        var total = productlis;

        //    }
        //}


        private void btnOrderSave_Click(object sender, RoutedEventArgs e)
        {
            var productArticle = productList.Select(p => p.ProductArticleNumber).ToArray();

            Random random = new Random();

            var date = DateTime.Now;


            if (productList.Any(p => p.ProductQuantityInStock < 3))
            {
                date = date.AddDays(6);
            }
            else
            {
                date = date.AddDays(3);
            }


            if (cmbPickupPoint.SelectedIndex == null)
            {
                MessageBox.Show("Выберите пункт выдачи!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            try
            {
                Order neworder = new Order()
                {
                    OrderStatusId=1,
                    OrderDate = DateTime.Now,
                    OrderPickupPoint = cmbPickupPoint.SelectedIndex + 1,
                    OrderDeliveryDate = date,   
                                        ReceiotCode=random.Next(100, 1000),

                    //User.=txtUser.Text,    
                    UserId=userid,

                };

                mssql_script_tradeEntities.GetContext().Order.Add(neworder);


                for (int i = 0; i < productArticle.Count(); i++)
                {

                    OrderProduct newOrderProduct = new OrderProduct()
                    {
                        OrderID = neworder.OrderID,
                        ProductArticleNumber = productArticle[i],
                        ProductCount=1
                    };

                    mssql_script_tradeEntities.GetContext().OrderProduct.Add(newOrderProduct);

                }

                mssql_script_tradeEntities.GetContext().SaveChanges();
                MessageBox.Show("Заказ оформлен!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                NavigationService.Navigate(new OrderTicketPage(neworder,productList));

                
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message.ToString());
                //throw;
            }
        }
    }
}
