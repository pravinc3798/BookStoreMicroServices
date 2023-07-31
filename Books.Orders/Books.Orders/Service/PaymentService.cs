using Books.Orders.Entity;
using Books.Orders.Interface;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Books.Orders.Service;

public class PaymentService : IPayment
{
    private readonly IConfiguration _config;

    public PaymentService(IConfiguration config)
    {
        _config = config;
    }

    public async Task<string> Payment(PaymentRequestEntity orderDetails)
    {
        using (HttpClient client = new HttpClient())
        {
            string payUri = _config["PayU:Gateway"];
            var content = new FormUrlEncodedContent(ToDictionary(orderDetails));
            content.Headers.Clear();
            content.Headers.Add("Content-Type", "application/x-www-form-urlencoded");

            var response = await client.PostAsync(payUri, content);

            if (response.IsSuccessStatusCode)
            {
                return response.RequestMessage.RequestUri.ToString();
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                return errorMessage;
            }
        }
    }

    private Dictionary<string, string> ToDictionary(PaymentRequestEntity orderDetails)
    {
        string merchantSalt = _config["PayU:MerchantSalt"];
        string merchantKey = _config["PayU:MerchantKey"];
        string successUrl = _config["PayU:SuccessUrl"];
        string failureUrl = _config["PayU:FailureUrl"];

        orderDetails.surl = successUrl;
        orderDetails.furl = failureUrl;
        orderDetails.key = merchantKey;
        orderDetails.hash = GetHash(orderDetails, merchantSalt);

        var dictionary = new Dictionary<string, string>();
        var properties = typeof(PaymentRequestEntity).GetProperties();

        foreach (var property in properties)
        {
            var value = property.GetValue(orderDetails)?.ToString() ?? string.Empty;
            dictionary.Add(property.Name, value);
        }
        return dictionary;
    }

    private string GetHash(PaymentRequestEntity orderDetails, string merchantSalt)
    {
        string generatedHash = "";
        var hashString = $"{orderDetails.key}|{orderDetails.txnid}|{orderDetails.amount}|{orderDetails.productinfo}|{orderDetails.firstname}|{orderDetails.email}|||||||||||{merchantSalt}";

        using (var sha512 = SHA512.Create())
        {
            byte[] bytes = Encoding.UTF8.GetBytes(hashString);
            byte[] hashBytes = sha512.ComputeHash(bytes);

            foreach (byte hashByte in hashBytes)
            {
                generatedHash += String.Format("{0:x2}", hashByte);
            }

            return generatedHash;
        }
    }

    public async Task<PaymentResponseEntity> PaymentStatus(Stream body)
    {
        var transactionData = await new StreamReader(body).ReadToEndAsync();

        var parsedData = HttpUtility.ParseQueryString(transactionData);

        PaymentResponseEntity response = new()
        {
            mihpayid = parsedData["mihpayid"],
            status = parsedData["status"],
            hash = parsedData["hash"],
            bank_ref_num = parsedData["bank_ref_name"],
            error = parsedData["error"],
            error_Message = parsedData["error_Message"],
            txnid = parsedData["txnid"]
        };

        return response;
    }
}
