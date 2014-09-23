namespace GlycanSeq_Form
{
    partial class frmInvokeProcesses
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblScan = new System.Windows.Forms.Label();
            this.lblCurrentSubProcesses = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblRunningTime = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.bgWorker = new System.ComponentModel.BackgroundWorker();
            this.SuspendLayout();
            // 
            // lblScan
            // 
            this.lblScan.AutoSize = true;
            this.lblScan.Location = new System.Drawing.Point(6, 8);
            this.lblScan.Name = "lblScan";
            this.lblScan.Size = new System.Drawing.Size(114, 13);
            this.lblScan.TabIndex = 5;
            this.lblScan.Text = "Current Subprocesses:";
            // 
            // lblCurrentSubProcesses
            // 
            this.lblCurrentSubProcesses.AutoSize = true;
            this.lblCurrentSubProcesses.Location = new System.Drawing.Point(147, 8);
            this.lblCurrentSubProcesses.Name = "lblCurrentSubProcesses";
            this.lblCurrentSubProcesses.Size = new System.Drawing.Size(30, 13);
            this.lblCurrentSubProcesses.TabIndex = 4;
            this.lblCurrentSubProcesses.Text = "0 / 0";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(327, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Running Time(Min):";
            // 
            // lblRunningTime
            // 
            this.lblRunningTime.AutoSize = true;
            this.lblRunningTime.Location = new System.Drawing.Point(433, 8);
            this.lblRunningTime.Name = "lblRunningTime";
            this.lblRunningTime.Size = new System.Drawing.Size(28, 13);
            this.lblRunningTime.TabIndex = 9;
            this.lblRunningTime.Text = "0.00";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(6, 31);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(40, 13);
            this.lblStatus.TabIndex = 10;
            this.lblStatus.Text = "Status:";
            // 
            // bgWorker
            // 
            this.bgWorker.WorkerReportsProgress = true;
            this.bgWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgWorker_DoWork);
            this.bgWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgWorker_RunWorkerCompleted);
            // 
            // frmInvokeProcesses
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(510, 79);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.lblRunningTime);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblScan);
            this.Controls.Add(this.lblCurrentSubProcesses);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "frmInvokeProcesses";
            this.Text = "Processing";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmProcessing_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblScan;
        private System.Windows.Forms.Label lblCurrentSubProcesses;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblRunningTime;
        private System.Windows.Forms.Label lblStatus;
        private System.ComponentModel.BackgroundWorker bgWorker;
    }
}