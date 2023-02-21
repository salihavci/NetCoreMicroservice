using Dapper;
using FreeCourse.Shared.Dtos;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace FreeCourse.Services.Discount.Services
{
    public class DiscountService : IDiscountService
    {
        private readonly IConfiguration _configuration;
        private readonly IDbConnection _dbConnection;

        public DiscountService(IConfiguration configuration)
        {
            _configuration = configuration;
            _dbConnection = new NpgsqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        }

        public async Task<Response<NoContent>> DeleteAsync(int id)
        {
            var deleteStatus = await _dbConnection.ExecuteAsync("DELETE FROM discount WHERE id = @Id", new { Id = id }).ConfigureAwait(false);
            if (deleteStatus > 0)
            {
                return Response<NoContent>.Success(204);
            }
            return Response<NoContent>.Fail("Discount could not deleted.", 500);
        }

        public async Task<Response<Models.Discount>> GetByCodeAndUserIdAsync(string code, string userId)
        {
            var discount = (await _dbConnection.QueryAsync<Models.Discount>("select * from discount where userid = @UserId and code = @Code", new { UserId = userId, Code = code }).ConfigureAwait(false)).SingleOrDefault();
            if (discount == null)
            {
                return Response<Models.Discount>.Fail("Discount not found.", 404);
            }
            return Response<Models.Discount>.Success(discount, 200);
        }

        public async Task<Response<Models.Discount>> GetByIdAsync(int id)
        {
            var discount = (await _dbConnection.QueryAsync<Models.Discount>("select * from discount where id = @Id", new { Id = id }).ConfigureAwait(false)).SingleOrDefault();
            if (discount == null)
            {
                return Response<Models.Discount>.Fail("Discount not found", 404);
            }
            return Response<Models.Discount>.Success(discount, 200);
        }

        public async Task<Response<List<Models.Discount>>> GetListAsync()
        {
            var discounts = await _dbConnection.QueryAsync<Models.Discount>("select * from discount").ConfigureAwait(false);
            return Response<List<Models.Discount>>.Success(discounts.ToList(), 200);
        }

        public async Task<Response<NoContent>> SaveAsync(Models.Discount data)
        {
            var saveStatus = await _dbConnection.ExecuteAsync("INSERT INTO discount(userid,rate,code) VALUES (@UserId,@Rate,@Code)", data).ConfigureAwait(false);
            if (saveStatus > 0)
            {
                return Response<NoContent>.Success(204);
            }
            return Response<NoContent>.Fail("Discount could not created.", 500);

        }

        public async Task<Response<NoContent>> UpdateAsync(Models.Discount data)
        {
            var updateStatus = await _dbConnection.ExecuteAsync("UPDATE discount SET userid = @UserId, code=@Code, rate = @Rate WHERE id = @Id", new { Id = data.Id, UserId = data.UserId, Code = data.Code, Rate = data.Rate }).ConfigureAwait(false);
            if (updateStatus > 0)
            {
                return Response<NoContent>.Success(204);
            }
            return Response<NoContent>.Fail("Discount could not updated.", 500);
        }
    }
}
