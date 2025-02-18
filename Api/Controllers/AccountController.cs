using System.Security.Cryptography;
using System.Text;
using Api.Data;
using Api.Dtos;
using Api.Entities;
using Api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    public class AccountController(DataContext context, ITokenService tokenService) : BaseApiController
    {
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await UsernameExists(registerDto.Username)) return BadRequest("username is taken");

            using var hmac = new HMACSHA512();

            var user = new AppUser
            {
                Username = registerDto.Username.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt = hmac.Key,
            };

            context.Users.Add(user);
            await context.SaveChangesAsync();

            return new UserDto{
                Username = user.Username,
                Token = tokenService.CreateToken(user)
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            // Check if the username exists in the database, and compare it with the input username
            var user = await context.Users.FirstOrDefaultAsync(x => x.Username == loginDto.Username.ToLower());

            // Return Unauthorized if the username doesn't match any user in the database
            if (user == null) return Unauthorized("user not found");

            // Create an instance of HMACSHA512 using the user's stored password salt
            using var hmac = new HMACSHA512(user.PasswordSalt);

            // Compute the hash of the input password using the same algorithm
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            // Compare each byte of the computed hash with the stored hash to verify password validity
            for (int i = 0; i < computedHash.Length; i++)
            {
                // If any byte doesn't match, the password is invalid
                if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid Password");
            }
            return new UserDto{
                Username = user.Username,
                Token = tokenService.CreateToken(user)
            };
        }

        // Function to check if the username exists in the database
        private async Task<bool> UsernameExists(string username)
        {
            return await context.Users.AnyAsync(x => x.Username.ToLower() == username.ToLower()); //Bob != bob
        }

    }
}