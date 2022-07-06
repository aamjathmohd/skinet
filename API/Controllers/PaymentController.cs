using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using API.Errors;
using Core.Entities;
using Core.Intefaces;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Stripe;
using Stripe.Infrastructure;
using Order=Core.Entities.OrderAggregate.Order;

namespace API.Controllers
{
    public class PaymentController:BaseApicontroller
    {
        private readonly IPaymentService _paymentService;
        private const string WhSecret="whsec_2405321374c5a3e93311840c335a0fc0267ef2746a276a0fcc1850222e6a762c";
        private readonly ILogger<PaymentController> _logger;
        public PaymentController(IPaymentService paymentService,ILogger<PaymentController> logger)
        {
            _logger = logger;
            _paymentService = paymentService;

        }
        [Authorize]
        [HttpPost("{basketId}")]
        public async Task<ActionResult<CustomerBasket>> CreateOrUpdatePaymentIntent(string basketId)
        {
            var basket= await _paymentService.CreateOrUpdatePaymentIntent(basketId);
            if(basket==null){
                return BadRequest(new ApiResponse(400,"Problem with your basket!"));
            }
            return basket;
        }
        [HttpPost("webhook")]
        public async Task<ActionResult> StripWebhook()
        {
            var json=await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            var stripEvent=EventUtility.ConstructEvent(json,Request.Headers["Stripe-Signature"],WhSecret);
            PaymentIntent intent;
            Order order;
            switch(stripEvent.Type)
            {
                case "payment_intent.succeeded":
                    intent =(PaymentIntent)stripEvent.Data.Object;
                    _logger.LogInformation("Payment Succeeded!: ",intent.Id);
                    order=await _paymentService.UpdateOrPaymentSucceeded(intent.Id);
                    _logger.LogInformation("Order updated to payment received: "+order.Id);
                    break;
                case "payment_intent.payment_failed":
                    intent=(PaymentIntent)stripEvent.Data.Object;
                    _logger.LogInformation("Payment Failed!: ",intent.Id);
                    order=await _paymentService.UpdateOrderPaymentFailed(intent.Id);
                    _logger.LogInformation("Payment Failed!: ",order.Id);
                    break;
                    
            }
            return new EmptyResult();

        }
    }
}