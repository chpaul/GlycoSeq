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
namespace GlycanSeq_Form
{
    public partial class frmInvokeProcesses : Form
    {        
        AminoAcidMass AAMW; 
        string _rawFilePath;
         DateTime StartTime;
        List<ProteinInfo> Proteins;
        Thread WorkThread;
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
        string _exportFile;
        string _fastaFile;
        string _glycanFile;
        int _GetTopRank;
        bool _UseHCD;
        bool _SeqHCD;
        bool _CompletedOnly;
        float _CompletedReward;
        int DividePartNum = 0;
        bool _IsFinishWorker = false;
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
        public frmInvokeProcesses(int argStartScan,
                                              int argEndScan,
                                              float argMSMSTol,
                                              float argPrecursorTol,
                                              bool argNGlycan,
                                              bool argHuman,
                                              List<GlycanCompound> argGlycanCompounds,
                                              Dictionary<double, GlycanCompound> argMassGlycanMapping,
                                              List<float> argGlycanCompoundMassList,
                                              string argGlycanFile,
                                              string argRawFilePath,
                                              string argFastaFile,
                                              List<Protease.Type> argProteaseType,
                                              int argMissCleavage,
                                              bool argAverageMass,
                                              bool argUseHCD,
                                              bool argSeqHCD,
                                              string argExportFile,
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
            _glycanFile = argGlycanFile;
            _rawFilePath = argRawFilePath;
            Proteins = FastaReader.ReadFasta(argFastaFile);
            _fastaFile = argFastaFile;
            _ProteaseType = argProteaseType;
            _MissCLeavage = argMissCleavage;
            _AverageMass = argAverageMass;
            _UseGlycanList = true;
            _exportFile = argExportFile;
            _GetTopRank = argGetTopRank;
            _UseHCD = argUseHCD;
            _SeqHCD = argSeqHCD;
            _CompletedOnly = argCompletedOnly;
            _CompletedReward = argCompletedReward;
            WorkThread = new Thread(RunWork);
            WorkThread.Start();
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
        public frmInvokeProcesses(int argStartScan,
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
                                     string argExportFile,
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
            _NoHexNAc = argNoHexNAc;
            _NoHex = argNoHex;
            _NoDeHex = argNoDeHex;
            _NoSia = argNoSia;
            _rawFilePath = argRawFilePath;
            Proteins = FastaReader.ReadFasta(argFastaFile);
            _fastaFile = argFastaFile;
            _ProteaseType = argProteaseType;
            _MissCLeavage = argMissCleavage;
            _AverageMass = argAverageMass;
            _UseGlycanList = false;
            _UseHCD = argUseHCD;
            _SeqHCD = argSeqHCD;
            _CompletedOnly = argCompletedOnly;
            _CompletedReward = argCompletedReward;
            _exportFile = argExportFile;
            _GetTopRank = argGetTopRank;
            WorkThread = new Thread(RunWork);
            WorkThread.Start();
        }
        private void RunWork()
        {
            bgWorker.RunWorkerAsync();
            //bgWorker_DoWork(this,new DoWorkEventArgs(this));
            _IsFinishWorker = false;
            while (!_IsFinishWorker)
            {
                //UpdateTime
                Thread.Sleep(500);
                this.lblRunningTime.SafeBeginInvoke(new Action(() => lblRunningTime.Text = DateTime.Now.Subtract(StartTime).TotalMinutes.ToString("0.00")));        
            }
            //Merge Result
            MergeReport();
            MessageBox.Show("Finish");
            this.SafeBeginInvoke(new Action(() => this.Close()));
        }
        private void bgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            StartTime = new DateTime(DateTime.Now.Ticks);
            WriteParametersToXML();         
            Process proc=null;
            try
            {
                for (int i = 1; i <= DividePartNum; i++)
                {
                    lblCurrentSubProcesses.SafeBeginInvoke(new Action(()=> lblCurrentSubProcesses.Text = i.ToString() +"/" + DividePartNum.ToString()));
                    lblStatus.SafeBeginInvoke(new Action(()=> lblStatus.Text = "Processing :" + Path.GetFileNameWithoutExtension(_exportFile) + "_" + (i).ToString() + ".xml"));
                    proc = new Process();
                    string TempEXE = @"D:\!Git\GlycanSequencing\src\GlycanSeq_Runner\bin\Debug\GlycanSeq_Runner.exe";
                    proc.StartInfo.FileName = TempEXE;
                    proc.StartInfo.Arguments = Path.GetDirectoryName(_exportFile) + "\\" + Path.GetFileNameWithoutExtension(_exportFile) + "_" + (i).ToString() + ".xml";
                    proc.Start();
                    proc.WaitForExit();
                }

            }
            catch (Exception ex)
            {
                StreamWriter sw = new StreamWriter(Path.GetDirectoryName(_exportFile) + "\\" + Path.GetFileNameWithoutExtension(_exportFile) + "_error.txt");
                sw.WriteLine(ex.ToString());
                sw.WriteLine();
                sw.Close();
                proc.Kill();
            }

        }
        private List<string> GenerateGlycoPeptide()
        {
            List<ProteinInfo> PInfos = FastaReader.ReadFasta(_fastaFile);
            List<string> GlycoPeptide = new List<string>();
            foreach (ProteinInfo Prot in PInfos)
            {
                GlycoPeptide.AddRange(Prot.NGlycopeptide(_MissCLeavage, _ProteaseType));
            }
            return GlycoPeptide;
        }
        private void WriteParametersToXML()
        {
            List<int> EndScans = new List<int>();
            int PartStart = _StartScan;
            int PartEndScan = 0;            
            int DivideInterval = 1000;
            while (true)
            {
                PartEndScan = PartStart + DivideInterval-1;
                if (PartEndScan >= _EndScan)
                {
                    PartEndScan = _EndScan;
                    EndScans.Add(PartEndScan);
                    break;
                }
                else
                {
                    EndScans.Add(PartEndScan);
                    PartStart = PartEndScan + 1;
                }
            }
            DividePartNum = EndScans.Count;
           for(int i=0;i<EndScans.Count;i++)
            {
               
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.IndentChars = "  ";
                settings.NewLineChars = "\r\n";
                settings.NewLineHandling = NewLineHandling.Replace;
                XmlWriter writer = XmlWriter.Create(Path.GetDirectoryName(_exportFile) + "\\" + Path.GetFileNameWithoutExtension(_exportFile) + "_" + (i + 1).ToString() + ".xml", settings);                
                writer.WriteStartDocument();
                writer.WriteStartElement("GlycanSequencing");
               //Raw
                    writer.WriteStartElement("RawFile");
                        writer.WriteElementString("FilePath",_rawFilePath);
                        //writer.WriteElementString("StartScan",(EndScans[i] - DivideInterval+1).ToString());
                        //writer.WriteElementString("EndScan", EndScans[i].ToString());
                        writer.WriteElementString("SequencingHCD", _SeqHCD.ToString());
                        writer.WriteElementString("UseHCDInfo", _UseHCD.ToString());
                        for(int j =EndScans[i] - DivideInterval+1;j<=EndScans[i];j++)
                        {
                            writer.WriteElementString("Scan", j.ToString());
                        }
                    writer.WriteEndElement();
               //Fasta
                    writer.WriteStartElement("FastaFile");
                        //writer.WriteElementString("FilePath", _fastaFile);
                        foreach (string glycopeptide in GenerateGlycoPeptide())
                        {
                            writer.WriteElementString("GlycoPeptide", glycopeptide);
                        }
                    writer.WriteEndElement();
               //Glycan
                    writer.WriteStartElement("Glycans");
                    writer.WriteAttributeString("GlycansFile",_UseGlycanList.ToString());
                    if (_UseGlycanList)
                    {
                        writer.WriteElementString("FilePath", _glycanFile);
                    }
                    else
                    {
                        writer.WriteElementString("Hex", _NoHex.ToString());
                        writer.WriteElementString("HexNAc", _NoHexNAc.ToString());
                        writer.WriteElementString("deHex", _NoDeHex.ToString());
                        writer.WriteElementString("Sia", _NoSia.ToString());
                    }
                    writer.WriteElementString("NLink",_NGlycan.ToString());
                    writer.WriteElementString("Human", _Human.ToString());
                    writer.WriteElementString("AvgMass", _AverageMass.ToString());
                    writer.WriteEndElement();
               //Torelance
                    writer.WriteStartElement("Torelance");
                    writer.WriteAttributeString("MSMS_Da", _MSMSTol.ToString());
                    writer.WriteAttributeString("Precursor_PPM", _PrecursorTol.ToString());
                    writer.WriteEndElement();

               //Export
                    writer.WriteStartElement("Export");
                    writer.WriteAttributeString("GetTopRank", _GetTopRank.ToString());
                    writer.WriteAttributeString("OnlyCompleteStructure", _CompletedOnly.ToString());
                    writer.WriteAttributeString("CompletedReward", _CompletedReward.ToString());
                    writer.WriteEndElement();

                   writer.WriteEndElement();
                writer.WriteEndDocument();
                writer.Flush();
                writer.Close();
            }
        }
        
        
        private void MergeReport()
        {
            StreamWriter sw = new StreamWriter(_exportFile);
            GenerateReportHeader(sw);
            for (int i = 1; i <= DividePartNum; i++)
            {
                if (File.Exists(Path.GetDirectoryName(_exportFile) + "\\" + Path.GetFileNameWithoutExtension(_exportFile) + "_" + (i).ToString() + ".xml_tmp"))
                {
                    //
                    StreamReader sr = new StreamReader(Path.GetDirectoryName(_exportFile) + "\\" + Path.GetFileNameWithoutExtension(_exportFile) + "_" + (i).ToString() + ".xml_tmp");
                    sw.Write(sr.ReadToEnd());
                    sr.Close();
                    File.Delete(Path.GetDirectoryName(_exportFile) + "\\" + Path.GetFileNameWithoutExtension(_exportFile) + "_" + (i).ToString() + ".xml_tmp");
                }
                if (File.Exists(Path.GetDirectoryName(_exportFile) + "\\" + Path.GetFileNameWithoutExtension(_exportFile) + "_" + (i).ToString() + ".xml"))
                {
                    //Delete xml Parameters
                    File.Delete(Path.GetDirectoryName(_exportFile) + "\\" + Path.GetFileNameWithoutExtension(_exportFile) + "_" + (i).ToString() + ".xml");
                }
            }
            GenerateReportFooter(sw);
            sw.Close();
        }
 
   
    
