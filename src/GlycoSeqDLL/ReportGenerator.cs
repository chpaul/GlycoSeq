using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COL.GlycoLib;
using COL.MassLib;
using System.Drawing;
using System.IO;
using COL.ProtLib;
using CSMSL.Chemistry;

namespace COL.GlycoSequence
{
    public class ReportGenerator
    {
        /// <summary>
        /// 
        /// </summary>
        public static void CSVFormatParameter(GlycanSeqParameters argGlycanSeqParas, string argFolder)
        {

            using (StreamWriter sw = new StreamWriter(argFolder + "\\Result.csv", true))
            {
                //Raw
                sw.WriteLine("Raw File," + argGlycanSeqParas.RawFilePath);
                sw.WriteLine("Scan Range," + argGlycanSeqParas.StartScan.ToString() + "~" + argGlycanSeqParas.EndScan.ToString());
                sw.WriteLine("Use HCD," + argGlycanSeqParas.UseHCD.ToString());
                sw.WriteLine("Sequencing HCD," + argGlycanSeqParas.SeqHCD.ToString());
                
                //Glycan
                if (argGlycanSeqParas.UseGlycanList)
                {
                    sw.WriteLine("Glycan File," + argGlycanSeqParas.GlycanFile);
                }
                else
                {
                    sw.WriteLine("Glycan Composition,HexNAc:" + argGlycanSeqParas.NoHexNAc.ToString() + ";" + "Hex:" + argGlycanSeqParas.NoHex.ToString() + ";" +
                      "DeHex:" + argGlycanSeqParas.NoDeHex.ToString() + ";" + "Sia" + argGlycanSeqParas.NoSia.ToString());
                }
                sw.WriteLine("N-Glycan," + argGlycanSeqParas.IsNGlycan.ToString());
                sw.WriteLine("Human," + argGlycanSeqParas.IsHuman.ToString());
                
                //Torelance
                sw.WriteLine("MS tolerance(PPM)," + argGlycanSeqParas.PrecursorTol.ToString());
                sw.WriteLine("MS2 tolerance(Da)," + argGlycanSeqParas.MSMSTol.ToString());
                sw.WriteLine("Search Y1 without matched peptide," + argGlycanSeqParas.UnknownPeptideSearch.ToString());
               
                //Export
                sw.WriteLine("Get Top," +argGlycanSeqParas.GetTopRank.ToString());
                sw.WriteLine("Export Completed Only," + argGlycanSeqParas.CompletedOnly.ToString());
                sw.WriteLine("Individual Result,"+argGlycanSeqParas.ExportIndividualSpectrum.ToString());
           
                //Sequencing Parameters
                sw.WriteLine("Top i Y1,"+argGlycanSeqParas.GetTopY1_i.ToString());
                sw.WriteLine("Top k Core,"+argGlycanSeqParas.GetTopCore_k.ToString());
                sw.WriteLine("Top l branch,"+argGlycanSeqParas.GetBranch_l.ToString());
                sw.WriteLine("Max padding glycans,"+argGlycanSeqParas.MaxPanddingGlycan.ToString());

                //Peptide
                if (argGlycanSeqParas.PeptideFromMascotResult)
                {
                     sw.WriteLine("Peptide File Type,Mascot");
                     sw.WriteLine("Time Shift,"+argGlycanSeqParas.MascotTimeShift.ToString());
                     sw.WriteLine("Time Torelance,"+argGlycanSeqParas.MascotTimeTolerance.ToString());
                }
                else
                {
                     sw.WriteLine("Peptide File Type,Fasta");
                     sw.WriteLine("Enzyme,"+argGlycanSeqParas.Enzyme);
                     sw.WriteLine("Missed cleavages,"+argGlycanSeqParas.MissedCleavages.ToString());
                }
                 sw.WriteLine("Peptide Mutation,"+argGlycanSeqParas.PeptideMutation.ToString());
                sw.WriteLine("Peptide File," + argGlycanSeqParas.FastaFile);
                //Title
                sw.WriteLine("MSn_Scan,Parent_Scan,Parent_Mz,Mono_Mz,Charge_State,Monoisotopic_Mass,CID_time,Y1,Peptide,Peptide_Mod,Identified_By_Mascot,Category,Category_Probability,Core_Score,Branch_Score,Append_Glycan_Score,MatchWithPrecursorMW,PPM,Full_Glycan(HexNac-Hex-deHex-NeuAC-NeuGc),Append_Glycan(HexNac-Hex-deHex-NeuAC-NeuGc),IUPAC");
            }
        }
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
      
            StreamWriter sw = new StreamWriter(argFolder + "\\Result.csv",  true);
            
            string header = argScan.ScanNo.ToString() + "," + argScan.ParentScanNo.ToString() + "," +
                            argScan.ParentMZ.ToString() + "," +argScan.ParentMonoMz+","+ argScan.ParentCharge.ToString() + "," +
                            (argScan.ParentMonoMz * argScan.ParentCharge - Atoms.ProtonMass*argScan.ParentCharge)+","+ argScan.Time.ToString("0.00")+",";
           argStructureResults = argStructureResults.OrderBy(y=>y.Item2.IsCompleteByPrecursorDifference).ThenByDescending(x => x.Item2.Score).ToList();

          // List<string> lstExportedResult = new List<string>();
           double previousScore = 0;
           int ExportedRank = 0;


           var GroupedRecords = argStructureResults.GroupBy(x => new {x.Item1, x.Item3});
           var GroupedRecordsWithPeptide = GroupedRecords.Where(x => !x.Key.Item1.StartsWith("+")).OrderByDescending(x=>x.Key.Item3);
           var GroupedRecordsNoPeptide =    GroupedRecords.Where(x => x.Key.Item1.StartsWith("+")).OrderByDescending(x => x.Key.Item3);
           var filteredRecords = GroupedRecordsWithPeptide.Take(argTopRank);
           
           int different = argTopRank - GroupedRecordsWithPeptide.ToList().Count;

           if (different > 0)
           {
               filteredRecords = filteredRecords.Concat(GroupedRecordsNoPeptide.Take(different));
           }
           foreach (var SeqResult in filteredRecords)
           {
               var lstRecords = SeqResult.ToArray();
               string IdentifedPeptide = "No";
               if (lstRecords[0].Item2.TargetPeptide != null && lstRecords[0].Item2.TargetPeptide.IdentifiedPeptide == true)
               {
                   IdentifedPeptide = "Yes";
               }
               string mergerdIUPAC="";
               
               //Replace append glycan
               string[] aryAppend = lstRecords[0].Item2.RestGlycanString.Split('-');
               string appendGlycanString = "";
               for (int i = 0; i < aryAppend.Length; i++)
               {
                   string label = "Hex";
                   switch (i)
                   {
                       case 0:
                           label = "Hex";
                           break;
                       case 1:
                           label = "HexNAc";
                           break;
                       case 2:
                           label = "DeHex";
                           break;
                       case 3:
                           label = "NeuAc";
                           break;
                       case 4:
                           label = "NeuGc";
                           break;
                   }
                   if (aryAppend[i] != "0")
                   {
                       appendGlycanString += label +  aryAppend[i];
                   }
               }

               for (int i = 0; i < lstRecords.Length; i++)
               {
                   mergerdIUPAC += lstRecords[i].Item2.IUPACString +"+" + appendGlycanString +",";
               }
               mergerdIUPAC=mergerdIUPAC.Substring(0, mergerdIUPAC.Length - 1);
               string PPM = lstRecords[0].Item2.PeptideSequence == ""? "-":lstRecords[0].Item2.PPM.ToString("0.00");
               string Peptide = lstRecords[0].Item2.PeptideSequence == "" ? "-" : lstRecords[0].Item2.PeptideSequence;
               string PeptideModificationString = lstRecords[0].Item2.PeptideModificationString == ""? "-": lstRecords[0].Item2.PeptideModificationString;
               sw.WriteLine(header + lstRecords[0].Item2.Y1.Mass.ToString("0.0000") + "," +
             Peptide + "," +
            PeptideModificationString + "," +
             IdentifedPeptide + "," +
             lstRecords[0].Item2.SVMPredictedLabel + "," +
          lstRecords[0].Item2.SVMPrrdictedProbabilities[lstRecords[0].Item2.SVMPredictedLabel].ToString("0.00") + "," +
          lstRecords[0].Item2.CoreScore.ToString("0.00") + "," +
          lstRecords[0].Item2.BranchScore.ToString("0.00") + "," +
          lstRecords[0].Item2.InCompleteScore.ToString("0.00") + "," +
          lstRecords[0].Item2.MatchWithPrecursorMW.ToString() + "," +
         PPM + "," +
          lstRecords[0].Item2.FullSequencedGlycanString.ToString() + "," +
          lstRecords[0].Item2.RestGlycanString + "," +
         mergerdIUPAC);
               //sw.WriteLine(header + lstRecords[0].Item2.Y1.Mass.ToString("0.0000") + "," +
               //             Peptide + "," +
               //            PeptideModificationString + "," +
               //             IdentifedPeptide + "," +
               //             lstRecords[0].Item2.SVMPredictedLabel +"," +
               //          lstRecords[0].Item2.SVMPrrdictedProbabilities[lstRecords[0].Item2.SVMPredictedLabel - 1].ToString("0.00") + ","+
               //          lstRecords[0].Item2.CoreScore.ToString("0.00") + "," +
               //          lstRecords[0].Item2.BranchScore.ToString("0.00") + "," +
               //          lstRecords[0].Item2.InCompleteScore.ToString("0.00") + "," +
               //          lstRecords[0].Item2.MatchWithPrecursorMW.ToString() + "," +
               //         PPM + "," +
               //          lstRecords[0].Item2.FullSequencedGlycanString.ToString() + "," +
               //          lstRecords[0].Item2.RestGlycanString + "," +
               //         mergerdIUPAC);
           }
           
