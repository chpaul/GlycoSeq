using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using COL.GlycoSequence;
using COL.ProtLib;
using COL.GlycoLib;
using COL.MassLib;
namespace GlycanSeq_Form
{
    public partial class frmProcessing : Form
    {
        DateTime StartTime;
        DateTime EndTime;
        private List<GlycanSequencing> _lstGS;
        AminoAcidMass AAMW;

        private int CurrentScan = 0;
        private string CurrentPeptide = "";
        DateTime Start;
        bool DoLog = false;
        string _rawFilePath;
        RawReader Raw = null;

        List<ProteinInfo> Proteins;

        int _StartScan = 0;
        int _EndScan = 0;
        float _MSMSTol = 0.0f;
        float _PrecursorTol = 0.0f;
        bool _NGlycan = true;
        bool _Human;
        List<GlycanCompound> _GlycanCompounds;
        Dictionary<double, GlycanCompound> _MassGlycanMapping;
        List<float> _GlycanCompoundMassList;
        int _NoHexNAc;
        int _NoHex;
        int _NoDeHex;
        int _NoSia;
        List<Protease.Type> _ProteaseType;
        int _MissCLeavage;
        bool _AverageMass;
        bool _UseGlycanList;
        string _exportFilename;
        int _GetTopRank;
        bool _UseHCD;
        bool _SeqHCD;
        bool _CompletedOnly;
        float _CompletedReward;
        /// <summary>
        /// Input GlycanCompostion (From Glycan list)
        /// </summary>
        /// <param name="argStartScan"></param>
        /// <param name="argEndScan"></param>
        /// <param name="argMSMSTol"></param>
        /// <param name="argPrecursorTol"></param>
        /// <param name="argNGlycan"></param>
        /// <param name="argHuman"></param>
        /// <param name="argGlycanCompounds"></param>
        /// <param name="argMassGlycanMapping"></param>
        /// <param name="argGlycanCompoundMassList"></param>
        /// <param name="argRawFilePath"></param>
        /// <param name="argFastaFile"></param>
        /// <param name="argProteaseType"></param>
        /// <param name="argMissCleavage"></param>
        /// <param name="argAverageMass"></param>
        /// <param name="argUseHCD"></param>
        /// <param name="argExportFilename"></param>
        /// <param name="argGetTopRank"></param>
        /// <param name="argCompletedOnly"></param>
        /// <param name="argCompletedReward"></param>
        public frmProcessing(int argStartScan,
                                              int argEndScan,
                                              float argMSMSTol,
                                              float argPrecursorTol,
                                              bool argNGlycan,
                                              bool argHuman,
                                              List<GlycanCompound> argGlycanCompounds,
                                              Dictionary<double, GlycanCompound> argMassGlycanMapping,
                                              List<float> argGlycanCompoundMassList,
                                              string argRawFilePath,
                                              string argFastaFile,
                                              List<Protease.Type> argProteaseType,
                                              int argMissCleavage,
                                              bool argAverageMass,
                                              bool argUseHCD,
                                              bool argSeqHCD,
                                              string argExportFilename,
                                              int argGetTopRank,
                                              bool argCompletedOnly,
                                              float argCompletedReward
                                            )
        {
            InitializeComponent();
            AAMW = new AminoAcidMass();
            _StartScan = argStartScan;
            _EndScan = argEndScan;
            _MSMSTol = argMSMSTol;
            _PrecursorTol = argPrecursorTol;
            _NGlycan = argNGlycan;
            _Human = argHuman;
            _GlycanCompounds = argGlycanCompounds;
            _MassGlycanMapping = argMassGlycanMapping;
            _GlycanCompoundMassList = argGlycanCompoundMassList;
            _rawFilePath = argRawFilePath;
            Proteins = FastaReader.ReadFasta(argFastaFile);
            _ProteaseType = argProteaseType;
            _MissCLeavage = argMissCleavage;
            _AverageMass = argAverageMass;
            _UseGlycanList = true;
            _exportFilename = argExportFilename;
            _GetTopRank = argGetTopRank;
            _lstGS = new List<GlycanSequencing>();
            _UseHCD = argUseHCD;
            _SeqHCD = argSeqHCD;
            _CompletedOnly = argCompletedOnly;
            _CompletedReward = argCompletedReward;

            if(Path.GetExtension(_rawFilePath).ToLower() ==".raw")
            {
                Raw = new RawReader(_rawFilePath, enumRawDataType.raw);
            }
            else
            {
                Raw = new RawReader(_rawFilePath, enumRawDataType.mzxml);
            }
            
            StartTime = new DateTime(DateTime.Now.Ticks);
            bgWorker_Process.RunWorkerAsync();

        }
        /// <summary>
        /// Input Number of Glycans (blind search)
        /// </summary>
        /// <param name="argStartScan"></param>
        /// <param name="argEndScan"></param>
        /// <param name="argMSMSTol"></param>
        /// <param name="argPrecursorTol"></param>
        /// <param name="argNGlycan"></param>
        /// <param name="argHuman"></param>
        /// <param name="argNoHexNAc"></param>
        /// <param name="argNoHex"></param>
        /// <param name="argNoDeHex"></param>
        /// <param name="argNoSia"></param>
        /// <param name="argRawFilePath"></param>
        /// <param name="argFastaFile"></param>
        /// <param name="argProteaseType"></param>
        /// <param name="argMissCleavage"></param>
        /// <param name="argAverageMass"></param>
        /// <param name="argUseHCD"></param>
        /// <param name="argExportFilename"></param>
        /// <param name="argGetTopRank"></param>
        /// <param name="argCompletedOnly"></param>
        /// <param name="argCompletedReward"></param>
        public frmProcessing(int argStartScan,
                                      int argEndScan,
                                      float argMSMSTol,
                                      float argPrecursorTol,
                                      bool argNGlycan,
                                      bool argHuman,
                                      int argNoHexNAc,
                                      int argNoHex,
                                      int argNoDeHex,
                                      int argNoSia,
                                      string argRawFilePath,
                                      string argFastaFile,
                                      List<Protease.Type> argProteaseType,
                                      int argMissCleavage,
                                      bool argAverageMass,
                                    bool argUseHCD,
                                     bool argSeqHCD,
                                     string argExportFilename,
                                     int argGetTopRank,
                                     bool argCompletedOnly,
                                    float argCompletedReward)
        {
            InitializeComponent();
            AAMW = new AminoAcidMass();
            _StartScan = argStartScan;
            _EndScan = argEndScan;
            _MSMSTol = argMSMSTol;
            _PrecursorTol = argPrecursorTol;
            _NGlycan = argNGlycan;
            _Human = argHuman;
            _NoHexNAc = argNoHexNAc;
            _NoHex = argNoHex;
            _NoDeHex = argNoDeHex;
            _NoSia = argNoSia;
            _rawFilePath = argRawFilePath;
            Proteins = FastaReader.ReadFasta(argFastaFile);
            _ProteaseType = argProteaseType;
            _MissCLeavage = argMissCleavage;
            _AverageMass = argAverageMass;
            _UseGlycanList = false;
            _UseHCD = argUseHCD;
            _SeqHCD = argSeqHCD;
            _CompletedOnly = argCompletedOnly;
            _CompletedReward = argCompletedReward;
            _exportFilename = argExportFilename;
            _GetTopRank = argGetTopRank;
            _lstGS = new List<GlycanSequencing>();
             if(Path.GetExtension(_rawFilePath).ToLower() ==".raw")
            {
                Raw = new RawReader(_rawFilePath, enumRawDataType.raw);
            }
            else
            {
                Raw = new RawReader(_rawFilePath, enumRawDataType.mzxml);
            }
            
              
            StartTime = new DateTime(DateTime.Now.Ticks);
            bgWorker_Process.RunWorkerAsync();
        }
        private void bgWorker_Process_DoWork(object sender, DoWorkEventArgs e)
        {            
            for (int ScanNo = _StartScan; ScanNo <= _EndScan; ScanNo++)
            {
                if (Raw.GetMsLevel(ScanNo) == 1)
                {
                    CurrentScan = ScanNo;
                    int ProcessReport = Convert.ToInt32(((ScanNo - _StartScan + 1) / (float)(_EndScan - _StartScan + 1) * 100));
                    //Console.WriteLine("Scan:" + ScanNo.ToString()+"\t Peptide:" + Peptide + "  completed");
                    bgWorker_Process.ReportProgress(ProcessReport);                    
                    continue;
                }
                if (!_SeqHCD&& !Raw.IsCIDScan(ScanNo))
                {
                    CurrentScan = ScanNo;
                    int ProcessReport = Convert.ToInt32(((ScanNo - _StartScan + 1) / (float)(_EndScan - _StartScan + 1) * 100));
                    //Console.WriteLine("Scan:" + ScanNo.ToString()+"\t Peptide:" + Peptide + "  completed");
                    bgWorker_Process.ReportProgress(ProcessReport);
                    continue;
                }
                MSScan _scan = Raw.ReadScan(ScanNo);               

                HCDInfo HCD = null ;
                int PrecursorCharge = _scan.ParentCharge;
                int HCDScanNo = 0;
                if (_UseHCD)
                {
                    int CheckScanNO = ScanNo;
                    //string ScanHeader = _scan.ScanHeader.Substring(_scan.ScanHeader.IndexOf("ms2")+4, _scan.ScanHeader.IndexOf("@") - _scan.ScanHeader.IndexOf("ms2")-3) + "hcd";
                    do
                    {
                          CheckScanNO++;
                    //    if (Raw.GlypIDReader.GetScanDescription(CheckScanNO).Contains(ScanHeader))
                    //    {
                    //        HCDScanNo = CheckScanNO;
                    //        HCD = new HCDInfo(Raw.GlypIDReader, HCDScanNo);
                    //        break;
                    //    }
                          if (Raw.GetHCDInfo(CheckScanNO) != null)
                          {
                              HCD = Raw.GetHCDInfo(CheckScanNO);
                              break;
                          }
                        
                    } while (Raw.GetMsLevel(CheckScanNO)!=1); //Check Until hit Next Full MS

                    //CA: Complex Asialyated, CS:Complex Sialylated, HM:High mannose, HY:Hybrid and NA

                }
                if (HCD != null)
                {
                    Console.WriteLine("CID Scan No:" + ScanNo.ToString() + "\tHCD Scan No:" + HCDScanNo.ToString() + "\tGlycanType:" + HCD.GlycanType.ToString());
                }
                foreach (ProteinInfo PInfo in Proteins)
                {
                    List<string> DigestedPeptides = new List<string>();
                    if (_NGlycan)
                    {
                        DigestedPeptides.AddRange(PInfo.NGlycopeptide(_MissCLeavage, _ProteaseType));
                    }
                    else
                    {
                        DigestedPeptides.AddRange(PInfo.OGlycopeptide(_MissCLeavage, _ProteaseType));
                    }

                    foreach (string Peptide in DigestedPeptides)
                    {

                        float PeptideMass = AAMW.GetMonoMW(Peptide, true);
                        for (int j = PrecursorCharge - 1; j <= PrecursorCharge; j++)
                        {
                            int Y1ChargeSt = j;
                            if (j == 0)
                            {
                                continue;
                            }
                            float PredictedY1 = 0.0f;
                            PredictedY1 = (float)(PeptideMass + GlycanMass.GetGlycanAVGMass(Glycan.Type.HexNAc) + COL.MassLib.Atoms.ProtonMass * Y1ChargeSt) / Y1ChargeSt;
                            GlycanSequencing GS = null;
                            if (_UseGlycanList)
                            {
                                float GlycanMonoMass = (_scan.ParentMZ - Atoms.ProtonMass) * _scan.ParentCharge - AAMW.GetAVGMonoMW(Peptide, true);
                                float PrecursorMono = _scan.ParentMonoMW;
                                //if (_scan.ParentAVGMonoMW != 0.0)
                                //{
                                //    GlycanMonoMass = _scan.ParentAVGMonoMW - PeptideMass + (Atoms.HydrogenAVGMass * 2 + Atoms.OxygenAVGMass);
                                //    PrecursorMono = _scan.ParentAVGMonoMW;
                                //}
                                //else
                                //{
                                //    GlycanMonoMass = (_scan.ParentMZ - Atoms.ProtonMass) * _scan.ParentCharge - PeptideMass + (Atoms.HydrogenAVGMass * 2 + Atoms.OxygenAVGMass);
                                //    PrecursorMono = _scan.ParentMonoMW;
                                //}
                                
                                List<GlycanCompound> ClosedGlycans = new List<GlycanCompound>();
                                foreach (float gMass in _GlycanCompoundMassList)
                                {
                                    if (Math.Abs(gMass - GlycanMonoMass) < 100.0f)
                                    {
                                        ClosedGlycans.Add(_GlycanCompounds[MassUtility.GetClosestMassIdx(_GlycanCompoundMassList, gMass)]);
                                    }
                                }
                                
                                //if (HCD != null)
                                //{
                                //    if ((HCD.GlycanType == GlypID.enmGlycanType.CA && ClosedGlycan.NoOfSia>0) ||
                                //         (HCD.GlycanType == GlypID.enmGlycanType.HM && (ClosedGlycan.NoOfSia!=0||ClosedGlycan.NoOfHexNAc!=2 || ClosedGlycan.NoOfDeHex!=0) ) ||
                                //         (HCD.GlycanType == GlypID.enmGlycanType.CS && ClosedGlycan.NoOfSia==0))
                                //    {
                                //        continue;
                                //    }              
                                //}
                                //if (Math.Abs(ClosedGlycan.AVGMass - GlycanMonoMass) <= _MSMSTol)
                                foreach (GlycanCompound ClosedGlycan in ClosedGlycans)
                                {
                                    if (_Human) //NeuAc
                                    {
                                        int NoOfSia = ClosedGlycan.NoOfSia;
                                        int NoOfDeHex = ClosedGlycan.NoOfDeHex;
                                        if (HCD!=null && HCD.GlycanType ==  COL.MassLib.enumGlycanType.CA && ClosedGlycan.NoOfSia > 0)
                                        {           
                                                NoOfDeHex = NoOfDeHex + NoOfSia * 2;
                                                NoOfSia = 0;                                                                                        
                                        }
                                        GS = new GlycanSequencing(_scan, Peptide, true, Y1ChargeSt, ClosedGlycan.NoOfHex, ClosedGlycan.NoOfHexNAc, NoOfDeHex, NoOfSia, 0, @"d:\tmp", _NGlycan, _MSMSTol, _PrecursorTol);
                                    }
                                    else //NeuGc
                                    {             
                                        GS = new GlycanSequencing(_scan, Peptide, true, Y1ChargeSt, ClosedGlycan.NoOfHex, ClosedGlycan.NoOfHexNAc, ClosedGlycan.NoOfDeHex, 0, ClosedGlycan.NoOfSia, @"d:\tmp", _NGlycan, _MSMSTol, _PrecursorTol);
                                    }
                                    GS.NumbersOfPeaksForSequencing = 140;
                                    GS.UseAVGMass = _AverageMass;
                                    GS.CreatePrecursotMZ = true;
                                    if (!_CompletedOnly)
                                    {
                                        GS.RewardForCompleteStructure = 0.0f;
                                    }
                                    if (HCD != null)
                                    {
                                        GS.GlycanType = HCD.GlycanType;
                                    }
                                    GS.StartSequencing();
                                    _lstGS.Add(GS);
                                    CurrentScan = ScanNo;
                                    CurrentPeptide = GS.PeptideSeq;
                                    int ProcessReport = Convert.ToInt32(((ScanNo - _StartScan + 1) / (float)(_EndScan - _StartScan + 1) * 100));
                                    //Console.WriteLine("Scan:" + ScanNo.ToString()+"\t Peptide:" + Peptide + "  completed");
                                    bgWorker_Process.ReportProgress(ProcessReport);
                                }
                                
                            }
                            else // no list
                            {
                                if (_Human) //NeuAc
                                {
                                    if (HCD != null)
                                    {
                                        //CA: Complex Asialyated, CS:Complex Sialylated, HM:High mannose, HY:Hybrid and NA
                                        if (HCD.GlycanType == enumGlycanType.CA)
                                        {
                                            GS = new GlycanSequencing(_scan, Peptide, true, Y1ChargeSt, _NoHex, _NoHexNAc, _NoDeHex, 0, 0, @"d:\tmp", _NGlycan, _MSMSTol, _PrecursorTol);
                                        }
                                        else if (HCD.GlycanType == enumGlycanType.HM)
                                        {
                                            GS = new GlycanSequencing(_scan, Peptide, true, Y1ChargeSt, _NoHex, 2, 0, 0, 0, @"d:\tmp", _NGlycan, _MSMSTol, _PrecursorTol);
                                        }
                                        else
                                        {
                                            GS = new GlycanSequencing(_scan, Peptide, true, Y1ChargeSt, _NoHex, _NoHexNAc, _NoDeHex, _NoSia, 0, @"d:\tmp", _NGlycan, _MSMSTol, _PrecursorTol);
                                        }
                                    }
                                    else
                                    {
                                        GS = new GlycanSequencing(_scan, Peptide, true, Y1ChargeSt, _NoHex, _NoHexNAc, _NoDeHex, _NoSia, 0, @"d:\tmp", _NGlycan, _MSMSTol, _PrecursorTol);
                                    }
                                }
                                else //NeuGc
                                {
                                    if (HCD != null)
                                    {
                                        //CA: Complex Asialyated, CS:Complex Sialylated, HM:High mannose, HY:Hybrid and NA
                                        if (HCD.GlycanType == enumGlycanType.CA)
                                        {
                                            GS = new GlycanSequencing(_scan, Peptide, true, Y1ChargeSt, _NoHex, _NoHexNAc, _NoDeHex, 0, 0, @"d:\tmp", _NGlycan, _MSMSTol, _PrecursorTol);
                                        }
                                        else if (HCD.GlycanType == enumGlycanType.HM)
                                        {
                                            GS = new GlycanSequencing(_scan, Peptide, true, Y1ChargeSt, _NoHex, 2, 0, 0, 0, @"d:\tmp", _NGlycan, _MSMSTol, _PrecursorTol);
                                        }
                                        else
                                        {
                                            GS = new GlycanSequencing(_scan, Peptide, true, Y1ChargeSt, _NoHex, _NoHexNAc, _NoDeHex, 0, _NoSia, @"d:\tmp", _NGlycan, _MSMSTol, _PrecursorTol);
                                        }
                                    }
                                    else
                                    {
                                        GS = new GlycanSequencing(_scan, Peptide, true, Y1ChargeSt, _NoHex, _NoHexNAc, _NoDeHex, 0, _NoSia, @"d:\tmp", _NGlycan, _MSMSTol, _PrecursorTol);
                                    }
                                }
                                GS.NumbersOfPeaksForSequencing = 140;
                                GS.UseAVGMass = _AverageMass;
                                GS.CreatePrecursotMZ = true;
                                if (!_CompletedOnly)
                                {
                                    GS.RewardForCompleteStructure = 0.0f;
                                }
                                if (HCD != null)
                                {
                                    GS.GlycanType = HCD.GlycanType;
                                }
                                GS.StartSequencing();
                                _lstGS.Add(GS);
                                CurrentScan = ScanNo;
                                CurrentPeptide = GS.PeptideSeq;
                                int ProcessReport = Convert.ToInt32(((ScanNo - _StartScan + 1) / (float)(_EndScan - _StartScan + 1) * 100));
                                //Console.WriteLine("Scan:" + ScanNo.ToString()+"\t Peptide:" + Peptide + "  completed");
                                bgWorker_Process.ReportProgress(ProcessReport);
                            }
                           
                        }//Foreach charge
                    }//Foreach peptide
                }//Foreach protein
            }//Foreach Scan
        }

