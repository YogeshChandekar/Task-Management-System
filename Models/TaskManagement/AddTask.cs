namespace TaskManagementPortal.Models.TaskManagement
{
    public class AddTask
    {
        public int ID { get; set; }
        public string TaskName { get; set; }

        public string Assignee { get; set; }

        public string Reporter { get; set; }

        public string StartDate { get; set; }

        public string EndDate { get; set; }
        public string reservation { get; set; }

        public int Status { get; set; }

        public int Active { get; set; } = 1;

        public IFormFile AttachFile { get; set; }
        public string AttachFileInBase64 { get; set; }

        public string Notes { get; set; }
    }
}
