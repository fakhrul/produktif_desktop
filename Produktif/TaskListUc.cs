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
       private Task _task { get; set; }
        public TaskManager _taskManager  { get;set;}
        public string Id { get { return _task.Id; } }

        public TaskListUc()
            :this (null, null)
        {
            InitializeComponent();
        }

        public TaskListUc(TaskManager tm, Task task)
        {
            InitializeComponent();

            _task = task;
            _task.PropertyChanged += _task_PropertyChanged;
            _taskManager = tm;
        }

        private void _task_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            RefreshInfo();
        }

        public void RefreshInfo()
        {
            lblDesription.Text = _task.Description;
            lblSpend.Text = UtilString.ToReadableString(_task.TotalSpend);
            lblStatus.Text = _task.CurrentStatus.ToString();

            string nextAction = "";
            switch (_task.CurrentStatus)
            {
                case Status.InProgress:
                    this.BackColor = Color.DarkGreen;
                    this.ForeColor = Color.White;
                    nextAction = "Stop";
                    break;
                default:
                case Status.Cancelled:
                    this.BackColor = SystemColors.Control;
                    this.ForeColor = Color.Black;
                    nextAction = "Start";
                    break;
            }


            btnAction.Text = nextAction;

        }

        private void btnAction_Click(object sender, EventArgs e)
        {
            if (btnAction.Text.ToLower() == "start")
            {
                _taskManager.UpdateTaskStatus(_task, Status.InProgress);
                //Task.UpdateStatus(Status.InProgress);
            }
            else
            {
                _taskManager.UpdateTaskStatus(_task, Status.Pause);
                //Task.UpdateStatus(Status.Pause);
            }
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
    }
