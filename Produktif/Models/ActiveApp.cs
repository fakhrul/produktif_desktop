using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Produktif.Models
{
    public class ActiveApp
    {
        [Key]
        public long Id { get; set; }

        [StringLength(50)]
        [Required]
        public string Name { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public string SynchStatus { get; set; }

        [NotMapped]
        public string UniqueIdentifer
        {
            get
            {
                return string.Format("{0}$$$!!!{1}", Name, Title);
            }
        }

        [NotMapped]
        public TimeSpan TotalTimeSpend
        {
            get
            {
                if (EndDateTime == DateTime.MinValue)
                    EndDateTime = DateTime.Now;

                return EndDateTime - StartDateTime;
            }
        }


    }
}
