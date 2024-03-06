using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonalPortfolio.Models
{
    public class Book
    {
        [Key, Column(Order = 1), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BookID { get; set; }

        [Required(ErrorMessage = "Please enter a title!"), StringLength(255)]
        public string Title { get; set; }

        [Required, Range(1, int.MaxValue, ErrorMessage = "No author selected!"), ForeignKey("Author")]
        public int AuthorID { get; set; }
        
        public Author? Author { get; set; } = null!;
    }
}