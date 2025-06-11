using Rul.Entities;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Windows.Media.Effects;
using System.Text;
using System.Linq;
using WpfApp1.Services;

namespace Rul.Pages
{
    public partial class Autho : Page
    {
        private int failedAttempts = 0;
        private DispatcherTimer blockTimer;
        private string currentCaptcha;
        private const int BLOCK_TIME_SECONDS = 10;

        public Autho()
        {
            InitializeComponent();
            blockTimer = new DispatcherTimer();
            blockTimer.Interval = TimeSpan.FromSeconds(1);
            blockTimer.Tick += BlockTimer_Tick;
        }

        private void BlockTimer_Tick(object sender, EventArgs e)
        {
            if (blockTimer.Tag is int timeLeft)
            {
                timeLeft--;
                blockTimer.Tag = timeLeft;
                tbTimeLeft.Text = $"Подождите {timeLeft} секунд перед следующей попыткой";

                if (timeLeft <= 0)
                {
                    blockTimer.Stop();
                    tbTimeLeft.Visibility = Visibility.Collapsed;
                    EnableInputs(true);
                }
            }
        }

        private void EnableInputs(bool enable)
        {
            txtLogin.IsEnabled = enable;
            txtPassword.IsEnabled = enable;
            btnEnter.IsEnabled = enable;
            btnEnterGuest.IsEnabled = enable;
            txtCaptcha.IsEnabled = enable;
        }

        private void btnEnterGuest_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Client(null));
        }

        private void btnEnter_Click(object sender, RoutedEventArgs e)
        {
            string login = txtLogin.Text.Trim();
            string password = txtPassword.Password.Trim();

            if (failedAttempts >= 1 && captchaPanel.Visibility == Visibility.Visible)
            {
                if (txtCaptcha.Text.Trim() != currentCaptcha)
                {
                    MessageBox.Show("Неверная CAPTCHA!");
                    BlockUser();
                    return;
                }
            }

            var user = mssql_script_tradeEntities.GetContext().User
                .FirstOrDefault(p => p.UserLogin == login && p.UserPassword == password);

            if (user != null)
            {
                failedAttempts = 0;
                captchaPanel.Visibility = Visibility.Collapsed;
                MessageBox.Show($"Вы вошли как: {user.Role.RoleName}");
                LoadForm(user.Role.RoleName, user);
            }
            else
            {
                failedAttempts++;
                MessageBox.Show("Неверный логин или пароль!");

                if (failedAttempts >= 1)
                {
                    GenerateCaptcha();
                    captchaPanel.Visibility = Visibility.Visible;
                }
            }
        }

        private void GenerateCaptcha()
        {
            // Generate random 4-character CAPTCHA
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            currentCaptcha = CapthchaGenerator.GenerateCaptchaText(4);
            capthcaImage.Text=currentCaptcha;
          
        }

        private void BlockUser()
        {
            EnableInputs(false);
            tbTimeLeft.Visibility = Visibility.Visible;
            blockTimer.Tag = BLOCK_TIME_SECONDS;
            tbTimeLeft.Text = $"Подождите {BLOCK_TIME_SECONDS} секунд перед следующей попыткой";
            blockTimer.Start();
            GenerateCaptcha(); // Generate new CAPTCHA after block
        }

        private void LoadForm(string role, User user)
        {
            switch (role)
            {
                case "Клиент":
                    NavigationService.Navigate(new Client(user));
                    break;
                case "Менеджер":
                    NavigationService.Navigate(new Client(user));
                    break;
                case "Адинистратор":
                    NavigationService.Navigate(new Admin(user));
                    break;
              
            }
        }
    }
}