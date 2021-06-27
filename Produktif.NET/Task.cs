using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Produktif
{
    public class Task : INotifyPropertyChanged
    {
        public string Id { get; set; }
        public DateTime Created { get; set; }
        public string Description { get; set; }
        public Status CurrentStatus
        {
            get
            {
                return TimeTrackList[0].Status;
            }
        }

        private DateTime _LastUpdateDateTime;
        public DateTime LastUpdateDateTime { get
            {
                return _LastUpdateDateTime;
            }
            set
            {
                if (value != _LastUpdateDateTime)
                {
                    _LastUpdateDateTime = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private List<TimeTrack> _TimeTrackList;
        public List<TimeTrack> TimeTrackList { get
            {
                return _TimeTrackList;
            }
            set
            {
                if (value != _TimeTrackList)
                {
                    _TimeTrackList = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public TimeSpan TotalSpend
        {
            get
            {
                TimeSpan timeSpend = TimeSpan.Zero;
                foreach (var timeTrack in TimeTrackList)
                {
                    if (timeTrack.EndDateTime == DateTime.MinValue)
                        continue;
                    if (timeTrack.Status == Status.InProgress)
                    {
                        timeSpend += (timeTrack.EndDateTime - timeTrack.StartDateTime);
                    }
                }

                return timeSpend;
            }
        }

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public Task()
        {
            if (string.IsNullOrEmpty(Id))
                Id = Guid.NewGuid().ToString();

            Created = DateTime.Now;

            if (LastUpdateDateTime == null)
                LastUpdateDateTime = DateTime.Now;
            UpdateStatus(Status.Pause);
        }

        public void UpdateStatus(Status newStatus)
        {
            DateTime currentDT = DateTime.Now;

            if (TimeTrackList == null || TimeTrackList.Count == 0)
            {
                TimeTrackList = new List<TimeTrack>();
                TimeTrackList.Insert(0, new TimeTrack { StartDateTime = currentDT, Status = Status.Pause });
            }

            TimeTrackList[0].EndDateTime = currentDT;

            TimeTrackList.Insert(0, new TimeTrack { StartDateTime = currentDT, Status = newStatus });

            LastUpdateDateTime = currentDT;


        }
    }

    public class TimeTrack
    {
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public Status Status { get; set; }
    }


    public enum Status
    {
        InProgress,
        Pause,
        Done,
        Cancelled,
    }
}
