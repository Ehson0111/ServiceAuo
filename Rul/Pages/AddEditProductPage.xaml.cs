//using Microsoft.Win32;
//using Rul.Entities;
//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Data;
//using System.Windows.Documents;
//using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;
//using System.Windows.Navigation;
////using System.Windows.Shapes;

//namespace Rul.Pages
//{
//    /// <summary>
//    /// Логика взаимодействия для AddEditProductPage.xaml
//    /// </summary>
//    public partial class AddEditProductPage : Page
//    {
//        //{ get; set; }

//        Product product = new Product();
//        public AddEditProductPage(Product currentProduct)
//        {
//            InitializeComponent();

//            if (currentProduct != null)
//            {

//                product = currentProduct;
//                btnDeleteProduct.Visibility = Visibility.Visible;
//                txtArticle.IsEnabled = false;
//            }
//            DataContext = product;
//            cmbCategory.ItemsSource = CategoryList;

//        }

//        public string[] CategoryList = {
//            "Аксессуары",
//            "Автозапчасти",
//            "Автосервис",
//            "Съемники подшипников",
//            "Ручные инструменты",
//            "Зарядные устройства",

//        };


//        private void btnSaveProduct_Click(object sender, RoutedEventArgs e)
//        {
//            StringBuilder errors = new StringBuilder();

//            //if (product.ProductCost < 0)
//            //    errors.AppendLine("Стоимость не может быть отрицательной!");
//            //if (product.MinCount < 0)
//            //    errors.AppendLine("Минимальное количество не может быть отрицательной!");
//            //if (product.ProductDiscountAmount > product.MaxDiscountAmount)
//            //    errors.AppendLine("Действующая скидка на товар не может быть более максимальной скидки!");

//            //if (errors.Length > 0)
//            //{
//            //    MessageBox.Show(errors.ToString());
//            //    return;
//            //}
//            //if (product.ProductArticleNumber != null)
//            //{
//            //    mssql_script_tradeEntities.GetContext().Product.Add(product);
//            //}
//            ////try
//            ////{

//            //mssql_script_tradeEntities.GetContext().SaveChanges();
//            //MessageBox.Show("Информация сохранена!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);

//            //NavigationService.GoBack();

//            if (product.ProductCost < 0)
//                errors.AppendLine("Стоимость не может быть отрицательной!");
//            if (product.MinCount < 0)
//                errors.AppendLine("Минимальное количество не может быть отрицательной!");
//            if (product.ProductDiscountAmount > product.MaxDiscountAmount)
//                errors.AppendLine("Действующая скидка на товар не может быть более максимальной скидки!");

//            if (errors.Length > 0)
//            {
//                MessageBox.Show(errors.ToString());
//                return;
//            }

//            try
//            {
//                if (product.ProductArticleNumber != null)
//                {
//                    mssql_script_tradeEntities.GetContext().Product.Add(product);
//                }
//                mssql_script_tradeEntities.GetContext().SaveChanges();
//                MessageBox.Show("Информация сохранена!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
//                NavigationService.GoBack();
//            }
//            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
//            {
//                StringBuilder validationErrors = new StringBuilder();
//                foreach (var validationError in ex.EntityValidationErrors)
//                {
//                    foreach (var error in validationError.ValidationErrors)
//                    {
//                        validationErrors.AppendLine($"Поле: {error.PropertyName}, Ошибка: {error.ErrorMessage}");
//                    }
//                }
//                MessageBox.Show($"Ошибка валидации:\n{validationErrors.ToString()}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
//            }

//            //}
//            //catch (Exception ex)
//            //{
//            //    MessageBox.Show(ex.Message.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
//            //    //throw;
//            //}
//        }

//        private void btnEnterImage_Click(object sender, RoutedEventArgs e)
//        {
//            OpenFileDialog openFileDialog = new OpenFileDialog();
//            openFileDialog.Filter = "Изображения (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg";

//            if (openFileDialog.ShowDialog() == true)
//            {
//                try
//                {
//                    // 1. Определяем папку для хранения изображений
//                    string imagesFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources");


//                    string fileName = Path.GetFileName(openFileDialog.FileName);
//                    string destinationPath = Path.Combine(imagesFolder, fileName);

//                    // 4. Копируем файл (перезаписываем, если уже существует)
//                    File.Copy(openFileDialog.FileName, destinationPath, true);

//                    // 5. Сохраняем только имя файла в базу
//                    product.ProductImage = fileName;

//                    MessageBox.Show("Изображение успешно сохранено", "Успех",
//                                  MessageBoxButton.OK, MessageBoxImage.Information);
//                }
//                catch (Exception ex)
//                {
//                    MessageBox.Show($"Ошибка при сохранении изображения: {ex.Message}",
//                                  "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
//                }
//            }
//        }


//        private void btnDeleteProduct_Click(object sender, RoutedEventArgs e)
//        {
//            if (MessageBox.Show($"Вы действительно хотите удалить {product.ProductName}?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
//            {
//                try
//                { 

