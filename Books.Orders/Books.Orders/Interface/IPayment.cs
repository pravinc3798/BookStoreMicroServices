using Books.Orders.Entity;

namespace Books.Orders.Interface;

public interface IPayment
{
    Task<PaymentResponseEntity> PaymentStatus(Stream body);
    Task<string> Payment(PaymentRequestEntity orderDetails);
}
