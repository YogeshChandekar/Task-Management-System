namespace TaskManagementPortal.Models
{
    public class BaseResponse
    {
        public int status { get; set; }
        public string succMsg { get; set; }
        public string retValue { get; set; }
        public string errMsg { get; set; }
        public string errCode { get; set; }
        public string txn { get; set; }
    }
}
