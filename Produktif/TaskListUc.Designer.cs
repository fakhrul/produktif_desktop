
namespace Produktif
{
    partial class TaskListUc
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblDesription = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblSpend = new System.Windows.Forms.Label();
            this.btnAction = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblDesription
            // 
            this.lblDesription.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDesription.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblDesription.Location = new System.Drawing.Point(3, 0);
            this.lblDesription.Name = "lblDesription";
            this.lblDesription.Size = new System.Drawing.Size(746, 86);
            this.lblDesription.TabIndex = 0;
            this.lblDesription.Text = "label1";
            // 
            // lblStatus
            // 
            this.lblStatus.Location = new System.Drawing.Point(3, 89);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(320, 25);
            this.lblStatus.TabIndex = 1;
            this.lblStatus.Text = "label2";
            // 
            // lblSpend
            // 
            this.lblSpend.Location = new System.Drawing.Point(3, 123);
            this.lblSpend.Name = "lblSpend";
            this.lblSpend.Size = new System.Drawing.Size(320, 25);
            this.lblSpend.TabIndex = 2;
            this.lblSpend.Text = "label3";
            // 
            // btnAction
            // 
            this.btnAction.Location = new System.Drawing.Point(600, 88);
            this.btnAction.Name = "btnAction";
            this.btnAction.Size = new System.Drawing.Size(149, 60);
            this.btnAction.TabIndex = 3;
            this.btnAction.Text = "Start";
            this.btnAction.UseVisualStyleBackColor = true;
            this.btnAction.Click += new System.EventHandler(this.btnAction_Click);
            // 
            // TaskListUc
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.btnAction);
            this.Controls.Add(this.lblSpend);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.lblDesription);
            this.Name = "TaskListUc";
            this.Size = new System.Drawing.Size(752, 156);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblDesription;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblSpend;
        private System.Windows.Forms.Button btnAction;
    }
}
