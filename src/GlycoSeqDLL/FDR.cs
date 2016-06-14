using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;
using CSMSL.Analysis.Identification;

namespace COL.GlycoSequence
{
    public class FDR
    {
    /// <summary>
    /// 
    /// </summary>
    /// <param name="argResultFile"></param>
    /// <param name="argFDRPrecentage"></param>
    /// <param name="argAlpha"></param>
    /// <param name="argBeta"></param>
    /// <returns>int: index of sorted score, double: score</returns>
        public static Tuple<int,double> ProduceFDRResult(string argResultFile, float argFDRPrecentage, double argAlpha = 1.0, double argBeta = 1.0)
        {
            List<Tuple<double, bool, int, string>> lstResult = new List<Tuple<double, bool, int, string>>();
            Dictionary<string,int> dictTitle = new Dictionary<string, int>();
            StringBuilder SB = new StringBuilder();
            using (System.IO.StreamReader sr = new StreamReader(argResultFile))
            {
                bool isInSection = false;
                string[] tmpStrAry;
                string tmpLine = "";
                int scanNum = 0;
                do
                {
                    tmpLine = sr.ReadLine();
                    tmpStrAry = tmpLine.Split(',');
                    if (tmpStrAry[0].StartsWith("MSn_Scan"))
                    {
                        isInSection = true;
                        for (int i = 0; i < tmpStrAry.Length; i++)
                        {
                            dictTitle.Add(tmpStrAry[i],i);    
                        }
                        SB.Append("FDR,"+ argFDRPrecentage+Environment.NewLine);
                        SB.Append("FDR parameter: y=alpha* core score + beta * branch score + Append glycan score, a=" +argAlpha +",b="+argBeta+ Environment.NewLine);
                        SB.Append(tmpLine+Environment.NewLine); //Title
                        continue;
                    }
                    if (!isInSection)
                    {
                        SB.Append(tmpLine+Environment.NewLine);
                        continue;
                    }
                    if (scanNum != Convert.ToInt32(tmpStrAry[dictTitle["MSn_Scan"]]))
                    {
                        scanNum = Convert.ToInt32(tmpStrAry[dictTitle["MSn_Scan"]]);
                        double totalScore = argAlpha*Convert.ToDouble(tmpStrAry[dictTitle["Core_Score"]]) +
                                            argBeta*Convert.ToDouble(tmpStrAry[dictTitle["Branch_Score"]]) +
                                            Convert.ToDouble(tmpStrAry[dictTitle["Append_Glycan_Score"]]);
                        bool isDecoyHit = (tmpStrAry[dictTitle["Peptide"]].ToUpper().EndsWith("X")) ? true : false;
                        
                        lstResult.Add(new Tuple<double, bool,int, string>(totalScore, isDecoyHit, scanNum, tmpLine));
                    }
                } while (!sr.EndOfStream);
            }
            lstResult = lstResult.OrderByDescending(x => x.Item1).ToList();
            int FDRIndex = 0;
            int DecoyHit = lstResult.Where(x => x.Item2 == true).Count();
            int TrueHit = lstResult.Where(x => x.Item2 == false).Count();
            var sbFDR = new StringBuilder();
            for (int i = 0; i < lstResult.Count; i++)
            {
                int decoyCount = (lstResult.Take(i+1).Where(x => x.Item2 == true)).Count();
                sbFDR.Append(i.ToString() + "," + decoyCount.ToString() + "," + (decoyCount/Convert.ToDouble(i + 1)).ToString("0.0000")+Environment.NewLine);
                if (decoyCount/Convert.ToDouble(i+1) >argFDRPrecentage)
                {
                    FDRIndex = i - 1;
                }
            }
            //List<Tuple<double, bool, int, string>> FDRList = lstResult.Take(FDRIndex).ToList().OrderBy(x => x.Item3).ToList();
            //for (int i = 0; i < FDRList.Count; i++)
            {
              //  SB.Append(lstResult[i] + Environment.NewLine);
            }
            using (StreamWriter sw = new StreamWriter(argResultFile.Replace(".csv", "_FDR.csv")))
            {
                sw.Write(SB.ToString());
            }
            //using (StreamWriter sw = new StreamWriter(argResultFile.Replace(".csv", "_FDRList.csv")))
            //{
            //    sw.Write(sbFDR.ToString());
            //}

            return new Tuple<int, double>(FDRIndex, lstResult[FDRIndex].Item1);
        }
    }
}