           // foreach (Tuple<string, GlycanStructure, double> SeqResult in argStructureResults)
           //{
           //    //string key = SeqResult.Item1 + "-" + SeqResult.Item2.NoOfHexNac+"-"+SeqResult.Item2.NoOfHex+"-"+SeqResult.Item2.NoOfDeHex+"-"+SeqResult.Item2.NoOfNeuAc+"-"+SeqResult.Item2.NoOfNeuGc;
           //    //if (lstExportedResult.Contains(key))
           //    //{
           //    //    continue;
           //    //}
           //    if (previousScore != Convert.ToSingle(SeqResult.Item3.ToString("0.00")))
           //    {
           //        ExportedRank = ExportedRank + 1;
           //        previousScore = Convert.ToSingle(SeqResult.Item3.ToString("0.00"));
           //        if (ExportedRank > argTopRank)
           //        {
           //            break;
           //        }
           //    }
           //   // lstExportedResult.Add(key);

         
           //    string IdentifedPeptide = "No";
           //    if (SeqResult.Item2.TargetPeptide != null && SeqResult.Item2.TargetPeptide.IdentifiedPeptide == true)
           //    {
           //        IdentifedPeptide = "Yes";
           //    }

           //    sw.WriteLine(header + SeqResult.Item2.Y1.Mass.ToString("0.0000") +"," +
           //                 SeqResult.Item2.PeptideSequence + "," +
           //                 SeqResult.Item2.PeptideModificationString + "," +
           //                 IdentifedPeptide + "," +
           //                 SeqResult.Item2.CoreScore.ToString("0.00")+","+
           //                 SeqResult.Item2.BranchScore.ToString("0.00") + "," +
           //                 SeqResult.Item2.InCompleteScore.ToString("0.00") + "," +
           //                 SeqResult.Item2.IsCompleteSequenced.ToString()+ "," +
           //                 SeqResult.Item2.PPM.ToString("0.00") + "," +
           //                 SeqResult.Item2.FullSequencedGlycanString.ToString() + "," +
           //                 SeqResult.Item2.RestGlycanString+ "," +
           //                 SeqResult.Item2.IUPACString);

           //}
            sw.Flush();
            sw.Close();
        }
        public static void GenerateLearningMatrixTwoLabels(GlycanSequencing_MultipleScoring argGS, string argExportFile, int argGetTopCount)
        {
            List<string> lstLearningMatrixStrings = new List<string>();
            //Title
            if (!File.Exists(argExportFile))
            {
                using (StreamWriter sw = new StreamWriter(argExportFile))
                {
                    sw.WriteLine("ScanNum,PeptideSequence,GlycanSequence,IsPeptide,CoreFuc,TotalGlycan,PPM,CorePeakIDNum,CorePeakIDScore,BranchPeakIDNum,BranchPeakIDScore,Y1,Y2,Y3,Y4,Y5,MatchedPrecursorMW,Category");
                }
            }
            List<GlycanStructure> AllStructure = argGS.FullSequencedStructures;
            AllStructure.AddRange(argGS.SequencedStructures);
            var glycanStructure = AllStructure.GroupBy(x => new { Peptide = x.PeptideSequence, Glycan = x.FullSequencedGlycanString}).OrderByDescending(z=>z.Key.Peptide.Length).ToList();
            foreach (var glycans in glycanStructure.AsEnumerable())
            {
                foreach (var glycan in glycans.OrderByDescending(x=> x.BranchScore+ x.CoreScore).AsEnumerable().Take(1))
                {
                    string Peptide = "0";
                    if (glycan.PeptideSequence != "")
                    {
                        Peptide = "1";
                    }
                    //Category
                    bool categoey = false; //1: Peptide + Glycan <10PPM; 2: No peptide but complete sequence; 3:Other
                    if (glycan.MatchWithPrecursorMW)
                    {
                        categoey = true;
                    }

                    string fuc = "1";
                    int CoreYPeaks = glycan.CoreIDPeak.Count;
                    float CoreYScore = glycan.CoreScore;
                    if (!glycan.IUPACString.EndsWith("(DeHex-)HexNAc"))
                    {
                        CoreYPeaks = glycan.CoreIDPeak.Count - glycan.CoreIDPeak.Where(x => x.Item2.Contains("deHex")).ToList().Count;
                        foreach (var core in glycan.CoreIDPeak.Where(x => x.Item2.EndsWith("(DeHex-)HexNAc")))
                        {
                            CoreYScore -= core.Item1.Intensity;
                        }
                        fuc = "0";
                    }
                    int totalGlycan = 0;
                    string[] tmpGlycan = glycan.FullSequencedGlycanString.Split('-');
                    for (int i = 0; i < tmpGlycan.Length; i++)
                    {
                        totalGlycan+= Convert.ToInt32(tmpGlycan[i]);
                    }
                    string tmpStr = argGS.ScanInfo.ScanNo.ToString() + "," +
                                 glycan.PeptideSequence +","+
                                 glycan.FullSequencedGlycanString + "," +
                                 Peptide + "," +
                                 fuc + "," +
                                 totalGlycan.ToString() + "," +
                                 glycan.PPM.ToString("0.000") + "," +
                                 CoreYPeaks.ToString() + "," +
                                 CoreYScore.ToString("0.0000") + "," +
                                 glycan.BranchIDPeak.Count.ToString() + "," +
                                 glycan.BranchScore.ToString("0.0000") + ",";


                    var CorePeaks = glycan.CoreIDPeak.Where(x => x.Item2 == "HexNAc").ToList();
                    tmpStr += (CorePeaks.Count != 0 ? CorePeaks[0].Item1.Intensity.ToString("0.0000") : "0") + ",";
                    CorePeaks = glycan.CoreIDPeak.Where(x => x.Item2 == "HexNAc-HexNAc").ToList();
                    tmpStr += (CorePeaks.Count != 0 ? CorePeaks[0].Item1.Intensity.ToString("0.0000") : "0") + ",";
                    CorePeaks = glycan.CoreIDPeak.Where(x => x.Item2 == "HexNAc-HexNAc-Hex").ToList();
                    tmpStr += (CorePeaks.Count != 0 ? CorePeaks[0].Item1.Intensity.ToString("0.0000") : "0") + ",";
                    CorePeaks = glycan.CoreIDPeak.Where(x => x.Item2 == "HexNAc-HexNAc-Hex-Hex").ToList();
                    tmpStr += (CorePeaks.Count != 0 ? CorePeaks[0].Item1.Intensity.ToString("0.0000") : "0") + ",";
                    CorePeaks = glycan.CoreIDPeak.Where(x => x.Item2 == "HexNAc-HexNAc-Hex-(Hex-)Hex").ToList();
                    tmpStr += (CorePeaks.Count != 0 ? CorePeaks[0].Item1.Intensity.ToString("0.0000") : "0") + ",";


                    if (glycan.MatchWithPrecursorMW)
                    {
                        tmpStr += "1,";
                    }
                    else
                    {
                        tmpStr += "0,";
                    }

                    tmpStr += categoey;


                    if (!lstLearningMatrixStrings.Contains(tmpStr))
                    {
                        using (StreamWriter sw = new StreamWriter(argExportFile, true))
                        {
                            sw.WriteLine(tmpStr);
                        }
                        lstLearningMatrixStrings.Add(tmpStr);
                    }
                }
            }
        }

