namespace TaskManagementPortal.CommonRequestHandler
{
    public interface ICommanWebRequestHandler
    {
        Task<HttpResponseMessage> WebRequestExecute(string httpclientname, string url, string method, string input, string functionalityName);
    }
}
