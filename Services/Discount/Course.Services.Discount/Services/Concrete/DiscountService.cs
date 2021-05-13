using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Course.Services.Discount.Services.Abstract;
using Course.Shared.Dtos;
using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Course.Services.Discount.Services.Concrete
{
    public class DiscountService : IDiscountService
    {
        private readonly IConfiguration _configuration;
        private readonly IDbConnection _dbConnection;

        public DiscountService(IConfiguration configuration)
        {
            _configuration = configuration;
            _dbConnection = new NpgsqlConnection(_configuration.GetConnectionString("PostgreSql"));
        }

        public async Task<Response<List<Models.Discount>>> GetAll()
        {
            var discounts = await _dbConnection.QueryAsync<Models.Discount>("Select * from discount");

            return Response<List<Models.Discount>>.Success(discounts.ToList(), 200);
        }

        public async Task<Response<Models.Discount>> GetById(int id)
        {
            var discount = (await _dbConnection.QueryAsync<Models.Discount>("select * from discount where id = @Id", new { id })).SingleOrDefault();

            if (discount == null)
            {
                return Response<Models.Discount>.Fail("Discount not found", 404);
            }

            return Response<Models.Discount>.Success(discount, 200);
        }

        public async Task<Response<NoContent>> Save(Models.Discount discount)
        {
            var saveStatus = await _dbConnection.ExecuteAsync("insert into discount (userid,rate,code) values (@UserId,@Rate,@Code)", discount);
            if (saveStatus > 0)
            {
                return Response<NoContent>.Success(200);
            }
            return Response<NoContent>.Fail("An error occurred while adding", 500);
        }

        public async Task<Response<NoContent>> Update(Models.Discount discount)
        {
            var updateStatus = await _dbConnection.ExecuteAsync("update discount set userid=@UserId, rate=@Rate, code=@Code where id=@Id",
                new
                {
                    Id = discount.Id,
                    UserId = discount.UserId,
                    Rate = discount.Rate,
                    Code = discount.Code
                });
            if (updateStatus > 0)
            {
                return Response<NoContent>.Success(200);
            }
            return Response<NoContent>.Fail("Discount not found", 400);
        }

        public async Task<Response<NoContent>> Delete(int id)
        {
            var deleteStatus = await _dbConnection.ExecuteAsync("delete from discount where id=@Id", new { Id = id });
            if (deleteStatus > 0)
            {
                return Response<NoContent>.Success(200);
            }
            return Response<NoContent>.Fail("Discount not found", 400);
        }

        public async Task<Response<Models.Discount>> GetByCodeAndUserId(string code, string userId)
        {
            var discount = (await _dbConnection.QueryAsync<Models.Discount>("select * from discount where code=@Code and userid=@UserId",
                new
                {
                    Code = code,
                    UserId = userId
                })).FirstOrDefault();
            return discount == null ?
                Response<Models.Discount>.Fail("Discount not found", 400)
                :
                Response<Models.Discount>.Success(discount, 200);
        }
    }
}
