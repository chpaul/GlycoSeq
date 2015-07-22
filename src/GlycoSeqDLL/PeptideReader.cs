using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
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
            List<TargetPeptide> lstTargetPeptides = new List<TargetPeptide>();
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
                        if (Mod != "")
                        {
                            string[] ModAry = Mod.Split(';');
                            Dictionary<string,List<int>> dictModifications = new Dictionary<string, List<int>>();
                            for (int i = 0; i < ModAry.Length; i++)
                            {
                                string modType = ModAry[i].Split(':')[1].Trim();
                                int modLocIdx = Convert.ToInt32(ModAry[i].Split(':')[0]) -1;
                                if (!dictModifications.ContainsKey(modType))
                                {
                                    dictModifications.Add(modType,new List<int>());
                                }
                                dictModifications[modType].Add(modLocIdx);
                            }
                            
                            //Add Native
                            TargetPeptide NativePeptide = (TargetPeptide)tPep.Clone();
                            foreach (string modKey in dictModifications.Keys)
                            {
                                NativePeptide.PeptideMass -= dictVariableModMass[modKey]*dictModifications[modKey].Count;
                            }
                            lstTargetPeptides.Add(NativePeptide);

                            if (dictModifications.Keys.Count >1)
                            {
                                List<string> lstMod = new List<string>();
                                List<List<int>> list = new List<List<int>>();
                                foreach (string modKey in dictModifications.Keys)
                                {
                                    lstMod.Add(modKey);
                                    list.Add(new List<int>());
                                    for (int i = 0; i <= dictModifications[modKey].Count; i++)
                                    {
                                        list[list.Count - 1].Add(i);
                                    }
                                }
                                var combinations = GenerateCombinations(list);
                                foreach (var combination in combinations)
                                {
                                    TargetPeptide ModifiedPeptide = (TargetPeptide)NativePeptide.Clone();
                                    for (int i = 0; i < combination.Count; i++)
                                    {
                                        if (combination[i] == 0)
                                        {
                                            continue;
                                        }
                                        ModifiedPeptide.PeptideMass += dictVariableModMass[lstMod[i]]*combination[i];
                                        ModifiedPeptide.Modifications.Add(lstMod[i],combination[i]);
                                    }
                                    if (ModifiedPeptide.PeptideMass == NativePeptide.PeptideMass)
                                    {
                                        continue;
                                    }
                                    lstTargetPeptides.Add(ModifiedPeptide);
                                }
                            }
                            else  //Only one modification
                            {
                                string ModType = dictModifications.Keys.ToArray()[0];
                                int ModCount = dictModifications[ModType].Count;

                                for (int i = 1; i <= ModCount; i++)
                                {
                                    TargetPeptide ModifiedPeptide = (TargetPeptide)NativePeptide.Clone();
                                    ModifiedPeptide.PeptideMass += dictVariableModMass[ModType] * i;
                                    ModifiedPeptide.Modifications.Add(ModType,i);
                                    
                                    if (ModifiedPeptide.PeptideMass == NativePeptide.PeptideMass)
                                    {
                                        continue;
                                    }
                                    lstTargetPeptides.Add(ModifiedPeptide);
                                }
                            }
                        }
                        else
                        {
                            lstTargetPeptides.Add(tPep);
                        }
                        break;
                }
            } while (!sr.EndOfStream);
            return lstTargetPeptides;
        }

        //Combination
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
