using ElectronicsStore_eCommerce.Models.ViewModel;
using ElectronicsStore_eCommerce.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ElectronicsStore_eCommerce.Services.Interface;
using System;
using System.Threading.Tasks;

namespace ElectronicsStore_eCommerce.Controllers
{
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly SignInManager<User> _signInManager;

        public UserController(IUserService userService, SignInManager<User> signInManager)
        {
            _userService = userService;
            _signInManager = signInManager;
        }

        [HttpGet]
        [Route("Register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _userService.RegisterAsync(model);

                    if (result.Succeeded)
                    {
                        // Optionally, you can automatically sign in the user after registration
                        await _signInManager.SignInAsync(await _userService.FindUserByEmailAsync(model.Email), isPersistent: false);
                        TempData["SuccessMessage"] = "Registration successful. Welcome!";
                        return RedirectToAction("Index", "Home"); // Redirect to home or another page
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }

                return View(model);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                TempData["ErrorMessage"] = $"An error occurred: {ex.Message}";
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        [Route("Login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _userService.LoginAsync(model);

                    if (result.Succeeded)
                    {
                        // User is not an admin, redirect to the home page or another page
                        TempData["SuccessMessage"] = "Login successful. Welcome!";
                        return RedirectToAction("Index", "Home");
                    }

                    ModelState.AddModelError(string.Empty, "Invalid login attempt");
                }

                return View(model);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                TempData["ErrorMessage"] = $"An error occurred: {ex.Message}";
                return RedirectToAction("Index", "Home");
            }
        }

        [Route("Logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await _signInManager.SignOutAsync();

                TempData["SuccessMessage"] = "Logout successful. Goodbye!";
                // Redirect to the home page or another page after logout
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                TempData["ErrorMessage"] = $"An error occurred: {ex.Message}";
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
