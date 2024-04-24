using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonalPortfolio.Models
{
    public class Author
    {
        [Key, Column(Order = 1), DatabaseGenerated(DatabaseGeneratedOption.Identity), ForeignKey("Author")]
        public int AuthorID { get; set; }

        [Required(ErrorMessage = "Please enter first name!"), StringLength(50)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter last name!"), StringLength(75)]
        public string LastName { get; set; }

        public ICollection<Book> Books { get; set; } = new List<Book>();

        [NotMapped] public string FullName => $"{FirstName} {LastName}";
    }
}