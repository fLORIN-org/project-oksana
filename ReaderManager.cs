using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp7
{
    // ============================================
    // КЛАСС ДЛЯ УПРАВЛЕНИЯ ЧИТАТЕЛЯМИ
    // ============================================
    public static class ReaderManager
    {
        // Приватное поле для хранения читателей
        private static readonly List<Reader> _readers = new();

        // ============================================
        // ЗАГРУЗКА ЧИТАТЕЛЕЙ
        // ============================================
        public static void Load(string path)
        {
            _readers.Clear();
            _readers.AddRange(DataService.LoadReaders(path));
        }

        // ============================================
        // СОХРАНЕНИЕ ЧИТАТЕЛЕЙ
        // ============================================
        public static void Save(string path) => DataService.SaveReaders(path, _readers);
        
        // ============================================
        // ПОЛУЧЕНИЕ ВСЕХ ЧИТАТЕЛЕЙ
        // ============================================
        public static List<Reader> GetAll() => new(_readers);

        // ============================================
        // ДОБАВЛЕНИЕ ЧИТАТЕЛЯ
        // ============================================
        public static bool Add(Reader reader)
        {
            // Проверяем уникальность ID
            if (_readers.Any(r => r.Id == reader.Id)) return false;
            
            _readers.Add(reader);
            return true;
        }

        // ============================================
        // УДАЛЕНИЕ ЧИТАТЕЛЯ ПО ID
        // ============================================
        public static bool RemoveById(int id) => _readers.RemoveAll(r => r.Id == id) > 0;

        // ============================================
        // ПОИСК ЧИТАТЕЛЕЙ ПО ФАМИЛИИ
        // ============================================
        public static List<Reader> SearchByLastName(string lastName) =>
            // Если фамилия пустая - возвращаем всех
            string.IsNullOrEmpty(lastName) ? new List<Reader>(_readers) :
            // Иначе ищем по фамилии
            _readers.FindAll(r => r.LastName.IndexOf(lastName, StringComparison.OrdinalIgnoreCase) >= 0);

        // ============================================
        // ОТОБРАЖЕНИЕ СПИСКА ЧИТАТЕЛЕЙ
        // ============================================
        public static void ShowReaders(List<Reader> list)
        {
            if (list == null || list.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Program.PrintCentered("Список читателей пуст.");
                Console.ResetColor();
                return;
            }

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            
            // Заголовок таблицы
            Console.WriteLine($"  {"ID",-5} {"Фамилия",-18} {"Имя",-18} {"Адрес",-25} Возраст");
            Console.WriteLine(new string('-', 80));
            Console.ResetColor();

            // Вывод читателей
            foreach (var r in list)
            {
                Console.WriteLine($"  {r.Id,-5} {FormatCell(r.LastName, 18)} {FormatCell(r.FirstName, 18)} {FormatCell(r.Address, 25)} {r.Age}");
            }

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine($"\n  Всего записей: {list.Count}");
            Console.ResetColor();
        }

        // ============================================
        // ВВОД НОВОГО ЧИТАТЕЛЯ
        // ============================================
        public static Reader? InputNewReader()
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Program.PrintCentered("--- Добавление читателя ---");
            Console.ResetColor();

            int id = ReadInt("ID:");
            string lastName = ReadStringWithLetters("Фамилия:");
            string firstName = ReadStringWithLetters("Имя:");
            string address = ReadString("Адрес:");
            int age = ReadInt("Возраст:");

            return new Reader(id, lastName, firstName, address, age);
        }

        // ============================================
        // ФОРМАТИРОВАНИЕ ЯЧЕЙКИ
        // ============================================
        private static string FormatCell(string value, int maxLength)
        {
            value = string.IsNullOrEmpty(value) ? "N/A" : value.Trim();
            return value.Length > maxLength ? value.Substring(0, maxLength - 3) + "..." : value.PadRight(maxLength);
        }

        // ============================================
        // ЧТЕНИЕ ЧИСЛА
        // ============================================
        private static int ReadInt(string prompt)
        {
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Program.PrintCentered(prompt);
                Console.ResetColor();
                
                if (int.TryParse(Console.ReadLine(), out int val)) 
                    return val;
                
                Console.ForegroundColor = ConsoleColor.Red;
                Program.PrintCentered("Ошибка: введите корректное число.");
                Console.ResetColor();
            }
        }

        // ============================================
        // ЧТЕНИЕ СТРОКИ
        // ============================================
        private static string ReadString(string prompt)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Program.PrintCentered(prompt);
            Console.ResetColor();
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

                if (string.IsNullOrEmpty(input))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Program.PrintCentered("Ошибка: поле не может быть пустым.");
                    Console.ResetColor();
                    continue;
                }

                // Проверяем наличие букв
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
