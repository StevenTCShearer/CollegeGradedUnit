using System.Linq;
using System.Web.Mvc;
using ValueFurniture.Extensions;
using ValueFurniture.Models;
using ValueFurniture.POCO_Classes;

namespace ValueFurniture.Controllers
{
    /// <summary>
    /// Controller for ShoppingCart 
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    [AllowAnonymous]
    public class ShoppingCartController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Show Products in Cart
        /// </summary>
        /// <returns>
        /// Items in Cart
        /// </returns>
        public ActionResult Index()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);

            var viewModel = new ShoppingCartViewModel
            {
                CartItems = cart.GetCartItems(),
                CartTotal = cart.GetTotal()
            };

            return View(viewModel);
        }

        /// <summary>
        /// Add Product to Cart
        /// </summary>
        /// <param name="id">ID of Product to be added</param>
        /// <returns>
        /// Home Page
        /// </returns>
        public ActionResult AddToCart(int id)
        {
            // Retrieve the product from the database
            var addedProduct = db.Products
                .Single(product => product.ProductId == id);

            // Add it to the shopping cart
            var cart = ShoppingCart.GetCart(this.HttpContext);

            if (addedProduct.ProductQuantity > 0)
            {
                cart.AddToCart(addedProduct);
                addedProduct.ProductQuantity--;
            }
            else
            {
                ViewBag.errorMessage = "Product out of stock — cannot be added to basket";
                return View("Error");
            }

            db.SaveChanges();

            // Go back to the main store page for more shopping
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Remove Product form Cart
        /// </summary>
        /// <param name="id">ID of Product to be Removed</param>
        /// <returns>
        /// Updated Cart
        /// </returns>
        [HttpPost]
        public ActionResult RemoveFromCart(int id)
        {
            // Remove the item from the cart
            var cart = ShoppingCart.GetCart(this.HttpContext);

            // Get the name of the product to display confirmation
            string productName = db.Carts
                .Single(item => item.ProductId == id).Product.ProductName;

            // Remove from cart
            int itemCount = cart.RemoveFromCart(id);

            Product product = db.Products.Find(id);
            product.ProductQuantity++;
            db.SaveChanges();
            this.AddNotification("Product " + product.ProductName + " has been successfully removed from cart", NotificationType.INFO);

            // Display the confirmation message
            var results = new ShoppingCartRemoveViewModel
            {
                Message = Server.HtmlEncode(productName) +
                    " has been removed from your shopping cart.",
                CartTotal = cart.GetTotal(),
                CartCount = cart.GetCount(),
                ItemCount = itemCount,
                DeleteId = id
            };
            return Json(results);
        }

        /// <summary>
        /// Shows how many items are in cart
        /// </summary>
        /// <returns>
        /// Number of Items in Cart
        /// </returns>
        [ChildActionOnly]
        public ActionResult CartSummary()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);

            ViewData["CartCount"] = cart.GetCount();
            return PartialView("CartSummary");
        }
    }
}