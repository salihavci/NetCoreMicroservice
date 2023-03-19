using System.Threading.Tasks;

namespace FreeCourse.Web.Abstractions
{
    public interface IClientCredentialTokenService
    {
        Task<string> GetToken();
    }
}
