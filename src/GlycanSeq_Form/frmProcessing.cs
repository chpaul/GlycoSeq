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
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Diagnostics;
using System.Text.RegularExpressions;
namespace GlycanSeq_Form
{
    public partial class frmProcessing : Form
    {
        private const  bool _exportLearingMatrix = false;
        private object lockObject = new object();
        private int completedScan = 0;
        private DateTime StartTime;
        private DateTime EndTime;
        private AminoAcidMass AAMW;

        private int CurrentScan = 0;
        private string CurrentPeptide = "";
        private bool DoLog = false;
        private string _rawFilePath;
        private ThermoRawReader Raw = null;

        private List<TargetPeptide> _TargetPeptide;
        //private List<ProteinInfo> Proteins;
        private StreamWriter swExport;
        private int _StartScan = 0;
        private int _EndScan = 0;
        private float _MSMSTol = 0.0f;
        private float _PrecursorTol = 0.0f;
        private bool _NGlycan = true;
        private bool _Human;
        private List<GlycanCompound> _GlycanCompounds;
        private Dictionary<double, GlycanCompound> _MassGlycanMapping;
        private List<float> _GlycanCompoundMassList;
        private int _NoHexNAc;
        private int _NoHex;
        private int _NoDeHex;
        private int _NoSia;
        //private List<Protease.Type> _ProteaseType;
        //private int _MissCLeavage;
        private bool _AverageMass;
        private bool _UseGlycanList;
        private string _exportFolder;
        private string _fastaFile;
        private string _glycanFile;
        private int _GetTopRank;
        private bool _UseHCD;
        private bool _SeqHCD;
        private bool _CompletedOnly;
        private float _CompletedReward;
        private int DividePartNum = 0;
        private enumPeptideMutation _PeptideMutation;
        private bool _ExportIndividualSpectrum = false;
        private Dictionary<string, List<Tuple<int, string, Tuple<double, double, double>, string, string, bool, double>>> dictResultSortByPeptide = new Dictionary<string, List<Tuple<int, string, Tuple<double, double, double>, string, string, bool, double>>>();
        //Tuple 1:Scan Number, 2:Glycan Sequence 3:Score 4:Completed
        private GlycanSeqParameters _sequencingParameters;
        private List<Tuple<float, string, TargetPeptide>> _peptideMassList;
        private List<GlycanCompound> lstGlycans;
        private List<int> _lstScans;
        /// <summary>
        /// Input Number of Glycans (blind search)
        /// </summary>
        /// <param name="argScans"></param>
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
        public frmProcessing(List<int>  argScans,
            float argMSMSTol,
            float argPrecursorTol,
            bool argNGlycan,
            bool argHuman,
            int argNoHexNAc,
            int argNoHex,
            int argNoDeHex,
            int argNoSia,
            string argRawFilePath,
            List<TargetPeptide> argTargetPeptides,
            bool argAverageMass,
            bool argUseHCD,
            bool argSeqHCD,
            string argExportFile,
            int argGetTopRank,
            bool argCompletedOnly,
            float argCompletedReward,
            bool argExportIndividualDetail
            )
        {
            InitializeComponent();
            AAMW = new AminoAcidMass();
            _lstScans = argScans;
            _MSMSTol = argMSMSTol;
            _PrecursorTol = argPrecursorTol;
            _NGlycan = argNGlycan;
            _Human = argHuman;
            _NoHexNAc = argNoHexNAc;
            _NoHex = argNoHex;
            _NoDeHex = argNoDeHex;
            _NoSia = argNoSia;
            _rawFilePath = argRawFilePath;
            //Proteins = FastaReader.ReadFasta(argFastaFile);
            //_fastaFile = argFastaFile;
            //_ProteaseType = argProteaseType;
            //_MissCLeavage = argMissCleavage;
            //_PeptideMutation = argPeptideMutation;
            _TargetPeptide = argTargetPeptides;
            _AverageMass = argAverageMass;
            _UseGlycanList = false;
            _UseHCD = argUseHCD;
            _SeqHCD = argSeqHCD;
            _CompletedOnly = argCompletedOnly;
            _CompletedReward = argCompletedReward;
            _exportFolder = argExportFile;
            _GetTopRank = argGetTopRank;
            _ExportIndividualSpectrum = argExportIndividualDetail;
            Raw = new ThermoRawReader(_rawFilePath);


            StartTime = new DateTime(DateTime.Now.Ticks);
            //WriteParametersToXML();
            //StartProcess();
            GeneratePeptideList();
            bgWorker_Process.RunWorkerAsync();
            //Thread WorkerThread = new Thread(PrepareSequencing);
            //WorkerThread.Start();
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
        public frmProcessing(List<int> argScans ,
            float argMSMSTol,
            float argPrecursorTol,
            bool argNGlycan,
            bool argHuman,
            string argGlycanFile,
            string argRawFilePath,
            List<TargetPeptide> argTargetPeptides,
            bool argAverageMass,
            bool argUseHCD,
            bool argSeqHCD,
            string argExportFile,
            int argGetTopRank,
            bool argCompletedOnly,
            float argCompletedReward,
            bool argExportIndividualDetail
            )
        {
            InitializeComponent();
            AAMW = new AminoAcidMass();
            _lstScans = argScans;
            _MSMSTol = argMSMSTol;
            _PrecursorTol = argPrecursorTol;
            _NGlycan = argNGlycan;
            _Human = argHuman;
            _glycanFile = argGlycanFile;
            _rawFilePath = argRawFilePath;

            _TargetPeptide = argTargetPeptides;
            _AverageMass = argAverageMass;
            _UseGlycanList = true;
            _UseHCD = argUseHCD;
            _SeqHCD = argSeqHCD;
            _CompletedOnly = argCompletedOnly;
            _CompletedReward = argCompletedReward;
            _exportFolder = argExportFile;
            _GetTopRank = argGetTopRank;
            _ExportIndividualSpectrum = argExportIndividualDetail;
            Raw = new ThermoRawReader(_rawFilePath);
            lstGlycans = ReadGlycanListFromFile.ReadGlycanList(_glycanFile, false, _Human, false);

            StartTime = new DateTime(DateTime.Now.Ticks);

            GeneratePeptideList();
            bgWorker_Process.RunWorkerAsync();

        }
        private void GeneratePeptideList()
        {
            _peptideMassList = new List<Tuple<float, string, TargetPeptide>>();
            foreach (TargetPeptide peptide in _TargetPeptide)
            {
                _peptideMassList.Add(new Tuple<float, string, TargetPeptide>(peptide.PeptideMass, peptide.PeptideSequence, peptide));
            }
        }
        public GlycanSeqParameters GlycanSequencingParemetets
        {
            get { return _sequencingParameters; }
            set { _sequencingParameters = value; }
        }
      

        private RunResults runResults;
        private int CurrentRunningPart = 0;
        private Process proc;

        private void UpdateStatus()
        {
            do
            {
                try
                {
                    Thread.Sleep(1000);

                    if (runResults.Output == null || !runResults.Output.StartsWith("Completed"))
                    {
                        continue;
                    }
                    this.lblStatus.SafeBeginInvoke(new Action(() => lblStatus.Text = "Status:" + runResults.Output));
                    string[] tmp = runResults.Output.Split(':')[1].Split(',');
                    int Progress =
                        Convert.ToInt32((Convert.ToSingle(tmp[1].Remove(tmp[1].Length - 1)) / DividePartNum) +
                                        ((CurrentRunningPart - 1) * (1 / (float)DividePartNum) * 100));
                    this.lblCurrentScan.SafeBeginInvoke(new Action(() => lblCurrentScan.Text = tmp[0]));
                    this.progressBar1.SafeBeginInvoke(new Action(() => progressBar1.Value = Progress));
                    this.lblPercentage.SafeBeginInvoke(new Action(() => lblPercentage.Text = Progress.ToString() + "%"));
                    this.lblRunningTime.SafeBeginInvoke(
                        new Action(
                            () => lblRunningTime.Text = DateTime.Now.Subtract(StartTime).TotalMinutes.ToString("0.00")));
                }
                catch
                {
                }
            } while (true);
        }

        private void RunExecutable(int argDividePart)
        {
            runResults = new RunResults { Output = "", Error = new StringBuilder(), RunException = null };

            proc = new Process();

            try
            {
                string TempEXE = @"D:\!Git\GlycanSequencing\src\GlycanSeq_Console\bin\Debug\GlycanSeq_Console.exe";
                TempEXE = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) +
                          "\\GlycanSeq_Console.exe";
                TempEXE = @"D:\!Git\GlycanSequencing\src\GlycanSeq_Console\bin\Debug\GlycanSeq_Console.exe";
                proc.StartInfo.FileName = TempEXE;
                proc.StartInfo.Arguments = Path.GetDirectoryName(_exportFolder) + "\\" +
                                           Path.GetFileNameWithoutExtension(_exportFolder) + "_" +
                                           (argDividePart).ToString() + ".xml";
                proc.StartInfo.UseShellExecute = false;
                //proc.StartInfo.CreateNoWindow = true;
                //proc.StartInfo.RedirectStandardOutput = true;
                //proc.StartInfo.RedirectStandardError = true;
                // proc.OutputDataReceived += (o, e) => runResults.Output = e.Data;
                // proc.ErrorDataReceived += (o, e) => runResults.Error.Append(e.Data).Append(Environment.NewLine);
                proc.Start();
                // proc.BeginOutputReadLine();
                // proc.BeginErrorReadLine();
                runResults.PID = proc.Id;
                proc.WaitForExit();
                runResults.ExitCode = proc.ExitCode;
            }
            catch (Exception e)
            {
                StreamWriter sw =
                    new StreamWriter(Path.GetDirectoryName(_exportFolder) + "\\" +
                                     Path.GetFileNameWithoutExtension(_exportFolder) + "_error.txt");
                sw.WriteLine(e.ToString());
                sw.WriteLine(runResults.Error.ToString());
                sw.Close();
                proc.Kill();
            }
        }

