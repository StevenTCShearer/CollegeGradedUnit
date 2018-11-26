using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ValueFurniture.Models;

namespace ValueFurniture.POCO_Classes
{
    /// <summary>
    /// Class for Order.
    /// </summary>
    public class Order
    {
        /// <summary>
        /// Gets or sets the order identifier.
        /// </summary>
        /// <value>
        /// The order identifier.
        /// </value>
        [ScaffoldColumn(false)]
        [Key]
        [Display(Name = "Order Id")]
        public int OrderId { get; set; }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        /// <value>
        /// The first name.
        /// </value>
        [Required(ErrorMessage = "First Name is required")]
        [DisplayName("First Name")]
        [StringLength(160)]
        [ScaffoldColumn(false)]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        /// <value>
        /// The last name.
        /// </value>
        [Required(ErrorMessage = "Last Name is required")]
        [DisplayName("Last Name")]
        [StringLength(160)]
        [ScaffoldColumn(false)]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the line1.
        /// </summary>
        /// <value>
        /// The line1.
        /// </value>
        [Required(ErrorMessage = "Line1 is required")]
        [StringLength(70)]
        public string Line1 { get; set; }

        /// <summary>
        /// Gets or sets the line2.
        /// </summary>
        /// <value>
        /// The line2.
        /// </value>
        public string Line2 { get; set; }

        /// <summary>
        /// Gets or sets the city.
        /// </summary>
        /// <value>
        /// The city.
        /// </value>
        [Required(ErrorMessage = "City is required")]
        [StringLength(40)]
        public string City { get; set; }

        /// <summary>
        /// Gets or sets the postal code.
        /// </summary>
        /// <value>
        /// The postal code.
        /// </value>
        [Required(ErrorMessage = "Postal Code is required")]
        [DisplayName("Postal Code")]
        [StringLength(10)]
        public string PostalCode { get; set; }

        /// <summary>
        /// Gets or sets the country.
        /// </summary>
        /// <value>
        /// The country.
        /// </value>
        [Required(ErrorMessage = "Country is required")]
        [StringLength(40)]
        public string Country { get; set; }

        /// <summary>
        /// Gets or sets the phone.
        /// </summary>
        /// <value>
        /// The phone.
        /// </value>
        [ScaffoldColumn(false)]
        [Required(ErrorMessage = "Phone is required")]
        [StringLength(11)]
        public string Phone { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>
        /// The email.
        /// </value>
        [ScaffoldColumn(false)]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the order date.
        /// </summary>
        /// <value>
        /// The order date.
        /// </value>
        [ScaffoldColumn(false)]
        [Column(TypeName = "datetime2")]
        [Display(Name = "Date of Order")]
        public DateTime OrderDate { get; set; }

        /// <summary>
        /// Gets or sets the order total.
        /// </summary>
        /// <value>
        /// The order total.
        /// </value>
        [ScaffoldColumn(false)]
        public decimal OrderTotal { get; set; }

        /// <summary>
        /// Gets or sets the customer identifier.
        /// </summary>
        /// <value>
        /// The customer identifier.
        /// </value>
        [ScaffoldColumn(false)]
        public string CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the transaction identifier.
        /// </summary>
        /// <value>
        /// The transaction identifier.
        /// </value>
        [ScaffoldColumn(false)]
        public string TransactionID { get; set; }

        /// <summary>
        /// Gets or sets the customer.
        /// </summary>
        /// <value>
        /// The customer.
        /// </value>
        [ScaffoldColumn(false)]
        public virtual ApplicationUser Customer { get; set; }

        /// <summary>
        /// Gets or sets the products.
        /// </summary>
        /// <value>
        /// The products.
        /// </value>
        public virtual ICollection<Product> Products { get; set; }
    }
}