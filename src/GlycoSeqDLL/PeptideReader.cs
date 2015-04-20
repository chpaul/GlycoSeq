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
        public static List<TargetPeptide> GetCandidatePeptides(string argFile)
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
    }
}
