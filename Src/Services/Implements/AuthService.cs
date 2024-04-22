using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using taller1WebMovil.Src.DTOs;
using taller1WebMovil.Src.Models;
using taller1WebMovil.Src.Repositories.Implements;
using taller1WebMovil.Src.Repositories.Interfaces;
using taller1WebMovil.Src.Services.Interfaces;



namespace taller1WebMovil.Src.Services.Implements
{

    public class AuthService : IAuthService
    {


        private readonly IUserRepository _userRepository; //Se inyecta el repositorio de usuarios
        private readonly IConfiguration _configuration; //Se inyecta la configuración

        private readonly IMapperService _mapperService; //Se inyecta el servicio de mapeo

        private readonly IRoleRepository _roleRepository; //Se inyecta el repositorio de roles

        public AuthService(IUserRepository userRepository, IConfiguration configuration, IMapperService mapperService, IRoleRepository roleRepository)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _mapperService = mapperService;
            _roleRepository = roleRepository;
        }
        
        public async Task<string> LoginUser(LoginUserDTO loginUserDTO) //Método para loguear un usuario
        {
            string message = "Credenciales inválidas.";

            var user = await _userRepository.GetUserByEmail(loginUserDTO.Email.ToString()); //Se obtiene el usuario por su email

            if (user == null) 
            {
                return message; //Si el usuario no existe, se retorna el mensaje de credenciales inválidas
            }

            if(user.Active == false)
            {
                return "Usuario inactivo, no puede iniciar sesión."; //Si el usuario esta inactivo, se retorna un mensaje de usuario inactivo
            }
            
            var result = BCrypt.Net.BCrypt.Verify(loginUserDTO.Password, user.Password); //Se verifica si la contraseña coincide pero estando hasheadas

            if (!result)
            {
                return message; //Si la contraseña no coincide, se retorna el mensaje de credenciales inválidas
            }
            
            return CreateToken(user); //Si el usuario existe y la contraseña coincide, se crea un token y se retorna
        }

        public async Task<string> RegisterUser(RegisterUserDTO registerUserDTO) //Método para registrar un usuario
        {
            var mappedUser = _mapperService.RegisterUserDTOToUser(registerUserDTO); //Se mapea el DTO a un objeto de tipo User

            if (_userRepository.VerifyUserByEMail(mappedUser.Email).Result) //Se verifica si el usuario ya existe
            {
                throw new Exception("El email ya se encuentra registrado.");
            }
            if (_userRepository.VerifyUserByRut(mappedUser.Rut).Result) //Se verifica si el usuario ya existe
            {
                throw new Exception("El rut ya se encuentra registrado.");
            }
            var salt = BCrypt.Net.BCrypt.GenerateSalt(12); //Se genera una sal para hashear la contraseña
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(registerUserDTO.Password, salt); //Se hashea la contraseña

            mappedUser.Password = passwordHash; //Se asigna la contraseña hasheada al usuario
            var role = await _roleRepository.GetRoleByName("User"); //Se obtiene el rol de usuario
            if (role == null)
            {
                throw new Exception("Error en el servidor, pruebe mas tarde.");
            }
            mappedUser.RoleId = role.Id; //Se asigna el rol al usuario
            mappedUser.Active = true; //Se activa el usuario
            await _userRepository.AddUser(mappedUser); //Se agrega el usuario a la base de datos
            var user = await _userRepository.GetUserByEmail(mappedUser.Email); //Se obtiene el usuario recien agregado
            if (user == null)
            {
                throw new Exception("Error en el servidor, pruebe mas tarde.");
            }
            return CreateToken(user); //Se crea un token y se retorna, para un inicio de sesion  automatico
        }

        private string CreateToken (User user){ //Método para crear un token

            var claims = new List<Claim> //Se crean los claims del token
            {
                new ("Id", user.Id.ToString()), //Se agrega el id del usuario
                new ("Email", user.Email), //Se agrega el email del usuario
                new (ClaimTypes.Role, user.Role.Name) //Se agrega el rol del usuario, importante para la autorización por roles
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