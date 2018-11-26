using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using ValueFurniture.POCO_Classes;

namespace ValueFurniture.Models
{
    public class ApplicationUser : IdentityUser
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationUser"/> class.
        /// </summary>
        public ApplicationUser()
        {
            Orders = new List<Order>();
        }


        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        /// <value>
        /// The first name.
        /// </value>
        [Required(ErrorMessage = "First Name is required")]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the surname.
        /// </summary>
        /// <value>
        /// The surname.
        /// </value>
        [Required(ErrorMessage = "Second Name is required")]
        public string Surname { get; set; }

        /// <summary>
        /// Gets or sets the name of the middle.
        /// </summary>
        /// <value>
        /// The name of the middle.
        /// </value>
        public string MiddleName { get; set; }
        
        /// <summary>
        /// Gets or sets the orders.
        /// </summary>
        /// <value>
        /// The orders.
        /// </value>
        public virtual ICollection<Order> Orders { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        /// <summary>
        /// Gets or sets the orders.
        /// </summary>
        /// <value>
        /// The orders.
        /// </value>
        public IDbSet<Order> Orders { get; set; }

        /// <summary>
        /// Gets or sets the products.
        /// </summary>
        /// <value>
        /// The products.
        /// </value>
        public IDbSet<Product> Products { get; set; }

        /// <summary>
        /// Gets or sets the carts.
        /// </summary>
        /// <value>
        /// The carts.
        /// </value>
        public IDbSet<Cart> Carts { get; set; }

        /// <summary>
        /// Gets or sets the order details.
        /// </summary>
        /// <value>
        /// The order details.
        /// </value>
        public IDbSet<OrderDetail> OrderDetails { get; set; }

        /// <summary>
        /// Gets or sets the categories.
        /// </summary>
        /// <value>
        /// The categories.
        /// </value>
        public IDbSet<Category> Categories { get; set; }

        /// <summary>
        /// Gets or sets the shopping cart view models.
        /// </summary>
        /// <value>
        /// The shopping cart view models.
        /// </value>
        public IDbSet<ShoppingCartViewModel> ShoppingCartViewModels { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationDbContext"/> class.
        /// </summary>
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        /// <summary>
        /// Creates this instance.
        /// </summary>
        /// <returns></returns>
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}