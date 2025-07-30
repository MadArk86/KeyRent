using KeyRent.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KeyRent.Properties.DAL
{
    public interface IUserRepository
    {
        // Asynchroniczne pobranie użytkowników
        Task<List<User>> GetUsersAsync();

        // Asynchroniczne pobranie użytkownika po ID
        Task<User> GetUserByIdAsync(int id);

        // Asynchroniczne dodanie użytkownika
        Task AddUserAsync(User user);

        // Asynchroniczne edytowanie użytkownika
        Task EditUserAsync(User user);

        // Asynchroniczne usunięcie użytkownika
        Task DeleteUserAsync(int id);

        // Sprawdzanie, czy użytkownik istnieje (asynchroniczne)
        Task<bool> UserExistsAsync(int id);
    }
}
