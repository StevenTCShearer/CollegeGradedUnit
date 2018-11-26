using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Data.Entity.Migrations;
using System.Linq;
using ValueFurniture.Models;
using ValueFurniture.POCO_Classes;

/// <summary>
/// Setting up the Database
/// </summary>
namespace ValueFurniture.Migrations
{

    /// <summary>
    /// Class To Create Database Data 
    /// </summary>
    /// <seealso cref="System.Data.Entity.Migrations.DbMigrationsConfiguration{ValueFurniture.Models.ApplicationDbContext}" />
    public sealed class DbMigrationsConfig : DbMigrationsConfiguration<ApplicationDbContext>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DbMigrationsConfig"/> class.
        /// </summary>
        public DbMigrationsConfig()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        /// <summary>
        /// Runs after upgrading to the latest migration to allow seed data to be updated.
        /// </summary>
        /// <param name="context">Context to be used for updating seed data.</param>

        protected override void Seed(ApplicationDbContext context)
        {
            if (!context.Users.Any())
            {
                var adminEmail = "admin@admin.com";
                var adminUserName = adminEmail;
                var adminFirstName = "System";
                var adminSecondName = "Administrator";
                var adminPassword = adminEmail;
                var adminRole = "Administrator";
                CreateAdmin(context, adminEmail, adminFirstName, adminSecondName, adminUserName, adminPassword, adminRole);

                var testEmail = "test@test.com";
                var testUserName = testEmail;
                var testFirstName = "Test";
                var testSecondName = "User";
                var testPassword = testEmail;
                var testRole = "User";
                CreateTest(context, testEmail, testFirstName, testSecondName, testUserName, testPassword, testRole);
                CreateProduct(context);
                CreateCategories(context);
            }
        }

