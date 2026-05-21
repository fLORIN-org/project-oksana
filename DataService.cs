using System;
using System.Collections.Generic;
using System.IO;

namespace ConsoleApp7
{
    public static class DataService
    {
        public static List<Book> LoadBooks(string path)
        {
            // Упрощенная инициализация
            var list = new List<Book>();
            if (!File.Exists(path)) return list;
            try
            {
                using var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
                using var br = new BinaryReader(fs);
                while (br.BaseStream.Position < br.BaseStream.Length)
                {
                    list.Add(new Book(br.ReadInt32(), br.ReadString(), br.ReadString(), br.ReadInt32(), br.ReadString()));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка чтения книг: {ex.Message}");
            }
            return list;
        }

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
                    list.Add(new Reader(br.ReadInt32(), br.ReadString(), br.ReadString(), br.ReadString(), br.ReadInt32()));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка чтения читателей: {ex.Message}");
            }
            return list;
        }

        public static void SaveBooks(string path, List<Book> books)
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path) ?? ".");
                using var fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None);
                using var bw = new BinaryWriter(fs);
                foreach (var b in books)
                {
                    bw.Write(b.Id);
                    bw.Write(b.Title ?? string.Empty);
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