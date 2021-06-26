using System;
using System.Collections.Generic;
using System.Text;

namespace Produktif
{
    public class TaskManager
    {
        private KeyboardInput keyboard;
        private MouseInput mouse;
        public DateTime LastMouseMoved { get; set; }
        public DateTime LastKeyboardPress { get; set; }

        public List<Task> TaskList
        {
            get
            {
                return _taskList;
            }
        }
        private List<Task> _taskList = new List<Task>();

        public event EventHandler<TaskManagerInfoUpdateEventArgs> TaskManagerInfoUpdate;
        public TaskManager()
        {
            if (LastMouseMoved == null || LastMouseMoved == DateTime.MinValue)
                LastMouseMoved = DateTime.Now;
            if (LastKeyboardPress == null || LastKeyboardPress == DateTime.MinValue)
                LastKeyboardPress = DateTime.Now;

            keyboard = new KeyboardInput();
            keyboard.KeyBoardKeyPressed += keyboard_KeyBoardKeyPressed;

            mouse = new MouseInput();
            mouse.MouseMoved += mouse_MouseMoved;
        }

        private void mouse_MouseMoved(object sender, EventArgs e)
        {
            LastMouseMoved = DateTime.Now;
        }

        private void keyboard_KeyBoardKeyPressed(object sender, EventArgs e)
        {
            LastKeyboardPress = DateTime.Now;
        }

        //internal void UpdateLatestStatus(Status status)
        //{
        //    foreach (var task in _taskList)
        //    {
        //        if (task.CurrentStatus == Status.InProgress)
        //            task.UpdateStatus(status);
        //    }

        //    SendTaskManagerInfoUpdate();
        //}

        internal bool IsStatusInProgress()
        {
            foreach (var task in _taskList)
            {
                if (task.CurrentStatus == Status.InProgress)
                {
                    return true;
                }
            }
            return false;
        }

        //internal void UpdateTaskStatus(string id, Status status)
        //{
        //    foreach (var task in TaskList)
        //    {
        //        if (task.Id == id)
        //        {
        //            task.UpdateStatus(status);
        //            break;
        //        }
        //    }
        //    SendTaskManagerInfoUpdate();
        //}

        //internal void UpdateTaskStatus(Task taskToModify, Status status)
        //{
        //    foreach (var task in TaskList)
        //    {
        //        if(task.CurrentStatus == Status.InProgress && !task.Equals(taskToModify))
        //        {
        //            task.UpdateStatus(Status.Pause);
        //        }
        //    }

        //    taskToModify.UpdateStatus(status);
        //}

        internal List<Task> GetTaskList()
        {
            return _taskList;
        }

        internal void RemoveTask(Task task)
        {
            _taskList.Remove(task);
            SendTaskManagerInfoUpdate();
        }

        internal void AddTask(Task task)
        {
            _taskList.Add(task);
            SendTaskManagerInfoUpdate();
        }

        private void SendTaskManagerInfoUpdate()
        {
            if (TaskManagerInfoUpdate != null)
                TaskManagerInfoUpdate(this, new TaskManagerInfoUpdateEventArgs());
        }
    }


    public class TaskManagerInfoUpdateEventArgs : EventArgs
    {

    }
}
