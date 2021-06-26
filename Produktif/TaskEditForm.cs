using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Produktif
{
    public partial class TaskEditForm : Form
    {
        public bool IsRemove { get; set; }
        public TaskEditForm()
        {
            InitializeComponent();
            UpdateStatusList();
            IsRemove = false;
        }

        private void UpdateStatusList()
        {
            foreach (var status in Enum.GetValues(typeof(Status)))
            {
                comboBox1.Items.Add(status);
            }
        }

        public Task Task { get; internal set; }
        public TaskManager TaskManager { get; internal set; }

        private void TaskEditForm_Load(object sender, EventArgs e)
        {
            lblDescription.Text = Task.Description;
            comboBox1.SelectedItem = Task.CurrentStatus;

            switch (Task.CurrentStatus)
            {
                case Status.InProgress:
                    btnAction.Text = "Stop";
                    break;
                case Status.Pause:
                    btnAction.Text = "Start";
                    break;
                case Status.Done:
                    btnAction.Text = "Start";
                    break;
                case Status.Cancelled:
                    btnAction.Text = "Start";
                    break;
                default:
                    break;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                var status = (Status)comboBox1.SelectedItem;
                TaskManager.UpdateTaskStatus(Task, status);
                //Task.UpdateStatus(status);
                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Are you sure to remove this item", "Remove", MessageBoxButtons.YesNo) != DialogResult.Yes)
            {
                return;
            }
            IsRemove = true;
            DialogResult = DialogResult.OK;
        }

        private void btnAction_Click(object sender, EventArgs e)
        {
            if (btnAction.Text.ToLower() == "start")
            {
                TaskManager.UpdateTaskStatus(Task, Status.InProgress);
                //Task.UpdateStatus(Status.InProgress);
            }
            else
            {
                TaskManager.UpdateTaskStatus(Task, Status.Pause);
                //Task.UpdateStatus(Status.Pause);
            }
            DialogResult = DialogResult.OK;
        }
    }
}
