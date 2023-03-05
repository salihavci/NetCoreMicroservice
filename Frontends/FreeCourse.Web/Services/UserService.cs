using FreeCourse.Web.Abstractions;
using FreeCourse.Web.DTOs;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace FreeCourse.Web.Services
{
    public class UserService : IUserService
    {
        private readonly HttpClient _client;

        public UserService(HttpClient client)
        {
            _client = client;
        }

        public async Task<UserDto> GetUser()
        {
            
            return await _client.GetFromJsonAsync<UserDto>("/api/user").ConfigureAwait(false);
        }
    }
}
