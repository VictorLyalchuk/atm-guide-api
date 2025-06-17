using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class ATM
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Serial number is required")]
        [MaxLength(100, ErrorMessage = "Serial number can't be longer than 50 characters")]
        public string SerialNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Status is required")]
        [MaxLength(100, ErrorMessage = "Status can't be longer than 100 characters")]
        public string Status { get; set; } = string.Empty;

        // Навігація до Моделі ATM
        public int? ATMModelId { get; set; }
        public ATMModel? ATMModel { get; set; }

        // Навігація до версії програмного забезпечення ATM
        public int? ATMSoftId { get; set; }
        public ATMSoft? ATMSoft { get; set; }

        // Навігація до інструкцій ATM
        public ICollection<Instruction>? Instructions { get; set; }

        // Навігація до помилок ATM
        public ICollection<ATMErrorCode>? ATMErrorCodes { get; set; }
    }
}