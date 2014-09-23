using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using COL.GlycoLib;
using COL.MassLib;
using System.Drawing;
namespace COL.GlycoSequence
{
    class GenerateHtmlReport
    {
        public static void ExporToFolder(string argFolder, GlycanSequencing argGS)
        {
            GlycanSequencing GS = argGS;
            if (GS.SequencedStructures.Count > 1)
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
                sb.AppendLine("<title>Glycoseq result for scan " + GS.ScanInfo.ScanNo.ToString() + "</title>\n</head>\n<body>");
                GlycansDrawer Draw;
                List<GlycanStructure> ExactGlycanTreeMatch = new List<GlycanStructure>();
                foreach (GlycanStructure GTStructure in GS.SequencedStructures)
                {
                    if (MassUtility.GetMassPPM(GTStructure.GlycanMonoMass,  GS.GlycanMonoMass) <=  GS.PrecursorTorelance)
                    {
                        ExactGlycanTreeMatch.Add(GTStructure);
                    }
                }
                argGS.SequencedStructures.Sort(delegate(GlycanStructure T1, GlycanStructure T2)
                {
                    return T2.GlycanMonoMass.CompareTo(T1.GlycanMonoMass) != 0 ? T2.GlycanMonoMass.CompareTo(T1.GlycanMonoMass) : T2.Score.CompareTo(T1.Score)
                      ;
                });

                sb.AppendLine("Precursor m/z :" + GS.ScanInfo.ParentMZ.ToString("0.0000") + "(" + GS.ScanInfo.ParentCharge.ToString() + "+)<br>");
                sb.AppendLine("Predicted Y1:" +  GS.Y1.ToString("0.0000") + "<br>");
                sb.AppendLine("Predicted GlycanMonoMass:" + GS.GlycanMonoMass.ToString("0.000") + "<br>");
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

                for (int i = 0; i < GS.SequencedStructures.Count; i++)
                {
                    GlycanStructure Gt = GS.SequencedStructures[i];
                    string BGColor = "#FFFFFF"; //White
                    if (MassUtility.GetMassPPM(Gt.GlycanMonoMass, GS.GlycanMonoMass) <= GS.PrecursorTorelance && Gt.HasNGlycanCore())
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
                        sb.AppendLine("<td bgcolor=\"" + BGColor + "\"><a href=\"#" + (i + 1).ToString() + "\">" + (i + 1).ToString() + "</a></td>");
                        sb.AppendLine("<td bgcolor=\"" + BGColor + "\">" + Gt.GlycanMonoMass.ToString() + "</td>");
                        sb.AppendLine("<td bgcolor=\"" + BGColor + "\">" + Gt.Score.ToString("0.0000") + "(" + MassUtility.GetMassPPM(Gt.GlycanMonoMass, GS.GlycanMonoMass).ToString("0.0") + ")" + "</td>");
                        sb.AppendLine("<td><img src=\".\\Pics\\" + Gt.IUPACString.ToString() + ".png\"/></td>");
                        sb.AppendLine("<td colspan=\"2\" bgcolor=\"" + BGColor + "\">" + Gt.IUPACString + "</td>");
                        sb.AppendLine("</tr>");
                    }
                    catch
                    {
                        sb.AppendLine("<tr>");
                        sb.AppendLine("<td bgcolor=\"" + BGColor + "\"><a href=\"#" + (i + 1).ToString() + "\">" + (i + 1).ToString() + "</a></td>");
                        sb.AppendLine("<td bgcolor=\"" + BGColor + "\">" + Gt.GlycanMonoMass.ToString() + "</td>");
                        sb.AppendLine("<td bgcolor=\"" + BGColor + "\">" + Gt.Score.ToString("0.0000") + "(" + MassUtility.GetMassPPM(Gt.GlycanMonoMass, GS.GlycanMonoMass).ToString("0.0") + ")" + "</td>");
                        sb.AppendLine("<td></td>");
                        sb.AppendLine("<td colspan=\"2\" bgcolor=\"" + BGColor + "\">" + Gt.IUPACString + "</td>");
                        sb.AppendLine("</tr>");
                    }
                }
                sb.AppendLine("</table><br><br>");

                //Detail Table

                for (int i = 0; i < GS.SequencedStructures.Count; i++)
                {
                    GlycanStructure Gt = GS.SequencedStructures[i];
                    int idx = i + 1;
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

                    Image AnnotatedSpectrum = GlycanImage.GetAnnotatedImage(GS.ScanInfo.ScanNo.ToString(), GS.AllPeaks, Gt);
                    System.IO.MemoryStream mss = new System.IO.MemoryStream();
                    System.IO.FileStream fs = new System.IO.FileStream(argFolder + "\\Pics\\AnnotatedSpectrum_" + argGS.ScanInfo.ScanNo+"_"+ (i + 1).ToString("000") + ".png", System.IO.FileMode.Create, System.IO.FileAccess.ReadWrite);

                    //AnnotatedSpectrum.Save(argFolder + "\\Pics\\AnnotatedSpectrum_" + argGS.ScanInfo.ScanNo + "_" + (i + 1).ToString("000") + ".png");
                    AnnotatedSpectrum.Save(mss, System.Drawing.Imaging.ImageFormat.Png);
                    byte[] matriz = mss.ToArray();
                    fs.Write(matriz, 0, matriz.Length);
                    
                    mss.Close();
                    fs.Close();
                    AnnotatedSpectrum.Dispose();
                    AnnotatedSpectrum = null;
                    sb.AppendLine("<a href=\".\\Pics\\AnnotatedSpectrum_" + argGS.ScanInfo.ScanNo + "_" + (i + 1).ToString("000") + ".png\" target=\"_blank\"><img src=\".\\Pics\\AnnotatedSpectrum_" + argGS.ScanInfo.ScanNo + "_" + (i + 1).ToString("000") + ".png\" height=\"450\" width=\"600\"/></a>");
                    //Fragement table
                    sb.AppendLine("<table  border=\"1\">");
                    sb.AppendLine("<tr><td colspan=\"2\">Fragement</td><td colspan=\"2\">Identified Peak</td></tr>");
                    sb.AppendLine("<tr><td>Glycopeptide MZ</td><td>Structure</td><td>Identified MZ</td><td>Identified Intensity</td></tr>");

                    if (lstFragement.Count >= lstIDPeaksMz.Count)
                    {
                        for (int j = 0; j < lstFragement.Count; j++)
                        {
                            sb.AppendLine("<tr>");
                            sb.AppendLine("<td>" + (GlycanMass.GetGlycanMasswithCharge(lstFragement[j].GlycanType, Gt.Charge) + GS.Y1 - GlycanMass.GetGlycanMasswithCharge(Glycan.Type.HexNAc, Gt.Charge)).ToString() + "</td>");
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
                                sb.AppendLine("<td>" + (GlycanMass.GetGlycanMasswithCharge(lstFragement[j].GlycanType, Gt.Charge) + GS.Y1 - GlycanMass.GetGlycanMasswithCharge(Glycan.Type.HexNAc, Gt.Charge)).ToString() + "</td>");
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
                    GC.Collect();
                }

                sb.AppendLine("</body>\n</html>");
                StreamWriter sw = new StreamWriter(argFolder + "\\Scan" + GS.ScanInfo.ScanNo.ToString() + ".htm");
                sw.Write(sb.ToString());
                sw.Flush();
                sw.Close();
            }
        }
    }
}
