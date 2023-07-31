using BookStore.Orders.Entity;

namespace Books.Orders.Interface;

public interface IBookService
{
    Task<BookEntity> GetBook(int id);
}
