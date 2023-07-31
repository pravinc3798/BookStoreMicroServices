using BookStore.Books.Entity;

namespace BookStore.Books.Interface;

public interface IBooks
{
    BookEntity AddBook(BookEntity book);
    BookEntity UpdateBook(int id, BookEntity book);
    bool DeleteBook(int id);
    IEnumerable<BookEntity> GetAllBooks();
    BookEntity GetBookById(int id);
}
