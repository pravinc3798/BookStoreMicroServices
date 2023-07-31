using Books.Orders.Entity;
using Books.Orders.Interface;
using BookStore.Orders.Entity;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace Books.Orders.Service;

public class BookService : IBookService
{
    private readonly IConfiguration _config;

    public BookService(IConfiguration config)
    {
        _config = config;
    }

    public async Task<BookEntity> GetBook(int id)
    {
        using (var httpClient = new HttpClient())
        {
            // Set the base address of the User API
            string bookUri = _config["ExternalApi:BookApi"];
            httpClient.BaseAddress = new Uri(bookUri);

            // Send a request to the User API to get the user details
            HttpResponseMessage response = await httpClient.GetAsync("Book?id=" + id);

            // Check if the request was successful
            if (response.IsSuccessStatusCode)
            {
                // Deserialize the response content to UserDetails object
                string responseContent = await response.Content.ReadAsStringAsync();
                ApiResponseEntity apiResponse = JsonConvert.DeserializeObject<ApiResponseEntity>(responseContent);
                string book = apiResponse.Data.ToString();
                BookEntity bookDetails = JsonConvert.DeserializeObject<BookEntity>(book);
                return bookDetails;
            }
            else
            {
                return null;
            }
        }
    }
}
