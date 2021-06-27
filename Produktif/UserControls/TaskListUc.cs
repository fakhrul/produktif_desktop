using Produktif.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Produktif
{
    public partial class TaskListUc : UserControl
    {
        private UserActivity _task { get; set; }
        public TaskManager _taskManager { get; set; }
        public long Id { get { return _task.Id; } }

        public event EventHandler<TaskListEventArgs> OnAction;

         public TaskListUc()
            : this(null, null)
        {
            InitializeComponent();
        }

        public TaskListUc(TaskManager tm, UserActivity task)
        {
            InitializeComponent();

            _task = task;
            //_task.PropertyChanged += _task_PropertyChanged;
            _taskManager = tm;
        }


        private void _task_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            RefreshInfo();
        }

        public void RefreshInfo()
        {
            lblDesription.Text = _task.Name;
            lblSpend.Text = Extensions.ToReadableString(_task.TotalSpend);
            if (_task.LatestStatus == ActivityStatusType.InProgress)
            {
                 btnAction.BackgroundImage = Produktif.Properties.Resources.pause;
            }
            else
            {

                btnAction.BackgroundImage = Produktif.Properties.Resources.play;
                //nextAction = "Start";
            }


        }

        private void btnAction_Click(object sender, EventArgs e)
        {
            if (_task.LatestStatus != ActivityStatusType.InProgress)
            {
                if (OnAction != null)
                    OnAction(this, new TaskListEventArgs(_task, ActivityStatusType.InProgress));
            }
            else
            {
                if (OnAction != null)
                    OnAction(this, new TaskListEventArgs(_task, ActivityStatusType.Pause));
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            if (OnAction != null)
                OnAction(this, new TaskListEventArgs(_task, ActivityStatusType.Stop));

        }

    }

    public class TaskListEventArgs : EventArgs
    {
        public UserActivity UserActivity { get; set; }
        public ActivityStatusType Action { get; set; }

        public TaskListEventArgs(UserActivity task, ActivityStatusType action)
        {
            UserActivity = task;
            Action = action;
        }
    }
}
