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
        TaskManager _taskManager;
        string FILE_NAME = "data.produktif";
        System.Windows.Forms.Timer _timer;

        WindowHelper _windowHelper;
        //WinEventDelegate dele = null;

        public Form1()
        {
            InitializeComponent();

            //ListViewExtender extender = new ListViewExtender(lvTaskList);
            //ListViewButtonColumn buttonAction = new ListViewButtonColumn(5);
            //buttonAction.Click += OnButtonActionClick;
            //buttonAction.FixedWidth = true;
            //extender.AddColumn(buttonAction);

            _taskManager = new TaskManager();
            _taskManager.TaskManagerInfoUpdate += _taskManager_TaskManagerInfoUpdate;

            _timer = new Timer();
            _timer.Tick += _timer_Tick;
            _timer.Interval = 1000;
            _timer.Start();

            _windowHelper = new WindowHelper();
            _windowHelper.OnActiveWindowUpdate += _windowHelper_OnActiveWindowUpdate;
            //dele = new WinEventDelegate(WinEventProc);
            //IntPtr m_hhook = SetWinEventHook(EVENT_SYSTEM_FOREGROUND, EVENT_SYSTEM_FOREGROUND, IntPtr.Zero, dele, 0, 0, WINEVENT_OUTOFCONTEXT);

        }

        private void _windowHelper_OnActiveWindowUpdate(object sender, ActiveWindowUpdateEventArgs e)
        {
            InsertList(e.ActiveWindow);
        }

        private void InsertList(ActiveWindow e)
        {
            if(InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    InsertList(e);
                });

                return;
            }
            lvTaskList.Items.Insert(0, new ListViewItem(new string[] { e.CheckDateTime.ToString("HH:mm:ss"),
            e.Process,
            e.Title,
            e.ClassName,
            e.Text,
            e.Url
            }));
        }

        //private void OnButtonActionClick(object sender, ListViewColumnMouseEventArgs e)
        //{
        //    //MessageBox.Show(this, @"you clicked " + e.SubItem.Text);
        //    if (e.SubItem.Text.ToLower() == "start")
        //    {
        //        _taskManager.UpdateTaskStatus(e.Item.Text, Status.InProgress);
        //        //Task.UpdateStatus(Status.InProgress);
        //    }
        //    else
        //    {
        //        _taskManager.UpdateTaskStatus(e.Item.Text, Status.Pause);
        //        //Task.UpdateStatus(Status.Pause);
        //    }

        //    RefreshList();
        //}

        private void _taskManager_TaskManagerInfoUpdate(object sender, TaskManagerInfoUpdateEventArgs e)
        {
            RefreshList();
        }

        private void _timer_Tick(object sender, EventArgs e)
        {
            DateTime current = DateTime.Now;
            var keyboardPressTimeSpan = current - _taskManager.LastKeyboardPress;
            var mouseMoveTimeSpan = current - _taskManager.LastMouseMoved;

            lblKeyPress.Text = UtilString.ToReadableString(keyboardPressTimeSpan);
            lblMouseMove.Text = UtilString.ToReadableString(mouseMoveTimeSpan);

            if (mouseMoveTimeSpan.TotalSeconds > Setting.IDLE_TIME_IN_SECONDS &&
                keyboardPressTimeSpan.TotalSeconds > Setting.IDLE_TIME_IN_SECONDS)
            {
                switch (this.WindowState)
                {
                    case FormWindowState.Normal:
                        break;
                    case FormWindowState.Minimized:
                        this.WindowState = FormWindowState.Normal;
                        break;
                    case FormWindowState.Maximized:
                        break;
                    default:
                        break;
                }
                if (chkAlwaysOnTop.Checked)
                {
                    this.TopMost = true;
                    this.TopMost = false;
                }
                _taskManager.UpdateLatestStatus(Status.Pause);
                RefreshList();
            }
            else
            {
                if (!_taskManager.IsStatusInProgress())
                {
                    switch (this.WindowState)
                    {
                        case FormWindowState.Normal:
                            break;
                        case FormWindowState.Minimized:
                            this.WindowState = FormWindowState.Normal;
                            break;
                        case FormWindowState.Maximized:
                            break;
                        default:
                            break;
                    }
                    if (chkAlwaysOnTop.Checked)
                    {
                        this.TopMost = true;
                        this.TopMost = false;
                    }
                }
            }
        }

        private void btnAddTask_Click(object sender, EventArgs e)
        {
        }

        private void RefreshList()
        {
            try
            {
                Save();

                //UpdateListView();
                UpdateTaskUc();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void UpdateTaskUc()
        {
            //lvTaskList.Items.Clear();
            var tasks = _taskManager.GetTaskList().OrderByDescending(c => c.LastUpdateDateTime);

            //RemoveTask();

            foreach (var task in tasks)
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
                    //taskList._task = task;
                    //taskList._taskManager = _taskManager;
                    //taskList.Dock = DockStyle.Top;
                    taskList.RefreshInfo();
                    flowLayoutPanel1.Controls.Add(taskList);
                }



                //string nextAction = "";
                //switch (task.CurrentStatus)
                //{
                //    case Status.InProgress:
                //        nextAction = "Stop";
                //        break;
                //    case Status.Pause:
                //        nextAction = "Start";
                //        break;
                //    case Status.Done:
                //        nextAction = "Start";
                //        break;
                //    default:
                //    case Status.Cancelled:
                //        nextAction = "Start";
                //        break;
                //}

                //ListViewItem lvi = new ListViewItem(new string[]
                //{
                //        task.Id,
                //        task.Created.ToString("dd/MM/yyyy HH:mm:ss"),
                //        task.Description,
                //        ToReadableString(task.TotalSpend),
                //        task.CurrentStatus.ToString(),
                //        nextAction
                //});

                //if (task.CurrentStatus == Status.InProgress)
                //    lvi.ForeColor = Color.DarkGreen;
                //else
                //    lvi.ForeColor = Color.LightGray;

                //lvi.Tag = task;

                //lvTaskList.Items.Add(lvi);
            }
        }

        //private void RemoveTask()
        //{
        //    List<TaskListUc> listOfControlToRemove = new List<TaskListUc>();
        //    foreach (var control in flowLayoutPanel1.Controls)
        //    {
        //        if (control is TaskListUc)
        //        {
        //            TaskListUc taskList = (TaskListUc)control;
        //            foreach (var task in _taskManager.GetTaskList())
        //            {
        //                if (taskList.Task.Id == task.Id)
        //                {
        //                    listOfControlToRemove.Add(taskList);
        //                    break;
        //                }
        //            }
        //        }
        //    }

        //    foreach (var taskUc in listOfControlToRemove)
        //    {
        //        flowLayoutPanel1.Controls.Remove(taskUc);
        //    }
        //}

        private void UpdateListView()
        {
            lvTaskList.Items.Clear();
            foreach (var task in _taskManager.GetTaskList())
            {
                string nextAction = "";
                switch (task.CurrentStatus)
                {
                    case Status.InProgress:
                        nextAction = "Stop";
                        break;
                    case Status.Pause:
                        nextAction = "Start";
                        break;
                    case Status.Done:
                        nextAction = "Start";
                        break;
                    default:
                    case Status.Cancelled:
                        nextAction = "Start";
                        break;
                }

                ListViewItem lvi = new ListViewItem(new string[]
                {
                        task.Id,
                        task.Created.ToString("dd/MM/yyyy HH:mm:ss"),
                        task.Description,
                        UtilString.ToReadableString(task.TotalSpend),
                        task.CurrentStatus.ToString(),
                        nextAction
                });

                if (task.CurrentStatus == Status.InProgress)
                    lvi.ForeColor = Color.DarkGreen;
                else
                    lvi.ForeColor = Color.LightGray;

                lvi.Tag = task;

                lvTaskList.Items.Add(lvi);
            }
        }

        private void lvTaskList_Click(object sender, EventArgs e)
        {
            return;
            try
            {
                var selectedItem = lvTaskList.SelectedItems[0];
                TaskEditForm edit = new TaskEditForm();
                edit.Task = (Task)selectedItem.Tag;
                edit.TaskManager = _taskManager;
                var result = edit.ShowDialog();
                switch (result)
                {
                    case DialogResult.None:
                        break;
                    case DialogResult.OK:
                        break;
                    case DialogResult.Cancel:

                        break;
                    case DialogResult.Abort:
                        break;
                    case DialogResult.Retry:
                        break;
                    case DialogResult.Ignore:
                        break;
                    case DialogResult.Yes:
                        break;
                    case DialogResult.No:
                        break;
                    default:
                        break;
                }

                if (edit.IsRemove)
                    _taskManager.RemoveTask(edit.Task);

                RefreshList();


            }
            catch (Exception ex)
            {
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                _taskManager = DeSerializeObject<TaskManager>(FILE_NAME);
            }
            catch (Exception ex)
            {
                _taskManager = new TaskManager();
            }
            RefreshList();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void Save()
        {
            try
            {
                SerializeObject<TaskManager>(_taskManager, FILE_NAME);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        public void SerializeObject<T>(T serializableObject, string fileName)
        {
            if (serializableObject == null) { return; }

            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                XmlSerializer serializer = new XmlSerializer(serializableObject.GetType());
                using (MemoryStream stream = new MemoryStream())
                {
                    serializer.Serialize(stream, serializableObject);
                    stream.Position = 0;
                    xmlDocument.Load(stream);
                    xmlDocument.Save(fileName);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public T DeSerializeObject<T>(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) { return default(T); }

            T objectOut = default(T);

            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(fileName);
                string xmlString = xmlDocument.OuterXml;

                using (StringReader read = new StringReader(xmlString))
                {
                    Type outType = typeof(T);

                    XmlSerializer serializer = new XmlSerializer(outType);
                    using (XmlReader reader = new XmlTextReader(read))
                    {
                        objectOut = (T)serializer.Deserialize(reader);
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return objectOut;
        }

        private void txtTaskDescription_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                _taskManager.AddTask(new Task { Description = txtTaskDescription.Text });
                RefreshList();

                txtTaskDescription.Text = "";
            }
        }

        private void txtTaskDescription_Click(object sender, EventArgs e)
        {
            txtTaskDescription.Text = "";
        }


        //public  string ToReadableString(TimeSpan span)
        //{
        //    string formatted = string.Format("{0}{1}{2}{3}",
        //        span.Duration().Days > 0 ? string.Format("{0:0} d, ", span.Days) : string.Empty,
        //        span.Duration().Hours > 0 ? string.Format("{0:0} h, ", span.Hours) : string.Empty,
        //        span.Duration().Minutes > 0 ? string.Format("{0:0} m, ", span.Minutes) : string.Empty,
        //        span.Duration().Seconds > 0 ? string.Format("{0:0} s", span.Seconds) : string.Empty);

        //    if (formatted.EndsWith(", ")) formatted = formatted.Substring(0, formatted.Length - 2);

        //    if (string.IsNullOrEmpty(formatted)) formatted = "0 s";

        //    return formatted;
        //}

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Do you want to close this application?", "Exit", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
            {
                e.Cancel = true;
            }
            else
            {
                _taskManager.UpdateLatestStatus(Status.Pause);
            }
        }

        private void Form1_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                //flowLayoutPanel1.Controls.Clear();
                //RefreshList();
            }
        }

        private void Form1_Activated(object sender, EventArgs e)
        {
            flowLayoutPanel1.Controls.Clear();
            RefreshList();
        }

        private void btnRefreshApp_Click(object sender, EventArgs e)
        {
            var ls = ChromeWindowTitles();
            lvTaskList.Items.Clear();
            foreach (var l in ls)
                lvTaskList.Items.Add(new ListViewItem(new string[] { l }));
        }

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
            if (chkAlwaysOnTop.Checked)
                TopMost = true;
            else
                TopMost = false;

        }
    }



}
