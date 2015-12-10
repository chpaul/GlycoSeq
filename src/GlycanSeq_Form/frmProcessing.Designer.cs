namespace GlycanSeq_Form
{
    partial class frmProcessing
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
            this.lblPercentage = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.lblScan = new System.Windows.Forms.Label();
            this.lblCurrentScan = new System.Windows.Forms.Label();
            this.bgWorker_Process = new System.ComponentModel.BackgroundWorker();
            this.label1 = new System.Windows.Forms.Label();
            this.lblRunningTime = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblPercentage
            // 
            this.lblPercentage.AutoSize = true;
            this.lblPercentage.Location = new System.Drawing.Point(474, 40);
            this.lblPercentage.Name = "lblPercentage";
            this.lblPercentage.Size = new System.Drawing.Size(24, 13);
            this.lblPercentage.TabIndex = 7;
            this.lblPercentage.Text = "0 %";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(9, 30);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(459, 23);
            this.progressBar1.TabIndex = 6;
            // 
            // lblScan
            // 
            this.lblScan.AutoSize = true;
            this.lblScan.Location = new System.Drawing.Point(6, 8);
            this.lblScan.Name = "lblScan";
            this.lblScan.Size = new System.Drawing.Size(72, 13);
            this.lblScan.TabIndex = 5;
            this.lblScan.Text = "Current Scan:";
            // 
            // lblCurrentScan
            // 
            this.lblCurrentScan.AutoSize = true;
            this.lblCurrentScan.Location = new System.Drawing.Point(94, 9);
            this.lblCurrentScan.Name = "lblCurrentScan";
            this.lblCurrentScan.Size = new System.Drawing.Size(30, 13);
            this.lblCurrentScan.TabIndex = 4;
            this.lblCurrentScan.Text = "0 / 0";
            // 
            // bgWorker_Process
            // 
            this.bgWorker_Process.WorkerReportsProgress = true;
            this.bgWorker_Process.WorkerSupportsCancellation = true;
            this.bgWorker_Process.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgWorker_Process_DoWork);
            this.bgWorker_Process.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bgWorker_Process_ProgressChanged);
            this.bgWorker_Process.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgWorker_Process_RunWorkerCompleted);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(362, 57);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Running Time(Min):";
            // 
            // lblRunningTime
            // 
            this.lblRunningTime.AutoSize = true;
            this.lblRunningTime.Location = new System.Drawing.Point(467, 57);
            this.lblRunningTime.Name = "lblRunningTime";
            this.lblRunningTime.Size = new System.Drawing.Size(28, 13);
            this.lblRunningTime.TabIndex = 9;
            this.lblRunningTime.Text = "0.00";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(6, 57);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(40, 13);
            this.lblStatus.TabIndex = 10;
            this.lblStatus.Text = "Status:";
            this.lblStatus.Visible = false;
            // 
            // frmProcessing
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(510, 79);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.lblRunningTime);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblPercentage);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.lblScan);
            this.Controls.Add(this.lblCurrentScan);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "frmProcessing";
            this.Text = "Processing";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmProcessing_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmProcessing_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblPercentage;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label lblScan;
        private System.Windows.Forms.Label lblCurrentScan;
        private System.ComponentModel.BackgroundWorker bgWorker_Process;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblRunningTime;
        private System.Windows.Forms.Label lblStatus;
    }
}