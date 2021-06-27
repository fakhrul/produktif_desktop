using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Produktif
{
    public partial class AddNewTaskForm : Form
    {
        public string Description { get; set; }
        public AddNewTaskForm()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void txtDescription_TextChanged(object sender, EventArgs e)
        {
            Description = txtDescription.Text;
        }
    }
}
