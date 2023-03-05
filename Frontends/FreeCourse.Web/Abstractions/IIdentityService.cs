using FreeCourse.Shared.Dtos;
using FreeCourse.Web.Inputs;
using IdentityModel.Client;
using System.Threading.Tasks;

namespace FreeCourse.Web.Abstractions
{
    public interface IIdentityService
    {
        Task<Response<bool>> SignInAsync(SigninInput data);
        Task<TokenResponse> GetAccessTokenByRefreshTokenAsync();
        Task RevokeRefreshToken();
    }
}
