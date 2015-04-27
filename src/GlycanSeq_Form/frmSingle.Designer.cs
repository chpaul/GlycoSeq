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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btnLoad = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.txtScanNo = new System.Windows.Forms.TextBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer5 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.label6 = new System.Windows.Forms.Label();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.txtMaxGlycansToCompleteStruct_m = new System.Windows.Forms.TextBox();
            this.txtTopBrancingPeaks_l = new System.Windows.Forms.TextBox();
            this.txtTopCorePeaks_k = new System.Windows.Forms.TextBox();
            this.txtTopDiagPeaks_j = new System.Windows.Forms.TextBox();
            this.txtTopPeaks_i = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
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
            this.label4 = new System.Windows.Forms.Label();
            this.txtPeaKTol = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtPrecusorTol = new System.Windows.Forms.TextBox();
            this.lblScan = new System.Windows.Forms.Label();
            this.dgView = new System.Windows.Forms.DataGridView();
            this.dgIDPeak = new System.Windows.Forms.DataGridView();
            this.zedSequence = new ZedGraph.ZedGraphControl();
            this.openFileDialog2 = new System.Windows.Forms.OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer5)).BeginInit();
            this.splitContainer5.Panel1.SuspendLayout();
            this.splitContainer5.Panel2.SuspendLayout();
            this.splitContainer5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.grpGlycan.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgIDPeak)).BeginInit();
            this.SuspendLayout();
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(523, 35);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(60, 47);
            this.btnLoad.TabIndex = 8;
            this.btnLoad.Text = "Start";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // txtScanNo
            // 
            this.txtScanNo.Location = new System.Drawing.Point(57, 5);
            this.txtScanNo.Name = "txtScanNo";
            this.txtScanNo.Size = new System.Drawing.Size(73, 20);
            this.txtScanNo.TabIndex = 0;
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
            this.splitContainer1.Size = new System.Drawing.Size(1125, 835);
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
            this.splitContainer5.Size = new System.Drawing.Size(1125, 392);
            this.splitContainer5.SplitterDistance = 682;
            this.splitContainer5.TabIndex = 5;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.label6);
            this.splitContainer2.Panel1.Controls.Add(this.btnBrowse);
            this.splitContainer2.Panel1.Controls.Add(this.groupBox7);
            this.splitContainer2.Panel1.Controls.Add(this.lblMass);
            this.splitContainer2.Panel1.Controls.Add(this.grpGlycan);
            this.splitContainer2.Panel1.Controls.Add(this.txtPeptideSeq);
            this.splitContainer2.Panel1.Controls.Add(this.label4);
            this.splitContainer2.Panel1.Controls.Add(this.txtPeaKTol);
            this.splitContainer2.Panel1.Controls.Add(this.label5);
            this.splitContainer2.Panel1.Controls.Add(this.txtPrecusorTol);
            this.splitContainer2.Panel1.Controls.Add(this.lblScan);
            this.splitContainer2.Panel1.Controls.Add(this.txtScanNo);
            this.splitContainer2.Panel1.Controls.Add(this.btnLoad);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.dgView);
            this.splitContainer2.Size = new System.Drawing.Size(682, 392);
            this.splitContainer2.SplitterDistance = 174;
            this.splitContainer2.TabIndex = 4;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(152, 10);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 13);
            this.label6.TabIndex = 42;
            this.label6.Text = "Peptide File:";
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(561, 3);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(22, 23);
            this.btnBrowse.TabIndex = 41;
            this.btnBrowse.Text = "...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.txtMaxGlycansToCompleteStruct_m);
            this.groupBox7.Controls.Add(this.txtTopBrancingPeaks_l);
            this.groupBox7.Controls.Add(this.txtTopCorePeaks_k);
            this.groupBox7.Controls.Add(this.txtTopDiagPeaks_j);
            this.groupBox7.Controls.Add(this.txtTopPeaks_i);
            this.groupBox7.Controls.Add(this.label19);
            this.groupBox7.Controls.Add(this.label18);
            this.groupBox7.Controls.Add(this.label17);
            this.groupBox7.Controls.Add(this.label16);
            this.groupBox7.Controls.Add(this.label15);
            this.groupBox7.Location = new System.Drawing.Point(192, 38);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(323, 134);
            this.groupBox7.TabIndex = 40;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Sequencing Parameters";
            // 
            // txtMaxGlycansToCompleteStruct_m
            // 
            this.txtMaxGlycansToCompleteStruct_m.Location = new System.Drawing.Point(274, 111);
            this.txtMaxGlycansToCompleteStruct_m.Name = "txtMaxGlycansToCompleteStruct_m";
            this.txtMaxGlycansToCompleteStruct_m.Size = new System.Drawing.Size(41, 20);
            this.txtMaxGlycansToCompleteStruct_m.TabIndex = 9;
            this.txtMaxGlycansToCompleteStruct_m.Text = "3";
            // 
            // txtTopBrancingPeaks_l
            // 
            this.txtTopBrancingPeaks_l.Location = new System.Drawing.Point(274, 88);
            this.txtTopBrancingPeaks_l.Name = "txtTopBrancingPeaks_l";
            this.txtTopBrancingPeaks_l.Size = new System.Drawing.Size(41, 20);
            this.txtTopBrancingPeaks_l.TabIndex = 8;
            this.txtTopBrancingPeaks_l.Text = "100";
            // 
            // txtTopCorePeaks_k
            // 
            this.txtTopCorePeaks_k.Location = new System.Drawing.Point(274, 63);
            this.txtTopCorePeaks_k.Name = "txtTopCorePeaks_k";
            this.txtTopCorePeaks_k.Size = new System.Drawing.Size(41, 20);
            this.txtTopCorePeaks_k.TabIndex = 7;
            this.txtTopCorePeaks_k.Text = "30";
            // 
            // txtTopDiagPeaks_j
            // 
            this.txtTopDiagPeaks_j.Location = new System.Drawing.Point(274, 41);
            this.txtTopDiagPeaks_j.Name = "txtTopDiagPeaks_j";
            this.txtTopDiagPeaks_j.Size = new System.Drawing.Size(41, 20);
            this.txtTopDiagPeaks_j.TabIndex = 6;
            this.txtTopDiagPeaks_j.Text = "30";
            // 
            // txtTopPeaks_i
            // 
            this.txtTopPeaks_i.Location = new System.Drawing.Point(274, 15);
            this.txtTopPeaks_i.Name = "txtTopPeaks_i";
            this.txtTopPeaks_i.Size = new System.Drawing.Size(41, 20);
            this.txtTopPeaks_i.TabIndex = 5;
            this.txtTopPeaks_i.Text = "30";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(6, 115);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(265, 13);
            this.label19.TabIndex = 4;
            this.label19.Text = "Add Max monosaccharides for incompleteing structure:";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(20, 92);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(251, 13);
            this.label18.TabIndex = 3;
            this.label18.Text = "Get top l peaks as sequencing \"Branch\" candidate:";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(28, 67);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(243, 13);
            this.label17.TabIndex = 2;
            this.label17.Text = "Get top k peaks as sequencing \"Core\" candidate:";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(77, 45);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(194, 13);
            this.label16.TabIndex = 1;
            this.label16.Text = "Get top j peaks as peptide + core peak:";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(159, 20);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(112, 13);
            this.label15.TabIndex = 0;
            this.label15.Text = "Get top i peaks as Y1:";
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
            this.grpGlycan.Location = new System.Drawing.Point(3, 26);
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
            this.txtPeptideSeq.Location = new System.Drawing.Point(223, 5);
            this.txtPeptideSeq.Name = "txtPeptideSeq";
            this.txtPeptideSeq.Size = new System.Drawing.Size(332, 20);
            this.txtPeptideSeq.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 141);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(138, 13);
            this.label4.TabIndex = 28;
            this.label4.Text = "Precursor Tolerance (PPM):";
            // 
            // txtPeaKTol
            // 
            this.txtPeaKTol.Location = new System.Drawing.Point(154, 117);
            this.txtPeaKTol.Name = "txtPeaKTol";
            this.txtPeaKTol.Size = new System.Drawing.Size(28, 20);
            this.txtPeaKTol.TabIndex = 9;
            this.txtPeaKTol.Text = "0.8";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(42, 121);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(106, 13);
            this.label5.TabIndex = 29;
            this.label5.Text = "Peak Tolerance(Da):";
            // 
            // txtPrecusorTol
            // 
            this.txtPrecusorTol.Location = new System.Drawing.Point(154, 140);
            this.txtPrecusorTol.Name = "txtPrecusorTol";
            this.txtPrecusorTol.Size = new System.Drawing.Size(28, 20);
            this.txtPrecusorTol.TabIndex = 10;
            this.txtPrecusorTol.Text = "10";
            // 
            // lblScan
            // 
            this.lblScan.AutoSize = true;
            this.lblScan.Location = new System.Drawing.Point(6, 12);
            this.lblScan.Name = "lblScan";
            this.lblScan.Size = new System.Drawing.Size(45, 13);
            this.lblScan.TabIndex = 3;
            this.lblScan.Text = "Scan #:";
            this.lblScan.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // dgView
            // 
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.dgView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgView.DefaultCellStyle = dataGridViewCellStyle8;
            this.dgView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgView.Location = new System.Drawing.Point(0, 0);
            this.dgView.Name = "dgView";
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgView.RowHeadersDefaultCellStyle = dataGridViewCellStyle9;
            this.dgView.RowTemplate.Height = 20;
            this.dgView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgView.Size = new System.Drawing.Size(682, 214);
            this.dgView.TabIndex = 0;
            this.dgView.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgView_CellClick);
            this.dgView.Sorted += new System.EventHandler(this.dgView_Sorted);
            // 
            // dgIDPeak
            // 
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle10.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle10.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle10.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle10.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgIDPeak.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle10;
            this.dgIDPeak.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle11.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle11.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle11.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle11.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle11.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgIDPeak.DefaultCellStyle = dataGridViewCellStyle11;
            this.dgIDPeak.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgIDPeak.Location = new System.Drawing.Point(0, 0);
            this.dgIDPeak.Name = "dgIDPeak";
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle12.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle12.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle12.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle12.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle12.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgIDPeak.RowHeadersDefaultCellStyle = dataGridViewCellStyle12;
            this.dgIDPeak.Size = new System.Drawing.Size(439, 392);
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
            this.zedSequence.Size = new System.Drawing.Size(1125, 439);
            this.zedSequence.TabIndex = 0;
            this.zedSequence.MouseDownEvent += new ZedGraph.ZedGraphControl.ZedMouseEventHandler(this.zedSequence_MouseDownEvent);
            this.zedSequence.MouseUpEvent += new ZedGraph.ZedGraphControl.ZedMouseEventHandler(this.zedSequence_MouseUpEvent);
            this.zedSequence.MouseMove += new System.Windows.Forms.MouseEventHandler(this.zedSequence_MouseMove);
            // 
            // openFileDialog2
            // 
            this.openFileDialog2.FileName = "openFileDialog2";
            // 
            // frmSingle
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1125, 835);
            this.Controls.Add(this.splitContainer1);
            this.Name = "frmSingle";
            this.Text = "GlycoSeq";
            this.Load += new System.EventHandler(this.frmSingle_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer5.Panel1.ResumeLayout(false);
            this.splitContainer5.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer5)).EndInit();
            this.splitContainer5.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.grpGlycan.ResumeLayout(false);
            this.grpGlycan.PerformLayout();
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
        private System.Windows.Forms.DataGridView dgIDPeak;
        private System.Windows.Forms.CheckBox chkNLinked;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtPeaKTol;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtPrecusorTol;
        private System.Windows.Forms.SplitContainer splitContainer5;
        private System.Windows.Forms.TextBox txtPeptideSeq;
        private System.Windows.Forms.GroupBox grpGlycan;
        private System.Windows.Forms.Label lblMass;
        private System.Windows.Forms.TextBox txtdeHex;
        private System.Windows.Forms.TextBox txtHexNAc;
        private System.Windows.Forms.TextBox txtHex;
        private System.Windows.Forms.TextBox txtSia;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton rdoNeuAc;
        private System.Windows.Forms.RadioButton rdoNeuGc;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.TextBox txtMaxGlycansToCompleteStruct_m;
        private System.Windows.Forms.TextBox txtTopBrancingPeaks_l;
        private System.Windows.Forms.TextBox txtTopCorePeaks_k;
        private System.Windows.Forms.TextBox txtTopDiagPeaks_j;
        private System.Windows.Forms.TextBox txtTopPeaks_i;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.OpenFileDialog openFileDialog2;
    }
}

