using System.ComponentModel.DataAnnotations;

namespace PersonalPortfolio.Models
{
    public class Book
    {
        public Author Author { get; set; }
        public int AuthorId { get; set; }
        public int BookId { get; set; }

        [StringLength(255)] public string Title { get; set; }
    }
}