        private void bgWorker_Process_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            try
            {
                lblCurrentScan.Text = (CurrentScan + 1).ToString() + "    Peptide:" + CurrentPeptide;

                progressBar1.Value = e.ProgressPercentage;
                lblPercentage.Text = e.ProgressPercentage.ToString() + "%";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void bgWorker_Process_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            lblCurrentScan.Text ="Outputing";
            progressBar1.Value = 99;
            lblPercentage.Text = "99%";
            FileInfo NewFile = new FileInfo(_exportFilename);
            
            int OutputRowCount = 0;
            if (NewFile.Exists)
            {
                File.Delete(NewFile.FullName);
            }
            OfficeOpenXml.ExcelPackage pck = new OfficeOpenXml.ExcelPackage(NewFile);

            if (pck.Workbook.Worksheets[Path.GetFileNameWithoutExtension(Raw.RawFilePath)] != null)
            {
                pck.Workbook.Worksheets.Delete(Path.GetFileNameWithoutExtension(Raw.RawFilePath));
            }

            var ws = pck.Workbook.Worksheets.Add(Path.GetFileNameWithoutExtension(Raw.RawFilePath));
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
            ws.Cells[OutputRowCount, 12].Value = "PPM";
            ws.Cells[OutputRowCount, 13].Value = "Picture";
            int count = 0;
            foreach (GlycanSequencing GS in _lstGS)
            {
                if (GS.SequencedStructures.Count == 0)
                {
                    continue;
                }
                // string exportStr = "";
                if (GS.FullSequencedStructures.Count != 0) //Print Complete
                {
                    //GS.FullSequencedStructures.Sort(
                    //        delegate(GlycanStructure GS1, GlycanStructure GS2)
                    //        {
                    //            if (GS1.NoOfTotalGlycan.CompareTo(GS2.NoOfTotalGlycan) == 0) //Equal compare score 
                    //            {
                    //                return -1 * GS1.Score.CompareTo(GS2.Score);
                    //            }
                    //            else
                    //            {
                    //                return -1 * GS1.NoOfTotalGlycan.CompareTo(GS2.NoOfTotalGlycan);
                    //            }
                    //        }
                    //   );
                    List<KeyValuePair<GlycanStructure, double>> SortedGlycanStructuresByMassDifferences = new List<KeyValuePair<GlycanStructure, double>>();
                    foreach (GlycanStructure g in GS.FullSequencedStructures)
                    {
                        SortedGlycanStructuresByMassDifferences.Add(new KeyValuePair<GlycanStructure, double>(g, MassUtility.GetMassPPM(g.GlycanMonoMass + AAMW.GetMonoMW(GS.PeptideSeq,true), GS.PrecusorMonoMass)));
                    }
                        SortedGlycanStructuresByMassDifferences.Sort((firstPair, nextPair) =>
                        {
                            if (firstPair.Value.CompareTo(nextPair.Value) == 0)
                            {
                                return -1 * firstPair.Key.Score.CompareTo(nextPair.Key.Score);
                            }
                            else
                            {
                                return firstPair.Value.CompareTo(nextPair.Value);
                            }
                        }
                    );
                        
                        try
                        {
                            foreach (KeyValuePair<GlycanStructure, double> glyc in SortedGlycanStructuresByMassDifferences)
                            {
                                if (glyc.Value >= _PrecursorTol)
                                {
                                    continue;
                                }

                                GlycanStructure g = glyc.Key;
                                string ComStr = g.NoOfHexNac.ToString() + "-" + g.NoOfHex.ToString() + "-" + g.NoOfDeHex.ToString() + "-" + g.NoOfNeuAc.ToString();

                                OutputRowCount++;
                                ws.Cells[OutputRowCount, 1].Value = GS.ScanInfo.ScanNo;
                                ws.Cells[OutputRowCount, 2].Value = GS.ScanInfo.ParentScanNo;
                                ws.Cells[OutputRowCount, 3].Value = GS.ScanInfo.ParentMZ;
                                ws.Cells[OutputRowCount, 4].Value = (g.Charge - 1);
                                ws.Cells[OutputRowCount, 5].Value = g.Y1.Mass;
                                ws.Cells[OutputRowCount, 6].Value = GS.PeptideSeq;
                                ws.Cells[OutputRowCount, 7].Value = g.IUPACString;
                                ws.Cells[OutputRowCount, 8].Value = g.NoOfTotalGlycan;
                                ws.Cells[OutputRowCount, 9].Value = ComStr;
                                ws.Cells[OutputRowCount, 10].Value = "Y";
                                ws.Cells[OutputRowCount, 11].Value = Convert.ToDouble(g.Score.ToString("0.000"));
                                ws.Cells[OutputRowCount, 12].Value = Convert.ToDouble(glyc.Value.ToString("0.000"));
                                //Create picture                                                    
                                if (count <= 2000)
                                {
                                    COL.GlycoLib.GlycansDrawer glycanimg = new GlycansDrawer(g.IUPACString);
                                    OfficeOpenXml.Drawing.ExcelPicture glycanImg = ws.Drawings.AddPicture("Glycan_" + OutputRowCount + "-12", glycanimg.GetImage());
                                    glycanImg.SetPosition(OutputRowCount - 1, 0, 13 - 1, 0);
                                }
                                count++;
                            }
                        }
                    catch
                    {
                        Console.WriteLine(count);
                    }
                    
                }
                else //Print Partial Top Tank
                {
                    if (_CompletedOnly == true)
                    {
                        continue;
                    }
                    //   List<GlycanStructure> SequencedStructure = GS.GetTopRankNoNCompletedSequencedStructre(Convert.ToInt32(_GetTopRank));

                    //   SequencedStructure.Sort(
                    //     delegate(GlycanStructure GS1, GlycanStructure GS2)
                    //     {
                    //         if (GS1.NoOfTotalGlycan.CompareTo(GS2.NoOfTotalGlycan) == 0) //Equal compare score 
                    //         {
                    //             return -1 * GS1.Score.CompareTo(GS2.Score);
                    //         }
                    //         else
                    //         {
                    //             return -1 * GS1.NoOfTotalGlycan.CompareTo(GS2.NoOfTotalGlycan);
                    //         }
                    //     }
                    //);
                    //  foreach (GlycanStructure g in SequencedStructure)
                    List<KeyValuePair<GlycanStructure, double>> SortedGlycanStructuresByMassDifferences = new List<KeyValuePair<GlycanStructure, double>>();
                    foreach (GlycanStructure g in  GS.GetTopRankNoNCompletedSequencedStructre(Convert.ToInt32(_GetTopRank)))
                    {
                        SortedGlycanStructuresByMassDifferences.Add(new KeyValuePair<GlycanStructure, double>(g, MassUtility.GetMassPPM(g.GlycanMonoMass + AAMW.GetMonoMW(GS.PeptideSeq, true), GS.PrecusorMonoMass)));
                    }
                    SortedGlycanStructuresByMassDifferences.Sort((firstPair, nextPair) =>
                    {
                        if (firstPair.Value.CompareTo(nextPair.Value) == 0)
                        {
                            return -1 * firstPair.Key.Score.CompareTo(nextPair.Key.Score);
                        }
                        else
                        {
                            return firstPair.Value.CompareTo(nextPair.Value);
                        }
                    }
                    );

                    foreach (KeyValuePair<GlycanStructure, double> glyc in SortedGlycanStructuresByMassDifferences)
                    {
                        GlycanStructure g = glyc.Key;
                        
                        string ComStr = g.NoOfHexNac.ToString() + "-" + g.NoOfHex.ToString() + "-" + g.NoOfDeHex.ToString() + "-" + g.NoOfNeuAc.ToString();


                        OutputRowCount++;
                        ws.Cells[OutputRowCount, 1].Value = GS.ScanInfo.ScanNo;
                        ws.Cells[OutputRowCount, 2].Value = GS.ScanInfo.ParentScanNo;
                        ws.Cells[OutputRowCount, 3].Value = GS.ScanInfo.ParentMZ;
                        ws.Cells[OutputRowCount, 4].Value = (g.Charge - 1);
                        ws.Cells[OutputRowCount, 5].Value = g.Y1.Mass;
                        ws.Cells[OutputRowCount, 6].Value = GS.PeptideSeq;
                        ws.Cells[OutputRowCount, 7].Value = g.IUPACString;
                        ws.Cells[OutputRowCount, 8].Value = g.NoOfTotalGlycan;
                        ws.Cells[OutputRowCount, 9].Value = ComStr;
                        ws.Cells[OutputRowCount, 10].Value = "N";
                        ws.Cells[OutputRowCount, 11].Value = Convert.ToDouble(g.Score.ToString("0.000"));
                        ws.Cells[OutputRowCount, 12].Value = Convert.ToDouble(glyc.Value.ToString("0.000"));
                        //Create picture                                                    
                        COL.GlycoLib.GlycansDrawer glycanimg = new GlycansDrawer(g.IUPACString);
                        OfficeOpenXml.Drawing.ExcelPicture glycanImg = ws.Drawings.AddPicture("Glycan_" + OutputRowCount + "-12", glycanimg.GetImage());
                        glycanImg.SetPosition(OutputRowCount - 1, 0, 12 - 1, 0);                    
                    }
                }           
            }//For glycansequencing
            pck.Workbook.Worksheets[1].Column(5).Width = 12;
            pck.Workbook.Worksheets[1].Column(6).Width = 30;
            pck.Workbook.Worksheets[1].Column(7).Width = 50;
            pck.Workbook.Worksheets[1].Column(8).Width = 15;
            pck.Workbook.Worksheets[1].Column(9).Width = 25;
            pck.Workbook.Worksheets[1].Column(10).Width = 13;
            pck.Save();
            lblCurrentScan.Text = "Done";
            progressBar1.Value = 100;
            lblPercentage.Text = "100%";
            EndTime = new DateTime(DateTime.Now.Ticks);

            if (count > 2000)
            {
                if (MessageBox.Show("Output file exceed memory limitation, some pics can not be generated!!\n\n Elapsed Tme:" + EndTime.Subtract(StartTime).TotalSeconds.ToString() + "sec.") == System.Windows.Forms.DialogResult.OK)
                {
                    this.Close();
                }
            }
            else
            {
                if (MessageBox.Show("Done!!\n\n Elapsed Tme:" + EndTime.Subtract(StartTime).TotalSeconds.ToString() +"sec.") == System.Windows.Forms.DialogResult.OK)
                {
                    this.Close();
                }
            }

        }//function
    }
}//class
