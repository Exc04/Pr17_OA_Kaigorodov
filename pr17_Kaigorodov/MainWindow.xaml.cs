using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
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

namespace pr17_Kaigorodov
{
    public partial class MainWindow : Window
    {
        private int currentTask = 0;

        public MainWindow()
        {
            InitializeComponent();
            AdditionalParamTextBox.Visibility = Visibility.Collapsed;
            AdditionalParamLabel.Visibility = Visibility.Collapsed;
        }

        private void Task1_Click(object sender, RoutedEventArgs e)
        {
            currentTask = 1;
            ActionButton.Content = "Найти слово";
            AdditionalParamTextBox.Clear();
            InputTextBox.Text = File.ReadAllText("task1.txt");
            ResultTextBox.Clear();

            AdditionalParamLabel.Text = "Искомое слово:";
            AdditionalParamTextBox.Visibility = Visibility.Visible;
            AdditionalParamLabel.Visibility = Visibility.Visible;
        }

        private void Task2_Click(object sender, RoutedEventArgs e)
        {
            currentTask = 2;
            ActionButton.Content = "Обработать массив";
            InputTextBox.Text = "a,b,c,1,2,3,/,d,e,f,4,5,6,/,g,h,i";
            ResultTextBox.Clear();

            AdditionalParamTextBox.Visibility = Visibility.Collapsed;
            AdditionalParamLabel.Visibility = Visibility.Collapsed;
        }

        private void Task3_Click(object sender, RoutedEventArgs e)
        {
            currentTask = 3;
            ActionButton.Content = "Вычислить частоту";
            InputTextBox.Text = "5,1;1,3;9,2;2;3;5,1;3";
            ResultTextBox.Clear();

            AdditionalParamTextBox.Visibility = Visibility.Collapsed;
            AdditionalParamLabel.Visibility = Visibility.Collapsed;
        }

        private void Task4_Click(object sender, RoutedEventArgs e)
        {
            currentTask = 4;
            ActionButton.Content = "Фильтровать страны";
            InputTextBox.Text = "Россия 146023195\nСингапур 6058739\nСША 335028178\n" +
                               "Франция 65790152\nИндия 1408044253\nБразилия 215681045\n" +
                               "Египет 105838455";
            ResultTextBox.Clear();
            AdditionalParamTextBox.Clear();
            AdditionalParamLabel.Text = "Минимальная численность населения:";
            AdditionalParamTextBox.Visibility = Visibility.Visible;
            AdditionalParamLabel.Visibility = Visibility.Visible;
        }

