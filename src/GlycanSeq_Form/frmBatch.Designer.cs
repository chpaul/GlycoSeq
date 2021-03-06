﻿namespace GlycanSeq_Form
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
            this.txtScoreAlpha = new System.Windows.Forms.TextBox();
            this.txtScoreBeta = new System.Windows.Forms.TextBox();
            this.txtFDRPrecentage = new System.Windows.Forms.TextBox();
            this.label24 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.lblDecoyMass = new System.Windows.Forms.Label();
            this.txtDecoyMass = new System.Windows.Forms.TextBox();
            this.chkDecoy = new System.Windows.Forms.CheckBox();
            this.rdoMascotExtractor = new System.Windows.Forms.RadioButton();
            this.btnChkPeptideCandidate = new System.Windows.Forms.Button();
            this.label14 = new System.Windows.Forms.Label();
            this.rdoFastaOnly = new System.Windows.Forms.RadioButton();
            this.txtTolTime = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.cboPepMutation = new System.Windows.Forms.ComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.chkEnzy_GlucE = new System.Windows.Forms.CheckBox();
            this.chkEnzy_Trypsin = new System.Windows.Forms.CheckBox();
            this.label11 = new System.Windows.Forms.Label();
            this.chkEnzy_GlucED = new System.Windows.Forms.CheckBox();
            this.chkEnzy_None = new System.Windows.Forms.CheckBox();
            this.txtShiftTime = new System.Windows.Forms.TextBox();
            this.cboMissCleavage = new System.Windows.Forms.ComboBox();
            this.lstModification = new System.Windows.Forms.ListBox();
            this.cboShiftSign = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.rdoPeptideWithTime = new System.Windows.Forms.RadioButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.chkIndividualReport = new System.Windows.Forms.CheckBox();
            this.chkCompletedOnly = new System.Windows.Forms.CheckBox();
            this.txtCompReward = new System.Windows.Forms.TextBox();
            this.lblCompReward = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtPrecusorTol = new System.Windows.Forms.TextBox();
            this.txtPeaKTol = new System.Windows.Forms.TextBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.chkUnknownPeptideSearch = new System.Windows.Forms.CheckBox();
            this.chkPrecursorCreate = new System.Windows.Forms.CheckBox();
            this.btnScanList = new System.Windows.Forms.Button();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.txtHCDScore = new System.Windows.Forms.TextBox();
            this.label20 = new System.Windows.Forms.Label();
            this.chkSeqHCD = new System.Windows.Forms.CheckBox();
            this.chkHCD = new System.Windows.Forms.CheckBox();
            this.lblEndScan = new System.Windows.Forms.Label();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.grpGlycan = new System.Windows.Forms.GroupBox();
            this.rdoNeuAc = new System.Windows.Forms.RadioButton();
            this.rdoNeuGc = new System.Windows.Forms.RadioButton();
            this.txtHexNAc = new System.Windows.Forms.TextBox();
            this.txtdeHex = new System.Windows.Forms.TextBox();
            this.txtSia = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtHex = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.chkNLinked = new System.Windows.Forms.CheckBox();
            this.chkHuman = new System.Windows.Forms.CheckBox();
            this.chkAvgMass = new System.Windows.Forms.CheckBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.btnGlycanList = new System.Windows.Forms.Button();
            this.txtGlycanList = new System.Windows.Forms.TextBox();
            this.chkGlycanList = new System.Windows.Forms.CheckBox();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.singleScanModeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mascotProteinIDExtractorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.txtMaxGlycansToCompleteStruct_m = new System.Windows.Forms.TextBox();
            this.txtTopBrancingPeaks_l = new System.Windows.Forms.TextBox();
            this.txtTopCorePeaks_k = new System.Windows.Forms.TextBox();
            this.txtTopPeaks_i = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.txtTopDiagPeaks_j = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.label21 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.grpGlycan.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox8.SuspendLayout();
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
            this.txtFasta.Location = new System.Drawing.Point(9, 156);
            this.txtFasta.Name = "txtFasta";
            this.txtFasta.Size = new System.Drawing.Size(301, 20);
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
            this.btnStart.Location = new System.Drawing.Point(693, 420);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 25;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnPeptideBrowse
            // 
            this.btnPeptideBrowse.Location = new System.Drawing.Point(313, 154);
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
            this.groupBox1.Location = new System.Drawing.Point(6, 27);
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
            this.groupBox2.Controls.Add(this.txtScoreAlpha);
            this.groupBox2.Controls.Add(this.txtScoreBeta);
            this.groupBox2.Controls.Add(this.txtFDRPrecentage);
            this.groupBox2.Controls.Add(this.label24);
            this.groupBox2.Controls.Add(this.label23);
            this.groupBox2.Controls.Add(this.label22);
            this.groupBox2.Controls.Add(this.lblDecoyMass);
            this.groupBox2.Controls.Add(this.txtDecoyMass);
            this.groupBox2.Controls.Add(this.chkDecoy);
            this.groupBox2.Controls.Add(this.rdoMascotExtractor);
            this.groupBox2.Controls.Add(this.btnChkPeptideCandidate);
            this.groupBox2.Controls.Add(this.label14);
            this.groupBox2.Controls.Add(this.rdoFastaOnly);
            this.groupBox2.Controls.Add(this.txtTolTime);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.cboPepMutation);
            this.groupBox2.Controls.Add(this.label13);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.chkEnzy_GlucE);
            this.groupBox2.Controls.Add(this.chkEnzy_Trypsin);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.txtFasta);
            this.groupBox2.Controls.Add(this.btnPeptideBrowse);
            this.groupBox2.Controls.Add(this.chkEnzy_GlucED);
            this.groupBox2.Controls.Add(this.chkEnzy_None);
            this.groupBox2.Controls.Add(this.txtShiftTime);
            this.groupBox2.Controls.Add(this.cboMissCleavage);
            this.groupBox2.Controls.Add(this.lstModification);
            this.groupBox2.Controls.Add(this.cboShiftSign);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Location = new System.Drawing.Point(426, 177);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(342, 239);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Peptide";
            // 
            // txtScoreAlpha
            // 
            this.txtScoreAlpha.Location = new System.Drawing.Point(49, 208);
            this.txtScoreAlpha.Name = "txtScoreAlpha";
            this.txtScoreAlpha.Size = new System.Drawing.Size(38, 20);
            this.txtScoreAlpha.TabIndex = 53;
            this.txtScoreAlpha.Text = "1.0";
            // 
            // txtScoreBeta
            // 
            this.txtScoreBeta.Location = new System.Drawing.Point(126, 208);
            this.txtScoreBeta.Name = "txtScoreBeta";
            this.txtScoreBeta.Size = new System.Drawing.Size(38, 20);
            this.txtScoreBeta.TabIndex = 52;
            this.txtScoreBeta.Text = "1.0";
            // 
            // txtFDRPrecentage
            // 
            this.txtFDRPrecentage.Location = new System.Drawing.Point(263, 208);
            this.txtFDRPrecentage.Name = "txtFDRPrecentage";
            this.txtFDRPrecentage.Size = new System.Drawing.Size(36, 20);
            this.txtFDRPrecentage.TabIndex = 51;
            this.txtFDRPrecentage.Text = "0.05";
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(193, 212);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(81, 13);
            this.label24.TabIndex = 50;
            this.label24.Text = "Score FDR <    ";
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(10, 212);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(33, 13);
            this.label23.TabIndex = 49;
            this.label23.Text = "alpha";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(91, 212);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(28, 13);
            this.label22.TabIndex = 48;
            this.label22.Text = "beta";
            // 
            // lblDecoyMass
            // 
            this.lblDecoyMass.AutoSize = true;
            this.lblDecoyMass.Enabled = false;
            this.lblDecoyMass.Location = new System.Drawing.Point(176, 184);
            this.lblDecoyMass.Name = "lblDecoyMass";
            this.lblDecoyMass.Size = new System.Drawing.Size(66, 13);
            this.lblDecoyMass.TabIndex = 47;
            this.lblDecoyMass.Text = "Decoy Mass";
            // 
            // txtDecoyMass
            // 
            this.txtDecoyMass.Enabled = false;
            this.txtDecoyMass.Location = new System.Drawing.Point(248, 180);
            this.txtDecoyMass.Name = "txtDecoyMass";
            this.txtDecoyMass.Size = new System.Drawing.Size(28, 20);
            this.txtDecoyMass.TabIndex = 46;
            this.txtDecoyMass.Text = "40";
            // 
            // chkDecoy
            // 
            this.chkDecoy.AutoSize = true;
            this.chkDecoy.Location = new System.Drawing.Point(9, 183);
            this.chkDecoy.Name = "chkDecoy";
            this.chkDecoy.Size = new System.Drawing.Size(161, 17);
            this.chkDecoy.TabIndex = 42;
            this.chkDecoy.Text = "Decoy search (AA: X added)";
            this.chkDecoy.UseVisualStyleBackColor = true;
            this.chkDecoy.CheckedChanged += new System.EventHandler(this.chkDecoy_CheckedChanged);
            // 
            // rdoMascotExtractor
            // 
            this.rdoMascotExtractor.AutoSize = true;
            this.rdoMascotExtractor.Checked = true;
            this.rdoMascotExtractor.Location = new System.Drawing.Point(6, 19);
            this.rdoMascotExtractor.Name = "rdoMascotExtractor";
            this.rdoMascotExtractor.Size = new System.Drawing.Size(161, 17);
            this.rdoMascotExtractor.TabIndex = 41;
            this.rdoMascotExtractor.TabStop = true;
            this.rdoMascotExtractor.Text = "Mascot Protein ID Ext Result";
            this.rdoMascotExtractor.UseVisualStyleBackColor = true;
            // 
            // btnChkPeptideCandidate
            // 
            this.btnChkPeptideCandidate.Enabled = false;
            this.btnChkPeptideCandidate.Location = new System.Drawing.Point(253, 13);
            this.btnChkPeptideCandidate.Name = "btnChkPeptideCandidate";
            this.btnChkPeptideCandidate.Size = new System.Drawing.Size(80, 23);
            this.btnChkPeptideCandidate.TabIndex = 40;
            this.btnChkPeptideCandidate.Text = "Candidates";
            this.btnChkPeptideCandidate.UseVisualStyleBackColor = true;
            this.btnChkPeptideCandidate.Click += new System.EventHandler(this.btnChkPeptideCandidate_Click);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(302, 45);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(28, 13);
            this.label14.TabIndex = 40;
            this.label14.Text = "mins";
            // 
            // rdoFastaOnly
            // 
            this.rdoFastaOnly.AutoSize = true;
            this.rdoFastaOnly.Location = new System.Drawing.Point(173, 19);
            this.rdoFastaOnly.Name = "rdoFastaOnly";
            this.rdoFastaOnly.Size = new System.Drawing.Size(73, 17);
            this.rdoFastaOnly.TabIndex = 32;
            this.rdoFastaOnly.Text = "Fasta only";
            this.rdoFastaOnly.UseVisualStyleBackColor = true;
            this.rdoFastaOnly.CheckedChanged += new System.EventHandler(this.rdoFastaOnly_CheckedChanged);
            // 
            // txtTolTime
            // 
            this.txtTolTime.Location = new System.Drawing.Point(270, 41);
            this.txtTolTime.Name = "txtTolTime";
            this.txtTolTime.Size = new System.Drawing.Size(29, 20);
            this.txtTolTime.TabIndex = 39;
            this.txtTolTime.Text = "8";
            this.txtTolTime.TextChanged += new System.EventHandler(this.txtTolTime_TextChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 129);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(113, 13);
            this.label10.TabIndex = 31;
            this.label10.Text = "Allow peptide mutation";
            // 
            // cboPepMutation
            // 
            this.cboPepMutation.FormattingEnabled = true;
            this.cboPepMutation.Items.AddRange(new object[] {
            "No Mutation",
            "Aspartic Acid(D) -> Asparagine (N)",
            "Any -> Asparagine (N)"});
            this.cboPepMutation.Location = new System.Drawing.Point(126, 126);
            this.cboPepMutation.Name = "cboPepMutation";
            this.cboPepMutation.Size = new System.Drawing.Size(202, 21);
            this.cboPepMutation.TabIndex = 30;
            this.cboPepMutation.SelectedIndexChanged += new System.EventHandler(this.cboPepMutation_SelectedIndexChanged);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(209, 45);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(55, 13);
            this.label13.TabIndex = 38;
            this.label13.Text = "Tolerance";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(4, 45);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(52, 13);
            this.label12.TabIndex = 37;
            this.label12.Text = "Time shift";
            // 
            // chkEnzy_GlucE
            // 
            this.chkEnzy_GlucE.AutoSize = true;
            this.chkEnzy_GlucE.Enabled = false;
            this.chkEnzy_GlucE.Location = new System.Drawing.Point(123, 71);
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
            this.chkEnzy_Trypsin.Enabled = false;
            this.chkEnzy_Trypsin.Location = new System.Drawing.Point(194, 71);
            this.chkEnzy_Trypsin.Name = "chkEnzy_Trypsin";
            this.chkEnzy_Trypsin.Size = new System.Drawing.Size(60, 17);
            this.chkEnzy_Trypsin.TabIndex = 28;
            this.chkEnzy_Trypsin.Text = "Trypsin";
            this.chkEnzy_Trypsin.UseVisualStyleBackColor = true;
            this.chkEnzy_Trypsin.CheckedChanged += new System.EventHandler(this.chkEnzy_Trypsin_CheckedChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(139, 45);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(28, 13);
            this.label11.TabIndex = 36;
            this.label11.Text = "mins";
            // 
            // chkEnzy_GlucED
            // 
            this.chkEnzy_GlucED.AutoSize = true;
            this.chkEnzy_GlucED.Enabled = false;
            this.chkEnzy_GlucED.Location = new System.Drawing.Point(266, 71);
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
            this.chkEnzy_None.Enabled = false;
            this.chkEnzy_None.Location = new System.Drawing.Point(65, 71);
            this.chkEnzy_None.Name = "chkEnzy_None";
            this.chkEnzy_None.Size = new System.Drawing.Size(52, 17);
            this.chkEnzy_None.TabIndex = 26;
            this.chkEnzy_None.Text = "None";
            this.chkEnzy_None.UseVisualStyleBackColor = true;
            this.chkEnzy_None.CheckedChanged += new System.EventHandler(this.chkEnzy_None_CheckedChanged);
            // 
            // txtShiftTime
            // 
            this.txtShiftTime.Location = new System.Drawing.Point(90, 41);
            this.txtShiftTime.Name = "txtShiftTime";
            this.txtShiftTime.Size = new System.Drawing.Size(43, 20);
            this.txtShiftTime.TabIndex = 35;
            this.txtShiftTime.Text = "8";
            this.txtShiftTime.TextChanged += new System.EventHandler(this.txtShiftTime_TextChanged);
            // 
            // cboMissCleavage
            // 
            this.cboMissCleavage.Enabled = false;
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
            this.cboMissCleavage.Location = new System.Drawing.Point(65, 99);
            this.cboMissCleavage.Name = "cboMissCleavage";
            this.cboMissCleavage.Size = new System.Drawing.Size(31, 21);
            this.cboMissCleavage.TabIndex = 7;
            this.cboMissCleavage.Text = "0";
            this.cboMissCleavage.SelectedIndexChanged += new System.EventHandler(this.cboMissCleavage_SelectedIndexChanged);
            // 
            // lstModification
            // 
            this.lstModification.Enabled = false;
            this.lstModification.FormattingEnabled = true;
            this.lstModification.Items.AddRange(new object[] {
            "Cys_CAM"});
            this.lstModification.Location = new System.Drawing.Point(274, 101);
            this.lstModification.Name = "lstModification";
            this.lstModification.Size = new System.Drawing.Size(54, 17);
            this.lstModification.TabIndex = 20;
            // 
            // cboShiftSign
            // 
            this.cboShiftSign.FormattingEnabled = true;
            this.cboShiftSign.Items.AddRange(new object[] {
            "+",
            "-"});
            this.cboShiftSign.Location = new System.Drawing.Point(57, 41);
            this.cboShiftSign.Name = "cboShiftSign";
            this.cboShiftSign.Size = new System.Drawing.Size(30, 21);
            this.cboShiftSign.TabIndex = 34;
            this.cboShiftSign.SelectedIndexChanged += new System.EventHandler(this.cboShiftSign_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(200, 103);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 13);
            this.label3.TabIndex = 25;
            this.label3.Text = "Modification";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Enabled = false;
            this.label1.Location = new System.Drawing.Point(4, 73);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 13);
            this.label1.TabIndex = 23;
            this.label1.Text = "Enzyme";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Enabled = false;
            this.label2.Location = new System.Drawing.Point(4, 102);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(182, 13);
            this.label2.TabIndex = 22;
            this.label2.Text = "Allow up to             missed cleavages";
            // 
            // rdoPeptideWithTime
            // 
            this.rdoPeptideWithTime.AutoSize = true;
            this.rdoPeptideWithTime.Location = new System.Drawing.Point(28, 36);
            this.rdoPeptideWithTime.Name = "rdoPeptideWithTime";
            this.rdoPeptideWithTime.Size = new System.Drawing.Size(148, 17);
            this.rdoPeptideWithTime.TabIndex = 33;
            this.rdoPeptideWithTime.Text = "Peptides with defined time";
            this.rdoPeptideWithTime.UseVisualStyleBackColor = true;
            this.rdoPeptideWithTime.Visible = false;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.chkIndividualReport);
            this.groupBox3.Controls.Add(this.chkCompletedOnly);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.cboTopRank);
            this.groupBox3.Location = new System.Drawing.Point(223, 306);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(197, 110);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Export";
            // 
            // chkIndividualReport
            // 
            this.chkIndividualReport.AutoSize = true;
            this.chkIndividualReport.Location = new System.Drawing.Point(13, 73);
            this.chkIndividualReport.Name = "chkIndividualReport";
            this.chkIndividualReport.Size = new System.Drawing.Size(129, 17);
            this.chkIndividualReport.TabIndex = 32;
            this.chkIndividualReport.Text = "Individual detail report";
            this.chkIndividualReport.UseVisualStyleBackColor = true;
            this.chkIndividualReport.CheckedChanged += new System.EventHandler(this.chkIndividualReport_CheckedChanged);
            // 
            // chkCompletedOnly
            // 
            this.chkCompletedOnly.AutoSize = true;
            this.chkCompletedOnly.Checked = true;
            this.chkCompletedOnly.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCompletedOnly.Location = new System.Drawing.Point(13, 41);
            this.chkCompletedOnly.Name = "chkCompletedOnly";
            this.chkCompletedOnly.Size = new System.Drawing.Size(137, 30);
            this.chkCompletedOnly.TabIndex = 33;
            this.chkCompletedOnly.Text = "Only complete structure\r\n(Cat 1 Only)";
            this.chkCompletedOnly.UseVisualStyleBackColor = true;
            // 
            // txtCompReward
            // 
            this.txtCompReward.Location = new System.Drawing.Point(158, 76);
            this.txtCompReward.Name = "txtCompReward";
            this.txtCompReward.Size = new System.Drawing.Size(29, 20);
            this.txtCompReward.TabIndex = 35;
            this.txtCompReward.Text = "1.0";
            this.txtCompReward.Visible = false;
            // 
            // lblCompReward
            // 
            this.lblCompReward.AutoSize = true;
            this.lblCompReward.Enabled = false;
            this.lblCompReward.Location = new System.Drawing.Point(28, 79);
            this.lblCompReward.Name = "lblCompReward";
            this.lblCompReward.Size = new System.Drawing.Size(124, 13);
            this.lblCompReward.TabIndex = 34;
            this.lblCompReward.Text = "Completed score reward:";
            this.lblCompReward.Visible = false;
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
            this.groupBox4.Controls.Add(this.chkUnknownPeptideSearch);
            this.groupBox4.Controls.Add(this.label4);
            this.groupBox4.Controls.Add(this.txtPeaKTol);
            this.groupBox4.Controls.Add(this.label5);
            this.groupBox4.Controls.Add(this.txtPrecusorTol);
            this.groupBox4.Location = new System.Drawing.Point(6, 306);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(211, 110);
            this.groupBox4.TabIndex = 3;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Torelance";
            // 
            // chkUnknownPeptideSearch
            // 
            this.chkUnknownPeptideSearch.AutoSize = true;
            this.chkUnknownPeptideSearch.Location = new System.Drawing.Point(13, 73);
            this.chkUnknownPeptideSearch.Name = "chkUnknownPeptideSearch";
            this.chkUnknownPeptideSearch.Size = new System.Drawing.Size(195, 30);
            this.chkUnknownPeptideSearch.TabIndex = 36;
            this.chkUnknownPeptideSearch.Text = "Search Y1 without matched peptide\r\n(Search for Cat. 2)";
            this.chkUnknownPeptideSearch.UseVisualStyleBackColor = true;
            // 
            // chkPrecursorCreate
            // 
            this.chkPrecursorCreate.AutoSize = true;
            this.chkPrecursorCreate.Checked = true;
            this.chkPrecursorCreate.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkPrecursorCreate.Location = new System.Drawing.Point(206, 59);
            this.chkPrecursorCreate.Name = "chkPrecursorCreate";
            this.chkPrecursorCreate.Size = new System.Drawing.Size(122, 17);
            this.chkPrecursorCreate.TabIndex = 26;
            this.chkPrecursorCreate.Text = "Precursor in MS/MS";
            this.chkPrecursorCreate.UseVisualStyleBackColor = true;
            this.chkPrecursorCreate.Visible = false;
            // 
            // btnScanList
            // 
            this.btnScanList.Location = new System.Drawing.Point(221, 41);
            this.btnScanList.Name = "btnScanList";
            this.btnScanList.Size = new System.Drawing.Size(24, 23);
            this.btnScanList.TabIndex = 33;
            this.btnScanList.Text = "...";
            this.btnScanList.UseVisualStyleBackColor = true;
            this.btnScanList.Click += new System.EventHandler(this.btnScanList_Click);
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.txtHCDScore);
            this.groupBox6.Controls.Add(this.label20);
            this.groupBox6.Controls.Add(this.chkSeqHCD);
            this.groupBox6.Controls.Add(this.chkHCD);
            this.groupBox6.Controls.Add(this.btnScanList);
            this.groupBox6.Controls.Add(this.rdoScanRange);
            this.groupBox6.Controls.Add(this.lblEndScan);
            this.groupBox6.Controls.Add(this.rdoScanList);
            this.groupBox6.Controls.Add(this.txtStart);
            this.groupBox6.Controls.Add(this.txtEnd);
            this.groupBox6.Location = new System.Drawing.Point(6, 83);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(414, 72);
            this.groupBox6.TabIndex = 26;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Scans";
            // 
            // txtHCDScore
            // 
            this.txtHCDScore.Enabled = false;
            this.txtHCDScore.Location = new System.Drawing.Point(355, 25);
            this.txtHCDScore.Name = "txtHCDScore";
            this.txtHCDScore.Size = new System.Drawing.Size(39, 20);
            this.txtHCDScore.TabIndex = 36;
            this.txtHCDScore.Text = "0.5";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Enabled = false;
            this.label20.Location = new System.Drawing.Point(273, 29);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(76, 13);
            this.label20.TabIndex = 35;
            this.label20.Text = "HCD Score >=";
            // 
            // chkSeqHCD
            // 
            this.chkSeqHCD.AutoSize = true;
            this.chkSeqHCD.Enabled = false;
            this.chkSeqHCD.Location = new System.Drawing.Point(274, 49);
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
            this.grpGlycan.Controls.Add(this.rdoNeuGc);
            this.grpGlycan.Controls.Add(this.txtHexNAc);
            this.grpGlycan.Controls.Add(this.txtdeHex);
            this.grpGlycan.Controls.Add(this.txtSia);
            this.grpGlycan.Controls.Add(this.label7);
            this.grpGlycan.Controls.Add(this.txtHex);
            this.grpGlycan.Controls.Add(this.label8);
            this.grpGlycan.Controls.Add(this.label9);
            this.grpGlycan.Controls.Add(this.chkNLinked);
            this.grpGlycan.Enabled = false;
            this.grpGlycan.Location = new System.Drawing.Point(7, 240);
            this.grpGlycan.Name = "grpGlycan";
            this.grpGlycan.Size = new System.Drawing.Size(413, 60);
            this.grpGlycan.TabIndex = 31;
            this.grpGlycan.TabStop = false;
            this.grpGlycan.Text = "Composition upper bound";
            // 
            // rdoNeuAc
            // 
            this.rdoNeuAc.AutoSize = true;
            this.rdoNeuAc.Checked = true;
            this.rdoNeuAc.Location = new System.Drawing.Point(241, 11);
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
            this.rdoNeuGc.Location = new System.Drawing.Point(241, 35);
            this.rdoNeuGc.Name = "rdoNeuGc";
            this.rdoNeuGc.Size = new System.Drawing.Size(59, 17);
            this.rdoNeuGc.TabIndex = 10;
            this.rdoNeuGc.Text = "NeuGc";
            this.rdoNeuGc.UseVisualStyleBackColor = true;
            // 
            // txtHexNAc
            // 
            this.txtHexNAc.Location = new System.Drawing.Point(128, 22);
            this.txtHexNAc.Name = "txtHexNAc";
            this.txtHexNAc.Size = new System.Drawing.Size(26, 20);
            this.txtHexNAc.TabIndex = 8;
            this.txtHexNAc.Text = "99";
            // 
            // txtdeHex
            // 
            this.txtdeHex.Location = new System.Drawing.Point(206, 22);
            this.txtdeHex.Name = "txtdeHex";
            this.txtdeHex.Size = new System.Drawing.Size(26, 20);
            this.txtdeHex.TabIndex = 7;
            this.txtdeHex.Text = "0";
            // 
            // txtSia
            // 
            this.txtSia.Location = new System.Drawing.Point(302, 22);
            this.txtSia.Name = "txtSia";
            this.txtSia.Size = new System.Drawing.Size(25, 20);
            this.txtSia.TabIndex = 6;
            this.txtSia.Text = "99";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(162, 26);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(38, 13);
            this.label7.TabIndex = 2;
            this.label7.Text = "deHex";
            // 
            // txtHex
            // 
            this.txtHex.Location = new System.Drawing.Point(38, 22);
            this.txtHex.Name = "txtHex";
            this.txtHex.Size = new System.Drawing.Size(25, 20);
            this.txtHex.TabIndex = 9;
            this.txtHex.Text = "99";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(75, 26);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(47, 13);
            this.label8.TabIndex = 1;
            this.label8.Text = "HexNAc";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 26);
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
            this.chkNLinked.Location = new System.Drawing.Point(338, 25);
            this.chkNLinked.Name = "chkNLinked";
            this.chkNLinked.Size = new System.Drawing.Size(69, 17);
            this.chkNLinked.TabIndex = 6;
            this.chkNLinked.Text = "N-Linked";
            this.chkNLinked.UseVisualStyleBackColor = true;
            // 
            // chkHuman
            // 
            this.chkHuman.AutoSize = true;
            this.chkHuman.Checked = true;
            this.chkHuman.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkHuman.Location = new System.Drawing.Point(140, 59);
            this.chkHuman.Name = "chkHuman";
            this.chkHuman.Size = new System.Drawing.Size(60, 17);
            this.chkHuman.TabIndex = 4;
            this.chkHuman.Text = "Human";
            this.chkHuman.UseVisualStyleBackColor = true;
            this.chkHuman.Visible = false;
            // 
            // chkAvgMass
            // 
            this.chkAvgMass.AutoSize = true;
            this.chkAvgMass.Checked = true;
            this.chkAvgMass.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAvgMass.Location = new System.Drawing.Point(31, 59);
            this.chkAvgMass.Name = "chkAvgMass";
            this.chkAvgMass.Size = new System.Drawing.Size(94, 17);
            this.chkAvgMass.TabIndex = 32;
            this.chkAvgMass.Text = "Average Mass";
            this.chkAvgMass.UseVisualStyleBackColor = true;
            this.chkAvgMass.Visible = false;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.btnGlycanList);
            this.groupBox5.Controls.Add(this.txtGlycanList);
            this.groupBox5.Location = new System.Drawing.Point(6, 181);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(414, 53);
            this.groupBox5.TabIndex = 29;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "List file";
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
            // chkGlycanList
            // 
            this.chkGlycanList.AutoSize = true;
            this.chkGlycanList.Checked = true;
            this.chkGlycanList.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkGlycanList.Location = new System.Drawing.Point(6, 161);
            this.chkGlycanList.Name = "chkGlycanList";
            this.chkGlycanList.Size = new System.Drawing.Size(94, 17);
            this.chkGlycanList.TabIndex = 5;
            this.chkGlycanList.Text = "Use glycan list";
            this.chkGlycanList.UseVisualStyleBackColor = true;
            this.chkGlycanList.CheckedChanged += new System.EventHandler(this.chkGlycanList_CheckedChanged);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(773, 24);
            this.menuStrip1.TabIndex = 33;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.singleScanModeToolStripMenuItem,
            this.mascotProteinIDExtractorToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // singleScanModeToolStripMenuItem
            // 
            this.singleScanModeToolStripMenuItem.Name = "singleScanModeToolStripMenuItem";
            this.singleScanModeToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.singleScanModeToolStripMenuItem.Text = "Single Scan Mode";
            this.singleScanModeToolStripMenuItem.Visible = false;
            this.singleScanModeToolStripMenuItem.Click += new System.EventHandler(this.singleScanModeToolStripMenuItem_Click);
            // 
            // mascotProteinIDExtractorToolStripMenuItem
            // 
            this.mascotProteinIDExtractorToolStripMenuItem.Name = "mascotProteinIDExtractorToolStripMenuItem";
            this.mascotProteinIDExtractorToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.mascotProteinIDExtractorToolStripMenuItem.Text = "MascotProteinIDExtractor";
            this.mascotProteinIDExtractorToolStripMenuItem.Click += new System.EventHandler(this.mascotProteinIDExtractorToolStripMenuItem_Click);
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.txtMaxGlycansToCompleteStruct_m);
            this.groupBox7.Controls.Add(this.txtTopBrancingPeaks_l);
            this.groupBox7.Controls.Add(this.txtTopCorePeaks_k);
            this.groupBox7.Controls.Add(this.txtTopPeaks_i);
            this.groupBox7.Controls.Add(this.label19);
            this.groupBox7.Controls.Add(this.label18);
            this.groupBox7.Controls.Add(this.label17);
            this.groupBox7.Controls.Add(this.label15);
            this.groupBox7.Location = new System.Drawing.Point(426, 27);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(342, 144);
            this.groupBox7.TabIndex = 39;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Sequencing Parameters";
            // 
            // txtMaxGlycansToCompleteStruct_m
            // 
            this.txtMaxGlycansToCompleteStruct_m.Location = new System.Drawing.Point(287, 116);
            this.txtMaxGlycansToCompleteStruct_m.Name = "txtMaxGlycansToCompleteStruct_m";
            this.txtMaxGlycansToCompleteStruct_m.Size = new System.Drawing.Size(41, 20);
            this.txtMaxGlycansToCompleteStruct_m.TabIndex = 9;
            this.txtMaxGlycansToCompleteStruct_m.Text = "3";
            // 
            // txtTopBrancingPeaks_l
            // 
            this.txtTopBrancingPeaks_l.Location = new System.Drawing.Point(287, 84);
            this.txtTopBrancingPeaks_l.Name = "txtTopBrancingPeaks_l";
            this.txtTopBrancingPeaks_l.Size = new System.Drawing.Size(41, 20);
            this.txtTopBrancingPeaks_l.TabIndex = 8;
            this.txtTopBrancingPeaks_l.Text = "100";
            // 
            // txtTopCorePeaks_k
            // 
            this.txtTopCorePeaks_k.Location = new System.Drawing.Point(287, 52);
            this.txtTopCorePeaks_k.Name = "txtTopCorePeaks_k";
            this.txtTopCorePeaks_k.Size = new System.Drawing.Size(41, 20);
            this.txtTopCorePeaks_k.TabIndex = 7;
            this.txtTopCorePeaks_k.Text = "150";
            // 
            // txtTopPeaks_i
            // 
            this.txtTopPeaks_i.Location = new System.Drawing.Point(287, 22);
            this.txtTopPeaks_i.Name = "txtTopPeaks_i";
            this.txtTopPeaks_i.Size = new System.Drawing.Size(41, 20);
            this.txtTopPeaks_i.TabIndex = 5;
            this.txtTopPeaks_i.Text = "30";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(19, 120);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(265, 13);
            this.label19.TabIndex = 4;
            this.label19.Text = "Add Max monosaccharides for incompleteing structure:";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(33, 88);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(251, 13);
            this.label18.TabIndex = 3;
            this.label18.Text = "Get top l peaks as sequencing \"Branch\" candidate:";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(41, 56);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(243, 13);
            this.label17.TabIndex = 2;
            this.label17.Text = "Get top k peaks as sequencing \"Core\" candidate:";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(172, 26);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(112, 13);
            this.label15.TabIndex = 0;
            this.label15.Text = "Get top i peaks as Y1:";
            // 
            // txtTopDiagPeaks_j
            // 
            this.txtTopDiagPeaks_j.Location = new System.Drawing.Point(219, 17);
            this.txtTopDiagPeaks_j.Name = "txtTopDiagPeaks_j";
            this.txtTopDiagPeaks_j.Size = new System.Drawing.Size(41, 20);
            this.txtTopDiagPeaks_j.TabIndex = 6;
            this.txtTopDiagPeaks_j.Text = "30";
            this.txtTopDiagPeaks_j.Visible = false;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(21, 20);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(194, 13);
            this.label16.TabIndex = 1;
            this.label16.Text = "Get top j peaks as peptide + core peak:";
            this.label16.Visible = false;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(274, 2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 37;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.label16);
            this.groupBox8.Controls.Add(this.button1);
            this.groupBox8.Controls.Add(this.txtTopDiagPeaks_j);
            this.groupBox8.Controls.Add(this.rdoPeptideWithTime);
            this.groupBox8.Controls.Add(this.chkHuman);
            this.groupBox8.Controls.Add(this.txtCompReward);
            this.groupBox8.Controls.Add(this.chkAvgMass);
            this.groupBox8.Controls.Add(this.lblCompReward);
            this.groupBox8.Controls.Add(this.chkPrecursorCreate);
            this.groupBox8.Location = new System.Drawing.Point(6, 420);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(328, 100);
            this.groupBox8.TabIndex = 40;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "Old component";
            this.groupBox8.Visible = false;
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(395, 420);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(273, 13);
            this.label21.TabIndex = 41;
            this.label21.Text = "Integrated score = alpha * core + beta * branch + extend";
            // 
            // frmBatch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnablePreventFocusChange;
            this.ClientSize = new System.Drawing.Size(773, 450);
            this.Controls.Add(this.groupBox8);
            this.Controls.Add(this.label21);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.groupBox7);
            this.Controls.Add(this.chkGlycanList);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.grpGlycan);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "frmBatch";
            this.Text = "GlycoSeq";
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
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
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
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox cboPepMutation;
        private System.Windows.Forms.RadioButton rdoPeptideWithTime;
        private System.Windows.Forms.RadioButton rdoFastaOnly;
        private System.Windows.Forms.CheckBox chkIndividualReport;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem singleScanModeToolStripMenuItem;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtShiftTime;
        private System.Windows.Forms.ComboBox cboShiftSign;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox txtTolTime;
        private System.Windows.Forms.Label label13;
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
        private System.Windows.Forms.RadioButton rdoMascotExtractor;
        private System.Windows.Forms.Button btnChkPeptideCandidate;
        private System.Windows.Forms.ToolStripMenuItem mascotProteinIDExtractorToolStripMenuItem;
        private System.Windows.Forms.CheckBox chkUnknownPeptideSearch;
        private System.Windows.Forms.TextBox txtHCDScore;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label lblDecoyMass;
        private System.Windows.Forms.TextBox txtDecoyMass;
        private System.Windows.Forms.CheckBox chkDecoy;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.TextBox txtScoreAlpha;
        private System.Windows.Forms.TextBox txtScoreBeta;
        private System.Windows.Forms.TextBox txtFDRPrecentage;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label21;
    }
}