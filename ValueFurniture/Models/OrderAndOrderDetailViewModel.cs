using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ValueFurniture.POCO_Classes;

namespace ValueFurniture.Models
{
    /// <summary>
    /// ViewModel for the displaying order details as well as the products bought.
    /// </summary>
    public class OrderAndOrderDetailViewModel
    {
        /// <summary>
        /// Gets current order.
        /// </summary>
        /// <value>
        /// The current order.
        /// </value>
        public Order CurrentOrder { get; set; }

        /// <summary>
        /// Gets the order collection.
        /// </summary>
        /// <value>
        /// The order collection.
        /// </value>
        public IEnumerable<OrderDetailsViewModel> OrderCollection { get; set; }
    }
}