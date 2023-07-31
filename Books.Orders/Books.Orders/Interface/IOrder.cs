using Books.Orders.Entity;

namespace Books.Orders.Interface;

public interface IOrder
{
    Task<string> PlaceOrder(string token, int userId, int bookId, int quantity);
    IEnumerable<OrderEntity> GetOrders(int userId);
    OrderEntity PaymentSuccessfull(string orderId);
}
