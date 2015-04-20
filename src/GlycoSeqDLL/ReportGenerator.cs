using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COL.GlycoLib;
using COL.MassLib;
using System.Drawing;
using System.IO;
namespace COL.GlycoSequence
{
    public class ReportGenerator
    {
       /// <summary>
       /// 
       /// </summary>
       /// <param name="argFolder"></param>
       /// <param name="argScan"></param>
       /// <param name="argExportIndividualReport"></param>
       /// <param name="argTopRank"></param>
       /// <param name="argStructureResults"></param>
        public static void CSVFormat(string argFolder, MSScan argScan, int argTopRank, List<Tuple<string, GlycanStructure, double>> argStructureResults)
        {
            bool hasFile = false;
            if (File.Exists(argFolder + "\\Result.csv"))
            {
                hasFile = true;
            }
            StreamWriter sw = new StreamWriter(argFolder + "\\Result.csv",  true);
            //Title
            if (!hasFile)
            {
                sw.WriteLine(
                    "MSn_Scan,Parent_Scan,Parent_Mz,Charge_State,CID_time,Peptide,Sequencing_Score,Completed_Sequencing,HexNAc,Hex,Fuc,NeuAc,NeuGc,IUPAC");
            }
            string header = argScan.ScanNo.ToString() + "," + argScan.ParentScanNo.ToString() + "," +
                            argScan.ParentMZ.ToString() + "," + argScan.ParentCharge.ToString() + "," +
                            argScan.Time.ToString("0.00")+",";
           argStructureResults = argStructureResults.OrderByDescending(x => x.Item2.Score).ToList();

          // List<string> lstExportedResult = new List<string>();
           double previousScore = 0;
           int ExportedRank = 0;
            foreach (Tuple<string, GlycanStructure, double> SeqResult in argStructureResults)
           {
               //string key = SeqResult.Item1 + "-" + SeqResult.Item2.NoOfHexNac+"-"+SeqResult.Item2.NoOfHex+"-"+SeqResult.Item2.NoOfDeHex+"-"+SeqResult.Item2.NoOfNeuAc+"-"+SeqResult.Item2.NoOfNeuGc;
               //if (lstExportedResult.Contains(key))
               //{
               //    continue;
               //}
               if (previousScore != Convert.ToSingle(SeqResult.Item3.ToString("0.00")))
               {
                   ExportedRank = ExportedRank + 1;
                   previousScore = Convert.ToSingle(SeqResult.Item3.ToString("0.00"));
                   if (ExportedRank > argTopRank)
                   {
                       break;
                   }
               }
              // lstExportedResult.Add(key);


               sw.WriteLine(header + SeqResult.Item1.ToString() + "," +
                            SeqResult.Item2.Score.ToString("0.00")+","+
                            SeqResult.Item2.IsCompleteSequenced.ToString()+ "," +
                            SeqResult.Item2.NoOfHexNac.ToString() + "," +
                            SeqResult.Item2.NoOfHex.ToString() + "," +
                            SeqResult.Item2.NoOfDeHex.ToString() + "," +
                            SeqResult.Item2.NoOfNeuAc.ToString() + "," +
                            SeqResult.Item2.NoOfNeuGc.ToString() + "," +
                            SeqResult.Item2.IUPACString);

           }
            sw.Flush();
            sw.Close();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="argExportFolder"></param>
        /// <param name="argResults">Key: Peptide Seq //Tuple 1:Scan Number, 2:Glycan Sequence 3:Score 4:Completed</param>
        public static void GenerateHtmlReportSortByPeptide(string argFolder, Dictionary<string, List<Tuple<int, string, double, bool>>> argResults, COL.GlycoSequence.GlycanSeqParameters argGlycanSeqParas)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">");
            sb.AppendLine("<html xmlns=\"http://www.w3.org/1999/xhtml\">");
            sb.AppendLine("<head>");
            sb.AppendLine("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />");
            sb.AppendLine("<title>Glycoseq result for raw " + Path.GetFileName(argGlycanSeqParas.RawFilePath) +
                          "</title>\n</head>\n<body>");
            sb.AppendLine("<Table border=\"1\">");

