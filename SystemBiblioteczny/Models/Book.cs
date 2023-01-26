namespace SystemBiblioteczny
{
    public class Book
    {
        public int Id_Book { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public bool Availability { get; set; }
        public int Id_Library { get; set; }

        public Book(int id_Book, string author, string title, bool availability, int id_Library)
        {
            Id_Book = id_Book;
            Author = author;
            Title = title;
            Availability = availability;
            Id_Library = id_Library;
        }

        public Book()
        {
        }
    }
}
