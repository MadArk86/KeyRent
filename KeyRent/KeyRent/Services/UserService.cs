using AutoMapper;
using KeyRent.Models;
using KeyRent.Properties.DAL;
using KeyRent.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KeyRent.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        // Pobranie wszystkich użytkowników jako ViewModel
        public async Task<List<UserViewModel>> GetUsersAsync()
        {
            var users = await _userRepository.GetUsersAsync();
            return _mapper.Map<List<UserViewModel>>(users);
        }

        // Pobranie jednego użytkownika po ID jako ViewModel
        public async Task<UserViewModel> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            return user != null ? _mapper.Map<UserViewModel>(user) : null;
        }

        // Dodanie użytkownika na podstawie ViewModel
        public async Task AddUserAsync(UserViewModel viewModel)
        {
            var user = _mapper.Map<User>(viewModel);
            await _userRepository.AddUserAsync(user);
        }

        // Edytowanie użytkownika na podstawie ViewModel
        public async Task<bool> EditUserAsync(UserViewModel viewModel)
        {
            if (!await _userRepository.UserExistsAsync(viewModel.Id))
                return false;

            var existingUser = await _userRepository.GetUserByIdAsync(viewModel.Id);
            _mapper.Map(viewModel, existingUser); // nadpisuje właściwości istniejącej encji

            await _userRepository.EditUserAsync(existingUser);
            return true;
        }

        // Usunięcie użytkownika
        public async Task<bool> DeleteUserAsync(int id)
        {
            if (!await _userRepository.UserExistsAsync(id))
                return false;

            await _userRepository.DeleteUserAsync(id);
            return true;
        }

        // Pobranie użytkowników z filtrowaniem i sortowaniem jako ViewModel
        public async Task<List<UserViewModel>> GetUsersAsync(string searchQuery, string sortOrder)
        {
            var users = await _userRepository.GetUsersAsync();

            // Filtrowanie
            if (!string.IsNullOrEmpty(searchQuery))
            {
                users = users.Where(u => u.LastName.Contains(searchQuery) || u.Email.Contains(searchQuery)).ToList();
            }

            // Sortowanie
            users = sortOrder switch
            {
                "name_desc" => users.OrderByDescending(u => u.LastName).ToList(),
                "email" => users.OrderBy(u => u.Email).ToList(),
                "email_desc" => users.OrderByDescending(u => u.Email).ToList(),
                _ => users.OrderBy(u => u.LastName).ToList(),
            };

            return _mapper.Map<List<UserViewModel>>(users);
        }
    }
}
