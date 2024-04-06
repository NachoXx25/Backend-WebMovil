using Bogus;
using taller1WebMovil.Src.Models;

namespace taller1WebMovil.Src.Data
{
    public class DataSeeder
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope()){
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<DataContext>();

                if(!context.Roles.Any()){
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
                        RoleId = 1
                        // faltaria añadir el campo de estado si este aplica en base de datos (estado siempre 1 para admin)
                    };
                    context.Users.Add(user);
                    /*
                    var faker = new Faker<User>()
                    // TO DO: .RuleFor(u => u.Rut, faker => faker.Person.Cpf()) validar rut
                    .RuleFor(u => u.Name, faker => faker.Person.FullName) 
                    .RuleFor(u => u.BirthDate, faker => faker.Person.DateOfBirth) 
                    .RuleFor(u => u.Email, faker => faker.Person.Email) // TO DO: validar email unico
                    .RuleFor(u => u.Gender, faker => faker.Random.Bool() ? "Masculino" : "Femenino") 
                    .RuleFor(u => u.Password, faker => BCrypt.Net.BCrypt.HashPassword("password")) 
                    .RuleFor(u => u.RoleId, faker => 2);

                    var users = faker.Generate(10);
                    context.Users.AddRange(users);
                    */
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
                context.SaveChanges();
            }
        }
    }
}