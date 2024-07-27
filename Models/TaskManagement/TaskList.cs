namespace TaskManagementPortal.Models.TaskManagement
{
    public class TaskList
    {
        public int ID { get; set; }
        public string TaskName { get; set; }
        public string Assignee { get; set; }
        public string Reporter { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int Status { get; set; } // 0 = Open, 1 = InProcess, 2 = Complete
        public int Active { get; set; } = 1;
        public string AttachFile { get; set; } // Base64 encoded PDF content
        public string Notes { get; set; }
    }
}
