using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using COL.GlycoLib;
namespace COL.GlycoSequence
{
    public class ReadFilterRules
    {
        public static List<StructureRule> ReadFilterRule()
        {
            List<StructureRule> _structureRules = new List<StructureRule>();
            if(!File.Exists(System.Windows.Forms.Application.StartupPath + "\\FilterRules.txt"))
            {
                return _structureRules;
            }
            StreamReader sr = new StreamReader(System.Windows.Forms.Application.StartupPath + "\\FilterRules.txt");           
            string[] tmpStr;
            sr.ReadLine(); //Title
            do
            {
                tmpStr = sr.ReadLine().Split('\t');
                if (tmpStr[0].StartsWith("#"))
                {
                    continue;
                }
                int distance = -999;
                if (tmpStr[1].Length > 1)
                {
                    distance = Convert.ToInt32(tmpStr[1].Substring(1));
                }

                if (tmpStr[2] == "Denied")
                {
                    _structureRules.Add(new StructureRule(tmpStr[0], distance, tmpStr[1].Substring(0, 1), StructureRule.FiltereTypes.Denied));
                }
                else if (tmpStr[2] == "Required")
                {
                    _structureRules.Add(new StructureRule(tmpStr[0], distance, tmpStr[1].Substring(0, 1), StructureRule.FiltereTypes.Required));
                }
                else// if(tmpStr[2] =="Option")
                {
                    _structureRules.Add(new StructureRule(tmpStr[0], distance, tmpStr[1].Substring(0, 1), StructureRule.FiltereTypes.Option));
                }
            } while (sr.EndOfStream != true);
            sr.Close();
            return _structureRules;
        }
    }
}
