using System.Collections.Generic;

namespace EFCoreWebDemo
{
    public class Author
    {
        public int AuthorId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ICollection<Book> books { get; set; } = new List<Book>();
    }
}
