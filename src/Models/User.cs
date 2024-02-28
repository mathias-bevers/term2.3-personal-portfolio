using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace PersonalPortfolio.Models
{
    public class User
    {
        private const string PASSWORD_PATTERN = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,15}$";
        
        [Key, Column(Order = 1), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserID { get; set; }

        [Required(ErrorMessage = "Username cannot be empty!"), StringLength(25, MinimumLength = 3)]
        public string UserName { get; set; }

        // The password has to be at least 8 characters but no more than 15 characters.
        // The password also requires a capital letter; a lowercase letter; a number; and a special character.
        [Required(ErrorMessage = "Password cannot be empty!"), DataType(DataType.Password)]
        public string Password { get; set; }
        
        [NotMapped] public string? ErrorMessage { get; set; }
    }
}