using ClosedXML.Excel;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
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
    /// Controller for Orders
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    [Authorize(Roles = "Administrator")]
    public class OrderController : Controller
    {
        /// <summary>
        /// The database
        /// </summary>
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Index for Order
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Administrator")]
        public ActionResult Index(string user = null)
        {
            if (user == null)
            {
                return View(db.Orders.ToList());
            }
            else
            {
                var orders = db.Orders.OrderByDescending(r => r.OrderDate)
                .Where(r => r.Email.StartsWith(user) || r.FirstName.StartsWith(user) || r.LastName.StartsWith(user)).ToList();

                return View(orders);
            }
        }

        /// <summary>
        /// View Details of Order
        /// </summary>
        /// <param name="id">ID of Order to View</param>
        /// <returns></returns>
        [OverrideAuthorization]
        [Authorize(Roles = "User, Administrator")]
        public ActionResult Details(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }

            var orders = db.OrderDetails.Select(OrderDetailsViewModel.ViewModel);
            var orderCollection = orders.Where(r => r.OrderId == id);

            return View(new OrderAndOrderDetailViewModel()
            {
                OrderCollection = orderCollection,
                CurrentOrder = order
            });
        }

        /// <summary>
        /// Create an Order
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            ViewBag.CustomerId = new SelectList(db.Users, "Id", "FirstName");
            return View();
        }

        /// <summary>
        /// Add Order to Database
        /// </summary>
        /// <param name="order">Order that is to be added</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OrderId,FirstName,LastName,Line1,Line2,City,PostalCode,Country,Phone,Email,OrderDate,OrderTotal,CustomerId,TransactionID")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Orders.Add(order);
                db.SaveChanges();
                CheckoutController c = new CheckoutController();
                var manager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                c.SendOrderConfirmationEmail(order.CustomerId, "Order Confirmation: #" + order.OrderId, order, manager);

                return RedirectToAction("Index");
            }

            ViewBag.CustomerId = new SelectList(db.Users, "Id", "FirstName", order.CustomerId);
            return View(order);
        }

        /// <summary>
        /// Edit an Order
        /// </summary>
        /// <param name="id">Order Id</param>
        /// <returns></returns>
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        /// <summary>
        /// Saves Edited Order to Database
        /// </summary>
        /// <param name="order">Order that has been Edited</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OrderId,FirstName,LastName,Address,City,State,PostalCode,Country,Phone,Email,OrderDate,OrderTotal")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                this.AddNotification("Order " + order.OrderId + " has been successfully edited", NotificationType.INFO);
                return RedirectToAction("Index");
            }
            return View(order);
        }

        /// <summary>
        /// Delete an Order
        /// </summary>
        /// <param name="id">Id of Order</param>
        /// <returns></returns>
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        /// <summary>
        /// Removes an Order from Database
        /// </summary>
        /// <param name="id">Id of Order</param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = db.Orders.Find(id);
            db.Orders.Remove(order);
            db.SaveChanges();
            this.AddNotification("Order " + order.OrderId + " has been deleted", NotificationType.INFO);
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Shows Orders Placed by User
        /// </summary>
        /// <returns>Orders placed by the user</returns>
        [OverrideAuthorization]
        [Authorize(Roles = "User")]
        public ActionResult MyOrders()
        {
            string currentUser = User.Identity.GetUserId();
            var orders = db.Orders.Select(OrderViewModel.ViewModel);

            var orderCollection = orders.Where(r => r.CustomerId == currentUser)
                .OrderBy(r => r.OrderDate);

            return View(new MyOrderViewList()
            {
                MyOrders = orderCollection
            });
        }

        /// <summary>
        /// Gets All Orders from Databse
        /// </summary>
        /// <returns>All ORders from Database</returns>
        public IList<OrderViewModel> GetAllOrders()
        {
            var orderList = db.Orders.Select(r => new OrderViewModel
            {
                OrderId = r.OrderId,
                FirstName = r.FirstName,
                LastName = r.LastName,
                Line1 = r.Line1,
                Line2 = r.Line2,
                City = r.City,
                PostalCode = r.PostalCode,
                Country = r.Country,
                Phone = r.Phone,
                Email = r.Email,
                OrderDate = r.OrderDate,
                OrderTotal = r.OrderTotal,
                CustomerId = r.CustomerId,
                Products = r.Products,
                TransactionID = r.TransactionID,
            }).ToList();

            return orderList;
        }

        /// <summary>
        /// Lists all Orders for use of PDF
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult ShowOrders()
        {
            return View(GetAllOrders());
        }

        /// <summary>
        /// Gathers all orders and then exports it to xls file format for use with Excel
        /// </summary>
        public void ExportToExcel()
        {
            //Creating a workbook
            var wb = new XLWorkbook();

            //Adding a worksheet
            var ws = wb.Worksheets.Add("Orders");

            //Headers
            ws.Cell("A1").Value = "Orders";
            ws.Cell("A2").Value = "Order Id";
            ws.Cell("B2").Value = "First Name";
            ws.Cell("C2").Value = "Last Name";
            ws.Cell("D2").Value = "Line 1";
            ws.Cell("E2").Value = "Line 2";
            ws.Cell("F2").Value = "City";
            ws.Cell("G2").Value = "Post Code";
            ws.Cell("H2").Value = "Country";
            ws.Cell("I2").Value = "Phone Number";
            ws.Cell("J2").Value = "Email Address";
            ws.Cell("K2").Value = "Date of Order";
            ws.Cell("L2").Value = "Total";
            ws.Cell("M2").Value = "Customer ID";

            int i = 3;

            //Populating data from database to excel
            foreach (Order p in db.Orders.ToList())
            {
                ws.Cell("A" + i).Value = p.OrderId;
                ws.Cell("B" + i).Value = p.FirstName;
                ws.Cell("C" + i).Value = p.LastName;
                ws.Cell("D" + i).Value = p.Line1;
                ws.Cell("E" + i).Value = p.Line2;
                ws.Cell("F" + i).Value = p.City;
                ws.Cell("G" + i).Value = p.PostalCode;
                ws.Cell("H" + i).Value = p.Country;
                ws.Cell("I" + i).Value = p.Phone;
                ws.Cell("J" + i).Value = p.Email;
                ws.Cell("K" + i).Value = p.OrderDate;
                ws.Cell("L" + i).Value = p.OrderTotal;
                ws.Cell("M" + i).Value = p.CustomerId;
                i++;
            }

            //Defining ranges
            var rngTable = ws.Range("A1:M" + (i - 1));

            //Formatting headers
            var rngHeaders = rngTable.Range("A2:M2");
            rngHeaders.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            rngHeaders.Style.Font.Bold = true;
            rngHeaders.Style.Fill.BackgroundColor = XLColor.Aqua;

            //Adding grid lines
            rngTable.Style.Border.BottomBorder = XLBorderStyleValues.Thin;

            //Format title cell
            rngTable.Cell(1, 1).Style.Font.Bold = true;
            rngTable.Cell(1, 1).Style.Fill.BackgroundColor = XLColor.CornflowerBlue;
            rngTable.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            //Merge title cells
            rngTable.Row(1).Merge();

            //Add a thick outside border
            rngTable.Style.Border.OutsideBorder = XLBorderStyleValues.Thick;

            //Adjust column widths to their content
            ws.Columns(1, 13).AdjustToContents();

            //Saving the workbook
            HttpResponseBase httpResponse = Response;
            httpResponse.Clear();
            httpResponse.ContentType = "application/vnd.openxmlformat-officedocument.spreadsheet.sheet";
            httpResponse.AddHeader("content-disposition", "attachment;filename=OrdersReport" + DateTime.Now + ".xlsx");

            using (MemoryStream memoryStream = new MemoryStream())
            {
                wb.SaveAs(memoryStream);
                memoryStream.WriteTo(httpResponse.OutputStream);
                memoryStream.Close();
            }

            httpResponse.End();
            this.AddNotification("Excel Created.", NotificationType.INFO);
        }

        /// <summary>
        /// Export Orders to PDF
        /// </summary>
        /// <returns>PDF of Orders</returns>
        public ActionResult ExportToPDF()
        {
            return new ActionAsPdf("ShowOrders")
            {
                FileName = "OrdersReport-" + DateTime.Now + ".pdf",
                CustomSwitches = "--print-media-type --header-center \"Value Furniture — Orders Report\""
            };
        }

        /// <summary>
        /// Cancels an Order
        /// </summary>
        /// <param name="id">ID of Order to Cancel</param>
        /// <returns></returns>
        [OverrideAuthorization]
        [Authorize(Roles = "User, Administrator")]
        public ActionResult Cancel(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order orderToCancel = db.Orders.Find(id);
            if (orderToCancel == null)
            {
                return HttpNotFound();
            }
            else if (orderToCancel.OrderDate < DateTime.Now.AddDays(-2))
            {
                ViewBag.errorMessage = "Order has been dispatched — unable to cancel";
                return View("Error");
            }

            return View(orderToCancel);
        }

        /// <summary>
        /// Cancels Order and Sends Confrimation Email and SMS. Updates Quantity of Products.
        /// </summary>
        /// <param name="id">ID of Order to Cancel</param>
        /// <returns></returns>
        [OverrideAuthorization]
        [Authorize(Roles = "User, Administrator")]
        [HttpPost, ActionName("Cancel")]
        [ValidateAntiForgeryToken]
        public ActionResult CancelConfirmed(int id)
        {
            Order orderToCancel = db.Orders.Find(id);
            var userId = User.Identity.GetUserId();
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var currentUser = userManager.FindById(User.Identity.GetUserId());

            CheckoutController c = new CheckoutController();
            c.Cancel(orderToCancel.TransactionID, orderToCancel.OrderId, userId);

            var manager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

            c.SendOrderCancellationCofirmationEmail(orderToCancel.CustomerId, "Order Cancellation: #" + orderToCancel.OrderId, orderToCancel, manager);
            if (currentUser.PhoneNumber != null)
            {
                c.SendOrderCancellationConfirmationSMS(orderToCancel.CustomerId, orderToCancel, manager);
            }

            var OrderDetails = db.OrderDetails.ToList();
            foreach (var o in OrderDetails)
            {

                if (o.OrderId == orderToCancel.OrderId)
                {
                    Product p = db.Products.Find(o.ProductId);
                    p.ProductQuantity++;
                }
            }

            db.Orders.Remove(orderToCancel);
            db.SaveChanges();
            this.AddNotification("Order " + orderToCancel.OrderId + " has been cancelled", NotificationType.INFO);
            if (User.IsInRole("User"))
            {
                return RedirectToAction("MyOrders");
            }
            return RedirectToAction("Index");
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