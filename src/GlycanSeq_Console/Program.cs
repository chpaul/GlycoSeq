using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Drawing;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using COL.GlycoSequence;
using COL.MassLib;
using COL.ProtLib;
using COL.GlycoLib;
namespace GlycanSeq_Console
{
    class Program
    {
        
        static int _StartScan, _EndScan, completedScan=0;
        static bool _UseHCD, _NGlycan, _Human, _CompletedOnly, _SeqHCD, _AverageMass,_UseGlycanList;
        static ThermoRawReader Raw;
        static string _rawFile;
        static string _exportFile;      
        static string _glycanFile;
        static List<float> _GlycanCompoundMassList;
        static List<GlycanCompound> _GlycanCompounds;
        static Dictionary<double, GlycanCompound> _MassGlycanMapping;

        static int _NoHexNAc;
        static int _NoHex;
        static int _NoDeHex;
        static int _NoSia;
        static float _MSMSTol = 0.0f;
        static float _PrecursorTol = 0.0f;
        static List<string> _Peptides;
        static int _GetTopRank;
        static float _CompletedReward;
        static DateTime StartTime;
        static List<int> lstScans;
        static object lockobject = new object();

        static BackgroundWorker bgWorker_Process;
        static AutoResetEvent _resetEvent;
        static void Main(string[] args)
        {
            try
            {
                lstScans = new List<int>();
                StartTime = DateTime.Now;
                Console.WriteLine("Parameters File:" + args[0]);
                ParseXMLFile(args[0]);
                _exportFile = args[0] + "_tmp";
                //StreamWriter sw =new StreamWriter(_exportFile);
                
                bgWorker_Process = new System.ComponentModel.BackgroundWorker();        
                bgWorker_Process.WorkerReportsProgress = true;
                bgWorker_Process.DoWork += new System.ComponentModel.DoWorkEventHandler(bgWorker_Process_DoWork);
                bgWorker_Process.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(bgWorker_Process_ProgressChanged);
                bgWorker_Process.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(bgWorker_Process_RunWorkerCompleted);
                _resetEvent = new AutoResetEvent(false);
                PrepareSequencing();


                Console.WriteLine("Completed in " + DateTime.Now.Subtract(StartTime).TotalMinutes.ToString("0.00") + " m");
                //Console.ReadLine();
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
        }
        private static void ParseXMLFile(string argXMLFile)
        {
            Console.Write("Parsing XML parameters");
            XDocument xdoc = XDocument.Load(argXMLFile);

            _Peptides = new List<string>();
            //Raw
            XElement xRaw =  xdoc.XPathSelectElement("//GlycanSequencing/RawFile");
            foreach(XElement element in xRaw.Elements())
            {
                switch(element.Name.ToString())
                {
                    case "FilePath":
                        _rawFile=element.Value;
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
                             _AverageMass= Convert.ToBoolean(element.Value);
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
                switch(att.Name.ToString())
                {
                    case "MSMS_Da":
                        _MSMSTol = Convert.ToSingle(att.Value);
                        break;
                    case "Precursor_PPM":
                        _MSMSTol = Convert.ToSingle(att.Value);
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
                         _CompletedOnly= Convert.ToBoolean(att.Value);
                        break;
                    case "CompletedReward":
                        _CompletedReward = Convert.ToSingle(att.Value);
                        break;
                }
            }
            Console.Write(".Completed\n");
        }
        private static void PrepareSequencing()
        {
            Console.Write("Init Raw");
            Raw = new ThermoRawReader(_rawFile);
            Console.WriteLine(".....Completed\n");
            //sw = new StreamWriter(_exportFile);         
            //ReadGlycanFile
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
            //Sequencing();
            bgWorker_Process.RunWorkerAsync();
            _resetEvent.WaitOne();
            //for (int ScanNo = _StartScan; ScanNo <= _EndScan; ScanNo++)
            //List<Thread> lstThread = new List<Thread>();
            //int MaxConcurrentThread = 3;
            /*List<Task> allTasks = new List<Task>();
            for(int i =0;i<lstScans.Count;i++)
            {
                if (Raw.GetMsLevel(lstScans[i]) != 2)
                {
                    completedScan = completedScan + 1;
                    float Precentage = Convert.ToSingle((i / Convert.ToSingle(lstScans.Count)) * 100.0);
                    Console.WriteLine("Completed Scan:" + lstScans[i].ToString() + "," + Precentage.ToString("0.00") + "%," + DateTime.Now.Subtract(StartTime).TotalSeconds.ToString("0.00") + " s");
                    continue;
                }
                MSScan _scan = Raw.ReadScan(lstScans[i]);
                HCDInfo HCD = null;
                if (_UseHCD)
                {
                    int CheckScanNO = lstScans[i];
                    
                    //string ScanHeader = _scan.ScanHeader.Substring(_scan.ScanHeader.IndexOf("ms2")+4, _scan.ScanHeader.IndexOf("@") - _scan.ScanHeader.IndexOf("ms2")-3) + "hcd";
                    do
                    {
                        CheckScanNO++;
                        if (Raw.IsHCDScan(CheckScanNO))
                        {
                            lock (lockobject)
                            {
                                HCD = Raw.GetHCDInfo(CheckScanNO);
                                break;
                            }
                        }

                    } while (Raw.GetMsLevel(CheckScanNO) != 1); //Check Until hit Next Full MS

                    //CA: Complex Asialyated, CS:Complex Sialylated, HM:High mannose, HY:Hybrid and NA

                }
                //do
                //{
                //    if (lstThread.Count < MaxConcurrentThread)
                //    {
                //        Thread t = new Thread(() => Sequencing(i));
                //        t.Start();
                //        lstThread.Add(t);
                //        break;
                //    }
                //} while (true);
                allTasks.Add(Task.Factory.StartNew(() => Sequencing()));

               // Sequencing(i);
                
            }//Foreach Scan          
            Task.WaitAll(allTasks.ToArray());*/
        }
        private static void SequencingOld(MSScan argScan,HCDInfo argHCD)
        {


            MSScan _scan = argScan;
            AminoAcidMass AAMW = new AminoAcidMass();
            List<GlycanSequencing> lstGS = new List<GlycanSequencing>();
            HCDInfo HCD = null;
            int ScanNo = argScan.ScanNo;
            int PrecursorCharge = argScan.ParentCharge;
            try
            {

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
                            List<GlycanCompound> ClosedGlycans = new List<GlycanCompound>();
                            foreach (float gMass in _GlycanCompoundMassList)
                            {
                                if (Math.Abs(gMass - GlycanMonoMass) < 100.0f)
                                {
                                    ClosedGlycans.Add(_GlycanCompounds[MassUtility.GetClosestMassIdx(_GlycanCompoundMassList, gMass)]);
                                }
                            }
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
                                GS.RewardForCompleteStructure = _CompletedReward;
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
                            GS.RewardForCompleteStructure = _CompletedReward;
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
                        }

                    }//Foreach charge                     
                }//Foreach peptide

            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
            finally
            {
                _scan.Dispose();
                completedScan = completedScan + 1;
                float Precentage = Convert.ToSingle((completedScan / Convert.ToSingle(lstScans.Count)) * 100.0);
                Console.WriteLine("Completed Scan:" + ScanNo.ToString() + "," + Precentage.ToString("0.00") + "%," + DateTime.Now.Subtract(StartTime).TotalSeconds.ToString("0.00") + " s");
                if (lstGS.Count > 0)
                {
                    GenerateReportBody(lstGS);
                }
            }
        }
        private static void GenerateReportBody(List<GlycanSequencing> argGSequencings)
        {
            lock (lockobject)
            {
                StreamWriter sw = new StreamWriter(_exportFile, true);
                sw.WriteLine("<h1>Scan number:" + argGSequencings[0].ScanInfo.ScanNo.ToString() + "[Precursor m/z:" + argGSequencings[0].ScanInfo.ParentMZ.ToString("0.000") + "]</h1>");
                sw.WriteLine("<Table border=\"1\">");

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
                sw.WriteLine("</table>\n<br><br>");
                sw.Flush();
                sw.Close();
            }
        }
        private static void Sequencing()
        {
            int ScanNo = 0;
            List<GlycanSequencing> lstGS = new List<GlycanSequencing>();
            float WorkingPrecentage = 0.0f;
            try
            {
                for (int i = 0; i < lstScans.Count; i++)
                {
                    ScanNo = lstScans[i];
                    if (Raw.GetMsLevel(ScanNo) == 1)
                    {
                        completedScan = completedScan + 1;
                        WorkingPrecentage = Convert.ToSingle((completedScan / Convert.ToSingle(lstScans.Count)) * 100.0);
                        Console.WriteLine("Skip MS Scan:" + ScanNo.ToString() + "," + WorkingPrecentage.ToString("0.00") + "%," + DateTime.Now.Subtract(StartTime).TotalSeconds.ToString("0.00") + " s");       
                        continue;
                    }
                    if (!_SeqHCD && !Raw.IsCIDScan(ScanNo))
                    {
                        completedScan = completedScan + 1;
                        WorkingPrecentage = Convert.ToSingle((completedScan / Convert.ToSingle(lstScans.Count)) * 100.0);
                        Console.WriteLine("HCD Scan:" + ScanNo.ToString() + "," + WorkingPrecentage.ToString("0.00") + "%," + DateTime.Now.Subtract(StartTime).TotalSeconds.ToString("0.00") + " s");
                        continue;
                    }
                    MSScan _scan = Raw.ReadScan(ScanNo);
                    AminoAcidMass AAMW = new AminoAcidMass();
                    HCDInfo HCD = null;
                    int PrecursorCharge = _scan.ParentCharge;                    
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
                            }

                        }//Foreach charge
                    }//Foreach peptide
                    completedScan = completedScan + 1;
                    WorkingPrecentage = Convert.ToSingle((completedScan / Convert.ToSingle(lstScans.Count)) * 100.0);
                    Console.WriteLine("Completed Scan:" + ScanNo.ToString() + "," + WorkingPrecentage.ToString("0.00") + "%," + DateTime.Now.Subtract(StartTime).TotalSeconds.ToString("0.00") + " s");       
                }//Foreach Scan
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
            finally
            {              
               
                Console.WriteLine("Completed Scans start export. "+ DateTime.Now.Subtract(StartTime).TotalSeconds.ToString("0.00") + " s");
                if (lstGS.Count > 0)
                {
                    GenerateReportBody(lstGS);
                }
            }
        }

        private static void bgWorker_Process_DoWork(object sender, DoWorkEventArgs e)
        {
            int ScanNo = 0;
            List<GlycanSequencing> lstGS = new List<GlycanSequencing>();
            float WorkingPrecentage = 0.0f;
            for (int i = 0; i < lstScans.Count; i++)
            {
                ScanNo = lstScans[i];
                if (Raw.GetMsLevel(ScanNo) == 1)
                {
                    completedScan = completedScan + 1;
                    WorkingPrecentage = Convert.ToSingle((completedScan / Convert.ToSingle(lstScans.Count)) * 100.0);
                    Console.WriteLine("Skip MS Scan:" + ScanNo.ToString() + "," + WorkingPrecentage.ToString("0.00") + "%," + DateTime.Now.Subtract(StartTime).TotalSeconds.ToString("0.00") + " s");
                    continue;
                }
                if (!_SeqHCD && !Raw.IsCIDScan(ScanNo))
                {
                    completedScan = completedScan + 1;
                    WorkingPrecentage = Convert.ToSingle((completedScan / Convert.ToSingle(lstScans.Count)) * 100.0);
                    Console.WriteLine("HCD Scan:" + ScanNo.ToString() + "," + WorkingPrecentage.ToString("0.00") + "%," + DateTime.Now.Subtract(StartTime).TotalSeconds.ToString("0.00") + " s");
                    continue;
                }
                MSScan _scan = Raw.ReadScan(ScanNo);
                AminoAcidMass AAMW = new AminoAcidMass();
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
                            }

                        }//Foreach charge
                    }//Foreach peptide
                    completedScan = completedScan + 1;
                    WorkingPrecentage = Convert.ToSingle((completedScan / Convert.ToSingle(lstScans.Count)) * 100.0);
                    Console.WriteLine("Completed Scan:" + ScanNo.ToString() + "," + WorkingPrecentage.ToString("0.00") + "%," + DateTime.Now.Subtract(StartTime).TotalSeconds.ToString("0.00") + " s");      
            }//Foreach Scan
            _resetEvent.Set();
        }
        private static void bgWorker_Process_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
 
        }

        private static void bgWorker_Process_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            

        }
    }
}
