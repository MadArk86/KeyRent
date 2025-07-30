using KeyRent.Services;
using KeyRent.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace KeyRent.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }
        [AllowAnonymous]
        // GET: Lista użytkowników
        public async Task<IActionResult> IndexUsers(string searchQuery, string sortOrder)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParam"] = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["EmailSortParam"] = sortOrder == "email" ? "email_desc" : "email";
            ViewData["CurrentSearchQuery"] = searchQuery;

            var users = await _userService.GetUsersAsync(searchQuery, sortOrder);
            return View(users);
        }

        // GET: Formularz dodawania użytkownika — dostęp publiczny (rejestracja)
        
        public IActionResult CreateUser()
        {
            ViewData["FormTitle"] = "Create New User";
            ViewData["FormType"] = "Create";
            return View();
        }

        // POST: Dodawanie użytkownika — dostęp publiczny (rejestracja)
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUser(UserViewModel userViewModel)
        {
            if (ModelState.IsValid)
            {
                await _userService.AddUserAsync(userViewModel);
                return RedirectToAction(nameof(IndexUsers));
            }
            return View(userViewModel);
        }
        [Authorize(Roles = "Admin")]
        // GET: Edytowanie użytkownika — wymaga zalogowania
        public async Task<IActionResult> EditUser(int? id)
        {
            if (id == null) return NotFound();

            var userViewModel = await _userService.GetUserByIdAsync(id.Value);
            if (userViewModel == null) return NotFound();

            return View(userViewModel);
        }
        [Authorize(Roles = "Admin")]
        // POST: Edytowanie użytkownika — wymaga zalogowania
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(int id, UserViewModel userViewModel)
        {
            if (id != userViewModel.Id) return NotFound();

            if (ModelState.IsValid)
            {
                var success = await _userService.EditUserAsync(userViewModel);
                if (!success) return NotFound();
                return RedirectToAction(nameof(IndexUsers));
            }
            return View(userViewModel);
        }
        [Authorize(Roles = "Admin")]
        // GET: Potwierdzenie usunięcia użytkownika — wymaga zalogowania
        public async Task<IActionResult> DeleteUser(int? id)
        {
            if (id == null) return NotFound();

            var userViewModel = await _userService.GetUserByIdAsync(id.Value);
            if (userViewModel == null) return NotFound();

            return View(userViewModel);
        }
        [Authorize(Roles = "Admin")]
        // POST: Usunięcie użytkownika — wymaga zalogowania
        [HttpPost, ActionName("DeleteUser")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUserConfirmed(int id)
        {
            var success = await _userService.DeleteUserAsync(id);
            if (!success) return NotFound();
            return RedirectToAction(nameof(IndexUsers));
        }
    }
}
