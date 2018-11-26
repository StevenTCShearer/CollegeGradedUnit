using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ValueFurniture.POCO_Classes;

namespace ValueFurniture.Models
{
    /// <summary>
    /// ViewModel for viewing ShoppingCart
    /// </summary>
    public class ShoppingCartViewModel
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the cart items.
        /// </summary>
        /// <value>
        /// The cart items.
        /// </value>
        public List<Cart> CartItems { get; set; }

        /// <summary>
        /// Gets or sets the cart total.
        /// </summary>
        /// <value>
        /// The cart total.
        /// </value>
        public decimal CartTotal { get; set; }
    }
}