        private List<Task> WorkerTasks = new List<Task>();

        private void PrepareSequencing()
        {
            StreamWriter SW = new StreamWriter(_exportFolder);
            foreach (int ScanNo in _lstScans)
            {

                if (Raw.GetMsLevel(ScanNo) != 2)
                {
                    completedScan = completedScan + 1;
                    continue;
                }
                MSScan _scan = Raw.ReadScan(ScanNo);

                HCDInfo HCD = null;
                int PrecursorCharge = _scan.ParentCharge;
                if (_UseHCD)
                {
                    int CheckScanNO = ScanNo;
                    //string ScanHeader = _scan.ScanHeader.Substring(_scan.ScanHeader.IndexOf("ms2")+4, _scan.ScanHeader.IndexOf("@") - _scan.ScanHeader.IndexOf("ms2")-3) + "hcd";
                    do
                    {
                        CheckScanNO++;
                        if (Raw.GetHCDInfo(CheckScanNO) != null)
                        {
                            HCD = Raw.GetHCDInfo(CheckScanNO);
                            break;
                        }
                    } while (Raw.GetMsLevel(CheckScanNO) != 1); //Check Until hit Next Full MS

                    //CA: Complex Asialyated, CS:Complex Sialylated, HM:High mannose, HY:Hybrid and NA
                }
                List<object> PassInfo = new List<object>();
                PassInfo.Add(_scan);
                PassInfo.Add(HCD);
                PassInfo.Add(SW);

                WorkerTasks.Add(Task.Factory.StartNew(() => Sequencing(PassInfo)));
                // Sequencing(PassInfo);
                _scan = null;
                HCD = null;
            } //Foreach Scan
            Task.WaitAll(WorkerTasks.ToArray());


            SW.Close();

            this.lblCurrentScan.SafeBeginInvoke(
                new Action(
                    () =>
                        lblCurrentScan.Text =
                            "Finish in " + (DateTime.Now.Subtract(StartTime)).TotalMinutes.ToString() + " m"));
        }


    

