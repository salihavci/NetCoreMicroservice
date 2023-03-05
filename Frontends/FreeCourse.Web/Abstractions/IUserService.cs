using FreeCourse.Web.DTOs;
using System.Threading.Tasks;

namespace FreeCourse.Web.Abstractions
{
    public interface IUserService
    {
        Task<UserDto> GetUser();
    }
}
