using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Windows.Forms;
using System.IO;
using COL.GlycoSequence;
using COL.ProtLib;
using COL.GlycoLib;
using COL.MassLib;
namespace GlycanSeq_Runner
{
    public partial class frmGlycanSeqRunner : Form
    {
        bool _UseHCD, _NGlycan, _Human, _CompletedOnly, _SeqHCD, _AverageMass, _UseGlycanList;
        int _NoHexNAc, _NoHex, _NoDeHex, _NoSia, _GetTopRank;
        float _MSMSTol, _PrecursorTol, _CompletedReward;
        List<string> _Peptides;
        List<int> lstScans;
        string _rawFile, _glycanFile, _exportFile;
        ThermoRawReader Raw = null;

        int CurrentScan = 0;
        string CurrentPeptide = "";
        DateTime StartTime;
        AminoAcidMass AAMW;
        Dictionary<double, GlycanCompound> _MassGlycanMapping;
        List<float> _GlycanCompoundMassList;
        List<GlycanCompound> _GlycanCompounds;
        public frmGlycanSeqRunner(string[] args)
        {
            StartTime = new DateTime(DateTime.Now.Ticks);
            this.Text = "";
            InitializeComponent();
            string argXML = "";
            openFileDialog1.Title = "Choose Sequencing parameterf file";
            openFileDialog1.Filter = "XML file(*.xml)|*.xml";
            if (args.Length <= 0)
            {
                while (true)
                {
                    if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        argXML = openFileDialog1.FileName;
                        break;
                    }
                    else
                    {
                        if (MessageBox.Show("Please select Sequencing XML parameters file. Do you want to cancel?", "No parameter file selected", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                        {
                            Environment.Exit(0);
                        }
                    }
                }

            }
            else
            {
                argXML = args[0];
            }
            this.Text = Path.GetFileNameWithoutExtension(argXML);
            _exportFile = argXML + "_tmp";
            ParseXMLFile(argXML);

            bgWorker_Process.RunWorkerAsync();
            //bgWorker_Process_DoWork(this, new DoWorkEventArgs(this));
        }

        private void bgWorker_Process_DoWork(object sender, DoWorkEventArgs e)
        {
            AAMW = new AminoAcidMass();
            this.lblStatus.SafeBeginInvoke(new Action(() => lblStatus.Text = "Begin initial raw file"));
            Raw = new ThermoRawReader(_rawFile);
            this.lblStatus.SafeBeginInvoke(new Action(() => lblStatus.Text = "Initial raw file completed"));
            List<GlycanSequencing> lstGS = new List<GlycanSequencing>();
            if (_UseGlycanList)
            {
                _GlycanCompounds = ReadGlycanListFromFile.ReadGlycanList(_glycanFile, false, _Human, false);
                _MassGlycanMapping = new Dictionary<double, GlycanCompound>();
                _GlycanCompoundMassList = new List<float>();
                foreach (GlycanCompound G in _GlycanCompounds)
                {
                    if (!_MassGlycanMapping.ContainsKey(G.AVGMass))
                    {
                        _MassGlycanMapping.Add(G.AVGMass, G);
                        _GlycanCompoundMassList.Add((float)G.AVGMass);
                    }
                }
            }

            for (int i = 0; i < lstScans.Count; i++)
            {
                int ScanNo = lstScans[i];
                if (Raw.GetMsLevel(ScanNo) == 1)
                {
                    CurrentScan = ScanNo;
                    int ProcessReport = Convert.ToInt32((i / (float)lstScans.Count) * 100);
                    //Console.WriteLine("Scan:" + ScanNo.ToString()+"\t Peptide:" + Peptide + "  completed");
                    bgWorker_Process.ReportProgress(ProcessReport);
                    this.lblStatus.SafeBeginInvoke(new Action(() => lblStatus.Text = "MS scan pass:" + ScanNo.ToString()));
                    continue;
                }
                if (!_SeqHCD && !Raw.IsCIDScan(ScanNo))
                {
                    CurrentScan = ScanNo;
                    int ProcessReport = Convert.ToInt32((i / (float)lstScans.Count) * 100);
                    //Console.WriteLine("Scan:" + ScanNo.ToString()+"\t Peptide:" + Peptide + "  completed");
                    bgWorker_Process.ReportProgress(ProcessReport);
                    this.lblStatus.SafeBeginInvoke(new Action(() => lblStatus.Text = "Not CID scan pass:" + ScanNo.ToString()));
                    continue;
                }
                lstGS.Clear();
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
                    Console.WriteLine("CID Scan No:" + ScanNo.ToString() + "\tHCD Scan No:" + HCDScanNo.ToString() + "\tGlycanType:" + HCD.GlycanType.ToString());
                }

                this.lblStatus.SafeBeginInvoke(new Action(() => lblStatus.Text = "Sequencing:" + ScanNo.ToString()));
                foreach (string Peptide in _Peptides)
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
                                    if (HCD != null && HCD.GlycanType == COL.MassLib.enumGlycanType.CA && ClosedGlycan.NoOfSia > 0)
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
                                if (_CompletedOnly && GS.FullSequencedStructures.Count == 0)
                                {
                                    continue;
                                }
                                lstGS.Add(GS);
                                CurrentScan = ScanNo;
                                CurrentPeptide = GS.PeptideSeq;
                                int ProcessReport = Convert.ToInt32((i / (float)lstScans.Count) * 100);
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
                            if (_CompletedOnly && GS.FullSequencedStructures.Count == 0)
                            {
                                continue;
                            }
                            lstGS.Add(GS);
                            CurrentScan = ScanNo;
                            CurrentPeptide = GS.PeptideSeq;
                            int ProcessReport = Convert.ToInt32((i / (float)lstScans.Count) * 100);
                            //Console.WriteLine("Scan:" + ScanNo.ToString()+"\t Peptide:" + Peptide + "  completed");
                            bgWorker_Process.ReportProgress(ProcessReport);
                        }

                    }//Foreach charge
                }//Foreach peptide
                if (lstGS.Count > 0)
                {
                    GenerateReportBody(lstGS);
                }
            }//Foreach Scan

        }
        private void bgWorker_Process_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            try
            {
                lblCurrentScan.Text = (CurrentScan + 1).ToString() + "    Peptide:" + CurrentPeptide;
                lblRunningTime.Text = DateTime.Now.Subtract(StartTime).TotalMinutes.ToString("0.00");
                progressBar1.Value = e.ProgressPercentage;
                lblPercentage.Text = e.ProgressPercentage.ToString() + "%";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private void ParseXMLFile(string argXMLFile)
        {
            lstScans = new List<int>();
            Console.Write("Parsing XML parameters");
            XDocument xdoc = XDocument.Load(argXMLFile);

            _Peptides = new List<string>();
            //Raw
            XElement xRaw = xdoc.XPathSelectElement("//GlycanSequencing/RawFile");
            foreach (XElement element in xRaw.Elements())
            {
                switch (element.Name.ToString())
                {
                    case "FilePath":
                        _rawFile = element.Value;
                        break;
                    //case "StartScan":
                    //     _StartScan = Convert.ToInt32(element.Value);
                    //      break;
                    //  case "EndScan":
                    //      _EndScan = Convert.ToInt32(element.Value);
                    //      break;
                    case "Scan":
                        lstScans.Add(Convert.ToInt32(element.Value));
                        break;
                    case "SequencingHCD":
                        _SeqHCD = Convert.ToBoolean(element.Value);
                        break;
                    case "UseHCDInfo":
                        _UseHCD = Convert.ToBoolean(element.Value);
                        break;
                }
            }
            Console.Write(".");
            //Fasta
            xRaw = xdoc.XPathSelectElement("//GlycanSequencing/FastaFile");
            foreach (XElement element in xRaw.Elements())
            {
                switch (element.Name.ToString())
                {
                    case "GlycoPeptide":
                        _Peptides.Add(element.Value);
                        break;
                }
            }
            Console.Write(".");
            //Glycan
            xRaw = xdoc.XPathSelectElement("//GlycanSequencing/Glycans");
            if (Convert.ToBoolean(xRaw.Attribute("GlycansFile").Value) == true)
            {
                _UseGlycanList = true;
                foreach (XElement element in xRaw.Elements())
                {
                    switch (element.Name.ToString())
                    {
                        case "FilePath":
                            _glycanFile = element.Value;
                            break;
                        case "NLink":
                            _NGlycan = Convert.ToBoolean(element.Value);
                            break;
                        case "Human":
                            _Human = Convert.ToBoolean(element.Value);
                            break;
                        case "AvgMass":
                            _AverageMass = Convert.ToBoolean(element.Value);
                            break;
                    }
                }
            }
            else
            {
                _UseGlycanList = false;
                foreach (XElement element in xRaw.Elements())
                {
                    switch (element.Name.ToString())
                    {
                        case "Hex":
                            _NoHex = Convert.ToInt32(element.Value);
                            break;
                        case "HexNAc":
                            _NoHexNAc = Convert.ToInt32(element.Value);
                            break;
                        case "deHex":
                            _NoDeHex = Convert.ToInt32(element.Value);
                            break;
                        case "Sia":
                            _NoSia = Convert.ToInt32(element.Value);
                            break;
                        case "NLink":
                            _NGlycan = Convert.ToBoolean(element.Value);
                            break;
                        case "Human":
                            _Human = Convert.ToBoolean(element.Value);
                            break;
                        case "AvgMass":
                            _AverageMass = Convert.ToBoolean(element.Value);
                            break;
                    }
                }
            }
            Console.Write(".");
            //Torelance
            xRaw = xdoc.XPathSelectElement("//GlycanSequencing/Torelance");
            foreach (XAttribute att in xRaw.Attributes())
            {
                switch (att.Name.ToString())
                {
                    case "MSMS_Da":
                        _MSMSTol = Convert.ToSingle(att.Value);
                        break;
                    case "Precursor_PPM":
                        _PrecursorTol = Convert.ToSingle(att.Value);
                        break;
                }
            }
            Console.Write(".");
            //Export
            xRaw = xdoc.XPathSelectElement("//GlycanSequencing/Export ");
            foreach (XAttribute att in xRaw.Attributes())
            {
                switch (att.Name.ToString())
                {
                    case "GetTopRank":
                        _GetTopRank = Convert.ToInt32(att.Value);
                        break;
                    case "OnlyCompleteStructure":
                        _CompletedOnly = Convert.ToBoolean(att.Value);
                        break;
                    case "CompletedReward":
                        _CompletedReward = Convert.ToSingle(att.Value);
                        break;
                }
            }
            Console.Write(".Completed\n");
        }

        private void bgWorker_Process_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //Export to tmp
            Environment.Exit(0);
        }
        private void GenerateReportBody(List<GlycanSequencing> argGSequencings)
        {

            StreamWriter sw = new StreamWriter(_exportFile, true);
            bool isHeaderPrint = false;


            foreach (GlycanSequencing GS in argGSequencings)
            {
                List<GlycanStructure> ExportStructure;
                if (_CompletedOnly)
                {
                    ExportStructure = GS.FullSequencedStructures;
                }
                else
                {
                    ExportStructure = GS.GetTopRankScoreStructre(_GetTopRank);
                }

                if (ExportStructure.Count > 0)
                {
                    if (!isHeaderPrint)
                    {
                        sw.WriteLine("<h1>Scan number:" + argGSequencings[0].ScanInfo.ScanNo.ToString() + "[Precursor m/z:" + argGSequencings[0].ScanInfo.ParentMZ.ToString("0.000") + "]</h1>");
                        sw.WriteLine("<Table border=\"1\">");
                        isHeaderPrint = true;
                    }
                    sw.WriteLine("<tr>");
                    sw.WriteLine("\t<td colspan=3>Peptide:" + GS.PeptideSeq + "</td></tr>");
                    sw.WriteLine("<tr>\n\t<td>Score</td>\n\t<td>Glycan  IUPAC</td>\n\t<td>IMG</td>\n</tr>");
                    foreach (GlycanStructure gs in ExportStructure)
                    {
                        string PicLocation = Path.GetDirectoryName(_exportFile) + "\\Pics\\" + gs.IUPACString.ToString() + ".png";
                        if (!File.Exists(PicLocation))
                        {
                            GlycansDrawer Draw = new GlycansDrawer(gs.IUPACString, false);
                            Image Pic = Draw.GetImage();

                            System.IO.MemoryStream mss = new System.IO.MemoryStream();
                            System.IO.FileStream fs = new System.IO.FileStream(PicLocation, System.IO.FileMode.Create, System.IO.FileAccess.ReadWrite);

                            Pic.Save(mss, System.Drawing.Imaging.ImageFormat.Png);
                            byte[] matriz = mss.ToArray();
                            fs.Write(matriz, 0, matriz.Length);

                            mss.Close();
                            fs.Close();
                            Pic.Dispose();
                            Pic = null;
                            Draw = null;
                        }
                        sw.WriteLine("<tr>\n\t<td>" + gs.Score.ToString("0.00") + "</td>\n\t<td>" + gs.IUPACString + "</td>\n\t<td><img src=\".\\Pics\\" + gs.IUPACString.ToString() + ".png\"/></td>\n</tr>");
                    }
                }
            }
            if (isHeaderPrint)
            {
                sw.WriteLine("</table>\n<br><br>");
            }
            sw.Flush();
            sw.Close();
        }
    }
}
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
