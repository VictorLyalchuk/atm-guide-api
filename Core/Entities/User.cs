using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;


namespace Core.Entities
{
    public class User : IdentityUser
    {
        [MaxLength(20, ErrorMessage = "Fisrt Name can't be longer than 20 characters")]
        public string FirstName { get; set; } = string.Empty;

        [MaxLength(20, ErrorMessage = "Last Name can't be longer than 20 characters")]
        public string LastName { get; set; } = string.Empty;

        [MaxLength(250, ErrorMessage = "Name can't be longer than 250 characters")]
        public string ImagePath { get; set; } = string.Empty;

        // Навігація до Region
        public Region? Region { get; set; }
        public int? RegionId { get; set; }

        // Навігація до Bank
        public Bank? Bank { get; set; }
        public int? BankId { get; set; }

        // Колекція логів сесії
        public ICollection<AppSession>? AppSessions { get; set; }
    }
}