        private void Sequencing(object argPassInfo)
        {
            List<object> PassInfo = (List<object>)argPassInfo;

            MSScan _scan = (MSScan)PassInfo[0];
            HCDInfo HCD = (HCDInfo)PassInfo[1];
            StreamWriter SW = (StreamWriter)PassInfo[2];
            int ScanNo = _scan.ScanNo;
            CurrentScan = ScanNo;
            int PrecursorCharge = _scan.ParentCharge;
            List<GlycanSequencing> lstGSequencing = new List<GlycanSequencing>();
            try
            {

                foreach (TargetPeptide TPeptide in _TargetPeptide)
                {
                    CurrentPeptide = TPeptide.PeptideSequence;
                    float PeptideMass = TPeptide.PeptideMass;
                    for (int j = PrecursorCharge - 1; j <= PrecursorCharge; j++)
                    {
                        int Y1ChargeSt = j;
                        if (j == 0)
                        {
                            continue;
                        }
                        float PredictedY1 = 0.0f;
                        PredictedY1 =
                            (float)
                                (PeptideMass + GlycanMass.GetGlycanAVGMass(Glycan.Type.HexNAc) +
                                 COL.MassLib.Atoms.ProtonMass * Y1ChargeSt) / Y1ChargeSt;
                        GlycanSequencing GS = null;
                        if (_UseGlycanList)
                        {
                            float GlycanMonoMass = (_scan.ParentMZ - Atoms.ProtonMass) * _scan.ParentCharge -
                                                 PeptideMass;
                            float PrecursorMono = _scan.ParentMonoMW;
                            List<GlycanCompound> ClosedGlycans = new List<GlycanCompound>();
                            foreach (float gMass in _GlycanCompoundMassList)
                            {
                                if (Math.Abs(gMass - GlycanMonoMass) < 100.0f)
                                {
                                    ClosedGlycans.Add(
                                        _GlycanCompounds[
                                            MassUtility.GetClosestMassIdx(_GlycanCompoundMassList, gMass)]);
                                }
                            }
                            foreach (GlycanCompound ClosedGlycan in ClosedGlycans)
                            {
                                if (_Human) //NeuAc
                                {
                                    int NoOfSia = ClosedGlycan.NoOfSia;
                                    int NoOfDeHex = ClosedGlycan.NoOfDeHex;
                                    if (HCD != null && HCD.GlycanType == COL.MassLib.enumGlycanType.CA &&
                                        ClosedGlycan.NoOfSia > 0)
                                    {
                                        NoOfDeHex = NoOfDeHex + NoOfSia * 2;
                                        NoOfSia = 0;
                                    }
                                    GS = new GlycanSequencing(_scan, TPeptide.PeptideSequence, true, Y1ChargeSt, ClosedGlycan.NoOfHex,
                                        ClosedGlycan.NoOfHexNAc, NoOfDeHex, NoOfSia, 0, @"d:\tmp", _NGlycan,
                                        _MSMSTol, _PrecursorTol);
                                }
                                else //NeuGc
                                {
                                    GS = new GlycanSequencing(_scan, TPeptide.PeptideSequence, true, Y1ChargeSt, ClosedGlycan.NoOfHex,
                                        ClosedGlycan.NoOfHexNAc, ClosedGlycan.NoOfDeHex, 0, ClosedGlycan.NoOfSia,
                                        @"d:\tmp", _NGlycan, _MSMSTol, _PrecursorTol);
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
                                //GS.ExporToFolder(_exportFolder);
                                if (_CompletedOnly && GS.FullSequencedStructures.Count == 0)
                                {
                                    continue;
                                }

                                lstGSequencing.Add(GS);

                                //Console.WriteLine("Scan:" + ScanNo.ToString()+"\t Peptide:" + Peptide + "  completed");
                                // bgWorker_Process.ReportProgress(ProcessReport);
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
                                        GS = new GlycanSequencing(_scan, TPeptide.PeptideSequence, true, Y1ChargeSt, _NoHex,
                                            _NoHexNAc, _NoDeHex, 0, 0, @"d:\tmp", _NGlycan, _MSMSTol, _PrecursorTol);
                                    }
                                    else if (HCD.GlycanType == enumGlycanType.HM)
                                    {
                                        GS = new GlycanSequencing(_scan, TPeptide.PeptideSequence, true, Y1ChargeSt, _NoHex, 2, 0, 0,
                                            0, @"d:\tmp", _NGlycan, _MSMSTol, _PrecursorTol);
                                    }
                                    else
                                    {
                                        GS = new GlycanSequencing(_scan, TPeptide.PeptideSequence, true, Y1ChargeSt, _NoHex,
                                            _NoHexNAc, _NoDeHex, _NoSia, 0, @"d:\tmp", _NGlycan, _MSMSTol,
                                            _PrecursorTol);
                                    }
                                }
                                else
                                {
                                    GS = new GlycanSequencing(_scan, TPeptide.PeptideSequence, true, Y1ChargeSt, _NoHex, _NoHexNAc,
                                        _NoDeHex, _NoSia, 0, @"d:\tmp", _NGlycan, _MSMSTol, _PrecursorTol);
                                }
                            }
                            else //NeuGc
                            {
                                if (HCD != null)
                                {
                                    //CA: Complex Asialyated, CS:Complex Sialylated, HM:High mannose, HY:Hybrid and NA
                                    if (HCD.GlycanType == enumGlycanType.CA)
                                    {
                                        GS = new GlycanSequencing(_scan, TPeptide.PeptideSequence, true, Y1ChargeSt, _NoHex,
                                            _NoHexNAc, _NoDeHex, 0, 0, @"d:\tmp", _NGlycan, _MSMSTol, _PrecursorTol);
                                    }
                                    else if (HCD.GlycanType == enumGlycanType.HM)
                                    {
                                        GS = new GlycanSequencing(_scan, TPeptide.PeptideSequence, true, Y1ChargeSt, _NoHex, 2, 0, 0,
                                            0, @"d:\tmp", _NGlycan, _MSMSTol, _PrecursorTol);
                                    }
                                    else
                                    {
                                        GS = new GlycanSequencing(_scan, TPeptide.PeptideSequence, true, Y1ChargeSt, _NoHex,
                                            _NoHexNAc, _NoDeHex, 0, _NoSia, @"d:\tmp", _NGlycan, _MSMSTol,
                                            _PrecursorTol);
                                    }
                                }
                                else
                                {
                                    GS = new GlycanSequencing(_scan, TPeptide.PeptideSequence, true, Y1ChargeSt, _NoHex, _NoHexNAc,
                                        _NoDeHex, 0, _NoSia, @"d:\tmp", _NGlycan, _MSMSTol, _PrecursorTol);
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
                            //GS.ExporToFolder(_exportFolder);
                            //Console.WriteLine("Scan:" + ScanNo.ToString()+"\t Peptide:" + Peptide + "  completed");
                            //bgWorker_Process.ReportProgress(ProcessReport);
                        }
                    } //Foreach charge
                    this.lblCurrentScan.SafeBeginInvoke(
                        new Action(() => lblCurrentScan.Text = CurrentScan.ToString() + "//" + CurrentPeptide));
                } //Foreach peptide

                // GenerateReportBody(lstGSequencing, swExport);
            }
            finally
            {
                //if (lstGSequencing.Count > 0)
                //{
                //    GenerateReportBody(lstGSequencing, SW);
                //}
                _scan.Dispose();
                _scan = null;
                HCD = null;
                completedScan = completedScan + 1;
                this.progressBar1.SafeBeginInvoke(
                    new Action(
                        () =>
                            progressBar1.Value =
                                Convert.ToInt32((completedScan / (float)(_lstScans.Count) * 100))));
                this.lblPercentage.SafeBeginInvoke(
                    new Action(() => lblPercentage.Text = progressBar1.Value.ToString() + "%"));
            }
        }

        private void bgWorker_Process_DoWork(object sender, DoWorkEventArgs e)
        {
            int ProcessReport = 0;
            ReportGenerator.CSVFormatParameter(_sequencingParameters,_exportFolder);
            foreach (int ScanNo in _lstScans)
            {
                if (bgWorker_Process!=null && bgWorker_Process.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }
                List<Tuple<string, GlycanStructure, double>> lstSequenceResult = new List<Tuple<string, GlycanStructure, double>>();
                if (Raw.GetMsLevel(ScanNo) == 1)
                {
                    CurrentScan = ScanNo;
                    ProcessReport =
                        Convert.ToInt32(((_lstScans.IndexOf(ScanNo)+1) / (float)(_lstScans.Count) * 100));
                    //Console.WriteLine("Scan:" + ScanNo.ToString()+"\t Peptide:" + Peptide + "  completed");
                    bgWorker_Process.ReportProgress(ProcessReport);
                    continue;
                }
                if (!_SeqHCD && !Raw.IsCIDScan(ScanNo))
                {
                    CurrentScan = ScanNo;
                    ProcessReport =
                        Convert.ToInt32(((_lstScans.IndexOf(ScanNo)+1) / (float)(_lstScans.Count) * 100));
                    //Console.WriteLine("Scan:" + ScanNo.ToString()+"\t Peptide:" + Peptide + "  completed");
                    bgWorker_Process.ReportProgress(ProcessReport);
                    continue;
                }
                MSScan _scan = Raw.ReadScan(ScanNo);

                HCDInfo HCD = null;
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
                    } while (Raw.GetMsLevel(CheckScanNO) != 1); //Check Until hit Next Full MS

                    //CA: Complex Asialyated, CS:Complex Sialylated, HM:High mannose, HY:Hybrid and NA
                }
                if (HCD != null)
                {
                    Console.WriteLine("CID Scan No:" + ScanNo.ToString() + "\tHCD Scan No:" + HCDScanNo.ToString() +
                                      "\tGlycanType:" + HCD.GlycanType.ToString());
                }

                GlycanSequencing_MultipleScoring GSM = null;
                List<Tuple<TargetPeptide, GlycanCompound>> lstGlycoPeptides = new List<Tuple<TargetPeptide, GlycanCompound>>();
                if (_glycanFile != null)
                {
                    if (!File.Exists(_exportFolder + "\\PrecursorMassMatch.csv"))
                    {
                        using (StreamWriter swNew = new StreamWriter(_exportFolder + "\\PrecursorMassMatch.csv"))
                        {
                            swNew.WriteLine("ScanNum,ParentMonoMW,PeptideSequence,PeptideMass,GlycanSequence,GlycanMass,PPM");
                        }
                    }
                    StreamWriter sw =new StreamWriter(_exportFolder+"\\PrecursorMassMatch.csv",true );
                    
                    foreach (TargetPeptide tPeptide in _TargetPeptide)
                    {
                        foreach (GlycanCompound glycan in lstGlycans)
                        {
                            double glycopeptideMass = tPeptide.PeptideMass + glycan.MonoMass - Atoms.HydrogenMass *2 - Atoms.OxygenMass;
                            if (MassUtility.GetMassPPM(glycopeptideMass, _scan.ParentMonoMW) < _PrecursorTol)
                            {
                                if (HCD != null && HCD.HCDScore >=4)
                                {
                                    //CA: Complex Asialyated, CS:Complex Sialylated, HM:High mannose, HY:Hybrid and NA
                                    if ( (HCD.GlycanType == enumGlycanType.CA   && (glycan.NoOfSia != 0))  || 
                                          (HCD.GlycanType == enumGlycanType.HM && (glycan.NoOfDeHex != 0 || glycan.NoOfSia != 0 || glycan.NoOfHexNAc != 2)) ||
                                          (HCD.GlycanType == enumGlycanType.CS   && (glycan.NoOfSia == 0)))
                                    {
                                       continue;
                                    }
                                    lstGlycoPeptides.Add(new Tuple<TargetPeptide, GlycanCompound>(tPeptide, glycan));
                                }
                                else
                                {
                                    lstGlycoPeptides.Add(new Tuple<TargetPeptide, GlycanCompound>(tPeptide, glycan));
                                }
                                string tmp = "";
                                tmp +=  _scan.ScanNo.ToString()+ ",";
                                tmp +=  _scan.ParentMonoMW.ToString("0.0000") +",";
                                tmp +=  tPeptide.PeptideSequence + ",";
                                tmp += tPeptide.PeptideMass.ToString("0.0000") + ",";
                                tmp += glycan.GlycanKey + ",";
                                tmp += (glycan.MonoMass - Atoms.HydrogenMass * 2 - Atoms.OxygenMass).ToString("0.0000")+",";
                                tmp += (MassUtility.GetMassPPM(glycopeptideMass, _scan.ParentMonoMW)).ToString("0.0000");
                                sw.WriteLine(tmp);
                            }
                        }
                    }
                    sw.Close();
                }
                if (HCD != null && lstGlycoPeptides.Count == 0)
                {
                    //CA: Complex Asialyated, CS:Complex Sialylated, HM:High mannose, HY:Hybrid and NA
                    if (HCD.GlycanType == enumGlycanType.CA)
                    {
                        GSM = new GlycanSequencing_MultipleScoring(_scan, PrecursorCharge, _NoHex,
                            _NoHexNAc, _NoDeHex, 0, 0, @"d:\tmp", _NGlycan, _MSMSTol, _PrecursorTol, _sequencingParameters.PeaksParameters, _peptideMassList);
                    }
                    else if (HCD.GlycanType == enumGlycanType.HM)
                    {
                        GSM = new GlycanSequencing_MultipleScoring(_scan, PrecursorCharge, _NoHex, 2, 0, 0,
                            0, @"d:\tmp", _NGlycan, _MSMSTol, _PrecursorTol, _sequencingParameters.PeaksParameters, _peptideMassList);
                    }
                    else
                    {
                        GSM = new GlycanSequencing_MultipleScoring(_scan, PrecursorCharge, _NoHex,
                            _NoHexNAc, _NoDeHex, _NoSia, 0, @"d:\tmp", _NGlycan, _MSMSTol,
                            _PrecursorTol, _sequencingParameters.PeaksParameters, _peptideMassList);
                    }
                }
                else if (HCD == null && lstGlycoPeptides.Count == 0)
                {
                    GSM = new GlycanSequencing_MultipleScoring(_scan, PrecursorCharge, _NoHex, _NoHexNAc,
                        _NoDeHex, _NoSia, 0, @"d:\tmp", _NGlycan, _MSMSTol, _PrecursorTol, _sequencingParameters.PeaksParameters, _peptideMassList);
                }
                else if ( lstGlycans.Count!=0)
                {
                    GSM = new GlycanSequencing_MultipleScoring(_scan, PrecursorCharge, lstGlycoPeptides, @"d:\tmp", _NGlycan, _MSMSTol, _PrecursorTol, _sequencingParameters.PeaksParameters);
                }

                GSM.UnknownPeptideSearch = _sequencingParameters.UnknownPeptideSearch;
                GSM.NumbersOfPeaksForSequencing = 140;
                GSM.UseAVGMass = _AverageMass;
                GSM.CreatePrecursotMZ = true;
                GSM.GlycansList = lstGlycans;
                GSM.PeptideList = _TargetPeptide;
                GSM.RewardForCompleteStructure = _CompletedReward;
                GSM.PeptideFromMascotResult = _sequencingParameters.PeptideFromMascotResult;
                if (HCD != null)
                {
                    GSM.GlycanType = HCD.GlycanType;
                }
                GSM.StartSequencing();
                //GS.ExportToFolder(_exportFolder, _ExportIndividualSpectrum, _GetTopRank);
                
                if (GSM.SequencedStructures.Count > 0)
                {
                    if (_CompletedOnly)
                    {
                        foreach (GlycanStructure g in GSM.FullSequencedStructures)
                        {
                            Tuple<string, GlycanStructure, double> Result;
                            if (g.TargetPeptide != null && g.TargetPeptide.IdentifiedPeptide)
                            {
                                Result =
                                    new Tuple<string, GlycanStructure, double>(
                                        g.PeptideSequence + "+" + g.PeptideModificationString, g, g.Score);
                            }
                            else
                            {
                                Result =
                                    new Tuple<string, GlycanStructure, double>(
                                        g.PeptideSequence + "+" + g.PeptideModificationString + "#", g, g.Score);
                            }
                            if (!lstSequenceResult.Contains(Result))
                            {
                                lstSequenceResult.Add(Result);
                            }
                        }
                    }
                    else
                    {
                        foreach (GlycanStructure g in GSM.GetTopRankScoreStructre(_GetTopRank))
                        {
                            Tuple<string, GlycanStructure, double> Result;
                            if (g.TargetPeptide != null && g.TargetPeptide.IdentifiedPeptide)
                            {
                                Result =
                                    new Tuple<string, GlycanStructure, double>(
                                        g.PeptideSequence + "+" + g.PeptideModificationString, g, g.Score);
                            }
                            else
                            {
                                Result =
                                    new Tuple<string, GlycanStructure, double>(
                                        g.PeptideSequence + "+" + g.PeptideModificationString + "#", g, g.Score);
                            }
                            if (!lstSequenceResult.Contains(Result))
                            {
                                lstSequenceResult.Add(Result);
                            }
                        }
                    }
                }
                CurrentScan = ScanNo;
                CurrentPeptide = GSM.PeptideSeq;
                ProcessReport = Convert.ToInt32(((_lstScans.IndexOf(ScanNo) + 1) / (float)(_lstScans.Count) * 100));
                //Console.WriteLine("Scan:" + ScanNo.ToString()+"\t Peptide:" + Peptide + "  completed");
                if (e.Cancel)
                {
                    break;
                }
                bgWorker_Process.ReportProgress(ProcessReport);

                //Update SVM score
                var glycanStructure = GSM.FullSequencedStructures.GroupBy(x => new { GlycanTotal = x.NoOfTotalGlycan, Score = x.BranchScore + x.CoreScore }).OrderByDescending(y => y.Key.GlycanTotal).ThenByDescending(z => z.Key.Score).ToList();
                glycanStructure.AddRange(GSM.SequencedStructures.GroupBy(x => new { GlycanTotal = x.NoOfTotalGlycan, Score = x.BranchScore + x.CoreScore }).OrderByDescending(y => y.Key.GlycanTotal).ThenByDescending(z => z.Key.Score).ToList());
                List<List<double>> allSVMMatrices = new List<List<double>>();

                foreach (var glycans in glycanStructure.AsEnumerable())
                {
                    foreach (var glycan in glycans.AsEnumerable())
                    {
                        allSVMMatrices.Add(glycan.SVMMatrices);
                    }
                }
                List<Tuple<int, List<double>>> results = SVMScoring.GetSVMScore(allSVMMatrices);

                int idx = 0;
                foreach (var glycans in glycanStructure.AsEnumerable())
                {
                    foreach (var glycan in glycans.AsEnumerable())
                    {
                        glycan.SVMPredictedLabel = results[idx].Item1;
                        glycan.SVMPrrdictedProbabilities = results[idx].Item2;
                        idx++;
                    }
                }
                //Export
                if (lstSequenceResult.Count > 0 && !e.Cancel)
                {
                    //ReportGenerator.HtmlFormatTempForEachScan(_exportFolder, _scan, _ExportIndividualSpectrum, _GetTopRank,lstSequenceResult);
                    ReportGenerator.CSVFormat(_exportFolder, _scan, _GetTopRank, lstSequenceResult);
                    foreach (Tuple<string, GlycanStructure, double> result in lstSequenceResult)
                    {

                        if (!dictResultSortByPeptide.ContainsKey(result.Item1))
                        {
                            dictResultSortByPeptide.Add(result.Item1, new List<Tuple<int, string, Tuple<double, double, double>, string, string, bool, double>>());
                        }

                        if (result.Item2.MatchWithPrecursorMW == true ||
                            result.Item2.IsCompleteByPrecursorDifference == true)
                        {
                            dictResultSortByPeptide[result.Item1].Add(
                                new Tuple<int, string, Tuple<double, double, double>, string, string, bool, double>(_scan.ScanNo,
                                    result.Item2.IUPACString, new Tuple<double, double, double>(result.Item2.CoreScore, result.Item2.BranchScore,
                                    result.Item2.InCompleteScore), result.Item2.FullSequencedGlycanString, result.Item2.RestGlycanString,
                                    true, result.Item2.PPM));
                        }
                        else
                        {
                            dictResultSortByPeptide[result.Item1].Add(
                                new Tuple<int, string, Tuple<double, double, double>, string, string, bool, double>(_scan.ScanNo,
                                    result.Item2.IUPACString, new Tuple<double, double, double>(result.Item2.CoreScore, result.Item2.BranchScore,
                                    result.Item2.InCompleteScore), result.Item2.FullSequencedGlycanString, result.Item2.RestGlycanString,
                                    false, result.Item2.PPM));
                        }
                    }
                }
                if (_exportLearingMatrix)
                {
                    ReportGenerator.GenerateLearningMatrix(GSM,_exportFolder+"\\LeanringMatrix.csv",_GetTopRank);
                }


                
            } //Foreach Scan
        }
       
