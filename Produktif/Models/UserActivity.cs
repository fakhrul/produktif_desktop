using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Linq;

namespace Produktif.Models
{
    public class UserActivity
    {
        [Key]
        public long Id { get; set; }
        [Required]
        public string Email { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual ICollection<ActivityTimer> ActivityTimerList { get; set; }


        public ActivityStatusType LatestStatus { get; set; }

        public string SynchStatus { get; set; }
        [NotMapped]
        public TimeSpan TotalSpend
        {
            get
            {
                TimeSpan timeSpend = TimeSpan.Zero;
                foreach (var timeTrack in ActivityTimerList)
                {
                    if (timeTrack.EndDateTime == DateTime.MinValue)
                        continue;
                    if (timeTrack.ActivityStatusType == ActivityStatusType.InProgress)
                    {
                        timeSpend += (timeTrack.EndDateTime - timeTrack.StartDateTime);
                    }
                }

                return timeSpend;
            }
        }

        public UserActivity()
        {
            //if(ActivityTimerList == null)
            ActivityTimerList = new ObservableCollection<ActivityTimer>();

        }

        public void UpdateStatus(ActivityStatusType status)
        {
            //add timer list here

            DateTime currentDT = DateTime.Now;

            if (ActivityTimerList.Count == 0)
            {
                var timer = new Models.ActivityTimer();
                timer.UserActivity = this;
                timer.StartDateTime = currentDT;
                timer.UserActivityId = Id;
                timer.ActivityStatusType = ActivityStatusType.Pause;
                ActivityTimerList.Add(timer);
                LatestStatus = timer.ActivityStatusType;
            }
            else
            {
                var latest = ActivityTimerList.OrderByDescending(c => c.StartDateTime).FirstOrDefault();
                latest.EndDateTime = currentDT;

                var timer = new Models.ActivityTimer();
                timer.StartDateTime = currentDT;
                //timer.EndDateTime = currentDT;
                timer.UserActivity = this;
                timer.UserActivityId = Id;
                timer.ActivityStatusType = status;
                ActivityTimerList.Add(timer);
                LatestStatus = status;
            }

        }
    }


}
