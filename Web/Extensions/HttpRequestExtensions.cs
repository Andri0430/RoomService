namespace Web.Extensions
{
    public static class HttpRequestExtensions
    {
        public static bool IsAjaxRequest(this HttpRequest request)
        => request.Headers["X-Requested-With"] == "XMLHttpRequest";
    }
}
