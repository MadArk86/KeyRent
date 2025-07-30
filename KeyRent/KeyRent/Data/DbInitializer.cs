using KeyRent.Models;
using System;
using System.Linq;

namespace KeyRent.Data
{
    public static class DbInitializer
    {
        // Metoda inicjalizująca bazę danych
        public static void Initialize(RentContext context)
        {
            // Sprawdzamy, czy baza danych istnieje. Jeśli nie, to ją tworzymy
            context.Database.EnsureCreated();

            // Jeśli tabele zawierają już dane, to nie wykonujemy ponownej inicjalizacji
            if (context.Lockers.Any() || context.Users.Any() || context.Rentals.Any())
            {
                return; // Baza danych już zawiera dane, nie trzeba jej ponownie inicjalizować
            }

            // Dodajemy przykładowych użytkowników
            var users = new User[]
            {
                new User { FirstName = "Jan", LastName = "Kowalski", Email = "jan.kowalski@example.com", PhoneNumber = "123-456-789" },
                new User { FirstName = "Anna", LastName = "Nowak", Email = "anna.nowak@example.com", PhoneNumber = "987-654-321" },
                new User { FirstName = "Marek", LastName = "Wiśniewski", Email = "marek.wisniewski@example.com", PhoneNumber = "555-555-555" }
            };

            // Dodajemy użytkowników do bazy danych
            foreach (var user in users)
            {
                context.Users.Add(user);
            }
            context.SaveChanges();

            // Dodajemy przykładowe szafki
            var lockers = new Locker[]
            {
                new Locker { Location = "Budynek A - Piętro 1", IsAvailable = true },
                new Locker { Location = "Budynek A - Piętro 2", IsAvailable = true },
                new Locker { Location = "Budynek B - Piętro 1", IsAvailable = false },
                new Locker { Location = "Budynek B - Piętro 2", IsAvailable = true }
            };

            // Dodajemy szafki do bazy danych
            foreach (var locker in lockers)
            {
                context.Lockers.Add(locker);
            }
            context.SaveChanges();

            // Dodajemy przykładowe wynajmy
            var rentals = new Rental[]
            {
                new Rental { UserId = 1, LockerId = 1, RentalStart = DateTime.Parse("2025-03-01"), RentalEnd = null },  // Jan wynajmuje szafkę 1
                new Rental { UserId = 2, LockerId = 2, RentalStart = DateTime.Parse("2025-03-02"), RentalEnd = null },  // Anna wynajmuje szafkę 2
                new Rental { UserId = 3, LockerId = 3, RentalStart = DateTime.Parse("2025-03-03"), RentalEnd = DateTime.Parse("2025-03-10") },  // Marek wynajmuje szafkę 3
                new Rental { UserId = 1, LockerId = 4, RentalStart = DateTime.Parse("2025-03-05"), RentalEnd = null }  // Jan wynajmuje szafkę 4
            };

            // Dodajemy wynajmy do bazy danych
            foreach (var rental in rentals)
            {
                context.Rentals.Add(rental);
            }
            context.SaveChanges();
        }
    }
}
