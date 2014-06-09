namespace GlycanSeq_Form
{
    partial class frmStart
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
            this.btnBatch = new System.Windows.Forms.Button();
            this.btnSingle = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnBatch
            // 
            this.btnBatch.Location = new System.Drawing.Point(2, 62);
            this.btnBatch.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnBatch.Name = "btnBatch";
            this.btnBatch.Size = new System.Drawing.Size(200, 50);
            this.btnBatch.TabIndex = 1;
            this.btnBatch.Text = "Batch Mode";
            this.btnBatch.UseVisualStyleBackColor = true;
            this.btnBatch.Click += new System.EventHandler(this.btnBatch_Click);
            // 
            // btnSingle
            // 
            this.btnSingle.Location = new System.Drawing.Point(2, 2);
            this.btnSingle.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnSingle.Name = "btnSingle";
            this.btnSingle.Size = new System.Drawing.Size(200, 50);
            this.btnSingle.TabIndex = 0;
            this.btnSingle.Text = "Single Mode";
            this.btnSingle.UseVisualStyleBackColor = true;
            this.btnSingle.Click += new System.EventHandler(this.btnSingle_Click);
            // 
            // frmStart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(204, 115);
            this.Controls.Add(this.btnSingle);
            this.Controls.Add(this.btnBatch);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmStart";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Glycoseq";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnBatch;
        private System.Windows.Forms.Button btnSingle;
    }
}