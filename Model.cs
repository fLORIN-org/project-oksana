namespace ConsoleApp7
{
    // Используем основной конструктор (Primary Constructor)
    public struct Book(int Id, string Title, string Author, int Year, string Genre)
    {
        public int Id { get; set; } = Id;
        public string Title { get; set; } = Title;
        public string Author { get; set; } = Author;
        public int Year { get; set; } = Year;
        public string Genre { get; set; } = Genre;
    }

    public struct Reader(int Id, string LastName, string FirstName, string Address, int Age)
    {
        public int Id { get; set; } = Id;
        public string LastName { get; set; } = LastName;
        public string FirstName { get; set; } = FirstName;
        public string Address { get; set; } = Address;
        public int Age { get; set; } = Age;
    }
}