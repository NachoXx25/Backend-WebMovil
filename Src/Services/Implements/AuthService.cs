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


        private readonly IUserRepository _userRepository; //Se inyecta el repositorio de usuarios
        private readonly IConfiguration _configuration; //Se inyecta la configuración

        public AuthService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }
        
        public async Task<string> LoginUser(LoginUserDTO loginUserDTO) //Método para loguear un usuario
        {
            string message = "Credenciales inválidas.";

            var user = await _userRepository.GetUserByEmail(loginUserDTO.Email.ToString()); //Se obtiene el usuario por su email

            if (user == null) 
            {
                return message; //Si el usuario no existe, se retorna el mensaje de credenciales inválidas
            }
            
            var result = BCrypt.Net.BCrypt.Verify(loginUserDTO.Password, user.Password); //Se verifica si la contraseña coincide pero estando hasheadas

            if (!result)
            {
                return message; //Si la contraseña no coincide, se retorna el mensaje de credenciales inválidas
            }
            
            return CreateToken(user); //Si el usuario existe y la contraseña coincide, se crea un token y se retorna
        }

        public Task<string> RegisterUser(RegisterUserDTO registerUserDTO) //Método para registrar un usuario
        {
            throw new NotImplementedException(); //pendiente
        }

        private string CreateToken (User user){ //Método para crear un token

            var claims = new List<Claim> //Se crean los claims del token
            {
                new ("Id", user.Id.ToString()), //Se agrega el id del usuario
                new ("Email", user.Email), //Se agrega el email del usuario
                new (ClaimTypes.Role, user.Role.Name) //Se agrega el rol del usuario, importante para la autorización
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value!)); //Se obtiene la clave secreta del token

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256); //Se crean las credenciales del token
            var token = new JwtSecurityToken( //Se crea el token
                claims: claims, //Se agregan los claims
                expires: DateTime.Now.AddHours(2), //Se agrega la expiración del token
                signingCredentials: creds //Se agregan las credenciales
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token); //Se crea el token
            return jwt; //Se retorna el token
        }

    }
}