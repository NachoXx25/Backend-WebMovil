using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using taller1WebMovil.Src.DTOs;
using taller1WebMovil.Src.Models;
using taller1WebMovil.Src.Repositories.Interfaces;
using taller1WebMovil.Src.Services.Interfaces;

namespace taller1WebMovil.Src.Services.Implements
{

    public class AuthService : IAuthService
    {


        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public AuthService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }
        
        public async Task<string> LoginUser(LoginUserDTO loginUserDTO)
        {
            string message = "Credenciales inválidas.";

            var user = await _userRepository.GetUserByEmail(loginUserDTO.Email.ToString());

            if (user == null)
            {
                return message;
            }
            
            var result = BCrypt.Net.BCrypt.Verify(loginUserDTO.Password, user.Password);

            if (!result)
            {
                return message;
            }
            
            return CreateToken(user);
        }

        public Task<string> RegisterUser(RegisterUserDTO registerUserDTO)
        {
            throw new NotImplementedException();
        }

        private string CreateToken (User user){

            var claims = new List<Claim>
            {
                new ("Id", user.Id.ToString()),
                new ("Email", user.Email),
                new (ClaimTypes.Role, user.Role.Name)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: creds
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }

    }
}