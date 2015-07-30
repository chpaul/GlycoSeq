using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COL.GlycoSequence
{
    public class enumModifications
    {
        public enum enumMod
        {
            [Description("Carbamidomethyl (M)")] Carbamidomethyl_M,
            [Description("Deamidated(N) (N)")] Deamidated_N,
            [Description("Oxidation (M)")] Oxidation_M
        }

         
    
    }
}
