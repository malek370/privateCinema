namespace Ath.Services
{
    public class Response<T>
    {
        public bool success { get; set; } = true;
        public string message { get; set; } = "";
        public T? data { get; set; } = default;
    }
}
