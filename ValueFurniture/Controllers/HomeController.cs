using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using ValueFurniture.Models;

namespace ValueFurniture.Controllers
{
    /// <summary>
    /// Controller for the homepage 
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    [AllowAnonymous]
    public class HomeController : Controller
    {
        /// <summary>
        /// The database
        /// </summary>
        ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Displays the Home Page
        /// </summary>
        /// <param name="searchTerm">Data that the users searches for</param>
        /// <returns>
        /// List of Products
        /// </returns>
        public ActionResult Index(string searchTerm = null)
        {
            var model = db.Products
                .OrderBy(r => r.ProductName)
                .Where(r => searchTerm == null || r.ProductName.StartsWith(searchTerm) || r.Category.CategoryName.Equals(searchTerm))
                .Select(r => new ProductListViewModel
                {
                    ProductId = r.ProductId,
                    ProductName = r.ProductName,
                    ProductPrice = r.ProductPrice,
                    ProductDetails = r.ProductDetails,
                    ProductPictureURL = r.ProductPictureURL,
                    ProductQuantity = r.ProductQuantity,
                    CategoryID = r.CategoryId,
                    Category = r.Category,
                });

            if (Request.IsAjaxRequest())
            {
                return PartialView("_Products", model);
            }

            return View(model);
        }

        /// <summary>
        /// Shows Products that have low quantities
        /// </summary>
        /// <returns>
        /// Most Popular Products
        /// </returns>
        [ChildActionOnly]
        public ActionResult PopularProducts()
        {
            var model = db.Products.OrderBy(r => r.ProductQuantity).Where(r => r.ProductQuantity > 0).Take(3).Select(r => new ProductListViewModel
            {
                ProductId = r.ProductId,
                ProductName = r.ProductName,
                ProductPrice = r.ProductPrice,
                ProductDetails = r.ProductDetails,
                ProductPictureURL = r.ProductPictureURL,
                ProductQuantity = r.ProductQuantity,
                CategoryID = r.CategoryId,
                Category = r.Category,
            });

            return PartialView(model);
        }

        /// <summary>
        /// Browse Products with an certain Category
        /// </summary>
        /// <param name="category">Category that Products must have</param>
        /// <returns>
        /// Products
        /// </returns>
        public ActionResult Browse(string category)
        {
            var categories = db.Categories.Include("Products").Single(r => r.CategoryName == category);
            var Products = db.Categories.Include("Products").Single(r => r.CategoryName == category).Products.Select(r => new ProductListViewModel
            {
                ProductId = r.ProductId,
                ProductName = r.ProductName,
                ProductPrice = r.ProductPrice,
                ProductDetails = r.ProductDetails,
                ProductPictureURL = r.ProductPictureURL,
                ProductQuantity = r.ProductQuantity,
                CategoryID = r.CategoryId,
                Category = r.Category,
            });

            return View(new ProductCategoryViewModel()
            {
                Category = categories,
                Products = Products,
            });
        }

        /// <summary>
        /// Hides the unavailable products.
        /// </summary>
        /// <returns></returns>
        public ActionResult HideUnavailableProducts()
        {
            var products = db.Products.Where(r => r.ProductQuantity > 0).Select(r => new ProductListViewModel
            {
                ProductId = r.ProductId,
                ProductName = r.ProductName,
                ProductPrice = r.ProductPrice,
                ProductDetails = r.ProductDetails,
                ProductPictureURL = r.ProductPictureURL,
                ProductQuantity = r.ProductQuantity,
                CategoryID = r.CategoryId,
                Category = r.Category,
            });

            return View(products);
        }

        /// <summary>
        /// Menu showcasing categories
        /// </summary>
        /// <returns>
        /// Categories Menu
        /// </returns>
        [ChildActionOnly]
        public ActionResult CategoryMenu()
        {
            var categories = db.Categories.ToList();
            return PartialView(categories);
        }
    }
}