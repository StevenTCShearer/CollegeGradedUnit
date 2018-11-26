using Braintree;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ValueFurniture.Models;
using ValueFurniture.POCO_Classes;

namespace ValueFurniture.Controllers
{
    /// <summary>
    /// Controller for Checkout Process, including Payment. 
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    public class CheckoutController : Controller
    {

        /// <summary>
        /// The database
        /// </summary>
        ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// The configuration
        /// </summary>
        public IBraintreeConfiguration config = new BraintreeConfiguration();

        /// <summary>
        /// The transaction success statuses
        /// </summary>
        public static readonly TransactionStatus[] transactionSuccessStatuses = {
                                                                                    TransactionStatus.AUTHORIZED,
                                                                                    TransactionStatus.AUTHORIZING,
                                                                                    TransactionStatus.SETTLED,
                                                                                    TransactionStatus.SETTLING,
                                                                                    TransactionStatus.SETTLEMENT_CONFIRMED,
                                                                                    TransactionStatus.SETTLEMENT_PENDING,
                                                                                    TransactionStatus.SUBMITTED_FOR_SETTLEMENT
                                                                                };

        /// <summary>
        /// Order Details that are fillied out by Customer
        /// </summary>
        /// <returns></returns>
        public ActionResult AddressAndPayment()
        {
            var userId = User.Identity.GetUserId();
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var currentUser = userManager.FindById(User.Identity.GetUserId());
            ViewBag.CurrentUser = currentUser;
            return View();
        }

        /// <summary>
        /// Adds Order And then passes to Complete method
        /// </summary>
        /// <param name="values">Details Entered by User on Order Page</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddressAndPayment(FormCollection values)
        {
            var order = new Order();

            TryUpdateModel(order);

            try
            {
                var cart = ShoppingCart.GetCart(HttpContext);

                //Getting Current User Details 
                var currentUserId = User.Identity.GetUserId();
                var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                var currentUser = manager.FindById(User.Identity.GetUserId());


                //Adding Order Details from the Information Stored from User 
                order.CustomerId = User.Identity.GetUserId();
                order.Email = User.Identity.Name;
                order.FirstName = currentUser.FirstName;
                order.LastName = currentUser.Surname;
                order.OrderDate = DateTime.Now;
                order.OrderTotal = cart.GetTotal();

                if (currentUser.PhoneNumber != null)
                {
                    order.Phone = currentUser.PhoneNumber;
                }

                db.Orders.Add(order);
                db.SaveChanges();
                db.Dispose();

                Session["totalCost"] = cart.GetTotal();                
                Session["orderId"] = order.OrderId;

                cart.CreateOrder(order);
                return RedirectToAction("New");
            }
            catch (Exception ex)
            {
                ex.InnerException.ToString();
                return View(order);
            }
        }

        /// <summary>
        /// Directs to Payment Page
        /// </summary>
        /// <returns>
        /// Create()
        /// </returns>
        public ActionResult New()
        {
            var gateway = config.GetGateway();
            var clientToken = gateway.ClientToken.generate();
            ViewBag.ClientToken = clientToken;
            return View();
        }

        /// <summary>
        /// Creates Payment
        /// </summary>
        /// <returns>
        /// Complete()
        /// </returns>
        public ActionResult Create()
        {
            var gateway = config.GetGateway();
            decimal amount;
            try
            {
                amount = Convert.ToDecimal(Session["totalCost"]);
            }
            catch (FormatException e)
            {
                e.ToString();
                TempData["Flash"] = "Error: 81503: Amount is an invalid format.";
                return RedirectToAction("New");
            }

            var nonce = Request["payment_method_nonce"];
            var request = new TransactionRequest
            {
                Amount = amount,
                PaymentMethodNonce = nonce,
                Options = new TransactionOptionsRequest
                {
                    SubmitForSettlement = true
                }
            };

            int orderId = (int)Session["orderId"];
            Result<Transaction> result = gateway.Transaction.Sale(request);

            if (result.IsSuccess())
            {
                Transaction transaction = result.Target;

                Order order = db.Orders.Find(orderId);
                order.TransactionID = transaction.Id;
                db.SaveChanges();

                return RedirectToAction("Complete", new { id = orderId });
            }
            else if (result.Transaction != null)
            {
                return RedirectToAction("Failed");
            }
            else
            {
                string errorMessages = "";
                foreach (ValidationError error in result.Errors.DeepAll())
                {
                    errorMessages += "Error: " + (int)error.Code + " - " + error.Message + "\n";
                }
                TempData["Flash"] = errorMessages;
                Failed();
                ViewBag.errorMessage = errorMessages;
                return View("Error");
            }

        }

        /// <summary>
        /// Transaction Successful Screen
        /// </summary>
        /// <param name="id">Transaction ID</param>
        /// <returns></returns>
        public ActionResult Show(string id)
        {
            var gateway = config.GetGateway();
            Transaction transaction = gateway.Transaction.Find(id);

            if (transactionSuccessStatuses.Contains(transaction.Status))
            {
                TempData["header"] = "Sweet Success!";
                TempData["icon"] = "success";
                TempData["message"] = "Your test transaction has been successfully processed. See the Braintree API response and try again.";
            }
            else
            {
                TempData["header"] = "Transaction Failed";
                TempData["icon"] = "fail";
                TempData["message"] = "Your test transaction has a status of " + transaction.Status + ". See the Braintree API response and try again.";
            };

            ViewBag.Transaction = transaction;
            return View();
        }

        /// <summary>
        /// Displays Confirmation of Order
        /// </summary>
        /// <param name="id">ID of Order</param>
        /// <returns>
        /// Confirmation of Order
        /// </returns>
        public ActionResult Complete(int id)
        {
            //Getting current users ID
            var currentUserId = User.Identity.GetUserId();
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var currentUser = manager.FindById(User.Identity.GetUserId());
           
            bool isValid = db.Orders.Any(o => o.OrderId == id && o.CustomerId == currentUserId);

            if (isValid)
            {
                Order order = db.Orders.Find(id);
                var managerHttp = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                SendOrderConfirmationEmail(order.CustomerId, "Order Confirmation: #" + order.OrderId, order, managerHttp);
                if (currentUser.PhoneNumber != null)
                {
                    SendOrderConfirmationSMS(order.CustomerId, order);
                }
                return View(id);
            }
            else
            {
                return View("Error");
            }

        }

        /// <summary>
        /// Sends Request for refund
        /// </summary>
        /// <param name="id">Transaction ID</param>
        /// <param name="orderId">The order identifier.</param>
        /// <param name="userId">The user identifier.</param>
        public void Cancel(string id, int orderId, string userId)
        {
            Order order = db.Orders.Find(orderId);
            var refundAmount = order.OrderTotal * (decimal)0.75;

            var gateway = config.GetGateway();
            Result<Transaction> result = gateway.Transaction.Refund(id, refundAmount);
            if (!result.IsSuccess())
            {
                List<ValidationError> errors = result.Errors.DeepAll();
            }
        }

        /// <summary>
        /// Deletes an order if the payment for it failed
        /// </summary>
        public void Failed()
        {
            int id = (int)Session["orderId"];
            Order order = db.Orders.Find(id);
            db.Orders.Remove(order);
            db.Dispose();
        }

        /// <summary>
        /// Sends Confirmation of Order via Email to User
        /// </summary>
        /// <param name="userId">Identifier of User</param>
        /// <param name="subject">Subject of the Email</param>
        /// <param name="order">Order that has been created</param>
        /// <param name="manager">Current ApplicationUserManager.</param>
        public void SendOrderConfirmationEmail(string userId, string subject, Order order, ApplicationUserManager manager)
        {
            manager.SendEmail(userId, subject, "Hi " + order.FirstName + ", <br>Order #" + order.OrderId + " has been placed. Thank you for shopping with us.");
        }

        /// <summary>
        /// Sends Confirmation of Order via SMS to User
        /// </summary>
        /// <param name="userId">Identifier of User</param>
        /// <param name="order">Order that has been placed</param>
        public void SendOrderConfirmationSMS(string userId, Order order)
        {
            var manager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            manager.SendSms(userId, "Hi " + order.FirstName + ", Order #" + order.OrderId + " has been placed. Thank you for shopping with us.");
        }

        /// <summary>
        /// Sends Confirmation of Order Cancellation via Email to User
        /// </summary>
        /// <param name="userId">Identifier of User</param>
        /// <param name="subject">Subject of Email</param>
        /// <param name="order">Order to be cancelled</param>
        /// <param name="manager">Current AplicationUserManager</param>
        public void SendOrderCancellationCofirmationEmail(string userId, string subject, Order order, ApplicationUserManager manager)
        {
            manager.SendEmail(userId, subject, "Hi " + order.FirstName + ", <br>Order #" + order.OrderId + " has been cancelled. Thank you for shopping with us.");
        }

        /// <summary>
        /// Sends the order cancellation confirmation SMS.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="order">Order being Cancelled.</param>
        /// <param name="manager">Current AplicationUserManager</param>
        public void SendOrderCancellationConfirmationSMS(string userId, Order order, ApplicationUserManager manager)
        {
            manager.SendSms(userId, "Hi " + order.FirstName + ", Order #" + order.OrderId + " has been succesfully cancelled. Thank you for shoppping with us.");
        }
    }
}