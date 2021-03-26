namespace hager_crm.Models
{
    public class AnnouncementEmployee
    {

        public int AnnouncementEmployeeID { get; set; }

        public int EmployeeID { get; set; }
        public int AnnouncementID { get; set; }
        
        public Employee Employee { get; set; } 
        public Announcement Announcement { get; set; } 
    }
}