            sb.AppendLine("<tr>\n\t<td>Raw File: </td>\n\t<td>" + argGlycanSeqParas.RawFilePath + "</td>\n</tr>");
            sb.AppendLine("<tr>\n\t<td>Peptide File: </td>\n\t<td>" + argGlycanSeqParas.FastaFile + "</td>\n</tr>");
            sb.AppendLine("<tr>\n\t<td>Scan Range: </td>\n\t<td>" + argGlycanSeqParas.StartScan.ToString() + "~" + argGlycanSeqParas.EndScan.ToString() +
                            "</td>\n</tr>");
            //argSW.WriteLine("<tr>\n\t<td>Fasta File: </td>\n\t<td>" + _fastaFile + "</td>\n</tr>");
            string tmp = "";
            //foreach (Protease.Type p in _ProteaseType)
            //{
            //    tmp = tmp + p.ToString() + ",";
            //}
            //argSW.WriteLine("<tr>\n\t<td>ProteaseType: </td>\n\t<td>" + tmp + "</td>\n</tr>");
            //argSW.WriteLine("<tr>\n\t<td>MissCleavage: </td>\n\t<td>" + _MissCLeavage.ToString() + "</td>\n</tr>");
            if (argGlycanSeqParas.UseGlycanList)
            {
                sb.AppendLine("<tr>\n\t<td>Glycan File: </td>\n\t<td>" + argGlycanSeqParas.GlycanFile + "</td>\n</tr>");
            }
            else
            {
                tmp = "HexNAc:" + argGlycanSeqParas.NoHexNAc.ToString() + ";" +
                      "Hex:" + argGlycanSeqParas.NoHex.ToString() + ";" +
                      "Sia" + argGlycanSeqParas.NoSia.ToString() + ";" +
                      "DeHex:" + argGlycanSeqParas.NoDeHex.ToString() + ";";
                sb.AppendLine("<tr>\n\t<td>Glycan Composition: </td>\n\t<td>" + tmp + "</td>\n</tr>");
            }
            sb.AppendLine("<tr>\n\t<td>MS tolerance(PPM)/MS2 tolerance(Da: </td>\n\t<td>" + argGlycanSeqParas.PrecursorTol.ToString() + "/" +
                            argGlycanSeqParas.MSMSTol.ToString() + "</td>\n</tr>");
            sb.AppendLine("<tr>\n\t<td>N-Glycan/Human: </td>\n\t<td>" + argGlycanSeqParas.IsNGlycan.ToString() + "/" + argGlycanSeqParas.IsHuman.ToString() +
                            "</td>\n</tr>");
            sb.AppendLine("<tr>\n\t<td>Use HCD/Sequencing HCD: </td>\n\t<td>" + argGlycanSeqParas.UseHCD.ToString() + "/" +
                            argGlycanSeqParas.SeqHCD.ToString() + "</td>\n</tr>");
            sb.AppendLine("<tr>\n\t<td>Export Completed Only/Completed Reward: </td>\n\t<td>" +
                            argGlycanSeqParas.CompletedOnly.ToString() + "/" + argGlycanSeqParas.CompletedReward.ToString() + "</td>\n</tr>");
            sb.AppendLine("<tr>\n\t<td>Peptide Mutation: </td>\n\t<td>" +
                             argGlycanSeqParas.PeptideMutation.ToString() + "</td>\n</tr>");
            sb.AppendLine("</Table>\n<br>\n<br>");

            foreach (string pepStr in argResults.Keys)
            {
                sb.AppendLine("Peptide:" + pepStr);
                sb.AppendLine("<table  border=\"1\">");
                sb.AppendLine("<tr><td>Scan No</td><td>Glycan Sequence</td><td>Glycan Picture</td><td>Score</td><td>Completed Sequence?</td></tr>");
                foreach (Tuple<int, string, double, bool> rResult in argResults[pepStr])
                {
                    sb.AppendLine("<tr>");
                    sb.AppendLine("<td>" + rResult.Item1 + "</td>");
                    sb.AppendLine("<td>" + rResult.Item2 + "</td>");
                    sb.AppendLine("<td><img src=\".\\Pics\\" + rResult.Item2 + ".png\"/></td>");
                    sb.AppendLine("<td>" + rResult.Item3.ToString("0.0000") + "</td>");
                    if (rResult.Item4)
                    {
                        sb.AppendLine("<td>Yes</td>");
                    }
                    else
                    {
                        sb.AppendLine("<td>No</td>");
                    }

                    sb.AppendLine("</tr>");
                }
                sb.AppendLine("</table><br>-----------------------------------------------------------<br>");
            }
            sb.AppendLine("</body>\n</html>");
            StreamWriter sw = new StreamWriter(argFolder + "\\Result_SortByPeptides.html");
            sw.WriteLine(sb.ToString());
            sw.Flush();
            sw.Close();
        }

