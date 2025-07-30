using System;
using System.Collections.Generic;

namespace KeyRent.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        // Relacja: użytkownik może wynająć wiele szafek
        public ICollection<Rental>? Rentals { get; set; }
    }
}
