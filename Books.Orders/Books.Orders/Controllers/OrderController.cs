using Books.Orders.Entity;
using Books.Orders.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Books.Orders.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly IOrder _order;
    private readonly IPayment _payment;

    public OrderController(IPayment payment, IOrder order)
    {
        _payment = payment;
        _order = order;
    }

    [Authorize]
    [HttpPost]
    [Route("PlaceOrder")]
    public async Task<IActionResult> PlaceOrder(int bookId, int quantity)
    {
        string authToken = Request.Headers["Authorization"].ToString();
        authToken = authToken.Substring("Bearer ".Length);
        int userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.Sid));
        string paymentUri = await _order.PlaceOrder(authToken, userId, bookId, quantity);

        if (paymentUri != null)
            return Ok(new { paymentUri = paymentUri, message = "Please pay your order to place the order", isSuccess = true });
        else
            return BadRequest(new { message = "Something went wrong", isSuccess = false });
    }

    [HttpPost]
    [Route("PaymentStatus")]
    public async Task<IActionResult> OrderConfirmation()
    {
        Stream body = Request.Body;
        //string response = await new StreamReader(body).ReadToEndAsync();
        PaymentResponseEntity response = await _payment.PaymentStatus(body);

        if (response.status == "success")
        {
            OrderEntity order = _order.PaymentSuccessfull(response.txnid);
            return Ok(new { orderDetails = order, paymentDetails = response });
        }
        else
            return BadRequest(new { message = "something went wrong" });
    }

    [Authorize]
    [HttpGet]
    [Route("MyOrders")]
    public IActionResult ViewOrders()
    {
        int userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.Sid));
        IEnumerable<OrderEntity> orders = _order.GetOrders(userId);

        if (orders.Any())
            return Ok(new { data = orders, isSuccess = true, message = "These are your orders" });
        else
            return BadRequest(new { isSuccess = false, message = "No orders placed yet" });
    }
}
