using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ValueFurniture.Models;
using Microsoft.AspNet.Identity;


namespace ValueFurniture.POCO_Classes
{
    /// <summary>
    /// Class for ShoppingCart.
    /// </summary>
    public partial class ShoppingCart
    {
        /// <summary>
        /// The database
        /// </summary>
        ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Gets or sets the shopping cart identifier.
        /// </summary>
        /// <value>
        /// The shopping cart identifier.
        /// </value>
        string ShoppingCartId { get; set; }

        /// <summary>
        /// The cart session key
        /// </summary>
        public const string CartSessionKey = "CartId";

        /// <summary>
        /// Gets the cart.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public static ShoppingCart GetCart(HttpContextBase context)
        {
            var cart = new ShoppingCart();
            cart.ShoppingCartId = cart.GetCartId(context);
            return cart;
        }

        /// <summary>
        /// Helper Method to simply shopping cart calls
        /// </summary>
        /// <param name="controller">The controller.</param>
        /// <returns></returns>
        public static ShoppingCart GetCart(Controller controller)
        {
            return GetCart(controller.HttpContext);
        }

        /// <summary>
        /// Adds Item to Cart
        /// </summary>
        /// <param name="product">Product getting added to cart</param>
        public void AddToCart(Product product)
        {
            //Getting matching cart and product instances
            var cartItem = db.Carts.SingleOrDefault(c => c.CartId == ShoppingCartId
            && c.ProductId == product.ProductId);

            if (cartItem == null)
            {

                //Creating new carts if not cart item exists
                cartItem = new Cart
                {
                    ProductId = product.ProductId,
                    CartId = ShoppingCartId,
                    Count = 1,
                    DateCreated = DateTime.Now
                };
                db.Carts.Add(cartItem);
            }
            else
            {
                //If items exists in cart then increment count
                cartItem.Count++;
            }

            db.SaveChanges();

        }

        /// <summary>
        /// Removes Item from Cart
        /// </summary>
        /// <param name="id">ID of Product to be Removed</param>
        /// <returns></returns>
        public int RemoveFromCart(int id)
        {
            var cartItem = db.Carts.Single(cart => cart.CartId == ShoppingCartId
            && cart.ProductId == id);

            int itemCount = 0;

            if (cartItem != null)
            {
                if (cartItem.Count > 1)
                {
                    cartItem.Count--;
                    itemCount = cartItem.Count;
                }
                else
                {
                    db.Carts.Remove(cartItem);
                }
                db.SaveChanges();
            }
            return itemCount;
        }

        /// <summary>
        /// Empties the Cart
        /// </summary>
        public void EmptyCart()
        {
            var cartItems = db.Carts.Where(cart => cart.CartId == ShoppingCartId);

            foreach (var cartItem in cartItems)
            {
                db.Carts.Remove(cartItem);
            }
            db.SaveChanges();
        }

        /// <summary>
        /// Gets Items In Cart
        /// </summary>
        /// <returns>
        /// Items in Cart
        /// </returns>
        public List<Cart> GetCartItems()
        {
            return db.Carts.Where(cart => cart.CartId == ShoppingCartId).ToList();
        }

        /// <summary>
        /// Gets the number of items in cart
        /// </summary>
        /// <returns>
        /// Number of items in cart
        /// </returns>
        public int GetCount()
        {
            int? count = (from cartItems in db.Carts
                          where cartItems.CartId == ShoppingCartId
                          select (int?)cartItems.Count).Sum();
            return count ?? 0;
        }

        /// <summary>
        /// Getting Total Price of Items
        /// </summary>
        /// <returns>
        /// Total Cost of Items
        /// </returns>
        public decimal GetTotal()
        {
            //Multiply Product Price with count of Products then sum it up for Total Price
            decimal? total = (from cartItems in db.Carts
                              where cartItems.CartId == ShoppingCartId
                              select (int?)cartItems.Count *
                              cartItems.Product.ProductPrice).Sum();

            return total ?? decimal.Zero;
        }

        /// <summary>
        /// Creating an Order
        /// </summary>
        /// <param name="order">The order.</param>
        /// <returns>
        /// OrderId number for confirmation
        /// </returns>
        public int CreateOrder(Order order)
        {
            decimal orderTotal = 0;
            var cartItems = GetCartItems();

            foreach (var item in cartItems)
            {
                var orderDetail = new OrderDetail
                {
                    ProductId = item.ProductId,
                    OrderId = order.OrderId,
                    Quantity = item.Count,
                    ProductName = item.Product.ProductName,
                    CustomerId = order.CustomerId,
                    Cost = item.Product.ProductPrice,
                    Product = item.Product
                };

                //Set order total as the total of shopping cart
                orderTotal += (item.Count * item.Product.ProductPrice);

                db.OrderDetails.Add(orderDetail);
            }

            //Setting order's total to the orderTotal count
            order.OrderTotal = orderTotal;

            //Saving Changes to DB and Clearing the Cart           
            db.SaveChanges();
            EmptyCart();
            return order.OrderId;
        }

        /// <summary>
        /// Getting CartId
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>
        /// CartId
        /// </returns>
        public string GetCartId(HttpContextBase context)
        {
            if (context.Session[CartSessionKey] == null)
            {
                if (!string.IsNullOrWhiteSpace(context.User.Identity.Name))
                {
                    context.Session[CartSessionKey] = context.User.Identity.Name;
                }
                else
                {
                    //Generate random GUID Using System.GUID class
                    Guid tempCartId = Guid.NewGuid();
                    //Sned tempCartId to client as a cookie
                    context.Session[CartSessionKey] = tempCartId.ToString();
                }
            }
            return context.Session[CartSessionKey].ToString();
        }

        /// <summary>
        /// When a user logs in the shopping cart is associated to their username
        /// </summary>
        /// <param name="userName">Username of user</param>
        public void MigrateCart(string userName)
        {
            var shoppingCart = db.Carts.Where(c => c.CartId == ShoppingCartId);

            foreach (Cart item in shoppingCart)
            {
                item.CartId = userName;
            }
            db.SaveChanges();
        }
    }
}