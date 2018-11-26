namespace ValueFurniture.Models
{
    /// <summary>
    /// ViewModel for Removing Item in Shopping Cart
    /// </summary>
    public class ShoppingCartRemoveViewModel
    {
        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the cart total.
        /// </summary>
        /// <value>
        /// The cart total.
        /// </value>
        public decimal CartTotal { get; set; }

        /// <summary>
        /// Gets or sets the cart count.
        /// </summary>
        /// <value>
        /// The cart count.
        /// </value>
        public int CartCount { get; set; }

        /// <summary>
        /// Gets or sets the item count.
        /// </summary>
        /// <value>
        /// The item count.
        /// </value>
        public int ItemCount { get; set; }

        /// <summary>
        /// Gets or sets the delete identifier.
        /// </summary>
        /// <value>
        /// The delete identifier.
        /// </value>
        public int DeleteId { get; set; }
    }
}