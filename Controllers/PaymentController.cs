using Microsoft.AspNetCore.Mvc;
using IBSRAStore.Services;
using IBSRAStore.DTOs;
using System.ComponentModel.DataAnnotations;

namespace IBSRAStore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        /// <summary>
        /// Process payment for an order
        /// </summary>
        [HttpPost("process")]
        public async Task<ActionResult<PaymentResponse>> ProcessPayment([FromBody] PaymentRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _paymentService.ProcessPaymentAsync(request);
                
                if (!result.Success)
                {
                    return BadRequest(result);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new PaymentResponse
                {
                    Success = false,
                    Message = "Payment processing failed due to server error"
                });
            }
        }

        /// <summary>
        /// Get order details by order ID
        /// </summary>
        [HttpGet("order/{orderId}")]
        public async Task<ActionResult<OrderDetailsDto>> GetOrderDetails(string orderId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(orderId))
                {
                    return BadRequest(new { message = "Order ID is required" });
                }

                var orderDetails = await _paymentService.GetOrderDetailsAsync(orderId);
                
                if (orderDetails == null)
                {
                    return NotFound(new { message = $"Order with ID {orderId} not found" });
                }

                return Ok(orderDetails);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }
    }
}
