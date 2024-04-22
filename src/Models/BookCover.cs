using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonalPortfolio.Models
{
    public class BookCover
    {
        [Key, Column(Order = 1), DatabaseGenerated(DatabaseGeneratedOption.Identity), ForeignKey("BookCover")]
        public int CoverID { get; set; }
        
        public byte[] Bytes { get; set; }
        
        public string FileExtension { get; set; }

        public int BookID;
        
        [ForeignKey("BookID")]
        public Book Book { get; set; } 
    }
}