        private void GenerateReportHeader(StreamWriter argSW)
        {

            argSW.WriteLine("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">");
            argSW.WriteLine("<html xmlns=\"http://www.w3.org/1999/xhtml\">");
            argSW.WriteLine("<head>");
            argSW.WriteLine("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />");
            argSW.WriteLine("<title>Glycoseq result for raw " + Path.GetFileName(_rawFilePath) + "</title>\n</head>\n<body>");
            argSW.WriteLine("<Table border=\"1\">");

            argSW.WriteLine("<tr>\n\t<td>Raw File: </td>\n\t<td>" + _rawFilePath + "</td>\n</tr>");
            argSW.WriteLine("<tr>\n\t<td>Scan Range: </td>\n\t<td>" + _StartScan.ToString() + "~" + _EndScan.ToString() + "</td>\n</tr>");
            argSW.WriteLine("<tr>\n\t<td>Fasta File: </td>\n\t<td>" + _fastaFile + "</td>\n</tr>");
            string tmp = "";
            foreach (Protease.Type p in _ProteaseType)
            {
                tmp = tmp + p.ToString() + ",";
            }
            argSW.WriteLine("<tr>\n\t<td>ProteaseType: </td>\n\t<td>" + tmp + "</td>\n</tr>");
            argSW.WriteLine("<tr>\n\t<td>MissCleavage: </td>\n\t<td>" + _MissCLeavage.ToString() + "</td>\n</tr>");
            if (_UseGlycanList)
            {
                argSW.WriteLine("<tr>\n\t<td>Glycan File: </td>\n\t<td>" + _glycanFile + "</td>\n</tr>");
            }
            else
            {
                tmp = "HexNAc:" + _NoHexNAc.ToString() + ";" +
                            "Hex:" + _NoHex.ToString() + ";" +
                            "Sia" +_NoSia.ToString()+";"+
                            "DeHex:" + _NoDeHex.ToString() + ";";
                argSW.WriteLine("<tr>\n\t<td>Glycan Composition: </td>\n\t<td>" + tmp + "</td>\n</tr>");
            }
            argSW.WriteLine("<tr>\n\t<td>MS Tol/MS2 Tol: </td>\n\t<td>" + _PrecursorTol.ToString() + "/" + _MSMSTol.ToString() + "</td>\n</tr>");
            argSW.WriteLine("<tr>\n\t<td>N-Glycan/Human: </td>\n\t<td>" + _NGlycan.ToString() + "/" + _Human.ToString() + "</td>\n</tr>");
            argSW.WriteLine("<tr>\n\t<td>Use HCD/Sequencing HCD: </td>\n\t<td>" + _UseHCD.ToString() + "/" + _SeqHCD.ToString() + "</td>\n</tr>");
            argSW.WriteLine("<tr>\n\t<td>Export Completed Only/Completed Reward: </td>\n\t<td>" + _CompletedOnly.ToString() + "/" + _CompletedReward.ToString() + "</td>\n</tr>");
            argSW.WriteLine("</Table>\n<br>\n<br>");
            argSW.Flush();
        }
        private void GenerateReportFooter(StreamWriter argSW)
        {
            argSW.WriteLine("<br></body>\n</html>");
        }
        private void GenerateReportBody(List<GlycanSequencing> argGSequencing, StreamWriter argSW)
        {
            
                argSW.WriteLine("<h1>Scan number:" + argGSequencing[0].ScanInfo.ScanNo.ToString() + "[Precursor m/z:" + argGSequencing[0].ScanInfo.ParentMZ.ToString("0.000") + "]</h1>");
                argSW.WriteLine("<Table border=\"1\">");
                
                foreach (GlycanSequencing GS in argGSequencing)
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

                    argSW.WriteLine("<tr>");
                    argSW.WriteLine("\t<td colspan=3>Peptide:" + GS.PeptideSeq + "</td></tr>");
                    argSW.WriteLine("<tr>\n\t<td>Score</td>\n\t<td>Glycan  IUPAC</td>\n\t<td>IMG</td>\n</tr>");
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
                        argSW.WriteLine("<tr>\n\t<td>" + gs.Score.ToString("0.00") + "</td>\n\t<td>" + gs.IUPACString + "</td>\n\t<td><img src=\".\\Pics\\" + gs.IUPACString.ToString() + ".png\"/></td>\n</tr>");
                    }                    
                }
                argSW.WriteLine("</table>\n<br><br>");
                argSW.Flush();
            
        }
        private void frmProcessing_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_IsFinishWorker)
            {
                if (MessageBox.Show("Cancel all process?", "Cancel", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                {
                    this.Dispose();
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }

        private void bgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _IsFinishWorker = true;
        }


        
    }
}//class
//public static class ControlExtensions
//{
//    public static void SafeInvoke(this Control control, Action action)
//    {
//        if (control.InvokeRequired)
//        {
//            control.Invoke(action);
//        }
//        else
//        {
//            action();
//        }
//    }
//    public static void SafeBeginInvoke(this Control control, Action action)
//    {
//        if (control.InvokeRequired)
//        {
//            control.BeginInvoke(action);
//        }
//        else
//        {
//            action();
//        }
//    }
//}


 