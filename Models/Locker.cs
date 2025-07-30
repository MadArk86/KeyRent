using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KeyRent.Models
{
    public class Locker
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Lokalizacja jest wymagana.")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Lokalizacja musi zawierać od {2} do {1} znaków.")]
        public string Location { get; set; } // np. "Budynek A - Piętro 2"

        public bool IsAvailable { get; set; }

        // Relacja: szafka może być wynajęta w ramach wielu rezerwacji
        public ICollection<Rental> Rentals { get; set; }
    }
}
