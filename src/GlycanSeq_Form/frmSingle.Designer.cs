namespace GlycanSeq_Form
{
    partial class frmSingle
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btnLoad = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.txtScanNo = new System.Windows.Forms.TextBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer5 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.lstPeak = new System.Windows.Forms.ListBox();
            this.chkAVGMass = new System.Windows.Forms.CheckBox();
            this.lblMass = new System.Windows.Forms.Label();
            this.grpGlycan = new System.Windows.Forms.GroupBox();
            this.rdoNeuAc = new System.Windows.Forms.RadioButton();
            this.rdoNeuGc = new System.Windows.Forms.RadioButton();
            this.txtHexNAc = new System.Windows.Forms.TextBox();
            this.txtdeHex = new System.Windows.Forms.TextBox();
            this.txtSia = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtHex = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.chkNLinked = new System.Windows.Forms.CheckBox();
            this.txtPeptideSeq = new System.Windows.Forms.TextBox();
            this.rdoPeptide = new System.Windows.Forms.RadioButton();
            this.rdoY1 = new System.Windows.Forms.RadioButton();
            this.label4 = new System.Windows.Forms.Label();
            this.lblPPM = new System.Windows.Forms.Label();
            this.txtPeaKTol = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtPrecusorTol = new System.Windows.Forms.TextBox();
            this.lblY1Z = new System.Windows.Forms.Label();
            this.txtY1ChargeStatus = new System.Windows.Forms.TextBox();
            this.lblScan = new System.Windows.Forms.Label();
            this.txtY1 = new System.Windows.Forms.TextBox();
            this.dgDetail = new System.Windows.Forms.DataGridView();
            this.cboScan = new System.Windows.Forms.ComboBox();
            this.dgView = new System.Windows.Forms.DataGridView();
            this.dgIDPeak = new System.Windows.Forms.DataGridView();
            this.zedSequence = new ZedGraph.ZedGraphControl();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.splitContainer5.Panel1.SuspendLayout();
            this.splitContainer5.Panel2.SuspendLayout();
            this.splitContainer5.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.grpGlycan.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgIDPeak)).BeginInit();
            this.SuspendLayout();
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(357, 88);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(69, 25);
            this.btnLoad.TabIndex = 8;
            this.btnLoad.Text = "Load";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // txtScanNo
            // 
            this.txtScanNo.Location = new System.Drawing.Point(77, 4);
            this.txtScanNo.Name = "txtScanNo";
            this.txtScanNo.Size = new System.Drawing.Size(73, 20);
            this.txtScanNo.TabIndex = 0;
            this.txtScanNo.Text = "1232";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer5);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.zedSequence);
            this.splitContainer1.Size = new System.Drawing.Size(1016, 803);
            this.splitContainer1.SplitterDistance = 392;
            this.splitContainer1.TabIndex = 3;
            // 
            // splitContainer5
            // 
            this.splitContainer5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer5.Location = new System.Drawing.Point(0, 0);
            this.splitContainer5.Name = "splitContainer5";
            // 
            // splitContainer5.Panel1
            // 
            this.splitContainer5.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer5.Panel2
            // 
            this.splitContainer5.Panel2.Controls.Add(this.dgIDPeak);
            this.splitContainer5.Size = new System.Drawing.Size(1016, 392);
            this.splitContainer5.SplitterDistance = 754;
            this.splitContainer5.TabIndex = 5;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.tabControl1);
            this.splitContainer2.Panel1.Controls.Add(this.chkAVGMass);
            this.splitContainer2.Panel1.Controls.Add(this.lblMass);
            this.splitContainer2.Panel1.Controls.Add(this.grpGlycan);
            this.splitContainer2.Panel1.Controls.Add(this.txtPeptideSeq);
            this.splitContainer2.Panel1.Controls.Add(this.rdoPeptide);
            this.splitContainer2.Panel1.Controls.Add(this.rdoY1);
            this.splitContainer2.Panel1.Controls.Add(this.label4);
            this.splitContainer2.Panel1.Controls.Add(this.lblPPM);
            this.splitContainer2.Panel1.Controls.Add(this.txtPeaKTol);
            this.splitContainer2.Panel1.Controls.Add(this.label5);
            this.splitContainer2.Panel1.Controls.Add(this.txtPrecusorTol);
            this.splitContainer2.Panel1.Controls.Add(this.lblY1Z);
            this.splitContainer2.Panel1.Controls.Add(this.txtY1ChargeStatus);
            this.splitContainer2.Panel1.Controls.Add(this.lblScan);
            this.splitContainer2.Panel1.Controls.Add(this.txtY1);
            this.splitContainer2.Panel1.Controls.Add(this.txtScanNo);
            this.splitContainer2.Panel1.Controls.Add(this.btnLoad);
            this.splitContainer2.Panel1.Controls.Add(this.dgDetail);
            this.splitContainer2.Panel1.Controls.Add(this.cboScan);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.dgView);
            this.splitContainer2.Size = new System.Drawing.Size(754, 392);
            this.splitContainer2.SplitterDistance = 147;
            this.splitContainer2.TabIndex = 4;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(446, 4);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(306, 141);
            this.tabControl1.TabIndex = 33;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.treeView1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(298, 115);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // treeView1
            // 
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.Location = new System.Drawing.Point(3, 3);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(292, 109);
            this.treeView1.TabIndex = 1;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.lstPeak);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(298, 115);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // lstPeak
            // 
            this.lstPeak.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstPeak.FormattingEnabled = true;
            this.lstPeak.Location = new System.Drawing.Point(3, 3);
            this.lstPeak.Name = "lstPeak";
            this.lstPeak.Size = new System.Drawing.Size(292, 109);
            this.lstPeak.TabIndex = 32;
            // 
            // chkAVGMass
            // 
            this.chkAVGMass.AutoSize = true;
            this.chkAVGMass.Checked = true;
            this.chkAVGMass.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAVGMass.Location = new System.Drawing.Point(357, 125);
            this.chkAVGMass.Name = "chkAVGMass";
            this.chkAVGMass.Size = new System.Drawing.Size(94, 17);
            this.chkAVGMass.TabIndex = 11;
            this.chkAVGMass.Text = "Average Mass";
            this.chkAVGMass.UseVisualStyleBackColor = true;
            // 
            // lblMass
            // 
            this.lblMass.AutoSize = true;
            this.lblMass.Location = new System.Drawing.Point(367, 107);
            this.lblMass.Name = "lblMass";
            this.lblMass.Size = new System.Drawing.Size(0, 13);
            this.lblMass.TabIndex = 31;
            // 
            // grpGlycan
            // 
            this.grpGlycan.Controls.Add(this.rdoNeuAc);
            this.grpGlycan.Controls.Add(this.rdoNeuGc);
            this.grpGlycan.Controls.Add(this.txtHexNAc);
            this.grpGlycan.Controls.Add(this.txtdeHex);
            this.grpGlycan.Controls.Add(this.txtSia);
            this.grpGlycan.Controls.Add(this.label3);
            this.grpGlycan.Controls.Add(this.txtHex);
            this.grpGlycan.Controls.Add(this.label1);
            this.grpGlycan.Controls.Add(this.label2);
            this.grpGlycan.Controls.Add(this.chkNLinked);
            this.grpGlycan.Location = new System.Drawing.Point(166, 4);
            this.grpGlycan.Name = "grpGlycan";
            this.grpGlycan.Size = new System.Drawing.Size(185, 90);
            this.grpGlycan.TabIndex = 30;
            this.grpGlycan.TabStop = false;
            this.grpGlycan.Text = "Contain";
            // 
            // rdoNeuAc
            // 
            this.rdoNeuAc.AutoSize = true;
            this.rdoNeuAc.Checked = true;
            this.rdoNeuAc.Location = new System.Drawing.Point(94, 16);
            this.rdoNeuAc.Name = "rdoNeuAc";
            this.rdoNeuAc.Size = new System.Drawing.Size(58, 17);
            this.rdoNeuAc.TabIndex = 11;
            this.rdoNeuAc.TabStop = true;
            this.rdoNeuAc.Text = "NeuAc";
            this.rdoNeuAc.UseVisualStyleBackColor = true;
            // 
            // rdoNeuGc
            // 
            this.rdoNeuGc.AutoSize = true;
            this.rdoNeuGc.Location = new System.Drawing.Point(94, 39);
            this.rdoNeuGc.Name = "rdoNeuGc";
            this.rdoNeuGc.Size = new System.Drawing.Size(59, 17);
            this.rdoNeuGc.TabIndex = 10;
            this.rdoNeuGc.Text = "NeuGc";
            this.rdoNeuGc.UseVisualStyleBackColor = true;
            // 
            // txtHexNAc
            // 
            this.txtHexNAc.Location = new System.Drawing.Point(54, 16);
            this.txtHexNAc.Name = "txtHexNAc";
            this.txtHexNAc.Size = new System.Drawing.Size(26, 20);
            this.txtHexNAc.TabIndex = 4;
            this.txtHexNAc.Text = "99";
            // 
            // txtdeHex
            // 
            this.txtdeHex.Location = new System.Drawing.Point(54, 62);
            this.txtdeHex.Name = "txtdeHex";
            this.txtdeHex.Size = new System.Drawing.Size(26, 20);
            this.txtdeHex.TabIndex = 6;
            this.txtdeHex.Text = "0";
            // 
            // txtSia
            // 
            this.txtSia.Location = new System.Drawing.Point(154, 21);
            this.txtSia.Name = "txtSia";
            this.txtSia.Size = new System.Drawing.Size(25, 20);
            this.txtSia.TabIndex = 7;
            this.txtSia.Text = "99";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 66);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "deHex";
            // 
            // txtHex
            // 
            this.txtHex.Location = new System.Drawing.Point(54, 36);
            this.txtHex.Name = "txtHex";
            this.txtHex.Size = new System.Drawing.Size(25, 20);
            this.txtHex.TabIndex = 5;
            this.txtHex.Text = "99";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(26, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Hex";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(1, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "HexNAc";
            // 
            // chkNLinked
            // 
            this.chkNLinked.AutoSize = true;
            this.chkNLinked.Checked = true;
            this.chkNLinked.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkNLinked.Location = new System.Drawing.Point(94, 67);
            this.chkNLinked.Name = "chkNLinked";
            this.chkNLinked.Size = new System.Drawing.Size(69, 17);
            this.chkNLinked.TabIndex = 10;
            this.chkNLinked.Text = "N-Linked";
            this.chkNLinked.UseVisualStyleBackColor = true;
            // 
            // txtPeptideSeq
            // 
            this.txtPeptideSeq.Enabled = false;
            this.txtPeptideSeq.Location = new System.Drawing.Point(77, 100);
            this.txtPeptideSeq.Name = "txtPeptideSeq";
            this.txtPeptideSeq.Size = new System.Drawing.Size(274, 20);
            this.txtPeptideSeq.TabIndex = 3;
            this.txtPeptideSeq.Text = "NAGSGIIISDTPVHDCNTTCQTPK\t";
            this.txtPeptideSeq.TextChanged += new System.EventHandler(this.txtPeptideSeq_TextChanged);
            // 
            // rdoPeptide
            // 
            this.rdoPeptide.AutoSize = true;
            this.rdoPeptide.Location = new System.Drawing.Point(13, 102);
            this.rdoPeptide.Name = "rdoPeptide";
            this.rdoPeptide.Size = new System.Drawing.Size(64, 17);
            this.rdoPeptide.TabIndex = 2;
            this.rdoPeptide.TabStop = true;
            this.rdoPeptide.Text = "Peptide:";
            this.rdoPeptide.UseVisualStyleBackColor = true;
            // 
            // rdoY1
            // 
            this.rdoY1.AutoSize = true;
            this.rdoY1.Checked = true;
            this.rdoY1.Location = new System.Drawing.Point(12, 70);
            this.rdoY1.Name = "rdoY1";
            this.rdoY1.Size = new System.Drawing.Size(65, 17);
            this.rdoY1.TabIndex = 2;
            this.rdoY1.TabStop = true;
            this.rdoY1.Text = "Y1(m/z):";
            this.rdoY1.UseVisualStyleBackColor = true;
            this.rdoY1.CheckedChanged += new System.EventHandler(this.rdoY1_CheckedChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(176, 126);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(138, 13);
            this.label4.TabIndex = 28;
            this.label4.Text = "Precursor Tolerance (PPM):";
            // 
            // lblPPM
            // 
            this.lblPPM.AutoSize = true;
            this.lblPPM.Location = new System.Drawing.Point(357, 8);
            this.lblPPM.Name = "lblPPM";
            this.lblPPM.Size = new System.Drawing.Size(33, 13);
            this.lblPPM.TabIndex = 1;
            this.lblPPM.Text = "PPM:";
            // 
            // txtPeaKTol
            // 
            this.txtPeaKTol.Location = new System.Drawing.Point(133, 124);
            this.txtPeaKTol.Name = "txtPeaKTol";
            this.txtPeaKTol.Size = new System.Drawing.Size(28, 20);
            this.txtPeaKTol.TabIndex = 9;
            this.txtPeaKTol.Text = "0.8";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 126);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(106, 13);
            this.label5.TabIndex = 29;
            this.label5.Text = "Peak Tolerance(Da):";
            // 
            // txtPrecusorTol
            // 
            this.txtPrecusorTol.Location = new System.Drawing.Point(323, 123);
            this.txtPrecusorTol.Name = "txtPrecusorTol";
            this.txtPrecusorTol.Size = new System.Drawing.Size(28, 20);
            this.txtPrecusorTol.TabIndex = 10;
            this.txtPrecusorTol.Text = "10";
            // 
            // lblY1Z
            // 
            this.lblY1Z.AutoSize = true;
            this.lblY1Z.Location = new System.Drawing.Point(44, 40);
            this.lblY1Z.Name = "lblY1Z";
            this.lblY1Z.Size = new System.Drawing.Size(33, 13);
            this.lblY1Z.TabIndex = 10;
            this.lblY1Z.Text = "Y1 Z:";
            // 
            // txtY1ChargeStatus
            // 
            this.txtY1ChargeStatus.Location = new System.Drawing.Point(77, 36);
            this.txtY1ChargeStatus.Name = "txtY1ChargeStatus";
            this.txtY1ChargeStatus.Size = new System.Drawing.Size(73, 20);
            this.txtY1ChargeStatus.TabIndex = 1;
            this.txtY1ChargeStatus.Text = "2";
            this.txtY1ChargeStatus.TextChanged += new System.EventHandler(this.txtPeptideSeq_TextChanged);
            // 
            // lblScan
            // 
            this.lblScan.AutoSize = true;
            this.lblScan.Location = new System.Drawing.Point(32, 8);
            this.lblScan.Name = "lblScan";
            this.lblScan.Size = new System.Drawing.Size(45, 13);
            this.lblScan.TabIndex = 3;
            this.lblScan.Text = "Scan #:";
            this.lblScan.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // txtY1
            // 
            this.txtY1.Location = new System.Drawing.Point(77, 68);
            this.txtY1.Name = "txtY1";
            this.txtY1.Size = new System.Drawing.Size(73, 20);
            this.txtY1.TabIndex = 3;
            // 
            // dgDetail
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgDetail.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgDetail.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgDetail.Enabled = false;
            this.dgDetail.Location = new System.Drawing.Point(560, 3);
            this.dgDetail.Name = "dgDetail";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgDetail.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgDetail.Size = new System.Drawing.Size(269, 144);
            this.dgDetail.TabIndex = 0;
            this.dgDetail.Visible = false;
            // 
            // cboScan
            // 
            this.cboScan.FormattingEnabled = true;
            this.cboScan.Location = new System.Drawing.Point(631, 123);
            this.cboScan.Name = "cboScan";
            this.cboScan.Size = new System.Drawing.Size(121, 21);
            this.cboScan.TabIndex = 11;
            this.cboScan.Visible = false;
            this.cboScan.SelectedIndexChanged += new System.EventHandler(this.cboScan_SelectedIndexChanged);
            // 
            // dgView
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dgView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgView.DefaultCellStyle = dataGridViewCellStyle5;
            this.dgView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgView.Location = new System.Drawing.Point(0, 0);
            this.dgView.Name = "dgView";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgView.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.dgView.RowTemplate.Height = 20;
            this.dgView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgView.Size = new System.Drawing.Size(754, 241);
            this.dgView.TabIndex = 0;
            this.dgView.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgView_CellClick);
            this.dgView.Sorted += new System.EventHandler(this.dgView_Sorted);
            // 
            // dgIDPeak
            // 
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgIDPeak.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.dgIDPeak.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgIDPeak.DefaultCellStyle = dataGridViewCellStyle8;
            this.dgIDPeak.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgIDPeak.Location = new System.Drawing.Point(0, 0);
            this.dgIDPeak.Name = "dgIDPeak";
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgIDPeak.RowHeadersDefaultCellStyle = dataGridViewCellStyle9;
            this.dgIDPeak.Size = new System.Drawing.Size(258, 392);
            this.dgIDPeak.TabIndex = 0;
            // 
            // zedSequence
            // 
            this.zedSequence.Dock = System.Windows.Forms.DockStyle.Fill;
            this.zedSequence.Location = new System.Drawing.Point(0, 0);
            this.zedSequence.Name = "zedSequence";
            this.zedSequence.ScrollGrace = 0D;
            this.zedSequence.ScrollMaxX = 0D;
            this.zedSequence.ScrollMaxY = 0D;
            this.zedSequence.ScrollMaxY2 = 0D;
            this.zedSequence.ScrollMinX = 0D;
            this.zedSequence.ScrollMinY = 0D;
            this.zedSequence.ScrollMinY2 = 0D;
            this.zedSequence.Size = new System.Drawing.Size(1016, 407);
            this.zedSequence.TabIndex = 0;
            this.zedSequence.MouseDownEvent += new ZedGraph.ZedGraphControl.ZedMouseEventHandler(this.zedSequence_MouseDownEvent);
            this.zedSequence.MouseUpEvent += new ZedGraph.ZedGraphControl.ZedMouseEventHandler(this.zedSequence_MouseUpEvent);
            this.zedSequence.MouseMove += new System.Windows.Forms.MouseEventHandler(this.zedSequence_MouseMove);
            // 
            // frmSingle
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1016, 803);
            this.Controls.Add(this.splitContainer1);
            this.Name = "frmSingle";
            this.Text = "Glycan Sequencing (v0.8)";
            this.Load += new System.EventHandler(this.frmSingle_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer5.Panel1.ResumeLayout(false);
            this.splitContainer5.Panel2.ResumeLayout(false);
            this.splitContainer5.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.grpGlycan.ResumeLayout(false);
            this.grpGlycan.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgIDPeak)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.TextBox txtScanNo;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.DataGridView dgView;
        private ZedGraph.ZedGraphControl zedSequence;
        private System.Windows.Forms.Label lblScan;
        private System.Windows.Forms.TextBox txtY1;
        private System.Windows.Forms.Label lblY1Z;
        private System.Windows.Forms.TextBox txtY1ChargeStatus;
        private System.Windows.Forms.ComboBox cboScan;
        private System.Windows.Forms.DataGridView dgDetail;
        private System.Windows.Forms.DataGridView dgIDPeak;
        private System.Windows.Forms.CheckBox chkNLinked;
        private System.Windows.Forms.Label lblPPM;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtPeaKTol;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtPrecusorTol;
        private System.Windows.Forms.SplitContainer splitContainer5;
        private System.Windows.Forms.TextBox txtPeptideSeq;
        private System.Windows.Forms.RadioButton rdoPeptide;
        private System.Windows.Forms.RadioButton rdoY1;
        private System.Windows.Forms.GroupBox grpGlycan;
        private System.Windows.Forms.Label lblMass;
        private System.Windows.Forms.ListBox lstPeak;
        private System.Windows.Forms.TextBox txtdeHex;
        private System.Windows.Forms.TextBox txtHexNAc;
        private System.Windows.Forms.TextBox txtHex;
        private System.Windows.Forms.TextBox txtSia;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton rdoNeuAc;
        private System.Windows.Forms.RadioButton rdoNeuGc;
        private System.Windows.Forms.CheckBox chkAVGMass;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
    }
}

