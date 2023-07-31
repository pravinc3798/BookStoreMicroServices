namespace Books.Orders.Entity;

public class ApiResponseEntity
{
    public object Data { get; set; }
    public bool IsSuccess { get; set; }
    public string Message { get; set; }
}
