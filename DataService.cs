using System;
using System.Collections.Generic;
using System.IO;

namespace ConsoleApp7
{
    // ============================================
    // КЛАСС ДЛЯ СОХРАНЕНИЯ И ЗАГРУЗКИ ДАННЫХ
    // ============================================
    public static class DataService
    {
        // ============================================
        // ЗАГРУЗКА КНИГ ИЗ ФАЙЛА
        // ============================================
        public static List<Book> LoadBooks(string path)
        {
            var list = new List<Book>();  // Создаем пустой список
            
            // Если файл не существует, возвращаем пустой список
            if (!File.Exists(path)) return list;
            
            try
            {
                // Открываем файл для чтения
                using var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
                using var br = new BinaryReader(fs);  // Читаем бинарные данные
                
                // Читаем все книги из файла
                while (br.BaseStream.Position < br.BaseStream.Length)
                {
                    list.Add(new Book
                    {
                        Id = br.ReadInt32(),      // Читаем int (4 байта)
                        Title = br.ReadString(),  // Читаем строку
                        Author = br.ReadString(),
                        Year = br.ReadInt32(),
                        Genre = br.ReadString()
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка чтения книг: {ex.Message}");
            }
            return list;
        }

        // ============================================
        // ЗАГРУЗКА ЧИТАТЕЛЕЙ ИЗ ФАЙЛА
        // ============================================
        public static List<Reader> LoadReaders(string path)
        {
            var list = new List<Reader>();
            
            if (!File.Exists(path)) return list;
            
            try
            {
                using var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
                using var br = new BinaryReader(fs);
                
                while (br.BaseStream.Position < br.BaseStream.Length)
                {
                    list.Add(new Reader
                    {
                        Id = br.ReadInt32(),
                        LastName = br.ReadString(),
                        FirstName = br.ReadString(),
                        Address = br.ReadString(),
                        Age = br.ReadInt32()
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка чтения читателей: {ex.Message}");
            }
            return list;
        }

        // ============================================
        // СОХРАНЕНИЕ КНИГ В ФАЙЛ
        // ============================================
        public static void SaveBooks(string path, List<Book> books)
        {
            try
            {
                // Создаем директорию, если её нет
                Directory.CreateDirectory(Path.GetDirectoryName(path) ?? ".");
                
                // Открываем файл для записи
                using var fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None);
                using var bw = new BinaryWriter(fs);
                
                // Записываем каждую книгу
                foreach (var b in books)
                {
                    bw.Write(b.Id);
                    bw.Write(b.Title ?? string.Empty);   // Если null, записываем пустую строку
                    bw.Write(b.Author ?? string.Empty);
                    bw.Write(b.Year);
                    bw.Write(b.Genre ?? string.Empty);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка записи книг: {ex.Message}");
            }
        }

        // ============================================
        // СОХРАНЕНИЕ ЧИТАТЕЛЕЙ В ФАЙЛ
        // ============================================
        public static void SaveReaders(string path, List<Reader> readers)
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path) ?? ".");
                
                using var fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None);
                using var bw = new BinaryWriter(fs);
                
                foreach (var r in readers)
                {
                    bw.Write(r.Id);
                    bw.Write(r.LastName ?? string.Empty);
                    bw.Write(r.FirstName ?? string.Empty);
                    bw.Write(r.Address ?? string.Empty);
                    bw.Write(r.Age);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка записи читателей: {ex.Message}");
            }
        }
    }
}
