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
            lblSpend.Text = UtilString.ToReadableString(_task.TotalSpend);
            //lblStatus.Text = _task.Status;

            string nextAction = "";
            if (_task.LatestStatus == ActivityStatusType.InProgress)
            {
                this.BackColor = Color.DarkGreen;
                this.ForeColor = Color.White;
                nextAction = "Pause";
            }
            else
            {
                this.BackColor = SystemColors.Control;
                this.ForeColor = Color.Black;
                nextAction = "Start";
            }
            //switch (_task.Status)
            //{
            //    case Status.InProgress:
            //        this.BackColor = Color.DarkGreen;
            //        this.ForeColor = Color.White;
            //        nextAction = "Stop";
            //        break;
            //    default:
            //    case Status.Cancelled:
            //        this.BackColor = SystemColors.Control;
            //        this.ForeColor = Color.Black;
            //        nextAction = "Start";
            //        break;
            //}


            btnAction.Text = nextAction;

        }

        private void btnAction_Click(object sender, EventArgs e)
        {
            if (btnAction.Text.ToLower() == "start")
            {
                if (OnAction != null)
                    OnAction(this, new TaskListEventArgs(_task, ActivityStatusType.InProgress));
                //_taskManager.UpdateTaskStatus(_task, Status.InProgress);
                //Task.UpdateStatus(Status.InProgress);
            }
            else
            {
                if (OnAction != null)
                    OnAction(this, new TaskListEventArgs(_task, ActivityStatusType.Pause));
                //_taskManager.UpdateTaskStatus(_task, Status.Pause);
                //Task.UpdateStatus(Status.Pause);
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            if (OnAction != null)
                OnAction(this, new TaskListEventArgs(_task, ActivityStatusType.Stop));

        }

        //private void btnAction_Click(object sender, EventArgs e)
        //{
        //    if (btnAction.Text.ToLower() == "start")
        //    {
        //        _taskManager.UpdateTaskStatus(_task.Id, Status.InProgress);
        //        //Task.UpdateStatus(Status.InProgress);
        //    }
        //    else
        //    {
        //        _taskManager.UpdateTaskStatus(_task.Id, Status.Pause);
        //        //Task.UpdateStatus(Status.Pause);
        //    }

        //    RefreshInfo();
        //}
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