        private void bgWorker_Process_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            try
            {
                lblCurrentScan.Text = (CurrentScan + 1).ToString();

                progressBar1.Value = e.ProgressPercentage;
                lblPercentage.Text = e.ProgressPercentage.ToString() + "%";
                lblRunningTime.Text = DateTime.Now.Subtract(StartTime).TotalMinutes.ToString("0.00");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void bgWorker_Process_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                Directory.Delete(_sequencingParameters.ExportFolder, true);
                return;
            }
            
            lblCurrentScan.Text = "Outputing";
            progressBar1.Value = 99;
            lblPercentage.Text = "99%";

            //Copy
            GlycanSeq_Form.Properties.Resources.DeHex.Save(_exportFolder+"\\Pics\\DeHex.png");
            GlycanSeq_Form.Properties.Resources.Hex.Save(_exportFolder+"\\Pics\\Hex.png");
            GlycanSeq_Form.Properties.Resources.HexNAc.Save(_exportFolder+"\\Pics\\HexNAc.png");
            GlycanSeq_Form.Properties.Resources.NeuAc.Save(_exportFolder+"\\Pics\\NeuAc.png");
            GlycanSeq_Form.Properties.Resources.NeuGc.Save(_exportFolder+"\\Pics\\NeuGc.png");
            GlycanSeq_Form.Properties.Resources.bracket.Save(_exportFolder+"\\Pics\\bracket.jpg");
            //Merge Report

