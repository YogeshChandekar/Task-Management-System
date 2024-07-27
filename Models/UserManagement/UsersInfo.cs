namespace TaskManagementPortal.Models.UserManagement
{
    public class UsersInfo
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int Approved { get; set; }
        public int IsAdmin { get; set; }
        public int Status { get; set; }
        public string ContactNo { get; set; }
        public int Attemptcount { get; set; }
        public int Islocked { get; set; }
    }
}
