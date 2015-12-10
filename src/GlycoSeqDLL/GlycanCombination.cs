using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COL.GlycoLib;
using CSMSL.Spectral;

namespace COL.GlycoSequence
{
    public static class GlycanCombination
    {
        public static List<Tuple<float, int, int, int, int, int>> GetAllGlycanCombination(int argMaxGlycanCount) //Tuple HexNAc,Hex,deHex,NeuAc,NeuGc
        {
            float HexMass = GlycanMass.GetGlycanMass(Glycan.Type.Hex);
            float deHexMass = GlycanMass.GetGlycanMass(Glycan.Type.DeHex);
            float HexNAcMass = GlycanMass.GetGlycanMass(Glycan.Type.HexNAc);
            float NeuACMass = GlycanMass.GetGlycanMass(Glycan.Type.NeuAc);
            float NeuGCMass = GlycanMass.GetGlycanMass(Glycan.Type.NeuGc);

            List<Tuple<float,int,int,int,int,int>> allCombintations  = new List<Tuple<float,int, int, int, int, int>>();
            for (int i = 0; i <= argMaxGlycanCount; i++)
            {
                for (int j = 0; j <= argMaxGlycanCount; j++)
                {
                    for (int k = 0; k <= argMaxGlycanCount; k++)
                    {
                        for (int l = 0; l <= argMaxGlycanCount; l++)
                        {
                            for (int m = 0; m <= argMaxGlycanCount; m++)
                            {
                                if ( (i + j + k + l + m) > 0 && (i+j+k+l+m)<=argMaxGlycanCount)
                                {
                                    float Mass = (i*HexNAcMass) + (j*HexMass) + (k*deHexMass) + (l*NeuACMass) + (m*NeuGCMass);
                                    allCombintations.Add(new Tuple<float,int, int, int, int, int>(Mass,i, j, k, l, m));
                                }
                            }
                        }
                    }
                }
            }
            allCombintations.Sort((a, b) => a.Item1.CompareTo(b.Item1));
            return allCombintations;
        }

        public static Tuple<float, int, int, int, int, int> GetClosedCombinationByMass(List<Tuple<float, int, int, int, int, int>> argAllCombinations, float argTargetMass)
        {
            float different = 100.0f;
            Tuple<float, int, int, int, int, int> targetCombination = null;
            foreach (Tuple<float, int, int, int, int, int> combination in argAllCombinations)
            {
                if(Math.Abs(combination.Item1-argTargetMass)<=different )
                {
                    different = Math.Abs(combination.Item1 - argTargetMass);
                    targetCombination = combination;
                }
            }
            return targetCombination;
        }
    }
}