            ReportGenerator.GenerateHtmlReport(_sequencingParameters);
            //ReportGenerator.GenerateHtmlReportSortByPeptide(_exportFolder, dictResultSortByPeptide, _sequencingParameters);
            //ReportGenerator.GenerateHtmlReportSortByScan(_sequencingParameters);
            //MergeResults(swExport);

            //foreach (GlycanSequencing GS in _lstGS)
            //{
            //    GS.ExporToFolder(_exportFolder);
            //}
            //FileInfo NewFile = new FileInfo(_exportFoldername);

            //int OutputRowCount = 0;
            //if (NewFile.Exists)
            //{
            //    File.Delete(NewFile.FullName);
            //}
            //OfficeOpenXml.ExcelPackage pck = new OfficeOpenXml.ExcelPackage(NewFile);

            //if (pck.Workbook.Worksheets[Path.GetFileNameWithoutExtension(Raw.RawFilePath)] != null)
            //{
            //    pck.Workbook.Worksheets.Delete(Path.GetFileNameWithoutExtension(Raw.RawFilePath));
            //}

            //var ws = pck.Workbook.Worksheets.Add(Path.GetFileNameWithoutExtension(Raw.RawFilePath));
            //ws.DefaultRowHeight = 50;
            //OutputRowCount++;
            //ws.Cells[OutputRowCount, 1].Value = "CID_Scan";
            //ws.Cells[OutputRowCount, 2].Value = "Parent_Scan";
            //ws.Cells[OutputRowCount, 3].Value = "Parent_Mz";
            //ws.Cells[OutputRowCount, 4].Value = "GlycoPeptide_Mz";
            //ws.Cells[OutputRowCount, 5].Value = "Y1_Mz";
            //ws.Cells[OutputRowCount, 6].Value = "Peptide_Sequence";
            //ws.Cells[OutputRowCount, 7].Value = "Glycan IUPAC";
            //ws.Cells[OutputRowCount, 8].Value = "No_of_Glycans";
            //ws.Cells[OutputRowCount, 9].Value = "HexNac-Hex-DeHex-NeuAc";
            //ws.Cells[OutputRowCount, 10].Value = "Full_Structure";
            //ws.Cells[OutputRowCount, 11].Value = "Score";
            //ws.Cells[OutputRowCount, 12].Value = "PPM";
            //ws.Cells[OutputRowCount, 13].Value = "Picture";
            //int count = 0;
            //foreach (GlycanSequencing GS in _lstGS)
            //{
            //    if (GS.SequencedStructures.Count == 0)
            //    {
            //        continue;
            //    }
            //    // string exportStr = "";
            //    if (GS.FullSequencedStructures.Count != 0) //Print Complete
            //    {
            //        //GS.FullSequencedStructures.Sort(
            //        //        delegate(GlycanStructure GS1, GlycanStructure GS2)
            //        //        {
            //        //            if (GS1.NoOfTotalGlycan.CompareTo(GS2.NoOfTotalGlycan) == 0) //Equal compare score 
            //        //            {
            //        //                return -1 * GS1.Score.CompareTo(GS2.Score);
            //        //            }
            //        //            else
            //        //            {
            //        //                return -1 * GS1.NoOfTotalGlycan.CompareTo(GS2.NoOfTotalGlycan);
            //        //            }
            //        //        }
            //        //   );
            //        List<KeyValuePair<GlycanStructure, double>> SortedGlycanStructuresByMassDifferences = new List<KeyValuePair<GlycanStructure, double>>();
            //        foreach (GlycanStructure g in GS.FullSequencedStructures)
            //        {
            //            SortedGlycanStructuresByMassDifferences.Add(new KeyValuePair<GlycanStructure, double>(g, MassUtility.GetMassPPM(g.GlycanMonoMass + AAMW.GetMonoMW(GS.PeptideSeq,true), GS.PrecusorMonoMass)));
            //        }
            //            SortedGlycanStructuresByMassDifferences.Sort((firstPair, nextPair) =>
            //            {
            //                if (firstPair.Value.CompareTo(nextPair.Value) == 0)
            //                {
            //                    return -1 * firstPair.Key.Score.CompareTo(nextPair.Key.Score);
            //                }
            //                else
            //                {
            //                    return firstPair.Value.CompareTo(nextPair.Value);
            //                }
            //            }
            //        );

