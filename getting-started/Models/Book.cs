namespace EFCoreWebDemo
{
    public class Book
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public int AutorId { get; set; }
        public Author author { get; set; }
    }
}
