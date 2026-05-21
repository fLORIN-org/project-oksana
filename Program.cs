using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp7
{
    class Program
    {
        private const string BookFile = "books.bin";
        private const string ReaderFile = "readers.bin";

        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.CursorVisible = false;

            // Убрали жесткую установку BufferWidth/Height, чтобы избежать предупреждений кроссплатформенности
            // Если нужно, можно обернуть в if (OperatingSystem.IsWindows())

            BookManager.Load(BookFile);
            ReaderManager.Load(ReaderFile);

            while (true)
            {
                Console.Clear();
                DrawHeader("БИБЛИОТЕЧНАЯ ИНФОРМАЦИОННАЯ СИСТЕМА");

                Console.ForegroundColor = ConsoleColor.Yellow;
                PrintCentered("1. Работа с книгами");
                PrintCentered("2. Работа с читателями");
                PrintCentered("3. Сохранить и выйти");
                Console.ResetColor();

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write(new string(' ', (Console.WindowWidth - "Enter Your choice:".Length) / 2));
                Console.Write("Enter Your choice:");
                Console.ResetColor();

                // Защита от null
                string choice = Console.ReadLine()?.Trim() ?? string.Empty;

                switch (choice)
                {
                    case "1": BooksMenu(); break;
                    case "2": ReadersMenu(); break;
                    case "3":
                        BookManager.Save(BookFile);
                        ReaderManager.Save(ReaderFile);
                        Console.ForegroundColor = ConsoleColor.Green;
                        PrintCentered("Данные сохранены. Работа завершена.");
                        Console.ResetColor();
                        return;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        PrintCentered("Неверный выбор.");
                        Console.ResetColor();
                        WaitForEnter();
                        break;
                }
            }
        }

        static void DrawHeader(string title)
        {
            int width = Console.WindowWidth;
            int separatorLength = Math.Min(60, width - 20);
            string separator = new string('-', separatorLength);

            Console.ForegroundColor = ConsoleColor.DarkGray;
            PrintCentered(separator + "***");
            Console.ForegroundColor = ConsoleColor.Yellow;
            PrintCentered($"\"{title}\"");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            PrintCentered(separator + "***");
            Console.ResetColor();
            Console.WriteLine();
        }

        // ✅ Сделано public, чтобы другие файлы видели этот метод
        public static void PrintCentered(string text)
        {
            int width = Console.WindowWidth;
            int padding = (width - text.Length) / 2;
            if (padding < 0) padding = 0;
            Console.WriteLine(new string(' ', padding) + text);
        }

        static void WaitForEnter()
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            PrintCentered("Для продолжения нажмите любую клавишу . . .");
            Console.ResetColor();
            Console.ReadKey();
        }

        static void BooksMenu()
        {
            while (true)
            {
                Console.Clear();
                DrawHeader("УПРАВЛЕНИЕ КНИГАМИ");

                Console.ForegroundColor = ConsoleColor.Yellow;
                PrintCentered("1. Добавить книгу");
                PrintCentered("2. Показать все книги");
                PrintCentered("3. Поиск и фильтрация");
                PrintCentered("4. Удалить книгу по ID");
                PrintCentered("5. Показать статистику");
                PrintCentered("6. Сортировать книги");
                PrintCentered("0. Выход в главное меню");
                Console.ResetColor();

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write(new string(' ', (Console.WindowWidth - "Enter Your choice:".Length) / 2));
                Console.Write("Enter Your choice:");
                Console.ResetColor();

                string choice = Console.ReadLine()?.Trim() ?? string.Empty;
                switch (choice)
                {
                    case "1":
                        var newBook = BookManager.InputNewBook();
                        if (newBook.HasValue)
                        {
                            if (BookManager.Add(newBook.Value))
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                                PrintCentered("Книга успешно добавлена!");
                                Console.ResetColor();
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                PrintCentered("Ошибка: книга с таким ID уже существует.");
                                Console.ResetColor();
                            }
                        }
                        break;
                    case "2": BookManager.ShowBooks(BookManager.GetAll()); break;
                    case "3":
                        var (title, author, minYear) = BookManager.InputSearchCriteria();
                        BookManager.ShowBooks(BookManager.Search(title, author, minYear));
                        break;
                    case "4":
                        int delId = ReadInt("Введите ID книги для удаления:");
                        if (BookManager.RemoveById(delId))
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            PrintCentered($"Книга с ID {delId} удалена.");
                            Console.ResetColor();
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            PrintCentered("Книга с таким ID не найдена.");
                            Console.ResetColor();
                        }
                        break;
                    case "5":
                        var (total, avg, recent) = BookManager.GetStats();
                        Console.ForegroundColor = ConsoleColor.White;
                        PrintCentered("СТАТИСТИКА ПО КНИГАМ:");
                        PrintCentered($"Общее количество: {total}");
                        PrintCentered($"Средний год издания: {avg:F1}");
                        PrintCentered($"Книг с 2000 года: {recent}");
                        Console.ResetColor();
                        break;
                    case "6":
                        Console.ForegroundColor = ConsoleColor.White;
                        PrintCentered("Выберите критерий сортировки:");
                        PrintCentered("1. По ID");
                        PrintCentered("2. По названию");
                        PrintCentered("3. По году издания");
                        PrintCentered("4. По автору");
                        Console.ResetColor();
                        BookManager.ShowBooks(BookManager.SortBy(ReadString("Ваш выбор:")));
                        break;
                    case "0": return;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        PrintCentered("Неверный выбор.");
                        Console.ResetColor();
                        break;
                }
                WaitForEnter();
            }
        }

        static void ReadersMenu()
        {
            while (true)
            {
                Console.Clear();
                DrawHeader("УПРАВЛЕНИЕ ЧИТАТЕЛЯМИ");

                Console.ForegroundColor = ConsoleColor.Yellow;
                PrintCentered("1. Добавить читателя");
                PrintCentered("2. Показать всех читателей");
                PrintCentered("3. Удалить читателя по ID");
                PrintCentered("4. Поиск читателя по фамилии");
                PrintCentered("0. Выход в главное меню");
                Console.ResetColor();

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write(new string(' ', (Console.WindowWidth - "Enter Your choice:".Length) / 2));
                Console.Write("Enter Your choice:");
                Console.ResetColor();

                string choice = Console.ReadLine()?.Trim() ?? string.Empty;
                switch (choice)
                {
                    case "1":
                        var newReader = ReaderManager.InputNewReader();
                        if (newReader.HasValue)
                        {
                            if (ReaderManager.Add(newReader.Value))
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                                PrintCentered("Читатель успешно добавлен!");
                                Console.ResetColor();
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                PrintCentered("Ошибка: читатель с таким ID уже существует.");
                                Console.ResetColor();
                            }
                        }
                        break;
                    case "2": ReaderManager.ShowReaders(ReaderManager.GetAll()); break;
                    case "3":
                        int delId = ReadInt("Введите ID читателя для удаления:");
                        if (ReaderManager.RemoveById(delId))
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            PrintCentered($"Читатель с ID {delId} удален.");
                            Console.ResetColor();
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            PrintCentered("Читатель с таким ID не найден.");
                            Console.ResetColor();
                        }
                        break;
                    case "4":
                        var found = ReaderManager.SearchByLastName(ReadString("Введите фамилию для поиска:"));
                        Console.ForegroundColor = ConsoleColor.White;
                        PrintCentered($"Найдено записей: {found.Count}");
                        Console.ResetColor();
                        ReaderManager.ShowReaders(found);
                        break;
                    case "0": return;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        PrintCentered("Неверный выбор.");
                        Console.ResetColor();
                        break;
                }
                WaitForEnter();
            }
        }

        static int ReadInt(string prompt)
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

        static string ReadString(string prompt)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write(new string(' ', (Console.WindowWidth - prompt.Length) / 2));
            Console.Write(prompt);
            Console.ResetColor();
            return Console.ReadLine()?.Trim() ?? string.Empty;
        }
    }
}