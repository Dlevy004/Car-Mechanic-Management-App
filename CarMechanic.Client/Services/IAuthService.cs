using CarMechanic.Shared.Models;

namespace CarMechanic.Client.Services
{
    public interface IAuthService
    {

        Task<LoginResult> Login(LoginModel loginModel);

        Task Logout();
    }
}
