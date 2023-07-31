using Books.Orders.Entity;

namespace Books.Orders.Interface;

public interface IUserService
{
    Task<UserEntity> GetUser(string jwtToken);
}
