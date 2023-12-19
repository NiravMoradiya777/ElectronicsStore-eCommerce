using ElectronicsStore_eCommerce.Models;
using ElectronicsStore_eCommerce.Models.ViewModel;
using ElectronicsStore_eCommerce.Services.Interface;
using Microsoft.AspNetCore.Identity;

namespace ElectronicsStore_eCommerce.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public UserService(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
        }

        public async Task<IdentityResult> RegisterAsync(RegisterViewModel model)
        {
            // Basic validation
            if (string.IsNullOrWhiteSpace(model.Email) || string.IsNullOrWhiteSpace(model.Password))
            {
                return IdentityResult.Failed(new IdentityError { Description = "Email and password are required." });
            }

            // Additional validation as needed

            var user = new User
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Birthdate = model.Birthdate
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            await _userManager.AddToRoleAsync(user, "Admin");

            return result;
        }

        public async Task<SignInResult> LoginAsync(LoginViewModel model)
        {
            // Basic validation
            if (string.IsNullOrWhiteSpace(model.Email) || string.IsNullOrWhiteSpace(model.Password))
            {
                return SignInResult.Failed;
            }

            // Additional validation as needed

            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
            
            return result;
        }

        public async Task<User> FindUserByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }
    }
}
