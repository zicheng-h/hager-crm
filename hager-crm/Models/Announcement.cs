using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;


namespace hager_crm.Models
{

    public enum AnnouncementSeverity
    {
        Neutral,
        Info,
        Warning,
        Important,
    }
    
    
    public class Announcement
    {
        public Announcement()
        {
            Severity = AnnouncementSeverity.Neutral;
            PostedAt = DateTime.Now;
        }
        
        public int AnnouncementID { get; set; }
        
        [Required]
        [StringLength(64, MinimumLength = 4)]
        public string Title { get; set; }
        
        [Required]
        [StringLength(256, MinimumLength = 16)]
        public string Message { get; set; }
        
        public DateTime PostedAt { get; set; }
        
        public AnnouncementSeverity Severity { get; set; }
        
        public string GetSeverity()
        {
            switch (Severity)
            {
                case AnnouncementSeverity.Important:
                    return "danger";
                case AnnouncementSeverity.Info:
                    return "info";
                case AnnouncementSeverity.Warning:
                    return "warning";
                default:
                    return "secondary";
            }
        }
        
        [JsonIgnore]
        public List<AnnouncementEmployee> EmployeesUnread { get; set; }
    }
}