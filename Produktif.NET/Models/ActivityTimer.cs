using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Produktif.Models
{
    public class ActivityTimer
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public long UserActivityId { get; set; }
        public virtual UserActivity UserActivity { get; set; }

        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }

        public ActivityStatusType ActivityStatusType { get; set; }
        public string SynchStatus { get; set; }

        public ActivityTimer()
        {

        }

    }

    public enum ActivityStatusType
    {
        Pause,
        InProgress,
        Stop
    }
}
