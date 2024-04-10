using System;
using System.Text.RegularExpressions;
using Bogus;
using taller1WebMovil.Src.Models;

namespace taller1WebMovil.Src.Data
{
    public class DataSeeder
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope()){ // se crea un scope
                var services = scope.ServiceProvider; // se obtienen los servicios
                var context = services.GetRequiredService<DataContext>(); // se obtiene el contexto de la base de datos

                if(!context.Roles.Any()){ // si no hay roles en la base de datos se crean los roles
                    context.Roles.AddRange(
                        new Role { Name = "Admin"},
                        new Role { Name = "User"}
                    );
                }

                if(!context.Users.Any()){

                    var user = new User {
                        Rut = "20.416.699-4",
                        Name = "Ignacio Mancilla",
                        BirthDate = new DateTime(2000, 10, 25),
                        Email = "ignacio.mancilla@gmail.com",
                        Gender = "Masculino",
                        Password = BCrypt.Net.BCrypt.HashPassword("P4ssw0rd"),
                        RoleId = 1,
                        Active = true
                    };
                    context.Users.Add(user);

                    string[] genders = { "Masculino", "Femenino", "Prefiero no decirlo", "Otro" }; // arreglo de 4 posibles generos
                    var ruts = new HashSet<string>();
                    var emails = new HashSet<string>(); // se crean dos hashsets para guardar los ruts y emails generados, para asegurar que sean unicos
                
                    var faker = new Faker<User>()
                    
                    .RuleFor(u => u.Rut, faker => GenerateRut(ruts)) // Genera un rut aleatorio, valido y unico para cada persona
                    .RuleFor(u => u.Name, f =>
                    {
                        var name = f.Person.FullName;
                        while (name.Length < 8)
                        {
                            name = f.Person.FullName;
                        }
                        return name;
                    }) //no se valida que sean de maximo 255 porque en un caso real no existen nombres tan grandes
                    .RuleFor(u => u.BirthDate, f => f.Date.Between(DateTime.Now.AddYears(-100), DateTime.Now)) // fecha de nacimiento entre 100 años atras y la fecha actual 
                    .RuleFor(u => u.Email, f =>
                    {
                                    var email = f.Person.Email; // se genera un email aleatorio gracias a faker
                                    while (!emails.Add(email)) // se asegura que el email sea unico
                                    {
                                        email = f.Person.Email; // si no es unico se genera otro email
                                    }
                                    return email; // se retorna el email unico  
                    })
                    .RuleFor(u => u.Gender, faker => genders[faker.Random.Int(0, 3)]) // se elige un genero aleatorio dentro del arreglo de generos
                    .RuleFor(u => u.Password, faker => BCrypt.Net.BCrypt.HashPassword("password"))  // se encripta la contraseña con bcrypt
                    .RuleFor(u => u.RoleId, faker => 2) // se asigna el rol de usuario tipo cliente
                    .RuleFor(u => u.Active, faker => true); // se asigna el estado activo al usuario por defecto

                    var users = faker.Generate(10);
                    context.Users.AddRange(users);
                    
                }
                
                if(!context.Products.Any()){
                    // no se hace uso de faker en los productos debido a que su generacion de datos no es controlable, es decir puede que sea un celular pero de categoria libros
                    var product = new Product {
                        Name = "Samsung Galaxy S21",
                        Type = "Teconología",
                        Price = 1000000,
                        Stock = 10,
                        Image = "https://www.samsung.com/co/smartphones/galaxy-s21-5g/buy/hero/hero-mobile-1-1.jpg"
                    };
                    context.Products.Add(product);

                    product = new Product {
                        Name = "Iphone 12",
                        Type = "Teconología",
                        Price = 1200000,
                        Stock = 5,
                        Image = "https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/iphone-12-blue-select-2020?wid=940&hei=1112&fmt=png-alpha&.v=1604343704000"
                    };
                    context.Products.Add(product);

                    product = new Product {
                        Name = "Spiderman",
                        Type = "Juguetería",
                        Price = 20000,
                        Stock = 3,
                        Image = "https://www.lego.com/cdn/cs/set/assets/blt3b6f5d2b6b2f4f3/76187.jpg?fit=bounds&format=jpg&quality=80&width=800&height=800&dpr=1"
                    };
                    context.Products.Add(product);
                }
                context.SaveChanges(); // se guardan los cambios en la base de datos (importante)
            }
        }
        private static Random random = new Random();
        public static string GenerateRut(HashSet<string> existingRuts) //Generación de Rut
        {
            int number;
            string rut;
            do
            {
                number = random.Next(10000000, 99999999); // Genera un número aleatorio de 8 dígitos
                char dv = GetDV(number); // Obtiene el dígito verificador para ese número
                rut = $"{number:N0}-{dv}"; // Retorna el número y el dígito verificador como una cadena
            } while (!IsValidRut(rut) || existingRuts.Contains(rut)); // Repite hasta que el RUT sea válido y único
            existingRuts.Add(rut);
            return rut;
        }
        private static bool IsValidRut(string? rut) //Validación de Rut
        {
            if (string.IsNullOrEmpty(rut))
            {
                return false;
            }

            rut = rut.Replace(".", "").Replace("-", "").ToLower();
            if (!int.TryParse(rut.AsSpan(0, rut.Length - 1), out int number)) 
            {
                return false; 
            }

            char dv = rut[^1]; //Digito verificador
            return dv == GetDV(number); 
        }

        private static char GetDV(int number) //Obtener digito verificador
        {
            int m = 0,
                s = 1;
            for (; number != 0; number /= 10) //Ciclo para obtener el digito verificador
            {
                s = (s + number % 10 * (9 - m++ % 6)) % 11; //Calculo del digito verificador
            }

            return (char)(s != 0 ? s + 47 : 75); //Retorno del digito verificador
        }
    }
}