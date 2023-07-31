using Books.Orders.Entity;
using Books.Orders.Interface;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text.Json.Serialization;

namespace Books.Orders.Service;

public class UserService : IUserService
{
    private readonly IConfiguration _config;

    public UserService(IConfiguration config)
    {
        _config = config;
    }
    public async Task<UserEntity> GetUser(string jwtToken)
    {
        using (var httpClient = new HttpClient())
        {
            // Set the base address of the User API
            string userUri = _config["ExternalApi:UserApi"];
            httpClient.BaseAddress = new Uri(userUri);

            // Set the JWT token in the request headers
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);

            // Send a request to the User API to get the user details
            HttpResponseMessage response = await httpClient.GetAsync("/api/User/UserDetails");

            // Check if the request was successful
            if (response.IsSuccessStatusCode)
            {
                // Deserialize the response content to UserDetails object
                string responseContent = await response.Content.ReadAsStringAsync();
                ApiResponseEntity apiResponse = JsonConvert.DeserializeObject<ApiResponseEntity>(responseContent);
                string user = apiResponse.Data.ToString();
                UserEntity userDetails = JsonConvert.DeserializeObject<UserEntity>(user);
                return userDetails;
            }
            else
            {
                return null;
            }
        }
    }
}
