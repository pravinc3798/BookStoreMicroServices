using BookStore.Books.Entity;
using BookStore.Books.Interface;

namespace BookStore.Books.Service
{
    public class BookService : IBooks
    {
        private readonly BookContext _db;

        public BookService(BookContext db)
        {
            _db = db;
        }

        public BookEntity AddBook(BookEntity book)
        {
            _db.Books.Add(book);
            _db.SaveChanges();
            return book;
        }

        public bool DeleteBook(int id)
        {
            BookEntity book = _db.Books.FirstOrDefault(x => x.BookId == id);

            if (book != null)
            {
                _db.Books.Remove(book);
                _db.SaveChanges();
                return true;
            }
            return false;
        }

        public IEnumerable<BookEntity> GetAllBooks()
        {
            IEnumerable<BookEntity> books = _db.Books;

            return books;
        }

        public BookEntity GetBookById(int id)
        {
            BookEntity book = _db.Books.FirstOrDefault(x => x.BookId == id);

            return book;
        }

        public BookEntity UpdateBook(int id, BookEntity book)
        {
            _db.Books.Update(book);
            _db.SaveChanges();
            return book;
        }
    }
}
