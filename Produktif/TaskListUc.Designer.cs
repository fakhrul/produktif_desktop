
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
            this.lblSpend = new System.Windows.Forms.Label();
            this.btnAction = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblDesription
            // 
            this.lblDesription.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDesription.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblDesription.Location = new System.Drawing.Point(3, 3);
            this.lblDesription.Name = "lblDesription";
            this.lblDesription.Size = new System.Drawing.Size(350, 62);
            this.lblDesription.TabIndex = 0;
            this.lblDesription.Text = "label1";
            // 
            // lblSpend
            // 
            this.lblSpend.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblSpend.Location = new System.Drawing.Point(3, 67);
            this.lblSpend.Name = "lblSpend";
            this.lblSpend.Size = new System.Drawing.Size(320, 16);
            this.lblSpend.TabIndex = 2;
            this.lblSpend.Text = "label3";
            // 
            // btnAction
            // 
            this.btnAction.Location = new System.Drawing.Point(359, 3);
            this.btnAction.Name = "btnAction";
            this.btnAction.Size = new System.Drawing.Size(80, 80);
            this.btnAction.TabIndex = 3;
            this.btnAction.Text = "Start";
            this.btnAction.UseVisualStyleBackColor = true;
            this.btnAction.Click += new System.EventHandler(this.btnAction_Click);
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(445, 3);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(80, 80);
            this.btnStop.TabIndex = 4;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // TaskListUc
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnAction);
            this.Controls.Add(this.lblSpend);
            this.Controls.Add(this.lblDesription);
            this.Name = "TaskListUc";
            this.Size = new System.Drawing.Size(530, 88);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblDesription;
        private System.Windows.Forms.Label lblSpend;
        private System.Windows.Forms.Button btnAction;
        private System.Windows.Forms.Button btnStop;
    }
}
