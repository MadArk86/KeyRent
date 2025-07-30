using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using KeyRent.Services;
using KeyRent.ViewModels;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly UserService _userService;

    public AdminController(UserService userService)
    {
        _userService = userService;
    }

    public async Task<IActionResult> Index()
    {
        var users = await _userService.GetUsersAsync();
        return View(users);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        return View(user);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(UserViewModel model)
    {
        if (ModelState.IsValid)
        {
            await _userService.EditUserAsync(model);
            return RedirectToAction(nameof(Index));
        }
        return View(model);
    }

    public async Task<IActionResult> Delete(int id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        return View(user);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _userService.DeleteUserAsync(id);
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = "Admin")]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(UserViewModel model)
    {
        if (ModelState.IsValid)
        {
            await _userService.AddUserAsync(model);
            return RedirectToAction(nameof(Index));
        }
        return View(model);
    }


}
