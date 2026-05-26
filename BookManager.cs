using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ConsoleApp7
{
    // ============================================
    // КЛАСС ДЛЯ УПРАВЛЕНИЯ КНИГАМИ
    // ============================================
    public static class BookManager
    {
        // Приватное статическое поле для хранения всех книг
        private static readonly List<Book> _books = new();

        // ============================================
        // ЗАГРУЗКА КНИГ ИЗ ФАЙЛА
        // ============================================
        public static void Load(string path)
        {
            _books.Clear();  // Очищаем текущий список
            _books.AddRange(DataService.LoadBooks(path));  // Добавляем загруженные книги
        }

        // ============================================
        // СОХРАНЕНИЕ КНИГ В ФАЙЛ
        // ============================================
        public static void Save(string path) => DataService.SaveBooks(path, _books);
        
        // ============================================
        // ПОЛУЧЕНИЕ ВСЕХ КНИГ (копия списка)
        // ============================================
        public static List<Book> GetAll() => new(_books);

        // ============================================
        // ДОБАВЛЕНИЕ НОВОЙ КНИГИ
        // ============================================
        public static bool Add(Book book)
        {
            // Проверяем, нет ли книги с таким ID
            if (_books.Any(b => b.Id == book.Id)) return false;
            
            _books.Add(book);  // Добавляем книгу
            return true;
        }

        // ============================================
        // УДАЛЕНИЕ КНИГИ ПО ID
        // ============================================
        public static bool RemoveById(int id) => _books.RemoveAll(b => b.Id == id) > 0;

        // ============================================
        // ПОИСК КНИГ (по названию, автору, году)
        // ============================================
        public static List<Book> Search(string title, string author, int? minYear) =>
            _books.Where(b =>
                // Если title пустой ИЛИ название книги содержит искомое
                (string.IsNullOrEmpty(title) || b.Title.IndexOf(title, StringComparison.OrdinalIgnoreCase) >= 0) &&
                // Если author пустой ИЛИ автор содержит искомое
                (string.IsNullOrEmpty(author) || b.Author.IndexOf(author, StringComparison.OrdinalIgnoreCase) >= 0) &&
                // Если minYear не задан ИЛИ год книги >= minYear
                (!minYear.HasValue || b.Year >= minYear.Value)
            ).ToList();

        // ============================================
        // СОРТИРОВКА КНИГ
        // ============================================
        public static List<Book> SortBy(string criterion)
        {
            var sorted = new List<Book>(_books);  // Создаем копию списка
            
            switch (criterion)
            {
                case "1":  // Сортировка по ID
                    sorted.Sort((a, b) => a.Id.CompareTo(b.Id)); 
                    break;
                case "2":  // Сортировка по названию
                    sorted.Sort((a, b) => string.Compare(a.Title, b.Title, StringComparison.OrdinalIgnoreCase)); 
                    break;
                case "3":  // Сортировка по году
                    sorted.Sort((a, b) => a.Year.CompareTo(b.Year)); 
                    break;
                case "4":  // Сортировка по автору
                    sorted.Sort((a, b) => string.Compare(a.Author, b.Author, StringComparison.OrdinalIgnoreCase)); 
                    break;
            }
            return sorted;
        }

        // ============================================
        // ПОЛУЧЕНИЕ СТАТИСТИКИ
        // ============================================
        public static (int Total, double AvgYear, int Recent) GetStats()
        {
            if (_books.Count == 0) return (0, 0, 0);
            
            int sum = 0, recent = 0;
            
            foreach (var b in _books)
            {
                sum += b.Year;  // Суммируем годы
                if (b.Year >= 2000) recent++;  // Считаем книги с 2000 года
            }
            
            // Возвращаем кортеж: (всего книг, средний год, книг с 2000 года)
            return (_books.Count, (double)sum / _books.Count, recent);
        }

        // ============================================
        // ОТОБРАЖЕНИЕ СПИСКА КНИГ
        // ============================================
        public static void ShowBooks(List<Book> list)
        {
            // Если список пуст
            if (list == null || list.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Program.PrintCentered("Список книг пуст.");
                Console.ResetColor();
                return;
            }

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            
            // Выводим заголовок таблицы
            Console.WriteLine($"  {"ID",-5} {"Название",-25} {"Автор",-20} {"Год",-6} Жанр");
            Console.WriteLine(new string('-', 70));  // Разделитель
            Console.ResetColor();

            // Выводим каждую книгу
            foreach (var b in list)
            {
                Console.WriteLine($"  {b.Id,-5} {FormatCell(b.Title, 25)} {FormatCell(b.Author, 20)} {b.Year,-6} {FormatCell(b.Genre, 15)}");
            }

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine($"\n  Всего записей: {list.Count}");
            Console.ResetColor();
        }

        // ============================================
        // ВВОД НОВОЙ КНИГИ С КОНСОЛИ
        // ============================================
        public static Book? InputNewBook()
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Program.PrintCentered("--- Добавление книги ---");
            Console.ResetColor();

            // Запрашиваем данные у пользователя
            int id = ReadInt("ID:");
            string title = ReadStringWithLetters("Название:");
            string author = ReadStringWithLetters("Автор:");
            int year = ReadInt("Год издания:");
            string genre = ReadStringWithLetters("Жанр:");

            return new Book(id, title, author, year, genre);
        }

        // ============================================
        // ВВОД КРИТЕРИЕВ ПОИСКА
        // ============================================
        public static (string Title, string Author, int? MinYear) InputSearchCriteria()
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Program.PrintCentered("--- Поиск (оставьте поле пустым для пропуска) ---");
            Console.ResetColor();

            string title = ReadString("Название содержит:");
            string author = ReadString("Автор содержит:");

            Console.ForegroundColor = ConsoleColor.Cyan;
            Program.PrintCentered("Год издания >= :");
            Console.ResetColor();
            
            string y = Console.ReadLine()?.Trim();
            int? minYear = null;
            
            // Пытаемся преобразовать в число
            if (!string.IsNullOrEmpty(y) && int.TryParse(y, out int yr)) 
                minYear = yr;

            return (title, author, minYear);
        }

        // ============================================
        // ФОРМАТИРОВАНИЕ ЯЧЕЙКИ ТАБЛИЦЫ
        // ============================================
        private static string FormatCell(string value, int maxLength)
        {
            value = string.IsNullOrEmpty(value) ? "N/A" : value.Trim();
            
            // Если строка слишком длинная, обрезаем и добавляем "..."
            return value.Length > maxLength ? value.Substring(0, maxLength - 3) + "..." : value.PadRight(maxLength);
        }

        // ============================================
        // ЧТЕНИЕ ЧИСЛА С КОНСОЛИ
        // ============================================
        private static int ReadInt(string prompt)
        {
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Program.PrintCentered(prompt);
                Console.ResetColor();
                
                // Пытаемся прочитать число
                if (int.TryParse(Console.ReadLine(), out int val)) 
                    return val;
                
                // Если не получилось - выводим ошибку
                Console.ForegroundColor = ConsoleColor.Red;
                Program.PrintCentered("Ошибка: введите корректное число.");
                Console.ResetColor();
            }
        }

        // ============================================
        // ЧТЕНИЕ СТРОКИ С КОНСОЛИ
        // ============================================
        private static string ReadString(string prompt)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Program.PrintCentered(prompt);
            Console.ResetColor();
            
            // Возвращаем строку или пустую, если null
            return Console.ReadLine()?.Trim() ?? string.Empty;
        }

        // ============================================
        // ЧТЕНИЕ СТРОКИ С ПРОВЕРКОЙ НА БУКВЫ
        // ============================================
        private static string ReadStringWithLetters(string prompt)
        {
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Program.PrintCentered(prompt);
                Console.ResetColor();
                
                string input = Console.ReadLine()?.Trim() ?? string.Empty;

                // Проверяем на пустоту
                if (string.IsNullOrEmpty(input))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Program.PrintCentered("Ошибка: поле не может быть пустым.");
                    Console.ResetColor();
                    continue;
                }

                // Проверяем, что есть хотя бы одна буква
                bool hasLetter = input.Any(c => char.IsLetter(c));
                
                if (hasLetter)
                {
                    return input;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Program.PrintCentered("Ошибка: поле должно содержать буквы.");
                    Console.ResetColor();
                }
            }
        }
    }
}
