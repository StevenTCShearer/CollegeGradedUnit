using ValueFurniture.POCO_Classes;

namespace ValueFurniture.Models
{
    /// <summary>
    /// ViewModel for listing all Products.
    /// </summary>
    public class ProductListViewModel
    {
        /// <summary>
        /// Gets or sets the product identifier.
        /// </summary>
        /// <value>
        /// The product identifier.
        /// </value>
        public int ProductId { get; set; }

        /// <summary>
        /// Gets or sets the name of the product.
        /// </summary>
        /// <value>
        /// The name of the product.
        /// </value>
        public string ProductName { get; set; }

        /// <summary>
        /// Gets or sets the product price.
        /// </summary>
        /// <value>
        /// The product price.
        /// </value>
        public decimal ProductPrice { get; set; }

        /// <summary>
        /// Gets or sets the product details.
        /// </summary>
        /// <value>
        /// The product details.
        /// </value>
        public string ProductDetails { get; set; }

        /// <summary>
        /// Gets or sets the product picture URL.
        /// </summary>
        /// <value>
        /// The product picture URL.
        /// </value>
        public string ProductPictureURL { get; set; }

        /// <summary>
        /// Gets or sets the product quantity.
        /// </summary>
        /// <value>
        /// The product quantity.
        /// </value>
        public int ProductQuantity { get; set; }

        /// <summary>
        /// Gets or sets the category identifier.
        /// </summary>
        /// <value>
        /// The category identifier.
        /// </value>
        public int CategoryID { get; set; }

        /// <summary>
        /// Gets or sets the category.
        /// </summary>
        /// <value>
        /// The category.
        /// </value>
        public Category Category { get; set; }

    }
}