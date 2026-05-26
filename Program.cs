using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp7

// ============================================
// ГЛАВНЫЙ КЛАСС ПРОГРАММЫ
// ============================================
class Program
{
    // Константы - имена файлов для сохранения
    private const string BookFile = "books.bin";
    private const string ReaderFile = "readers.bin";

    // ============================================
    // ТОЧКА ВХОДА В ПРОГРАММУ
    // ============================================
    static void Main()
    {
        // Настраиваем консоль
        Console.OutputEncoding = System.Text.Encoding.UTF8;  // Поддержка кириллицы
        Console.CursorVisible = false;  // Скрываем курсор

        // Загружаем данные из файлов
        BookManager.Load(BookFile);
        ReaderManager.Load(ReaderFile);

        // Главный цикл программы
        while (true)
        {
            Console.Clear();  // Очищаем экран
            DrawHeader("БИБЛИОТЕЧНАЯ ИНФОРМАЦИОННАЯ СИСТЕМА");

            // Выводим главное меню
            Console.ForegroundColor = ConsoleColor.Yellow;
            PrintCentered("1. Работа с книгами");
            PrintCentered("2. Работа с читателями");
            PrintCentered("3. Сохранить и выйти");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Cyan;
            PrintCentered("Enter Your choice:");
            Console.ResetColor();

            // Читаем выбор пользователя
            string choice = Console.ReadLine()?.Trim();
            
            // Обрабатываем выбор
            switch (choice)
            {
                case "1": BooksMenu(); break;  // Меню книг
                case "2": ReadersMenu(); break;  // Меню читателей
                case "3":  // Выход с сохранением
                    BookManager.Save(BookFile);
                    ReaderManager.Save(ReaderFile);
                    Console.ForegroundColor = ConsoleColor.Green;
                    PrintCentered("Данные сохранены. Работа завершена.");
                    Console.ResetColor();
                    return;
                default:  // Неверный выбор
                    Console.ForegroundColor = ConsoleColor.Red;
                    PrintCentered("Неверный выбор.");
                    Console.ResetColor();
                    WaitForEnter();
                    break;
            }
        }
    }

    // ============================================
    // ОТРИСОВКА ЗАГОЛОВКА
    // ============================================
    static void DrawHeader(string title)
    {
        Console.ForegroundColor = ConsoleColor.DarkGray;
        PrintCentered("-----------------------------------***");
        Console.ForegroundColor = ConsoleColor.Yellow;
        PrintCentered($"\"{title}\"");
        Console.ForegroundColor = ConsoleColor.DarkGray;
        PrintCentered("-----------------------------------***");
        Console.ResetColor();
    }

    // ============================================
    // ВЫВОД ТЕКСТА ПО ЦЕНТРУ
    // ============================================
    static void PrintCentered(string text)
    {
        int width = Console.WindowWidth;
        int padding = (width - text.Length) / 2;
        if (padding < 0) padding = 0;
        Console.WriteLine(new string(' ', padding) + text);
    }

    // ============================================
    // ОЖИДАНИЕ НАЖАТИЯ КЛАВИШИ
    // ============================================
    static void WaitForEnter()
    {
        Console.ForegroundColor = ConsoleColor.Gray;
        PrintCentered("Для продолжения нажмите любую клавишу . . .");
        Console.ResetColor();
        Console.ReadKey();
    }

