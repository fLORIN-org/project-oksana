using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp7
{
    public static class ReaderManager
    {
        private static readonly List<Reader> _readers = new();

        public static void Load(string path)
        {
            _readers.Clear();
            _readers.AddRange(DataService.LoadReaders(path));
        }

        public static void Save(string path) => DataService.SaveReaders(path, _readers);
        public static List<Reader> GetAll() => new(_readers);

        public static bool Add(Reader reader)
        {
            if (_readers.Any(r => r.Id == reader.Id)) return false;
            _readers.Add(reader);
            return true;
        }

        public static bool RemoveById(int id) => _readers.RemoveAll(r => r.Id == id) > 0;

        public static List<Reader> SearchByLastName(string lastName) =>
            string.IsNullOrEmpty(lastName) ? new List<Reader>(_readers) :
            _readers.FindAll(r => r.LastName.IndexOf(lastName, StringComparison.OrdinalIgnoreCase) >= 0);

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
            Console.WriteLine($"  {"ID",-5} {"Фамилия",-18} {"Имя",-18} {"Адрес",-25} Возраст");
            Console.WriteLine(new string('-', 80));
            Console.ResetColor();

            foreach (var r in list)
            {
                Console.WriteLine($"  {r.Id,-5} {FormatCell(r.LastName, 18)} {FormatCell(r.FirstName, 18)} {FormatCell(r.Address, 25)} {r.Age}");
            }

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine($"\n  Всего записей: {list.Count}");
            Console.ResetColor();
        }

        public static Reader? InputNewReader()
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Program.PrintCentered("--- Добавление читателя ---");
            Console.ResetColor();

            int id = ReadInt("ID: ");
            string lastName = ReadStringWithLetters("Фамилия: ");
            string firstName = ReadStringWithLetters("Имя: ");
            string address = ReadString("Адрес: ");
            int age = ReadInt("Возраст: ");

            return new Reader(id, lastName, firstName, address, age);
        }

        private static string FormatCell(string value, int maxLength)
        {
            value = string.IsNullOrEmpty(value) ? "N/A" : value.Trim();
            return value.Length > maxLength ? value.Substring(0, maxLength - 3) + "..." : value.PadRight(maxLength);
        }

        private static int ReadInt(string prompt)
        {
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Program.PrintCenteredInput(prompt);
                Console.ResetColor();

                if (int.TryParse(Console.ReadLine(), out int val)) return val;

                Console.ForegroundColor = ConsoleColor.Red;
                Program.PrintCentered("Ошибка: введите корректное число.");
                Console.ResetColor();
            }
        }

        private static string ReadString(string prompt)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Program.PrintCenteredInput(prompt);
            Console.ResetColor();
            return Console.ReadLine()?.Trim() ?? string.Empty;
        }

        private static string ReadStringWithLetters(string prompt)
        {
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Program.PrintCenteredInput(prompt);
                Console.ResetColor();

                string input = Console.ReadLine()?.Trim() ?? string.Empty;
                if (string.IsNullOrEmpty(input))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Program.PrintCentered("Ошибка: поле не может быть пустым.");
                    Console.ResetColor();
                    continue;
                }

                if (input.Any(char.IsLetter)) return input;

                Console.ForegroundColor = ConsoleColor.Red;
                Program.PrintCentered("Ошибка: поле должно содержать буквы.");
                Console.ResetColor();
            }
        }
    }
}
