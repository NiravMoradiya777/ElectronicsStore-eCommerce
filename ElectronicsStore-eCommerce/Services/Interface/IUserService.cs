using ElectronicsStore_eCommerce.Models;
using ElectronicsStore_eCommerce.Models.ViewModel;
using Microsoft.AspNetCore.Identity;

namespace ElectronicsStore_eCommerce.Services.Interface
{
    public interface IUserService
    {
        Task<IdentityResult> RegisterAsync(RegisterViewModel model);
        Task<SignInResult> LoginAsync(LoginViewModel model);
        Task<User> FindUserByEmailAsync(string email);
    }
}
