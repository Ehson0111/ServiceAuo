using Microsoft.Win32;
using Rul.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
//using System.Windows.Shapes;

namespace Rul.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddEditProductPage.xaml
    /// </summary>
    public partial class AddEditProductPage : Page
    {
        //{ get; set; }

        Product product = new Product();
        public AddEditProductPage(Product currentProduct)
        {
            InitializeComponent();

            if (currentProduct != null)
            {

                product = currentProduct;
                btnDeleteProduct.Visibility = Visibility.Visible;
                txtArticle.IsEnabled = false;
            }
            DataContext = product;
            cmbCategory.ItemsSource = CategoryList;

        }

        public string[] CategoryList = {
            "Аксессуары",
            "Автозапчасти",
            "Автосервис",
            "Съемники подшипникиов",
            "Ручные инструменты",
            "Зарядные устройства",

        };

        //private void btnEnterImage_Click(object sender, RoutedEventArgs e)
        //{
        //    OpenFileDialog GetImageDialog = new OpenFileDialog();

        //    GetImageDialog.Filter = "Файлы изображений (*.png,*.jpg, *.jpeg)| *.png; *.jpg; *.jpeg";
        //    GetImageDialog.InitialDirectory = "C:\\Users\\ekhso\\source\\repos\\Rul\\Rul\\Resources";

        //    //}
        //    if (GetImageDialog.ShowDialog() == true)
        //    {
        //        try
        //        {
        //            // Путь к папке Resources в проекте
        //            string resourcesPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources");
        //            if (!Directory.Exists(resourcesPath))
        //            {
        //                Directory.CreateDirectory(resourcesPath);
        //            }

        //            // Имя файла
        //            string fileName = GetImageDialog.SafeFileName;
        //            // Путь назначения в папке Resources
        //            string destPath = System.IO.Path.Combine(resourcesPath, fileName);

        //            // Копируем файл в папку Resources
        //            File.Copy(GetImageDialog.FileName, destPath, true);

        //            // Сохраняем только имя файла в ProductImage
        //            product.ProductImage = fileName;

        //            MessageBox.Show($"Изображение {fileName} успешно добавлено!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show($"Ошибка при копировании изображения: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        //        }
        //    }
        //}

        //private void btnEnterImage_Click(object sender, RoutedEventArgs e)
        //{
        //    OpenFileDialog openFileDialog = new OpenFileDialog();
        //    openFileDialog.Filter = "Изображения (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg";

        //    if (openFileDialog.ShowDialog() == true)
        //    {
        //        try
        //        {
        //            // 1. Определяем папку для хранения изображений
        //            string imagesFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ProductImages");



        //            // 3. Генерируем уникальное имя файла
        //            string fileExtension = Path.GetExtension(openFileDialog.FileName);
        //            string uniqueFileName = Guid.NewGuid().ToString() + fileExtension;
        //            string destinationPath = Path.Combine(imagesFolder, uniqueFileName);

        //            // 4. Копируем файл
        //            File.Copy(openFileDialog.FileName, destinationPath);

        //            // 5. Сохраняем только имя файла в базу
        //            product.ProductImage = uniqueFileName;

        //            MessageBox.Show("Изображение успешно сохранено", "Успех",
        //                          MessageBoxButton.OK, MessageBoxImage.Information);
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show($"Ошибка при сохранении изображения: {ex.Message}",
        //                          "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        //        }
        //    }
        //}

        //private void btnEnterImage_Click(object sender, RoutedEventArgs e)
        //{
        //    OpenFileDialog openFileDialog = new OpenFileDialog();
        //    openFileDialog.Filter = "Изображения (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg";

        //    if (openFileDialog.ShowDialog() == true)
        //    {
        //        try
        //        {
        //            // 1. Определяем папку для хранения изображений
        //            string imagesFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ProductImages");


        //            // 3. Генерируем уникальное имя файла
        //            string fileExtension = Path.GetExtension(openFileDialog.FileName);
        //            string uniqueFileName = Guid.NewGuid().ToString() + fileExtension;
        //            string destinationPath = Path.Combine(imagesFolder, uniqueFileName);

        //            // 4. Копируем файл
        //            File.Copy(openFileDialog.FileName, destinationPath);

        //            // 5. Сохраняем только имя файла в базу
        //            product.ProductImage = uniqueFileName;

        //            MessageBox.Show("Изображение успешно сохранено", "Успех",
        //                          MessageBoxButton.OK, MessageBoxImage.Information);
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show($"Ошибка при сохранении изображения: {ex.Message}",
        //                          "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        //        }
        //    }
        //}
        private void btnSaveProduct_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (product.ProductCost < 0)
                errors.AppendLine("Стоимость не может быть отрицательной!");
            if (product.MinCount < 0)
                errors.AppendLine("Минимальное количество не может быть отрицательной!");
            if (product.ProductDiscountAmount > product.MaxDiscountAmount)
                errors.AppendLine("Действующая скидка на товар не может быть более максимальной скидки!");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;


            }
            if (product.ProductArticleNumber == null)
            {
                mssql_script_tradeEntities.GetContext().Product.Add(product);
            }
            try
            {

                mssql_script_tradeEntities.GetContext().SaveChanges();
                MessageBox.Show("Информация сохранена!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);

                NavigationService.GoBack();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                //throw;
            }
        }

        private void btnEnterImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Изображения (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg";

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    // 1. Определяем папку для хранения изображений
                    string imagesFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources");

                    // 2. Создаем папку, если ее нет
              
                    string fileName = Path.GetFileName(openFileDialog.FileName);
                    string destinationPath = Path.Combine(imagesFolder, fileName);

                    // 4. Копируем файл (перезаписываем, если уже существует)
                    File.Copy(openFileDialog.FileName, destinationPath, true);

                    // 5. Сохраняем только имя файла в базу
                    product.ProductImage = fileName;

                    MessageBox.Show("Изображение успешно сохранено", "Успех",
                                  MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при сохранении изображения: {ex.Message}",
                                  "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private mssql_script_tradeEntities db;

        private void btnDeleteProduct_Click(object sender, RoutedEventArgs e)
        {
            db=new mssql_script_tradeEntities();
            if (MessageBox.Show($"Вы действительно хотите удалить {product.ProductName}?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                try
                {
                    db.Product.Remove(product);
                    db.SaveChanges();
                    //mssql_script_tradeEntities.GetContext().SaveChanges();
                    MessageBox.Show("Запись удалена!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                    NavigationService.GoBack();
                }
                catch (Exception ex )
                {
                    MessageBox.Show(ex.Message.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

                }

            }

        }

        private void txtDiscount_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
