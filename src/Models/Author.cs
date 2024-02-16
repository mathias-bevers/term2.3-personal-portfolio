using System.ComponentModel.DataAnnotations;

namespace PersonalPortfolio.Models
{
    public class Author
    {
        public ICollection<Book> Books { get; set; } = new List<Book>();
        public int AuthorId { get; set; }

        [StringLength(50)] public string FirstName { get; set; }

        [StringLength(75)] public string LastName { get; set; }
    }
}