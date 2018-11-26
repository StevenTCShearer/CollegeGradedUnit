using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ValueFurniture.Models
{
    /// <summary>
    /// ViewModel for the MyOrders Page.
    /// </summary>
    public class MyOrderViewList
    {
        /// <summary>
        /// Gets my orders.
        /// </summary>
        /// <value>
        /// My orders.
        /// </value>
        public IEnumerable<OrderViewModel> MyOrders { get; set; }

    }
}