using Microsoft.Ajax.Utilities;
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
    /// ViewModel for the OrderDetails
    /// </summary>
    public class OrderDetailsViewModel
    {
        /// <summary>
        /// Gets or sets the product identifier.
        /// </summary>
        /// <value>
        /// The product identifier.
        /// </value>
        [Display(Name = "Product Id")]
        public int ProductId { get; set; }

        /// <summary>
        /// Gets or sets the order identifier.
        /// </summary>
        /// <value>
        /// The order identifier.
        /// </value>
        [Display(Name = "Order Id")]
        public int OrderId { get; set; }

        /// <summary>
        /// Gets or sets the quantity.
        /// </summary>
        /// <value>
        /// The quantity.
        /// </value>
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets the cost.
        /// </summary>
        /// <value>
        /// The cost.
        /// </value>
        public decimal Cost { get; set; }

        /// <summary>
        /// Gets or sets the name of the product.
        /// </summary>
        /// <value>
        /// The name of the product.
        /// </value>
        [Display(Name = "Product Name")]
        public string ProductName { get; set; }

        /// <summary>
        /// Gets or sets the customer identifier.
        /// </summary>
        /// <value>
        /// The customer identifier.
        /// </value>
        [Display(Name = "Customer Id")]
        public string CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the product.
        /// </summary>
        /// <value>
        /// The product.
        /// </value>
        public Product Product { get; set; }

        /// <summary>
        /// Gets the view model.
        /// </summary>
        /// <value>
        /// The view model.
        /// </value>
        public static Expression<Func<OrderDetail, OrderDetailsViewModel>> ViewModel
        {
            get
            {
                return r => new OrderDetailsViewModel()
                {
                    ProductId = r.ProductId,
                    OrderId = r.OrderId,
                    Quantity = r.Quantity,
                    ProductName = r.ProductName,
                    CustomerId = r.CustomerId,
                    Cost = r.Cost,
                    Product = r.Product,
                };
            }
        }
    }
}