            //            try
            //            {
            //                foreach (KeyValuePair<GlycanStructure, double> glyc in SortedGlycanStructuresByMassDifferences)
            //                {
            //                    if (glyc.Value >= _PrecursorTol)
            //                    {
            //                        continue;
            //                    }

            //                    GlycanStructure g = glyc.Key;
            //                    string ComStr = g.NoOfHexNac.ToString() + "-" + g.NoOfHex.ToString() + "-" + g.NoOfDeHex.ToString() + "-" + g.NoOfNeuAc.ToString();

            //                    OutputRowCount++;
            //                    ws.Cells[OutputRowCount, 1].Value = GS.ScanInfo.ScanNo;
            //                    ws.Cells[OutputRowCount, 2].Value = GS.ScanInfo.ParentScanNo;
            //                    ws.Cells[OutputRowCount, 3].Value = GS.ScanInfo.ParentMZ;
            //                    ws.Cells[OutputRowCount, 4].Value = (g.Charge - 1);
            //                    ws.Cells[OutputRowCount, 5].Value = g.Y1.Mass;
            //                    ws.Cells[OutputRowCount, 6].Value = GS.PeptideSeq;
            //                    ws.Cells[OutputRowCount, 7].Value = g.IUPACString;
            //                    ws.Cells[OutputRowCount, 8].Value = g.NoOfTotalGlycan;
            //                    ws.Cells[OutputRowCount, 9].Value = ComStr;
            //                    ws.Cells[OutputRowCount, 10].Value = "Y";
            //                    ws.Cells[OutputRowCount, 11].Value = Convert.ToDouble(g.Score.ToString("0.000"));
            //                    ws.Cells[OutputRowCount, 12].Value = Convert.ToDouble(glyc.Value.ToString("0.000"));
            //                    //Create picture                                                    
            //                    if (count <= 2000)
            //                    {
            //                        COL.GlycoLib.GlycansDrawer glycanimg = new GlycansDrawer(g.IUPACString);
            //                        OfficeOpenXml.Drawing.ExcelPicture glycanImg = ws.Drawings.AddPicture("Glycan_" + OutputRowCount + "-12", glycanimg.GetImage());
            //                        glycanImg.SetPosition(OutputRowCount - 1, 0, 13 - 1, 0);
            //                    }
            //                    count++;
            //                }
            //            }
            //        catch
            //        {
            //            Console.WriteLine(count);
            //        }

            //    }
            //    else //Print Partial Top Tank
            //    {
            //        if (_CompletedOnly == true)
            //        {
            //            continue;
            //        }
            //        //   List<GlycanStructure> SequencedStructure = GS.GetTopRankNoNCompletedSequencedStructre(Convert.ToInt32(_GetTopRank));

            //        //   SequencedStructure.Sort(
            //        //     delegate(GlycanStructure GS1, GlycanStructure GS2)
            //        //     {
            //        //         if (GS1.NoOfTotalGlycan.CompareTo(GS2.NoOfTotalGlycan) == 0) //Equal compare score 
            //        //         {
            //        //             return -1 * GS1.Score.CompareTo(GS2.Score);
            //        //         }
            //        //         else
            //        //         {
            //        //             return -1 * GS1.NoOfTotalGlycan.CompareTo(GS2.NoOfTotalGlycan);
            //        //         }
            //        //     }
            //        //);
            //        //  foreach (GlycanStructure g in SequencedStructure)
            //        List<KeyValuePair<GlycanStructure, double>> SortedGlycanStructuresByMassDifferences = new List<KeyValuePair<GlycanStructure, double>>();
            //        foreach (GlycanStructure g in  GS.GetTopRankNoNCompletedSequencedStructre(Convert.ToInt32(_GetTopRank)))
            //        {
            //            SortedGlycanStructuresByMassDifferences.Add(new KeyValuePair<GlycanStructure, double>(g, MassUtility.GetMassPPM(g.GlycanMonoMass + AAMW.GetMonoMW(GS.PeptideSeq, true), GS.PrecusorMonoMass)));
            //        }
            //        SortedGlycanStructuresByMassDifferences.Sort((firstPair, nextPair) =>
            //        {
            //            if (firstPair.Value.CompareTo(nextPair.Value) == 0)
            //            {
            //                return -1 * firstPair.Key.Score.CompareTo(nextPair.Key.Score);
            //            }
            //            else
            //            {
            //                return firstPair.Value.CompareTo(nextPair.Value);
            //            }
            //        }
            //        );

            //        foreach (KeyValuePair<GlycanStructure, double> glyc in SortedGlycanStructuresByMassDifferences)
            //        {
            //            GlycanStructure g = glyc.Key;

            //            string ComStr = g.NoOfHexNac.ToString() + "-" + g.NoOfHex.ToString() + "-" + g.NoOfDeHex.ToString() + "-" + g.NoOfNeuAc.ToString();


            //            OutputRowCount++;
            //            ws.Cells[OutputRowCount, 1].Value = GS.ScanInfo.ScanNo;
            //            ws.Cells[OutputRowCount, 2].Value = GS.ScanInfo.ParentScanNo;
            //            ws.Cells[OutputRowCount, 3].Value = GS.ScanInfo.ParentMZ;
            //            ws.Cells[OutputRowCount, 4].Value = (g.Charge - 1);
            //            ws.Cells[OutputRowCount, 5].Value = g.Y1.Mass;
            //            ws.Cells[OutputRowCount, 6].Value = GS.PeptideSeq;
            //            ws.Cells[OutputRowCount, 7].Value = g.IUPACString;
            //            ws.Cells[OutputRowCount, 8].Value = g.NoOfTotalGlycan;
            //            ws.Cells[OutputRowCount, 9].Value = ComStr;
            //            ws.Cells[OutputRowCount, 10].Value = "N";
            //            ws.Cells[OutputRowCount, 11].Value = Convert.ToDouble(g.Score.ToString("0.000"));
            //            ws.Cells[OutputRowCount, 12].Value = Convert.ToDouble(glyc.Value.ToString("0.000"));
            //            //Create picture                                                    
            //            COL.GlycoLib.GlycansDrawer glycanimg = new GlycansDrawer(g.IUPACString);
            //            OfficeOpenXml.Drawing.ExcelPicture glycanImg = ws.Drawings.AddPicture("Glycan_" + OutputRowCount + "-12", glycanimg.GetImage());
            //            glycanImg.SetPosition(OutputRowCount - 1, 0, 12 - 1, 0);                    
            //        }
            //    }           
            //}//For glycansequencing
            //pck.Workbook.Worksheets[1].Column(5).Width = 12;
            //pck.Workbook.Worksheets[1].Column(6).Width = 30;
            //pck.Workbook.Worksheets[1].Column(7).Width = 50;
            //pck.Workbook.Worksheets[1].Column(8).Width = 15;
            //pck.Workbook.Worksheets[1].Column(9).Width = 25;
            //pck.Workbook.Worksheets[1].Column(10).Width = 13;
            //pck.Save();
            lblCurrentScan.Text = "Done";
            progressBar1.Value = 100;
            lblPercentage.Text = "100%";
            EndTime = new DateTime(DateTime.Now.Ticks);

