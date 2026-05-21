using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp7
{
    public static class BookManager
    {
        // ✅ Упрощенная инициализация
        private static readonly List<Book> _books = new();

        public static void Load(string path)
        {
            _books.Clear();
            _books.AddRange(DataService.LoadBooks(path));
        }

        public static void Save(string path) => DataService.SaveBooks(path, _books);
        public static List<Book> GetAll() => new(_books);

        public static bool Add(Book book)
        {
            if (_books.Any(b => b.Id == book.Id)) return false;
            _books.Add(book);
            return true;
        }

        public static bool RemoveById(int id) => _books.RemoveAll(b => b.Id == id) > 0;

        public static List<Book> Search(string title, string author, int? minYear) =>
            _books.Where(b =>
                (string.IsNullOrEmpty(title) || b.Title.Contains(title, StringComparison.OrdinalIgnoreCase)) &&
                (string.IsNullOrEmpty(author) || b.Author.Contains(author, StringComparison.OrdinalIgnoreCase)) &&
                (!minYear.HasValue || b.Year >= minYear.Value)
            ).ToList();

        public static List<Book> SortBy(string criterion)
        {
            var sorted = new List<Book>(_books);
            switch (criterion)
            {
                case "1": sorted.Sort((a, b) => a.Id.CompareTo(b.Id)); break;
                case "2": sorted.Sort((a, b) => string.Compare(a.Title, b.Title, StringComparison.OrdinalIgnoreCase)); break;
                case "3": sorted.Sort((a, b) => a.Year.CompareTo(b.Year)); break;
                case "4": sorted.Sort((a, b) => string.Compare(a.Author, b.Author, StringComparison.OrdinalIgnoreCase)); break;
            }
            return sorted;
        }

        public static (int Total, double AvgYear, int Recent) GetStats()
        {
            if (_books.Count == 0) return (0, 0, 0);
            int sum = 0, recent = 0;
            foreach (var b in _books)
            {
                sum += b.Year;
                if (b.Year >= 2000) recent++;
            }
            return (_books.Count, (double)sum / _books.Count, recent);
        }

        public static void ShowBooks(List<Book> list)
        {
            if (list == null || list.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Program.PrintCentered("Список книг пуст.");
                Console.ResetColor();
                return;
            }

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            int windowWidth = Console.WindowWidth;
            string header = $"  {"ID",-5} {"Название",-25} {"Автор",-20} {"Год",-6} Жанр";
            int padding = (windowWidth - header.Length) / 2;
            if (padding < 0) padding = 0;
            Console.WriteLine(new string(' ', padding) + header);
            Console.WriteLine(new string(' ', padding) + new string('-', Math.Min(70, windowWidth - padding)));
            Console.ResetColor();

            foreach (var b in list)
            {
                string line = $"  {b.Id,-5} {FormatCell(b.Title, 25)} {FormatCell(b.Author, 20)} {b.Year,-6} {FormatCell(b.Genre, 15)}";
                Console.WriteLine(new string(' ', padding) + line);
            }

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine(new string(' ', padding) + $"\n  Всего записей: {list.Count}");
            Console.ResetColor();
        }

        public static Book? InputNewBook()
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Program.PrintCentered("--- Добавление книги ---");
            Console.ResetColor();

            int id = ReadInt("ID:");
            string title = ReadStringWithLetters("Название:");
            string author = ReadStringWithLetters("Автор:");
            int year = ReadInt("Год издания:");
            string genre = ReadStringWithLetters("Жанр:");

            return new Book(id, title, author, year, genre);
        }

        public static (string Title, string Author, int? MinYear) InputSearchCriteria()
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Program.PrintCentered("--- Поиск (оставьте поле пустым для пропуска) ---");
            Console.ResetColor();

            string title = ReadString("Название содержит:");
            string author = ReadString("Автор содержит:");

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write(new string(' ', (Console.WindowWidth - "Год издания >= :".Length) / 2));
            Console.Write("Год издания >= :");
            Console.ResetColor();
            string y = Console.ReadLine()?.Trim();
            int? minYear = null;
            if (!string.IsNullOrEmpty(y) && int.TryParse(y, out int yr)) minYear = yr;

            return (title, author, minYear);
        }

        private static string FormatCell(string value, int maxLength)
        {
            value = string.IsNullOrEmpty(value) ? "N/A" : value.Trim();
            return value.Length > maxLength ? value[..(maxLength - 3)] + "..." : value.PadRight(maxLength);
        }

        private static int ReadInt(string prompt)
        {
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write(new string(' ', (Console.WindowWidth - prompt.Length) / 2));
                Console.Write(prompt);
                Console.ResetColor();

                if (int.TryParse(Console.ReadLine(), out int val)) return val;

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(new string(' ', (Console.WindowWidth - 35) / 2) + "Ошибка: введите корректное число.");
                Console.ResetColor();
            }
        }

        private static string ReadString(string prompt)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write(new string(' ', (Console.WindowWidth - prompt.Length) / 2));
            Console.Write(prompt);
            Console.ResetColor();
            return Console.ReadLine()?.Trim() ?? string.Empty;
        }

        private static string ReadStringWithLetters(string prompt)
        {
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write(new string(' ', (Console.WindowWidth - prompt.Length) / 2));
                Console.Write(prompt);
                Console.ResetColor();

                string input = Console.ReadLine()?.Trim() ?? string.Empty;

                if (string.IsNullOrEmpty(input))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(new string(' ', (Console.WindowWidth - 40) / 2) + "Ошибка: поле не может быть пустым.");
                    Console.ResetColor();
                    continue;
                }

                if (input.Any(char.IsLetter)) return input;

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(new string(' ', (Console.WindowWidth - 45) / 2) + "Ошибка: поле должно содержать буквы.");
                Console.ResetColor();
            }
        }
    }
}