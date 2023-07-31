using BookStore.Books.Entity;
using BookStore.Books.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Books.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBooks _book;

        public BooksController(IBooks book)
        {
            _book = book;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("Add")]
        public IActionResult AddBook(BookEntity newBook)
        {
            BookEntity book = _book.AddBook(newBook);

            if (book != null)
                return Ok(new { data = book, isSuccess = true, message = "book added successfully" });
            else
                return BadRequest(new { isSuccess = false, message = "Something went wrong" });
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        [Route("Update")]
        public IActionResult UpdateBook(int id, BookEntity updatedBook)
        {
            BookEntity book = _book.UpdateBook(id, updatedBook);

            if (book != null)
                return Ok(new { data = book, isSuccess = true, message = "book updated successfully" });
            else
                return BadRequest(new { isSuccess = false, message = "Something went wrong" });
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("Delete")]
        public IActionResult Delete(int id)
        {
            bool result = _book.DeleteBook(id);

            if (result)
                return Ok(new { isSuccess = true, message = "book deleted successfully" });
            else
                return BadRequest(new { isSuccess = false, message = "Something went wrong" });
        }

        [HttpGet]
        [Route("Books")]
        public IActionResult GetBooks()
        {
            IEnumerable<BookEntity> books = _book.GetAllBooks();

            if (books != null)
                return Ok(new { data = books, isSuccess = true, message = "books retrived successfully" });
            else
                return BadRequest(new { isSuccess = false, message = "Something went wrong" });
        }

        [HttpGet]
        [Route("Book")]
        public IActionResult GetBookById(int id)
        {
            BookEntity book = _book.GetBookById(id);

            if (book != null)
                return Ok(new { data = book, isSuccess = true, message = "book retrived successfully" });
            else
                return BadRequest(new { isSuccess = false, message = "Something went wrong" });
        }
    }
}
