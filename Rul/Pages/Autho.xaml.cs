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

namespace Rul.Pages
{
    /// <summary>
    /// Логика взаимодействия для Autho.xaml
    /// </summary>
    public partial class Autho : Page
    {

        private int countUnsuccesful = 0;
        public Autho()
        {
            InitializeComponent();

            TextBlockCatcha.Visibility = Visibility.Hidden;
            txtCaptcha.Visibility = Visibility.Hidden;
        }

        private void btnEnterGuest_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Client(null));
        }

        private void btnEnter_Click(object sender, RoutedEventArgs e)
        {
            //countUnsuccesful++;

            string password = txtPassword.Text.Trim();
            string login = txtLogin.Text.Trim();
            User user = new User();

            user = mssql_script_tradeEntities.GetContext().User.Where(p => p.UserLogin == login && p.UserPassword == password).FirstOrDefault();
            //MessageBox.Show(user.UserLogin, user.UserPassword);
            int userCount = mssql_script_tradeEntities.GetContext().User.Where(p => p.UserLogin == login && p.UserPassword == password).Count();
            if (countUnsuccesful < 1)
            {
                if (userCount > 0)
                {
                    MessageBox.Show("Вы вошли под:" + user.Role.RoleName.ToString());
                    LoadForm(user.Role.RoleName.ToString(), user);

                }
                else
                {
                    MessageBox.Show("Вы ввели неверно логин или пароль!");
                    countUnsuccesful++;
                    if (countUnsuccesful == 1)
                        GenerateCaptha();

                }

            }
            else 
            {
                //GenerateCaptha();
                //if (user != null && txtCaptcha.Text.Trim()  == TextBlockCatcha.Text.Trim())
                //{
                //    MessageBox.Show("Вы вошли под:" + user.Role.RoleName.ToString());
                //    LoadForm(user.Role.RoleName.ToString(), user);
                //}
                //else
                //{
                //    MessageBox.Show("Введите данные заново!");

                //}

                if (userCount>0&&TextBlockCatcha.Text==txtCaptcha.Text)
                {
                    MessageBox.Show("Вы вошли под:" + user.Role.RoleName.ToString());

                    LoadForm(user.Role.RoleName.ToString(), user);

                }
                else
                {
                    MessageBox.Show("Введите данные заново!");

                }
                //if (true)
                //{

                ////}
                //if (userCount > 0)
                //{



                //    MessageBox.Show("Вы вошли под:" + user.Role.RoleName.ToString());
                //    LoadForm(user.Role.RoleName.ToString());

                //}
                //else
                //{
                //    MessageBox.Show("Введите данные заново!");
                //}


            }
        }

        private void GenerateCaptha()
        {

            txtLogin.Clear();
            txtPassword.Clear();
            txtCaptcha.Clear();
            txtCaptcha.Visibility = Visibility.Visible;
            TextBlockCatcha.Visibility = Visibility.Visible;
            Random random = new Random();

            int randNum = random.Next(0, 3);

            switch (randNum)
            {
                //default:
                //    break;


                case 1:

                    TextBlockCatcha.Text = "ju2sT8Cbs";
                    TextBlockCatcha.TextDecorations = TextDecorations.Strikethrough;
                    break;

                case 2:
                    TextBlockCatcha.Text = "iNwk2cl";
                    TextBlockCatcha.TextDecorations = TextDecorations.Strikethrough;
                    break;

                case 3:
                    TextBlockCatcha.Text = "uOozGk95";
                    TextBlockCatcha.TextDecorations = TextDecorations.Strikethrough;
                    break;
            }

        }

        private void LoadForm(string _role, User user)
        {
            switch (_role)
            {
                //default:
                //     break;
                case "Клиент":
                    NavigationService.Navigate(new Client(user));
                    break;

            }
        }
    }
}
