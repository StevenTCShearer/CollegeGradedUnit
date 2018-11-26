using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ValueFurniture.POCO_Classes;

namespace ValueFurniture.Models
{
    /// <summary>
    /// ViewModel for the Cateogry of a Product and Products within that Category.
    /// </summary>
    public class ProductCategoryViewModel
    {
        /// <summary>
        /// Gets or sets the category.
        /// </summary>
        /// <value>
        /// The category.
        /// </value>
        public Category Category { get; set; }

        /// <summary>
        /// Gets or sets the products.
        /// </summary>
        /// <value>
        /// The products.
        /// </value>
        public IEnumerable<ProductListViewModel> Products { get; set; }
    }
}