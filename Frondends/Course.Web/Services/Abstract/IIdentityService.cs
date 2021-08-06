using System.Threading.Tasks;
using Course.Shared.Dtos;
using Course.Web.Models;
using IdentityModel.Client;

namespace Course.Web.Services.Abstract
{
    public interface IIdentityService
    {
        Task<Response<bool>> SignIn(SignInInput signInInput);

        Task<TokenResponse> GetAccessTokenByRefleshToken();

        Task RemoveRefleshToken();
    }
}