        /// <summary>
        /// Creates the test.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="testEmail">The test email.</param>
        /// <param name="testFirstName">First name of the test user.</param>
        /// <param name="testSecondName">Second name of the test user.</param>
        /// <param name="testUserName">Username of the test user.</param>
        /// <param name="testPassword">The test password.</param>
        /// <param name="testRole">The test role.</param>
        /// <exception cref="Exception">
        /// </exception>
        private void CreateTest(ApplicationDbContext context, string testEmail, string testFirstName, string testSecondName, string testUserName, string testPassword, string testRole)
        {
            //Creating Test User
            var testUser = new ApplicationUser
            {
                UserName = testUserName,
                FirstName = testFirstName,
                Surname = testSecondName,
                Email = testEmail,
                EmailConfirmed = true,
            };
            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);
            userManager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 1,
                RequireDigit = false,
                RequireLowercase = false,
                RequireNonLetterOrDigit = false,
                RequireUppercase = false,
            };
            var userCreateResult = userManager.Create(testUser, testPassword);
            if (!userCreateResult.Succeeded)
            {
                throw new Exception(string.Join("; ", userCreateResult.Errors));
            }

            //Creating User Role
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var roleCreateResult = roleManager.Create(new IdentityRole(testRole));
            if (!roleCreateResult.Succeeded)
            {
                throw new Exception(string.Join("; ", userCreateResult.Errors));
            }

            //Add Test User to User role
            var addTestRoleResult = userManager.AddToRole(testUser.Id, testRole);
            if (!addTestRoleResult.Succeeded)
            {
                throw new Exception(string.Join("; ", userCreateResult.Errors));
            }
        }

        /// <summary>
        /// Creates the admin.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="adminEmail">The admin email.</param>
        /// <param name="adminFirstName">First name of the admin user.</param>
        /// <param name="adminSecondName">Second name of the admin user.</param>
        /// <param name="adminUserName">Username of the admin user.</param>
        /// <param name="adminPassword">The admin password.</param>
        /// <param name="adminRole">The admin role.</param>
        /// <exception cref="Exception">
        /// </exception>
        private void CreateAdmin(ApplicationDbContext context, string adminEmail, string adminFirstName, string adminSecondName, string adminUserName, string adminPassword, string adminRole)
        {
            //Creating Admin User
            var adminUser = new ApplicationUser
            {
                UserName = adminUserName,
                FirstName = adminFirstName,
                Surname = adminSecondName,
                Email = adminEmail,
                EmailConfirmed = true,
            };
            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);
            userManager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 1,
                RequireDigit = false,
                RequireLowercase = false,
                RequireNonLetterOrDigit = false,
                RequireUppercase = false,
            };
            var userCreateResult = userManager.Create(adminUser, adminPassword);
            if (!userCreateResult.Succeeded)
            {
                throw new Exception(string.Join("; ", userCreateResult.Errors));
            }

            //Creating Administrator Role
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var roleCreateResult = roleManager.Create(new IdentityRole(adminRole));
            if (!roleCreateResult.Succeeded)
            {
                throw new Exception(string.Join("; ", userCreateResult.Errors));
            }

            //Add Admin to Administrator role
            var addAdminRoleResult = userManager.AddToRole(adminUser.Id, adminRole);
            if (!addAdminRoleResult.Succeeded)
            {
                throw new Exception(string.Join("; ", userCreateResult.Errors));
            }
        }

        /// <summary>
        /// Creates the products.
        /// </summary>
        /// <param name="context">The context.</param>
        private void CreateProduct(ApplicationDbContext context)
        {
            context.Products.Add(new Product()
            {
                ProductName = "Lerhamn",
                ProductPrice = 39,
                ProductDetails = "Light antique stain/white stain",
                ProductQuantity = 5,
                ProductPictureURL = "/Pictures/Lerhamn.jpg",
                CategoryId = 1,
            });

            context.Products.Add(new Product()
            {
                ProductName = "Bjursta",
                ProductPrice = 150,
                ProductDetails = "Oak veneer",
                ProductQuantity = 3,
                ProductPictureURL = "/Pictures/Bjursta.jpg",
                CategoryId = 1,
            });

            context.Products.Add(new Product()
            {
                ProductName = "Torkel",
                ProductPrice = 35,
                ProductDetails = "You sit comfortably since the chair is adjustable in height.",
                ProductQuantity = 2,
                ProductPictureURL = "/Pictures/Torkel.jpg",
                CategoryId = 2,

            });

            context.Products.Add(new Product()
            {
                ProductName = "Millberget",
                ProductPrice = 49,
                ProductDetails = "Adjustable tilt tension allows to adjust resistance to suit movements.",
                ProductQuantity = 3,
                ProductPictureURL = "/Pictures/Millberget.jpg",
                CategoryId = 2,

            });

            context.Products.Add(new Product()
            {
                ProductName = "Bekant",
                ProductPrice = 140,
                ProductDetails = "10 year guarantee. Read about the terms in the guarantee brochure.",
                ProductQuantity = 1,
                ProductPictureURL = "/Pictures/Bekant.jpg",
                CategoryId = 1,
            });

            context.Products.Add(new Product()
            {
                ProductName = "Mockelby",
                ProductPrice = 300,
                ProductDetails = "Table with a top layer of solid wood.",
                ProductQuantity = 2,
                ProductPictureURL = "/Pictures/mockelby.jpg",
                CategoryId = 1,
            });

            context.Products.Add(new Product()
            {
                ProductName = "Stornas",
                ProductPrice = 225,
                ProductDetails = "1 extension leaf included.",
                ProductQuantity = 12,
                ProductPictureURL = "/Pictures/Stornas.jpg",
                CategoryId = 1,
            });

            context.Products.Add(new Product()
            {
                ProductName = "Ingatorp",
                ProductPrice = 250,
                ProductDetails = "2 extension leafs included.",
                ProductQuantity = 4,
                ProductPictureURL = "/Pictures/Stornas.jpg",
                CategoryId = 1,
            });

            context.Products.Add(new Product()
            {
                ProductName = "Lack",
                ProductPrice = 16,
                ProductDetails = "Separate shelf for magazines.",
                ProductQuantity = 17,
                ProductPictureURL = "/Pictures/Lack.jpg",
                CategoryId = 1,
            });

            context.Products.Add(new Product()
            {
                ProductName = "Hermnes",
                ProductPrice = 260,
                ProductDetails = "Made of solid wood and warm natural material.",
                ProductQuantity = 8,
                ProductPictureURL = "/Pictures/Hemnes.jpg",
                CategoryId = 3,
            });

            context.Products.Add(new Product()
            {
                ProductName = "Tyssedal",
                ProductPrice = 275,
                ProductDetails = "Hinges with integrated dampers catch the door and close it slowly, and softly.",
                ProductQuantity = 15,
                ProductPictureURL = "/Pictures/Tyssedal.jpg",
                CategoryId = 3,
            });

            context.Products.Add(new Product()
            {
                ProductName = "Brimnes",
                ProductPrice = 140,
                ProductDetails = "The mirror door can be placed on the left side, right side or in the middle.",
                ProductQuantity = 7,
                ProductPictureURL = "/Pictures/Brimnes.jpg",
                CategoryId = 3,
            });

            context.Products.Add(new Product()
            {
                ProductName = "Dombas",
                ProductPrice = 59,
                ProductDetails = "Adjustable shelves make it easy to customise space according to needs.",
                ProductQuantity = 9,
                ProductPictureURL = "/Pictures/Dombas.jpg",
                CategoryId = 3,
            });

            context.Products.Add(new Product()
            {
                ProductName = "Pax",
                ProductPrice = 308,
                ProductDetails = "10 year guarantee. Read about the terms in the guarantee brochure.",
                ProductQuantity = 5,
                ProductPictureURL = "/Pictures/Pax.jpg",
                CategoryId = 3,
            });

            context.Products.Add(new Product()
            {
                ProductName = "Stefan",
                ProductPrice = 18,
                ProductDetails = "Solid wood is a hardwearing natural material",
                ProductQuantity = 7,
                ProductPictureURL = "/Pictures/Stefan.jpg",
                CategoryId = 2,
            });

            context.Products.Add(new Product()
            {
                ProductName = "Markus",
                ProductPrice = 130,
                ProductDetails = "10 year guarantee. Read about the terms in the guarantee brochure.",
                ProductQuantity = 14,
                ProductPictureURL = "/Pictures/Markus.jpg",
                CategoryId = 2,
            });

            context.Products.Add(new Product()
            {
                ProductName = "Volmar",
                ProductPrice = 175,
                ProductDetails = "Seat and backrest adjustable in height.",
                ProductQuantity = 11,
                ProductPictureURL = "/Pictures/Volmar.jpg",
                CategoryId = 2,
            });

            context.Products.Add(new Product()
            {
                ProductName = "Flintan",
                ProductPrice = 49,
                ProductDetails = "You can lean back with perfect balance.",
                ProductQuantity = 3,
                ProductPictureURL = "/Pictures/Flintan.jpg",
                CategoryId = 2,
            });

            context.Products.Add(new Product()
            {
                ProductName = "Malm",
                ProductPrice = 35,
                ProductDetails = "A chest of drawers.",
                ProductQuantity = 7,
                ProductPictureURL = "/Pictures/Malm.jpg",
                CategoryId = 4,
            });

            context.Products.Add(new Product()
            {
                ProductName = "Hemnes",
                ProductPrice = 90,
                ProductDetails = "Home should be a safe place. Hence a safety fitting is included.",
                ProductQuantity = 5,
                ProductPictureURL = "/Pictures/HemnesChest.jpg",
                CategoryId = 4,
            });

            context.Products.Add(new Product()
            {
                ProductName = "Kullen",
                ProductPrice = 20,
                ProductDetails = "Oak chest of drawers.",
                ProductQuantity = 12,
                ProductPictureURL = "/Pictures/Kullen.jpg",
                CategoryId = 4,
            });

        }

        /// <summary>
        /// Creates the categories.
        /// </summary>
        /// <param name="context">The context.</param>
        private void CreateCategories(ApplicationDbContext context)
        {
            context.Categories.Add(new Category()
            {
                CategoryId = 1,
                CategoryName = "Table"
            });

            context.Categories.Add(new Category()
            {
                CategoryId = 2,
                CategoryName = "Chair"
            });

            context.Categories.Add(new Category()
            {
                CategoryId = 3,
                CategoryName = "Wardrobe"
            });

            context.Categories.Add(new Category()
            {
                CategoryId = 4,
                CategoryName = "Chest of Drawers"
            });
        }
    }
}