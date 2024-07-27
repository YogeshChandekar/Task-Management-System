namespace TaskManagementPortal.Models.UserManagement
{
    public class AddUser
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public int IsAdmin { get; set; }
        public string Password { get; set; }
    }
}