        public static void GenerateHtmlReportSortByScan(COL.GlycoSequence.GlycanSeqParameters argGlycanSeqParas)
        {

            StreamWriter argSW = new StreamWriter(argGlycanSeqParas.ExportFolder + "\\Result_SortByScans.html");
            try
            {
                argSW.WriteLine("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">");
                argSW.WriteLine("<html xmlns=\"http://www.w3.org/1999/xhtml\">");
                argSW.WriteLine("<head>");
                argSW.WriteLine("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />");
                argSW.WriteLine("<title>Glycoseq result for raw " + Path.GetFileName(argGlycanSeqParas.RawFilePath) +
                                "</title>\n</head>\n<body>");
                argSW.WriteLine("<Table border=\"1\">");

                argSW.WriteLine("<tr>\n\t<td>Raw File: </td>\n\t<td>" + argGlycanSeqParas.RawFilePath + "</td>\n</tr>");
                argSW.WriteLine("<tr>\n\t<td>Scan Range: </td>\n\t<td>" + argGlycanSeqParas.StartScan.ToString() + "~" + argGlycanSeqParas.EndScan.ToString() +
                                "</td>\n</tr>");
                //argSW.WriteLine("<tr>\n\t<td>Fasta File: </td>\n\t<td>" + _fastaFile + "</td>\n</tr>");
                string tmp = "";
                //foreach (Protease.Type p in _ProteaseType)
                //{
                //    tmp = tmp + p.ToString() + ",";
                //}
                //argSW.WriteLine("<tr>\n\t<td>ProteaseType: </td>\n\t<td>" + tmp + "</td>\n</tr>");
                //argSW.WriteLine("<tr>\n\t<td>MissCleavage: </td>\n\t<td>" + _MissCLeavage.ToString() + "</td>\n</tr>");
                if (argGlycanSeqParas.UseGlycanList)
                {
                    argSW.WriteLine("<tr>\n\t<td>Glycan File: </td>\n\t<td>" + argGlycanSeqParas.GlycanFile + "</td>\n</tr>");
                }
                else
                {
                    tmp = "HexNAc:" + argGlycanSeqParas.NoHexNAc.ToString() + ";" +
                          "Hex:" + argGlycanSeqParas.NoHex.ToString() + ";" +
                          "Sia" + argGlycanSeqParas.NoSia.ToString() + ";" +
                          "DeHex:" + argGlycanSeqParas.NoDeHex.ToString() + ";";
                    argSW.WriteLine("<tr>\n\t<td>Glycan Composition: </td>\n\t<td>" + tmp + "</td>\n</tr>");
                }
                argSW.WriteLine("<tr>\n\t<td>MS tolerance(PPM)/MS2 tolerance(Da: </td>\n\t<td>" + argGlycanSeqParas.PrecursorTol.ToString() + "/" +
                                argGlycanSeqParas.MSMSTol.ToString() + "</td>\n</tr>");
                argSW.WriteLine("<tr>\n\t<td>N-Glycan/Human: </td>\n\t<td>" + argGlycanSeqParas.IsNGlycan.ToString() + "/" + argGlycanSeqParas.IsHuman.ToString() +
                                "</td>\n</tr>");
                argSW.WriteLine("<tr>\n\t<td>Use HCD/Sequencing HCD: </td>\n\t<td>" + argGlycanSeqParas.UseHCD.ToString() + "/" +
                                argGlycanSeqParas.SeqHCD.ToString() + "</td>\n</tr>");
                argSW.WriteLine("<tr>\n\t<td>Export Completed Only/Completed Reward: </td>\n\t<td>" +
                                argGlycanSeqParas.CompletedOnly.ToString() + "/" + argGlycanSeqParas.CompletedReward.ToString() + "</td>\n</tr>");
                argSW.WriteLine("<tr>\n\t<td>Peptide Mutation: </td>\n\t<td>" +
                                 argGlycanSeqParas.PeptideMutation.ToString() + "</td>\n</tr>");
                argSW.WriteLine("</Table>\n<br>\n<br>");
                argSW.Flush();
                StreamReader Sr;
                foreach (string fileName in Directory.GetFiles(argGlycanSeqParas.ExportFolder))
                {
                    if (Path.GetExtension(fileName) == ".txt")
                    {
                        Sr = new StreamReader(fileName);
                        do
                        {
                            argSW.WriteLine(Sr.ReadLine());
                        } while (!Sr.EndOfStream);
                        Sr.Close();
                        File.Delete(fileName);
                        argSW.WriteLine("<a>");
                    }
                }
                argSW.WriteLine("<br></body>\n</html>");

            }
            catch (Exception)
            {

            }
            finally
            {
                argSW.Close();
            }

        }
        public static void ExportToFolder_old(string argFolder, MSScan argScan, bool argExportIndividualReport, int argTopRank, List<Tuple<string, GlycanStructure, double>> argStructureResults)
        {
            // GlycanSequencing GS = argGS;
            List<Tuple<string, GlycanStructure, double>> lstStructureResults = new List<Tuple<string, GlycanStructure, double>>();
            MSScan ScanInfo = argScan;
            argStructureResults.Sort((a, b) => -1 * a.Item3.CompareTo(b.Item3));

            List<string> lstExportedResult = new List<string>();

            double previousScore = 0;
            int ExportedRank = 0;
            foreach (Tuple<string, GlycanStructure, double> result in argStructureResults)
            {
                string key = result.Item1 + "-" + result.Item2.IUPACString;
                if (lstExportedResult.Contains(key))
                {
                    continue;
                }
                if (previousScore != result.Item3)
                {
                    ExportedRank = ExportedRank + 1;
                    previousScore = result.Item3;
                    if (ExportedRank > argTopRank)
                    {
                        continue;
                    }
                }
                lstExportedResult.Add(key);
                lstStructureResults.Add(result);
            }
            lstStructureResults.Sort((a, b) => -1 * a.Item3.CompareTo(b.Item3));

            StringBuilder sbResult = new StringBuilder();

            sbResult.AppendLine("Scan :" + ScanInfo.ScanNo.ToString() + "(" + ScanInfo.Time.ToString("0.00") + " mins)<br>");
            sbResult.AppendLine("Precursor m/z :" + ScanInfo.ParentMZ.ToString("0.0000") + "(" + ScanInfo.ParentCharge.ToString() + "+)<br>");

            sbResult.AppendLine("<table border=\"1\" style=\"width:100%\">");
            sbResult.AppendLine("\t<tr>");
            sbResult.AppendLine("\t\t<td>Peptide Sequence</td>");
            sbResult.AppendLine("\t\t<td>Glycan Sequence</td>");
            sbResult.AppendLine("\t\t<td>Glycan Picture</td>");
            sbResult.AppendLine("\t\t<td>Score</td>");
            sbResult.AppendLine("\t\t<td>Completed Sequence?</td>");
            sbResult.AppendLine("\t</tr>");

            foreach (Tuple<string, GlycanStructure, double> SeqResult in lstStructureResults)
            {
                sbResult.AppendLine("\t<tr>");
                sbResult.AppendLine("\t\t<td>" + SeqResult.Item1 + "</td>");
                sbResult.AppendLine("\t\t<td>" + SeqResult.Item2.IUPACString + "</td>");

                //Picture 
                GlycansDrawer Draw;
                string PicLocation = argFolder + "\\Pics\\" + SeqResult.Item2.IUPACString.ToString() + ".png";
                if (!File.Exists(PicLocation))
                {
                    Draw = new GlycansDrawer(SeqResult.Item2.IUPACString, false);
                    Image Pic = GlycanImage.RotateImage(Draw.GetImage(), 180);
                    Pic.Save(PicLocation);
                    Pic.Dispose();
                    Pic = null;
                }
                sbResult.AppendLine("<td><img src=\".\\Pics\\" + SeqResult.Item2.IUPACString.ToString() + ".png\"/></td>");
                sbResult.AppendLine("\t\t<td>" + SeqResult.Item3.ToString("0.000") + "</td>");
                if (SeqResult.Item2.IsCompleteSequenced)
                {
                    sbResult.AppendLine("\t\t<td>Y</td>");
                }
                else
                {
                    sbResult.AppendLine("\t\t<td>N</td>");
                }
                sbResult.AppendLine("\t</tr>");

            }
            sbResult.AppendLine("</table>");

            StreamWriter swResult = new StreamWriter(argFolder + "\\" + ScanInfo.ScanNo.ToString("00000") + ".txt");
            swResult.Write(sbResult.ToString());
            swResult.Close();


            //Detail Report
            if (!argExportIndividualReport)
            {
                return;
            }
            if (lstStructureResults.Count > 0)
            {
                if (!Directory.Exists(argFolder))
                {
                    Directory.CreateDirectory(argFolder);
                }
                if (!Directory.Exists(argFolder + "\\Pics"))
                {
                    Directory.CreateDirectory(argFolder + "\\Pics");
                }
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">");
                sb.AppendLine("<html xmlns=\"http://www.w3.org/1999/xhtml\">");
                sb.AppendLine("<head>");
                sb.AppendLine("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />");
                sb.AppendLine("<title>Glycoseq result for scan " + ScanInfo.ScanNo.ToString() + "</title>\n</head>\n<body>");
                GlycansDrawer Draw;
                //List<GlycanStructure> ExactGlycanTreeMatch = new List<GlycanStructure>();

                //foreach (Tuple<string,GlycanStructure,float> result in lstStructureResults)
                //{
                //    GlycanStructure GTStructure = result.Item2;
                //    if (MassUtility.GetMassPPM(GTStructure.GlycanMonoMass,  GS.GlycanMonoMass) <=  GS.PrecursorTorelance)
                //    {
                //        ExactGlycanTreeMatch.Add(GTStructure);
                //    }
                //}
                //argGS.SequencedStructures.Sort(delegate(GlycanStructure T1, GlycanStructure T2)
                //{
                //    return T2.GlycanMonoMass.CompareTo(T1.GlycanMonoMass) != 0 ? T2.GlycanMonoMass.CompareTo(T1.GlycanMonoMass) : T2.Score.CompareTo(T1.Score)
                //      ;
                //});

                sb.AppendLine("Precursor m/z :" + ScanInfo.ParentMZ.ToString("0.0000") + "(" + ScanInfo.ParentCharge.ToString() + "+)<br>");
                //sb.AppendLine("Predicted Y1:" +  GS.Y1MZ.ToString("0.0000") + "<br>");
                //sb.AppendLine("Predicted GlycanMonoMass:" + GS.GlycanMonoMass.ToString("0.000") + "<br>");
                sb.AppendLine("Structure match with precursor will highlight in red");
                sb.AppendLine("<table  border=\"1\">");
                sb.AppendLine("<tr>");
                sb.AppendLine("<td>Id</td>");
                sb.AppendLine("<td>Peptide</td>");
                sb.AppendLine("<td>GlycanMass</td>");
                sb.AppendLine("<td>Score</td>");
                sb.AppendLine("<td>Image</td>");
                sb.AppendLine("<td>IUPAC</td>");
                sb.AppendLine("</tr>");
                //if (ExactGlycanTreeMatch.Count == 0)
                //{
                //    sb.AppendLine("<tr><td colspan=\"5\">No glycan structure match with precursor</td></tr>");
                //}
                int idx = 0;

                foreach (Tuple<string, GlycanStructure, double> result in lstStructureResults)
                {

                    GlycanStructure Gt = result.Item2;

                    string BGColor = "#FFFFFF"; //White
                    if (Gt.IsCompleteSequenced)
                    {
                        BGColor = "#FF0000"; //Red
                    }
                    try
                    {
                        string PicLocation = argFolder + "\\Pics\\" + Gt.IUPACString.ToString() + ".png";
                        if (!File.Exists(PicLocation))
                        {
                            Draw = new GlycansDrawer(Gt.IUPACString, false);
                            Image Pic = GlycanImage.RotateImage(Draw.GetImage(), 180);
                            Pic.Save(PicLocation);
                            Pic.Dispose();
                            Pic = null;
                        }
                        sb.AppendLine("<tr>");
                        sb.AppendLine("<td bgcolor=\"" + BGColor + "\"><a href=\"#" + (idx + 1).ToString() + "\">" + (idx + 1).ToString() + "</a></td>");
                        sb.AppendLine("<td bgcolor=\"" + BGColor + "\">" + result.Item1 + "</td>");
                        sb.AppendLine("<td bgcolor=\"" + BGColor + "\">" + Gt.GlycanMonoMass.ToString() + "</td>");
                        sb.AppendLine("<td bgcolor=\"" + BGColor + "\">" + Gt.Score.ToString("0.0000") + "</td>");
                        sb.AppendLine("<td><img src=\".\\Pics\\" + Gt.IUPACString.ToString() + ".png\"/></td>");
                        sb.AppendLine("<td colspan=\"2\" bgcolor=\"" + BGColor + "\">" + Gt.IUPACString + "</td>");
                        sb.AppendLine("</tr>");
                    }
                    catch
                    {
                        sb.AppendLine("<tr>");
                        sb.AppendLine("<td bgcolor=\"" + BGColor + "\"><a href=\"#" + (idx + 1).ToString() + "\">" + (idx + 1).ToString() + "</a></td>");
                        sb.AppendLine("<td bgcolor=\"" + BGColor + "\">" + result.Item1 + "</td>");
                        sb.AppendLine("<td bgcolor=\"" + BGColor + "\">" + Gt.GlycanMonoMass.ToString() + "</td>");
                        sb.AppendLine("<td bgcolor=\"" + BGColor + "\">" + Gt.Score.ToString("0.0000") + "</td>");
                        sb.AppendLine("<td></td>");
                        sb.AppendLine("<td colspan=\"2\" bgcolor=\"" + BGColor + "\">" + Gt.IUPACString + "</td>");
                        sb.AppendLine("</tr>");
                    }
                    idx++;
                }
                sb.AppendLine("</table><br><br>");

                //Detail Summary table

                idx = 0;
                foreach (Tuple<string, GlycanStructure, double> result in lstStructureResults)
                {

                    GlycanStructure Gt = result.Item2;

                    sb.AppendLine("<table  border=\"1\">");
                    sb.AppendLine("<tr>");
                    sb.AppendLine("<td><a name=\"" + idx.ToString() + "\">Id: " + idx.ToString() + "</a></td>");
                    sb.AppendLine("<td>GlycanMass:" + Gt.GlycanMonoMass.ToString() + "</td>");
                    sb.AppendLine("<td>Score:" + Gt.Score.ToString("0.000") + "</td>");
                    sb.AppendLine("</tr>");
                    sb.AppendLine("<tr>");
                    sb.AppendLine("<td>" + result.Item1 + "</td>");
                    sb.AppendLine("<td><img src=\".\\Pics\\" + Gt.IUPACString.ToString() + ".png\"/></td>");
                    sb.AppendLine("<td>" + Gt.IUPACString + "</td>");
                    sb.AppendLine("</tr>");
                    sb.AppendLine("</table><br>");


                    List<GlycanTreeNode> lstFragement = Gt.TheoreticalFragment;
                    lstFragement.Sort(delegate(GlycanTreeNode t1, GlycanTreeNode t2) { return GlycanMass.GetGlycanMasswithCharge(t1.GlycanType, Gt.Charge).CompareTo(GlycanMass.GetGlycanMasswithCharge(t2.GlycanType, Gt.Charge)); });


                    List<MSPoint> msp = new List<MSPoint>();
                    foreach (MSPeak p in ScanInfo.MSPeaks)
                    {
                        msp.Add(new MSPoint(p.MonoMass, p.MonoIntensity));
                    }

                    Image AnnotatedSpectrum = GlycanImage.GetAnnotatedImage(result.Item1, ScanInfo, msp, Gt);
                    System.IO.MemoryStream mss = new System.IO.MemoryStream();
                    System.IO.FileStream fs = new System.IO.FileStream(argFolder + "\\Pics\\AnnotatedSpectrum_" + ScanInfo.ScanNo + "_" + (idx + 1).ToString("000") + ".png", System.IO.FileMode.Create, System.IO.FileAccess.ReadWrite);


                    AnnotatedSpectrum.Save(mss, System.Drawing.Imaging.ImageFormat.Png);
                    byte[] matriz = mss.ToArray();
                    fs.Write(matriz, 0, matriz.Length);

                    mss.Close();
                    fs.Close();
                    AnnotatedSpectrum.Dispose();
                    AnnotatedSpectrum = null;
                    sb.AppendLine("<a href=\".\\Pics\\AnnotatedSpectrum_" + ScanInfo.ScanNo + "_" + (idx + 1).ToString("000") + ".png\" target=\"_blank\"><img src=\".\\Pics\\AnnotatedSpectrum_" + ScanInfo.ScanNo + "_" + (idx + 1).ToString("000") + ".png\" height=\"450\" width=\"600\"/></a>");

                    //Fragement table
                    sb.AppendLine("<table  border=\"1\">");
                    sb.AppendLine("<tr><td>Structure</td><td>Identified MZ</td><td>Identified Intensity</td></tr>");


                    foreach (GlycanTreeNode t in Gt.Root.FetchAllGlycanNode().OrderBy(o => o.IDMass).ToList())
                    {

                        sb.AppendLine("<tr>");
                        sb.AppendLine("<td>" + t.IUPACFromRoot + "</td>");
                        sb.AppendLine("<td>" + t.IDMass.ToString("0.0000") + "</td>");
                        sb.AppendLine("<td>" + t.IDIntensity.ToString("0.0000") + "</td>");
                        sb.AppendLine("</tr>");
                    }

                    //if (lstFragement.Count >= lstIDPeaksMz.Count)
                    //{
                    //    for (int j = 0; j < lstFragement.Count; j++)
                    //    {
                    //        sb.AppendLine("<tr>");
                    //        // sb.AppendLine("<td>" + (GlycanMass.GetGlycanMasswithCharge(lstFragement[j].GlycanType, Gt.Charge) + Gt.Y1.Mass- GlycanMass.GetGlycanMasswithCharge(Glycan.Type.HexNAc, Gt.Charge)).ToString() + "</td>");

                    //        //string PicLocation = argFolder + "\\Pics\\" + lstFragement[j].GetIUPACString() + ".png";
                    //        //if (!File.Exists(PicLocation))
                    //        //{
                    //        //    Draw = new GlycansDrawer(lstFragement[j].GetIUPACString(), false);
                    //        //    Image tmpImg = Draw.GetImage();
                    //        //    tmpImg.Save(PicLocation);
                    //        //    tmpImg.Dispose();
                    //        //    tmpImg = null;
                    //        //}

                    //        sb.AppendLine("<td>" + lstFragement[j].IUPAC + "</td>");

                    //        if (j < lstIDPeaksMz.Count)
                    //        {
                    //            sb.AppendLine("<td>" + lstIDPeaksMz[j].ToString("0.0000") + "</td>");
                    //            sb.AppendLine("<td>" + lstIDPeaksInt[j].ToString("0.0000") + "</td>");
                    //        }
                    //        else
                    //        {
                    //            sb.AppendLine("<td>-</td>");
                    //            sb.AppendLine("<td>-</td>");
                    //        }
                    //        sb.AppendLine("</tr>");
                    //    }
                    //}
                    //else
                    //{
                    //    for (int j = 0; j < lstIDPeaksMz.Count; j++)
                    //    {
                    //        sb.AppendLine("<tr>");

                    //        if (j < lstFragement.Count)
                    //        {
                    //            //sb.AppendLine("<td>" + (GlycanMass.GetGlycanMasswithCharge(lstFragement[j].GlycanType, Gt.Charge) + Gt.Y1.Mass - GlycanMass.GetGlycanMasswithCharge(Glycan.Type.HexNAc, Gt.Charge)).ToString() + "</td>");
                    //            //Draw = new GlycansDrawer(lstFragement[j].GetIUPACString(), true);
                    //            //Image tmpImg = Draw.GetImage();
                    //            //tmpImg.Save(argFolder + "\\Pics\\SubStructure_" + idx.ToString("000") + "-F" + (j + 1).ToString("00") + ".png");
                    //            //tmpImg.Dispose();
                    //            //tmpImg = null;
                    //            //sb.AppendLine("<td><img src=\".\\Pics\\SubStructure_" + idx.ToString("000") + "-F" + (j + 1).ToString("00") + ".png\"/>" + "</td>");
                    //            sb.AppendLine("<td>" + lstFragement[j].IUPAC + "</td>");
                    //        }
                    //        else
                    //        {
                    //            // sb.AppendLine("<td>-</td>");
                    //            sb.AppendLine("<td>-</td>");
                    //        }
                    //        sb.AppendLine("<td>" + lstIDPeaksMz[j].ToString() + "</td>");
                    //        sb.AppendLine("<td>" + lstIDPeaksInt[j].ToString() + "</td>");
                    //        sb.AppendLine("</tr>");
                    //    }
                    //}
                    sb.AppendLine("</table><br>-----------------------------------------------------------<br>");
                    idx++;
                    GC.Collect();
                }

                sb.AppendLine("</body>\n</html>");
                StreamWriter sw = new StreamWriter(argFolder + "\\Scan" + ScanInfo.ScanNo.ToString() + ".htm");
                sw.Write(sb.ToString());
                sw.Flush();
                sw.Close();
            }
        }
        public static void HtmlFormatTempForEachScan(string argFolder, MSScan argScan, bool argExportIndividualReport, int argTopRank, List<Tuple<string, GlycanStructure, double>> argStructureResults)
        {
            // GlycanSequencing GS = argGS;
            List<Tuple<string, GlycanStructure, double>> lstStructureResults = argStructureResults;
            MSScan ScanInfo = argScan;
            lstStructureResults.Sort((a, b) => -1 * a.Item3.CompareTo(b.Item3));

            StringBuilder sbResult = new StringBuilder();

            sbResult.AppendLine("Scan :" + ScanInfo.ScanNo.ToString() + "<br>");
            sbResult.AppendLine("Precursor m/z :" + ScanInfo.ParentMZ.ToString("0.0000") + "(" + ScanInfo.ParentCharge.ToString() + "+)<br>");

            sbResult.AppendLine("<table border=\"1\" style=\"width:100%\">");
            sbResult.AppendLine("\t<tr>");
            sbResult.AppendLine("\t\t<td>Peptide Sequence</td>");
            sbResult.AppendLine("\t\t<td>Glycan Sequence</td>");
            sbResult.AppendLine("\t\t<td>Glycan Picture</td>");
            sbResult.AppendLine("\t\t<td>Score</td>");
            sbResult.AppendLine("\t\t<td>Completed Sequence?</td>");
            sbResult.AppendLine("\t</tr>");

            foreach (Tuple<string, GlycanStructure, double> SeqResult in lstStructureResults)
            {
                sbResult.AppendLine("\t<tr>");
                sbResult.AppendLine("\t\t<td>" + SeqResult.Item1 + "</td>");
                sbResult.AppendLine("\t\t<td>" + SeqResult.Item2.IUPACString + "</td>");

                //Picture 
                GlycansDrawer Draw;
                string PicLocation = argFolder + "\\Pics\\" + SeqResult.Item2.IUPACString.ToString() + ".png";
                if (!File.Exists(PicLocation))
                {
                    Draw = new GlycansDrawer(SeqResult.Item2.IUPACString, false);
                    Image Pic = Draw.GetImage();
                    Pic.Save(PicLocation);
                    Pic.Dispose();
                    Pic = null;
                }
                sbResult.AppendLine("<td><img src=\".\\Pics\\" + SeqResult.Item2.IUPACString.ToString() + ".png\"/></td>");
                sbResult.AppendLine("\t\t<td>" + SeqResult.Item3.ToString("0.000") + "</td>");
                if (SeqResult.Item2.IsCompleteSequenced)
                {
                    sbResult.AppendLine("\t\t<td>Y</td>");
                }
                else
                {
                    sbResult.AppendLine("\t\t<td>N</td>");
                }
                sbResult.AppendLine("\t</tr>");

            }
            sbResult.AppendLine("</table>");

            StreamWriter swResult = new StreamWriter(argFolder + "\\" + ScanInfo.ScanNo.ToString("00000") + ".txt");
            swResult.Write(sbResult.ToString());
            swResult.Close();


            //Detail Report
            if (!argExportIndividualReport)
            {
                return;
            }
            if (lstStructureResults.Count > 0)
            {
                if (!Directory.Exists(argFolder))
                {
                    Directory.CreateDirectory(argFolder);
                }
                if (!Directory.Exists(argFolder + "\\Pics"))
                {
                    Directory.CreateDirectory(argFolder + "\\Pics");
                }
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">");
                sb.AppendLine("<html xmlns=\"http://www.w3.org/1999/xhtml\">");
                sb.AppendLine("<head>");
                sb.AppendLine("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />");
                sb.AppendLine("<title>Glycoseq result for scan " + ScanInfo.ScanNo.ToString() + "</title>\n</head>\n<body>");
                GlycansDrawer Draw;
                //List<GlycanStructure> ExactGlycanTreeMatch = new List<GlycanStructure>();

                //foreach (Tuple<string,GlycanStructure,float> result in lstStructureResults)
                //{
                //    GlycanStructure GTStructure = result.Item2;
                //    if (MassUtility.GetMassPPM(GTStructure.GlycanMonoMass,  GS.GlycanMonoMass) <=  GS.PrecursorTorelance)
                //    {
                //        ExactGlycanTreeMatch.Add(GTStructure);
                //    }
                //}
                //argGS.SequencedStructures.Sort(delegate(GlycanStructure T1, GlycanStructure T2)
                //{
                //    return T2.GlycanMonoMass.CompareTo(T1.GlycanMonoMass) != 0 ? T2.GlycanMonoMass.CompareTo(T1.GlycanMonoMass) : T2.Score.CompareTo(T1.Score)
                //      ;
                //});

                sb.AppendLine("Precursor m/z :" + ScanInfo.ParentMZ.ToString("0.0000") + "(" + ScanInfo.ParentCharge.ToString() + "+)<br>");
                //sb.AppendLine("Predicted Y1:" +  GS.Y1MZ.ToString("0.0000") + "<br>");
                //sb.AppendLine("Predicted GlycanMonoMass:" + GS.GlycanMonoMass.ToString("0.000") + "<br>");
                sb.AppendLine("Structure match with precursor will highlight in red");
                sb.AppendLine("<table  border=\"1\">");
                sb.AppendLine("<tr>");
                sb.AppendLine("<td>Id</td>");
                sb.AppendLine("<td>GlycanMass</td>");
                sb.AppendLine("<td>Score(PPM)</td>");
                sb.AppendLine("<td>Image</td>");
                sb.AppendLine("<td>IUPAC</td>");
                sb.AppendLine("</tr>");
                //if (ExactGlycanTreeMatch.Count == 0)
                //{
                //    sb.AppendLine("<tr><td colspan=\"5\">No glycan structure match with precursor</td></tr>");
                //}
                int idx = 0;
                foreach (Tuple<string, GlycanStructure, double> result in lstStructureResults)
                {
                    GlycanStructure Gt = result.Item2;

                    string BGColor = "#FFFFFF"; //White
                    if (Gt.IsCompleteSequenced)
                    {
                        BGColor = "#FF0000"; //Red
                    }
                    try
                    {
                        string PicLocation = argFolder + "\\Pics\\" + Gt.IUPACString.ToString() + ".png";
                        if (!File.Exists(PicLocation))
                        {
                            Draw = new GlycansDrawer(Gt.IUPACString, false);
                            Image Pic = Draw.GetImage();
                            Pic.Save(PicLocation);
                            Pic.Dispose();
                            Pic = null;
                        }
                        sb.AppendLine("<tr>");
                        sb.AppendLine("<td bgcolor=\"" + BGColor + "\"><a href=\"#" + (idx + 1).ToString() + "\">" + (idx + 1).ToString() + "</a></td>");
                        sb.AppendLine("<td bgcolor=\"" + BGColor + "\">" + Gt.GlycanMonoMass.ToString() + "</td>");
                        sb.AppendLine("<td bgcolor=\"" + BGColor + "\">" + Gt.Score.ToString("0.0000") + "</td>");
                        sb.AppendLine("<td><img src=\".\\Pics\\" + Gt.IUPACString.ToString() + ".png\"/></td>");
                        sb.AppendLine("<td colspan=\"2\" bgcolor=\"" + BGColor + "\">" + Gt.IUPACString + "</td>");
                        sb.AppendLine("</tr>");
                    }
                    catch
                    {
                        sb.AppendLine("<tr>");
                        sb.AppendLine("<td bgcolor=\"" + BGColor + "\"><a href=\"#" + (idx + 1).ToString() + "\">" + (idx + 1).ToString() + "</a></td>");
                        sb.AppendLine("<td bgcolor=\"" + BGColor + "\">" + Gt.GlycanMonoMass.ToString() + "</td>");
                        sb.AppendLine("<td bgcolor=\"" + BGColor + "\">" + Gt.Score.ToString("0.0000") + "</td>");
                        sb.AppendLine("<td></td>");
                        sb.AppendLine("<td colspan=\"2\" bgcolor=\"" + BGColor + "\">" + Gt.IUPACString + "</td>");
                        sb.AppendLine("</tr>");
                    }
                    idx++;
                }
                sb.AppendLine("</table><br><br>");

                //Detail Table
                idx = 1;
                foreach (Tuple<string, GlycanStructure, double> result in lstStructureResults)
                {
                    GlycanStructure Gt = result.Item2;

                    sb.AppendLine("<table  border=\"1\">");
                    sb.AppendLine("<tr>");
                    sb.AppendLine("<td><a name=\"" + idx.ToString() + "\">Id: " + idx.ToString() + "</a></td>");
                    sb.AppendLine("<td>GlycanMass:" + Gt.GlycanMonoMass.ToString() + "</td>");
                    sb.AppendLine("<td>Score" + Gt.Score.ToString() + "</td>");
                    sb.AppendLine("</tr>");
                    sb.AppendLine("<tr>");
                    sb.AppendLine("<td><img src=\".\\Pics\\" + Gt.IUPACString.ToString() + ".png\"/></td>");
                    sb.AppendLine("<td>" + Gt.IUPACString + "</td>");
                    sb.AppendLine("</tr>");
                    sb.AppendLine("</table><br>");


                    List<GlycanTreeNode> lstFragement = Gt.TheoreticalFragment;
                    lstFragement.Sort(delegate(GlycanTreeNode t1, GlycanTreeNode t2) { return GlycanMass.GetGlycanMasswithCharge(t1.GlycanType, Gt.Charge).CompareTo(GlycanMass.GetGlycanMasswithCharge(t2.GlycanType, Gt.Charge)); });
                    List<float> lstIDPeaksMz = new List<float>();
                    List<float> lstIDPeaksInt = new List<float>();

                    foreach (GlycanTreeNode t in Gt.Root.FetchAllGlycanNode())
                    {
                        lstIDPeaksMz.Add(t.IDMass);
                        lstIDPeaksInt.Add(t.IDIntensity);
                    }
                    List<MSPoint> msp = new List<MSPoint>();
                    foreach (MSPeak p in ScanInfo.MSPeaks)
                    {
                        msp.Add(new MSPoint(p.MonoMass, p.MonoIntensity));
                    }

                    Image AnnotatedSpectrum = GlycanImage.GetAnnotatedImage(result.Item1, ScanInfo, msp, Gt);
                    System.IO.MemoryStream mss = new System.IO.MemoryStream();
                    System.IO.FileStream fs = new System.IO.FileStream(argFolder + "\\Pics\\AnnotatedSpectrum_" + ScanInfo.ScanNo + "_" + (idx + 1).ToString("000") + ".png", System.IO.FileMode.Create, System.IO.FileAccess.ReadWrite);

                    //AnnotatedSpectrum.Save(argFolder + "\\Pics\\AnnotatedSpectrum_" + ScanInfo.ScanNo + "_" + (i + 1).ToString("000") + ".png");
                    AnnotatedSpectrum.Save(mss, System.Drawing.Imaging.ImageFormat.Png);
                    byte[] matriz = mss.ToArray();
                    fs.Write(matriz, 0, matriz.Length);

                    mss.Close();
                    fs.Close();
                    AnnotatedSpectrum.Dispose();
                    AnnotatedSpectrum = null;
                    sb.AppendLine("<a href=\".\\Pics\\AnnotatedSpectrum_" + ScanInfo.ScanNo + "_" + (idx + 1).ToString("000") + ".png\" target=\"_blank\"><img src=\".\\Pics\\AnnotatedSpectrum_" + ScanInfo.ScanNo + "_" + (idx + 1).ToString("000") + ".png\" height=\"450\" width=\"600\"/></a>");
                    //Fragement table
                    sb.AppendLine("<table  border=\"1\">");
                    sb.AppendLine("<tr><td colspan=\"2\">Fragement</td><td colspan=\"2\">Identified Peak</td></tr>");
                    sb.AppendLine("<tr><td>Glycopeptide MZ</td><td>Structure</td><td>Identified MZ</td><td>Identified Intensity</td></tr>");

                    if (lstFragement.Count >= lstIDPeaksMz.Count)
                    {
                        for (int j = 0; j < lstFragement.Count; j++)
                        {
                            sb.AppendLine("<tr>");
                            sb.AppendLine("<td>" + (GlycanMass.GetGlycanMasswithCharge(lstFragement[j].GlycanType, Gt.Charge) + Gt.Y1.Mass - GlycanMass.GetGlycanMasswithCharge(Glycan.Type.HexNAc, Gt.Charge)).ToString() + "</td>");
                            string PicLocation = argFolder + "\\Pics\\" + lstFragement[j].GetIUPACString() + ".png";
                            if (!File.Exists(PicLocation))
                            {
                                Draw = new GlycansDrawer(lstFragement[j].GetIUPACString(), false);
                                Image tmpImg = Draw.GetImage();
                                tmpImg.Save(PicLocation);
                                tmpImg.Dispose();
                                tmpImg = null;
                            }

                            sb.AppendLine("<td><img src=\".\\Pics\\" + lstFragement[j].GetIUPACString() + ".png\"/>" + "</td>");

                            if (j < lstIDPeaksMz.Count)
                            {
                                sb.AppendLine("<td>" + lstIDPeaksMz[j].ToString() + "</td>");
                                sb.AppendLine("<td>" + lstIDPeaksInt[j].ToString() + "</td>");
                            }
                            else
                            {
                                sb.AppendLine("<td>-</td>");
                                sb.AppendLine("<td>-</td>");
                            }
                            sb.AppendLine("</tr>");
                        }
                    }
                    else
                    {
                        for (int j = 0; j < lstIDPeaksMz.Count; j++)
                        {
                            sb.AppendLine("<tr>");

                            if (j < lstFragement.Count)
                            {
                                sb.AppendLine("<td>" + (GlycanMass.GetGlycanMasswithCharge(lstFragement[j].GlycanType, Gt.Charge) + Gt.Y1.Mass - GlycanMass.GetGlycanMasswithCharge(Glycan.Type.HexNAc, Gt.Charge)).ToString() + "</td>");
                                Draw = new GlycansDrawer(lstFragement[j].GetIUPACString(), true);
                                Image tmpImg = Draw.GetImage();
                                tmpImg.Save(argFolder + "\\Pics\\SubStructure_" + idx.ToString("000") + "-F" + (j + 1).ToString("00") + ".png");
                                tmpImg.Dispose();
                                tmpImg = null;
                                sb.AppendLine("<td><img src=\".\\Pics\\SubStructure_" + idx.ToString("000") + "-F" + (j + 1).ToString("00") + ".png\"/>" + "</td>");

                            }
                            else
                            {
                                sb.AppendLine("<td>-</td>");
                                sb.AppendLine("<td>-</td>");
                            }
                            sb.AppendLine("<td>" + lstIDPeaksMz[j].ToString() + "</td>");
                            sb.AppendLine("<td>" + lstIDPeaksInt[j].ToString() + "</td>");
                            sb.AppendLine("</tr>");
                        }
                    }
                    sb.AppendLine("</table><br>-----------------------------------------------------------<br>");
                    idx++;
                    GC.Collect();
                }

                sb.AppendLine("</body>\n</html>");
                StreamWriter sw = new StreamWriter(argFolder + "\\Scan" + ScanInfo.ScanNo.ToString() + ".htm");
                sw.Write(sb.ToString());
                sw.Flush();
                sw.Close();
            }
        }
    }
}
