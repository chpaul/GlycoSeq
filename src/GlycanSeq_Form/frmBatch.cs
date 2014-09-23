using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Net;
using COL.GlycoSequence;
using COL.GlycoLib;
using COL.MassLib;
using COL.ProtLib;
namespace GlycanSeq_Form
{
    public partial class frmBatch : Form
    {
        List<int> ScanNum;
        //List<string> _peptideSeq ;
        //List<float>_peptideMass ;
        List<GlycanCompound> _glycanCompounds;
        RawReader Raw;
        Dictionary<double, GlycanCompound> _MassGlycanMapping;
        List<float> GlycanCompoundMassList;

        public frmBatch()
        {
            InitializeComponent();
            cboMissCleavage.SelectedItem = "0";
            ScanNum = new List<int>();
        }
        private void btnRawBrowse_Click(object sender, EventArgs e)
        {

            openFileDialog1.FileName = "";
            openFileDialog1.Filter = "Xcalibur file (*.RAW)|*.RAW";
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtRaw.Text = openFileDialog1.FileName;

                Raw = new RawReader(openFileDialog1.FileName, enumRawDataType.raw);
                txtStart.Text = "1";
                txtEnd.Text = Raw.NumberOfScans.ToString();
            }
        }
        private void btnPeptideBrowse_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = "";
            openFileDialog1.Filter = "fasta file (*.fasta)|*.fasta";
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtFasta.Text = openFileDialog1.FileName;
            }
        }
        private void btnScanList_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = "";
            openFileDialog1.Filter = "Scan list file (*.*)|*.*";
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtScanFileList.Text = openFileDialog1.FileName;
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            //folderBrowserDialog1.SelectedPath = Path.GetDirectoryName(txtRaw.Text);
            saveFileDialog1.InitialDirectory = Path.GetDirectoryName(txtRaw.Text);
            string FileName = Path.GetDirectoryName(txtRaw.Text) + "\\" + Path.GetFileNameWithoutExtension(txtRaw.Text) +"_" +txtStart.Text+"~"+txtEnd.Text+ "-" + DateTime.Now.ToString("yyMMdd-HHmm");
            if (chkHCD.Checked)
            {
                FileName = FileName + "_HCD";
            }
            if (chkSeqHCD.Checked)
            {
                FileName = FileName + "_SeqHCD";
            }
            if (chkGlycanList.Checked)
            {
                FileName = FileName + "_GlycanList";
            }
            saveFileDialog1.FileName = FileName + ".htm";
            saveFileDialog1.Filter = "HTML file (*.htm)|*.htm";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)            
            {

                if (txtFasta.Text != "" && txtRaw.Text != "" && txtStart.Text != "" && txtEnd.Text != "")
                {
                    List<Protease.Type> ProteaseType = new List<Protease.Type>();

                    if (chkEnzy_Trypsin.Checked)//Trypsin
                    {
                        ProteaseType.Add(Protease.Type.Trypsin);
                    }
                    if (chkEnzy_GlucE.Checked)//GlucE
                    {
                        ProteaseType.Add(Protease.Type.GlucE);
                    }
                    if(chkEnzy_GlucED.Checked) //GlucED
                    {
                        ProteaseType.Add( Protease.Type.GlucED);
                    }
                    if (chkEnzy_None.Checked || ProteaseType.Count==0)
                    {
                        ProteaseType.Add(Protease.Type.None);
                    }


                    if (cboMissCleavage.SelectedItem.ToString() == "")
                    {
                        cboMissCleavage.SelectedItem = "0";
                    }
                    if (txtCompReward.Text == "")
                    {
                        txtCompReward.Text = "1.0";
                    }
                   

                    if (!Directory.Exists(Path.GetDirectoryName(saveFileDialog1.FileName) + "\\Pics"))
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(saveFileDialog1.FileName) + "\\Pics");
                    }
                    frmInvokeProcesses frmProcess;
                    if (chkGlycanList.Checked)
                    {
                        GenerateGlycanList();

                        frmProcess = new frmInvokeProcesses(Convert.ToInt32(txtStart.Text),
                                                                                               Convert.ToInt32(txtEnd.Text),
                                                                                               Convert.ToSingle(txtPeaKTol.Text),
                                                                                               Convert.ToSingle(txtPrecusorTol.Text),
                                                                                               chkNLinked.Checked,
                                                                                               rdoNeuAc.Checked,
                                                                                               _glycanCompounds,
                                                                                               _MassGlycanMapping,
                                                                                               GlycanCompoundMassList,
                                                                                               txtGlycanList.Text,
                                                                                               txtRaw.Text,
                                                                                               txtFasta.Text,
                                                                                               ProteaseType,
                                                                                               Convert.ToInt32(cboMissCleavage.SelectedItem.ToString()),
                                                                                               chkAvgMass.Checked,
                                                                                               chkHCD.Checked,
                                                                                               chkSeqHCD.Checked,
                                                                                               saveFileDialog1.FileName,
                                                                                               Convert.ToInt32(cboTopRank.Text),
                                                                                               chkCompletedOnly.Checked,
                                                                                               Convert.ToSingle(txtCompReward.Text)                                                                                               
                                                                                               );
                    }
                    else
                    {
                        frmProcess = new frmInvokeProcesses(Convert.ToInt32(txtStart.Text),
                                                                                            Convert.ToInt32(txtEnd.Text),
                                                                                            Convert.ToSingle(txtPeaKTol.Text),
                                                                                            Convert.ToSingle(txtPrecusorTol.Text),
                                                                                            chkNLinked.Checked,
                                                                                            rdoNeuAc.Checked,
                                                                                            Convert.ToInt32(txtHexNAc.Text),
                                                                                            Convert.ToInt32(txtHex.Text),
                                                                                            Convert.ToInt32(txtdeHex.Text),
                                                                                            Convert.ToInt32(txtSia.Text),
                                                                                            txtRaw.Text,
                                                                                            txtFasta.Text,
                                                                                            ProteaseType,
                                                                                            Convert.ToInt32(cboMissCleavage.SelectedItem.ToString()),
                                                                                            chkAvgMass.Checked,
                                                                                            chkHCD.Checked,
                                                                                            chkSeqHCD.Checked,
                                                                                            saveFileDialog1.FileName,
                                                                                            Convert.ToInt32(cboTopRank.Text),
                                                                                            chkCompletedOnly.Checked,
                                                                                            Convert.ToSingle(txtCompReward.Text)
                                                                                            );

                    }
                    frmProcess.ShowDialog();
                }

                #region No backgroupworker
                /*if (txtExport.Text != "" && txtFasta.Text != "" && txtRaw.Text != "" && txtStart.Text != "" && txtEnd.Text != "")
            {
                int OutputRowCount = 0;
                
                FileInfo NewFile = new FileInfo(txtExport.Text );
                if (NewFile.Exists)
                {
                    File.Delete(NewFile.FullName);
                }
                OfficeOpenXml.ExcelPackage pck = new OfficeOpenXml.ExcelPackage(NewFile);
                
                if (pck.Workbook.Worksheets[Path.GetFileNameWithoutExtension(txtRaw.Text)] != null)
                {
                    pck.Workbook.Worksheets.Delete(Path.GetFileNameWithoutExtension(txtRaw.Text));
                }

                var ws = pck.Workbook.Worksheets.Add(Path.GetFileNameWithoutExtension(txtRaw.Text));
                ws.DefaultRowHeight = 50;
                OutputRowCount++;
                ws.Cells[OutputRowCount, 1].Value = "CID_Scan";
                ws.Cells[OutputRowCount, 2].Value = "Parent_Scan";
                ws.Cells[OutputRowCount, 3].Value = "Parent_Mz";
                ws.Cells[OutputRowCount, 4].Value = "GlycoPeptide_Mz";
                ws.Cells[OutputRowCount, 5].Value = "Y1_Mz";
                ws.Cells[OutputRowCount, 6].Value = "Peptide_Sequence";
                ws.Cells[OutputRowCount, 7].Value = "Glycan IUPAC";
                ws.Cells[OutputRowCount, 8].Value = "No_of_Glycans";
                ws.Cells[OutputRowCount, 9].Value = "HexNac-Hex-DeHex-NeuAc";
                ws.Cells[OutputRowCount, 10].Value = "Full_Structure";
                ws.Cells[OutputRowCount, 11].Value = "Score";
                ws.Cells[OutputRowCount, 12].Value = "Picture";
                

                _torelance = Convert.ToSingle(txtPeaKTol.Text);
                _precursorTorelance = Convert.ToSingle(txtPrecusorTol.Text);
                ScanNum.Clear();
                if (rdoScanRange.Checked)
                {
                    for (int i = Convert.ToInt32(txtStart.Text); i <= Convert.ToInt32(txtEnd.Text); i++)
                    {
                        ScanNum.Add(i);
                    }
                }
                else
                {
                    StreamReader sr = new StreamReader(txtScanFileList.Text);
                    do
                    {
                        ScanNum.Add(Convert.ToInt32(sr.ReadLine()));
                    } while (!sr.EndOfStream);
                    ScanNum.Sort();
                    sr.Close();
                }
                List<ProteinInfo> Proteins = ProteinParser(txtFasta.Text);

                XRawReader Raw = new XRawReader(txtRaw.Text);
               

                //if (rdoScanList.Checked)  //filter out scan no in list 
                //{
                //    List<MSScan> tmpMSScans = new List<MSScan>();
                //    foreach (MSScan scan in MSScans)
                //    {
                //        if (ScanNum.Contains(scan.ScanNo))
                //        {
                //            tmpMSScans.Add(scan);
                //        }
                //    }
                //    MSScans = tmpMSScans;
                //}

                if (chkGlycanList.Checked)
                {
                    GenerateGlycanList();
                }
                //StreamWriter sw = new StreamWriter(txtExport.Text);
                //sw.WriteLine("CID_Scan,Parent_Scan,Parent_Z,GlycoPeptideMz,Y1_Mz,Peptide_Seq,GlycanStructure,No_of_Glycans,HexNac-Hex-DeHex-NeuAc,Full_Seqquence,Score");

                float PeakTolerance = Convert.ToSingle(txtPeaKTol.Text);
                float PrecursorTolerance =  Convert.ToSingle(txtPrecusorTol.Text);
                AminoAcidMass AAMW = new AminoAcidMass();
                if (cboMissCleavage.SelectedItem.ToString() == "")
                {
                    cboMissCleavage.SelectedItem = "0";
                }
                //List<MSScan> MSScans = Raw.ReadScans(ScanNum[0], ScanNum[ScanNum.Count - 1]);
                for(int k = 0 ;k<ScanNum.Count;k++)                
                {
                    MSScan _scan = Raw.ReadScan(ScanNum[k]);
                    if (_scan.MsLevel == 1 || _scan.MSPeaks.Count==0)
                    {
                        continue;
                    }
                    int PrecursorCharge = _scan.ParentCharge;
                    foreach (ProteinInfo PInfo in Proteins)
                    {
                        List<string> Peptides  = new List<string>();
                        if (cboEnzyme.SelectedItem.ToString() == "None")
                        {
                            Peptides.Add(PInfo.Sequence);
                        }
                        else if (cboEnzyme.SelectedItem.ToString() == "Trypsin")//Trypsin
                        {
                            if (chkNLinked.Checked)
                            {
                                Peptides.AddRange(PInfo.NGlycopeptide(Convert.ToInt32(cboMissCleavage.SelectedItem.ToString()), Protease.Type.Trypsin));
                            }
                            else
                            {
                                Peptides.AddRange(PInfo.OGlycopeptide(Convert.ToInt32(cboMissCleavage.SelectedItem.ToString()), Protease.Type.Trypsin));
                            }
                        }
                        else if (cboEnzyme.SelectedItem.ToString() == "Gluc(E)")//GlucE
                        {
                            if (chkNLinked.Checked)
                            {
                                Peptides.AddRange(PInfo.NGlycopeptide(Convert.ToInt32(cboMissCleavage.SelectedItem.ToString()), Protease.Type.GlucE));
                            }
                            else
                            {
                                Peptides.AddRange(PInfo.OGlycopeptide(Convert.ToInt32(cboMissCleavage.SelectedItem.ToString()), Protease.Type.GlucE));
                            }
                        }
                        else //GlucED
                        {
                            if (chkNLinked.Checked)
                            {
                                Peptides.AddRange(PInfo.NGlycopeptide(Convert.ToInt32(cboMissCleavage.SelectedItem.ToString()), Protease.Type.GlucED));
                            }
                            else
                            {
                                Peptides.AddRange(PInfo.OGlycopeptide(Convert.ToInt32(cboMissCleavage.SelectedItem.ToString()), Protease.Type.GlucED));
                            }
                        }
                        // 
                        foreach (string Peptide in Peptides)
                        {
                             float PeptideMass = AAMW.GetAVGMonoMW(Peptide, true);

                            for (int i = PrecursorCharge - 1; i <= PrecursorCharge; i++)
                            {
                                int Y1ChargeSt = i;                          
                                float PredictedY1 = 0.0f;
                                PredictedY1 = (float)(PeptideMass + GlycanMass.GetGlycanAVGMass(Glycan.Type.HexNAc) + COL.MassLib.Atoms.ProtonMass * Y1ChargeSt) / Y1ChargeSt;
                                COL.GlycoSequence.GlycanSequencing GS =null;
                                if (chkGlycanList.Checked)
                                {
                                    float GlycanMonoMass = 0.0f;
                                    float PrecursorMono = 0.0f;
                                    if (_scan.ParentAVGMonoMW != 0.0)
                                    {
                                        GlycanMonoMass = _scan.ParentAVGMonoMW - PeptideMass + (Atoms.HydrogenAVGMass * 2 + Atoms.OxygenAVGMass); 
                                        PrecursorMono = _scan.ParentAVGMonoMW;
                                    }
                                    else
                                    {
                                        GlycanMonoMass = (_scan.ParentMZ - Atoms.ProtonMass) * _scan.ParentCharge - PeptideMass + (Atoms.HydrogenAVGMass * 2 + Atoms.OxygenAVGMass); 
                                        PrecursorMono = _scan.ParentMonoMW;
                                    }
                                    GlycanCompound ClosedGlycan = _glycanCompounds[MassUtility.GetClosestMassIdx(GlycanCompoundMassList, GlycanMonoMass)];
                                    if (Math.Abs(ClosedGlycan.AVGMass- GlycanMonoMass) <= PeakTolerance)
                                    {
                                        GS = new GlycanSequencing(_scan, Peptide, true, Y1ChargeSt, ClosedGlycan.NoOfHex, ClosedGlycan.NoOfHexNAc, ClosedGlycan.NoOfDeHex, ClosedGlycan.NoOfSia, 0, @"d:\tmp", true, PeakTolerance, PrecursorTolerance);
                                    }
                                    else
                                    {
                                        continue;
                                    }

                                }
                                else
                                {
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
                                    GS = new COL.GlycoSequence.GlycanSequencing(_scan, PredictedY1, Y1ChargeSt,
                                                                                                                          Convert.ToInt32(txtHex.Text),
                                                                                                                          Convert.ToInt32(txtHexNAc.Text),
                                                                                                                          Convert.ToInt32(txtdeHex.Text),
                                                                                                                          NoNeuAc, NoNeuGc, Path.GetDirectoryName(openFileDialog1.FileName).ToString(),
                                                                                                                          chkNLinked.Checked, PeakTolerance, PrecursorTolerance
                                                                                                                        );
                                }
                                GS.NumbersOfPeaksForSequencing = 140;
                                GS.UseAVGMass =true;
                                GS.CreatePrecursotMZ = true;
                                if (chkPrecursorCreate.Checked)
                                {
                                    GS.CreatePrecursotMZ = true;
                                }
                                GS.StartSequencing();
                                int NumOfFullStructureWithNCore = 0;
                                int TopScoreCount = 0;
                                double MaxScore = 0.0;
                                if (GS.SequencedStructures.Count >= 1)
                                {
                                    //Get overall stat
                                    GlycanStructure MaxMatch = null;
                                    double SmallestPPM = 100000.0;
                                    foreach (GlycanStructure GTree in GS.SequencedStructures)
                                    {
                                        if (MassUtility.GetMassPPM(GTree.GlycanMonoMass, GS.GlycanMonoMass) < _precursorTorelance && GTree.HasNGlycanCore())
                                        {
                                            if (MaxScore == 0.0f)
                                            {
                                                MaxScore = GTree.Score;
                                                TopScoreCount++;
                                            }
                                            else if (MaxScore.ToString("#0.00000") == GTree.Score.ToString("#0.00000"))
                                            {
                                                TopScoreCount++;
                                            }
                                            else if (MaxScore < GTree.Score)
                                            {
                                                MaxScore = GTree.Score;
                                                TopScoreCount = 1;
                                            }
                                            if (GTree.HasNGlycanCore())
                                            {
                                                NumOfFullStructureWithNCore++;
                                            }
                                            //Console.WriteLine(GTree.Score);
                                        }
                                        if (MassUtility.GetMassPPM(GTree.GlycanMonoMass, GS.GlycanMonoMass) < SmallestPPM)
                                        {
                                            SmallestPPM = MassUtility.GetMassPPM(GTree.GlycanMonoMass, GS.GlycanMonoMass);
                                            MaxMatch = GTree;
                                        }
                                    }

                                    //Print out
                                    if (GS.SequencedStructures.Count != 0)
                                    {                                     
                                       // string exportStr = "";
                                        if (GS.FullSequencedStructures.Count != 0) //Print Complete
                                        {
                                            List<GlycanStructure> SequencedStructure = GS.GetTopRankScoreStructre(Convert.ToInt32(cboTopRank.Text));
                                            foreach (GlycanStructure g in SequencedStructure)
                                            {
                                                if (g.NoOfTotalGlycan == GS.FullSequencedStructures[0].NoOfTotalGlycan)
                                                {
                                                    string ComStr = g.NoOfHexNac.ToString() + "-" + g.NoOfHex.ToString() + "-" + g.NoOfDeHex.ToString() + "-" + g.NoOfNeuAc.ToString();
                                                    ////exportStr = "CID_Scan,Parent_Scan,Parent_Z,GlycoPeptideMz,Y1_Mz,Peptide_Seq,GlycanStructure,Full_Seqquence,Score";
                                                    //exportStr = _scan.ScanNo.ToString() + "," +
                                                    //                    _scan.ParentScanNo.ToString() + "," +
                                                    //                    _scan.ParentCharge.ToString() + "," +
                                                    //                    (g.Charge - 1).ToString() + "," +
                                                    //                    g.Y1.Mass.ToString() + "," +
                                                    //                   Peptide + "," +
                                                    //                   g.IUPACString + "," +
                                                    //                   g.NoOfTotalGlycan.ToString() + "," +
                                                    //                   ComStr + "," +
                                                    //                   "Y" + "," +
                                                    //                   g.Score.ToString("0.000");
                                                    //sw.WriteLine(exportStr);


                                                    OutputRowCount++;
                                                    ws.Cells[OutputRowCount, 1].Value = _scan.ScanNo;
                                                    ws.Cells[OutputRowCount, 2].Value = _scan.ParentScanNo;
                                                    ws.Cells[OutputRowCount, 3].Value = _scan.ParentCharge;
                                                    ws.Cells[OutputRowCount, 4].Value = (g.Charge - 1);
                                                    ws.Cells[OutputRowCount, 5].Value = g.Y1.Mass;
                                                    ws.Cells[OutputRowCount, 6].Value = Peptide;
                                                    ws.Cells[OutputRowCount, 7].Value = g.IUPACString;
                                                    ws.Cells[OutputRowCount, 8].Value = g.NoOfTotalGlycan;
                                                    ws.Cells[OutputRowCount, 9].Value = ComStr;
                                                    ws.Cells[OutputRowCount, 10].Value = "Y";
                                                    ws.Cells[OutputRowCount, 11].Value = Convert.ToDouble(g.Score.ToString("0.000"));
                                                    //Create picture                                                    
                                                    COL.GlycoLib.GlycansDrawer glycanimg = new GlycansDrawer(g.IUPACString);
                                                    OfficeOpenXml.Drawing.ExcelPicture glycanImg = ws.Drawings.AddPicture("Glycan_" + OutputRowCount + "-12", glycanimg.GetImage());                                                    
                                                    glycanImg.SetPosition(OutputRowCount-1, 0, 12-1, 0);
    
                                                }
                                            }
                                            continue;
                                        }
                                        else //Print Partial Top Tank
                                        {
                                            List<GlycanStructure> SequencedStructure = GS.GetTopRankNoNCompletedSequencedStructre(Convert.ToInt32(cboTopRank.Text));
                                            foreach (GlycanStructure g in SequencedStructure)
                                            {
                                                string ComStr = g.NoOfHexNac.ToString() + "-" + g.NoOfHex.ToString() + "-" + g.NoOfDeHex.ToString() + "-" + g.NoOfNeuAc.ToString();
                                                ////exportStr = "CID_Scan,Parent_Scan,Parent_Z,GlycoPeptideMz,Y1_Mz,Peptide_Seq,GlycanStructure,Full_Seqquence,Score";
                                                //exportStr = _scan.ScanNo.ToString() + "," +
                                                //                    _scan.ParentScanNo.ToString() + "," +
                                                //                    _scan.ParentCharge.ToString() + "," +
                                                //                    (g.Charge-1).ToString() + "," +
                                                //                    g.Y1.Mass.ToString() + "," +
                                                //                   Peptide + "," +
                                                //                   g.IUPACString + "," +
                                                //                   g.NoOfTotalGlycan.ToString() + "," +
                                                //                     ComStr + "," +
                                                //                   "N" + "," +
                                                //                   g.Score.ToString("0.000");
                                                //sw.WriteLine(exportStr);

                                                OutputRowCount++;
                                                ws.Cells[OutputRowCount, 1].Value = _scan.ScanNo;
                                                ws.Cells[OutputRowCount, 2].Value = _scan.ParentScanNo;
                                                ws.Cells[OutputRowCount, 3].Value = _scan.ParentCharge;
                                                ws.Cells[OutputRowCount, 4].Value = (g.Charge - 1);
                                                ws.Cells[OutputRowCount, 5].Value = g.Y1.Mass;
                                                ws.Cells[OutputRowCount, 6].Value = Peptide;
                                                ws.Cells[OutputRowCount, 7].Value = g.IUPACString;
                                                ws.Cells[OutputRowCount, 8].Value = g.NoOfTotalGlycan;
                                                ws.Cells[OutputRowCount, 9].Value = ComStr;
                                                ws.Cells[OutputRowCount, 10].Value = "N";
                                                ws.Cells[OutputRowCount, 11].Value = Convert.ToDouble(g.Score.ToString("0.000"));
                                                //Create picture                                                    
                                                COL.GlycoLib.GlycansDrawer glycanimg = new GlycansDrawer(g.IUPACString);
                                                OfficeOpenXml.Drawing.ExcelPicture glycanImg = ws.Drawings.AddPicture("Glycan_" + OutputRowCount + "-12", glycanimg.GetImage());
                                                glycanImg.SetPosition(OutputRowCount - 1, 0, 12 - 1, 0);
                                            }
                                        }
                                    }
                                    
                                } //If sequenced structure found

                            } //charge
                        } //peptide
                    } //protein
                } //scan
                //sw.Close();
                pck.Workbook.Worksheets[1].Column(5).Width = 12;
                pck.Workbook.Worksheets[1].Column(6).Width = 30;
                pck.Workbook.Worksheets[1].Column(7).Width = 50;
                pck.Workbook.Worksheets[1].Column(8).Width = 15;
                pck.Workbook.Worksheets[1].Column(9).Width = 25;
                pck.Workbook.Worksheets[1].Column(10).Width = 13;
                pck.Save();
                MessageBox.Show("Done!!");
            }*/
                #endregion

            }
        }
        private void GenerateGlycanList()
        {
            _glycanCompounds = ReadGlycanListFromFile.ReadGlycanList(txtGlycanList.Text, chkPerm.Checked, chkHuman.Checked, chkReducedReducingEnd.Checked);
            _MassGlycanMapping = new Dictionary<double, GlycanCompound>();
            GlycanCompoundMassList = new List<float>();
            foreach (GlycanCompound G in _glycanCompounds)
            {
                if (!_MassGlycanMapping.ContainsKey(G.AVGMass))
                {
                    _MassGlycanMapping.Add(G.AVGMass, G);
                    GlycanCompoundMassList.Add((float)G.AVGMass);
                }

            }
        }
        private string GetPreviousCompositionAndMass(GlycanStructure argStr, int argNodeID)
        {
            int Hex = 0;
            int HexNac = 0;
            int NeuAc = 0;
            int NeuGc = 0;
            int deHex = 0;
            foreach (GlycanTreeNode TNode in argStr.Root.FetchAllGlycanNode())
            {
                if (TNode.NodeID <= argNodeID)
                {
                    if (TNode.GlycanType == Glycan.Type.Hex)
                    {
                        Hex++;
                    }
                    else if (TNode.GlycanType == Glycan.Type.DeHex)
                    {
                        deHex++;
                    }
                    else if (TNode.GlycanType == Glycan.Type.HexNAc)
                    {
                        HexNac++;
                    }
                    else if (TNode.GlycanType == Glycan.Type.NeuAc)
                    {
                        NeuAc++;
                    }
                    else
                    {
                        NeuGc++;
                    }
                }
            }
            float Mass = 0.0f;
            Mass = GlycanMass.GetGlycanMasswithCharge(Glycan.Type.DeHex, 2) * deHex +
                         GlycanMass.GetGlycanMasswithCharge(Glycan.Type.Hex, 2) * Hex +
                          GlycanMass.GetGlycanMasswithCharge(Glycan.Type.HexNAc, 2) * HexNac +
                           GlycanMass.GetGlycanMasswithCharge(Glycan.Type.NeuAc, 2) * NeuAc +
                          GlycanMass.GetGlycanMasswithCharge(Glycan.Type.NeuGc, 2) * NeuGc;

            string OutStr = "Hex * " + Hex.ToString() +
                                         " HexNac * " + HexNac.ToString() +
                                        " deHex * " + deHex.ToString() +
                                        " NeuAc * " + NeuAc.ToString() +
                                        " NexGc * " + NeuGc.ToString() +
                                        "," + Mass.ToString();
            return OutStr;

        }
        private List<ProteinInfo> ProteinParser(string argFastaFile)
        {
            StreamReader SR = new StreamReader(argFastaFile);
            List<ProteinInfo> ProteinInfo = new List<ProteinInfo>();
            string title = "";
            string sequence = "";
            do
            {
                string tmp = SR.ReadLine().Trim();
                if (tmp.StartsWith(">"))
                {
                    if (title != "")
                    {
                        ProteinInfo.Add(new ProteinInfo(title, sequence));
                    }
                    title = tmp.Substring(1); //
                    sequence = "";
                }
                else
                {
                    sequence = sequence + tmp;
                }
            } while (!SR.EndOfStream);
            ProteinInfo.Add(new ProteinInfo(title, sequence));
            SR.Close();
            return ProteinInfo;
        }

        private void frmBatch_Load(object sender, EventArgs e)
        {
            cboTopRank.SelectedIndex = 2;
        }
        private void rdoNeuAc_CheckedChanged(object sender, EventArgs e)
        {
            rdoNeuGc.Checked = rdoNeuAc.Checked;
        }

        private void rdoScanRange_CheckedChanged(object sender, EventArgs e)
        {
            txtStart.Enabled = rdoScanRange.Checked;
            txtEnd.Enabled = rdoScanRange.Checked;
            txtScanFileList.Enabled = !rdoScanRange.Checked;
            btnScanList.Enabled = !rdoScanRange.Checked;
        }

        private void btnGlycanList_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Glycan list file (*.csv)|*.csv";
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtGlycanList.Text = openFileDialog1.FileName;
            }
        }

        private void chkGlycanList_CheckedChanged(object sender, EventArgs e)
        {
            groupBox5.Enabled = chkGlycanList.Checked;
            grpGlycan.Enabled = !chkGlycanList.Checked;
        }

        private Bitmap GetImageFromURL(string argGlycan)
        {
            string address = "http://burrow.soic.indiana.edu:8099/GlycanDrawWebApp/GlycanImageHandler.ashx?IUPAC=" + argGlycan;
            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(address);
            myRequest.Method = "GET";
            HttpWebResponse myResponse = (HttpWebResponse)myRequest.GetResponse();
            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(myResponse.GetResponseStream());
            myResponse.Close();
            return bmp;
        }

        private void chkHCD_CheckedChanged(object sender, EventArgs e)
        {
            chkSeqHCD.Enabled = chkHCD.Checked;            
        }

        private void chkEnzy_None_CheckedChanged(object sender, EventArgs e)
        {
            if (chkEnzy_None.Checked)
            {
                chkEnzy_GlucE.Checked = false;
                chkEnzy_GlucED.Checked = false;
                chkEnzy_Trypsin.Checked = false;
            }
        }

        private void chkEnzy_GlucE_CheckedChanged(object sender, EventArgs e)
        {
            if (chkEnzy_GlucE.Checked)
            {
                chkEnzy_None.Checked = false;
                chkEnzy_GlucED.Checked = false;
            }
        }

        private void chkEnzy_GlucED_CheckedChanged(object sender, EventArgs e)
        {
            if (chkEnzy_GlucED.Checked)
            {
                chkEnzy_None.Checked = false;
                chkEnzy_GlucE.Checked = false;
            }
        }

        private void chkEnzy_Trypsin_CheckedChanged(object sender, EventArgs e)
        {
            if (chkEnzy_Trypsin.Checked)
            {
                chkEnzy_None.Checked = false;
            }
        }

     


    }
}