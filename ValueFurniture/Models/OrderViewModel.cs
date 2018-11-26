using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using ValueFurniture.POCO_Classes;

namespace ValueFurniture.Models
{
    /// <summary>
    /// ViewModel for the Order
    /// </summary>
    public class OrderViewModel
    {
        /// <summary>
        /// Gets or sets the order identifier.
        /// </summary>
        /// <value>
        /// The order identifier.
        /// </value>
        [Display(Name = "Order Id")]
        public int OrderId { get; set; }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        /// <value>
        /// The first name.
        /// </value>
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        /// <value>
        /// The last name.
        /// </value>
        [Display(Name = "Second Name")]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the line1.
        /// </summary>
        /// <value>
        /// The line1.
        /// </value>
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
        public string City { get; set; }

        /// <summary>
        /// Gets or sets the postal code.
        /// </summary>
        /// <value>
        /// The postal code.
        /// </value>
        [Display(Name = "Post Code")]
        public string PostalCode { get; set; }

        /// <summary>
        /// Gets or sets the country.
        /// </summary>
        /// <value>
        /// The country.
        /// </value>
        public string Country { get; set; }

        /// <summary>
        /// Gets or sets the phone.
        /// </summary>
        /// <value>
        /// The phone.
        /// </value>
        public string Phone { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>
        /// The email.
        /// </value>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the order date.
        /// </summary>
        /// <value>
        /// The order date.
        /// </value>
        public DateTime OrderDate { get; set; }

        /// <summary>
        /// Gets or sets the order total.
        /// </summary>
        /// <value>
        /// The order total.
        /// </value>
        public decimal OrderTotal { get; set; }

        /// <summary>
        /// Gets or sets the transaction identifier.
        /// </summary>
        /// <value>
        /// The transaction identifier.
        /// </value>
        public string TransactionID { get; set; }

        /// <summary>
        /// Gets or sets the customer identifier.
        /// </summary>
        /// <value>
        /// The customer identifier.
        /// </value>
        public string CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the products.
        /// </summary>
        /// <value>
        /// The products.
        /// </value>
        public virtual ICollection<Product> Products { get; set; }

        /// <summary>
        /// Gets the view model.
        /// </summary>
        /// <value>
        /// The view model.
        /// </value>
        public static Expression<Func<Order, OrderViewModel>> ViewModel
        {
            get
            {
                return r => new OrderViewModel()
                {
                    OrderId = r.OrderId,
                    FirstName = r.FirstName,
                    LastName = r.LastName,
                    Line1 = r.Line1,
                    Line2 = r.Line2,
                    City = r.City,
                    PostalCode = r.PostalCode,
                    Country = r.Country,
                    Phone = r.Phone,
                    Email = r.Email,
                    OrderDate = r.OrderDate,
                    OrderTotal = r.OrderTotal,
                    CustomerId = r.CustomerId,
                    Products = r.Products,
                    TransactionID = r.TransactionID,
                };
            }
        }
    }
}