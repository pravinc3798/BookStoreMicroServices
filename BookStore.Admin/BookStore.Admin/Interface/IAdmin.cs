using BookStore.Admin.Entity;

namespace BookStore.Admin.Interface;

public interface IAdmin
{
    AdminEntity RegisterAdmin(AdminEntity newAdmin);
    string Login(string email, string password);

}
