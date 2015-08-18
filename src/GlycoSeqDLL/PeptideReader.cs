using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.IO;
using COL.GlycoLib;
namespace COL.GlycoSequence
{
    public static class PeptideReader
    {
        public static List<TargetPeptide> GetCandidatePeptidesFromFile(string argFile)
        {
            List<TargetPeptide> lstTargetPeptides = new List<TargetPeptide>();
            StreamReader sr = new StreamReader(argFile);
            string[] tmpAry = sr.ReadLine().Split(',');
            Dictionary<string, int> TitleMapping = new Dictionary<string, int>();
            for (var i = 0; i < tmpAry.Length; i++)
            {
                TitleMapping.Add(tmpAry[i], i);
            }

            do
            {
                tmpAry = sr.ReadLine().Split(',');

                if (TitleMapping.Count == 1)  //Only has peptide
                {
                    lstTargetPeptides.Add(new TargetPeptide(tmpAry[TitleMapping["Peptide_Sequence"]]));
                }
                else if (TitleMapping.Count == 4) //has peptide, protein, start time, end time
                {
                    lstTargetPeptides.Add(new TargetPeptide(
                                                                tmpAry[TitleMapping["Peptide_Sequence"]],
                                                                tmpAry[TitleMapping["Protein_Name"]],
                                                                Convert.ToSingle(tmpAry[TitleMapping["Start_Search_Time_(Mins)"]]),
                                                                Convert.ToSingle(tmpAry[TitleMapping["End_Search_Time_(Mins)"]])));
                }
                else
                {
                    lstTargetPeptides.Add(new TargetPeptide(
                                                        tmpAry[TitleMapping["Peptide_Sequence"]],
                                                        tmpAry[TitleMapping["Protein_Name"]],
                                                        Convert.ToSingle(tmpAry[TitleMapping["Peptide_Mass"]]),
                                                        Convert.ToSingle(tmpAry[TitleMapping["Start_Search_Time_(Mins)"]]),
                                                        Convert.ToSingle(tmpAry[TitleMapping["End_Search_Time_(Mins)"]])));
                }
            } while (!sr.EndOfStream);
            sr.Close();
            return lstTargetPeptides;
        }