            //if (count > 2000)
            //{
            //    if (MessageBox.Show("Output file exceed memory limitation, some pics can not be generated!!\n\n Elapsed Tme:" + EndTime.Subtract(StartTime).TotalSeconds.ToString() + "sec.") == System.Windows.Forms.DialogResult.OK)
            //    {
            //        this.Close();
            //    }
            //}
            //else
            //{
            //    if (MessageBox.Show("Done!!\n\n Elapsed Tme:" + EndTime.Subtract(StartTime).TotalSeconds.ToString() +"sec.") == System.Windows.Forms.DialogResult.OK)
            //    {
            //        this.Close();
            //    }
            //}
        }

        private void frmProcessing_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (bgWorker_Process.IsBusy)
            {
                if (MessageBox.Show("Cancel all process?", "Cancel", MessageBoxButtons.YesNo) ==
                    System.Windows.Forms.DialogResult.Yes)
                {
                    bgWorker_Process.CancelAsync();
                    //Delete file and folder
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }

        private void frmProcessing_FormClosed(object sender, FormClosedEventArgs e)
        {
            
        }

        //Old

        //Merge to Class GenerateHtmlReport
        //private void MergeResults(StreamWriter argSW)
        //{
        //    argSW.WriteLine(
        //        "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">");
        //    argSW.WriteLine("<html xmlns=\"http://www.w3.org/1999/xhtml\">");
        //    argSW.WriteLine("<head>");
        //    argSW.WriteLine("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />");
        //    argSW.WriteLine("<title>Glycoseq result for raw " + Path.GetFileName(_rawFilePath) +
        //                    "</title>\n</head>\n<body>");
        //    argSW.WriteLine("<Table border=\"1\">");

        //    argSW.WriteLine("<tr>\n\t<td>Raw File: </td>\n\t<td>" + _rawFilePath + "</td>\n</tr>");
        //    argSW.WriteLine("<tr>\n\t<td>Scan Range: </td>\n\t<td>" + _StartScan.ToString() + "~" + _EndScan.ToString() +
        //                    "</td>\n</tr>");
        //    //argSW.WriteLine("<tr>\n\t<td>Fasta File: </td>\n\t<td>" + _fastaFile + "</td>\n</tr>");
        //    string tmp = "";
        //    //foreach (Protease.Type p in _ProteaseType)
        //    //{
        //    //    tmp = tmp + p.ToString() + ",";
        //    //}
        //    //argSW.WriteLine("<tr>\n\t<td>ProteaseType: </td>\n\t<td>" + tmp + "</td>\n</tr>");
        //    //argSW.WriteLine("<tr>\n\t<td>MissCleavage: </td>\n\t<td>" + _MissCLeavage.ToString() + "</td>\n</tr>");
        //    if (_UseGlycanList)
        //    {
        //        argSW.WriteLine("<tr>\n\t<td>Glycan File: </td>\n\t<td>" + _glycanFile + "</td>\n</tr>");
        //    }
        //    else
        //    {
        //        tmp = "HexNAc:" + _NoHexNAc.ToString() + ";" +
        //              "Hex:" + _NoHex.ToString() + ";" +
        //              "Sia" + _NoSia.ToString() + ";" +
        //              "DeHex:" + _NoDeHex.ToString() + ";";
        //        argSW.WriteLine("<tr>\n\t<td>Glycan Composition: </td>\n\t<td>" + tmp + "</td>\n</tr>");
        //    }
        //    argSW.WriteLine("<tr>\n\t<td>MS Tol/MS2 Tol: </td>\n\t<td>" + _PrecursorTol.ToString() + "/" +
        //                    _MSMSTol.ToString() + "</td>\n</tr>");
        //    argSW.WriteLine("<tr>\n\t<td>N-Glycan/Human: </td>\n\t<td>" + _NGlycan.ToString() + "/" + _Human.ToString() +
        //                    "</td>\n</tr>");
        //    argSW.WriteLine("<tr>\n\t<td>Use HCD/Sequencing HCD: </td>\n\t<td>" + _UseHCD.ToString() + "/" +
        //                    _SeqHCD.ToString() + "</td>\n</tr>");
        //    argSW.WriteLine("<tr>\n\t<td>Export Completed Only/Completed Reward: </td>\n\t<td>" +
        //                    _CompletedOnly.ToString() + "/" + _CompletedReward.ToString() + "</td>\n</tr>");
        //    argSW.WriteLine("</Table>\n<br>\n<br>");
        //    argSW.Flush();
        //    StreamReader Sr;
        //    foreach (string fileName in Directory.GetFiles(_exportFolder))
        //    {
        //        if (Path.GetExtension(fileName) == ".txt")
        //        {
        //            Sr = new StreamReader(fileName);
        //            do
        //            {
        //                argSW.WriteLine(Sr.ReadLine());
        //            } while (!Sr.EndOfStream);
        //            Sr.Close();
        //            File.Delete(fileName);
        //            argSW.WriteLine("<a>");
        //        }
        //    }
        //    argSW.WriteLine("<br></body>\n</html>");
        //}
        //private void GenerateReportBody(List<GlycanSequencing> argGSequencing, StreamWriter argSW)
        //{
        //    lock (lockObject)
        //    {
        //        argSW.WriteLine("<h1>Scan number:" + argGSequencing[0].ScanInfo.ScanNo.ToString() + "[Precursor m/z:" +
        //                        argGSequencing[0].ScanInfo.ParentMZ.ToString("0.000") + "]</h1>");
        //        argSW.WriteLine("<Table border=\"1\">");

        //        foreach (GlycanSequencing GS in argGSequencing)
        //        {
        //            List<GlycanStructure> ExportStructure;
        //            if (_CompletedOnly)
        //            {
        //                ExportStructure = GS.FullSequencedStructures;
        //            }
        //            else
        //            {
        //                ExportStructure = GS.GetTopRankScoreStructre(_GetTopRank);
        //            }

        //            argSW.WriteLine("<tr>");
        //            argSW.WriteLine("\t<td colspan=3>Peptide:" + GS.PeptideSeq + "</td></tr>");
        //            argSW.WriteLine("<tr>\n\t<td>Score</td>\n\t<td>Glycan  IUPAC</td>\n\t<td>IMG</td>\n</tr>");
        //            foreach (GlycanStructure gs in ExportStructure)
        //            {
        //                string PicLocation = Path.GetDirectoryName(_exportFolder) + "\\Pics\\" + gs.IUPACString.ToString() +
        //                                     ".png";
        //                //if (!File.Exists(PicLocation))
        //                //{
        //                //    GlycansDrawer Draw = new GlycansDrawer(gs.IUPACString, false);
        //                //    Image Pic = Draw.GetImage();

        //                //    System.IO.MemoryStream mss = new System.IO.MemoryStream();
        //                //    System.IO.FileStream fs = new System.IO.FileStream(PicLocation, System.IO.FileMode.Create, System.IO.FileAccess.ReadWrite);

        //                //    Pic.Save(mss, System.Drawing.Imaging.ImageFormat.Png);
        //                //    byte[] matriz = mss.ToArray();
        //                //    fs.Write(matriz, 0, matriz.Length);

        //                //    mss.Close();
        //                //    fs.Close();
        //                //    Pic.Dispose();
        //                //    Pic = null;
        //                //    Draw = null;
        //                //}
        //                argSW.WriteLine("<tr>\n\t<td>" + gs.Score.ToString("0.00") + "</td>\n\t<td>" + gs.IUPACString +
        //                                "</td>\n\t<td><img src=\".\\Pics\\" + gs.IUPACString.ToString() +
        //                                ".png\"/></td>\n</tr>");
        //            }
        //        }
        //        argSW.WriteLine("</table>\n<br><br>");
        //        argSW.Flush();
        //    }
        //}

        //private List<string> GenerateGlycoPeptide()
        //{
        //    List<ProteinInfo> PInfos = FastaReader.ReadFasta(_fastaFile);
        //    List<string> GlycoPeptide = new List<string>();
        //    foreach (ProteinInfo Pinfo in PInfos)
        //    {
        //        GlycoPeptide.AddRange(Pinfo.NGlycopeptide(_MissCLeavage,_ProteaseType,_PeptideMutation));
        //    }
        //    return GlycoPeptide;
        //}

        //private void WriteParametersToXML()
        //{
        //    List<int> EndScans = new List<int>();
        //    int PartStart = _StartScan;
        //    int PartEndScan = 0;
        //    int DivideInterval = 1000;
        //    while (true)
        //    {
        //        PartEndScan = PartStart + DivideInterval - 1;
        //        if (PartEndScan >= _EndScan)
        //        {
        //            PartEndScan = _EndScan;
        //            EndScans.Add(PartEndScan);
        //            break;
        //        }
        //        else
        //        {
        //            EndScans.Add(PartEndScan);
        //            PartStart = PartEndScan + 1;
        //        }
        //    }
        //    DividePartNum = EndScans.Count;
        //    for (int i = 0; i < EndScans.Count; i++)
        //    {
        //        XmlWriter writer =
        //            XmlWriter.Create(Path.GetDirectoryName(_exportFolder) + "\\" +
        //                             Path.GetFileNameWithoutExtension(_exportFolder) + "_" + (i + 1).ToString() + ".xml");
        //        writer.WriteStartDocument();
        //        writer.WriteStartElement("GlycanSequencing");
        //        //Raw
        //        writer.WriteStartElement("RawFile");
        //        writer.WriteElementString("FilePath", _rawFilePath);
        //        //writer.WriteElementString("StartScan",(EndScans[i] - DivideInterval+1).ToString());
        //        //writer.WriteElementString("EndScan", EndScans[i].ToString());
        //        writer.WriteElementString("SequencingHCD", _SeqHCD.ToString());
        //        writer.WriteElementString("UseHCDInfo", _UseHCD.ToString());
        //        for (int j = EndScans[i] - DivideInterval + 1; j <= EndScans[i]; j++)
        //        {
        //            writer.WriteElementString("Scan", j.ToString());
        //        }
        //        writer.WriteEndElement();
        //        //Fasta
        //        // writer.WriteStartElement("FastaFile");
        //        //writer.WriteElementString("FilePath", _fastaFile);

        //        //foreach (string glycopeptide in GenerateGlycoPeptide())
        //        //{
        //        //    writer.WriteElementString("GlycoPeptide", glycopeptide);
        //        //}
        //        //writer.WriteEndElement();
        //        //Glycan
        //        writer.WriteStartElement("Glycans");
        //        writer.WriteAttributeString("GlycansFile", _UseGlycanList.ToString());
        //        if (_UseGlycanList)
        //        {
        //            writer.WriteElementString("FilePath", _glycanFile);
        //        }
        //        else
        //        {
        //            writer.WriteElementString("Hex", _NoHex.ToString());
        //            writer.WriteElementString("HexNAc", _NoHexNAc.ToString());
        //            writer.WriteElementString("deHex", _NoDeHex.ToString());
        //            writer.WriteElementString("Sia", _NoSia.ToString());
        //        }
        //        writer.WriteElementString("NLink", _NGlycan.ToString());
        //        writer.WriteElementString("Human", _Human.ToString());
        //        writer.WriteElementString("AvgMass", _AverageMass.ToString());
        //        writer.WriteEndElement();
        //        //Torelance
        //        writer.WriteStartElement("Torelance");
        //        writer.WriteAttributeString("MSMS_Da", _MSMSTol.ToString());
        //        writer.WriteAttributeString("Precursor_PPM", _PrecursorTol.ToString());
        //        writer.WriteEndElement();

        //        //Export
        //        writer.WriteStartElement("Export");
        //        writer.WriteAttributeString("GetTopRank", _GetTopRank.ToString());
        //        writer.WriteAttributeString("OnlyCompleteStructure", _CompletedOnly.ToString());
        //        writer.WriteAttributeString("CompletedReward", _CompletedReward.ToString());
        //        writer.WriteEndElement();

        //        writer.WriteEndElement();
        //        writer.WriteEndDocument();
        //        writer.Flush();
        //        writer.Close();
        //    }
        //}


        //private void StartProcess()
        //{
        //    Thread ThreadUpdate = new Thread(() => UpdateStatus());
        //    ThreadUpdate.Start();
        //    for (int i = 1; i <= DividePartNum; i++)
        //    {
        //        CurrentRunningPart = i;
        //        RunExecutable(i);
        //    }
        //    ThreadUpdate.Abort();
        //    this.lblCurrentScan.SafeBeginInvoke(new Action(() => lblCurrentScan.Text = "Exporting Result"));
        //    MergeReport();
        //}

        //private void MergeReport()
        //{
        //    StreamWriter sw = new StreamWriter(_exportFolder);
        //    GenerateReportHeader(sw);
        //    for (int i = 1; i <= DividePartNum; i++)
        //    {
        //        if (
        //            File.Exists(Path.GetDirectoryName(_exportFolder) + "\\" +
        //                        Path.GetFileNameWithoutExtension(_exportFolder) + "_" + (i).ToString() + ".xml_tmp"))
        //        {
        //            //
        //            StreamReader sr =
        //                new StreamReader(Path.GetDirectoryName(_exportFolder) + "\\" +
        //                                 Path.GetFileNameWithoutExtension(_exportFolder) + "_" + (i).ToString() +
        //                                 ".xml_tmp");
        //            sw.Write(sr.ReadToEnd());
        //            sr.Close();
        //            //Delete xml Parameters
        //            File.Delete(Path.GetDirectoryName(_exportFolder) + "\\" +
        //                        Path.GetFileNameWithoutExtension(_exportFolder) + "_" + (i).ToString() + ".xml");
        //            File.Delete(Path.GetDirectoryName(_exportFolder) + "\\" +
        //                        Path.GetFileNameWithoutExtension(_exportFolder) + "_" + (i).ToString() + ".xml_tmp");
        //        }
        //    }
        //    GenerateReportFooter(sw);
        //    sw.Close();
        //}

    }
} //class

public static class ControlExtensions
{
    public static void SafeInvoke(this Control control, Action action)
    {
        if (control.InvokeRequired)
        {
            control.Invoke(action);
        }
        else
        {
            action();
        }
    }

    public static void SafeBeginInvoke(this Control control, Action action)
    {
        if (control.InvokeRequired)
        {
            control.BeginInvoke(action);
        }
        else
        {
            action();
        }
    }
}

internal class RunResults
{
    public int ExitCode;
    public Exception RunException;
    public string Output;
    public StringBuilder Error;
    public int PID;
}