    // ============================================
    // МЕНЮ УПРАВЛЕНИЯ КНИГАМИ
    // ============================================
    static void BooksMenu()
    {
        while (true)
        {
            Console.Clear();
            DrawHeader("УПРАВЛЕНИЕ КНИГАМИ");

            // Меню операций с книгами
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
            PrintCentered("Enter Your choice:");
            Console.ResetColor();

            string choice = Console.ReadLine()?.Trim();
            
            switch (choice)
            {
                case "1":  // Добавить книгу
                    var newBook = BookManager.InputNewBook();
                    if (newBook != null)
                    {
                        if (BookManager.Add(newBook.Value))
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            PrintCentered("Row added successfully!");
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
                    
                case "2":  // Показать все книги
                    BookManager.ShowBooks(BookManager.GetAll());
                    break;
                    
                case "3":  // Поиск
                    var (title, author, minYear) = BookManager.InputSearchCriteria();
                    BookManager.ShowBooks(BookManager.Search(title, author, minYear));
                    break;
                    
                case "4":  // Удалить книгу
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
                    
                case "5":  // Статистика
                    var (total, avg, recent) = BookManager.GetStats();
                    Console.ForegroundColor = ConsoleColor.White;
                    PrintCentered("СТАТИСТИКА ПО КНИГАМ:");
                    PrintCentered($"Общее количество: {total}");
                    PrintCentered($"Средний год издания: {avg:F1}");
                    PrintCentered($"Книг с 2000 года: {recent}");
                    Console.ResetColor();
                    break;
                    
                case "6":  // Сортировка
                    Console.ForegroundColor = ConsoleColor.White;
                    PrintCentered("Выберите критерий сортировки:");
                    PrintCentered("1. По ID");
                    PrintCentered("2. По названию");
                    PrintCentered("3. По году издания");
                    PrintCentered("4. По автору");
                    Console.ResetColor();
                    BookManager.ShowBooks(BookManager.SortBy(ReadString("Ваш выбор:")));
                    break;
                    
                case "0":  // Выход
                    return;
                    
                default:  // Ошибка
                    Console.ForegroundColor = ConsoleColor.Red;
                    PrintCentered("Неверный выбор.");
                    Console.ResetColor();
                    break;
            }
            WaitForEnter();
        }
    }

    // ============================================
    // МЕНЮ УПРАВЛЕНИЯ ЧИТАТЕЛЯМИ
    // ============================================
    static void ReadersMenu()
    {
        while (true)
        {
            Console.Clear();
            DrawHeader("УПРАВЛЕНИЕ ЧИТАТЕЛЯМИ");

            // Меню операций с читателями
            Console.ForegroundColor = ConsoleColor.Yellow;
            PrintCentered("1. Добавить читателя");
            PrintCentered("2. Показать всех читателей");
            PrintCentered("3. Удалить читателя по ID");
            PrintCentered("4. Поиск читателя по фамилии");
            PrintCentered("0. Выход в главное меню");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Cyan;
            PrintCentered("Enter Your choice:");
            Console.ResetColor();

            string choice = Console.ReadLine()?.Trim();
            
            switch (choice)
            {
                case "1":  // Добавить читателя
                    var newReader = ReaderManager.InputNewReader();
                    if (newReader != null)
                    {
                        if (ReaderManager.Add(newReader.Value))
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            PrintCentered("Row added successfully!");
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
                    
                case "2":  // Показать всех
                    ReaderManager.ShowReaders(ReaderManager.GetAll());
                    break;
                    
                case "3":  // Удалить
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
                    
                case "4":  // Поиск
                    var found = ReaderManager.SearchByLastName(ReadString("Введите фамилию для поиска:"));
                    Console.ForegroundColor = ConsoleColor.White;
                    PrintCentered($"Найдено записей: {found.Count}");
                    Console.ResetColor();
                    ReaderManager.ShowReaders(found);
                    break;
                    
                case "0":  // Выход
                    return;
                    
                default:  // Ошибка
                    Console.ForegroundColor = ConsoleColor.Red;
                    PrintCentered("Неверный выбор.");
                    Console.ResetColor();
                    break;
            }
            WaitForEnter();
        }
    }

    // ============================================
    // ЧТЕНИЕ ЧИСЛА С КОНСОЛИ
    // ============================================
    static int ReadInt(string prompt)
    {
        while (true)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            PrintCentered(prompt);
            Console.ResetColor();
            
            if (int.TryParse(Console.ReadLine(), out int val)) 
                return val;
            
            Console.ForegroundColor = ConsoleColor.Red;
            PrintCentered("Ошибка: введите корректное число.");
            Console.ResetColor();
        }
    }

    // ============================================
    // ЧТЕНИЕ СТРОКИ С КОНСОЛИ
    // ============================================
    static string ReadString(string prompt)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        PrintCentered(prompt);
        Console.ResetColor();
        return Console.ReadLine()?.Trim() ?? string.Empty;
    }
}
