using Microsoft.AspNetCore.Mvc;
using GreenGrassAPI.Dtos;
using GreenGrassAPI.Models;

namespace GreenGrassAPI.Interfaces
{
    public interface IUserRepository : IGenericRepository<User>
    {
        string GenerateToken(User user);
        Task<ActionResult<User?>> Register(UserRegisterRequestDto request);
        Task<User?> GetUserByLogin(string request);
        bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt);
        //Task<List<User>> GetUsersByRoleId(string role);
    }
}