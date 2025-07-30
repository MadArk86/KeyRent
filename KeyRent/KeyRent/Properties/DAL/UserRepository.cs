using KeyRent.Data;
using KeyRent.Models;
using KeyRent.Properties.DAL;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KeyRent.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly RentContext _context;

        public UserRepository(RentContext context)
        {
            _context = context;
        }

        // Pobranie wszystkich użytkowników asynchronicznie
        public async Task<List<User>> GetUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        // Pobranie użytkownika po ID asynchronicznie
        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        // Dodanie użytkownika asynchronicznie
        public async Task AddUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        // Edytowanie użytkownika asynchronicznie
        public async Task EditUserAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        // Usunięcie użytkownika asynchronicznie
        public async Task DeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }

        // Sprawdzanie, czy użytkownik istnieje
        public async Task<bool> UserExistsAsync(int id)
        {
            return await _context.Users.AnyAsync(e => e.Id == id);
        }
    }
}