//                    mssql_script_tradeEntities.GetContext().Product.Remove(product);
//                    mssql_script_tradeEntities.GetContext().SaveChanges();
//                    //mssql_script_tradeEntities.GetContext().SaveChanges();
//                    MessageBox.Show("Запись удалена!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
//                    NavigationService.GoBack();
//                }
//                catch (Exception ex )
//                {
//                    MessageBox.Show(ex.Message.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

//                }

//            }
//        }

//        private void txtDiscount_TextChanged(object sender, TextChangedEventArgs e)
//        {

//        }
//    }
//}


using Microsoft.Win32;
using Rul.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace Rul.Pages
{
    public partial class AddEditProductPage : Page
    {
        private Product _currentProduct = new Product();
        private string _originalArticleNumber;

        public AddEditProductPage(Product currentProduct)
        {
            InitializeComponent();

            if (currentProduct != null)
            {
                _currentProduct = currentProduct;
                _originalArticleNumber = currentProduct.ProductArticleNumber;
                btnDeleteProduct.Visibility = Visibility.Visible;
                txtArticle.IsEnabled = false;
            }

            DataContext = _currentProduct;
            cmbCategory.ItemsSource = CategoryList;
        }

        public string[] CategoryList = {
            "Аксессуары",
            "Автозапчасти",
            "Автосервис",
            "Съемники подшипников",
            "Ручные инструменты",
            "Зарядные устройства",
        };

        private void btnSaveProduct_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            // Валидация данных
            if (string.IsNullOrWhiteSpace(_currentProduct.ProductArticleNumber))
                errors.AppendLine("Укажите артикул товара");
            if (string.IsNullOrWhiteSpace(_currentProduct.ProductName))
                errors.AppendLine("Укажите наименование товара");
            if (_currentProduct.ProductCost < 0)
                errors.AppendLine("Стоимость не может быть отрицательной!");
            if (_currentProduct.MinCount < 0)
                errors.AppendLine("Минимальное количество не может быть отрицательной!");
            if (_currentProduct.ProductDiscountAmount > _currentProduct.MaxDiscountAmount)
                errors.AppendLine("Действующая скидка на товар не может быть более максимальной скидки!");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                var context = mssql_script_tradeEntities.GetContext();

                // Если это новый товар
                if (_originalArticleNumber == null)
                {
                    // Проверяем, нет ли уже товара с таким артикулом
                    if (context.Product.Any(p => p.ProductArticleNumber == _currentProduct.ProductArticleNumber))
                    {
                        MessageBox.Show("Товар с таким артикулом уже существует!", "Ошибка",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    context.Product.Add(_currentProduct);
                }
                else
                {
                    // Для существующего товара
                    var productInDb = context.Product.FirstOrDefault(p => p.ProductArticleNumber == _originalArticleNumber);
                    if (productInDb != null)
                    {
                        // Обновляем все свойства, кроме артикула
                        context.Entry(productInDb).CurrentValues.SetValues(_currentProduct);
                        productInDb.ProductArticleNumber = _originalArticleNumber; // Гарантируем, что артикул не изменится
                    }
                }

                context.SaveChanges();
                MessageBox.Show("Информация сохранена!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                NavigationService.GoBack();
            }
            catch (DbEntityValidationException ex)
            {
                StringBuilder validationErrors = new StringBuilder();
                foreach (var validationError in ex.EntityValidationErrors)
                {
                    foreach (var error in validationError.ValidationErrors)
                    {
                        validationErrors.AppendLine($"Поле: {error.PropertyName}, Ошибка: {error.ErrorMessage}");
                    }
                }
                MessageBox.Show($"Ошибка валидации:\n{validationErrors.ToString()}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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
                    string imagesFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources");

                    // Создаем папку, если ее нет
                    if (!Directory.Exists(imagesFolder))
                        Directory.CreateDirectory(imagesFolder);

                    string fileName = Path.GetFileName(openFileDialog.FileName);
                    string destinationPath = Path.Combine(imagesFolder, fileName);

                    // Копируем файл (перезаписываем, если уже существует)
                    File.Copy(openFileDialog.FileName, destinationPath, true);

                    // Сохраняем только имя файла в базу
                    _currentProduct.ProductImage = fileName;

                    // Обновляем изображение в интерфейсе
                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(destinationPath);
                    bitmap.EndInit();
                    img.Source = bitmap;

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

        private void btnDeleteProduct_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show($"Вы действительно хотите удалить {_currentProduct.ProductName}?",
                "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                try
                {
                    var context = mssql_script_tradeEntities.GetContext();
                    var productToDelete = context.Product.FirstOrDefault(p => p.ProductArticleNumber == _currentProduct.ProductArticleNumber);

                    if (productToDelete != null)
                    {
                        context.Product.Remove(productToDelete);
                        context.SaveChanges();
                        MessageBox.Show("Товар удален!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        NavigationService.GoBack();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении товара: {ex.Message}",
                        "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void txtDiscount_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Логика обработки изменения скидки
        }
    }
}
