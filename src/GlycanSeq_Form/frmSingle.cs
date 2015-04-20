using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using COL.GlycoLib;
using COL.MassLib;
using COL.ProtLib;
using COL.GlycoSequence;
//http://web.expasy.org/glycanmass/
//http://web.expasy.org/peptide_mass/
namespace GlycanSeq_Form
{
    public partial class frmSingle : Form
    {
        List<GlycanStructure> ReportStructure;
        DataTable dtTrees;
        public COL.GlycoSequence.GlycanSequencing GS = null;
        MSScan scan;
        float _torelance = 500.0f;
        float _precursorTorelance = 50.0f;
        float PredictedY1;
        private void frmSingle_Load(object sender, EventArgs e)
        {
            /*rdoPeptide.Checked = false;
            txtY1.Text = "1236.4983";
            txtPeptideSeq.Text = "VVHAVEVALATFNAESNGSYLQLVEISR";
            txtY1ChargeStatus.Text = "2";
            txtScanNo.Text = "1242";
            chkFuc.Checked = false;*/
        }

        public frmSingle()
        {

            InitializeComponent();
            dtTrees = new DataTable();
            DataColumn dcID = new DataColumn("ID", Type.GetType("System.Int32"));
            DataColumn dcMass = new DataColumn("Mass", Type.GetType("System.Single"));
            DataColumn dcScore = new DataColumn("Score", Type.GetType("System.Single"));
            DataColumn dcPPM = new DataColumn("PPM", Type.GetType("System.Single"));
            DataColumn dcIUPAC = new DataColumn("IUPAC", Type.GetType("System.String"));
            DataColumn dcStructure = new DataColumn("Structure", typeof(Image));
            
            dtTrees.Columns.Add(dcID);
            dtTrees.Columns.Add(dcMass);
            dtTrees.Columns.Add(dcScore);
            dtTrees.Columns.Add(dcPPM);
            dtTrees.Columns.Add(dcStructure);
            dtTrees.Columns.Add(dcIUPAC);

            dgView.DataSource = dtTrees;
            dgView.Columns[0].Width = 30;
            dgView.Columns[1].Width = 70;
            dgView.Columns[2].Width = 45;
            dgView.Columns[3].Width = 45;
            dgView.Columns[4].Width = 315;
            dgView.Columns[5].Width = 200;

            cboScan.Items.Add("2471-1113.1011");
            cboScan.Items.Add("2449-1113.1011");
            cboScan.Items.Add("1287-1162.33");
            cboScan.Items.Add("2196-1113.1011");
            cboScan.Items.Add("1407-1395.142");
            cboScan.Items.Add("1189-1395.142");
            cboScan.Items.Add("1325-1395.142");

        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Raw files (*.RAW,*.mzXML)|*.RAW;*.mzXML";
            openFileDialog1.RestoreDirectory = true;
            if (txtScanNo.Text == "" || txtY1ChargeStatus.Text == "" || (rdoY1.Checked == true && txtY1.Text == "") || (rdoPeptide.Checked == true && txtPeptideSeq.Text == ""))
            {
                MessageBox.Show("Please fill the information");
                return;
            }
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                _torelance = Convert.ToSingle(txtPeaKTol.Text);
                _precursorTorelance = Convert.ToSingle(txtPrecusorTol.Text);
                dtTrees.Rows.Clear();
                int ScanNo = 0;
                if (Int32.TryParse(txtScanNo.Text, out ScanNo) == false)
                {
                    MessageBox.Show("Input Scan Number Error");
                    return;
                }

                if (Path.GetExtension(openFileDialog1.FileName).ToLower() == ".raw")
                {

                    ThermoRawReader RawReader = new COL.MassLib.ThermoRawReader(openFileDialog1.FileName);
                    /*GlypID.Peaks.clsPeakProcessorParameters clsParameters = new GlypID.Peaks.clsPeakProcessorParameters();
                    clsParameters.SignalToNoiseThreshold = 0.0f;
                    clsParameters.PeakBackgroundRatio = 0.01f;*/
                    //RawReader.SetPeakProcessorParameter(2, 2);
                    scan = RawReader.ReadScan(ScanNo);
                }
                else
                {
                    //scan = new mzXMLReader(openFileDialog1.FileName).ReadScan(ScanNo);
                }

                //PlotScan(scan);
                //scan.ProcessFineMonoPeak();
                //int MonoPeak = scan.Peak.Length;
                //int DeisotopePea = scan.MonoTransfromResult.Length;
                //label1.Text = MonoPeak.ToString() + ";" + DeisotopePea.ToString();

                int NoNeuAc = 0;
                int NoNeuGc = 0;
                if (rdoNeuAc.Checked)
                {
                    NoNeuAc = Convert.ToInt32(txtSia.Text);
                }
                else
                {
                    NoNeuGc = Convert.ToInt32(txtSia.Text);
                }

                int PredictedY1CS = Convert.ToInt32(txtY1ChargeStatus.Text);
                float PredictedY1 = 0.0f;
               
                if (rdoPeptide.Checked)
                {

                    GS = new COL.GlycoSequence.GlycanSequencing(scan, txtPeptideSeq.Text,true, PredictedY1CS,
                                                                                            Convert.ToInt32(txtHex.Text),
                                                                                            Convert.ToInt32(txtHexNAc.Text),
                                                                                            Convert.ToInt32(txtdeHex.Text),
                                                                                            NoNeuAc, NoNeuGc, Path.GetDirectoryName(openFileDialog1.FileName).ToString(),
                                                                                            chkNLinked.Checked, Convert.ToSingle(txtPeaKTol.Text),
                                                                                            Convert.ToSingle(txtPrecusorTol.Text));
                }
                else
                {
                    AminoAcidMass AAMW = new AminoAcidMass();
                    PredictedY1 = (AAMW.GetAVGMonoMW(txtPeptideSeq.Text, true) + GlycanMass.GetGlycanAVGMass(Glycan.Type.HexNAc) + Atoms.ProtonMass * PredictedY1CS) /
                                 (float)PredictedY1CS;
                    PredictedY1 = Convert.ToSingle(scan.MSPeaks[MassUtility.GetClosestMassIdx(scan.MSPeaks, Convert.ToSingle(txtY1.Text))].MonoMass);
                    GS = new COL.GlycoSequence.GlycanSequencing(scan, PredictedY1, PredictedY1CS,
                                                                                            Convert.ToInt32(txtHex.Text),
                                                                                            Convert.ToInt32(txtHexNAc.Text),
                                                                                            Convert.ToInt32(txtdeHex.Text),
                                                                                            NoNeuAc, NoNeuGc, Path.GetDirectoryName(openFileDialog1.FileName).ToString(),
                                                                                            chkNLinked.Checked, Convert.ToSingle(txtPeaKTol.Text),
                                                                                            Convert.ToSingle(txtPrecusorTol.Text));
                }


      
                GS.NumbersOfPeaksForSequencing = 140;
                GS.UseAVGMass = chkAVGMass.Checked;
                //GS.DebugMode(@"E:\temp\SeqTmp\");
                GS.CreatePrecursotMZ = true;
                GS.RewardForCompleteStructure = 3;
                GS.StartSequencing();
                GS.GetTopRankScoreStructre(1);
                lstPeak.Items.Clear();


                //float[] _mz = new float[scan.MSPeaks.Count];
                //float[] _intesity = new float[scan.MSPeaks.Count];
                //for (int i = 0; i < scan.MSPeaks.Count; i++)
                //{
                //    _mz[i] = scan.MSPeaks[i].MonoMass;
                //    _intesity[i] = scan.MSPeaks[i].MonoIntensity;
                //}
                //MSScan _tmpScan = new MSScan(_mz, _intesity, 1226.2273922296611f, 4898.8771594054051f, 4);
                //COL.GlycoSequence.GlycanSequencing _Gs = new COL.GlycoSequence.GlycanSequencing(scan, PredictedY1, PredictedY1CS);
                //_Gs.NumbersOfPeaksForSequencing = 150;
                //_Gs.StartSequencing();

                lstPeak.Items.Add("Top " + GS.FilteredPeaks.Count.ToString() + "  peaks");
                lstPeak.Items.Add("m/z / normailzed intensity ");
                foreach (MSPoint p in GS.FilteredPeaks)
                {
                    lstPeak.Items.Add(p.Mass.ToString("0.0000") +"/" + p.Intensity.ToString("0.0000"));
                }
                //YxFinder yx = new YxFinder(scan.MSPeaks, PredictedY1, PredictedY1CS, 500.0f);
                //GS = new GlycanSequencing(scan, PredictedY1, PredictedY1CS,chkHex.Checked,chkHexNAc.Checked, chkFuc.Checked, rdoNeuAc.Checked, rdoNeuGC.Checked, Path.GetDirectoryName(openFileDialog1.FileName).ToString(), chkNLinked.Checked, Convert.ToSingle(txtPeaKTol.Text), Convert.ToSingle(txtPrecusorTol.Text));
                //DataGridViewRowCollection row = dgView.Rows;

    
                bool isFullSeq = false;
                ReportStructure = GS.SequencedStructures;
                if (ReportStructure.Count == 0)
                {
                    MessageBox.Show("No Structure Found");
                    return;
                }
                if (GS.FullSequencedStructures.Count != 0)
                {
                    ReportStructure = GS.FullSequencedStructures;
                    isFullSeq = true;
                }
                AminoAcidMass AA = new AminoAcidMass();
                for(int i =0;i<ReportStructure.Count;i++) 
                {
                    GlycanStructure gt = ReportStructure[i];
                    DataRow row = dtTrees.NewRow();
                    //row.Add(new Object[] (gt.Mass.ToString("0.0000"), gt.Score.ToString("0.0000"),gt.GetIUPACString()));
                    row["ID"] = i.ToString();
                    row["Mass"] = gt.GlycanAVGMonoMass.ToString("0.0000");
                    row["Score"] =Convert.ToSingle( (gt.Score).ToString("0.0000") );
                    row["PPM"] = MassUtility.GetMassPPM(gt.GlycanMonoMass + AA.GetMonoMW(GS.PeptideSeq, true), GS.PrecusorMonoMass);
                    row["IUPAC"] = gt.IUPACString;
                   
                    GlycansDrawer GDRaw = new GlycansDrawer(gt.IUPACString, false);
                    Image tmpImg = GDRaw.GetImage();
                    row["Structure"] = tmpImg;
                    dtTrees.Rows.Add(row);

                }
                //GS.SequencStructures[0].TheoreticalFragment
                dtTrees.DefaultView.Sort = "Mass DESC, Score  DESC";


                for (int i = 0; i < dgView.Rows.Count; i++)
                {
                    this.dgView.AutoResizeRow(i);
                    if (isFullSeq)
                    {
                        dgView.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                    }
                }
                dgView.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            }
        }


     
        private Image RotateImage(Image inputImg, double degreeAngle)
        {
            //Corners of the image
            PointF[] rotationPoints = { new PointF(0, 0),
                                        new PointF(inputImg.Width, 0),
                                        new PointF(0, inputImg.Height),
                                        new PointF(inputImg.Width, inputImg.Height)};

            //Rotate the corners
            PointMath.RotatePoints(rotationPoints, new PointF(inputImg.Width / 2.0f, inputImg.Height / 2.0f), degreeAngle);

            //Get the new bounds given from the rotation of the corners
            //(avoid clipping of the image)
            Rectangle bounds = PointMath.GetBounds(rotationPoints);

            //An empy bitmap to draw the rotated image
            Bitmap rotatedBitmap = new Bitmap(bounds.Width, bounds.Height);

            using (Graphics g = Graphics.FromImage(rotatedBitmap))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;

                //Transformation matrix
                Matrix m = new Matrix();
                m.RotateAt((float)degreeAngle, new PointF(inputImg.Width / 2.0f, inputImg.Height / 2.0f));
                m.Translate(-bounds.Left, -bounds.Top, MatrixOrder.Append); //shift to compensate for the rotation

                g.Transform = m;
                g.DrawImage(inputImg, 0, 0);
            }
            return (Image)rotatedBitmap;
        }
        public Image Resizing(Image img, double percentage)
        {
            //get the height and width of the image
            int originalW = img.Width;
            int originalH = img.Height;

            //get the new size based on the percentage change
            int resizedW = (int)(originalW * percentage);
            int resizedH = (int)(originalH * percentage);

            //create a new Bitmap the size of the new image
            Bitmap bmp = new Bitmap(resizedW, resizedH);
            //create a new graphic from the Bitmap
            Graphics graphic = Graphics.FromImage((Image)bmp);
            graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
            //draw the newly resized image
            graphic.DrawImage(img, 0, 0, resizedW, resizedH);
            //dispose and free up the resources
            graphic.Dispose();
            //return the image
            return (Image)bmp;
        }
        /*private void PlotScan(MSScan argScan)
        {

            ZedGraph.PointPairList pplPeak = new ZedGraph.PointPairList();
            for (int i = 0; i < argScan.ScanMZs.Length; i++)
            {
                pplPeak.Add(argScan.ScanMZs[i], argScan.ScanIntensities[i]);
            }
            ZedGraph.GraphPane Pane = zedScan.GraphPane;
            Pane.XAxis.MajorTic.IsInside = false;
            Pane.XAxis.MinorTic.IsInside = false;
            Pane.CurveList.Clear();
            Pane.AddStick("Peaks", pplPeak, Color.Red);
            Pane.XAxis.Scale.Min = argScan.ScanMZs[0] - 10;
            Pane.XAxis.Scale.Max = argScan.ScanMZs[argScan.ScanMZs.Length - 1] - 10;
            Pane.Title.Text = "No. " + txtScanNo.Text;
            zedScan.AxisChange();

        }*/



        private void dgView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            GlycanStructure Structure = ReportStructure[Convert.ToInt32(dgView.Rows[e.RowIndex].Cells[0].Value)];
            treeView1.Nodes.Clear();
            treeView1.Nodes.Add(TreeStructure2TreeView.Convert2TreeView(Structure.Root));
            treeView1.ExpandAll();
            if (GS.FullSequencedStructures.Count != 0)
            {
                Structure = GS.FullSequencedStructures[Convert.ToInt32(dgView.Rows[e.RowIndex].Cells[0].Value)];
            }

            ListFragementStructure(Structure);
            DrawsequencingGraph(Structure);
            ListIDPeak(Structure);
            float ParentMono = scan.ParentMonoMW;
            float ParentAVGMono = scan.ParentAVGMonoMW;
            /*lblPPM.Text = "                                 Mono       AVG\n" +
                                      "                  "+ParentMono.ToString() + "                 " + ParentAVGMono.ToString() + "\n" +
                                      "Hex:         " + (GS.PeptideMonoMass + Structure.GlycanMonoMass + GlycanMass.GetGlycanMass(Glycan.Type.Hex)).ToString("0000.000") + "(" + MassUtility.GetMassPPM(GS.PeptideMonoMass + Structure.GlycanMonoMass + GlycanMass.GetGlycanMass(Glycan.Type.Hex), ParentMono).ToString("00000.000") + ")   " +
                                      (GS.PeptideMonoMass + Structure.GlycanAVGMonoMass + GlycanMass.GetGlycanAVGMass(Glycan.Type.Hex)).ToString("0000.000") + "(" + MassUtility.GetMassPPM(GS.PeptideMonoMass + Structure.GlycanAVGMonoMass + GlycanMass.GetGlycanAVGMass(Glycan.Type.Hex), ParentAVGMono).ToString("00000.000") + ")\n" +
                                      "HexNAc:  " + (GS.PeptideMonoMass + Structure.GlycanMonoMass + GlycanMass.GetGlycanMass(Glycan.Type.HexNAc)).ToString("0000.000") + "(" + MassUtility.GetMassPPM(GS.PeptideMonoMass + Structure.GlycanMonoMass + GlycanMass.GetGlycanMass(Glycan.Type.HexNAc), ParentMono).ToString("00000.000") + ") " +
                                      (GS.PeptideMonoMass + Structure.GlycanAVGMonoMass + GlycanMass.GetGlycanAVGMass(Glycan.Type.HexNAc)).ToString("0000.000") + "(" + MassUtility.GetMassPPM(GS.PeptideMonoMass + Structure.GlycanAVGMonoMass + GlycanMass.GetGlycanAVGMass(Glycan.Type.HexNAc), ParentAVGMono).ToString("00000.000") + ")\n" +
                                      "deHex:     " + (GS.PeptideMonoMass + Structure.GlycanMonoMass + GlycanMass.GetGlycanMass(Glycan.Type.DeHex)).ToString("0000.000") + "(" + MassUtility.GetMassPPM(GS.PeptideMonoMass + Structure.GlycanMonoMass + GlycanMass.GetGlycanMass(Glycan.Type.DeHex), ParentMono).ToString("00000.000") + ") " +
                                      (GS.PeptideMonoMass + Structure.GlycanAVGMonoMass + GlycanMass.GetGlycanAVGMass(Glycan.Type.DeHex)).ToString("0000.000") + "(" + MassUtility.GetMassPPM(GS.PeptideMonoMass + Structure.GlycanAVGMonoMass + GlycanMass.GetGlycanAVGMass(Glycan.Type.DeHex), ParentAVGMono).ToString("00000.000") + ")\n" +
                                      "Sia:          " + (GS.PeptideMonoMass + Structure.GlycanMonoMass + GlycanMass.GetGlycanMass(Glycan.Type.NeuAc)).ToString("0000.000") + "(" + MassUtility.GetMassPPM(GS.PeptideMonoMass + Structure.GlycanMonoMass + GlycanMass.GetGlycanMass(Glycan.Type.NeuAc), ParentMono).ToString("00000.000") + ")  " +
                                      (GS.PeptideMonoMass + Structure.GlycanAVGMonoMass + GlycanMass.GetGlycanAVGMass(Glycan.Type.NeuAc)).ToString("0000.000") + "(" + MassUtility.GetMassPPM(GS.PeptideMonoMass + Structure.GlycanAVGMonoMass + GlycanMass.GetGlycanAVGMass(Glycan.Type.NeuAc), ParentAVGMono).ToString("00000.000") + ")\n";*/
            if (GS.FullSequencedStructures.Count != 0)
            {
                if (chkAVGMass.Checked && ParentAVGMono!=0.0f)
                {
                    lblPPM.Text = "AVG Precursor Mass: " + ParentAVGMono.ToString() + "\n" +
                          (GS.PeptideMonoMass + Structure.GlycanAVGMonoMass).ToString("0000.000") + "(" + MassUtility.GetMassPPM(GS.PeptideMonoMass + Structure.GlycanAVGMonoMass, ParentAVGMono).ToString("000.000") + ") ppm";
                }
                else
                {
                    lblPPM.Text = "Mono Precursor Mass: " + ParentMono.ToString() + "\n" +
                    (GS.PeptideMonoMass + Structure.GlycanMonoMass).ToString("0000.000") + "(" + MassUtility.GetMassPPM(GS.PeptideMonoMass + Structure.GlycanMonoMass, ParentMono).ToString("000.000") + ") ppm";
                }
            }
            else
            {
                if (chkAVGMass.Checked && ParentAVGMono != 0.0f)
                {
                    lblPPM.Text = "     AVG Mass\n" +
                              "Precursor Mass: " + ParentAVGMono.ToString() + "\n" +
                              "Extra Hex:         " + (GS.PeptideMonoMass + Structure.GlycanAVGMonoMass + GlycanMass.GetGlycanAVGMass(Glycan.Type.Hex)).ToString("0000.000") + "(" + MassUtility.GetMassPPM(GS.PeptideMonoMass + Structure.GlycanAVGMonoMass + GlycanMass.GetGlycanAVGMass(Glycan.Type.Hex), ParentAVGMono).ToString("00000.000") + ")\n" +
                              "Extra HexNAc:  " + (GS.PeptideMonoMass + Structure.GlycanAVGMonoMass + GlycanMass.GetGlycanAVGMass(Glycan.Type.HexNAc)).ToString("0000.000") + "(" + MassUtility.GetMassPPM(GS.PeptideMonoMass + Structure.GlycanAVGMonoMass + GlycanMass.GetGlycanAVGMass(Glycan.Type.HexNAc), ParentAVGMono).ToString("00000.000") + ")\n" +
                              "Extra deHex:     " + (GS.PeptideMonoMass + Structure.GlycanAVGMonoMass + GlycanMass.GetGlycanAVGMass(Glycan.Type.DeHex)).ToString("0000.000") + "(" + MassUtility.GetMassPPM(GS.PeptideMonoMass + Structure.GlycanAVGMonoMass + GlycanMass.GetGlycanAVGMass(Glycan.Type.DeHex), ParentAVGMono).ToString("00000.000") + ")\n" +
                              "Extra Sia:          " + (GS.PeptideMonoMass + Structure.GlycanAVGMonoMass + GlycanMass.GetGlycanAVGMass(Glycan.Type.NeuAc)).ToString("0000.000") + "(" + MassUtility.GetMassPPM(GS.PeptideMonoMass + Structure.GlycanAVGMonoMass + GlycanMass.GetGlycanAVGMass(Glycan.Type.NeuAc), ParentAVGMono).ToString("00000.000") + ")\n";
                }
                else
                {
                    lblPPM.Text = "     Mono Mass\n" +
                              "Precursor Mass: " + ParentMono.ToString() + "\n" +
                              "Extra Hex:         " + (GS.PeptideMonoMass + Structure.GlycanMonoMass + GlycanMass.GetGlycanMass(Glycan.Type.Hex)).ToString("0000.000") + "(" + MassUtility.GetMassPPM(GS.PeptideMonoMass + Structure.GlycanMonoMass + GlycanMass.GetGlycanMass(Glycan.Type.Hex), ParentMono).ToString("00000.000") + ")\n" +
                              "Extra HexNAc:  " + (GS.PeptideMonoMass + Structure.GlycanMonoMass + GlycanMass.GetGlycanMass(Glycan.Type.HexNAc)).ToString("0000.000") + "(" + MassUtility.GetMassPPM(GS.PeptideMonoMass + Structure.GlycanMonoMass + GlycanMass.GetGlycanMass(Glycan.Type.HexNAc), ParentMono).ToString("00000.000") + ")\n" +
                              "Extra deHex:     " + (GS.PeptideMonoMass + Structure.GlycanMonoMass + GlycanMass.GetGlycanMass(Glycan.Type.DeHex)).ToString("0000.000") + "(" + MassUtility.GetMassPPM(GS.PeptideMonoMass + Structure.GlycanMonoMass + GlycanMass.GetGlycanMass(Glycan.Type.DeHex), ParentMono).ToString("00000.000") + ")\n" +
                              "Extra Sia:          " + (GS.PeptideMonoMass + Structure.GlycanMonoMass + GlycanMass.GetGlycanMass(Glycan.Type.NeuAc)).ToString("0000.000") + "(" + MassUtility.GetMassPPM(GS.PeptideMonoMass + Structure.GlycanMonoMass + GlycanMass.GetGlycanMass(Glycan.Type.NeuAc), ParentMono).ToString("00000.000") + ")\n";

                }
            }
        }
        private void ListIDPeak(GlycanStructure argStructure)
        {
            DataTable dtIDPeak = new DataTable();
            DataColumn dcMass = new DataColumn("Glycopeptide m/z", Type.GetType("System.Single"));
            DataColumn dcScore = new DataColumn("Score", Type.GetType("System.Single"));
            DataColumn dcStructure = new DataColumn("Structure", typeof(Image));

            dtIDPeak.Columns.Add(dcMass);
            dtIDPeak.Columns.Add(dcScore);
            dtIDPeak.Columns.Add(dcStructure);
            dgIDPeak.DataSource = dtIDPeak;
            dgIDPeak.Columns[0].Width = 70;
            dgIDPeak.Columns[1].Width = 50;
            dgIDPeak.Columns[2].Width = 315;

            dtIDPeak.DefaultView.Sort = "Glycopeptide m/z";

            GlycansDrawer GDraw;
            foreach (GlycanTreeNode GT in argStructure.Root.FetchAllGlycanNode())
            {
                DataRow row = dtIDPeak.NewRow();                
                row[0] = GT.IDMass;
                row[1] = GT.IDIntensity;
                string tmp = argStructure.GetSequqncedIUPACwNodeID(GT.NodeID);
                GDraw = new GlycansDrawer(tmp);
                row[2] = GDraw.GetImage();
                dtIDPeak.Rows.Add(row);
                GT.GetIUPACString();
            }
            dgIDPeak.Sort(dgIDPeak.Columns[0], ListSortDirection.Descending);
            for (int i = 0; i < dtIDPeak.Rows.Count; i++)
            {
                this.dgIDPeak.AutoResizeRow(i);
            }
            this.dgIDPeak.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
        }
        private string GetRepresentStructure(GlycanStructure argCompleteGS, int argNodeID)
        {

            int ID = argNodeID;
            GlycanStructure Clone = (GlycanStructure)argCompleteGS.Clone();
            List<GlycanTreeNode> removelist = new List<GlycanTreeNode>();
            foreach (GlycanTreeNode node in Clone.Root.FetchAllGlycanNode())
            {
                if (node.NodeID > argNodeID)
                {
                    removelist.Add(node);
                }
            }
            removelist.Sort(delegate(GlycanTreeNode t1, GlycanTreeNode t2) { return t2.NodeID.CompareTo(t1.NodeID); });
            foreach (GlycanTreeNode node in removelist)
            {
                Clone.Root.RemoveGlycan(node);
            }
            return Clone.Root.GetIUPACString();
        }
        private void ListFragementStructure(GlycanStructure argStructure)
        {
            //dgDetail
            DataTable dtDetail = new DataTable();
            DataColumn dcID = new DataColumn("ID", Type.GetType("System.Int32"));
            DataColumn dcMass = new DataColumn("Mass", Type.GetType("System.Single"));
            DataColumn dcScore = new DataColumn("Score", Type.GetType("System.Single"));
            DataColumn dcStructure = new DataColumn("Structure", typeof(Image));

            dtDetail.Columns.Add(dcID);
            dtDetail.Columns.Add(dcMass);
            dtDetail.Columns.Add(dcScore);
            dtDetail.Columns.Add(dcStructure);

            dgDetail.DataSource = dtDetail;
            dgDetail.Columns[0].Width = 30;
            dgDetail.Columns[1].Width = 70;
            dgDetail.Columns[2].Width = 50;
            dgDetail.Columns[3].Width = 315;

            dtDetail.DefaultView.Sort = "Mass";
            int idx = 0;
            float TotalScore = 0.0f;
            GlycansDrawer GDRaw;
            foreach (GlycanTreeNode frm in argStructure.TheoreticalFragment)
            {
                DataRow row = dtDetail.NewRow();
                row[0] = idx;
                float glycanmass = COL.GlycoLib.GlycanMass.GetGlycanMasswithCharge(frm.GlycanType, argStructure.Charge) + PredictedY1 - GlycanMass.GetGlycanMasswithCharge(Glycan.Type.HexNAc, argStructure.Charge);
                
                int closepeakidx = MassUtility.GetClosestMassIdx(scan.MSPeaks, glycanmass);
                row[1] = glycanmass;
               
                if (MassUtility.GetMassPPM(glycanmass, scan.MSPeaks[closepeakidx].MonoisotopicMZ) < _torelance)
                {
                    row[2] = scan.MSPeaks[closepeakidx].MonoIntensity / scan.MaxIntensity;
                }
                else
                {
                    row[2] = 0.0f;
                }
                GDRaw = new GlycansDrawer(frm.GetIUPACString(), false);
                row[3] = GDRaw.GetImage();
                idx++;
                dtDetail.Rows.Add(row);
                TotalScore = TotalScore + Convert.ToSingle(row[2]);
            }

            DataRow dtrow = dtDetail.NewRow();
            dtrow[0] = idx;
            dtrow[1] = GlycanMass.GetGlycanMasswithCharge(argStructure.Root.GlycanType, argStructure.Charge) + PredictedY1 - GlycanMass.GetGlycanMasswithCharge(Glycan.Type.HexNAc, argStructure.Charge);
            dtrow[2] = TotalScore;
            GDRaw = new GlycansDrawer(argStructure.IUPACString, false);
            dtrow[3] = GDRaw.GetImage();
            dtDetail.Rows.Add(dtrow);


            for (int i = 0; i < dgDetail.Rows.Count; i++)
            {
                this.dgDetail.AutoResizeRow(i);
            }
        }

        public void DrawsequencingGraph(GlycanStructure argStructure)
        {
            zedSequence.GraphPane.GraphObjList.Clear();
            zedSequence.GraphPane.Legend.IsVisible = false;
            List<String> tmp = argStructure.Root.GetSequencingMapList();
            if (tmp.Count == 0)
            {
                return;
            }
            float Xmin = Convert.ToSingle(tmp[0].Split('-')[0]);
            float Xmax = Convert.ToSingle(tmp[tmp.Count - 1].Split('-')[2]);
            if (Xmax == 0.0f)
            {
                Xmax = scan.MSPeaks[scan.MSPeaks.Count - 1].MonoMass;
            }
            double YMax = 0.0;

            ZedGraph.PointPairList pplPeak = new ZedGraph.PointPairList();
            for (int i = 0; i < GS.FilteredPeaks.Count; i++)
            {
                if (GS.FilteredPeaks[i].Mass >= Xmin + 10.0f && GS.FilteredPeaks[i].Mass <= Xmax + 10.0f)
                {
                    if (GS.FilteredPeaks[i].Intensity > YMax)
                    {
                        YMax = GS.FilteredPeaks[i].Intensity;
                    }
                }
            }
            for (int i = 0; i < GS.FilteredPeaks.Count; i++)
            {
                if (GS.FilteredPeaks[i].Mass >= Xmin + 10.0f && GS.FilteredPeaks[i].Mass <= Xmax + 10.0f)
                {
                    pplPeak.Add(GS.FilteredPeaks[i].Mass, GS.FilteredPeaks[i].Intensity / YMax * 100.0f);
                }
            }

            ZedGraph.GraphPane Pane = zedSequence.GraphPane;


            Pane.XAxis.MajorTic.IsInside = false;
            Pane.XAxis.MinorTic.IsInside = false;
            Pane.CurveList.Clear();
            Pane.AddStick("Peaks", pplPeak, Color.Red);
            //Pane.XAxis.Scale.Min = Xmin - 10;
            //Pane.XAxis.Scale.Max = Xmax + 10;
           Pane.Title.Text = "No. " + txtScanNo.Text + "; Y1 m/z:" + argStructure.Y1.Mass.ToString("0.000");//+ "  Structure:" + Convert.ToString(dgView.Rows[e.RowIndex].Cells[0].Value);
            Pane.AxisChange();
            double YLevel = YMax;
            double outX, outY, outY2, diff;

            Pane.ReverseTransform(new Point(100, 100), out outX, out outY);
            Pane.ReverseTransform(new Point(100, 110), out outX, out outY2);
            diff = outY - outY2;
            GlycanTreeNode GT = argStructure.Root;//GS.GlycanTrees[e.RowIndex];

            //Peak Interval
            //List<string> SeqList = new List<string>();
            //for (int i = 0; i < GT.GetSequencingMapList().Count; i++)
            //{
            //    //Split the string
            //    string[] strArray = GT.GetSequencingMapList()[i].Split('-');
            //    ZedGraph.TextObj TxtObg = new ZedGraph.TextObj();
            //    TxtObg.Text = strArray[1];
            //    double Start = Convert.ToDouble(strArray[0]);
            //    double End = Convert.ToDouble(strArray[2]);

            //    System.Drawing.Drawing2D.DashStyle LineStyle = DashStyle.Solid;
            //    if (Start == 0)
            //    {
            //        if (strArray[1] == "HexNAc")
            //        {
            //            Start = End - GlycanMass.GetGlycanMasswithCharge(Glycan.Type.HexNAc, GT.Charge);
            //        }
            //        else if (strArray[1] == "DeHex")
            //        {
            //            Start = End - GlycanMass.GetGlycanMasswithCharge(Glycan.Type.DeHex, GT.Charge);
            //        }
            //        else if (strArray[1] == "Hex")
            //        {
            //            Start = End - GlycanMass.GetGlycanMasswithCharge(Glycan.Type.Hex, GT.Charge);
            //        }
            //        else if (strArray[1] == "NeuAc")
            //        {
            //            Start = End - GlycanMass.GetGlycanMasswithCharge(Glycan.Type.NeuAc, GT.Charge);
            //        }
            //        else
            //        {
            //            Start = End - GlycanMass.GetGlycanMasswithCharge(Glycan.Type.NeuGc, GT.Charge);
            //        }
            //        TxtObg.Text = TxtObg.Text + "?";
            //        LineStyle = DashStyle.Dash;
            //    }
            //    else
            //    {
            //        if (strArray[1] == "HexNAc")
            //        {
            //            Start = End - GlycanMass.GetGlycanMasswithCharge(Glycan.Type.HexNAc, GT.Charge);
            //        }
            //        else if (strArray[1] == "DeHex")
            //        {
            //            Start = End - GlycanMass.GetGlycanMasswithCharge(Glycan.Type.DeHex, GT.Charge);
            //        }
            //        else if (strArray[1] == "Hex")
            //        {
            //            Start = End - GlycanMass.GetGlycanMasswithCharge(Glycan.Type.Hex, GT.Charge);
            //        }
            //        else if (strArray[1] == "NeuAc")
            //        {
            //            Start = End - GlycanMass.GetGlycanMasswithCharge(Glycan.Type.NeuAc, GT.Charge);
            //        }
            //        else
            //        {
            //            Start = End - GlycanMass.GetGlycanMasswithCharge(Glycan.Type.NeuGc, GT.Charge);
            //        }
            //    }
            //    if (End == 0)
            //    {
            //        if (strArray[1] == "HexNAc")
            //        {
            //            End = Start + GlycanMass.GetGlycanMasswithCharge(Glycan.Type.HexNAc, GT.Charge);
            //        }
            //        else if (strArray[1] == "DeHex")
            //        {
            //            End = Start + GlycanMass.GetGlycanMasswithCharge(Glycan.Type.DeHex, GT.Charge);
            //        }
            //        else if (strArray[1] == "Hex")
            //        {
            //            End = Start + GlycanMass.GetGlycanMasswithCharge(Glycan.Type.Hex, GT.Charge);
            //        }
            //        else if (strArray[1] == "NeuAc")
            //        {
            //            End = Start + GlycanMass.GetGlycanMasswithCharge(Glycan.Type.NeuAc, GT.Charge);
            //        }
            //        else
            //        {
            //            End = Start + GlycanMass.GetGlycanMasswithCharge(Glycan.Type.NeuGc, GT.Charge);
            //        }
            //        TxtObg.Text = TxtObg.Text + "?";
            //        LineStyle = DashStyle.Dash;
            //    }
            //    //Determine the Y level
            //    int Ylevel = 0;
            //    if (SeqList.Count == 0)
            //    {
            //        SeqList.Add(Start.ToString() + "," + End.ToString() + ",0");
            //    }
            //    else
            //    {

            //        for (int j = i - 1; j >= 0; j--)
            //        {
            //            double PreStart = Convert.ToDouble(SeqList[j].Split(',')[0]);
            //            double PreEnd = Convert.ToDouble(SeqList[j].Split(',')[1]);
            //            int Prelevel = Convert.ToInt32(SeqList[j].Split(',')[2]);
            //            if ((PreStart <= Start && Start <= PreEnd))
            //            {
            //                if (Math.Abs(PreEnd - Start) <= 10.0)
            //                {
            //                    Ylevel = Prelevel;
            //                    break;
            //                }
            //                else
            //                {
            //                    Ylevel = Prelevel + 1;
            //                    break;
            //                }
            //            }
            //        }
            //        SeqList.Add(Start.ToString() + "," + End.ToString() + "," + Ylevel.ToString());
            //    }
            //    TxtObg.FontSpec.Size = TxtObg.FontSpec.Size * 0.8f;
            //    YLevel = YMax + diff * Ylevel;
            //    ZedGraph.LineObj Lne = new ZedGraph.LineObj(Start, YLevel + diff, Start, YLevel - diff); //Left V Line
            //    Pane.GraphObjList.Add(Lne);

            //    Lne = new ZedGraph.LineObj(End, YLevel + diff, End, YLevel - diff); //Right V Line
            //    Pane.GraphObjList.Add(Lne);

            //    Lne = new ZedGraph.LineObj(Start, YLevel, End, YLevel);  //Add Line
            //    Lne.Line.Style = LineStyle;
            //    Pane.GraphObjList.Add(Lne);

            //    //ZedGraph.ArrowObj arr= new ZedGraph.ArrowObj(Start, YLevel, End, YLevel);
            //    //arr.IsArrowHead = true;
            //    //arr.Line.Style = LineStyle;
            //    //Pane.GraphObjList.Add(arr);

            //    TxtObg.Location = new ZedGraph.Location(((Start + End) / 2), (double)YLevel, ZedGraph.CoordType.AxisXYScale);
            //    TxtObg.FontSpec.Border.IsVisible = false;
            //    TxtObg.Location.AlignH = ZedGraph.AlignH.Center;
            //    TxtObg.Location.AlignV = ZedGraph.AlignV.Center;
            //    Pane.GraphObjList.Insert(0, TxtObg);
            //}


            /////Annotation
            GlycansDrawer GDraw;
            double previousX2 = 0;

            List<GlycanTreeNode> Fragements = argStructure.Root.FetchAllGlycanNode();
            Fragements.Sort(delegate(GlycanTreeNode T1, GlycanTreeNode T2) { return Comparer<float>.Default.Compare(T1.IDMass, T2.IDMass); });
            foreach (GlycanTreeNode FGS in Fragements)
            {
                string Exp = argStructure.GetIUPACfromParentToNodeID(FGS.NodeID);
                //Queue<string> tmpQue = new Queue<string>();
                //for (int i = 0; i < Exp.Length; i++)
                //{
                //    int NodeID = 0;
                //    if (Exp[i].StartsWith("(") || Exp[i].StartsWith(")"))
                //    {
                //        NodeID = Convert.ToInt32(Exp[i].Split(',')[0].Substring(1));
                //    }
                //    else
                //    {
                //        NodeID = Convert.ToInt32(Exp[i].Split(',')[0]);
                //    }
                //    if (NodeID > FGS.NodeID)
                //    {
                //        if (Exp[i].StartsWith("(") || Exp[i].StartsWith(")"))
                //        {
                //            tmpQue.Enqueue(Exp[i].Substring(0, 1));  //
                //        }
                //    }
                //    else
                //    {
                //        tmpQue.Enqueue(Exp[i]);
                //    }
                //}
                //string IUPAC = "";
              
                //do
                //{
                //    string tmp =tmpQue.Dequeue();
                //    if(tmp == "(" && tmpQue.Peek() == ")" )
                //    {
                //    }



                //}while(tmpQue.Count!=0)

                string tmpIUPAC = argStructure.GetSequqncedIUPACwNodeID(FGS.NodeID);
                GDraw = new GlycansDrawer(tmpIUPAC);
                float glycopeptideMZ = FGS.IDMass;
                if (double.IsNaN(glycopeptideMZ))
                {
                    continue;
                }
                Image imgStructure = RotateImage(GDraw.GetImage(), 270);

                double PositionX = glycopeptideMZ;
                if (previousX2 >= PositionX)
                {
                    PositionX = previousX2 + 20;
                }

                ZedGraph.ImageObj glycan = new ZedGraph.ImageObj(imgStructure, PositionX, 130, imgStructure.Width * 0.3f, imgStructure.Height * 0.3f);

                glycan.IsScaled = true;
                glycan.Location.AlignV = ZedGraph.AlignV.Bottom;                
                Pane.GraphObjList.Add(glycan);

                ZedGraph.TextObj txtGlycanMz = new ZedGraph.TextObj(glycopeptideMZ.ToString("0.000"), 100, 140);
                txtGlycanMz.Location.X = Pane.GraphObjList[Pane.GraphObjList.Count - 1].Location.X1+ (Pane.GraphObjList[Pane.GraphObjList.Count - 1].Location.X2 - Pane.GraphObjList[Pane.GraphObjList.Count - 1].Location.X1)/2;
                txtGlycanMz.FontSpec.Size = txtGlycanMz.FontSpec.Size * 0.5f;
                txtGlycanMz.FontSpec.Border.IsVisible = false;
                Pane.GraphObjList.Add(txtGlycanMz);

                if (Pane.GraphObjList[Pane.GraphObjList.Count - 1].Location.X2 > Pane.GraphObjList[Pane.GraphObjList.Count - 2].Location.X2)
                {
                    previousX2 = Pane.GraphObjList[Pane.GraphObjList.Count - 1].Location.X2;
                }
                else
                {
                    previousX2 = Pane.GraphObjList[Pane.GraphObjList.Count - 2].Location.X2;
                }

            }
            Pane.AxisChange();

            Pane.YAxis.Scale.Max = 145;
            Pane.XAxis.Scale.Min = Convert.ToInt32(argStructure.Y1.Mass - 100);
            Pane.XAxis.Scale.Max = pplPeak[pplPeak.Count - 1].X + 100;


            ////////////
            //Glycan Structure on the Right Top Header
            ////////////
            GDraw = new GlycansDrawer(argStructure.IUPACString, false);
            Image imgStruc = GDraw.GetImage();
            ZedGraph.ImageObj fullStructure = new ZedGraph.ImageObj(imgStruc, 0.01f, 0.01f, imgStruc.Width, imgStruc.Height);

            fullStructure.IsScaled = false;
            Pane.GraphObjList.Add(fullStructure);

            Pane.GraphObjList[Pane.GraphObjList.Count - 1].Location.CoordinateFrame = ZedGraph.CoordType.PaneFraction;
            zedSequence.AxisChange();
            zedSequence.Refresh();

        }

        ZedGraph.TextObj MassStartValue;
        ZedGraph.TextObj MassEndValue;
        ZedGraph.TextObj MassDiffValue;
        ZedGraph.LineObj MassLine;
        private bool zedSequence_MouseDownEvent(ZedGraph.ZedGraphControl sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {

                double outX, outY;
                zedSequence.GraphPane.ReverseTransform(new Point(e.X, e.Y), out outX, out outY);
                MassStartValue = new ZedGraph.TextObj(outX.ToString("0.00"), outX, outY);
                MassStartValue.FontSpec.Border.IsVisible = false;
                MassStartValue.FontSpec.Size = MassStartValue.FontSpec.Size * 0.7f;

                zedSequence.GraphPane.GraphObjList.Insert(0, MassStartValue);
                zedSequence.Refresh();
            }

            return default(bool);
        }

        private void zedSequence_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (zedSequence.GraphPane.GraphObjList.Count != 0)
                {

                    zedSequence.GraphPane.GraphObjList.Remove(MassEndValue);
                    zedSequence.GraphPane.GraphObjList.Remove(MassDiffValue);
                    zedSequence.GraphPane.GraphObjList.Remove(MassLine);
                    zedSequence.Refresh();
                }
                double outX, outY;

                zedSequence.GraphPane.ReverseTransform(new Point(e.X, e.Y), out outX, out outY);
                outY = MassStartValue.Location.Y;

                //Add Line
                MassLine = new ZedGraph.LineObj(MassStartValue.Location.X, MassStartValue.Location.Y, outX, MassStartValue.Location.Y);
                MassLine.Line.Style = DashStyle.Dash;
                MassLine.Line.Color = Color.Blue;
                zedSequence.GraphPane.GraphObjList.Insert(1, MassLine);



                MassEndValue = new ZedGraph.TextObj(outX.ToString("0.00"), outX, outY);
                MassEndValue.FontSpec.Border.IsVisible = false;
                MassEndValue.FontSpec.Size = MassEndValue.FontSpec.Size * 0.5f;
                zedSequence.GraphPane.GraphObjList.Insert(0, MassEndValue);
                double massdiff = Convert.ToDouble(MassEndValue.Text) - Convert.ToDouble(MassStartValue.Text);
                double diffLoc = (MassStartValue.Location.X + MassEndValue.Location.X) / 2;
                MassDiffValue = new ZedGraph.TextObj(Math.Abs(massdiff).ToString("0.00"), diffLoc, outY);
                MassDiffValue.FontSpec.Border.IsVisible = false;
                MassDiffValue.FontSpec.Size = MassDiffValue.FontSpec.Size * 0.8f;
                zedSequence.GraphPane.GraphObjList.Insert(0, MassDiffValue);


                zedSequence.Refresh();
            }

        }

        private bool zedSequence_MouseUpEvent(ZedGraph.ZedGraphControl sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                //Clean the object
                if (zedSequence.GraphPane.GraphObjList.Count != 0)
                {
                    zedSequence.GraphPane.GraphObjList.Remove(MassStartValue);
                    zedSequence.GraphPane.GraphObjList.Remove(MassEndValue);
                    zedSequence.GraphPane.GraphObjList.Remove(MassDiffValue);
                    zedSequence.GraphPane.GraphObjList.Remove(MassLine);

                    zedSequence.Refresh();
                }
            }
            return default(bool);
        }


        private void cboScan_SelectedIndexChanged(object sender, EventArgs e)
        {
            string[] tmpAry = cboScan.Text.Split('-');
            txtScanNo.Text = tmpAry[0];
            txtY1.Text = tmpAry[1];
        }

        private void rdoY1_CheckedChanged(object sender, EventArgs e)
        {
            txtY1.Enabled = rdoY1.Checked;
            txtPeptideSeq.Enabled = !rdoY1.Checked;
        }





        private void dgView_Sorted(object sender, EventArgs e)
        {
            for (int i = 0; i < dgView.Rows.Count; i++)
            {
                this.dgView.AutoResizeRow(i);
                if (GS.FullSequencedStructures.Count != 0)
                {
                    dgView.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                }
            }
        }

        private void txtPeptideSeq_TextChanged(object sender, EventArgs e)
        {
            if (txtY1ChargeStatus.Text == "")
            {
                return;
            }
            AminoAcidMass AAMW = new AminoAcidMass();
            int Y1ChargeSt = Convert.ToInt32(txtY1ChargeStatus.Text);

            double PeptideMass;
            if (chkAVGMass.Checked)
            {
                PeptideMass = AAMW.GetAVGMonoMW(txtPeptideSeq.Text, true);
                 txtY1.Text = Convert.ToString( (float)(PeptideMass + GlycanMass.GetGlycanAVGMass(Glycan.Type.HexNAc) + COL.MassLib.Atoms.ProtonMass * Y1ChargeSt) / Y1ChargeSt);
            }
            else
            {
                PeptideMass = AAMW.GetMonoMW(txtPeptideSeq.Text, true);
                 txtY1.Text = Convert.ToString( (float)(PeptideMass + GlycanMass.GetGlycanMass(Glycan.Type.HexNAc) + COL.MassLib.Atoms.ProtonMass * Y1ChargeSt) / Y1ChargeSt);

            }
            lblMass.Text = "Peptide m/z:" + PeptideMass.ToString();

            
           
        }



 

    }    
}
