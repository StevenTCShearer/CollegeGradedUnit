using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ValueFurniture.POCO_Classes
{
    /// <summary>
    /// Class for Product.
    /// </summary>
    public class Product
    {

        /// <summary>
        /// Gets or sets the product identifier.
        /// </summary>
        /// <value>
        /// The product identifier.
        /// </value>
        [Key]
        [Display(Name = "Product Id")]
        public int ProductId { get; set; }

        /// <summary>
        /// Gets or sets the name of the product.
        /// </summary>
        /// <value>
        /// The name of the product.
        /// </value>
        [Display(Name = "Product Name")]
        [Required]
        public string ProductName { get; set; }

        /// <summary>
        /// Gets or sets the product price.
        /// </summary>
        /// <value>
        /// The product price.
        /// </value>
        [Display(Name = "Price")]
        [Required]
        public decimal ProductPrice { get; set; }

        /// <summary>
        /// Gets or sets the product details.
        /// </summary>
        /// <value>
        /// The product details.
        /// </value>
        [Display(Name = "Description")]
        public string ProductDetails { get; set; }

        /// <summary>
        /// Gets or sets the product picture URL.
        /// </summary>
        /// <value>
        /// The product picture URL.
        /// </value>
        [Display(Name = "Picture")]
        public string ProductPictureURL { get; set; }

        /// <summary>
        /// Gets or sets the product quantity.
        /// </summary>
        /// <value>
        /// The product quantity.
        /// </value>
        [Display(Name = "Quantity")]
        public int ProductQuantity { get; set; }

        /// <summary>
        /// Gets or sets the category identifier.
        /// </summary>
        /// <value>
        /// The category identifier.
        /// </value>
        public int CategoryId { get; set; }

        /// <summary>
        /// Gets or sets the category.
        /// </summary>
        /// <value>
        /// The category.
        /// </value>
        public virtual Category Category { get; set; }

        /// <summary>
        /// Gets or sets the orders.
        /// </summary>
        /// <value>
        /// The orders.
        /// </value>
        public virtual ICollection<Order> Orders { get; set; }
    }
}