        public static void GenerateLearningMatrix(GlycanSequencing_MultipleScoring argGS, string argExportFile, int argGetTopCount)
        {
            List<string> lstLearningMatrixStrings = new List<string>();
             //Title
            if (!File.Exists(argExportFile))
            {
                using (StreamWriter sw = new StreamWriter(argExportFile))
                {
                    sw.WriteLine("ScanNum,PeptideSequence,GlycanSequence,CoreFuc,TotalGlycan,PPM,CorePeakIDNum,CorePeakIDScore,BranchPeakIDNum,BranchPeakIDScore,Y1,Y2,Y3,Y4,Y5,MatchedPrecursorMW,Category");
                }
            }
            var glycanStructure = argGS.FullSequencedStructures.GroupBy(x => new { GlycanTotal = x.NoOfTotalGlycan, Score = x.BranchScore + x.CoreScore }).OrderByDescending(y => y.Key.GlycanTotal).ThenByDescending(z => z.Key.Score).ToList();
            glycanStructure.AddRange(argGS.SequencedStructures.GroupBy(x => new { GlycanTotal = x.NoOfTotalGlycan, Score = x.BranchScore + x.CoreScore }).OrderByDescending(y => y.Key.GlycanTotal).ThenByDescending(z => z.Key.Score).Take(argGetTopCount).ToList());

            foreach (var glycans in glycanStructure.AsEnumerable())
            {
                foreach (var glycan in glycans.AsEnumerable())
                {
                    string Peptide = "0";
                    //Category
                    string categoey = "3"; //1: Peptide + Glycan <10PPM; 2: No peptide but complete sequence; 3:Other
                    if (glycan.MatchWithPrecursorMW)
                    {
                        categoey = "2";
                    }
                    if (glycan.PeptideSequence != null && glycan.PeptideSequence!="")
                    {
                        Peptide = "1";
                        if (glycan.PPM <= 10)
                        {
                            categoey = "1";
                        }
                    }
                    string fuc = "1";
                    int CoreYPeaks =glycan.CoreIDPeak.Count ;
                    float CoreYScore = glycan.CoreScore;
                    if (!glycan.IUPACString.EndsWith("(DeHex-)HexNAc"))
                    {
                        CoreYPeaks = glycan.CoreIDPeak.Count- glycan.CoreIDPeak.Where(x => x.Item2.Contains("deHex")).ToList().Count;
                        foreach (var core in glycan.CoreIDPeak.Where(x => x.Item2.EndsWith("(DeHex-)HexNAc")))
                        {
                            CoreYScore -= core.Item1.Intensity;
                        }
                        fuc = "0";
                    }

                    string tmpStr = argGS.ScanInfo.ScanNo.ToString() + "," +
                                glycan.FullSequencedGlycanString + "," +
                                 Peptide + "," +
                                 fuc+","+
                                 glycan.NoOfTotalGlycan.ToString() + "," +
                                 glycan.PPM.ToString("0.000") + "," +
                                 CoreYPeaks.ToString() + "," +
                                 CoreYScore.ToString("0.0000") + "," +
                                 glycan.BranchIDPeak.Count.ToString() + "," +
                                 glycan.BranchScore.ToString("0.0000") + ",";

                   
                    var CorePeaks = glycan.CoreIDPeak.Where(x => x.Item2 == "HexNAc").ToList();
                    tmpStr += (CorePeaks.Count != 0 ? CorePeaks[0].Item1.Intensity.ToString("0.0000") : "0") +",";
                    CorePeaks = glycan.CoreIDPeak.Where(x => x.Item2 == "HexNAc-HexNAc").ToList();
                    tmpStr += (CorePeaks.Count != 0 ? CorePeaks[0].Item1.Intensity.ToString("0.0000") : "0") + ",";
                    CorePeaks = glycan.CoreIDPeak.Where(x => x.Item2 == "HexNAc-HexNAc-Hex").ToList();
                    tmpStr += (CorePeaks.Count != 0 ? CorePeaks[0].Item1.Intensity.ToString("0.0000") : "0") + ",";
                    CorePeaks = glycan.CoreIDPeak.Where(x => x.Item2 == "HexNAc-HexNAc-Hex-Hex").ToList();
                    tmpStr += (CorePeaks.Count != 0 ? CorePeaks[0].Item1.Intensity.ToString("0.0000") : "0") + ",";
                    CorePeaks = glycan.CoreIDPeak.Where(x => x.Item2 == "HexNAc-HexNAc-Hex-(Hex-)Hex").ToList();
                    tmpStr += (CorePeaks.Count != 0 ? CorePeaks[0].Item1.Intensity.ToString("0.0000") : "0") + ",";


                    if (glycan.MatchWithPrecursorMW)
                    {
                        tmpStr += "1,";
                    }
                    else
                    {
                         tmpStr += "0,";
                    }

                    tmpStr += categoey;

                    
                    if (!lstLearningMatrixStrings.Contains(tmpStr))
                    {
                        using (StreamWriter sw = new StreamWriter(argExportFile, true))
                        {
                            sw.WriteLine(tmpStr);
                        }
                        lstLearningMatrixStrings.Add(tmpStr);
                    }
                }
            }
        }

        public static void GenerateHtmlReport(GlycanSeqParameters argGlycanSeqParas)
        {
            Dictionary<string,int> dictTitleMapping = new Dictionary<string, int>();
            Dictionary<string,List<string>> dictRecordsByPeptides = new Dictionary<string, List<string>>(); //Key:Peptide squence, Value:List of record;
            //Html Result By Scan
            using (StreamWriter swScan = new StreamWriter(argGlycanSeqParas.ExportFolder + "\\Result_SortByScans.html"))
            {
                WriteHTMLTitle(swScan, argGlycanSeqParas);
                //Read Result
                using (StreamReader sr = new StreamReader(argGlycanSeqParas.ExportFolder+"\\Result.csv"))
                {
                    bool StartRecord = false;
                    List<string> lstRecords = new List<string>();
                    string CurrentScanNum = "0";
                    while (!sr.EndOfStream)
                    {
                        string tmpLine = sr.ReadLine();
                        string[] tmpAry = tmpLine.Split(',');
                        if (tmpAry[0] == "MSn_Scan")
                        {
                            StartRecord = true;
                            for (int i = 0; i < tmpAry.Length; i++)
                            {
                                dictTitleMapping.Add(tmpAry[i],i);   
                            }
                            continue;
                        }
                        if (!StartRecord)
                        {
                            continue;
                        }
                        
                        //Store for Peptide
                        string PeptideSeq = tmpAry[dictTitleMapping["Peptide"]]==""?"Unknown": tmpAry[dictTitleMapping["Peptide"]];
                        if (!dictRecordsByPeptides.ContainsKey(PeptideSeq))
                        {
                            dictRecordsByPeptides.Add(PeptideSeq,new List<string>());
                        }
                        dictRecordsByPeptides[PeptideSeq].Add(tmpLine);

                       
                        string ScanNum = tmpAry[dictTitleMapping["MSn_Scan"]];
                        if (CurrentScanNum != "0" && CurrentScanNum != ScanNum) //Export
                        {
                            swScan.WriteLine(WriteHTMLForScan(lstRecords, dictTitleMapping, argGlycanSeqParas.ExportFolder).ToString());
                            CurrentScanNum = ScanNum;
                            lstRecords.Clear();
                            lstRecords.Add(tmpLine);
                        }
                        else
                        {
                            lstRecords.Add(tmpLine);
                            CurrentScanNum = ScanNum;
                        }
                    }
                    if (lstRecords.Count != 0)
                    {
                        swScan.WriteLine(WriteHTMLForScan(lstRecords, dictTitleMapping, argGlycanSeqParas.ExportFolder).ToString());
                    }
                    swScan.WriteLine("<br></body>\n</html>");
                }
            }
            //Html Result By Peptide
            using (StreamWriter swPeptide = new StreamWriter(argGlycanSeqParas.ExportFolder + "\\Result_SortByPeptide.html"))
            {
                WriteHTMLTitle(swPeptide, argGlycanSeqParas);
                if (dictRecordsByPeptides.ContainsKey("-"))
                {
                    swPeptide.Write(WriteHTMLForPeptide(dictRecordsByPeptides["-"], dictTitleMapping, argGlycanSeqParas.ExportFolder).ToString());
                }
                foreach (string peptide in dictRecordsByPeptides.Keys)
                {
                    if (peptide == "-")
                    {
                        continue;
                    }
                    swPeptide.Write(WriteHTMLForPeptide(dictRecordsByPeptides[peptide], dictTitleMapping, argGlycanSeqParas.ExportFolder).ToString());
                }
                swPeptide.WriteLine("<br></body>\n</html>");
            }
        }

