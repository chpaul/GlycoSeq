namespace GlycanSeq_Form
{
    partial class frmBatch
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
            this.btnRawBrowse = new System.Windows.Forms.Button();
            this.txtFasta = new System.Windows.Forms.TextBox();
            this.txtRaw = new System.Windows.Forms.TextBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnPeptideBrowse = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.txtStart = new System.Windows.Forms.TextBox();
            this.txtEnd = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cboTopRank = new System.Windows.Forms.ComboBox();
            this.rdoScanList = new System.Windows.Forms.RadioButton();
            this.rdoScanRange = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chkEnzy_GlucE = new System.Windows.Forms.CheckBox();
            this.chkEnzy_Trypsin = new System.Windows.Forms.CheckBox();
            this.chkEnzy_GlucED = new System.Windows.Forms.CheckBox();
            this.chkEnzy_None = new System.Windows.Forms.CheckBox();
            this.cboMissCleavage = new System.Windows.Forms.ComboBox();
            this.lstModification = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.txtCompReward = new System.Windows.Forms.TextBox();
            this.lblCompReward = new System.Windows.Forms.Label();
            this.chkCompletedOnly = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtPrecusorTol = new System.Windows.Forms.TextBox();
            this.txtPeaKTol = new System.Windows.Forms.TextBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.chkPrecursorCreate = new System.Windows.Forms.CheckBox();
            this.txtScanFileList = new System.Windows.Forms.TextBox();
            this.btnScanList = new System.Windows.Forms.Button();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.chkSeqHCD = new System.Windows.Forms.CheckBox();
            this.chkHCD = new System.Windows.Forms.CheckBox();
            this.lblEndScan = new System.Windows.Forms.Label();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.grpGlycan = new System.Windows.Forms.GroupBox();
            this.rdoNeuAc = new System.Windows.Forms.RadioButton();
            this.chkHuman = new System.Windows.Forms.CheckBox();
            this.rdoNeuGc = new System.Windows.Forms.RadioButton();
            this.txtHexNAc = new System.Windows.Forms.TextBox();
            this.chkAvgMass = new System.Windows.Forms.CheckBox();
            this.txtdeHex = new System.Windows.Forms.TextBox();
            this.txtSia = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtHex = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.chkNLinked = new System.Windows.Forms.CheckBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.btnGlycanList = new System.Windows.Forms.Button();
            this.txtGlycanList = new System.Windows.Forms.TextBox();
            this.chkReducedReducingEnd = new System.Windows.Forms.CheckBox();
            this.chkPerm = new System.Windows.Forms.CheckBox();
            this.chkGlycanList = new System.Windows.Forms.CheckBox();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.cboThreading = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.grpGlycan.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnRawBrowse
            // 
            this.btnRawBrowse.Location = new System.Drawing.Point(379, 16);
            this.btnRawBrowse.Name = "btnRawBrowse";
            this.btnRawBrowse.Size = new System.Drawing.Size(23, 23);
            this.btnRawBrowse.TabIndex = 1;
            this.btnRawBrowse.Text = "...";
            this.btnRawBrowse.UseVisualStyleBackColor = true;
            this.btnRawBrowse.Click += new System.EventHandler(this.btnRawBrowse_Click);
            // 
            // txtFasta
            // 
            this.txtFasta.Location = new System.Drawing.Point(6, 19);
            this.txtFasta.Name = "txtFasta";
            this.txtFasta.Size = new System.Drawing.Size(367, 20);
            this.txtFasta.TabIndex = 5;
            // 
            // txtRaw
            // 
            this.txtRaw.Location = new System.Drawing.Point(6, 19);
            this.txtRaw.Name = "txtRaw";
            this.txtRaw.Size = new System.Drawing.Size(367, 20);
            this.txtRaw.TabIndex = 0;
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(485, 299);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 25;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnPeptideBrowse
            // 
            this.btnPeptideBrowse.Location = new System.Drawing.Point(379, 16);
            this.btnPeptideBrowse.Name = "btnPeptideBrowse";
            this.btnPeptideBrowse.Size = new System.Drawing.Size(23, 23);
            this.btnPeptideBrowse.TabIndex = 6;
            this.btnPeptideBrowse.Text = "...";
            this.btnPeptideBrowse.UseVisualStyleBackColor = true;
            this.btnPeptideBrowse.Click += new System.EventHandler(this.btnPeptideBrowse_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // txtStart
            // 
            this.txtStart.Location = new System.Drawing.Point(93, 16);
            this.txtStart.Name = "txtStart";
            this.txtStart.Size = new System.Drawing.Size(59, 20);
            this.txtStart.TabIndex = 2;
            // 
            // txtEnd
            // 
            this.txtEnd.Location = new System.Drawing.Point(172, 16);
            this.txtEnd.Name = "txtEnd";
            this.txtEnd.Size = new System.Drawing.Size(58, 20);
            this.txtEnd.TabIndex = 3;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtRaw);
            this.groupBox1.Controls.Add(this.btnRawBrowse);
            this.groupBox1.Location = new System.Drawing.Point(6, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(414, 52);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Raw File";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(10, 20);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(87, 13);
            this.label6.TabIndex = 28;
            this.label6.Text = "Get Top ? Rank:";
            // 
            // cboTopRank
            // 
            this.cboTopRank.FormattingEnabled = true;
            this.cboTopRank.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6"});
            this.cboTopRank.Location = new System.Drawing.Point(97, 15);
            this.cboTopRank.Name = "cboTopRank";
            this.cboTopRank.Size = new System.Drawing.Size(37, 21);
            this.cboTopRank.TabIndex = 27;
            // 
            // rdoScanList
            // 
            this.rdoScanList.AutoSize = true;
            this.rdoScanList.Location = new System.Drawing.Point(6, 47);
            this.rdoScanList.Name = "rdoScanList";
            this.rdoScanList.Size = new System.Drawing.Size(91, 17);
            this.rdoScanList.TabIndex = 31;
            this.rdoScanList.Text = "Scan List File:";
            this.rdoScanList.UseVisualStyleBackColor = true;
            // 
            // rdoScanRange
            // 
            this.rdoScanRange.AutoSize = true;
            this.rdoScanRange.Checked = true;
            this.rdoScanRange.Location = new System.Drawing.Point(6, 19);
            this.rdoScanRange.Name = "rdoScanRange";
            this.rdoScanRange.Size = new System.Drawing.Size(88, 17);
            this.rdoScanRange.TabIndex = 30;
            this.rdoScanRange.TabStop = true;
            this.rdoScanRange.Text = "Scan Range:";
            this.rdoScanRange.UseVisualStyleBackColor = true;
            this.rdoScanRange.CheckedChanged += new System.EventHandler(this.rdoScanRange_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chkEnzy_GlucE);
            this.groupBox2.Controls.Add(this.chkEnzy_Trypsin);
            this.groupBox2.Controls.Add(this.chkEnzy_GlucED);
            this.groupBox2.Controls.Add(this.chkEnzy_None);
            this.groupBox2.Controls.Add(this.cboMissCleavage);
            this.groupBox2.Controls.Add(this.lstModification);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.txtFasta);
            this.groupBox2.Controls.Add(this.btnPeptideBrowse);
            this.groupBox2.Location = new System.Drawing.Point(6, 148);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(414, 108);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Fasta";
            // 
            // chkEnzy_GlucE
            // 
            this.chkEnzy_GlucE.AutoSize = true;
            this.chkEnzy_GlucE.Location = new System.Drawing.Point(99, 62);
            this.chkEnzy_GlucE.Name = "chkEnzy_GlucE";
            this.chkEnzy_GlucE.Size = new System.Drawing.Size(65, 17);
            this.chkEnzy_GlucE.TabIndex = 29;
            this.chkEnzy_GlucE.Text = "GluC (E)";
            this.chkEnzy_GlucE.UseVisualStyleBackColor = true;
            this.chkEnzy_GlucE.CheckedChanged += new System.EventHandler(this.chkEnzy_GlucE_CheckedChanged);
            // 
            // chkEnzy_Trypsin
            // 
            this.chkEnzy_Trypsin.AutoSize = true;
            this.chkEnzy_Trypsin.Location = new System.Drawing.Point(13, 82);
            this.chkEnzy_Trypsin.Name = "chkEnzy_Trypsin";
            this.chkEnzy_Trypsin.Size = new System.Drawing.Size(60, 17);
            this.chkEnzy_Trypsin.TabIndex = 28;
            this.chkEnzy_Trypsin.Text = "Trypsin";
            this.chkEnzy_Trypsin.UseVisualStyleBackColor = true;
            this.chkEnzy_Trypsin.CheckedChanged += new System.EventHandler(this.chkEnzy_Trypsin_CheckedChanged);
            // 
            // chkEnzy_GlucED
            // 
            this.chkEnzy_GlucED.AutoSize = true;
            this.chkEnzy_GlucED.Location = new System.Drawing.Point(99, 82);
            this.chkEnzy_GlucED.Name = "chkEnzy_GlucED";
            this.chkEnzy_GlucED.Size = new System.Drawing.Size(73, 17);
            this.chkEnzy_GlucED.TabIndex = 27;
            this.chkEnzy_GlucED.Text = "GluC (ED)";
            this.chkEnzy_GlucED.UseVisualStyleBackColor = true;
            this.chkEnzy_GlucED.CheckedChanged += new System.EventHandler(this.chkEnzy_GlucED_CheckedChanged);
            // 
            // chkEnzy_None
            // 
            this.chkEnzy_None.AutoSize = true;
            this.chkEnzy_None.Checked = true;
            this.chkEnzy_None.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkEnzy_None.Location = new System.Drawing.Point(13, 62);
            this.chkEnzy_None.Name = "chkEnzy_None";
            this.chkEnzy_None.Size = new System.Drawing.Size(52, 17);
            this.chkEnzy_None.TabIndex = 26;
            this.chkEnzy_None.Text = "None";
            this.chkEnzy_None.UseVisualStyleBackColor = true;
            this.chkEnzy_None.CheckedChanged += new System.EventHandler(this.chkEnzy_None_CheckedChanged);
            // 
            // cboMissCleavage
            // 
            this.cboMissCleavage.FormattingEnabled = true;
            this.cboMissCleavage.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9"});
            this.cboMissCleavage.Location = new System.Drawing.Point(283, 81);
            this.cboMissCleavage.Name = "cboMissCleavage";
            this.cboMissCleavage.Size = new System.Drawing.Size(31, 21);
            this.cboMissCleavage.TabIndex = 7;
            this.cboMissCleavage.Text = "0";
            // 
            // lstModification
            // 
            this.lstModification.Enabled = false;
            this.lstModification.FormattingEnabled = true;
            this.lstModification.Items.AddRange(new object[] {
            "Cys_CAM"});
            this.lstModification.Location = new System.Drawing.Point(291, 58);
            this.lstModification.Name = "lstModification";
            this.lstModification.Size = new System.Drawing.Size(54, 17);
            this.lstModification.TabIndex = 20;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(221, 62);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 13);
            this.label3.TabIndex = 25;
            this.label3.Text = "Modification";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 13);
            this.label1.TabIndex = 23;
            this.label1.Text = "Enzyme";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(221, 83);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(182, 13);
            this.label2.TabIndex = 22;
            this.label2.Text = "Allow up to             missed cleavages";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.txtCompReward);
            this.groupBox3.Controls.Add(this.lblCompReward);
            this.groupBox3.Controls.Add(this.chkCompletedOnly);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.cboTopRank);
            this.groupBox3.Location = new System.Drawing.Point(426, 70);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(183, 95);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Export";
            // 
            // txtCompReward
            // 
            this.txtCompReward.Enabled = false;
            this.txtCompReward.Location = new System.Drawing.Point(148, 65);
            this.txtCompReward.Name = "txtCompReward";
            this.txtCompReward.Size = new System.Drawing.Size(29, 20);
            this.txtCompReward.TabIndex = 35;
            this.txtCompReward.Text = "1.0";
            // 
            // lblCompReward
            // 
            this.lblCompReward.AutoSize = true;
            this.lblCompReward.Enabled = false;
            this.lblCompReward.Location = new System.Drawing.Point(11, 68);
            this.lblCompReward.Name = "lblCompReward";
            this.lblCompReward.Size = new System.Drawing.Size(131, 13);
            this.lblCompReward.TabIndex = 34;
            this.lblCompReward.Text = "Completed Score Reward:";
            // 
            // chkCompletedOnly
            // 
            this.chkCompletedOnly.AutoSize = true;
            this.chkCompletedOnly.Checked = true;
            this.chkCompletedOnly.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCompletedOnly.Location = new System.Drawing.Point(13, 42);
            this.chkCompletedOnly.Name = "chkCompletedOnly";
            this.chkCompletedOnly.Size = new System.Drawing.Size(137, 17);
            this.chkCompletedOnly.TabIndex = 33;
            this.chkCompletedOnly.Text = "Only complete structure";
            this.chkCompletedOnly.UseVisualStyleBackColor = true;
            this.chkCompletedOnly.CheckedChanged += new System.EventHandler(this.chkCompletedOnly_CheckedChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 50);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(87, 13);
            this.label4.TabIndex = 24;
            this.label4.Text = "Precursor (PPM):";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(29, 22);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(68, 13);
            this.label5.TabIndex = 25;
            this.label5.Text = "MS/MS (da):";
            // 
            // txtPrecusorTol
            // 
            this.txtPrecusorTol.Location = new System.Drawing.Point(102, 47);
            this.txtPrecusorTol.Name = "txtPrecusorTol";
            this.txtPrecusorTol.Size = new System.Drawing.Size(36, 20);
            this.txtPrecusorTol.TabIndex = 24;
            this.txtPrecusorTol.Text = "10";
            // 
            // txtPeaKTol
            // 
            this.txtPeaKTol.Location = new System.Drawing.Point(102, 19);
            this.txtPeaKTol.Name = "txtPeaKTol";
            this.txtPeaKTol.Size = new System.Drawing.Size(36, 20);
            this.txtPeaKTol.TabIndex = 23;
            this.txtPeaKTol.Text = "0.8";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.chkPrecursorCreate);
            this.groupBox4.Controls.Add(this.label4);
            this.groupBox4.Controls.Add(this.txtPeaKTol);
            this.groupBox4.Controls.Add(this.label5);
            this.groupBox4.Controls.Add(this.txtPrecusorTol);
            this.groupBox4.Location = new System.Drawing.Point(426, 172);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(183, 100);
            this.groupBox4.TabIndex = 3;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Torelance";
            // 
            // chkPrecursorCreate
            // 
            this.chkPrecursorCreate.AutoSize = true;
            this.chkPrecursorCreate.Checked = true;
            this.chkPrecursorCreate.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkPrecursorCreate.Location = new System.Drawing.Point(13, 79);
            this.chkPrecursorCreate.Name = "chkPrecursorCreate";
            this.chkPrecursorCreate.Size = new System.Drawing.Size(122, 17);
            this.chkPrecursorCreate.TabIndex = 26;
            this.chkPrecursorCreate.Text = "Precursor in MS/MS";
            this.chkPrecursorCreate.UseVisualStyleBackColor = true;
            // 
            // txtScanFileList
            // 
            this.txtScanFileList.Enabled = false;
            this.txtScanFileList.Location = new System.Drawing.Point(93, 46);
            this.txtScanFileList.Name = "txtScanFileList";
            this.txtScanFileList.Size = new System.Drawing.Size(280, 20);
            this.txtScanFileList.TabIndex = 32;
            // 
            // btnScanList
            // 
            this.btnScanList.Enabled = false;
            this.btnScanList.Location = new System.Drawing.Point(379, 43);
            this.btnScanList.Name = "btnScanList";
            this.btnScanList.Size = new System.Drawing.Size(24, 23);
            this.btnScanList.TabIndex = 33;
            this.btnScanList.Text = "...";
            this.btnScanList.UseVisualStyleBackColor = true;
            this.btnScanList.Click += new System.EventHandler(this.btnScanList_Click);
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.chkSeqHCD);
            this.groupBox6.Controls.Add(this.chkHCD);
            this.groupBox6.Controls.Add(this.btnScanList);
            this.groupBox6.Controls.Add(this.rdoScanRange);
            this.groupBox6.Controls.Add(this.txtScanFileList);
            this.groupBox6.Controls.Add(this.lblEndScan);
            this.groupBox6.Controls.Add(this.rdoScanList);
            this.groupBox6.Controls.Add(this.txtStart);
            this.groupBox6.Controls.Add(this.txtEnd);
            this.groupBox6.Location = new System.Drawing.Point(6, 70);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(414, 72);
            this.groupBox6.TabIndex = 26;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Scans";
            // 
            // chkSeqHCD
            // 
            this.chkSeqHCD.AutoSize = true;
            this.chkSeqHCD.Enabled = false;
            this.chkSeqHCD.Location = new System.Drawing.Point(274, 26);
            this.chkSeqHCD.Name = "chkSeqHCD";
            this.chkSeqHCD.Size = new System.Drawing.Size(101, 17);
            this.chkSeqHCD.TabIndex = 33;
            this.chkSeqHCD.Text = "Sequence HCD";
            this.chkSeqHCD.UseVisualStyleBackColor = true;
            // 
            // chkHCD
            // 
            this.chkHCD.AutoSize = true;
            this.chkHCD.Location = new System.Drawing.Point(274, 9);
            this.chkHCD.Name = "chkHCD";
            this.chkHCD.Size = new System.Drawing.Size(91, 17);
            this.chkHCD.TabIndex = 34;
            this.chkHCD.Text = "Use HCD info";
            this.chkHCD.UseVisualStyleBackColor = true;
            this.chkHCD.CheckedChanged += new System.EventHandler(this.chkHCD_CheckedChanged);
            // 
            // lblEndScan
            // 
            this.lblEndScan.AutoSize = true;
            this.lblEndScan.Location = new System.Drawing.Point(154, 20);
            this.lblEndScan.Name = "lblEndScan";
            this.lblEndScan.Size = new System.Drawing.Size(14, 13);
            this.lblEndScan.TabIndex = 12;
            this.lblEndScan.Text = "~";
            // 
            // grpGlycan
            // 
            this.grpGlycan.Controls.Add(this.rdoNeuAc);
            this.grpGlycan.Controls.Add(this.chkHuman);
            this.grpGlycan.Controls.Add(this.rdoNeuGc);
            this.grpGlycan.Controls.Add(this.txtHexNAc);
            this.grpGlycan.Controls.Add(this.chkAvgMass);
            this.grpGlycan.Controls.Add(this.txtdeHex);
            this.grpGlycan.Controls.Add(this.txtSia);
            this.grpGlycan.Controls.Add(this.label7);
            this.grpGlycan.Controls.Add(this.txtHex);
            this.grpGlycan.Controls.Add(this.label8);
            this.grpGlycan.Controls.Add(this.label9);
            this.grpGlycan.Controls.Add(this.chkNLinked);
            this.grpGlycan.Location = new System.Drawing.Point(7, 344);
            this.grpGlycan.Name = "grpGlycan";
            this.grpGlycan.Size = new System.Drawing.Size(413, 90);
            this.grpGlycan.TabIndex = 31;
            this.grpGlycan.TabStop = false;
            this.grpGlycan.Text = "Contain";
            // 
            // rdoNeuAc
            // 
            this.rdoNeuAc.AutoSize = true;
            this.rdoNeuAc.Checked = true;
            this.rdoNeuAc.Location = new System.Drawing.Point(85, 10);
            this.rdoNeuAc.Name = "rdoNeuAc";
            this.rdoNeuAc.Size = new System.Drawing.Size(58, 17);
            this.rdoNeuAc.TabIndex = 11;
            this.rdoNeuAc.TabStop = true;
            this.rdoNeuAc.Text = "NeuAc";
            this.rdoNeuAc.UseVisualStyleBackColor = true;
            // 
            // chkHuman
            // 
            this.chkHuman.AutoSize = true;
            this.chkHuman.Checked = true;
            this.chkHuman.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkHuman.Location = new System.Drawing.Point(176, 31);
            this.chkHuman.Name = "chkHuman";
            this.chkHuman.Size = new System.Drawing.Size(60, 17);
            this.chkHuman.TabIndex = 4;
            this.chkHuman.Text = "Human";
            this.chkHuman.UseVisualStyleBackColor = true;
            // 
            // rdoNeuGc
            // 
            this.rdoNeuGc.AutoSize = true;
            this.rdoNeuGc.Location = new System.Drawing.Point(85, 33);
            this.rdoNeuGc.Name = "rdoNeuGc";
            this.rdoNeuGc.Size = new System.Drawing.Size(59, 17);
            this.rdoNeuGc.TabIndex = 10;
            this.rdoNeuGc.Text = "NeuGc";
            this.rdoNeuGc.UseVisualStyleBackColor = true;
            // 
            // txtHexNAc
            // 
            this.txtHexNAc.Location = new System.Drawing.Point(54, 31);
            this.txtHexNAc.Name = "txtHexNAc";
            this.txtHexNAc.Size = new System.Drawing.Size(26, 20);
            this.txtHexNAc.TabIndex = 8;
            this.txtHexNAc.Text = "99";
            // 
            // chkAvgMass
            // 
            this.chkAvgMass.AutoSize = true;
            this.chkAvgMass.Checked = true;
            this.chkAvgMass.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAvgMass.Location = new System.Drawing.Point(176, 52);
            this.chkAvgMass.Name = "chkAvgMass";
            this.chkAvgMass.Size = new System.Drawing.Size(94, 17);
            this.chkAvgMass.TabIndex = 32;
            this.chkAvgMass.Text = "Average Mass";
            this.chkAvgMass.UseVisualStyleBackColor = true;
            // 
            // txtdeHex
            // 
            this.txtdeHex.Location = new System.Drawing.Point(54, 52);
            this.txtdeHex.Name = "txtdeHex";
            this.txtdeHex.Size = new System.Drawing.Size(26, 20);
            this.txtdeHex.TabIndex = 7;
            this.txtdeHex.Text = "0";
            // 
            // txtSia
            // 
            this.txtSia.Location = new System.Drawing.Point(145, 15);
            this.txtSia.Name = "txtSia";
            this.txtSia.Size = new System.Drawing.Size(25, 20);
            this.txtSia.TabIndex = 6;
            this.txtSia.Text = "99";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(10, 56);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(38, 13);
            this.label7.TabIndex = 2;
            this.label7.Text = "deHex";
            // 
            // txtHex
            // 
            this.txtHex.Location = new System.Drawing.Point(54, 10);
            this.txtHex.Name = "txtHex";
            this.txtHex.Size = new System.Drawing.Size(25, 20);
            this.txtHex.TabIndex = 9;
            this.txtHex.Text = "99";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(1, 35);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(47, 13);
            this.label8.TabIndex = 1;
            this.label8.Text = "HexNAc";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(22, 14);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(26, 13);
            this.label9.TabIndex = 0;
            this.label9.Text = "Hex";
            // 
            // chkNLinked
            // 
            this.chkNLinked.AutoSize = true;
            this.chkNLinked.Checked = true;
            this.chkNLinked.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkNLinked.Location = new System.Drawing.Point(176, 10);
            this.chkNLinked.Name = "chkNLinked";
            this.chkNLinked.Size = new System.Drawing.Size(69, 17);
            this.chkNLinked.TabIndex = 6;
            this.chkNLinked.Text = "N-Linked";
            this.chkNLinked.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.btnGlycanList);
            this.groupBox5.Controls.Add(this.txtGlycanList);
            this.groupBox5.Enabled = false;
            this.groupBox5.Location = new System.Drawing.Point(7, 285);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(414, 53);
            this.groupBox5.TabIndex = 29;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Glycan List";
            // 
            // btnGlycanList
            // 
            this.btnGlycanList.Location = new System.Drawing.Point(379, 14);
            this.btnGlycanList.Name = "btnGlycanList";
            this.btnGlycanList.Size = new System.Drawing.Size(23, 23);
            this.btnGlycanList.TabIndex = 1;
            this.btnGlycanList.Text = "...";
            this.btnGlycanList.UseVisualStyleBackColor = true;
            this.btnGlycanList.Click += new System.EventHandler(this.btnGlycanList_Click);
            // 
            // txtGlycanList
            // 
            this.txtGlycanList.Location = new System.Drawing.Point(3, 16);
            this.txtGlycanList.Name = "txtGlycanList";
            this.txtGlycanList.Size = new System.Drawing.Size(370, 20);
            this.txtGlycanList.TabIndex = 0;
            // 
            // chkReducedReducingEnd
            // 
            this.chkReducedReducingEnd.AutoSize = true;
            this.chkReducedReducingEnd.Location = new System.Drawing.Point(430, 367);
            this.chkReducedReducingEnd.Name = "chkReducedReducingEnd";
            this.chkReducedReducingEnd.Size = new System.Drawing.Size(141, 17);
            this.chkReducedReducingEnd.TabIndex = 3;
            this.chkReducedReducingEnd.Text = "Reduced Reducing End";
            this.chkReducedReducingEnd.UseVisualStyleBackColor = true;
            this.chkReducedReducingEnd.Visible = false;
            // 
            // chkPerm
            // 
            this.chkPerm.AutoSize = true;
            this.chkPerm.Location = new System.Drawing.Point(439, 344);
            this.chkPerm.Name = "chkPerm";
            this.chkPerm.Size = new System.Drawing.Size(93, 17);
            this.chkPerm.TabIndex = 2;
            this.chkPerm.Text = "Permethylated";
            this.chkPerm.UseVisualStyleBackColor = true;
            this.chkPerm.Visible = false;
            // 
            // chkGlycanList
            // 
            this.chkGlycanList.AutoSize = true;
            this.chkGlycanList.Location = new System.Drawing.Point(10, 262);
            this.chkGlycanList.Name = "chkGlycanList";
            this.chkGlycanList.Size = new System.Drawing.Size(64, 17);
            this.chkGlycanList.TabIndex = 5;
            this.chkGlycanList.Text = "Use List";
            this.chkGlycanList.UseVisualStyleBackColor = true;
            this.chkGlycanList.CheckedChanged += new System.EventHandler(this.chkGlycanList_CheckedChanged);
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.label10);
            this.groupBox7.Controls.Add(this.cboThreading);
            this.groupBox7.Location = new System.Drawing.Point(426, 13);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(183, 51);
            this.groupBox7.TabIndex = 32;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Threading";
            // 
            // cboThreading
            // 
            this.cboThreading.FormattingEnabled = true;
            this.cboThreading.Location = new System.Drawing.Point(97, 18);
            this.cboThreading.Name = "cboThreading";
            this.cboThreading.Size = new System.Drawing.Size(75, 21);
            this.cboThreading.TabIndex = 0;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(10, 21);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(84, 13);
            this.label10.TabIndex = 1;
            this.label10.Text = "Thread Number:";
            // 
            // frmBatch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnablePreventFocusChange;
            this.ClientSize = new System.Drawing.Size(619, 438);
            this.Controls.Add(this.groupBox7);
            this.Controls.Add(this.chkReducedReducingEnd);
            this.Controls.Add(this.chkPerm);
            this.Controls.Add(this.chkGlycanList);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.grpGlycan);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnStart);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "frmBatch";
            this.Text = "Glycoseq (Batch Mode)";
            this.Load += new System.EventHandler(this.frmBatch_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.grpGlycan.ResumeLayout(false);
            this.grpGlycan.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnRawBrowse;
        private System.Windows.Forms.TextBox txtFasta;
        private System.Windows.Forms.TextBox txtRaw;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnPeptideBrowse;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.TextBox txtStart;
        private System.Windows.Forms.TextBox txtEnd;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox lstModification;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cboMissCleavage;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtPrecusorTol;
        private System.Windows.Forms.TextBox txtPeaKTol;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cboTopRank;
        private System.Windows.Forms.RadioButton rdoScanList;
        private System.Windows.Forms.RadioButton rdoScanRange;
        private System.Windows.Forms.Button btnScanList;
        private System.Windows.Forms.TextBox txtScanFileList;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.CheckBox chkPrecursorCreate;
        private System.Windows.Forms.GroupBox grpGlycan;
        private System.Windows.Forms.RadioButton rdoNeuAc;
        private System.Windows.Forms.RadioButton rdoNeuGc;
        private System.Windows.Forms.TextBox txtHexNAc;
        private System.Windows.Forms.TextBox txtdeHex;
        private System.Windows.Forms.TextBox txtSia;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtHex;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.CheckBox chkNLinked;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.CheckBox chkHuman;
        private System.Windows.Forms.CheckBox chkReducedReducingEnd;
        private System.Windows.Forms.CheckBox chkPerm;
        private System.Windows.Forms.Button btnGlycanList;
        private System.Windows.Forms.TextBox txtGlycanList;
        private System.Windows.Forms.CheckBox chkGlycanList;
        private System.Windows.Forms.CheckBox chkAvgMass;
        private System.Windows.Forms.CheckBox chkHCD;
        private System.Windows.Forms.CheckBox chkCompletedOnly;
        private System.Windows.Forms.TextBox txtCompReward;
        private System.Windows.Forms.Label lblCompReward;
        private System.Windows.Forms.CheckBox chkSeqHCD;
        private System.Windows.Forms.Label lblEndScan;
        private System.Windows.Forms.CheckBox chkEnzy_GlucE;
        private System.Windows.Forms.CheckBox chkEnzy_Trypsin;
        private System.Windows.Forms.CheckBox chkEnzy_GlucED;
        private System.Windows.Forms.CheckBox chkEnzy_None;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox cboThreading;
    }
}