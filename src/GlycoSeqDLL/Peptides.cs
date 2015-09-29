using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COL.ProtLib;
using COL.GlycoLib;
namespace COL.GlycoSequence
{
    public static class Peptides
    {
        public static List<Tuple<float, string,TargetPeptide>> ReadFastaFile(string argFastaFile)
        {
            List<ProteinInfo> pInfo = FastaReader.ReadFasta(argFastaFile);
            List<Tuple<float, string,TargetPeptide>> lstPeptideList = new List<Tuple<float, string,TargetPeptide>>();
            AminoAcidMass AAMass = new AminoAcidMass();
            foreach (ProteinInfo p in pInfo)
            {
                float Mass = AAMass.GetMonoMW(p.Sequence, true);
                lstPeptideList.Add(new Tuple<float, string, TargetPeptide>(Mass, p.Sequence, new TargetPeptide(p.Sequence)));
            }
            lstPeptideList.Sort((a,b)=>a.Item1.CompareTo(b.Item1));
            return lstPeptideList;
        }

        public static List<Tuple<float, string, TargetPeptide>> GetClosedPeptideByMass(List<Tuple<float, string, TargetPeptide>> argPeptideList,
            float argTargetMass, float argTolerance = 0.5f)
        {
            
            List<Tuple<float, string, TargetPeptide>> minPeptides = new List<Tuple<float, string, TargetPeptide>>();
            foreach (Tuple<float, string, TargetPeptide> peptide in argPeptideList)
            {
                if (Math.Abs(peptide.Item1 - argTargetMass) <= argTolerance)
                {
                    minPeptides.Add(peptide);
                }
            }
            return minPeptides;
        }
    }
}
