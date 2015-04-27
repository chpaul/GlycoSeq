using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COL.ProtLib;
namespace COL.GlycoSequence
{
    public static class Peptides
    {
        public static List<Tuple<float, string>> ReadFastaFile(string argFastaFile)
        {
            List<ProteinInfo> pInfo = FastaReader.ReadFasta(argFastaFile);
            List<Tuple<float, string>> lstPeptideList = new List<Tuple<float, string>>();
            AminoAcidMass AAMass = new AminoAcidMass();
            foreach (ProteinInfo p in pInfo)
            {
                float Mass = AAMass.GetMonoMW(p.Sequence, true);
                lstPeptideList.Add(new Tuple<float, string>(Mass, p.Sequence));
            }
            lstPeptideList.Sort((a,b)=>a.Item1.CompareTo(b.Item1));
            return lstPeptideList;
        }

        public static Tuple<float, string> GetClosedPeptideByMass(List<Tuple<float, string>> argPeptideList,
            float argTargetMass)
        {
            float minDistance = 100.0f;
            Tuple<float, string> minPeptide = null;
            foreach (Tuple<float, string> peptide in argPeptideList)
            {
                if (Math.Abs(peptide.Item1 - argTargetMass) <= minDistance)
                {
                    minDistance = Math.Abs(peptide.Item1 - argTargetMass);
                    minPeptide = peptide;
                }
            }
            return minPeptide;
        }
    }
}
