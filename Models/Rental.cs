using System;
using System.ComponentModel.DataAnnotations;

namespace KeyRent.Models
{
    public class Rental
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int LockerId { get; set; }

        [Required(ErrorMessage = "Data rozpoczęcia wynajmu jest wymagana.")]
        [DataType(DataType.DateTime)]
        public DateTime RentalStart { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? RentalEnd { get; set; } // może być null, jeśli wynajem trwa

        // Relacje
        [Required(ErrorMessage = "Użytkownik jest wymagany.")]
        public User User { get; set; }

        [Required(ErrorMessage = "Szafka jest wymagana.")]
        public Locker Locker { get; set; }
    }
}
