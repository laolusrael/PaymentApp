using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaymentApp.Core.Shared.Dtos;
using PaymentApp.UI.Models.Interfaces;

namespace PaymentApp.UI.Controllers
{
    [AllowAnonymous]
    [Route("")]
    public class PaymentController : Controller
    {
        private readonly IPaymentViewModel _viewModel;
        private readonly ICreditCardChecker _creditCardChecker;
        private ISession _session; 

        public PaymentController(IPaymentViewModel viewModel, ICreditCardChecker checker, IHttpContextAccessor context)
        {
            _viewModel = viewModel;
            _creditCardChecker = checker;
            _session = context.HttpContext.Session;
        }


        [HttpGet("welcome")]
        public IActionResult Index(int orderId = 0)
        {
           return View(new DateConfirmationDto() { OrderId = orderId});
        }

        [HttpPost("welcome")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(DateConfirmationDto model)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    if(model.DateOfBirth == DateTime.MinValue)
                    {
                        throw new Exception("The date you entered is not valid, Please check you input and try again");
                    }

                    // process input and redirect here
                    var isCorrect = await _viewModel.ConfirmCustomerBirthDateAsync(model);
                    if (isCorrect)
                    {
                        _session.SetString("_sessionState", "valid");
                        return RedirectToAction("OrderDetails", new { orderId = model.OrderId });
                    }

                    ModelState.AddModelError("", "Entered date is not correct");
                }
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View(model);
        }

        [HttpGet("orderSummary/{orderId}")]
        public async Task<IActionResult> OrderDetails(int orderId) {

            var sessState = _session.GetString("_sessionState");


            if (string.IsNullOrEmpty(sessState) || sessState != "valid")
            {
                return RedirectToAction("Index", new { orderId = orderId });
            }

            OrderSummaryDto orderSummary = null;

            try
            {
                
                orderSummary = await _viewModel.GetOrderSummaryAsync(orderId);

                

                if (orderSummary == null)
                {
                    TempData.Add("Message", "Order not found");
                    return RedirectToAction("Error");
                }

                if(orderSummary.Status == "PAID")
                {
                    TempData.Add("Message", "Order has already been paid for");
                    return RedirectToAction("Error");
                }


                await _viewModel.UpdateOrderStatusAsync(orderId, "viewed");

            }
            catch(Exception ex)
            {
                TempData.Add("Message", ex.Message);
                return RedirectToAction("Error");
            }

            ViewBag.OrderSummary = orderSummary;


            return View(new PaymentDto() { OrderId = orderId});
        }
        [HttpPost("orderSummary/{orderId}")]
        public async Task<IActionResult> OrderDetails(int orderId, PaymentDto model)
        {

            OrderSummaryDto orderSummary = null;

            try
            {
                orderSummary = await _viewModel.GetOrderSummaryAsync(model.OrderId);

                if (orderSummary == null)
                {
                    TempData.Add("Message", "Unable to load your order details. Please try again latter or contact support.");
                    return RedirectToAction("Error");
                }

                ViewBag.OrderSummary = orderSummary;

                if (ModelState.IsValid == false)
                {
                    return View(model);
                }


                if (string.IsNullOrWhiteSpace(model.CreditCardNumber))
                {
                    throw new Exception("Please enter your credit card details to proceed with payment");
                }


                var isValidCard = await _creditCardChecker.CheckCard(model.CreditCardNumber);

                if (isValidCard == false)
                {
                    ModelState.AddModelError("", "Invalid credit card number entered");
                    return View(model);
                }

                await _viewModel.UpdateOrderStatusAsync(model.OrderId, "paid");


                TempData.Add("Name", $"{orderSummary.FirstName} {orderSummary.LastName}");
                TempData.Add("Total", $"{ orderSummary.Total.ToString("$ #, ##0.00")}");
                return RedirectToAction("Completed");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            return View(model);
        }

        [HttpGet("error")]
        public IActionResult Error()
        {

            return View();
        }
        [HttpGet("thankyou")]
        public IActionResult Completed()
        {
            _session.Clear();

            return View();
        }
    }
}