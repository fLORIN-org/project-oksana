namespace ConsoleApp7
{
    public struct Book
    {
        public int Id;
        public string Title;
        public string Author;
        public int Year;
        public string Genre;

        public Book(int id, string title, string author, int year, string genre)
        {
            Id = id;
            Title = title;
            Author = author;
            Year = year;
            Genre = genre;
        }
    }

    public struct Reader
    {
        public int Id;
        public string LastName;
        public string FirstName;
        public string Address;
        public int Age;

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
