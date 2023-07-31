using BookStore.User.Entity;

namespace BookStore.User.Interface;

public interface IUser
{
    UserEntity Register(UserEntity newUser);
    string Login(string email, string password);
    bool ForgetPassword(string email);
    bool ResetPassword(string password, string confirmPassword);
    UserEntity GetUserDetails(int userId);
}