        private void ExecuteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                switch (currentTask)
                {
                    case 1:
                        ExecuteTask1();
                        break;
                    case 2:
                        ExecuteTask2();
                        break;
                    case 3:
                        ExecuteTask3();
                        break;
                    case 4:
                        ExecuteTask4();
                        break;
                    default:
                        MessageBox.Show("Пожалуйста, выберите задание из меню.", "Предупреждение",
                                      MessageBoxButton.OK, MessageBoxImage.Warning);
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //ЗАДАНИЕ-1
        private void ExecuteTask1()
        {
            string text = InputTextBox.Text;
            if (string.IsNullOrWhiteSpace(text))
            {
                MessageBox.Show("Пожалуйста, введите текст.", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string searchWord = AdditionalParamTextBox.Text;
            if (string.IsNullOrWhiteSpace(searchWord))
            {
                MessageBox.Show("Пожалуйста, введите слово для поиска.", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string[] words = Regex.Split(text.ToLower(), @"\W+");
            string searchWordLower = searchWord.ToLower();

            // LINQ: подсчет количества вхождений
            int count = words.Count(w => w == searchWordLower);

            ResultTextBox.Text = $"Поисковый запрос: \"{searchWord}\"\n" +
                                $"Были найдены {count} вхождений(е) поискового запроса \"{searchWord}\"";
        }

        //ЗАДАНИЕ-2
        private void ExecuteTask2()
        {
            string input = InputTextBox.Text;
            if (string.IsNullOrWhiteSpace(input))
            {
                MessageBox.Show("Пожалуйста, введите массив строк.", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Разбиваем строку на массив
            string[] array = input.Split(',').Select(s => s.Trim()).ToArray();

            ResultTextBox.Text = $"Исходный массив: [{string.Join(", ", array)}]\n\n";

            // А) Определение цифр с помощью LINQ
            var digits = array.Where(s => s.Length == 1 && char.IsDigit(s[0])).ToList();
            ResultTextBox.Text += $"А) Количество цифр: {digits.Count}\n";
            ResultTextBox.Text += $"   Найденные цифры: {string.Join(", ", digits)}\n\n";
            // Б) Вывод элементов до первого символа '/' с помощью LINQ
            int slashIndex = Array.FindIndex(array, s => s == "/");
            if (slashIndex == -1)
            {
                ResultTextBox.Text += "Б) Символ '/' не найден, выведены все элементы:\n";
                ResultTextBox.Text += $"   {string.Join(", ", array)}\n\n";
            }
            else
            {
                var beforeSlash = array.Take(slashIndex);
                ResultTextBox.Text += $"Б) Элементы до символа '/': {string.Join(", ", beforeSlash)}\n\n";
            }
            // В) Элементы после '/' с заменой регистра
            if (slashIndex != -1 && slashIndex < array.Length - 1)
            {
                var afterSlash = array.Skip(slashIndex + 1);

                // Меняем регистр букв
                var transformed = afterSlash.Select(s => new string(s.Select(c =>
                    char.IsLetter(c) ? (char.IsUpper(c) ? char.ToLower(c) : char.ToUpper(c)) : c).ToArray()));

                ResultTextBox.Text += $"В) Элементы после символа '/' (с измененным регистром):\n";
                ResultTextBox.Text += $"   {string.Join(", ", transformed)}\n\n";

                // Запись в файл
                string filePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "result_task2.txt");
                System.IO.File.WriteAllLines(filePath, transformed);
                ResultTextBox.Text += $"   Результат сохранен в файл: {filePath}\n";
            }
            else
            {
                ResultTextBox.Text += "В) Нет элементов после символа '/'.\n";
            }
        }

        //ЗАДАНИЕ-3
        private void ExecuteTask3()
        {
            string input = InputTextBox.Text;
            if (string.IsNullOrWhiteSpace(input))
            {
                MessageBox.Show("Пожалуйста, введите массив чисел.", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            // Парсим числа 
            string[] parts = input.Split(new char[] { ';', ',' }, StringSplitOptions.RemoveEmptyEntries);
            List<double> numbers = new List<double>();

            foreach (string part in parts)
            {
                string cleaned = part.Trim().Replace(',', '.');
                if (double.TryParse(cleaned, System.Globalization.NumberStyles.Any,
                    System.Globalization.CultureInfo.InvariantCulture, out double num))
                {
                    numbers.Add(num);
                }
            }
            if (numbers.Count == 0)
            {
                MessageBox.Show("Не удалось распознать числа. Используйте точку с запятой как разделитель.\n" +
                              "Пример: 5,1;1,3;9,2;2;3;5,1;3",
                              "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            ResultTextBox.Text = "Исходный массив: " + string.Join(", ", numbers) + "\n\n";
            ResultTextBox.Text += "а) Числа и их частота:\n";
            ResultTextBox.Text += "----------------------------------------\n";

            // LINQ: группировка и подсчет частоты
            var frequency = numbers
                .GroupBy(n => n)
                .Select(g => new { Number = g.Key, Count = g.Count() })
                .OrderBy(x => x.Number);

            foreach (var item in frequency)
            {
                ResultTextBox.Text += $"{item.Number} - {item.Count}\n";
            }
            ResultTextBox.Text += "\nб) Новый массив (число * частота):\n";
            ResultTextBox.Text += "----------------------------------------\n";

            // LINQ: создание нового массива a[i] = a[i] * частота
            var newArray = numbers.Select(n => n * frequency.First(f => f.Number == n).Count).ToList();

            for (int i = 0; i < numbers.Count; i++)
            {
                ResultTextBox.Text += $"{numbers[i]} * {frequency.First(f => f.Number == numbers[i]).Count} = {newArray[i]}\n";
            }
        }

        //ЗАДАНИЕ-4
        private void ExecuteTask4()
        {
            string input = InputTextBox.Text;
            if (string.IsNullOrWhiteSpace(input))
            {
                MessageBox.Show("Пожалуйста, введите список стран.", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Парсим страны
            var countries = new List<Country>();
            string[] lines = input.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string line in lines)
            {
                string[] parts = line.Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length >= 2)
                {
                    string name = parts[0];
                    string populationStr = string.Join("", parts.Skip(1));
                    if (long.TryParse(populationStr, out long population))
                    {
                        countries.Add(new Country { Name = name, Population = population });
                    }
                }
            }

            if (countries.Count == 0)
            {
                MessageBox.Show("Не удалось распознать страны. Формат: 'Название Численность'\n" +
                              "Пример: Россия 146023195",
                              "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!long.TryParse(AdditionalParamTextBox.Text, out long minPopulation))
            {
                MessageBox.Show("Пожалуйста, введите корректную минимальную численность населения.",
                              "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // LINQ: фильтрация и сортировка
            var filtered = countries
                .Where(c => c.Population > minPopulation)
                .OrderBy(c => c.Name.Length)     
                .ThenBy(c => c.Name)               
                .ToList();

            ResultTextBox.Text = $"Исходные страны:\n";
            foreach (var c in countries)
            {
                ResultTextBox.Text += $"{c.Name} {c.Population:N0}\n";
            }

            ResultTextBox.Text += $"\nМинимальная численность: {minPopulation:N0}\n";
            ResultTextBox.Text += "\nУпорядоченный список стран, у которых численность больше n:\n";
            ResultTextBox.Text += "----------------------------------------\n";

            if (filtered.Count == 0)
            {
                ResultTextBox.Text += "Нет стран с численностью больше указанной.";
            }
            else
            {
                foreach (var c in filtered)
                {
                    ResultTextBox.Text += $"{c.Name} {c.Population:N0}\n";
                }
            }
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            InputTextBox.Clear();
            ResultTextBox.Clear();
            AdditionalParamTextBox.Clear();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
