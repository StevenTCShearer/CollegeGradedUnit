using ClosedXML.Excel;
using Rotativa;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using ValueFurniture.Extensions;
using ValueFurniture.Models;
using ValueFurniture.POCO_Classes;

namespace ValueFurniture.Controllers
{
    /// <summary>
    /// Controller for Products 
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    public class ProductsController : Controller
    {
        /// <summary>
        /// The database
        /// </summary>
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Shows Products
        /// </summary>
        /// <param name="name">The search name.</param>
        /// <returns>
        /// Products from Database
        /// </returns>
        public ActionResult Index(string name = null)
        {
            if (name == null)
            {
                var products = db.Products.Include(p => p.Category);
                return View(products.ToList());
            }
            else
            {
                var products = db.Products.Include(p => p.Category)
                .Where(r => r.ProductName.StartsWith(name) || r.ProductName.Contains(name)).ToList();

                return View(products);
            }
        }

        /// <summary>
        /// Product Details
        /// </summary>
        /// <param name="id">ID of Product</param>
        /// <returns>
        /// Page with Product details
        /// </returns>
        [AllowAnonymous]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var model = db.Products.Where(r => r.ProductId == id).Select(r => new ProductListViewModel
            {
                ProductId = r.ProductId,
                ProductName = r.ProductName,
                ProductPrice = r.ProductPrice,
                ProductDetails = r.ProductDetails,
                ProductPictureURL = r.ProductPictureURL,
                CategoryID = r.CategoryId,
                Category = r.Category,
                ProductQuantity = r.ProductQuantity,
            });
            if (model == null)
            {
                return HttpNotFound();
            }

            return View(model);
        }

        /// <summary>
        /// Create New Product
        /// </summary>
        /// <returns>
        /// View for Creating a Product
        /// </returns>
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName");
            return View();
        }

        /// <summary>
        /// Adds new Product to Database
        /// </summary>
        /// <param name="product">New Product to be added to Databse</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductId,ProductName,ProductPrice,ProductDetails,ProductPictureURL,ProductQuantity,CategoryId")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges();
                this.AddNotification("Product " + product.ProductId + " has been successfully created", NotificationType.INFO);
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName", product.CategoryId);
            return View(product);
        }

        /// <summary>
        /// Edit Product Information
        /// </summary>
        /// <param name="id">ID of Product to be Edited</param>
        /// <returns>
        /// Post Edit
        /// </returns>
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName", product.CategoryId);
            return View(product);
        }

        /// <summary>
        /// Saves Updated Product to Database
        /// </summary>
        /// <param name="product">Updated Product Saved to Database</param>
        /// <returns>
        /// Product
        /// </returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductId,ProductName,ProductPrice,ProductDetails,ProductPictureURL,ProductQuantity,CategoryId")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                this.AddNotification("Product " + product.ProductId + " has been successfully edited", NotificationType.INFO);

                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName", product.CategoryId);
            return View(product);
        }

        /// <summary>
        /// Delete Product
        /// </summary>
        /// <param name="id">ID of Product to be Deleted</param>
        /// <returns>
        /// Post Delete
        /// </returns>
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        /// <summary>
        /// Removes Product from Database
        /// </summary>
        /// <param name="id">ID of Product to be Removed</param>
        /// <returns>
        /// Product Index Page
        /// </returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            this.AddNotification("Product " + product.ProductId + " has been successfully deleted", NotificationType.INFO);
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Gets All Products from Database
        /// </summary>
        /// <returns>
        /// List of Products
        /// </returns>
        public IList<ProductListViewModel> GetAllProducts()
        {
            var productList = db.Products.Select(r => new ProductListViewModel
            {
                ProductId = r.ProductId,
                ProductName = r.ProductName,
                ProductDetails = r.ProductDetails,
                ProductPictureURL = r.ProductPictureURL,
                ProductPrice = r.ProductPrice,
                ProductQuantity = r.ProductQuantity,
                Category = r.Category,
                CategoryID = r.CategoryId
            }).ToList();

            return productList;
        }

        /// <summary>
        /// Show All Products (for PDF)
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult ShowProducts()
        {
            return View(GetAllProducts());
        }

        /// <summary>
        /// Gathers all orders and then exports it to xlsx file format for use with Excel
        /// </summary>
        public void ExportToExcel()
        {
            //Creating a workbook
            var wb = new XLWorkbook();

            //Adding a worksheet
            var ws = wb.Worksheets.Add("Products");

            //Headers
            ws.Cell("A1").Value = "Products";
            ws.Cell("A2").Value = "Product Id";
            ws.Cell("B2").Value = "Product Name";
            ws.Cell("C2").Value = "Details";
            ws.Cell("D2").Value = "Price";
            ws.Cell("E2").Value = "Quantity";

            int i = 3;

            //Populating data from database to excel
            foreach (Product p in db.Products.ToList())
            {
                ws.Cell("A" + i).Value = p.ProductId;
                ws.Cell("B" + i).Value = p.ProductName;
                ws.Cell("C" + i).Value = p.ProductDetails;
                ws.Cell("D" + i).Value = p.ProductPrice;
                ws.Cell("E" + i).Value = p.ProductQuantity;

                i++;
            }

            //Defining ranges
            var rngTable = ws.Range("A1:E" + (i - 1));

            //Formatting headers
            var rngHeaders = rngTable.Range("A2:E2");
            rngHeaders.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            rngHeaders.Style.Font.Bold = true;
            rngHeaders.Style.Fill.BackgroundColor = XLColor.Aqua;

            //Adding grid lines
            rngTable.Style.Border.BottomBorder = XLBorderStyleValues.Thin;

            //Format title cell
            rngTable.Cell(1, 1).Style.Font.Bold = true;
            rngTable.Cell(1, 1).Style.Fill.BackgroundColor = XLColor.BabyBlue;
            rngTable.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            //Merge title cells
            rngTable.Row(1).Merge();

            //Add a thick outside border
            rngTable.Style.Border.OutsideBorder = XLBorderStyleValues.Thick;

            //Adjust column widths to their content
            ws.Columns(1, 6).AdjustToContents();

            //Saving the workbook
            HttpResponseBase httpResponse = Response;
            httpResponse.Clear();
            httpResponse.ContentType = "application/vnd.openxmlformat-officedocument.spreadsheet.sheet";
            httpResponse.AddHeader("content-disposition", "attachment;filename=ProductReport" + DateTime.Now + ".xlsx");

            using (MemoryStream memoryStream = new MemoryStream())
            {
                wb.SaveAs(memoryStream);
                memoryStream.WriteTo(httpResponse.OutputStream);
                memoryStream.Close();
            }

            httpResponse.End();
        }

        /// <summary>
        /// Exports Products List to a PDF
        /// </summary>
        /// <returns>
        /// PDF of Products
        /// </returns>
        public ActionResult ExportToPDF()
        {
            return new ActionAsPdf("ShowProducts")
            {
                FileName = "ProductReport-" + DateTime.Now + ".pdf",
                CustomSwitches = "--print-media-type --header-center \"Value Furniture — Product Report\""
            };
        }

        /// <summary>
        /// Releases unmanaged resources and optionally releases managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
