namespace ConsoleApp7
{
    // ============================================
    // СТРУКТУРА КНИГИ
    // ============================================
    public struct Book
    {
        // Поля структуры (данные книги)
        public int Id;           // Уникальный идентификатор книги
        public string Title;     // Название книги
        public string Author;    // Автор книги
        public int Year;         // Год издания
        public string Genre;     // Жанр книги

        // Конструктор для создания книги с параметрами
        public Book(int id, string title, string author, int year, string genre)
        {
            Id = id;
            Title = title;
            Author = author;
            Year = year;
            Genre = genre;
        }
    }

    // ============================================
    // СТРУКТУРА ЧИТАТЕЛЯ
    // ============================================
    public struct Reader
    {
        // Поля структуры (данные читателя)
        public int Id;              // Уникальный идентификатор читателя
        public string LastName;     // Фамилия
        public string FirstName;    // Имя
        public string Address;      // Адрес
        public int Age;             // Возраст

        // Конструктор для создания читателя с параметрами
        public Reader(int id, string lastName, string firstName, string address, int age)
        {
            Id = id;
            LastName = lastName;
            FirstName = firstName;
            Address = address;
            Age = age;
        }
    }
}