        private static void WriteHTMLTitle(StreamWriter argSW, GlycanSeqParameters argGlycanSeqParas)
        {
            
                argSW.WriteLine(
                    "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">");
                argSW.WriteLine("<html xmlns=\"http://www.w3.org/1999/xhtml\">");
                argSW.WriteLine("<head>");
                argSW.WriteLine("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />");
                argSW.WriteLine("<title>Glycoseq result for raw " + Path.GetFileName(argGlycanSeqParas.RawFilePath) +
                                "</title>\n</head>\n<body>");
                argSW.WriteLine("<Table border=\"1\">");

                argSW.WriteLine("<tr>\n\t<td>Raw File: </td>\n\t<td>" + argGlycanSeqParas.RawFilePath + "</td>\n</tr>");
                argSW.WriteLine("<tr>\n\t<td>Scan Range: </td>\n\t<td>" + argGlycanSeqParas.StartScan.ToString() + "~" +
                                argGlycanSeqParas.EndScan.ToString() + "</td>\n</tr>");
                argSW.WriteLine("<tr>\n\t<td>Use HCD/Sequencing HCD: </td>\n\t<td>" +
                                argGlycanSeqParas.UseHCD.ToString() + "/" + argGlycanSeqParas.SeqHCD.ToString() +
                                "</td>\n</tr>");

                string tmp = "";
                if (argGlycanSeqParas.UseGlycanList)
                {
                    argSW.WriteLine("<tr>\n\t<td>Glycan File: </td>\n\t<td>" + argGlycanSeqParas.GlycanFile +
                                    "</td>\n</tr>");
                }
                else
                {
                    tmp = "HexNAc:" + argGlycanSeqParas.NoHexNAc.ToString() + ";" +
                          "Hex:" + argGlycanSeqParas.NoHex.ToString() + ";" +
                          "Sia" + argGlycanSeqParas.NoSia.ToString() + ";" +
                          "DeHex:" + argGlycanSeqParas.NoDeHex.ToString() + ";";
                    argSW.WriteLine("<tr>\n\t<td>Glycan Composition: </td>\n\t<td>" + tmp + "</td>\n</tr>");
                }
                argSW.WriteLine("<tr>\n\t<td>N-Glycan/Human: </td>\n\t<td>" + argGlycanSeqParas.IsNGlycan.ToString() +
                                "/" + argGlycanSeqParas.IsHuman.ToString() + "</td>\n</tr>");

                argSW.WriteLine("<tr>\n\t<td>MS tolerance(PPM)/MS2 tolerance(Da): </td>\n\t<td>" +
                                argGlycanSeqParas.PrecursorTol.ToString() + "/" + argGlycanSeqParas.MSMSTol.ToString() +
                                "</td>\n</tr>");
                argSW.WriteLine("<tr>\n\t<td>Search Y1 without matched peptide: </td>\n\t<td>" +
                                argGlycanSeqParas.UnknownPeptideSearch.ToString() + "</td>\n</tr>");

                //Export
                argSW.WriteLine("<tr>\n\t<td>Get Top: </td>\n\t<td>" + argGlycanSeqParas.GetTopRank.ToString() +
                                "</td>\n</tr>");
                argSW.WriteLine("<tr>\n\t<td>Export Completed Only/Individual Result: </td>\n\t<td>" +
                                argGlycanSeqParas.PrecursorTol.ToString() + "/" + argGlycanSeqParas.MSMSTol.ToString() +
                                "</td>\n</tr>");

                //Sequencing Parameters
                argSW.WriteLine("<tr>\n\t<td>Top i Y1: </td>\n\t<td>" + argGlycanSeqParas.GetTopY1_i.ToString() +
                                "</td>\n</tr>");
                argSW.WriteLine("<tr>\n\t<td>Top k Core: </td>\n\t<td>" + argGlycanSeqParas.GetTopCore_k.ToString() +
                                "</td>\n</tr>");
                argSW.WriteLine("<tr>\n\t<td>Top l branch: </td>\n\t<td>" + argGlycanSeqParas.GetBranch_l.ToString() +
                                "</td>\n</tr>");
                argSW.WriteLine("<tr>\n\t<td>Max padding glycans: </td>\n\t<td>" +
                                argGlycanSeqParas.MaxPanddingGlycan.ToString() + "</td>\n</tr>");

                if (argGlycanSeqParas.PeptideFromMascotResult)
                {
                    argSW.WriteLine("<tr>\n\t<td>Peptide File Type </td>\n\t<td>Mascot</td>\n</tr>");
                    argSW.WriteLine("<tr>\n\t<td>Time Shift/Time Torelance: </td>\n\t<td>" +
                                    argGlycanSeqParas.MascotTimeShift.ToString() + "/" +
                                    argGlycanSeqParas.MascotTimeTolerance.ToString() + "</td>\n</tr>");
                }
                else
                {
                    argSW.WriteLine("<tr>\n\t<td>Peptide File Type </td>\n\t<td>Fasta</td>\n</tr>");
                    argSW.WriteLine("<tr>\n\t<td>Enzy/Missed cleavages: </td>\n\t<td>" + argGlycanSeqParas.Enzyme + "/" +
                                    argGlycanSeqParas.MissedCleavages.ToString() + "</td>\n</tr>");
                }

                argSW.WriteLine("<tr>\n\t<td>Peptide Mutation: </td>\n\t<td>" +
                                argGlycanSeqParas.PeptideMutation.ToString() + "</td>\n</tr>");
                argSW.WriteLine("<tr>\n\t<td>Peptide File: </td>\n\t<td>" + argGlycanSeqParas.FastaFile.ToString() +
                                "</td>\n</tr>");
                argSW.WriteLine("</Table>\n<br>\n<br>");

                if (argGlycanSeqParas.PeptideFromMascotResult)
                {
                    argSW.WriteLine("#: Peptide identified by Mascot\n<br>\n<br>");
                }
                argSW.WriteLine(
                    "Category: 1~ Glycan and peptide are found and within tolerance<br> 2~ Peptide is not found, but given a Y1 glycan can match with precursor mass<br> 3~ Peptide is not found and glycan is sequenced partial\n<br>\n<br>");
                argSW.Flush();
            
        }

        private static StringBuilder WriteHTMLForScan(List<string> argPeptideResult, Dictionary<string, int> argDictTitleMapping, string argFolder)
        {
            StringBuilder sbResult = new StringBuilder();
            string ScanNum = argPeptideResult[0].Split(',')[argDictTitleMapping["MSn_Scan"]];
            string PrecursorMonoMZ = argPeptideResult[0].Split(',')[argDictTitleMapping["Mono_Mz"]];
            string PrecursorCharge = argPeptideResult[0].Split(',')[argDictTitleMapping["Charge_State"]];
            sbResult.AppendLine("Scan :" + ScanNum + "<br>");
            sbResult.AppendLine("Precursor m/z :" + PrecursorMonoMZ + "(" + PrecursorCharge + "+)<br>");
            sbResult.AppendLine("<table border=\"1\" style=\"width:100%\">");
            sbResult.AppendLine("\t<tr>");
            sbResult.AppendLine("\t\t<td>Peptide Sequence</td>");
            sbResult.AppendLine("\t\t<td>Glycan Sequence</td>");
            sbResult.AppendLine("\t\t<td>Glycan Picture</td>");
            sbResult.AppendLine("\t\t<td>Category</td>");
            sbResult.AppendLine("\t\t<td>Category Probability</td>");
            sbResult.AppendLine("\t\t<td>Core + Branch +Append Glycan = Total Score</td>");
            sbResult.AppendLine("\t\t<td>Full Glycan Structure</td>");
            sbResult.AppendLine("\t\t<td>Append Glycan<br>(HexNAc-Hex-deHex-NeuAc-NeuGc)</td>");
            sbResult.AppendLine("\t\t<td>MatchWithPrecursorMW</td>");
            sbResult.AppendLine("\t\t<td>PPM</td>");
            sbResult.AppendLine("\t</tr>");

            foreach (string record in argPeptideResult)
            {
               
                int Exportidx = 0;
                List<string> tmpAry = record.Split(',').ToList();

                int IUPACCount = tmpAry.Count - argDictTitleMapping.Count;
                while (Exportidx <= IUPACCount)
                {
                    sbResult.AppendLine("\t<tr>");
                    if (tmpAry[argDictTitleMapping["Peptide"]] == "")
                    {
                        sbResult.AppendLine("\t\t<td>-</td>");
                    }
                    else
                    {
                        if (tmpAry[argDictTitleMapping["Identified_By_Mascot"]] == "No")
                        {
                            sbResult.AppendLine("\t\t<td>" + tmpAry[argDictTitleMapping["Peptide"]] + "   " + tmpAry[argDictTitleMapping["Peptide_Mod"]] + "#</td>");
                        }
                        else
                        {
                            sbResult.AppendLine("\t\t<td>" + tmpAry[argDictTitleMapping["Peptide"]] + "   " + tmpAry[argDictTitleMapping["Peptide_Mod"]] + "</td>");
                        }
                    }
                    sbResult.AppendLine("\t\t<td>" + tmpAry[argDictTitleMapping["IUPAC"] + Exportidx].Split('+')[0] + "</td>");
                    GlycansDrawer Draw;
                    string PicLocation = argFolder + "\\Pics\\" + tmpAry[argDictTitleMapping["IUPAC"] + Exportidx].Split('+')[0] + ".png";
                    if (!File.Exists(PicLocation))
                    {
                        Draw = new GlycansDrawer(tmpAry[argDictTitleMapping["IUPAC"] + Exportidx].Split('+')[0], false);
                        Image Pic = Draw.GetImage();
                        Pic.Save(PicLocation);
                        Pic.Dispose();
                        Pic = null;
                    }
                    if (tmpAry[argDictTitleMapping["Append_Glycan(HexNac-Hex-deHex-NeuAC-NeuGc)"]] == "")
                    {
                        sbResult.AppendLine("<td><img src=\".\\Pics\\" + tmpAry[argDictTitleMapping["IUPAC"] + Exportidx].Split('+')[0] + ".png\"/></td>");
                    }
                    else
                    {
                        string imgTmp = "<td align=\"center\"><img src=\".\\Pics\\" + tmpAry[argDictTitleMapping["IUPAC"] + Exportidx].Split('+')[0] + ".png\"/><br>";
                        imgTmp += "<img src=\".\\Pics\\bracket.jpg\"/><br>";
                        string[] argTmp = tmpAry[argDictTitleMapping["Append_Glycan(HexNac-Hex-deHex-NeuAC-NeuGc)"]].Split('-');
                        for (int k = 0; k < argTmp.Length; k++)
                        {
                            int monoCount = Convert.ToInt32(argTmp[k]);
                            if (monoCount != 0)
                            {
                                string monoLabel = "";
                                switch (k)
                                {
                                    case 0: //Hex
                                        monoLabel = "Hex";
                                        break;
                                    case 1: //HexNAc
                                        monoLabel = "HexNAc";
                                        break;
                                    case 2:
                                        monoLabel = "DeHex";
                                        break;
                                    case 3:
                                        monoLabel = "NeuAc";
                                        break;
                                    case 4:
                                        monoLabel = "NeuGc";
                                        break;
                                }
                                for (int j = 1; j <= monoCount; j++)
                                {
                                    imgTmp += "<img src=\".\\Pics\\" + monoLabel + ".png\"/>";
                                }
                            }
                        }
                        imgTmp += "</td>";
                        sbResult.AppendLine(imgTmp);
                    }

                    sbResult.AppendLine("\t\t<td>" + tmpAry[argDictTitleMapping["Category"]] + "</td>");
                    sbResult.AppendLine("\t\t<td>" + tmpAry[argDictTitleMapping["Category_Probability"]] + "</td>");
                    float totalScore = Convert.ToSingle(tmpAry[argDictTitleMapping["Core_Score"]]) + Convert.ToSingle(tmpAry[argDictTitleMapping["Branch_Score"]]) + Convert.ToSingle(tmpAry[argDictTitleMapping["Append_Glycan_Score"]]);
                    sbResult.AppendLine("\t\t<td>" + tmpAry[argDictTitleMapping["Core_Score"]] + "+" + tmpAry[argDictTitleMapping["Branch_Score"]] + tmpAry[argDictTitleMapping["Append_Glycan_Score"]] + "=" + totalScore.ToString("0.000") + "</td>");
                    sbResult.AppendLine("\t\t<td>" + tmpAry[argDictTitleMapping["Full_Glycan(HexNac-Hex-deHex-NeuAC-NeuGc)"]] + "</td>");
                    sbResult.AppendLine("\t\t<td>" + tmpAry[argDictTitleMapping["Append_Glycan(HexNac-Hex-deHex-NeuAC-NeuGc)"]] + "</td>");
                    sbResult.AppendLine("\t\t<td>" + tmpAry[argDictTitleMapping["MatchWithPrecursorMW"]] + "</td>");
                    sbResult.AppendLine("\t\t<td>" + tmpAry[argDictTitleMapping["PPM"]] + "</td>");
                    sbResult.AppendLine("\t</tr>");

                    Exportidx++;
                }
            }
            sbResult.AppendLine("</table><br><br>");
            return sbResult;
        }
        private static StringBuilder WriteHTMLForPeptide(List<string> argPeptideResult, Dictionary<string, int> argDictTitleMapping, string argFolder)
        {
            StringBuilder sbResult = new StringBuilder();
            string PeptideSequence = argPeptideResult[0].Split(',')[argDictTitleMapping["Peptide"]];
            
            sbResult.AppendLine("Peptide Sequence :" + PeptideSequence + "<br>");
            sbResult.AppendLine("<table border=\"1\" style=\"width:100%\">");
            sbResult.AppendLine("\t<tr>");
            sbResult.AppendLine("\t\t<td>Scan Number</td>");
            sbResult.AppendLine("\t\t<td>Y1</td>");
            sbResult.AppendLine("\t\t<td>PrecursorMono m/z(z)</td>");
            sbResult.AppendLine("\t\t<td>Glycan Sequence</td>");
            sbResult.AppendLine("\t\t<td>Glycan Picture</td>");
            sbResult.AppendLine("\t\t<td>Category</td>");
            sbResult.AppendLine("\t\t<td>Category Probability</td>");
            sbResult.AppendLine("\t\t<td>Core + Branch +Append Glycan = Total Score</td>");
            sbResult.AppendLine("\t\t<td>Full Glycan Structure</td>");
            sbResult.AppendLine("\t\t<td>Append Glycan<br>(HexNAc-Hex-deHex-NeuAc-NeuGc)</td>");
            sbResult.AppendLine("\t\t<td>MatchWithPrecursorMW</td>");
            sbResult.AppendLine("\t\t<td>PPM</td>");
            sbResult.AppendLine("\t</tr>");

            foreach (string record in argPeptideResult)
            {

                int Exportidx = 0;
                List<string> tmpAry = record.Split(',').ToList();

                int IUPACCount = tmpAry.Count - argDictTitleMapping.Count;
                while (Exportidx <= IUPACCount)
                {
                    sbResult.AppendLine("\t<tr>");

                    sbResult.AppendLine("\t\t<td>" + tmpAry[argDictTitleMapping["MSn_Scan"]] + "</td>");
                    sbResult.AppendLine("\t\t<td>" + tmpAry[argDictTitleMapping["Y1"]] + "</td>");
                    sbResult.AppendLine("\t\t<td>" + tmpAry[argDictTitleMapping["Mono_Mz"]] + "(" + tmpAry[argDictTitleMapping["Charge_State"]] + ")</td>");

                    sbResult.AppendLine("\t\t<td>" + tmpAry[argDictTitleMapping["IUPAC"] + Exportidx].Split('+')[0] + "</td>");
                    GlycansDrawer Draw;
                    string PicLocation = argFolder + "\\Pics\\" + tmpAry[argDictTitleMapping["IUPAC"] + Exportidx].Split('+')[0] + ".png";
                    if (!File.Exists(PicLocation))
                    {
                        Draw = new GlycansDrawer(tmpAry[argDictTitleMapping["IUPAC"] + Exportidx].Split('+')[0], false);
                        Image Pic = Draw.GetImage();
                        Pic.Save(PicLocation);
                        Pic.Dispose();
                        Pic = null;
                    }
                    if (tmpAry[argDictTitleMapping["Append_Glycan(HexNac-Hex-deHex-NeuAC-NeuGc)"]] == "")
                    {
                        sbResult.AppendLine("<td><img src=\".\\Pics\\" + tmpAry[argDictTitleMapping["IUPAC"] + Exportidx].Split('+')[0] + ".png\"/></td>");
                    }
                    else
                    {
                        string imgTmp = "<td align=\"center\"><img src=\".\\Pics\\" + tmpAry[argDictTitleMapping["IUPAC"] + Exportidx].Split('+')[0] + ".png\"/><br>";
                        imgTmp += "<img src=\".\\Pics\\bracket.jpg\"/><br>";
                        string[] argTmp = tmpAry[argDictTitleMapping["Append_Glycan(HexNac-Hex-deHex-NeuAC-NeuGc)"]].Split('-');
                        for (int k = 0; k < argTmp.Length; k++)
                        {
                            int monoCount = Convert.ToInt32(argTmp[k]);
                            if (monoCount != 0)
                            {
                                string monoLabel = "";
                                switch (k)
                                {
                                    case 0: //Hex
                                        monoLabel = "Hex";
                                        break;
                                    case 1: //HexNAc
                                        monoLabel = "HexNAc";
                                        break;
                                    case 2:
                                        monoLabel = "DeHex";
                                        break;
                                    case 3:
                                        monoLabel = "NeuAc";
                                        break;
                                    case 4:
                                        monoLabel = "NeuGc";
                                        break;
                                }
                                for (int j = 1; j <= monoCount; j++)
                                {
                                    imgTmp += "<img src=\".\\Pics\\" + monoLabel + ".png\"/>";
                                }
                            }
                        }
                        imgTmp += "</td>";
                        sbResult.AppendLine(imgTmp);
                    }

                    sbResult.AppendLine("\t\t<td>" + tmpAry[argDictTitleMapping["Category"]] + "</td>");
                    sbResult.AppendLine("\t\t<td>" + tmpAry[argDictTitleMapping["Category_Probability"]] + "</td>");
                    float totalScore = Convert.ToSingle(tmpAry[argDictTitleMapping["Core_Score"]]) + Convert.ToSingle(tmpAry[argDictTitleMapping["Branch_Score"]]) + Convert.ToSingle(tmpAry[argDictTitleMapping["Append_Glycan_Score"]]);
                    sbResult.AppendLine("\t\t<td>" + tmpAry[argDictTitleMapping["Core_Score"]] + "+" + tmpAry[argDictTitleMapping["Branch_Score"]] + tmpAry[argDictTitleMapping["Append_Glycan_Score"]] + "=" + totalScore.ToString("0.000") + "</td>");
                    sbResult.AppendLine("\t\t<td>" + tmpAry[argDictTitleMapping["Full_Glycan(HexNac-Hex-deHex-NeuAC-NeuGc)"]] + "</td>");
                    sbResult.AppendLine("\t\t<td>" + tmpAry[argDictTitleMapping["Append_Glycan(HexNac-Hex-deHex-NeuAC-NeuGc)"]] + "</td>");
                    sbResult.AppendLine("\t\t<td>" + tmpAry[argDictTitleMapping["MatchWithPrecursorMW"]] + "</td>");
                    sbResult.AppendLine("\t\t<td>" + tmpAry[argDictTitleMapping["PPM"]] + "</td>");
                    sbResult.AppendLine("\t</tr>");

                    Exportidx++;
                }
            }
            sbResult.AppendLine("</table><br><br>");
            return sbResult;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="argExportFolder"></param>
        /// <param name="argResults">Key: Peptide Seq //Tuple 1:Scan Number, 2:Glycan Sequence 3-1:CoreScore 3-2:BranceScore 3-3:IncompletedScore 4Full Glycan 5:RestGlycan 6:isCompleted 7:PPM</param>
        public static void OldGenerateHtmlReportSortByPeptide(string argFolder, Dictionary<string, List<Tuple<int, string, Tuple<double, double, double>, string, string, bool, double>>> argResults, COL.GlycoSequence.GlycanSeqParameters argGlycanSeqParas)
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
            sb.AppendLine("<tr>\n\t<td>Export Completed Only: </td>\n\t<td>" +
                            argGlycanSeqParas.CompletedOnly.ToString()  + "</td>\n</tr>");
            //sb.AppendLine("<tr>\n\t<td>Export Completed Only/Completed Reward: </td>\n\t<td>" +argGlycanSeqParas.CompletedOnly.ToString() + "/" + argGlycanSeqParas.CompletedReward.ToString() + "</td>\n</tr>");
            sb.AppendLine("<tr>\n\t<td>Peptide Mutation: </td>\n\t<td>" +
                             argGlycanSeqParas.PeptideMutation.ToString() + "</td>\n</tr>");
            sb.AppendLine("</Table>\n<br>\n<br>");
            sb.AppendLine("#: Peptide identified by Mascot\n<br>\n<br>");
            foreach (string pepStr in argResults.Keys)
            {
                if (pepStr == "+#" || pepStr == "+")
                {
                    sb.AppendLine("Peptide: Unknown" );
                }
                else
                {
                    sb.AppendLine("Peptide:" + pepStr);
                }
                sb.AppendLine("<table  border=\"1\">");
                sb.AppendLine("<tr><td>Scan No</td><td>Glycan Sequence</td><td>Glycan Picture</td><td>Core + Branch - Append Score = Total Score</td><td>Full Glycan</td><td>Append Glycan</td><td>MatchWithPrecursorMW</td><td>PPM</td></tr>");

                var tmpResultList = argResults[pepStr].OrderBy(z=>z.Item1).ThenByDescending(x => x.Item3.Item1 + x.Item3.Item2 + x.Item3.Item3).GroupBy(y => y.Item1).ToList();
                
                foreach(var Results in tmpResultList) //Groups
                {
                    foreach (Tuple<int, string, Tuple<double, double, double>, string, string, bool, double> rResult in Results.Take(argGlycanSeqParas.GetTopRank))
                    {

                        sb.AppendLine("<tr>");
                        sb.AppendLine("<td>" + rResult.Item1 + "</td>");
                        sb.AppendLine("<td>" + rResult.Item2 + "</td>");
                        if (rResult.Item5 == "")
                        {
                            sb.AppendLine("<td><img src=\".\\Pics\\" + rResult.Item2 + ".png\"/></td>");
                        }
                        else
                        {
                            string imgTmp = "<td align=\"center\"><img src=\".\\Pics\\" + rResult.Item2 + ".png\"/><br>";
                            imgTmp += "<img src=\".\\Pics\\bracket.jpg\"/><br>";
                            string[] argTmp = rResult.Item5.Split('-');
                            for (int k = 0; k < argTmp.Length; k++)
                            {
                                int monoCount = Convert.ToInt32(argTmp[k]);
                                if (monoCount!=0)
                                {
                                    string monoLabel = "";
                                    switch (k)
                                    {
                                        case 0: //Hex
                                            monoLabel = "Hex";
                                            break;
                                        case 1: //HexNAc
                                            monoLabel = "HexNAc";
                                            break;
                                        case 2:
                                            monoLabel = "DeHex";
                                            break;
                                        case 3:
                                            monoLabel = "NeuAc";
                                            break;
                                        case 4:
                                            monoLabel = "NeuGc";
                                            break;
                                    }
                                    for (int j = 1; j <= monoCount; j++)
                                    {
                                        imgTmp +=  "<img src=\".\\Pics\\" + monoLabel + ".png\"/>";
                                    }
                                }
                            }
                            imgTmp += "</td>";
                            sb.AppendLine(imgTmp);
                        }
                        double TotalScore = rResult.Item3.Item1 + rResult.Item3.Item2 + rResult.Item3.Item3;
                        sb.AppendLine("<td>" + rResult.Item3.Item1.ToString("0.00") + " + " +
                                      rResult.Item3.Item2.ToString("0.0000") + " - " +
                                      rResult.Item3.Item3.ToString("0.00") + " = " + TotalScore.ToString("0.00") +
                                      "</td>");
                        //sb.AppendLine("<td>" + rResult.Item3.Item2.ToString("0.00") + "</td>");
                        //sb.AppendLine("<td>" + rResult.Item3.Item3.ToString("0.00") + "</td>");
                        sb.AppendLine("<td>" + rResult.Item4.ToString() + "</td>");
                        if (rResult.Item5 == "")
                        {
                            sb.AppendLine("<td>-</td>");
                        }
                        else
                        {
                            sb.AppendLine("<td>" + rResult.Item5.ToString() + "</td>");
                        }

                        if (rResult.Item6)
                        {
                            sb.AppendLine("<td>Yes</td>");
                        }
                        else
                        {
                            sb.AppendLine("<td>No</td>");
                        }
                        //PPM
                        if (pepStr == "+#" || pepStr == "+")
                        {
                            sb.AppendLine("<td>-</td>");
                        }
                        else
                        {
                            sb.AppendLine("<td>" + rResult.Item7.ToString("0.00") + "</td>");    
                        }
                        sb.AppendLine("</tr>");
                    }
                }
                sb.AppendLine("</table><br>-----------------------------------------------------------<br>");
            }
            
            
            sb.AppendLine("</body>\n</html>");
            StreamWriter sw = new StreamWriter(argFolder + "\\Result_SortByPeptides.html");
            sw.WriteLine(sb.ToString());
            sw.Flush();
            sw.Close();
        }

        public static void OldGenerateHtmlReportSortByScan(COL.GlycoSequence.GlycanSeqParameters argGlycanSeqParas)
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
                argSW.WriteLine("<tr>\n\t<td>Export Completed Only: </td>\n\t<td>" + argGlycanSeqParas.CompletedOnly.ToString() + "</td>\n</tr>");
                //argSW.WriteLine("<tr>\n\t<td>Export Completed Only/Completed Reward: </td>\n\t<td>" +argGlycanSeqParas.CompletedOnly.ToString() + "/" + argGlycanSeqParas.CompletedReward.ToString() + "</td>\n</tr>");
                argSW.WriteLine("<tr>\n\t<td>Peptide Mutation: </td>\n\t<td>" +
                                 argGlycanSeqParas.PeptideMutation.ToString() + "</td>\n</tr>");
                argSW.WriteLine("</Table>\n<br>\n<br>");

                argSW.WriteLine("#: Peptide identified by Mascot\n<br>\n<br>");
                argSW.WriteLine("Category: 1~ Glycan and peptide are found and within tolerance 2~ Peptide is not found, but given a Y1 glycan can match with precursor mass 3~ Peptide is not found and glycan is sequenced partial\n<br>\n<br>");
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
        public static void OldHtmlFormatTempForEachScan(string argFolder, MSScan argScan, bool argExportIndividualReport, int argTopRank, List<Tuple<string, GlycanStructure, double>> argStructureResults)
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
            sbResult.AppendLine("\t\t<td>Category</td>");
            sbResult.AppendLine("\t\t<td>Category Probability</td>");
            sbResult.AppendLine("\t\t<td>Core + Branch +Append Glycan = Total Score</td>");
            sbResult.AppendLine("\t\t<td>Full Glycan Structure</td>");
            sbResult.AppendLine("\t\t<td>Append Glycan<br>(HexNAc-Hex-deHex-NeuAc-NeuGc)</td>");
            sbResult.AppendLine("\t\t<td>MatchWithPrecursorMW</td>");
            sbResult.AppendLine("\t\t<td>PPM</td>");
            sbResult.AppendLine("\t</tr>");

            foreach (Tuple<string, GlycanStructure, double> SeqResult in lstStructureResults)
            {
                sbResult.AppendLine("\t<tr>");
                if (SeqResult.Item2.PeptideSequence == "")
                {
                    sbResult.AppendLine("\t\t<td>-</td>");
                }
                else
                {
                    if (SeqResult.Item2.TargetPeptide != null && SeqResult.Item2.TargetPeptide.IdentifiedPeptide)
                    {
                        sbResult.AppendLine("\t\t<td>" + SeqResult.Item2.PeptideSequence + "   " + SeqResult.Item2.PeptideModificationString + "#</td>");
                    }
                    else
                    {
                        sbResult.AppendLine("\t\t<td>" + SeqResult.Item2.PeptideSequence + "   " + SeqResult.Item2.PeptideModificationString + "</td>");
                    }

                }
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
                float totalScore = SeqResult.Item2.CoreScore + SeqResult.Item2.BranchScore +SeqResult.Item2.InCompleteScore;

                if (SeqResult.Item2.RestGlycanString == "")
                {
                    sbResult.AppendLine("<td><img src=\".\\Pics\\" + SeqResult.Item2.IUPACString.ToString() +".png\"/></td>");
                }
                else
                {
                    string imgTmp = "<td align=\"center\"><img src=\".\\Pics\\" + SeqResult.Item2.IUPACString + ".png\"/><br>";
                    imgTmp += "<img src=\".\\Pics\\bracket.jpg\"/><br>";
                    string[] argTmp = SeqResult.Item2.RestGlycanString.Split('-');
                    for (int k = 0; k < argTmp.Length; k++)
                    {
                        int monoCount = Convert.ToInt32(argTmp[k]);
                        if (monoCount != 0)
                        {
                            string monoLabel = "";
                            switch (k)
                            {
                                case 0: //Hex
                                    monoLabel = "Hex";
                                    break;
                                case 1: //HexNAc
                                    monoLabel = "HexNAc";
                                    break;
                                case 2:
                                    monoLabel = "DeHex";
                                    break;
                                case 3:
                                    monoLabel = "NeuAc";
                                    break;
                                case 4:
                                    monoLabel = "NeuGc";
                                    break;
                            }
                            for (int j = 1; j <= monoCount; j++)
                            {
                                imgTmp += "<img src=\".\\Pics\\" + monoLabel + ".png\"/>";
                            }
                        }
                    }
                    imgTmp += "</td>";
                    sbResult.AppendLine(imgTmp);
                }
                if (SeqResult.Item2.SVMPrrdictedProbabilities.Count > 0)
                {
                    sbResult.AppendLine("\t\t<td>" + SeqResult.Item2.SVMPredictedLabel+"</td>");
                    sbResult.AppendLine("\t\t<td>" +SeqResult.Item2.SVMPrrdictedProbabilities[SeqResult.Item2.SVMPredictedLabel - 1].ToString("0.00") + "</td>");
                }
                else
                {
                    sbResult.AppendLine("\t\t<td>0</td>");
                    sbResult.AppendLine("\t\t<td>0.00</td>");
                }
                sbResult.AppendLine("\t\t<td>" + SeqResult.Item2.CoreScore.ToString("0.00") + "+" + SeqResult.Item2.BranchScore.ToString("0.00") + SeqResult.Item2.InCompleteScore.ToString("0.00") + "=" + totalScore.ToString("0.000") + "</td>");
                //sbResult.AppendLine("\t\t<td>" + SeqResult.Item2.BranchScore.ToString("0.000") + "</td>");
                //sbResult.AppendLine("\t\t<td>" + SeqResult.Item2.InCompleteScore.ToString("0.000") + "</td>");
                //Sequenced Glycan
                sbResult.AppendLine("\t\t<td>" + SeqResult.Item2.FullSequencedGlycanString.ToString() + "</td>");

                if (SeqResult.Item2.RestGlycanString == "")
                {
                    sbResult.AppendLine("\t\t<td>-</td>");
                }
                else
                {
                    sbResult.AppendLine("\t\t<td>" + SeqResult.Item2.RestGlycanString.ToString() + "</td>");
                }

                if (SeqResult.Item2.MatchWithPrecursorMW || SeqResult.Item2.IsCompleteByPrecursorDifference)
                {
                    sbResult.AppendLine("\t\t<td>Yes</td>");
                }
                else
                {
                    sbResult.AppendLine("\t\t<td>No</td>");
                }
                if (SeqResult.Item2.PeptideSequence == null || SeqResult.Item2.PeptideSequence=="")
                {
                    sbResult.AppendLine("\t\t<td>-</td>");
                }
                else
                {
                    sbResult.AppendLine("\t\t<td>" + SeqResult.Item2.PPM.ToString("0.00") + "</td>");    
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
                    if (Gt.MatchWithPrecursorMW)
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
        //public static void ExportToFolder_old(string argFolder, MSScan argScan, bool argExportIndividualReport, int argTopRank, List<Tuple<string, GlycanStructure, double>> argStructureResults)
        //{
        //    // GlycanSequencing GS = argGS;
        //    List<Tuple<string, GlycanStructure, double>> lstStructureResults = new List<Tuple<string, GlycanStructure, double>>();
        //    MSScan ScanInfo = argScan;
        //    argStructureResults.Sort((a, b) => -1 * a.Item3.CompareTo(b.Item3));

        //    List<string> lstExportedResult = new List<string>();

        //    double previousScore = 0;
        //    int ExportedRank = 0;
        //    foreach (Tuple<string, GlycanStructure, double> result in argStructureResults)
        //    {
        //        string key = result.Item1 + "-" + result.Item2.IUPACString;
        //        if (lstExportedResult.Contains(key))
        //        {
        //            continue;
        //        }
        //        if (previousScore != result.Item3)
        //        {
        //            ExportedRank = ExportedRank + 1;
        //            previousScore = result.Item3;
        //            if (ExportedRank > argTopRank)
        //            {
        //                continue;
        //            }
        //        }
        //        lstExportedResult.Add(key);
        //        lstStructureResults.Add(result);
        //    }
        //    lstStructureResults.Sort((a, b) => -1 * a.Item3.CompareTo(b.Item3));

        //    StringBuilder sbResult = new StringBuilder();

        //    sbResult.AppendLine("Scan :" + ScanInfo.ScanNo.ToString() + "(" + ScanInfo.Time.ToString("0.00") + " mins)<br>");
        //    sbResult.AppendLine("Precursor m/z :" + ScanInfo.ParentMZ.ToString("0.0000") + "(" + ScanInfo.ParentCharge.ToString() + "+)<br>");

        //    sbResult.AppendLine("<table border=\"1\" style=\"width:100%\">");
        //    sbResult.AppendLine("\t<tr>");
        //    sbResult.AppendLine("\t\t<td>Peptide Sequence</td>");
        //    sbResult.AppendLine("\t\t<td>Glycan Sequence</td>");
        //    sbResult.AppendLine("\t\t<td>Glycan Picture</td>");
        //    sbResult.AppendLine("\t\t<td>Score</td>");
        //    sbResult.AppendLine("\t\t<td>Completed Sequence?</td>");
        //    sbResult.AppendLine("\t</tr>");

        //    foreach (Tuple<string, GlycanStructure, double> SeqResult in lstStructureResults)
        //    {
        //        sbResult.AppendLine("\t<tr>");
        //        sbResult.AppendLine("\t\t<td>" + SeqResult.Item1 + "</td>");
        //        sbResult.AppendLine("\t\t<td>" + SeqResult.Item2.IUPACString + "</td>");

        //        //Picture 
        //        GlycansDrawer Draw;
        //        string PicLocation = argFolder + "\\Pics\\" + SeqResult.Item2.IUPACString.ToString() + ".png";
        //        if (!File.Exists(PicLocation))
        //        {
        //            Draw = new GlycansDrawer(SeqResult.Item2.IUPACString, false);
        //            Image Pic = GlycanImage.RotateImage(Draw.GetImage(), 180);
        //            Pic.Save(PicLocation);
        //            Pic.Dispose();
        //            Pic = null;
        //        }
        //        sbResult.AppendLine("<td><img src=\".\\Pics\\" + SeqResult.Item2.IUPACString.ToString() + ".png\"/></td>");
        //        sbResult.AppendLine("\t\t<td>" + SeqResult.Item3.ToString("0.000") + "</td>");
        //        if (SeqResult.Item2.IsCompleteSequenced)
        //        {
        //            sbResult.AppendLine("\t\t<td>Y</td>");
        //        }
        //        else
        //        {
        //            sbResult.AppendLine("\t\t<td>N</td>");
        //        }
        //        sbResult.AppendLine("\t</tr>");

        //    }
        //    sbResult.AppendLine("</table>");

        //    StreamWriter swResult = new StreamWriter(argFolder + "\\" + ScanInfo.ScanNo.ToString("00000") + ".txt");
        //    swResult.Write(sbResult.ToString());
        //    swResult.Close();


        //    //Detail Report
        //    if (!argExportIndividualReport)
        //    {
        //        return;
        //    }
        //    if (lstStructureResults.Count > 0)
        //    {
        //        if (!Directory.Exists(argFolder))
        //        {
        //            Directory.CreateDirectory(argFolder);
        //        }
        //        if (!Directory.Exists(argFolder + "\\Pics"))
        //        {
        //            Directory.CreateDirectory(argFolder + "\\Pics");
        //        }
        //        StringBuilder sb = new StringBuilder();
        //        sb.AppendLine("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">");
        //        sb.AppendLine("<html xmlns=\"http://www.w3.org/1999/xhtml\">");
        //        sb.AppendLine("<head>");
        //        sb.AppendLine("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />");
        //        sb.AppendLine("<title>Glycoseq result for scan " + ScanInfo.ScanNo.ToString() + "</title>\n</head>\n<body>");
        //        GlycansDrawer Draw;
        //        //List<GlycanStructure> ExactGlycanTreeMatch = new List<GlycanStructure>();

        //        //foreach (Tuple<string,GlycanStructure,float> result in lstStructureResults)
        //        //{
        //        //    GlycanStructure GTStructure = result.Item2;
        //        //    if (MassUtility.GetMassPPM(GTStructure.GlycanMonoMass,  GS.GlycanMonoMass) <=  GS.PrecursorTorelance)
        //        //    {
        //        //        ExactGlycanTreeMatch.Add(GTStructure);
        //        //    }
        //        //}
        //        //argGS.SequencedStructures.Sort(delegate(GlycanStructure T1, GlycanStructure T2)
        //        //{
        //        //    return T2.GlycanMonoMass.CompareTo(T1.GlycanMonoMass) != 0 ? T2.GlycanMonoMass.CompareTo(T1.GlycanMonoMass) : T2.Score.CompareTo(T1.Score)
        //        //      ;
        //        //});

        //        sb.AppendLine("Precursor m/z :" + ScanInfo.ParentMZ.ToString("0.0000") + "(" + ScanInfo.ParentCharge.ToString() + "+)<br>");
        //        //sb.AppendLine("Predicted Y1:" +  GS.Y1MZ.ToString("0.0000") + "<br>");
        //        //sb.AppendLine("Predicted GlycanMonoMass:" + GS.GlycanMonoMass.ToString("0.000") + "<br>");
        //        sb.AppendLine("Structure match with precursor will highlight in red");
        //        sb.AppendLine("<table  border=\"1\">");
        //        sb.AppendLine("<tr>");
        //        sb.AppendLine("<td>Id</td>");
        //        sb.AppendLine("<td>Peptide</td>");
        //        sb.AppendLine("<td>GlycanMass</td>");
        //        sb.AppendLine("<td>Score</td>");
        //        sb.AppendLine("<td>Image</td>");
        //        sb.AppendLine("<td>IUPAC</td>");
        //        sb.AppendLine("</tr>");
        //        //if (ExactGlycanTreeMatch.Count == 0)
        //        //{
        //        //    sb.AppendLine("<tr><td colspan=\"5\">No glycan structure match with precursor</td></tr>");
        //        //}
        //        int idx = 0;

        //        foreach (Tuple<string, GlycanStructure, double> result in lstStructureResults)
        //        {

        //            GlycanStructure Gt = result.Item2;

        //            string BGColor = "#FFFFFF"; //White
        //            if (Gt.IsCompleteSequenced)
        //            {
        //                BGColor = "#FF0000"; //Red
        //            }
        //            try
        //            {
        //                string PicLocation = argFolder + "\\Pics\\" + Gt.IUPACString.ToString() + ".png";
        //                if (!File.Exists(PicLocation))
        //                {
        //                    Draw = new GlycansDrawer(Gt.IUPACString, false);
        //                    Image Pic = GlycanImage.RotateImage(Draw.GetImage(), 180);
        //                    Pic.Save(PicLocation);
        //                    Pic.Dispose();
        //                    Pic = null;
        //                }
        //                sb.AppendLine("<tr>");
        //                sb.AppendLine("<td bgcolor=\"" + BGColor + "\"><a href=\"#" + (idx + 1).ToString() + "\">" + (idx + 1).ToString() + "</a></td>");
        //                sb.AppendLine("<td bgcolor=\"" + BGColor + "\">" + result.Item1 + "</td>");
        //                sb.AppendLine("<td bgcolor=\"" + BGColor + "\">" + Gt.GlycanMonoMass.ToString() + "</td>");
        //                sb.AppendLine("<td bgcolor=\"" + BGColor + "\">" + Gt.Score.ToString("0.0000") + "</td>");
        //                sb.AppendLine("<td><img src=\".\\Pics\\" + Gt.IUPACString.ToString() + ".png\"/></td>");
        //                sb.AppendLine("<td colspan=\"2\" bgcolor=\"" + BGColor + "\">" + Gt.IUPACString + "</td>");
        //                sb.AppendLine("</tr>");
        //            }
        //            catch
        //            {
        //                sb.AppendLine("<tr>");
        //                sb.AppendLine("<td bgcolor=\"" + BGColor + "\"><a href=\"#" + (idx + 1).ToString() + "\">" + (idx + 1).ToString() + "</a></td>");
        //                sb.AppendLine("<td bgcolor=\"" + BGColor + "\">" + result.Item1 + "</td>");
        //                sb.AppendLine("<td bgcolor=\"" + BGColor + "\">" + Gt.GlycanMonoMass.ToString() + "</td>");
        //                sb.AppendLine("<td bgcolor=\"" + BGColor + "\">" + Gt.Score.ToString("0.0000") + "</td>");
        //                sb.AppendLine("<td></td>");
        //                sb.AppendLine("<td colspan=\"2\" bgcolor=\"" + BGColor + "\">" + Gt.IUPACString + "</td>");
        //                sb.AppendLine("</tr>");
        //            }
        //            idx++;
        //        }
        //        sb.AppendLine("</table><br><br>");

        //        //Detail Summary table

        //        idx = 0;
        //        foreach (Tuple<string, GlycanStructure, double> result in lstStructureResults)
        //        {

        //            GlycanStructure Gt = result.Item2;

        //            sb.AppendLine("<table  border=\"1\">");
        //            sb.AppendLine("<tr>");
        //            sb.AppendLine("<td><a name=\"" + idx.ToString() + "\">Id: " + idx.ToString() + "</a></td>");
        //            sb.AppendLine("<td>GlycanMass:" + Gt.GlycanMonoMass.ToString() + "</td>");
        //            sb.AppendLine("<td>Score:" + Gt.Score.ToString("0.000") + "</td>");
        //            sb.AppendLine("</tr>");
        //            sb.AppendLine("<tr>");
        //            sb.AppendLine("<td>" + result.Item1 + "</td>");
        //            sb.AppendLine("<td><img src=\".\\Pics\\" + Gt.IUPACString.ToString() + ".png\"/></td>");
        //            sb.AppendLine("<td>" + Gt.IUPACString + "</td>");
        //            sb.AppendLine("</tr>");
        //            sb.AppendLine("</table><br>");


        //            List<GlycanTreeNode> lstFragement = Gt.TheoreticalFragment;
        //            lstFragement.Sort(delegate(GlycanTreeNode t1, GlycanTreeNode t2) { return GlycanMass.GetGlycanMasswithCharge(t1.GlycanType, Gt.Charge).CompareTo(GlycanMass.GetGlycanMasswithCharge(t2.GlycanType, Gt.Charge)); });


        //            List<MSPoint> msp = new List<MSPoint>();
        //            foreach (MSPeak p in ScanInfo.MSPeaks)
        //            {
        //                msp.Add(new MSPoint(p.MonoMass, p.MonoIntensity));
        //            }

        //            Image AnnotatedSpectrum = GlycanImage.GetAnnotatedImage(result.Item1, ScanInfo, msp, Gt);
        //            System.IO.MemoryStream mss = new System.IO.MemoryStream();
        //            System.IO.FileStream fs = new System.IO.FileStream(argFolder + "\\Pics\\AnnotatedSpectrum_" + ScanInfo.ScanNo + "_" + (idx + 1).ToString("000") + ".png", System.IO.FileMode.Create, System.IO.FileAccess.ReadWrite);


        //            AnnotatedSpectrum.Save(mss, System.Drawing.Imaging.ImageFormat.Png);
        //            byte[] matriz = mss.ToArray();
        //            fs.Write(matriz, 0, matriz.Length);

        //            mss.Close();
        //            fs.Close();
        //            AnnotatedSpectrum.Dispose();
        //            AnnotatedSpectrum = null;
        //            sb.AppendLine("<a href=\".\\Pics\\AnnotatedSpectrum_" + ScanInfo.ScanNo + "_" + (idx + 1).ToString("000") + ".png\" target=\"_blank\"><img src=\".\\Pics\\AnnotatedSpectrum_" + ScanInfo.ScanNo + "_" + (idx + 1).ToString("000") + ".png\" height=\"450\" width=\"600\"/></a>");

        //            //Fragement table
        //            sb.AppendLine("<table  border=\"1\">");
        //            sb.AppendLine("<tr><td>Structure</td><td>Identified MZ</td><td>Identified Intensity</td></tr>");


        //            foreach (GlycanTreeNode t in Gt.Root.FetchAllGlycanNode().OrderBy(o => o.IDMass).ToList())
        //            {

        //                sb.AppendLine("<tr>");
        //                sb.AppendLine("<td>" + t.IUPACFromRoot + "</td>");
        //                sb.AppendLine("<td>" + t.IDMass.ToString("0.0000") + "</td>");
        //                sb.AppendLine("<td>" + t.IDIntensity.ToString("0.0000") + "</td>");
        //                sb.AppendLine("</tr>");
        //            }

        //            //if (lstFragement.Count >= lstIDPeaksMz.Count)
        //            //{
        //            //    for (int j = 0; j < lstFragement.Count; j++)
        //            //    {
        //            //        sb.AppendLine("<tr>");
        //            //        // sb.AppendLine("<td>" + (GlycanMass.GetGlycanMasswithCharge(lstFragement[j].GlycanType, Gt.Charge) + Gt.Y1.Mass- GlycanMass.GetGlycanMasswithCharge(Glycan.Type.HexNAc, Gt.Charge)).ToString() + "</td>");

        //            //        //string PicLocation = argFolder + "\\Pics\\" + lstFragement[j].GetIUPACString() + ".png";
        //            //        //if (!File.Exists(PicLocation))
        //            //        //{
        //            //        //    Draw = new GlycansDrawer(lstFragement[j].GetIUPACString(), false);
        //            //        //    Image tmpImg = Draw.GetImage();
        //            //        //    tmpImg.Save(PicLocation);
        //            //        //    tmpImg.Dispose();
        //            //        //    tmpImg = null;
        //            //        //}

        //            //        sb.AppendLine("<td>" + lstFragement[j].IUPAC + "</td>");

        //            //        if (j < lstIDPeaksMz.Count)
        //            //        {
        //            //            sb.AppendLine("<td>" + lstIDPeaksMz[j].ToString("0.0000") + "</td>");
        //            //            sb.AppendLine("<td>" + lstIDPeaksInt[j].ToString("0.0000") + "</td>");
        //            //        }
        //            //        else
        //            //        {
        //            //            sb.AppendLine("<td>-</td>");
        //            //            sb.AppendLine("<td>-</td>");
        //            //        }
        //            //        sb.AppendLine("</tr>");
        //            //    }
        //            //}
        //            //else
        //            //{
        //            //    for (int j = 0; j < lstIDPeaksMz.Count; j++)
        //            //    {
        //            //        sb.AppendLine("<tr>");

        //            //        if (j < lstFragement.Count)
        //            //        {
        //            //            //sb.AppendLine("<td>" + (GlycanMass.GetGlycanMasswithCharge(lstFragement[j].GlycanType, Gt.Charge) + Gt.Y1.Mass - GlycanMass.GetGlycanMasswithCharge(Glycan.Type.HexNAc, Gt.Charge)).ToString() + "</td>");
        //            //            //Draw = new GlycansDrawer(lstFragement[j].GetIUPACString(), true);
        //            //            //Image tmpImg = Draw.GetImage();
        //            //            //tmpImg.Save(argFolder + "\\Pics\\SubStructure_" + idx.ToString("000") + "-F" + (j + 1).ToString("00") + ".png");
        //            //            //tmpImg.Dispose();
        //            //            //tmpImg = null;
        //            //            //sb.AppendLine("<td><img src=\".\\Pics\\SubStructure_" + idx.ToString("000") + "-F" + (j + 1).ToString("00") + ".png\"/>" + "</td>");
        //            //            sb.AppendLine("<td>" + lstFragement[j].IUPAC + "</td>");
        //            //        }
        //            //        else
        //            //        {
        //            //            // sb.AppendLine("<td>-</td>");
        //            //            sb.AppendLine("<td>-</td>");
        //            //        }
        //            //        sb.AppendLine("<td>" + lstIDPeaksMz[j].ToString() + "</td>");
        //            //        sb.AppendLine("<td>" + lstIDPeaksInt[j].ToString() + "</td>");
        //            //        sb.AppendLine("</tr>");
        //            //    }
        //            //}
        //            sb.AppendLine("</table><br>-----------------------------------------------------------<br>");
        //            idx++;
        //            GC.Collect();
        //        }

        //        sb.AppendLine("</body>\n</html>");
        //        StreamWriter sw = new StreamWriter(argFolder + "\\Scan" + ScanInfo.ScanNo.ToString() + ".htm");
        //        sw.Write(sb.ToString());
        //        sw.Flush();
        //        sw.Close();
        //    }
        //}
       
    }
}
