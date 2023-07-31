using Books.Orders.Entity;
using Books.Orders.Interface;

namespace Books.Orders.Service;

public class OrderServices : IOrder
{
    private readonly OrderContext _db;
    private readonly IBookService _bookService;
    private readonly IUserService _userService;
    private readonly IPayment _payment;

    public OrderServices(OrderContext db, IBookService bookService, IUserService userService, IPayment payment)
    {
        _db = db;
        _bookService = bookService;
        _userService = userService;
        _payment = payment;
    }
    public IEnumerable<OrderEntity> GetOrders(int userId)
    {
        IEnumerable<OrderEntity> orders = _db.Orders.Where(x => x.UserId == userId);
        return orders;
    }

    public OrderEntity PaymentSuccessfull(string orderId)
    {
        OrderEntity order = _db.Orders.FirstOrDefault(x => x.OrderId == orderId);

        if (order != null)
        {
            order.IsPaid = true;
            order.OrderStatus = "Order Placed Successfully";
            _db.Orders.Update(order);
            _db.SaveChanges();
            return order;
        }
        else
            return null;
    }

    public async Task<string> PlaceOrder(string token, int userId, int bookId, int quantity)
    {
        OrderEntity newOrder = new()
        {
            OrderId = Guid.NewGuid().ToString(),
            UserId = userId,
            BookId = bookId,
            Quantity = quantity,
            CreatedDate = DateTime.Now,
            Book = await _bookService.GetBook(bookId),
            User = await _userService.GetUser(token),
        };

        newOrder.OrderAmount = (decimal)newOrder.Book.DiscountedPrice * newOrder.Quantity;

        _db.Orders.Add(newOrder);
        _db.SaveChanges();

        PaymentRequestEntity paymentRequest = new PaymentRequestEntity()
        {
            amount = newOrder.OrderAmount.ToString(),
            email = newOrder.User.EmailId,
            firstname = newOrder.User.FullName,
            phoneNumber = newOrder.User.ContactNumber,
            productinfo = newOrder.Book.BookName,
            txnid = newOrder.OrderId,
        };

        string paymentUrl = await _payment.Payment(paymentRequest);
        return paymentUrl;
    }
}