        public static List<TargetPeptide> GetCandidatePeptidesFromMascotProteinIDExtractorResult(string argFile)
        {
            HashSet<TargetPeptide> lstTargetPeptides = new HashSet<TargetPeptide>();
            StreamReader sr = new StreamReader(argFile);
            string sectionSwitch = "";
            Dictionary<string, float> dictVariableModMass = new Dictionary<string, float>();
            Dictionary<string, float> dictFixModMass = new Dictionary<string, float>();
            int tmpInt = 0;
            do
            {
                string[] argTmp = sr.ReadLine().Split(',');
                if (argTmp.Length == 0) continue;

                if (argTmp[0].StartsWith("Fixed modifications"))
                {
                    sectionSwitch = "Fix";
                    continue;
                }
                else if (argTmp[0].StartsWith("Variable modifications"))
                {
                    sectionSwitch = "Variable";
                    continue;
                }
                else if(argTmp[0].StartsWith("Protein_Name"))
                {
                    sectionSwitch = "Protein";
                    continue;
                }

                switch (sectionSwitch)
                {
                    case "Fix":
                        if (int.TryParse(argTmp[0], out tmpInt))
                        {
                            dictFixModMass.Add(argTmp[1].Trim(),Convert.ToSingle(argTmp[2]));
                        }
                        break;
                    case "Variable":
                        if (int.TryParse(argTmp[0], out tmpInt))
                        {
                            dictVariableModMass.Add(argTmp[1].Trim(), Convert.ToSingle(argTmp[2]));
                        }
                        break;
                    case "Protein":
                        List<TargetPeptide> tmpPeptides = new List<TargetPeptide>();
                        string ProteinName = argTmp[0];
                        string PeptideSequence = argTmp[1];
                        float PeptideMass = Convert.ToSingle(argTmp[2]);
                        string NTerm = argTmp[14];
                        string CTerm = argTmp[15];
                        string Mod = argTmp[16];
                        float StartTime =Convert.ToSingle(argTmp[9]);
                        float EndTime = Convert.ToSingle(argTmp[11]);
                        TargetPeptide tPep = new TargetPeptide(PeptideSequence, ProteinName, PeptideMass, StartTime,EndTime);
                        tPep.AminoAcidBefore = NTerm;
                        tPep.AminoAcidAfter = CTerm;
                        tPep.IdentifiedPeptide = true;

                        Regex sequon = new Regex("N[ARNDCEQGHILKMFSTWYV][S|T]", RegexOptions.IgnoreCase);  //NXS NXT  X!=P
                        string FullSeq = tPep.AminoAcidBefore + tPep.PeptideSequence + tPep.AminoAcidAfter;

                        if (sequon.Matches(FullSeq).Count ==0)
                        {
                            continue;
                        }

                        //Modification Result
                        //For any peptide from the MASCOT result,
                        //AAAAAN(1)XS/TAAAAAN(2)AMAACAAK,
                        //For N residue,
                        //i. If N(1) is not detected to be deamidated, then exclude this peptide from candidate list.
                        //ii. If N(1) is deamidated, no matter the N(2) deamidated or not, then take the mass from “pep_calc_mr” column and substract 0.9840 as the peptide back bone mass.
                        //iii. If there are multiple N(1)s (glycosylation sites) detected, x of the N(1)s are deamidated, then take the mass from “pep_calc_mr” column and substract x *0.9840 as the peptide back bone mass.
                        //iv. For N(2) deamidation, please treat it as a variable modification. (If only deamidated N(2) is detected, then create an native one (-0.9840). If only native N(2) is detected, then create a deamidated one(+0.9840))
                        //For M residue,
                        //i. If M is detected as native M, then create a variable modification of O (+15.9949).
                        //ii. If M is detected as oxidation form, then create a variable modification of native form (-15.9949).
                        //iii. If M is detected as Carbamidomethylated M, then create the native form (-57.0215) and the oxidated form 
                        //(-57.0215+15.9949).
                        //For C residue,
                        //Always keep Carbamidomethylated C as a fixed modification.
                        //Carbamidomethyl (M)
                        //Deamidated(N) (N)
                        //Oxidation (M)

                        if (Mod != "")
                        {
                            string[] ModAry = Mod.Split(';');
                            Dictionary<string,int> dictModifications = new Dictionary<string, int>();
                            List<int> lstDeamidatedIdx= new List<int>();
                            for (int i = 0; i < ModAry.Length; i++)
                            {
                                string modType = ModAry[i].Split(':')[1].Trim();
                                int modLocIdx = Convert.ToInt32(ModAry[i].Split(':')[0]);
                                if (!dictModifications.ContainsKey(modType))
                                {
                                    dictModifications.Add(modType,0);
                                }
                                dictModifications[modType] += 1;
                                if (modType == "Deamidated(N) (N)" )
                                {
                                    foreach (Match match in sequon.Matches(tPep.PeptideSequence+tPep.AminoAcidAfter))
                                    {
                                        if (match.Index == modLocIdx)
                                        {
                                            lstDeamidatedIdx.Add(modLocIdx);
                                        }
                                    }
                                }
                            }
                            tPep.Modifications = dictModifications;
                            if (lstDeamidatedIdx.Count==0) // Has Sequon but no deamidated modification 
                            {
                                continue;
                            }

                            #region N residue
                            int NumOfN = tPep.PeptideSequence.Count(f => f == 'N');
                           

                 
                            TargetPeptide NativePeptide = (TargetPeptide) tPep.Clone();
                            NativePeptide.IdentifiedPeptide = false;
                            foreach (string key in NativePeptide.Modifications.Keys.ToList())
                            {
                                NativePeptide.PeptideMass -= dictVariableModMass[key]*NativePeptide.Modifications[key];
                                NativePeptide.Modifications[key] -= NativePeptide.Modifications[key];
                            }


                           for (int i = 0; i <= NumOfN - lstDeamidatedIdx.Count; i++)
                           {
                               TargetPeptide deAmidatedPeptide = (TargetPeptide)NativePeptide.Clone();
                               deAmidatedPeptide.IdentifiedPeptide = false;
                               deAmidatedPeptide.PeptideMass += dictVariableModMass["Deamidated(N) (N)"] * i;
                               deAmidatedPeptide.Modifications["Deamidated(N) (N)"] += i;
                               tmpPeptides.Add(deAmidatedPeptide);
                           }

                            #endregion
                            #region M Residue
                            int NumOfM = tPep.PeptideSequence.ToUpper().Count(f => f == 'M');
                            if (NumOfM!=0)
                            {
                                    List<List<int>> list = new List<List<int>>();
                                    list.Add(Enumerable.Range(0, NumOfM+1).ToList()); //Native
                                    list.Add(Enumerable.Range(0, NumOfM+1).ToList()); //Oxidation
                                    if (dictModifications.ContainsKey("Carbamidomethyl (M)"))
                                    {
                                        list.Add(Enumerable.Range(0, NumOfM+1).ToList());
                                    }
                                    else
                                    {
                                        list.Add(Enumerable.Range(0, 1).ToList());
                                    }
                                    var combinations = GenerateCombinations(list);
                                    List<TargetPeptide> ModedPeptides = new List<TargetPeptide>();
                                    foreach (var combination in combinations)
                                    {
                                        if (combination[0] + combination[1] +combination[2] != NumOfM)
                                        {
                                            continue;
                                        }
                                        foreach (TargetPeptide ModifiedPeptide in tmpPeptides)
                                        {
                                            TargetPeptide newPeptide = (TargetPeptide) ModifiedPeptide.Clone();
                                            //Combination : 0:Native, 1: Oxidation, 2: Carbamidomethyl
                                            if (combination[1] != 0)
                                            {
                                                newPeptide.PeptideMass += dictVariableModMass["Oxidation (M)"] * combination[1];
                                                if (!  newPeptide.Modifications.ContainsKey("Oxidation (M)"))
                                                {
                                                    newPeptide.Modifications.Add("Oxidation (M)", 0);
                                                }
                                                newPeptide.Modifications["Oxidation (M)"] = combination[1];
                                            }
                                            if (combination[2] != 0)
                                            {
                                                newPeptide.PeptideMass += dictVariableModMass["Carbamidomethyl (M)"] * combination[2];
                                                if (!newPeptide.Modifications.ContainsKey("Carbamidomethyl (M)"))
                                                {
                                                    newPeptide.Modifications.Add("Carbamidomethyl (M)", 0);
                                                }
                                                newPeptide.Modifications["Carbamidomethyl (M)"] = combination[2];
                                            }
                                            ModedPeptides.Add(newPeptide);
                                        }
                                    }
                                    tmpPeptides.AddRange(ModedPeptides);
                            }
                            #endregion
                        }
                        else // Has Sequon but no deamidated modification 
                        {
                            continue;
                        }
                        //Remove Zero Modifications 
                        lstTargetPeptides.Add(tPep);
                        foreach (TargetPeptide t in tmpPeptides)
                        {
                            List<string> RemoveKey = new List<string>();
                            foreach (string key in t.Modifications.Keys)
                            {
                                if (t.Modifications[key] == 0)
                                {
                                    RemoveKey.Add(key);
                                }
                            }
                            foreach (string key in RemoveKey)
                            {
                                t.Modifications.Remove(key);
                            }
                            if (t.PeptideMass == tPep.PeptideMass && t.Modifications.Count == tPep.Modifications.Count)
                            {
                                continue;
                            }
                            lstTargetPeptides.Add(t);
                        }

                        break;
                }
            } while (!sr.EndOfStream);
            sr.Close();
            return lstTargetPeptides.ToList();
        }

        //Combination
        //http://stackoverflow.com/questions/545703/combination-of-listlistint
        private static List<List<T>> GenerateCombinations<T>(List<List<T>> collectionOfSeries)
        {
            List<List<T>> generatedCombinations =
                collectionOfSeries.Take(1)
                                  .FirstOrDefault()
                                  .Select(i => (new T[] { i }).ToList())
                                  .ToList();

            foreach (List<T> series in collectionOfSeries.Skip(1))
            {
                generatedCombinations =
                    generatedCombinations
                          .Join(series as List<T>,
                                combination => true,
                                i => true,
                                (combination, i) =>
                                {
                                    List<T> nextLevelCombination =
                                        new List<T>(combination);
                                    nextLevelCombination.Add(i);
                                    return nextLevelCombination;
                                }).ToList();

            }

            return generatedCombinations;
        }
    }
}
