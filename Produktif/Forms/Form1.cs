using Microsoft.EntityFrameworkCore;
using Produktif.Interfaces;
using Produktif.Models;
using Produktif.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace Produktif
{
    public partial class Form1 : Form
    {
        private readonly IActivityTimerRepository _activityTimerRepository;
        private readonly IUserActivityRepository _userActivityRepository;
        private readonly IActiveAppRepository _activeAppRepository;
        private readonly IUnitOfWork _unitOfWork;

        TaskManager _taskManager;
        Timer _timerCheckActivity;
        Timer _refreshActiveAppDisplay;

        private WindowHelper _windowHelper;
        //WinEventDelegate dele = null;
        private object _dbLock = new object();
        AddNewTaskForm _addNewTaskForm;
        public Form1()
        {
            InitializeComponent();
            RepositionWindowsBottomRight();

            IDatabaseFactory database = new DatabaseFactory();

            _userActivityRepository = new UserActivityRepository(database);
            _activityTimerRepository = new ActivityTimerRepository(database);
            _activeAppRepository = new ActiveAppRepository(database);
            _unitOfWork = new UnitOfWork(database);

            _taskManager = new TaskManager();


            _windowHelper = new WindowHelper();
            _windowHelper.OnActiveWindowUpdate += _windowHelper_OnActiveWindowUpdate;

        }

        private void _refreshActiveAppDisplay_Tick(object sender, EventArgs e)
        {
            try
            {
                IEnumerable<ActiveApp> activeApps;
                lock (_dbLock)
                    activeApps = _activeAppRepository.SinceInHour(8);

                UpdateGuiActiveApps(activeApps);

                UpdateGuiMostBrowseUrl(activeApps);


                IEnumerable<UserActivity> userActivities;
                lock (_dbLock)
                    userActivities = _userActivityRepository.SinceInHour(24);

                activeAppuc1.UpdateTaskProgress(userActivities);

            }
            catch (Exception ex)
            {
            }

        }

        private void UpdateGuiMostBrowseUrl(IEnumerable<ActiveApp> activeApps)
        {
            Dictionary<string, TimeSpan> mostBrowsingUrl = new Dictionary<string, TimeSpan>();

            foreach (var activeApp in activeApps)
            {
                string unique = string.Format("{0}", activeApp.Url);
                if (!string.IsNullOrEmpty(unique))
                {
                    if (!string.IsNullOrEmpty(Extensions.IsValidUrl(unique)))
                    {
                        if (mostBrowsingUrl.ContainsKey(unique))
                        {
                            mostBrowsingUrl[unique] += activeApp.TotalTimeSpend;
                        }
                        else
                        {
                            mostBrowsingUrl.Add(unique, activeApp.TotalTimeSpend);
                        }
                    }
                }
            }

            activeAppuc1.UpdateMostBrowseUrl(mostBrowsingUrl);
        }

        private void UpdateGuiActiveApps(IEnumerable<ActiveApp> activeApps)
        {
            Dictionary<string, TimeSpan> activeAppDict = new Dictionary<string, TimeSpan>();

            foreach (var activeApp in activeApps)
            {
                string unique = string.Format("{0}", activeApp.Name);
                if (activeAppDict.ContainsKey(unique))
                {
                    activeAppDict[unique] += activeApp.TotalTimeSpend;
                }
                else
                {
                    activeAppDict.Add(unique, activeApp.TotalTimeSpend);
                }
            }

            activeAppuc1.UpdateMostUseApps(activeAppDict);
        }

        private void _windowHelper_OnActiveWindowUpdate(object sender, ActiveWindowUpdateEventArgs e)
        {
            ActiveApp latestActivity;
            lock (_dbLock)
                latestActivity = _activeAppRepository.ByLatestDateTime();

            if (latestActivity == null)
            {
                lock (_dbLock)
                {
                    ActiveApp activeApp = new ActiveApp
                    {
                        Name = e.ActiveWindow.Process,
                        Title = e.ActiveWindow.Title,
                        StartDateTime = DateTime.Now,
                        Url = e.ActiveWindow.Url,
                    };
                    _activeAppRepository.Add(activeApp);
                    _unitOfWork.Commit();
                }
                return;
            }

            if (latestActivity.UniqueIdentifer != e.ActiveWindow.ToString())
            {
                lock (_dbLock)
                {
                    latestActivity.EndDateTime = DateTime.Now;
                    _activeAppRepository.Update(latestActivity);

                    ActiveApp activeApp = new ActiveApp
                    {
                        Name = e.ActiveWindow.Process,
                        Title = e.ActiveWindow.Title,
                        StartDateTime = DateTime.Now,
                        Url = e.ActiveWindow.Url,
                    };
                    _activeAppRepository.Add(activeApp);
                    _unitOfWork.Commit();
                }
            }

        }

        private void _timer_Tick(object sender, EventArgs e)
        {
            DateTime current = DateTime.Now;
            var keyboardPressTimeSpan = current - _taskManager.LastKeyboardPress;
            var mouseMoveTimeSpan = current - _taskManager.LastMouseMoved;

            //lblKeyPress.Text = Extensions.ToReadableString(keyboardPressTimeSpan);
            //lblMouseMove.Text = Extensions.ToReadableString(mouseMoveTimeSpan);

            if (mouseMoveTimeSpan.TotalSeconds > Setting.IDLE_TIME_IN_SECONDS &&
                keyboardPressTimeSpan.TotalSeconds > Setting.IDLE_TIME_IN_SECONDS)
            {
                switch (this.WindowState)
                {
                    case FormWindowState.Minimized:
                        this.WindowState = FormWindowState.Normal;
                        RepositionWindowsBottomRight();

                        break;
                    default:
                    case FormWindowState.Normal:
                        //RepositionWindowsBottomRight();
                        break;
                    case FormWindowState.Maximized:

                        break;
                }
                if (_addNewTaskForm == null)
                {

                    this.TopMost = true;
                    this.TopMost = false;
                }
                lock (_dbLock)
                {
                    var inProgress = _userActivityRepository.FindAllInProgressStatus();
                    if (inProgress != null)
                    {
                        foreach (var ip in inProgress)
                        {
                            ip.UpdateStatus(ActivityStatusType.Pause);
                            _userActivityRepository.Update(ip);
                        }
                        _unitOfWork.Commit();
                    }
                }

                RefreshUserActivityList();
            }
            else
            {
                bool isInProgress = false;
                lock (_dbLock)
                {
                    isInProgress = _userActivityRepository.IsInProgress();
                }

                if (!isInProgress)
                {
                    switch (this.WindowState)
                    {
                        case FormWindowState.Normal:
                            //RepositionWindowsBottomRight();
                            break;
                        case FormWindowState.Minimized:
                            this.WindowState = FormWindowState.Normal;
                            RepositionWindowsBottomRight();
                            break;
                        case FormWindowState.Maximized:
                            break;
                        default:
                            break;
                    }

                    if (_addNewTaskForm == null)
                    {
                        this.TopMost = true;
                        this.TopMost = false;
                    }
                }
            }

            //RefreshActiveApp();
        }

        private void btnAddTask_Click(object sender, EventArgs e)
        {
        }

        private void RefreshUserActivityList()
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(delegate { RefreshUserActivityList(); }));
            }

            flowLayoutPanel1.Controls.Clear();

            IEnumerable<UserActivity> userActivityList;
            lock (_dbLock)
                userActivityList = _userActivityRepository.FindAllActiveStatus();

            foreach (var task in userActivityList)
            {
                bool isFound = false;
                foreach (var control in flowLayoutPanel1.Controls)
                {
                    if (control is TaskListUc)
                    {
                        TaskListUc taskList = (TaskListUc)control;
                        if (taskList.Id == task.Id)
                        {
                            isFound = true;
                            break;
                        }
                    }
                }

                if (!isFound)
                {
                    TaskListUc taskList = new TaskListUc(_taskManager, task);
                    taskList.OnAction += TaskList_OnAction;
                    //taskList._task = task;
                    //taskList._taskManager = _taskManager;
                    //taskList.Dock = DockStyle.Top;
                    taskList.RefreshInfo();
                    flowLayoutPanel1.Controls.Add(taskList);
                }

            }


            this.Refresh();
        }

        private void TaskList_OnAction(object sender, TaskListEventArgs e)
        {
            try
            {
                lock (_dbLock)
                {
                    var obj = _userActivityRepository.GetById(e.UserActivity.Id);
                    if (obj != null)
                    {
                        if (e.Action == ActivityStatusType.InProgress)
                        {
                            var inProgress = _userActivityRepository.FindAllInProgressStatus();
                            if (inProgress != null)
                            {
                                foreach (var ip in inProgress)
                                {
                                    ip.UpdateStatus(ActivityStatusType.Pause);
                                    //ip.Status = "pause";
                                    _userActivityRepository.Update(ip);
                                }
                            }
                        }

                        obj.UpdateStatus(e.Action);
                        _userActivityRepository.Update(obj);
                        _unitOfWork.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
            }

            RefreshUserActivityList();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                RefreshUserActivityList();

                _timerCheckActivity = new Timer();
                _timerCheckActivity.Tick += _timer_Tick;
                _timerCheckActivity.Interval = 1000;
                _timerCheckActivity.Start();


                _refreshActiveAppDisplay = new Timer();
                _refreshActiveAppDisplay.Tick += _refreshActiveAppDisplay_Tick;
                _refreshActiveAppDisplay.Interval = 1000 * 5;
                _refreshActiveAppDisplay.Start();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
        }



        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Do you want to close this application?", "Exit", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
            {
                e.Cancel = true;
            }
            else
            {
                ActiveApp latestActivity;
                lock (_dbLock)
                {
                    latestActivity = _activeAppRepository.ByLatestDateTime();
                    if (latestActivity != null && (latestActivity.EndDateTime == DateTime.MinValue))
                        latestActivity.EndDateTime = DateTime.Now;
                    _activeAppRepository.Update(latestActivity);

                    var inProgress = _userActivityRepository.FindAllInProgressStatus();
                    if (inProgress != null)
                    {
                        foreach (var ip in inProgress)
                        {
                            ip.UpdateStatus(ActivityStatusType.Pause);
                            _userActivityRepository.Update(ip);
                        }
                    }
                    _unitOfWork.Commit();
                }

            }
        }

        private void Form1_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                // RepositionWindowsBottomRight();

                //flowLayoutPanel1.Controls.Clear();
                //RefreshList();
            }
        }

        bool _isActivate;
        private void Form1_Activated(object sender, EventArgs e)
        {
            _isActivate = true;
            //flowLayoutPanel1.Controls.Clear();
            //RepositionWindowsBottomRight();

        }

        //private void btnRefreshApp_Click(object sender, EventArgs e)
        //{
        //    var ls = ChromeWindowTitles();
        //    lvTaskList.Items.Clear();
        //    foreach (var l in ls)
        //        lvTaskList.Items.Add(new ListViewItem(new string[] { l }));
        //}

        public IEnumerable<string> ChromeWindowTitles()
        {
            foreach (var title in WindowsByClassFinder.WindowTitlesForClass("Chrome_WidgetWin_0"))
                if (!string.IsNullOrWhiteSpace(title))
                    yield return title;

            foreach (var title in WindowsByClassFinder.WindowTitlesForClass("Chrome_WidgetWin_1"))
                if (!string.IsNullOrWhiteSpace(title))
                    yield return title;
        }




        //public void WinEventProc(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
        //{
        //    string title = GetActiveWindowTitle();
        //    lvTaskList.Items.Insert(0, new ListViewItem(new string[] { title }));

        //}

        private void chkAlwaysOnTop_CheckedChanged(object sender, EventArgs e)
        {
            //if (chkAlwaysOnTop.Checked)
            //    TopMost = true;
            //else
            //    TopMost = false;

        }

        private void txtTaskDescription_TextChanged(object sender, EventArgs e)
        {

        }


        private void Form1_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Normal)
            {
                RepositionWindowsBottomRight();
            }
        }

        private void RepositionWindowsBottomRight()
        {
            Rectangle workingArea = Screen.GetWorkingArea(this);
            if (this.Location.X != workingArea.Right - Size.Width)
                this.Location = new Point(workingArea.Right - Size.Width,
                                          workingArea.Bottom - Size.Height);
        }

        private void btnAddNewTask_Click(object sender, EventArgs e)
        {
            _addNewTaskForm = new AddNewTaskForm();
            if (_addNewTaskForm.ShowDialog() == DialogResult.OK)
            {
                if (!string.IsNullOrEmpty(_addNewTaskForm.Description))
                {
                    lock (_dbLock)
                    {
                        var act = new Models.UserActivity();
                        act.Name = _addNewTaskForm.Description;
                        act.Email = "fakhrulazran@gmail.com";
                        act.UpdateStatus(ActivityStatusType.Pause);
                        _userActivityRepository.Add(act);
                        _unitOfWork.Commit();
                    }
                }
            }
            RefreshUserActivityList();

            _addNewTaskForm = null;
        }

        private void Form1_Deactivate(object sender, EventArgs e)
        {
            _isActivate = false;
        }
    }



}
