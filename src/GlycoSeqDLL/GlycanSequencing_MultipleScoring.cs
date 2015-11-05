using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using System.Windows.Forms.VisualStyles;
using COL.GlycoLib;
using COL.MassLib;
using COL.ProtLib;
using CSMSL.Chemistry;

namespace COL.GlycoSequence
{
    public class GlycanSequencing_MultipleScoring
    {
        private bool _isDebug = false;
        private bool _NGlycanData = true;
        private bool _allowSiaConnectToHexNac = false;
        private bool _createPrecursor = false;
        private float _rewardForCompleteStructure = 0.0f;
        StreamWriter DebugSW;
        private string _debugFolderPath = "c:\\temp";
        float _MS2torelance = 0.8f; //dalton
        //float _torelance = 500.0f; // old one
        float _precursorTorelance = 50.0f;
        private float _y1MZ = 0.0f;
        private int _y1charge = 0;
        private int _precursorCharge = 0;
        private MSScan _scan;
        private List<MSPoint> _potentialPeaks; // mass greater than Y1        
        private List<MSPoint> _allPeaks;
        //private bool _isHex = true;
        //private bool _isDeHex = true;
        //private bool _isHexNAc = true;
        //private bool _isNeuAc = true;
        //private bool _isNeuGc = false;
        private int _NoHex = 99;
        private int _NoDeHex = 99;
        private int _NoHexNac = 99;
        private int _NoNeuAc = 99;
        private int _NoNeuGc = 99;
        private float MaxIntensityInScan = 0.0f;
        private float _glycanMonoMass = 0.0f;// Precusor -Y1 + GalNAc;
        private float _peptideMass = 0.0f;
        //private float _peptideMz = 0.0f;
        private List<GlycanStructure> _SequencedGlycanStructure = new List<GlycanStructure>();
        private List<GlycanStructure> _FullSequencedGlycanStructure = new List<GlycanStructure>();
        private string _OutputLocation;
        private List<StructureRule> _structureRules;
        private List<Glycan> _glycanBuildingblock = new List<Glycan>();
        private float _PrecusorMono;
        private int _NumbersOfPeaksForSeq = 150;
        private bool _UseAVGMass = true;
        // If true use the rest compostion to complete structure.
        private bool _NumCompostionSet = false;
        private string _peptideStr = "";
        private bool _isCYS_CAM = true;
        private string _status = "";
        private enumGlycanType _glycanType;
        private TargetPeptide _TargetPeptide;
        private int _topPeaks_i = 0;
        private int _topDiagPeaks_j = 0;
        private int _topCorePeaks_k = 0;
        private int _topBrancingPeaks_l = 0;
        private int _maxGlycansToCompleteStruct_m = 0;
        private List<Tuple<float, string, TargetPeptide>> _peptideList = new List<Tuple<float, string, TargetPeptide>>();
        private List<Tuple<TargetPeptide,GlycanCompound>> _glycopeptides = new List<Tuple<TargetPeptide, GlycanCompound>>(); 
        private bool _UnknownPeptideSearch = false; // Have peptide and glycan mass already match with precursor;
        private List<GlycanCompound>  lstGlycans = new List<GlycanCompound>();
        private List<TargetPeptide> lstPeptides =new List<TargetPeptide>();
        private bool _PeptideFromMascotResult = false;
        public GlycanSequencing_MultipleScoring(MSScan argScan, int argPrecursorCharge, int argNoHex, int argNoHexNAc, int argNoDeHex, int argNoNeuAc, int argNoNeuGc, string argOutput, bool argNGlycanData, float argPeakTol, float argPrecursorTol, List<int> argPeaksParameters, List<Tuple<float, string,TargetPeptide>> argPeptideList)
        {
            _NGlycanData = argNGlycanData;
            _precursorCharge = argPrecursorCharge;
            _scan = argScan;
            _NoDeHex = argNoDeHex;
            _NoHex = argNoHex;
            _NoHexNac = argNoHexNAc;
            _NoNeuAc = argNoNeuAc;
            _NoNeuGc = argNoNeuGc;
            _OutputLocation = argOutput;
            _MS2torelance = argPeakTol;
            _precursorTorelance = argPrecursorTol;
            _NumCompostionSet = true;
            _structureRules = ReadFilterRules.ReadFilterRule();
            _topPeaks_i = argPeaksParameters[0];
            _topDiagPeaks_j = argPeaksParameters[1];
            _topCorePeaks_k = argPeaksParameters[2];
            _topBrancingPeaks_l = argPeaksParameters[3];
            _maxGlycansToCompleteStruct_m = argPeaksParameters[4];
            _peptideList = argPeptideList;
        }
        public GlycanSequencing_MultipleScoring(MSScan argScan, int argPrecursorCharge, List<Tuple<TargetPeptide,GlycanCompound>> argGlycoPeptides, string argOutput, bool argNGlycanData, float argPeakTol, float argPrecursorTol, List<int> argPeaksParameters)
        {
            _NGlycanData = argNGlycanData;
            _precursorCharge = argPrecursorCharge;
            _scan = argScan;

            _OutputLocation = argOutput;
            _MS2torelance = argPeakTol;
            _precursorTorelance = argPrecursorTol;
            _NumCompostionSet = true;
            _structureRules = ReadFilterRules.ReadFilterRule();
            _topPeaks_i = argPeaksParameters[0];
            _topDiagPeaks_j = argPeaksParameters[1];
            _topCorePeaks_k = argPeaksParameters[2];
            _topBrancingPeaks_l = argPeaksParameters[3];
            _maxGlycansToCompleteStruct_m = argPeaksParameters[4];
            _glycopeptides = argGlycoPeptides;
            
        }

        public bool PeptideFromMascotResult
        {
            get { return _PeptideFromMascotResult; }
            set { _PeptideFromMascotResult = value; }
        }
        public TargetPeptide Peptide
        {
            get { return _TargetPeptide; }
            set { _TargetPeptide = value; }
        }
        public enumGlycanType GlycanType
        {
            get { return _glycanType; }
            set { _glycanType = value; }
        }

        public string PeptideSeq
        {
            get { return _peptideStr; }
        }

        public List<TargetPeptide> PeptideList
        {
            set { lstPeptides = value; }
        }
        public List<GlycanCompound> GlycansList
        {
            set { lstGlycans = value; }
        }
        public MSScan ScanInfo
        {
            set { _scan = value; }
            get { return _scan; }
        }
        public float Y1MZ
        {
            set { _y1MZ = value; }
            get { return _y1MZ; }
        }
        public float PrecursorTorelance
        {
            set { _precursorTorelance = value; }
            get { return _precursorTorelance; }
        }
        public float RewardForCompleteStructure
        {
            set { _rewardForCompleteStructure = value; }
            get { return _rewardForCompleteStructure; }
        }
        public bool UseAVGMass
        {
            set
            {
                _UseAVGMass = value;
                if (_UseAVGMass == false)
                {
                    AminoAcidMass MW = new AminoAcidMass();
                    _y1MZ = (MW.GetMonoMW(_peptideStr, _isCYS_CAM) + GlycanMass.GetGlycanMass(Glycan.Type.HexNAc) + Atoms.ProtonMass * _y1charge) / _y1charge;
                }
            }
            get { return _UseAVGMass; }
        }
        public void DebugMode(string argDebugFilePath)
        {
            _isDebug = true;
            _debugFolderPath = argDebugFilePath;
        }
        public bool CreatePrecursotMZ
        {
            set { _createPrecursor = value; }
            get { return _createPrecursor; }
        }
        public bool AllowSiaConnectToHexNac
        {
            get { return _allowSiaConnectToHexNac; }
            set { _allowSiaConnectToHexNac = true; }
        }
        public List<MSPoint> AllPeaks
        {
            get { return _allPeaks; }
        }
        public float PeptideMonoMass
        {
            get { return _peptideMass; }
        }
        public float GlycanMonoMass
        {
            get { return _glycanMonoMass; }
        }
        public float PrecusorMonoMass
        {
            get { return _scan.ParentMonoMW; }
        }
        public float PrecusorAVGMonoMass
        {
            get { return _scan.ParentAVGMonoMW; }
        }
        public List<GlycanStructure> SequencedStructures
        {
            get { return _SequencedGlycanStructure; }
        }
        public List<GlycanStructure> FullSequencedStructures
        {
            get { return _FullSequencedGlycanStructure; }
        }
        public List<MSPoint> FilteredPeaks
        {
            get { return _potentialPeaks; }
        }
        public int NumbersOfPeaksForSequencing
        {
            get { return _NumbersOfPeaksForSeq; }
            set { _NumbersOfPeaksForSeq = value; }
        }
        public string Status
        {
            get { return _status; }
        }

        public bool UnknownPeptideSearch
        {
            set { _UnknownPeptideSearch = value; }
            get { return _UnknownPeptideSearch; }
        }
        private void UpdateStatus(string argStatue)
        {
            _status = _status + argStatue + ";";
        }
        public int NumFullStructure
        {
            get
            {
                int count = 0;
                foreach (GlycanStructure GT in _SequencedGlycanStructure)
                {
                    if (MassUtility.GetMassPPM(GT.GlycanMonoMass, _glycanMonoMass) <= _precursorTorelance)
                    {
                        count++;
                    }
                }
                return count;
            }
        }
        public int NumFullStructureWithNCore
        {
            get
            {
                int count = 0;
                foreach (GlycanStructure GT in _SequencedGlycanStructure)
                {
                    if (MassUtility.GetMassPPM(GT.GlycanMonoMass, _glycanMonoMass) <= _precursorTorelance && GT.HasNGlycanCore())
                    {
                        count++;
                    }
                }
                return count;
            }
        }
        //Public Functions
        public List<GlycanStructure> GetTopRankScoreStructre(int argRank)
        {
            Dictionary<double, List<GlycanStructure>> RankedStructure = new Dictionary<double, List<GlycanStructure>>();
            List<double> SortedScore = new List<double>();
            foreach (GlycanStructure g in _FullSequencedGlycanStructure)
            {
                if (!RankedStructure.ContainsKey(g.Score))
                {
                    RankedStructure.Add(g.Score, new List<GlycanStructure>());
                    SortedScore.Add(g.Score);
                }
                RankedStructure[g.Score].Add(g);
            }
            foreach (GlycanStructure g in _SequencedGlycanStructure)
            {
                if (!RankedStructure.ContainsKey(g.Score))
                {
                    RankedStructure.Add(g.Score, new List<GlycanStructure>());
                    SortedScore.Add(g.Score);
                }
                RankedStructure[g.Score].Add(g);
            }
            SortedScore.Sort();
            SortedScore.Reverse();

            List<GlycanStructure> TopRanked = new List<GlycanStructure>();
            if (SortedScore.Count < argRank)
            {
                argRank = SortedScore.Count;
            }
            for (int i = 0; i < argRank; i++)
            {
                TopRanked.AddRange(RankedStructure[SortedScore[i]]);
            }
            return TopRanked;
        }
        public List<GlycanStructure> GetTopRankNoNCompletedSequencedStructre(int argRank)
        {
            List<int> Rank = new List<int>();

            foreach (GlycanStructure gs in _SequencedGlycanStructure)
            {
                if (!Rank.Contains(gs.NoOfTotalGlycan))
                {
                    Rank.Add(gs.NoOfTotalGlycan);
                }
            }

            Rank.Sort();
            List<GlycanStructure> RankedList = new List<GlycanStructure>();
            if (Rank.Count >= argRank)
            {
                int TargetNumber = Rank[argRank - 1];

                foreach (GlycanStructure gs in _SequencedGlycanStructure)
                {
                    if (gs.NoOfTotalGlycan == TargetNumber)
                    {
                        RankedList.Add(gs);
                    }
                }
            }
            else
            {
                RankedList.AddRange(_SequencedGlycanStructure);
            }
            return RankedList;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>
        /// 0 = finish sequence and no structures are found;
        /// 1 = finish sequence and no full structures are found;
        /// 2 = finish sequence and full structures are found;
        /// Error:
        /// -1 = y1 m/z >=2000.0
        /// -2 = Dont have enough peaks ( number of peaks in scan is less than required parameter)
        /// </returns>
        public int StartSequencing()
        {
            try
            {
                FindNLinkedStructureFromPentamer();
                _SequencedGlycanStructure.Sort();
                if (_FullSequencedGlycanStructure.Count != 0)
                {
                    return 2;
                }
                else if (_SequencedGlycanStructure.Count != 0)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
        }

        //Private Function
        private List<GlycanCompound> FindMatchedGlycans(float argPeptideMass, float argPrecursorMass)
        {
            float glycanMass = argPrecursorMass - argPeptideMass;
            List<GlycanCompound> lstMatchedGlycans =lstGlycans.Where(T => Math.Abs(T.MassWithoutWater - glycanMass) < _MS2torelance).ToList();
            if (lstMatchedGlycans == null)
            {
                lstMatchedGlycans = new List<GlycanCompound>();   
            }
            return lstMatchedGlycans;
        }

        private List<TargetPeptide> FindMatchedPeptides(float argPeptideMass )
        {
            List<TargetPeptide> lstMatchedPeptides =lstPeptides.Where(T => Math.Abs(argPeptideMass- T.PeptideMass) <= _MS2torelance*2).ToList();
            if (lstMatchedPeptides == null)
            {
                lstMatchedPeptides = new List<TargetPeptide>();
            }
            return lstMatchedPeptides;
        }

        private List<Tuple<TargetPeptide, GlycanCompound>> FindMatchedGlycoPeptide(List<TargetPeptide> argPeptides, List<GlycanCompound> argGlycans ,  float argPrecursorMass)
        {
            List<Tuple<TargetPeptide,GlycanCompound>> MatchedGlycopeptides = new List<Tuple<TargetPeptide, GlycanCompound>>();
            foreach (TargetPeptide tPeptide in argPeptides)
            {
                foreach (GlycanCompound glycan in argGlycans)
                {
                    if (MassUtility.GetMassPPM(tPeptide.PeptideMass + glycan.MassWithoutWater, argPrecursorMass) <=_precursorTorelance)
                    {
                        MatchedGlycopeptides.Add(new Tuple<TargetPeptide, GlycanCompound>(tPeptide,glycan));
                    }
                }
            }
            return MatchedGlycopeptides;
        }
        /// <summary>
        /// Find N Glycan Check Y1~ Y4
        /// </summary>
        private void FindNLinkedStructureFromPentamer()
        {
       
            bool isBiscectingSignal = false;
            var allPeaks = new List<MSPoint>(); // m/z, intensity, rank
            
            for (int i = 0; i < _scan.MZs.Length; i++)
            {
                allPeaks.Add(new MSPoint(_scan.MZs[i], _scan.Intensities[i]));
            }
            MaxIntensityInScan = allPeaks.OrderByDescending(x => x.Intensity).ToList()[0].Intensity;

            foreach (MSPoint p in allPeaks)
            {
                p.Intensity = Convert.ToSingle((p.Intensity / MaxIntensityInScan * 100));
            }
            List<MSPoint> lstY1Peaks = new List<MSPoint>();
            List<MSPoint> lstCorePeaks = new List<MSPoint>();
            List<MSPoint> lstBranchPeaks = new List<MSPoint>();

            //lstY1Peaks.AddRange(allPeaks.Take(_topPeaks_i));
            lstY1Peaks.AddRange( allPeaks.OrderByDescending(p => p.Intensity).Take(_topPeaks_i).OrderBy(t => t.Mass).ToList());

            //Use Peptide Create pseudo Y1
            if (_PeptideFromMascotResult)
            {
                foreach (TargetPeptide tPeptide in lstPeptides.Where(x => x.StartTime <= _scan.Time && _scan.Time <= x.EndTime).ToList())
                {
                    for (int i = 1; i <= 4; i++)
                    {
                        float mz = (tPeptide.PeptideMass + Atoms.ProtonMass*i + GlycanMass.GetGlycanMass(Glycan.Type.HexNAc))/(float) i;
                        
                        if (mz > _scan.MaxMZ)
                        {
                            continue;
                        }
                        if (!lstY1Peaks.Exists(x => Math.Abs(x.Mass - mz) <= _MS2torelance))
                        {
                            lstY1Peaks.Add(new MSPoint(mz, 0));
                        }
                    }
                }
                lstY1Peaks = lstY1Peaks.OrderBy(t => t.Mass).ToList();
            }
            //lstCorePeaks.AddRange(allPeaks.Take(_topCorePeaks_k));
            //lstBranchPeaks.AddRange(allPeaks.Take(_topBrancingPeaks_l));
            //lstY1Peaks.Sort();

            //lstCorePeaks.Sort();
            //lstBranchPeaks.Sort();
            //_potentialPeaks = lstBranchPeaks;
            //
            float HexMass = GlycanMass.GetGlycanMass(Glycan.Type.Hex);
            float deHexMass = GlycanMass.GetGlycanMass(Glycan.Type.DeHex);
            float HexNAcMass = GlycanMass.GetGlycanMass(Glycan.Type.HexNAc);
            float NeuACMass = GlycanMass.GetGlycanMass(Glycan.Type.NeuAc);
            float NeuGCMass = GlycanMass.GetGlycanMass(Glycan.Type.NeuGc);
            GlycanCompound preciseGlycan = null;
            TargetPeptide precisePeptide = null;
            foreach (MSPoint Y1 in lstY1Peaks)
            {
                List<GlycanStructure> sequencedGlycanStructure = new List<GlycanStructure>();
                int currentCharge = _precursorCharge - 1;
                for (int chargePlus = 0; chargePlus <= 1; chargePlus++)
                {

                    currentCharge = currentCharge + chargePlus;
                    float peptidemono = Y1.Mass * currentCharge - Atoms.ProtonMass * currentCharge - HexNAcMass;

                    //Match PeptideMemo 
                    List<TargetPeptide> ClosedPeptides = FindMatchedPeptides(peptidemono);
                    ClosedPeptides = ClosedPeptides.GroupBy(t => new { t.PeptideSequence, t.ModificationString }).Distinct().Select(t => t.First()).ToList();
                    //No peptide matched
                    if (!_UnknownPeptideSearch && ClosedPeptides.Count == 0)
                    {
                        continue;
                    }

                    //Find Matched Glycan
                    List<GlycanCompound> ClosedGlycan = new List<GlycanCompound>();
                    if (lstGlycans!=null && lstGlycans.Count != 0)
                    {
                        ClosedGlycan.AddRange(FindMatchedGlycans(peptidemono, ScanInfo.ParentMonoMW));
                        ClosedGlycan = ClosedGlycan.Distinct().ToList();
                    }

                    #region coreSequencing

                    //Generated All possible fragements from Y1
                    float[] coreTheoreticalFragements = new float[5];
                    float[] coreTheorericalFragementsWithCoreFuc = new float[5];
                    coreTheoreticalFragements[0] = Y1.Mass;
                    coreTheoreticalFragements[1] = coreTheoreticalFragements[0] + GlycanMass.GetGlycanMass(Glycan.Type.HexNAc) / currentCharge;
                    coreTheoreticalFragements[2] = coreTheoreticalFragements[1] + GlycanMass.GetGlycanMass(Glycan.Type.Hex) / currentCharge;
                    coreTheoreticalFragements[3] = coreTheoreticalFragements[2] + GlycanMass.GetGlycanMass(Glycan.Type.Hex) / currentCharge;
                    coreTheoreticalFragements[4] = coreTheoreticalFragements[3] + GlycanMass.GetGlycanMass(Glycan.Type.Hex) / currentCharge;

                    for (int i = 0; i < 5; i++)
                    {
                        coreTheorericalFragementsWithCoreFuc[i] = coreTheoreticalFragements[i] +GlycanMass.GetGlycanMass(Glycan.Type.DeHex)/currentCharge;
                    }
                    lstCorePeaks.Clear();
                    lstCorePeaks.AddRange(
                        allPeaks.Where(t=> t.Mass >= Y1.Mass-10 && t.Mass <= coreTheorericalFragementsWithCoreFuc[4]+10).OrderByDescending(p => p.Intensity).Take(_topCorePeaks_k).OrderBy(x => x.Mass).ToList());

                    string[] coreFragementTags = new string[5] { "HexNAc", "HexNAc-HexNAc", "HexNAc-HexNAc-Hex", "HexNAc-HexNAc-Hex-Hex", "HexNAc-HexNAc-Hex-(Hex-)Hex" };
                    string[] coreFucFragementTasgs =new string[5] { "HexNAc-dexHex", "HexNAc-(deHex-)HexNAc", "HexNAc-(deHex-)HexNAc-Hex", "HexNAc-(deHex-)HexNAc-Hex-Hex", "HexNAc-(deHex-)HexNAc-Hex-(Hex-)Hex" };
                    //Find Peaks
                    int coreIdentifiedFragementsCount  = 0;
                    int coreIdentifiedFragementsWithCoreFucCount = 0;
                    float[] coreIdentifiedFragements =new float[5]{0,0,0,0,0};
                    float[] coreIdentifiedFragementsWithCoreFuc = new float[5] { 0, 0, 0, 0, 0 };
                    for (int i = 0; i < 5; i++)
                    {
                        List<MSPoint> coreFragement = lstCorePeaks.Where(t => Math.Abs(t.Mass - coreTheoreticalFragements[i]) <= _MS2torelance).OrderBy(t => Math.Abs(t.Mass - coreTheoreticalFragements[i])).ToList();
                        if (coreFragement.Count != 0)
                        {
                            coreIdentifiedFragements[i] = coreFragement[0].Mass;
                            coreIdentifiedFragementsCount++;
                        }
                        List<MSPoint> coreFucFragement = lstCorePeaks.Where(t => Math.Abs(t.Mass - coreTheorericalFragementsWithCoreFuc[i]) <= _MS2torelance).OrderBy(t => Math.Abs(t.Mass - coreTheorericalFragementsWithCoreFuc[i])).ToList();
                        if (coreFucFragement.Count != 0)
                        {
                            coreIdentifiedFragementsWithCoreFuc[i] = coreFucFragement[0].Mass;
                            coreIdentifiedFragementsWithCoreFucCount++;
                        }
                    }

                    if ((!UnknownPeptideSearch && ClosedPeptides.Count == 0) && (lstGlycans!=null && ClosedGlycan.Count == 0))
                    {
                        continue;
                    }



                    List<Tuple<TargetPeptide, GlycanCompound>> MatchedGlycoPeptides =new List<Tuple<TargetPeptide, GlycanCompound>>();
                    if (ClosedPeptides.Count != 0 && ClosedGlycan.Count != 0)
                    {
                        MatchedGlycoPeptides = FindMatchedGlycoPeptide(ClosedPeptides, ClosedGlycan, _scan.ParentMonoMW);

                        //Filter By Time
                        if (MatchedGlycoPeptides.GroupBy(t =>new {t.Item1.PeptideSequence,t.Item2.GlycanKey}).Distinct().Select(t => t.First()).ToList().Count>1)
                        {
                            MatchedGlycoPeptides =
                                MatchedGlycoPeptides.Where(
                                    t => t.Item1.StartTime <= _scan.Time && _scan.Time <= t.Item1.EndTime).ToList();
                        }
                    }

                    #region Core Evaluation
                    ////Core Evaluation
                    //StreamWriter sw = new StreamWriter(Environment.CurrentDirectory + "\\Output.csv", true);
                   
                   
                    //    string tmpOut = _scan.ScanNo + ",";
                    //    tmpOut += _scan.ParentMonoMz + ",";
                    //    tmpOut += _scan.ParentMonoMW + ",";
                    //    tmpOut += Y1.Mass.ToString("0.0000") + ",";
                    //    tmpOut += currentCharge + ",";
                    //    if (ClosedPeptides.Count != 0)
                    //    {
                    //        tmpOut += ClosedPeptides[0].PeptideSequence + ",";
                    //    }
                    //    else
                    //    {
                    //        tmpOut += "-,";
                    //    }
                    //if (MatchedGlycoPeptides.Count != 0)
                    //{
                    //    tmpOut += MatchedGlycoPeptides[0].Item2.GlycanKey + ",";
                    //}
                    //else
                    //{
                    //    tmpOut += "-,";
                    //}
                        
                    //    tmpOut += coreIdentifiedFragementsCount + ",";
                    //float CoreFrageScore = 0;
                    //float CoreFucFragScore = 0;
                    //for (int i = 0; i < 5; i++)
                    //{
                    //    if (coreIdentifiedFragements[i] != 0)
                    //    {
                    //        CoreFrageScore += allPeaks.Find(p => p.Mass == coreIdentifiedFragements[i]).Intensity;
                    //    }
                    //    if (coreIdentifiedFragementsWithCoreFuc[i] != 0)
                    //    {
                    //        CoreFucFragScore +=allPeaks.Find(p => p.Mass == coreIdentifiedFragementsWithCoreFuc[i]).Intensity;
                    //    }
                    //}

                    
                    //    tmpOut += CoreFrageScore.ToString("0.000") + ",";
                    //    tmpOut += coreIdentifiedFragementsWithCoreFucCount + ",";
                    
                    //tmpOut += CoreFucFragScore.ToString("0.000") + ",";
                    //    sw.WriteLine(tmpOut);
                    
                   
                    //sw.Close();
                    //continue;
                    #endregion
                    //Check if core found
                    bool IsCoreFuc = (coreIdentifiedFragementsWithCoreFucCount >= 3) ? true : false;
                    if ( coreIdentifiedFragementsCount <3 || coreIdentifiedFragementsCount<3)
                    {
                        continue;
                    }

                    GlycanStructure Core;
                    GlycanTreeNode ParentNode;
                    GlycanTreeNode ExtraCoreGlycan;
                    //Build Core with Fucose Structure
                    if (IsCoreFuc)
                    {
                        Core = new GlycanStructure(new Glycan(Glycan.Type.HexNAc, currentCharge), Y1);
                        Core.Charge = currentCharge;
                        Core.PrecursorMonoMass = _scan.ParentMonoMW;
                        
                        //Store Core ID Peaks
                        for (int i = 0; i < 5; i++)
                        {
                            if (coreIdentifiedFragements[i] != 0)
                            {
                                Core.CoreIDPeak.Add(new Tuple<MSPoint, string>(allPeaks.Find(p => p.Mass == coreIdentifiedFragements[i]), coreFragementTags[i]));
                            }
                        }
                        //for (int i = 0; i < 5; i++)
                        //{
                        //    if (coreIdentifiedFragementsWithCoreFuc[i] != 0)
                        //    {
                        //        Core.CoreIDPeak.Add(new Tuple<MSPoint, string>(allPeaks.Find(p => p.Mass == coreIdentifiedFragementsWithCoreFuc[i]), coreFucFragementTasgs[i]));
                        //    }
                        //}
                        ParentNode = Core.GetGlycanTreeByID(1);
                        //HexNAc-deHex
                        ExtraCoreGlycan = new GlycanTreeNode(Glycan.Type.DeHex, Core.NextID);
                        if (Core.CoreIDPeak.Where(t => t.Item2 == coreFucFragementTasgs[0]).ToList().Count != 0)
                        {
                            MSPoint msp = Core.CoreIDPeak.Where(t => t.Item2 == coreFucFragementTasgs[0]).ToList()[0].Item1;
                            ExtraCoreGlycan.IDPeak(msp.Mass, msp.Intensity);
                        }
                        Core.AddGlycanToStructure(ExtraCoreGlycan, 1);

                        //HexNAc-(deHex-)HexNAc
                        ExtraCoreGlycan = new GlycanTreeNode(Glycan.Type.HexNAc, Core.NextID);
                        if (Core.CoreIDPeak.Where(t => t.Item2 == coreFucFragementTasgs[1]).ToList().Count != 0)
                        {
                            MSPoint msp = Core.CoreIDPeak.Where(t => t.Item2 == coreFucFragementTasgs[1]).ToList()[0].Item1;
                            ExtraCoreGlycan.IDPeak(msp.Mass, msp.Intensity);
                        }
                        Core.AddGlycanToStructure(ExtraCoreGlycan, 1);

                        //HexNAc-(deHex-)HexNAc-Hex
                        ExtraCoreGlycan = new GlycanTreeNode(Glycan.Type.Hex, Core.NextID);
                        if (Core.CoreIDPeak.Where(t => t.Item2 == coreFucFragementTasgs[2]).ToList().Count != 0)
                        {
                            MSPoint msp = Core.CoreIDPeak.Where(t => t.Item2 == coreFucFragementTasgs[2]).ToList()[0].Item1;
                            ExtraCoreGlycan.IDPeak(msp.Mass, msp.Intensity);
                        }
                        Core.AddGlycanToStructure(ExtraCoreGlycan, 3);

                        //HexNAc-(deHex-)HexNAc-Hex-Hex
                        ExtraCoreGlycan = new GlycanTreeNode(Glycan.Type.Hex, Core.NextID);
                        if (Core.CoreIDPeak.Where(t => t.Item2 == coreFucFragementTasgs[3]).ToList().Count != 0)
                        {
                            MSPoint msp = Core.CoreIDPeak.Where(t => t.Item2 == coreFucFragementTasgs[3]).ToList()[0].Item1;
                            ExtraCoreGlycan.IDPeak(msp.Mass, msp.Intensity);
                        }
                        Core.AddGlycanToStructure(ExtraCoreGlycan, 4);

                        //HexNAc-(deHex-)HexNAc-Hex-(Hex-)Hex
                        ExtraCoreGlycan = new GlycanTreeNode(Glycan.Type.Hex, Core.NextID);
                        if (Core.CoreIDPeak.Where(t => t.Item2 == coreFucFragementTasgs[4]).ToList().Count != 0)
                        {
                            MSPoint msp = Core.CoreIDPeak.Where(t => t.Item2 == coreFucFragementTasgs[4]).ToList()[0].Item1;
                            ExtraCoreGlycan.IDPeak(msp.Mass, msp.Intensity);
                        }
                        Core.AddGlycanToStructure(ExtraCoreGlycan, 4);
                        sequencedGlycanStructure.Add(Core);
                    }
                    
                    
                   //Core 
                    Core = new GlycanStructure(new Glycan(Glycan.Type.HexNAc, currentCharge), Y1);
                    Core.Charge = currentCharge;
                    Core.PrecursorMonoMass = _scan.ParentMonoMW;
                    //Store Core ID Peaks
                    for (int i = 0; i < 5; i++)
                    {
                        if (coreIdentifiedFragements[i] != 0)
                        {
                            Core.CoreIDPeak.Add(new Tuple<MSPoint, string>(allPeaks.Find(p => p.Mass == coreIdentifiedFragements[i]), coreFragementTags[i]));
                        }
                    }

                    ParentNode = Core.GetGlycanTreeByID(1);
                    //HexNAc-HexNAc
                    ExtraCoreGlycan = new GlycanTreeNode(Glycan.Type.HexNAc, Core.NextID);
                    if (Core.CoreIDPeak.Where(t => t.Item2 == coreFragementTags[1]).ToList().Count != 0)
                    {
                        MSPoint msp = Core.CoreIDPeak.Where(t => t.Item2 == coreFragementTags[1]).ToList()[0].Item1;
                        ExtraCoreGlycan.IDPeak(msp.Mass, msp.Intensity);
                    }
                    Core.AddGlycanToStructure(ExtraCoreGlycan, 1);
                        
                    //HexNAc-HexNAc-Hex
                    ExtraCoreGlycan = new GlycanTreeNode(Glycan.Type.Hex, Core.NextID);
                    if (Core.CoreIDPeak.Where(t => t.Item2 == coreFragementTags[2]).ToList().Count != 0)
                    {
                        MSPoint msp = Core.CoreIDPeak.Where(t => t.Item2 == coreFragementTags[2]).ToList()[0].Item1;
                        ExtraCoreGlycan.IDPeak(msp.Mass, msp.Intensity);
                    }
                    Core.AddGlycanToStructure(ExtraCoreGlycan, 2);

                    //HexNAc-HexNAc-Hex-Hex
                    ExtraCoreGlycan = new GlycanTreeNode(Glycan.Type.Hex, Core.NextID);
                    if (Core.CoreIDPeak.Where(t => t.Item2 == coreFragementTags[3]).ToList().Count != 0)
                    {
                        MSPoint msp = Core.CoreIDPeak.Where(t => t.Item2 == coreFragementTags[3]).ToList()[0].Item1;
                        ExtraCoreGlycan.IDPeak(msp.Mass, msp.Intensity);
                    }
                    Core.AddGlycanToStructure(ExtraCoreGlycan, 3);

                    //HexNAc-HexNAc-Hex-(Hex-)Hex
                    ExtraCoreGlycan = new GlycanTreeNode(Glycan.Type.Hex, Core.NextID);
                    if (Core.CoreIDPeak.Where(t => t.Item2 == coreFragementTags[4]).ToList().Count != 0)
                    {
                        MSPoint msp = Core.CoreIDPeak.Where(t => t.Item2 == coreFragementTags[4]).ToList()[0].Item1;
                        ExtraCoreGlycan.IDPeak(msp.Mass, msp.Intensity);
                    }
                    Core.AddGlycanToStructure(ExtraCoreGlycan, 3);
                    sequencedGlycanStructure.Add(Core);  //Add N-Glycan Core to candidate
                    #endregion coreSequencing
                    #region Branch Sequencing
                    lstBranchPeaks.Clear();
                    lstBranchPeaks.AddRange(
                        allPeaks.Where(t=> t.Mass >=coreTheorericalFragementsWithCoreFuc[4]-10 ).OrderByDescending(p => p.Intensity).Take(_topBrancingPeaks_l).OrderBy(x => x.Mass).ToList());

                    if (ClosedGlycan.Count != 0)
                    {
                        string[] glycanStr = new string[5] {"0", "0", "0", "0", "0"};
                        
                        foreach (GlycanCompound GC in ClosedGlycan)
                        {
                            string[] tmpGlycanStr = GC.GlycanKey.Split('-');
                            for (int i = 0; i < tmpGlycanStr.Length; i++)
                            {
                                if (tmpGlycanStr[i] != "0")
                                {
                                    if (i == 0 && Convert.ToInt32(tmpGlycanStr[i]) - 2 <=0) //HexNAc
                                    {
                                        continue;
                                    }
                                    if (i == 1 && Convert.ToInt32(tmpGlycanStr[i]) - 3 <= 0) //Hex
                                    {
                                        continue;
                                    }
                                    glycanStr[i] = "1";
                                }
                            }
                        }
                        string fullGlycanStr = glycanStr[0] + "-" + glycanStr[1] + "-" + glycanStr[2] + "-" +glycanStr[3] + "-" + glycanStr[4];
                        GenerateGlycanMass(currentCharge, fullGlycanStr,out _glycanBuildingblock);
                    }
                    else
                    {
                        GenerateGlycanMass(currentCharge, out _glycanBuildingblock);
                    }
                    _glycanBuildingblock=_glycanBuildingblock.OrderBy(t => t.Mz).ToList();
          
                    foreach (MSPoint pk in lstBranchPeaks)
                    {
                        for (int i = 0; i < sequencedGlycanStructure.Count; i++)
                        {
                            GlycanStructure CandidateTree = (GlycanStructure)sequencedGlycanStructure[i];
                            if (CandidateTree.Charge != currentCharge)
                            {
                                continue;
                            }
                            for (int GlycanIdx = 0; GlycanIdx < _glycanBuildingblock.Count; GlycanIdx++)
                            {
                                Glycan ExtraGlycan = _glycanBuildingblock[GlycanIdx];

                                if (ExtraGlycan.GlycanType == Glycan.Type.DeHex &&
                                    _NoDeHex - CandidateTree.NoOfDeHex <= 0)
                                {
                                    continue;
                                }

                                if (ExtraGlycan.GlycanType == Glycan.Type.HexNAc &&
                                    _NoHexNac - CandidateTree.NoOfHexNac <= 0)
                                {
                                    continue;
                                }
                                if (ExtraGlycan.GlycanType == Glycan.Type.NeuAc &&
                                    _NoNeuAc - CandidateTree.NoOfNeuAc <= 0)
                                {
                                    continue;
                                }
                                if (ExtraGlycan.GlycanType == Glycan.Type.NeuGc &&
                                    _NoNeuGc - CandidateTree.NoOfNeuGc <= 0)
                                {
                                    continue;
                                }
                                if ((ExtraGlycan.GlycanType == Glycan.Type.Hex ||
                                     ExtraGlycan.GlycanType == Glycan.Type.Gal ||
                                     ExtraGlycan.GlycanType == Glycan.Type.Man
                                    )
                                    &&
                                    _NoHex - CandidateTree.NoOfHex <= 0)
                                {
                                    continue;
                                }
                                if (_UseAVGMass)
                                {
                                    peptidemono = CandidateTree.Y1.Mass * CandidateTree.Charge - Atoms.ProtonMass * CandidateTree.Charge - HexNAcMass;
                                    float glycopeptideMass = CandidateTree.GlycanAVGMonoMass + peptidemono +ExtraGlycan.AVGMass;
                                    if (
                                        Math.Abs(((CandidateTree.GlycanAVGMonoMass + peptidemono + ExtraGlycan.AVGMass +
                                                   MassLib.Atoms.ProtonMass * currentCharge) / currentCharge) - pk.Mass) >_MS2torelance ||
                                         glycopeptideMass >_scan.ParentMonoMW)
                                    
                                    {
                                        continue;
                                    }
                                    else //Add One monosaccharide to Tree
                                    {
                                        if (ExtraGlycan.GlycanType == Glycan.Type.DeHex &&
                                            CandidateTree.Root.GlycanType != Glycan.Type.HexNAc)
                                        // DeHex only connect to HexNac
                                        {
                                            continue;
                                        }
                                        if ((CandidateTree.IUPACString.ToLower() == "hex-hexnac-hexnac" &&
                                             ExtraGlycan.GlycanType == Glycan.Type.HexNAc) ||
                                            (CandidateTree.IUPACString.ToLower() == "hex-hexnac-(dehex-)hexnac" &&
                                             ExtraGlycan.GlycanType == Glycan.Type.HexNAc)
                                            )
                                        {
                                            isBiscectingSignal = true;
                                        }

                                        GlycanStructure CloneTree = (GlycanStructure)CandidateTree.Clone();

                                        List<GlycanStructure> AddedTrees = AddGlycanToNGlycanTree(CloneTree, ExtraGlycan,
                                            pk.Mass, pk.Intensity, isBiscectingSignal, IsCoreFuc);

                                        if (_isDebug)
                                        {
                                            string tmp = pk.Mass.ToString("0.000") + "," + CloneTree.IUPACString + "," +
                                                         ExtraGlycan.GlycanType.ToString() + ",";
                                            List<string> Structure = new List<string>();
                                            foreach (GlycanStructure T in AddedTrees)
                                            {
                                                if (!Structure.Contains(T.IUPACString))
                                                {
                                                    Structure.Add(T.IUPACString);
                                                }
                                            }
                                            foreach (string str in Structure)
                                            {
                                                tmp = tmp + str + ",";
                                            }
                                            DebugSW.WriteLine(tmp.Substring(0, tmp.Length - 1));
                                        }
                                        foreach (GlycanStructure iterGT in AddedTrees)
                                        {
                                            iterGT.BranchIDPeak.Add(new Tuple<MSPoint, string>(pk, iterGT.IUPACString));
                                            iterGT.Root.SortSubTree();
                                            iterGT.Charge = currentCharge;
                                            if (!sequencedGlycanStructure.Contains(iterGT))
                                            {
                                                sequencedGlycanStructure.Add(iterGT);
                                            }
                                        }
                                    }
                                }
                            } //iterate Glycan

                        } //All Candidate Structure
                    }//iterate branch peaks
                    #endregion Branch Sequencing
                    //Generate Combination
                    List<Tuple<float, int, int, int, int, int>> allCombinations = GlycanCombination.GetAllGlycanCombination(_maxGlycansToCompleteStruct_m);

                    sequencedGlycanStructure.Sort((a, b) => -1 * a.NoOfTotalGlycan.CompareTo(b.NoOfTotalGlycan));
                   
                    //Add extra glycan
                    foreach (GlycanStructure t in sequencedGlycanStructure)
                    {
                        peptidemono = t.Y1.Mass * t.Charge - Atoms.ProtonMass * t.Charge - HexNAcMass;
                        //if (_FullSequencedGlycanStructure.Count == 0) 
                        
                            float GlycanMassDifferent = _scan.ParentMonoMW - GetGlycanMass(t) - peptidemono;
                            Tuple<float, int, int, int, int, int> TargetCombination = GlycanCombination.GetClosedCombinationByMass(allCombinations, GlycanMassDifferent);
                            if (TargetCombination == null || Math.Abs(TargetCombination.Item1 - GlycanMassDifferent) > _MS2torelance)
                            {
                                if (_SequencedGlycanStructure.Where(x => x.IUPACString == t.IUPACString && x.Y1.Mass ==t.Y1.Mass).ToList().Count == 0)
                                {
                                    _SequencedGlycanStructure.Add((GlycanStructure)t.Clone());
                                }        
                                continue;
                            }
                           
                            float TotalMass = peptidemono + (t.NoOfHexNac + TargetCombination.Item2) * HexNAcMass +
                                              (t.NoOfHex + TargetCombination.Item3) * HexMass +
                                              (t.NoOfDeHex + TargetCombination.Item4) * deHexMass +
                                              (t.NoOfNeuAc + TargetCombination.Item5) * NeuACMass +
                                              (t.NoOfNeuGc + TargetCombination.Item6) * NeuGCMass;

                            if (Math.Abs(TotalMass - _scan.ParentMonoMW) <= _MS2torelance)
                            {
                                t.RestGlycanString = TargetCombination.Item2.ToString() + "-" +
                                                     TargetCombination.Item3.ToString() + "-" +
                                                     TargetCombination.Item4.ToString() + "-" +
                                                     TargetCombination.Item5.ToString() + "-" +
                                                     TargetCombination.Item6.ToString();

                                if ((TargetCombination.Item2 + TargetCombination.Item3 +
                                     TargetCombination.Item4 + TargetCombination.Item5 +
                                     TargetCombination.Item6) > 1)
                                {
                                    t.IsCompleteByPrecursorDifference = false;
                                    t.InCompleteScore = 0 - (TargetCombination.Item2 + TargetCombination.Item3 +
                                                             TargetCombination.Item4 + TargetCombination.Item5 +
                                                             TargetCombination.Item6);
                                }
                                else
                                {
                                    t.IsCompleteByPrecursorDifference = true;   
                                }
                            }
                        //Assign Peptide 
                        if (MatchedGlycoPeptides.Count != 0 )
                        {
                            foreach (Tuple<TargetPeptide, GlycanCompound> glycopeptide in MatchedGlycoPeptides.Where(x => t.FullSequencedGlycanString == x.Item2.GlycanKey))
                            {
                                AminoAcidMass AAMS = new AminoAcidMass();
                                float theoreticalPeptideMass = AAMS.GetMonoMW(glycopeptide.Item1.PeptideSequence, true);
                                float glycanMass = (t.NoOfHexNac + TargetCombination.Item2) * HexNAcMass +
                                                                  (t.NoOfHex + TargetCombination.Item3) * HexMass +
                                                                  (t.NoOfDeHex + TargetCombination.Item4) * deHexMass +
                                                                  (t.NoOfNeuAc + TargetCombination.Item5) * NeuACMass +
                                                                  (t.NoOfNeuGc + TargetCombination.Item6) * NeuGCMass;

                                if (MassUtility.GetMassPPM(theoreticalPeptideMass + glycanMass, _scan.ParentMonoMW) <=
                                    _precursorTorelance)
                                {
                                    t.TargetPeptide = glycopeptide.Item1;
                                    List<GlycanStructure> tmpStructures =_FullSequencedGlycanStructure.Where(x =>x.PeptideSequence == t.PeptideSequence &&x.FullSequencedGlycanString == t.FullSequencedGlycanString&&x.IUPACString==t.IUPACString).ToList().OrderByDescending(x=>x.BranchScore).ToList();
                                    if (tmpStructures.Count == 0)
                                    {
                                        
                                        _FullSequencedGlycanStructure.Add((GlycanStructure)t.Clone());
                                        _FullSequencedGlycanStructure[_FullSequencedGlycanStructure.Count - 1].MatchWithPrecursorMW = true;
                                    }
                                    else
                                    {
                                        if (tmpStructures[0].BranchScore < t.BranchScore) //replace record with higher score
                                        {
                                            _FullSequencedGlycanStructure.RemoveAt(_FullSequencedGlycanStructure.IndexOf(tmpStructures[0]));
                                            _FullSequencedGlycanStructure.Add((GlycanStructure)t.Clone());
                                            _FullSequencedGlycanStructure[_FullSequencedGlycanStructure.Count - 1].MatchWithPrecursorMW = true;
                                        }
                                    }
                                }
                                else
                                {
                                    if (_SequencedGlycanStructure.Where(x => x.IUPACString == t.IUPACString).ToList().Count == 0)
                                    {
                                        _SequencedGlycanStructure.Add((GlycanStructure) t.Clone());
                                        _SequencedGlycanStructure[_SequencedGlycanStructure.Count - 1].MatchWithPrecursorMW = false;
                                    }
                                }
                            }
                        }
                        else if (lstGlycans == null && ClosedPeptides.Count != 0)
                        {
                            foreach (TargetPeptide tPeptide in ClosedPeptides)
                            {
                                AminoAcidMass AAMS = new AminoAcidMass();
                                float theoreticalPeptideMass = AAMS.GetMonoMW(tPeptide.PeptideSequence, true);
                                float glycanMass = (t.NoOfHexNac + TargetCombination.Item2) * HexNAcMass +
                                                                  (t.NoOfHex + TargetCombination.Item3) * HexMass +
                                                                  (t.NoOfDeHex + TargetCombination.Item4) * deHexMass +
                                                                  (t.NoOfNeuAc + TargetCombination.Item5) * NeuACMass +
                                                                  (t.NoOfNeuGc + TargetCombination.Item6) * NeuGCMass;

                                if (MassUtility.GetMassPPM(theoreticalPeptideMass + glycanMass, _scan.ParentMonoMW) <=
                                    _precursorTorelance)
                                {
                                    t.TargetPeptide = tPeptide;
                                    List<GlycanStructure> tmpStructures = _FullSequencedGlycanStructure.Where(x => x.PeptideSequence == t.PeptideSequence && x.FullSequencedGlycanString == t.FullSequencedGlycanString && x.IUPACString==t.IUPACString).ToList().OrderByDescending(x => x.BranchScore).ToList();
                                    if (tmpStructures.Count == 0)
                                    {

                                        _FullSequencedGlycanStructure.Add((GlycanStructure)t.Clone());
                                        _FullSequencedGlycanStructure[_FullSequencedGlycanStructure.Count - 1].MatchWithPrecursorMW = true;
                                    }
                                    else
                                    {
                                        if (tmpStructures[0].BranchScore < t.BranchScore) //replace record with higher score
                                        {
                                            _FullSequencedGlycanStructure.RemoveAt(_FullSequencedGlycanStructure.IndexOf(tmpStructures[0]));
                                            _FullSequencedGlycanStructure.Add((GlycanStructure)t.Clone());
                                            _FullSequencedGlycanStructure[_FullSequencedGlycanStructure.Count - 1].MatchWithPrecursorMW = true;
                                        }
                                    }
                                }
                                else
                                {
                                    if (_SequencedGlycanStructure.Where(x => x.IUPACString == t.IUPACString).ToList().Count == 0)
                                    {
                                        _SequencedGlycanStructure.Add((GlycanStructure)t.Clone());
                                        _SequencedGlycanStructure[_SequencedGlycanStructure.Count - 1].MatchWithPrecursorMW = false;
                                    }
                                }
                            }
                        }
                        else if (_UnknownPeptideSearch)
                        {
                            if (Math.Abs(TotalMass - _scan.ParentMonoMW) <= _MS2torelance)
                            {
                                t.IsCompleteByPrecursorDifference = true;
                                t.RestGlycanString = TargetCombination.Item2.ToString() + "-" +
                                                     TargetCombination.Item3.ToString() + "-" +
                                                     TargetCombination.Item4.ToString() + "-" +
                                                     TargetCombination.Item5.ToString() + "-" +
                                                     TargetCombination.Item6.ToString();
                                t.InCompleteScore = 0 - (TargetCombination.Item2 + TargetCombination.Item3 +
                                                         TargetCombination.Item4 + TargetCombination.Item5 +
                                                         TargetCombination.Item6);

                                List<GlycanStructure> tmpStructures = _FullSequencedGlycanStructure.Where(x => x.Y1.Mass == t.Y1.Mass && x.FullSequencedGlycanString == t.FullSequencedGlycanString &&t.IUPACString==x.IUPACString).ToList().OrderByDescending(x => x.BranchScore).ToList();
                                //Get Peptide
                                if (tmpStructures.Count == 0)
                                {
                                    _FullSequencedGlycanStructure.Add((GlycanStructure)t.Clone());
                                    _FullSequencedGlycanStructure[_FullSequencedGlycanStructure.Count - 1].MatchWithPrecursorMW = true;
                                }
                                else if (tmpStructures[0].BranchScore < t.BranchScore) //replace record with higher score
                                {
                                    _FullSequencedGlycanStructure.RemoveAt(_FullSequencedGlycanStructure.IndexOf(tmpStructures[0]));
                                    _FullSequencedGlycanStructure.Add((GlycanStructure)t.Clone());
                                    _FullSequencedGlycanStructure[_FullSequencedGlycanStructure.Count - 1].MatchWithPrecursorMW = true;
                                }
                            }
                            else
                            {
                                if (_SequencedGlycanStructure.Where(x => x.IUPACString == t.IUPACString).ToList().Count == 0)
                                {
                                    _SequencedGlycanStructure.Add((GlycanStructure)t.Clone());
                                    _SequencedGlycanStructure[_SequencedGlycanStructure.Count - 1].MatchWithPrecursorMW = false;
                                }
                            }
                        }
                    }
                }//Charge
            }//iterate Y1 peaks
            //List<GlycanStructure> tmpLstGS = new List<GlycanStructure>();

            //foreach (GlycanStructure GS in _FullSequencedGlycanStructure)
            //{
            //    //Assign Peptide
            //    float peptidemono = GS.PrecursorMonoMass - GS.GlycanMonoMass;
            //    List<TargetPeptide> ClosedPeptides = FindMatchedPeptides(peptidemono);
                
            //    if (ClosedPeptides.Count != 0)
            //    {
            //        AminoAcidMass AAMS = new AminoAcidMass();
            //        foreach (TargetPeptide closedpeptide in ClosedPeptides)
            //        {
            //            float theoreticalPeptideMass = AAMS.GetMonoMW(closedpeptide.PeptideSequence, true);

            //            for (int i = _precursorCharge - 1; i >= 1; i--)
            //            {
            //                float theoreticalY1MZ = (theoreticalPeptideMass + Atoms.ProtonMass*i +
            //                                              GlycanMass.GetGlycanMass(Glycan.Type.HexNAc))/(int) i;
            //                if (Math.Abs(theoreticalY1MZ - GS.Y1.Mass) < _MS2torelance)
            //                {
            //                    GS.TargetPeptide = closedpeptide;
            //                    if (
            //                        !tmpLstGS.Where(s =>
            //                            s.PeptideSequence == GS.PeptideSequence &&
            //                            s.IUPACString == GS.IUPACString &&
            //                            s.Charge == GS.Charge).Any())   
            //                    {
            //                        tmpLstGS.Add((GlycanStructure) GS.Clone());
            //                    }
            //                }
            //            }
            //        }
            //    }
            //    else //No peptide found
            //    {
            //        if (
            //                !tmpLstGS.Where(s =>
            //                    s.IUPACString == GS.IUPACString &&
            //                    s.Charge == GS.Charge).Any())
            //                {
            //                    tmpLstGS.Add((GlycanStructure)GS.Clone());
            //                }
            //    }
            //}
            //Check Complete Status
            //_FullSequencedGlycanStructure = new List<GlycanStructure>();
            //foreach (GlycanStructure GS in tmpLstGS)
            //{
            //    if (GS.TargetPeptide != null && GS.PPM <= PrecursorTorelance)
            //    {
            //        GS.IsCompleteSequenced = true;
            //        _FullSequencedGlycanStructure.Add(GS);
            //    }
            //}
            //_FullSequencedGlycanStructure = tmpLstGS;
        }
        private void GetAllPeaks()
        {
            try
            {
                _allPeaks = new List<MSPoint>();
                for (int i = 0; i < _scan.MZs.Length; i++)
                {

                    _allPeaks.Add(new MSPoint(_scan.MZs[i], _scan.Intensities[i]));
                    if (_scan.Intensities[i] > MaxIntensityInScan)
                    {
                        MaxIntensityInScan = (float)_scan.Intensities[i];
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
        }

        private void GenerateGlycanMass(int argCharge, out List<Glycan> argBuildingBlocks)
        {
            argBuildingBlocks = new List<Glycan>();
            if (_NoNeuAc != 0)
            {
                argBuildingBlocks.Add(new Glycan(Glycan.Type.NeuAc, argCharge));
            }
            if (_NoNeuGc != 0)
            {
                argBuildingBlocks.Add(new Glycan(Glycan.Type.NeuGc, argCharge));
            }

            if (_NoHexNac != 0)
            {
                argBuildingBlocks.Add(new Glycan(Glycan.Type.HexNAc, argCharge));
            }
            if (_NoHex != 0)
            {
                argBuildingBlocks.Add(new Glycan(Glycan.Type.Hex, argCharge));
            }
            if (_NoDeHex != 0)
            {
                argBuildingBlocks.Add(new Glycan(Glycan.Type.DeHex, argCharge));
            }
        }
        private void GenerateGlycanMass(int argCharge,string argGlycan ,out List<Glycan> argBuildingBlocks)
        {
            argBuildingBlocks = new List<Glycan>();
            string[] glycanString = argGlycan.Split('-');
            if (glycanString[3] != "0")
            {
                argBuildingBlocks.Add(new Glycan(Glycan.Type.NeuAc, argCharge));
            }
            if (glycanString[4] != "0")
            {
                argBuildingBlocks.Add(new Glycan(Glycan.Type.NeuGc, argCharge));
            }

            if (glycanString[0] != "0")
            {
                argBuildingBlocks.Add(new Glycan(Glycan.Type.HexNAc, argCharge));
            }
            if (glycanString[1] != "0")
            {
                argBuildingBlocks.Add(new Glycan(Glycan.Type.Hex, argCharge));
            }
            if (glycanString[2] != "0")
            {
                argBuildingBlocks.Add(new Glycan(Glycan.Type.DeHex, argCharge));
            }
        }
        /// <summary>
        /// Get top x normized peaks
        /// </summary>
        /// <param name="argNoOfPeaks">Top x peaks</param>
        private void GetCandidatePeaks(int argNoOfPeaks)
        {
            _potentialPeaks = new List<MSPoint>();
            _allPeaks.Sort(delegate(MSPoint P1, MSPoint P2) { return -1 * Comparer<float>.Default.Compare(P1.Intensity, P2.Intensity); });  //sorted by intensity         

            for (int i = 0; i < _allPeaks.Count; i++)
            {
                if (_allPeaks[i].Mass >= _y1MZ - 2.0f)
                {
                    _potentialPeaks.Add(new MSPoint(_allPeaks[i].Mass, _allPeaks[i].Intensity / MaxIntensityInScan));
                    if (_potentialPeaks.Count >= argNoOfPeaks)
                    {
                        break;
                    }
                }
            }
            if (_potentialPeaks.Count != 0)
            {
                float MinIntensity = _potentialPeaks[_potentialPeaks.Count - 1].Intensity;
                foreach (MSPoint p in _potentialPeaks)
                {
                    p.Intensity = Convert.ToSingle(Math.Log10(p.Intensity / MinIntensity));
                }
            }
            _potentialPeaks.Sort();
            _allPeaks.Sort();
        }
        private int FindClosedPeakIdx(List<MSPoint> argLstPoints, float argTargetMz)
        {
            float Distance = 10000.0f;
            int smallidx = 0;
            for (int i = 0; i < argLstPoints.Count; i++)
            {
                if (Math.Abs(argLstPoints[i].Mass - argTargetMz) < Distance)
                {
                    Distance = Math.Abs(argLstPoints[i].Mass - argTargetMz);
                    smallidx = i;
                }
            }
            return smallidx;
        }
        private int FindClosedPeakIdx(float argTargetMz)
        {
            float Distance = 10000.0f;
            int smallidx = 0;
            for (int i = 0; i < _potentialPeaks.Count; i++)
            {
                if (Math.Abs(_potentialPeaks[i].Mass - argTargetMz) < Distance)
                {
                    Distance = Math.Abs(_potentialPeaks[i].Mass - argTargetMz);
                    smallidx = i;
                }
            }
            return smallidx;
        }
        private float GetGlycanMass(GlycanStructure argGlycan)
        {
            float GlycanMass = 0.0f;
            GlycanMass = GlycanMass + argGlycan.NoOfHex * GlycoLib.GlycanMass.GetGlycanMass(Glycan.Type.Hex);
            GlycanMass = GlycanMass + argGlycan.NoOfHexNac * GlycoLib.GlycanMass.GetGlycanMass(Glycan.Type.HexNAc);
            GlycanMass = GlycanMass + argGlycan.NoOfDeHex * GlycoLib.GlycanMass.GetGlycanMass(Glycan.Type.DeHex);
            GlycanMass = GlycanMass + argGlycan.NoOfNeuAc * GlycoLib.GlycanMass.GetGlycanMass(Glycan.Type.NeuAc);
            GlycanMass = GlycanMass + argGlycan.NoOfNeuGc * GlycoLib.GlycanMass.GetGlycanMass(Glycan.Type.NeuGc);
            return GlycanMass;
        }
        private List<GlycanStructure> AddGlycanToNGlycanTree(GlycanStructure argParentTree, Glycan argGlycan, float argMZ, float argIntensity, bool argIsBisecting = false, bool argIsCoreFucose = false)
        {
            List<GlycanStructure> addedGT = new List<GlycanStructure>();
            List<GlycanTreeNode> AllGlycanNodeInParent = argParentTree.Root.FetchAllGlycanNode();
            List<int> AddPosition = new List<int>(); //AddPosition using GlycanNodeID start with 1;

            #region DeHex
            if (argGlycan.GlycanType == Glycan.Type.DeHex) // DeHex can place at the 1st core HexNac, and Non-reducing end HexNac  [RULE 8]
            {
                if (argParentTree.Root.NoOfDeHex == 0 && argIsCoreFucose) //Fill out the core fucose
                {
                    AddPosition.Add(argParentTree.Root.NodeID);
                }
                else
                {
                    foreach (GlycanTreeNode T in AllGlycanNodeInParent)
                    {
                        //if (argIsCoreFucose)
                        //{

                        //    if (T.GlycanType == Glycan.Type.HexNAc && T.DistanceRoot != 3 && (T.Subtrees == null || T.Subtrees.Count < 4) && T.DistanceRoot != 1 && T.NoOfDeHex == 0)
                        //    {
                        //        AddPosition.Add(T.NodeID);
                        //    }
                        //}


                        if (T.GlycanType == Glycan.Type.HexNAc && T.DistanceRoot > 3 && (T.Subtrees == null || T.Subtrees.Count < 4) && T.NoOfDeHexInChild == 0)
                        {
                            AddPosition.Add(T.NodeID);
                        }

                    }
                }

            }
            #endregion
            #region HexNAc
            else if (argGlycan.GlycanType == Glycan.Type.HexNAc)
            {
                if (argParentTree.HasNGlycanCore())
                {
                    int NoOfHexNACRemain = argParentTree.NoOfHexNac - 2;
                    int NoOfHexRemain = argParentTree.NoOfHex - 3;

                    if (NoOfHexNACRemain == 0 && NoOfHexRemain == 0) //Attache HexNAc to core before any thing attach to it [RULE 4]
                    {
                        foreach (GlycanTreeNode T in AllGlycanNodeInParent)
                        {
                            if (argIsBisecting)
                            {
                                if ((T.DistanceRoot == 2 || T.DistanceRoot == 3) && T.GlycanType == Glycan.Type.Hex)
                                {
                                    AddPosition.Add(T.NodeID);
                                }
                            }
                            else
                            {   //No Bisecting
                                if ((T.DistanceRoot == 3) && T.GlycanType == Glycan.Type.Hex)
                                {
                                    AddPosition.Add(T.NodeID);
                                }
                            }
                        }
                    }
                    else if (NoOfHexRemain > NoOfHexNACRemain)//  Hybrid Structure[RULE 9]  HexNAC-Hex reapeat [RULE 10]
                    {
                        foreach (GlycanTreeNode T in AllGlycanNodeInParent)
                        {
                            if (T.DistanceRoot == 2 && T.NoOfHexNacInChild == 0 && argIsBisecting)
                            {
                                AddPosition.Add(T.NodeID); //bisecting
                            }
                            else //Add to branch (none high mannos branch)
                            {
                                if (T.DistanceRoot >= 3 && T.DistanceRoot % 2 == 1 && T.GlycanType == Glycan.Type.Hex && T.Subtrees == null)
                                {
                                    //HexNAC-Hex reapeat [RULE 10]       
                                    AddPosition.Add(T.NodeID);
                                }
                            }
                        }
                    }
                    else //HexNAc can't attach to another HexNAc [RULE 5]
                    {
                        foreach (GlycanTreeNode T in AllGlycanNodeInParent)
                        {
                            if (T.DistanceRoot >= 3 && T.GlycanType == Glycan.Type.Hex)
                            {
                                if (T.Subtrees == null && T.Parent.GlycanType != Glycan.Type.Hex)
                                {
                                    AddPosition.Add(T.NodeID); //Add to First HexNac;
                                }
                                else if (T.DistanceRoot == 3 && (T.Subtrees == null || T.Subtrees.Count <= 1)) //Branching
                                {
                                    AddPosition.Add(T.NodeID);
                                }
                                else
                                {
                                    //if (T.DistanceRoot >= 4)
                                    //{
                                    //    continue;
                                    //}

                                    //if (T.NoOfHexNacInChild == 0 && T.DistanceRoot == 3 )
                                    //{
                                    //    AddPosition.Add(T.NodeID); //Add to First HexNac;
                                    //}

                                }
                            }
                            if (T.DistanceRoot == 2 && T.NoOfHexNacInChild == 0 && argIsBisecting)
                            {
                                AddPosition.Add(T.NodeID); //Add Bisecting GlcNAc ;
                            }
                        }
                    }
                }
                else // Core Structure [RULE 2]
                {
                    foreach (GlycanTreeNode T in AllGlycanNodeInParent)
                    {
                        if (T.DistanceRoot == 0 && T.GlycanType == Glycan.Type.HexNAc)
                        {
                            //int NoOfHexNAc = 0;
                            if (T.Subtrees == null)
                            {
                                AddPosition.Add(T.NodeID); //Add to First HexNac Tree root;
                            }
                            else  //A deHex already attach on it (DeHex-HexNAc)
                            {
                                if (T.NoOfHexNacInChild == 0)
                                {
                                    AddPosition.Add(T.NodeID); //Add to First HexNac; HexNAc-(DeHex)-HexNAc
                                }
                            }
                        }
                        if (T.DistanceRoot == 2 && T.NoOfHexNacInChild == 0 && argIsBisecting) //Bisecting 
                        {
                            AddPosition.Add(T.NodeID);
                        }
                        if (T.DistanceRoot == 3 && T.GlycanType == Glycan.Type.Hex && (T.Subtrees == null || T.Subtrees.Count <= 1)) //Branching 
                        {
                            AddPosition.Add(T.NodeID);
                        }
                        //if (T.GlycanType == Glycan.Type.Hex && T.Subtrees == null) //Add to terminal Hex
                        //{
                        //    AddPosition.Add(T.NodeID);
                        //}
                    }
                }
            }
            #endregion HexNAc
            #region Hex
            else if (argGlycan.GlycanType == Glycan.Type.Hex)
            {
                if (argParentTree.HasNGlycanCore())
                {
                    int NoOfHexNACRemain = argParentTree.NoOfHexNac - 2;
                    int NoOfHexRemain = argParentTree.NoOfHex - 3;
                    if (NoOfHexNACRemain == 0)
                    {
                        //AttachToAntenna [RULE 3]
                        foreach (GlycanTreeNode T in AllGlycanNodeInParent)
                        {
                            if (T.GlycanType == Glycan.Type.Hex && T.Subtrees == null && T.DistanceRoot >= 3)  //Add to leaf Hex
                            {
                                if (!(T.Parent.GlycanType == Glycan.Type.HexNAc))
                                {
                                    AddPosition.Add(T.NodeID);
                                }
                            }
                            if (T.DistanceRoot > 3 && T.GlycanType == Glycan.Type.HexNAc && (T.NoOfTotalGlycan == T.NoOfDeHex))
                            {
                                AddPosition.Add(T.NodeID);
                            }
                        }
                    }
                    else //[RULE 9]
                    {
                        foreach (GlycanTreeNode T in AllGlycanNodeInParent)
                        {
                            if ((T.DistanceRoot > 3 && T.Subtrees == null && T.GlycanType == Glycan.Type.Hex && T.Parent.GlycanType != Glycan.Type.HexNAc) || //High Man
                                (T.DistanceRoot > 3 && T.Subtrees == null && T.GlycanType == Glycan.Type.HexNAc) ||//Add to leaf Hex //Complex
                                (T.DistanceRoot == 3 && T.GlycanType == Glycan.Type.Hex && (T.Subtrees == null || T.Subtrees.Count <= 1))
                                )
                            {
                                AddPosition.Add(T.NodeID);
                            }

                        }
                    }
                    //Attach to Terminal Hex
                    //foreach (GlycanTreeNode T in AllGlycanNodeInParent)
                    //{
                    //    if (T.GlycanType == Glycan.Type.Hex && T.Subtrees == null && T.Parent.GlycanType != Glycan.Type.HexNAc)  //Add to leaf Hex
                    //    {
                    //        AddPosition.Add(T.NodeID);
                    //    }
                    //}
                }
                else  // Core Structure [RULE 2]
                {
                    if (argParentTree.NoOfHex == 0)
                    {
                        foreach (GlycanTreeNode T in AllGlycanNodeInParent)
                        {
                            if (T.DistanceRoot == 1 && T.GlycanType == Glycan.Type.HexNAc)
                            {
                                AddPosition.Add(T.NodeID);
                            }
                        }
                    }
                    else
                    {
                        foreach (GlycanTreeNode T in AllGlycanNodeInParent)
                        {
                            if ((T.DistanceRoot == 2 && T.GlycanType == Glycan.Type.Hex && T.NoOfHexInChild <= 1) || //core
                                (T.DistanceRoot == 3 && T.GlycanType == Glycan.Type.Hex && (T.Subtrees == null || T.Subtrees.Count == 1)))
                            {
                                AddPosition.Add(T.NodeID);
                            }
                            else if (T.DistanceRoot > 3 && T.GlycanType == Glycan.Type.HexNAc && T.Subtrees == null)
                            {
                                AddPosition.Add(T.NodeID);
                            }
                            else if (T.DistanceRoot > 3 && T.GlycanType == Glycan.Type.Hex && T.Subtrees == null && T.Parent.GlycanType == Glycan.Type.Hex) //High Man
                            {
                                AddPosition.Add(T.NodeID);
                            }
                        }
                    }
                }
            }
            #endregion Hex
            #region NeuAc
            else //NeuAc,NeuGc
            {
                foreach (GlycanTreeNode T in AllGlycanNodeInParent) //Sialic acid only attach to terminal Hex(Gal) [RULE 6]
                {
                    if (T.DistanceRoot >= 3 && T.GlycanType == Glycan.Type.Hex && T.Subtrees == null)
                    {
                        if (T.HasHexNAcAncestorOtherThanCore())
                        {
                            AddPosition.Add(T.NodeID);
                        }
                    }
                }
                if (_allowSiaConnectToHexNac)
                {
                    if (AddPosition.Count == 0) //All Terminal Hex are occupied
                    {
                        foreach (GlycanTreeNode T in AllGlycanNodeInParent) //sialic acid can be attached to HexNAc, but that when all the terminal Hex are occupied [RULE 7]
                        {
                            if (T.DistanceRoot > 3 && T.GlycanType == Glycan.Type.HexNAc && T.Subtrees == null)
                            {
                                AddPosition.Add(T.NodeID);
                            }
                        }
                    }
                }
            }
            #endregion NeuAc
            for (int i = 0; i < AddPosition.Count; i++)
            {
                int AddedIndex = AddPosition[i];
                GlycanStructure newTree = (GlycanStructure)argParentTree.Clone();
                GlycanTreeNode ToBeAddGlycanTree = new GlycanTreeNode(argGlycan.GlycanType, newTree.NextID);
                ToBeAddGlycanTree.IDPeak(argMZ, argIntensity);
                ToBeAddGlycanTree.Parent = newTree.GetGlycanTreeByID(AddedIndex);

                if (ToBeAddGlycanTree.Parent.GlycanType != Glycan.Type.DeHex &&
                    ToBeAddGlycanTree.Parent.GlycanType != Glycan.Type.NeuAc &&
                    ToBeAddGlycanTree.Parent.GlycanType != Glycan.Type.NeuGc &&
                    (ToBeAddGlycanTree.Parent.Subtrees == null || ToBeAddGlycanTree.Parent.Subtrees.Count < 4))
                {
                    newTree.AddGlycanToStructure(ToBeAddGlycanTree, ToBeAddGlycanTree.Parent.NodeID);
                    newTree.Root.SortSubTree();
                    addedGT.Add(newTree);
                }
            }
            return addedGT;
        }
        
        
        







        //*************************************************************
        //Referance,  old, non used code
        //*************************************************************
        //private void FindOLinkedStructure()
        //{
        //    #region AVG Mass
        //    if (_UseAVGMass)
        //    {
        //        if (_scan.ParentAVGMonoMW != 0.0f)
        //        {
        //            UpdateStatus("Parent avg mono found");
        //            _PrecusorMono = _scan.ParentAVGMonoMW;
        //        }
        //        else
        //        {

        //            // try to recover PrecusorMono
        //            if (_peptideStr != "")
        //            {
        //                UpdateStatus("Parent avg mono not found use peptide and composition");
        //                AminoAcidMass MW = new AminoAcidMass();
        //                _PrecusorMono = MW.GetAVGMonoMW(_peptideStr, _isCYS_CAM) +
        //                                                 GlycanMass.GetGlycanAVGMass(Glycan.Type.HexNAc) * _NoHexNac +
        //                                                 GlycanMass.GetGlycanAVGMass(Glycan.Type.Hex) * _NoHex +
        //                                                 GlycanMass.GetGlycanAVGMass(Glycan.Type.DeHex) * _NoDeHex +
        //                                                 GlycanMass.GetGlycanAVGMass(Glycan.Type.NeuAc) * _NoNeuAc +
        //                                                 GlycanMass.GetGlycanAVGMass(Glycan.Type.NeuGc) * _NoNeuGc;
        //                _y1MZ = (MW.GetAVGMonoMW(_peptideStr, _isCYS_CAM) + GlycanMass.GetGlycanAVGMass(Glycan.Type.HexNAc) + Atoms.ProtonMass * _y1charge) / _y1charge;
        //            }
        //            else if (_scan.ParentMonoMW != 0.0)
        //            {
        //                UpdateStatus("Parent avg mono not found use parent mono");
        //                _PrecusorMono = _scan.ParentMonoMW;
        //            }
        //            else
        //            {
        //                UpdateStatus("Parent avg mono not found use header mz");
        //                _PrecusorMono = (_scan.ParentMZ - Atoms.ProtonMass) * _scan.ParentMZ;
        //            }
        //        } //End  of Recovery Precursor mono


        //        if (_y1MZ == 0.0f)
        //        {
        //            AminoAcidMass MW = new AminoAcidMass();
        //            _y1MZ = (MW.GetAVGMonoMW(_peptideStr, _isCYS_CAM) + GlycanMass.GetGlycanAVGMass(Glycan.Type.HexNAc) + Atoms.ProtonMass * _y1charge) / _y1charge;
        //        }
        //        _peptideMass = (float)(_y1MZ - GlycanMass.GetGlycanAVGMasswithCharge(Glycan.Type.HexNAc, _y1charge)) * _y1charge - MassLib.Atoms.ProtonMass;
        //        _glycanMonoMass = (float)(_PrecusorMono - _peptideMass);
        //        //_peptideMz = _y1mass - GlycanMass.GetGlycanAVGMasswithCharge(Glycan.Type.HexNAc, _y1charge);

        //    }
        //    #endregion
        //    #region Use Mono Mass
        //    else // Use mono mass
        //    {
        //        if (_scan.ParentMonoMW != 0.0f)
        //        {
        //            _PrecusorMono = _scan.ParentMonoMW;
        //        }
        //        else
        //        {
        //            // try to recover PrecusorMono
        //            if (_peptideStr != "")
        //            {
        //                AminoAcidMass MW = new AminoAcidMass();
        //                _PrecusorMono = MW.GetMonoMW(_peptideStr, _isCYS_CAM) +
        //                                                 GlycanMass.GetGlycanMass(Glycan.Type.HexNAc) * _NoHexNac +
        //                                                 GlycanMass.GetGlycanMass(Glycan.Type.Hex) * _NoHex +
        //                                                 GlycanMass.GetGlycanMass(Glycan.Type.DeHex) * _NoDeHex +
        //                                                 GlycanMass.GetGlycanMass(Glycan.Type.NeuAc) * _NoNeuAc +
        //                                                 GlycanMass.GetGlycanMass(Glycan.Type.NeuGc) * _NoNeuGc;
        //                _y1MZ = (MW.GetMonoMW(_peptideStr, _isCYS_CAM) + GlycanMass.GetGlycanMass(Glycan.Type.HexNAc) + Atoms.ProtonMass * _y1charge) / _y1charge;
        //            }
        //            else
        //            {
        //                _PrecusorMono = (_scan.ParentMZ - Atoms.ProtonMass) * _scan.ParentMZ;
        //            }
        //        }
        //        if (_y1MZ == 0.0f)
        //        {
        //            AminoAcidMass MW = new AminoAcidMass();
        //            _y1MZ = (MW.GetMonoMW(_peptideStr, _isCYS_CAM) + GlycanMass.GetGlycanMass(Glycan.Type.HexNAc) + Atoms.ProtonMass * _y1charge) / _y1charge;
        //        }
        //        _peptideMass = (float)(_y1MZ - GlycanMass.GetGlycanMasswithCharge(Glycan.Type.HexNAc, _y1charge)) * _y1charge - MassLib.Atoms.ProtonMass;
        //        _glycanMonoMass = (float)(_PrecusorMono - _peptideMass);
        //        // _peptideMz = _y1mass - GlycanMass.GetGlycanMasswithCharge(Glycan.Type.HexNAc, _y1charge);
        //    }
        //    #endregion

        //    //-------Hard code part--------
        //    //_PrecusorMono = 5407.23229f;
        //    //_peptideMass = 2399.187f;
        //    //_glycanMonoMass = (float)(_PrecusorMono - _peptideMass);
        //    //_y1mass = (float)(_peptideMass + GlycanMass.GetGlycanMass(Glycan.Type.HexNAc) + COL.MassLib.Atoms.ProtonMass * 3) / 3;
        //    //_peptideMz = _y1mass - GlycanMass.GetGlycanMasswithCharge(Glycan.Type.HexNAc, _y1charge);
        //    //---------------

        //    _SequencedGlycanStructure.Add(new GlycanStructure(new Glycan(Glycan.Type.HexNAc, _y1charge), _y1MZ)); //Add Y1 into Candidate;
        //    UpdateStatus("Y1=" + _y1MZ.ToString("0.0000") + "  Y1z=" + _y1charge.ToString());
        //    //Find Y1
        //    int CloseY1Idx = MassUtility.GetClosestMassIdx(_potentialPeaks, _y1MZ);
        //    if (Math.Abs(_y1MZ - _potentialPeaks[CloseY1Idx].Mass) <= _MS2torelance)
        //    {
        //        _SequencedGlycanStructure[0].Root.IDPeak(_potentialPeaks[CloseY1Idx].Mass, _potentialPeaks[CloseY1Idx].Intensity);
        //    }

        //    if (_isDebug)
        //    {
        //        DebugSW = new StreamWriter(_debugFolderPath + "\\StructureListByMass_" + _scan.ScanNo + ".txt");
        //    }
        //    //charge
        //    for (int chargePlus = 0; chargePlus <= 1; chargePlus++)
        //    {
        //        int CurrentCharge = _y1charge + chargePlus;
        //        float CurrentY1Mz = Convert.ToSingle((_y1MZ * _y1charge + 1.0079 * chargePlus) / CurrentCharge);
        //        GenerateGlycanMass(CurrentCharge, out _glycanBuildingblock);
        //        float PeptideMone = 0.0f;
        //        if (_UseAVGMass)
        //        {
        //            PeptideMone = (CurrentY1Mz - GlycanMass.GetGlycanAVGMasswithCharge(Glycan.Type.HexNAc, CurrentCharge)) * CurrentCharge - MassLib.Atoms.ProtonMass;
        //        }
        //        else
        //        {
        //            PeptideMone = (CurrentY1Mz - GlycanMass.GetGlycanMasswithCharge(Glycan.Type.HexNAc, CurrentCharge)) * CurrentCharge - MassLib.Atoms.ProtonMass;
        //        }
        //        foreach (MSPoint pk in _potentialPeaks)
        //        {
        //            for (int i = 0; i < _SequencedGlycanStructure.Count; i++)
        //            {
        //                GlycanStructure CandidateTree = (GlycanStructure)_SequencedGlycanStructure[i];
        //                foreach (GlycanTreeNode Node in CandidateTree.Root.TravelGlycanTreeBFS()) //Convert All Internal node to current charge
        //                {
        //                    Node.Charge = CurrentCharge;
        //                }

        //                for (int GlycanIdx = 0; GlycanIdx < _glycanBuildingblock.Count; GlycanIdx++)
        //                {
        //                    Glycan ExtraGlycan = _glycanBuildingblock[GlycanIdx];

        //                    if (ExtraGlycan.GlycanType == Glycan.Type.DeHex &&
        //                        _NoDeHex - CandidateTree.NoOfDeHex <= 0)
        //                    {
        //                        continue;
        //                    }

        //                    if (ExtraGlycan.GlycanType == Glycan.Type.HexNAc &&
        //                         _NoHexNac - CandidateTree.NoOfHexNac <= 0)
        //                    {
        //                        continue;
        //                    }
        //                    if (ExtraGlycan.GlycanType == Glycan.Type.NeuAc &&
        //                        _NoNeuAc - CandidateTree.NoOfNeuAc <= 0)
        //                    {
        //                        continue;
        //                    }
        //                    if (ExtraGlycan.GlycanType == Glycan.Type.NeuGc &&
        //                       _NoNeuGc - CandidateTree.NoOfNeuGc <= 0)
        //                    {
        //                        continue;
        //                    }
        //                    if ((ExtraGlycan.GlycanType == Glycan.Type.Hex ||
        //                            ExtraGlycan.GlycanType == Glycan.Type.Gal ||
        //                            ExtraGlycan.GlycanType == Glycan.Type.Man
        //                        )
        //                        &&
        //                        _NoHex - CandidateTree.NoOfHex <= 0)
        //                    {
        //                        continue;
        //                    }



        //                    /*if (Math.Abs((float)(CandidateTree.GlycanAVGMonoMass + MassLib.Atoms.ProtonMass * CurrentCharge) 
        //                            / (float)CurrentCharge + ExtraGlycan.AVGMz + CurrentY1Mz - GlycanMass.GetGlycanAVGMasswithCharge(Glycan.Type.HexNAc, CurrentCharge)
        //                            - pk.Mass) 
        //                            > _MS2torelance)*/
        //                    if (_UseAVGMass)
        //                    {
        //                        if (Math.Abs(((CandidateTree.GlycanAVGMonoMass + PeptideMone + ExtraGlycan.AVGMass + MassLib.Atoms.ProtonMass * CurrentCharge) / CurrentCharge) - pk.Mass) > _MS2torelance)
        //                        {
        //                            continue;
        //                        }
        //                        else //Add One monosaccharide to Tree
        //                        {
        //                            if (ExtraGlycan.GlycanType == Glycan.Type.DeHex && CandidateTree.Root.GlycanType != Glycan.Type.HexNAc) // DeHex only connect to HexNac
        //                            {
        //                                continue;
        //                            }
        //                            GlycanStructure CloneTree = (GlycanStructure)CandidateTree.Clone();


        //                            List<GlycanStructure> AddedTrees = AddGlycanToOGlycanTree(CloneTree, ExtraGlycan, pk.Mass, pk.Intensity);

        //                            if (_isDebug)
        //                            {
        //                                string tmp = pk.Mass.ToString("0.000") + "," + CloneTree.IUPACString + "," + ExtraGlycan.GlycanType.ToString() + ",";
        //                                List<string> Structure = new List<string>();
        //                                foreach (GlycanStructure T in AddedTrees)
        //                                {
        //                                    if (!Structure.Contains(T.IUPACString))
        //                                    {
        //                                        Structure.Add(T.IUPACString);
        //                                    }
        //                                }
        //                                foreach (string str in Structure)
        //                                {
        //                                    tmp = tmp + str + ",";
        //                                }
        //                                DebugSW.WriteLine(tmp.Substring(0, tmp.Length - 1));
        //                            }

        //                            foreach (GlycanStructure iterGT in AddedTrees)
        //                            {
        //                                iterGT.Root.SortSubTree();
        //                                iterGT.Charge = CurrentCharge;
        //                                if (!_SequencedGlycanStructure.Contains(iterGT))
        //                                {
        //                                    _SequencedGlycanStructure.Add(iterGT);
        //                                }
        //                            }
        //                        }
        //                    }
        //                    else
        //                    {
        //                        if (Math.Abs(((CandidateTree.GlycanMonoMass + PeptideMone + ExtraGlycan.Mass + MassLib.Atoms.ProtonMass * CurrentCharge) / CurrentCharge) - pk.Mass) > _MS2torelance)
        //                        {
        //                            continue;
        //                        }
        //                        else //Add One monosaccharide to Tree
        //                        {
        //                            if (ExtraGlycan.GlycanType == Glycan.Type.DeHex && CandidateTree.Root.GlycanType != Glycan.Type.HexNAc) // DeHex only connect to HexNac
        //                            {
        //                                continue;
        //                            }
        //                            GlycanStructure CloneTree = (GlycanStructure)CandidateTree.Clone();

        //                            List<GlycanStructure> AddedTrees = AddGlycanToOGlycanTree(CloneTree, ExtraGlycan, pk.Mass, pk.Intensity);

        //                            if (_isDebug)
        //                            {
        //                                string tmp = CloneTree.IUPACString + "," + ExtraGlycan.GlycanType.ToString() + ",";
        //                                List<string> Structure = new List<string>();
        //                                foreach (GlycanStructure T in AddedTrees)
        //                                {
        //                                    if (!Structure.Contains(T.IUPACString))
        //                                    {
        //                                        Structure.Add(T.IUPACString);
        //                                    }
        //                                }
        //                                foreach (string str in Structure)
        //                                {
        //                                    tmp = tmp + str + ",";
        //                                }
        //                                DebugSW.WriteLine(tmp.Substring(0, tmp.Length - 1));
        //                            }

        //                            foreach (GlycanStructure iterGT in AddedTrees)
        //                            {
        //                                iterGT.Root.SortSubTree();
        //                                iterGT.Charge = CurrentCharge;
        //                                if (!_SequencedGlycanStructure.Contains(iterGT))
        //                                {
        //                                    _SequencedGlycanStructure.Add(iterGT);
        //                                }
        //                            }
        //                        }
        //                    }


        //                }//iterate Glycan
        //            } //iterate CandidateGlycan
        //        }//iterate Peaks


        //        //Extra structure check
        //        //List<GlycanStructure> NewlyAdded = new List<GlycanStructure>();
        //        //float ParentMonoMW = _scan.ParentMonoMW;

        //        //foreach (GlycanStructure t in NewlyAdded)
        //        //{
        //        //    t.Root.SortSubTree();
        //        //    _SequencedGlycanStructure.Add(t);
        //        //}

        //        //NewlyAdded.Clear();      

        //        //foreach (GlycanStructure t in NewlyAdded)
        //        //{
        //        //    t.Root.SortSubTree();
        //        //    _SequencedGlycanStructure.Add(t);
        //        //}                
        //    }
        //    _SequencedGlycanStructure = CleanOLinkedStructure(_SequencedGlycanStructure);

        //    //Complete the structure (add last glycan) using percursor average or first mono mass             

        //    foreach (GlycanStructure t in _SequencedGlycanStructure)
        //    {
        //        for (int i = 0; i < _glycanBuildingblock.Count; i++)
        //        {
        //            Glycan NextGlycan = new Glycan(_glycanBuildingblock[i].GlycanType, 1);
        //            if (_UseAVGMass)
        //            {
        //                if (PrecusorAVGMonoMass != 0.0f)
        //                {
        //                    if (MassUtility.GetMassPPM(t.GlycanAVGMonoMass + PeptideMonoMass + NextGlycan.AVGMass, PrecusorAVGMonoMass) < _precursorTorelance)
        //                    {
        //                        List<GlycanStructure> AddedTrees = AddGlycanToNGlycanTree(t, NextGlycan, _PrecusorMono, 0.0f);
        //                        _FullSequencedGlycanStructure.AddRange(AddedTrees);
        //                        continue;
        //                    }
        //                    if (_createPrecursor)
        //                    {
        //                        //Create Precursor m/z
        //                        for (int j = t.Charge; j <= _scan.ParentCharge; j++)
        //                        {
        //                            float PrecusorMZ = (_PrecusorMono + Atoms.ProtonMass * j) / (float)j;
        //                            float GlycoPeptidMZ = (t.GlycanAVGMonoMass + PeptideMonoMass + NextGlycan.AVGMass + Atoms.ProtonMass * j) / (float)j;

        //                            if (Math.Abs(PrecusorMZ - GlycoPeptidMZ) <= _MS2torelance)
        //                            {
        //                                List<GlycanStructure> AddedTrees = AddGlycanToNGlycanTree(t, NextGlycan, _PrecusorMono, 0.0f);
        //                                _FullSequencedGlycanStructure.AddRange(AddedTrees);
        //                            }
        //                        }
        //                    }
        //                }
        //                else //Precursor AVG Mono not found
        //                {
        //                    if (MassUtility.GetMassPPM(t.GlycanAVGMonoMass + PeptideMonoMass + NextGlycan.AVGMass, PrecusorMonoMass) < _precursorTorelance * 3)
        //                    {
        //                        List<GlycanStructure> AddedTrees = AddGlycanToNGlycanTree(t, NextGlycan, _PrecusorMono, 0.0f);
        //                        _FullSequencedGlycanStructure.AddRange(AddedTrees);
        //                        continue;
        //                    }
        //                    if (_createPrecursor)
        //                    {
        //                        //Create Precursor m/z
        //                        for (int j = t.Charge - 1; j <= _scan.ParentCharge; j++)
        //                        {
        //                            float PrecusorMZ = (_PrecusorMono + Atoms.ProtonMass * j) / (float)j;
        //                            float GlycoPeptidMZ = (t.GlycanAVGMonoMass + PeptideMonoMass + NextGlycan.AVGMass + Atoms.ProtonMass * j) / (float)j;

        //                            if (Math.Abs(PrecusorMZ - GlycoPeptidMZ) <= _MS2torelance)
        //                            {
        //                                List<GlycanStructure> AddedTrees = AddGlycanToNGlycanTree(t, NextGlycan, _PrecusorMono, 0.0f);
        //                                _FullSequencedGlycanStructure.AddRange(AddedTrees);
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //            else //non-AVG Mass
        //            {
        //                if (MassUtility.GetMassPPM(t.GlycanMonoMass + PeptideMonoMass + NextGlycan.Mass, PrecusorMonoMass) < _precursorTorelance)
        //                {
        //                    List<GlycanStructure> AddedTrees = AddGlycanToNGlycanTree(t, NextGlycan, _PrecusorMono, 0.0f);
        //                    _FullSequencedGlycanStructure.AddRange(AddedTrees);
        //                    continue;
        //                }
        //                if (_createPrecursor)
        //                {
        //                    //Create Precursor m/z
        //                    for (int j = t.Charge; j <= _scan.ParentCharge; j++)
        //                    {
        //                        float PrecusorMZ = (_PrecusorMono + Atoms.ProtonMass * j) / (float)j;
        //                        float GlycoPeptidMZ = (t.GlycanMonoMass + PeptideMonoMass + NextGlycan.Mass + Atoms.ProtonMass * j) / (float)j;

        //                        if (Math.Abs(PrecusorMZ - GlycoPeptidMZ) <= _MS2torelance)
        //                        {
        //                            List<GlycanStructure> AddedTrees = AddGlycanToNGlycanTree(t, NextGlycan, _PrecusorMono, 0.0f);
        //                            _FullSequencedGlycanStructure.AddRange(AddedTrees);
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    //Use number of compostion to complete structure
        //    if (_FullSequencedGlycanStructure.Count == 0 && _NumCompostionSet == true)
        //    {
        //        foreach (GlycanStructure GS in _SequencedGlycanStructure)
        //        {
        //            int LeftGlycan = 0;
        //            LeftGlycan = _NoHex - GS.NoOfHex;
        //            LeftGlycan = LeftGlycan + (_NoHexNac - GS.NoOfHexNac);
        //            LeftGlycan = LeftGlycan + (_NoDeHex - GS.NoOfDeHex);
        //            LeftGlycan = LeftGlycan + (_NoNeuAc - GS.NoOfNeuAc);
        //            LeftGlycan = LeftGlycan + (_NoNeuGc - GS.NoOfNeuGc);

        //            List<Glycan> ExtraGlycan = new List<Glycan>();
        //            if (LeftGlycan <= 2)
        //            {
        //                for (int i = 0; i < _NoHex - GS.NoOfHex; i++)
        //                {
        //                    ExtraGlycan.Add(new Glycan(Glycan.Type.Hex, GS.Charge));
        //                }
        //                for (int i = 0; i < (_NoHexNac - GS.NoOfHexNac); i++)
        //                {
        //                    ExtraGlycan.Add(new Glycan(Glycan.Type.HexNAc, GS.Charge));
        //                }
        //                for (int i = 0; i < (_NoDeHex - GS.NoOfDeHex); i++)
        //                {
        //                    ExtraGlycan.Add(new Glycan(Glycan.Type.DeHex, GS.Charge));
        //                }
        //                for (int i = 0; i < (_NoNeuAc - GS.NoOfNeuAc); i++)
        //                {
        //                    ExtraGlycan.Add(new Glycan(Glycan.Type.NeuAc, GS.Charge));
        //                }
        //                for (int i = 0; i < (_NoNeuGc - GS.NoOfNeuGc); i++)
        //                {
        //                    ExtraGlycan.Add(new Glycan(Glycan.Type.NeuGc, GS.Charge));
        //                }
        //            }
        //        }
        //    }

        //    if (_FullSequencedGlycanStructure.Count != 0)
        //    {
        //        _FullSequencedGlycanStructure = CleanOLinkedStructure(_FullSequencedGlycanStructure);
        //        //Reware for complete structure;
        //        if (_rewardForCompleteStructure != 0.0f)
        //        {
        //            foreach (GlycanStructure g in _FullSequencedGlycanStructure)
        //            {
        //                //g.Score = g.Score + _rewardForCompleteStructure;
        //            }
        //        }
        //    }
        //    if (_isDebug)
        //    {
        //        DebugSW.Close();
        //    }
        //}
        //private void FindNLinkedStructure()
        //{
        //    bool isBiscectingSignal = false;
        //    bool isCoreFucsse = false;
        //    AminoAcidMass MW = new AminoAcidMass();

        //    _SequencedGlycanStructure.Add(new GlycanStructure(new Glycan(Glycan.Type.HexNAc, _y1charge), _y1MZ)); //Add Y1 into Candidate;
        //    UpdateStatus("Y1=" + _y1MZ.ToString("0.0000") + "  Y1z=" + _y1charge.ToString());
        //    //Find Y1
        //    int CloseY1Idx = MassUtility.GetClosestMassIdx(_potentialPeaks, _y1MZ);
        //    if (Math.Abs(_y1MZ - _potentialPeaks[CloseY1Idx].Mass) <= _MS2torelance)
        //    {
        //        _SequencedGlycanStructure[0].Root.IDPeak(_potentialPeaks[CloseY1Idx].Mass, _potentialPeaks[CloseY1Idx].Intensity);
        //    }

        //    if (_isDebug)
        //    {
        //        DebugSW = new StreamWriter(_debugFolderPath + "\\StructureListByMass_" + _scan.ScanNo + ".txt");
        //    }
        //    //charge
        //    for (int chargePlus = 0; chargePlus <= 1; chargePlus++)
        //    {
        //        int CurrentCharge = _y1charge + chargePlus;
        //        float CurrentY1Mz = Convert.ToSingle((_y1MZ * _y1charge + 1.0079 * chargePlus) / CurrentCharge);
        //        GenerateGlycanMass(CurrentCharge, out _glycanBuildingblock);
        //        float PeptideMono = 0.0f;
        //        if (_UseAVGMass)
        //        {
        //            PeptideMono = (CurrentY1Mz - MassLib.Atoms.ProtonMass) * CurrentCharge - GlycanMass.GetGlycanAVGMass(Glycan.Type.HexNAc);
        //        }
        //        else
        //        {
        //            PeptideMono = (CurrentY1Mz - MassLib.Atoms.ProtonMass) * CurrentCharge - GlycanMass.GetGlycanMass(Glycan.Type.HexNAc);
        //        }
        //        foreach (MSPoint pk in _potentialPeaks)
        //        {
        //            for (int i = 0; i < _SequencedGlycanStructure.Count; i++)
        //            {
        //                GlycanStructure CandidateTree = (GlycanStructure)_SequencedGlycanStructure[i];
        //                //foreach (GlycanTreeNode Node in CandidateTree.Root.TravelGlycanTreeBFS()) //Convert All Internal node to current charge
        //                //{
        //                //    Node.Charge = CurrentCharge;
        //                //}

        //                for (int GlycanIdx = 0; GlycanIdx < _glycanBuildingblock.Count; GlycanIdx++)
        //                {
        //                    Glycan ExtraGlycan = _glycanBuildingblock[GlycanIdx];

        //                    if (ExtraGlycan.GlycanType == Glycan.Type.DeHex &&
        //                        _NoDeHex - CandidateTree.NoOfDeHex <= 0)
        //                    {
        //                        continue;
        //                    }

        //                    if (ExtraGlycan.GlycanType == Glycan.Type.HexNAc &&
        //                         _NoHexNac - CandidateTree.NoOfHexNac <= 0)
        //                    {
        //                        continue;
        //                    }
        //                    if (ExtraGlycan.GlycanType == Glycan.Type.NeuAc &&
        //                        _NoNeuAc - CandidateTree.NoOfNeuAc <= 0)
        //                    {
        //                        continue;
        //                    }
        //                    if (ExtraGlycan.GlycanType == Glycan.Type.NeuGc &&
        //                       _NoNeuGc - CandidateTree.NoOfNeuGc <= 0)
        //                    {
        //                        continue;
        //                    }
        //                    if ((ExtraGlycan.GlycanType == Glycan.Type.Hex ||
        //                            ExtraGlycan.GlycanType == Glycan.Type.Gal ||
        //                            ExtraGlycan.GlycanType == Glycan.Type.Man
        //                        )
        //                        &&
        //                        _NoHex - CandidateTree.NoOfHex <= 0)
        //                    {
        //                        continue;
        //                    }

        //                    /*if (Math.Abs((float)(CandidateTree.GlycanAVGMonoMass + MassLib.Atoms.ProtonMass * CurrentCharge) 
        //                            / (float)CurrentCharge + ExtraGlycan.AVGMz + CurrentY1Mz - GlycanMass.GetGlycanAVGMasswithCharge(Glycan.Type.HexNAc, CurrentCharge)
        //                            - pk.Mass) 
        //                            > _MS2torelance)*/
        //                    if (_UseAVGMass)
        //                    {
        //                        if (Math.Abs(((CandidateTree.GlycanAVGMonoMass + PeptideMono + ExtraGlycan.AVGMass + MassLib.Atoms.ProtonMass * CurrentCharge) / CurrentCharge) - pk.Mass) > _MS2torelance)
        //                        //if (Math.Abs((_y1mass + CandidateTree.GlycanAVGMZ + ExtraGlycan.AVGMz - GlycanMass.GetGlycanAVGMasswithCharge(Glycan.Type.HexNAc, CurrentCharge)) - pk.Mass) > _MS2torelance)
        //                        {
        //                            continue;
        //                        }
        //                        else //Add One monosaccharide to Tree
        //                        {
        //                            if (ExtraGlycan.GlycanType == Glycan.Type.DeHex && CandidateTree.Root.GlycanType != Glycan.Type.HexNAc) // DeHex only connect to HexNac
        //                            {
        //                                continue;
        //                            }
        //                            if ((CandidateTree.IUPACString.ToLower() == "hex-hexnac-hexnac" &&
        //                                ExtraGlycan.GlycanType == Glycan.Type.HexNAc) ||
        //                                (CandidateTree.IUPACString.ToLower() == "hex-hexnac-(dehex-)hexnac" &&
        //                                ExtraGlycan.GlycanType == Glycan.Type.HexNAc)
        //                                )
        //                            {
        //                                isBiscectingSignal = true;
        //                            }
        //                            if ((CandidateTree.IUPACString.ToLower() == "hexnac" &&
        //                                ExtraGlycan.GlycanType == Glycan.Type.DeHex) ||
        //                                (CandidateTree.IUPACString.ToLower() == "hexnac-hexnac" &&
        //                                ExtraGlycan.GlycanType == Glycan.Type.DeHex) ||
        //                                (CandidateTree.IUPACString.ToLower() == "hex-hexnac-hexnac" &&
        //                                ExtraGlycan.GlycanType == Glycan.Type.DeHex) ||
        //                                (CandidateTree.IUPACString.ToLower() == "hex-hex-hexnac-hexnac" &&
        //                                ExtraGlycan.GlycanType == Glycan.Type.DeHex) ||
        //                                (CandidateTree.IUPACString.ToLower() == "hex-(hex-)hex-hexnac-hexnac" &&
        //                                ExtraGlycan.GlycanType == Glycan.Type.DeHex))
        //                            {
        //                                isCoreFucsse = true;
        //                            }
        //                            GlycanStructure CloneTree = (GlycanStructure)CandidateTree.Clone();

        //                            List<GlycanStructure> AddedTrees = AddGlycanToNGlycanTree(CloneTree, ExtraGlycan, pk.Mass, pk.Intensity, isBiscectingSignal, isCoreFucsse);

        //                            if (_isDebug)
        //                            {
        //                                string tmp = pk.Mass.ToString("0.000") + "," + CloneTree.IUPACString + "," + ExtraGlycan.GlycanType.ToString() + ",";
        //                                List<string> Structure = new List<string>();
        //                                foreach (GlycanStructure T in AddedTrees)
        //                                {
        //                                    if (!Structure.Contains(T.IUPACString))
        //                                    {
        //                                        Structure.Add(T.IUPACString);
        //                                    }
        //                                }
        //                                foreach (string str in Structure)
        //                                {
        //                                    tmp = tmp + str + ",";
        //                                }
        //                                DebugSW.WriteLine(tmp.Substring(0, tmp.Length - 1));
        //                            }

        //                            foreach (GlycanStructure iterGT in AddedTrees)
        //                            {
        //                                iterGT.Root.SortSubTree();
        //                                iterGT.Charge = CurrentCharge;
        //                                if (!_SequencedGlycanStructure.Contains(iterGT))
        //                                {
        //                                    _SequencedGlycanStructure.Add(iterGT);
        //                                }
        //                            }
        //                        }
        //                    }
        //                    //else

        //                }//iterate Glycan
        //            } //iterate CandidateGlycan
        //        }//iterate Peaks


        //    }
        //    _SequencedGlycanStructure = CleanNLinkedStructure(_SequencedGlycanStructure);

        //    //Complete the structure (add last glycan) using percursor average or first mono mass            

        //    foreach (GlycanStructure t in _SequencedGlycanStructure)
        //    {
        //        for (int i = 0; i < _glycanBuildingblock.Count; i++)
        //        {
        //            Glycan NextGlycan = new Glycan(_glycanBuildingblock[i].GlycanType, 1);


        //            if (MassUtility.GetMassPPM((GetGlycanMass(t) + NextGlycan.Mass + _peptideMass), _scan.ParentMonoMW) <= _precursorTorelance)
        //            {
        //                List<GlycanStructure> AddedTrees = AddGlycanToNGlycanTree(t, NextGlycan, _scan.ParentMonoMW, 0.0f);
        //                foreach (GlycanStructure GS in AddedTrees)
        //                {
        //                    GS.IsCompleteSequenced = true;
        //                    GS.Root.IDIntensity = GS.Root.IDIntensity + _rewardForCompleteStructure;
        //                    //if (MassLib.MassUtility.GetMassPPM(_scan.ParentMonoMW, _peptideMass+GS.GlycanAVGMonoMass  ) < _precursorTorelance)
        //                    //{
        //                    _FullSequencedGlycanStructure.Add(GS);
        //                    //}
        //                }
        //            }
        //        }
        //    }

        //    if (_isDebug)
        //    {
        //        DebugSW.Close();
        //    }
        //}
        
        //private List<GlycanStructure> CleanNLinkedStructure(List<GlycanStructure> argStructures)
        //{
        //    //Remove duplicate
        //    List<String> _UniqueStructureIUPAC = new List<string>();
        //    List<GlycanStructure> UniqueTrees = new List<GlycanStructure>();
        //    foreach (GlycanStructure t in argStructures)
        //    {
        //        if (!_UniqueStructureIUPAC.Contains(t.IUPACString) && t.GlycanAVGMonoMass + _peptideMass <= PrecusorMonoMass + 10.0f)
        //        {
        //            _UniqueStructureIUPAC.Add(t.IUPACString);
        //            // t.UpdateDistance(-1);
        //            UniqueTrees.Add(t);
        //        }
        //    }
        //    argStructures = UniqueTrees;

        //    List<GlycanStructure> PassStructureCheckStructure = new List<GlycanStructure>();
        //    foreach (GlycanStructure t in argStructures)
        //    {
        //        if (GlycanSequencing_MultipleScoring.IsStructureObeyAllRules(t, _structureRules) && HasNCore(t))
        //        {
        //            PassStructureCheckStructure.Add(t);
        //        }
        //    }
        //    argStructures = PassStructureCheckStructure;

        //    return argStructures;
        //}
        //private bool HasNCore(GlycanStructure argStructure)
        //{
        //    bool HasCore = false;
        //    if (argStructure.Root.GlycanType == Glycan.Type.HexNAc &&
        //        argStructure.Root.NoOfHexNacInChild == 1)  //Root
        //    {
        //        GlycanTreeNode TreeNode; //Glycan2
        //        if (argStructure.Root.SubTree1.GlycanType == Glycan.Type.HexNAc)
        //        {
        //            TreeNode = argStructure.Root.SubTree1;
        //        }
        //        else
        //        {
        //            TreeNode = argStructure.Root.SubTree2;
        //        }
        //        if (TreeNode.NoOfHexInChild == 1 &&
        //            TreeNode.SubTree1.NoOfHexInChild == 2)
        //        {
        //            HasCore = true;
        //        }
        //    }
        //    return HasCore;
        //}
        //private List<GlycanStructure> CleanOLinkedStructure(List<GlycanStructure> argStructures)
        //{
        //    //Remove duplicate
        //    List<String> _UniqueStructureIUPAC = new List<string>();
        //    List<GlycanStructure> UniqueTrees = new List<GlycanStructure>();
        //    foreach (GlycanStructure t in argStructures)
        //    {
        //        if (!_UniqueStructureIUPAC.Contains(t.IUPACString) && t.GlycanAVGMonoMass + _peptideMass <= PrecusorMonoMass + 10.0f)
        //        {
        //            _UniqueStructureIUPAC.Add(t.IUPACString);
        //            // t.UpdateDistance(-1);
        //            UniqueTrees.Add(t);
        //        }
        //    }
        //    argStructures = UniqueTrees;

        //    return argStructures;
        //}
        ///* private void FindStructure()
        // {
        //     //#region Find Structure 
        //     double TreeLeftMass = 0.0;

        //     DebugSW = null; //For debug
        //     if (_isDebug)
        //     {
        //         DebugSW = new StreamWriter(_debugFolderPath + "\\StructureListByMass_" + _scan.ScanNo + ".txt");
        //     }

        //     _SequencedGlycanStructure.Add(new GlycanStructure(new Glycan(Glycan.Type.HexNAc, _y1charge), _y1mass));
        //     foreach (MSPoint pk in _potentialPeaks)
        //     {
        //         if (_isDebug)
        //         {
        //             DebugSW.WriteLine("-------" + pk.Mass.ToString() + "---------");
        //         }
        //         for (int GlycanIdx = 0; GlycanIdx < _glycanBuildingblock.Count; GlycanIdx++)
        //         {

        //             Glycan glycan = _glycanBuildingblock[GlycanIdx];
        //             if (MassUtility.GetMassPPM(_y1mass + glycan.Mz, pk.Mass) < _MS2torelance) // One monosaccharide match
        //             {
        //                 GlycanStructure newGT = (GlycanStructure)_SequencedGlycanStructure[0].Clone();
        //                 //newGT.SubTree1 = new GlycanTree(glycan);
        //                 newGT.AddGlycanToStructure(new GlycanTreeNode(glycan.GlycanType, newGT.NextID), newGT.Root.NodeID);
        //                 newGT.GetGlycanTreeByID(newGT.NextID-1).IDPeak(pk.Mass, pk.Intensity);

        //                 if (newGT.Score > 0)
        //                 {
        //                     if (!_SequencedGlycanStructure.Contains(newGT))//(delegate(GlycanTree t1) { return (t1.GetIUPACString() == newGT.GetIUPACString() && t1.Charge); }))
        //                     {
        //                         if (_isDebug == true)
        //                         {
        //                             DebugSW.WriteLine((_SequencedGlycanStructure.Count).ToString() + "-" + newGT.Charge.ToString() + "-" + newGT.IUPACString);
        //                         }

        //                         if (_NGlycanData && newGT.isObyeNLinkedCore())
        //                         {
        //                             _SequencedGlycanStructure.Add(newGT);
        //                         }
        //                         else if (_NGlycanData == false)
        //                         {
        //                             _SequencedGlycanStructure.Add(newGT);
        //                         }
        //                         // _SequencedGlycanStructure.Sort(delegate(GlycanTree t1, GlycanTree t2) { return Comparer<float>.Default.Compare(t2.TreeMass, t1.TreeMass); });
        //                     }
        //                     else
        //                     {
        //                         GlycanStructure ExistTree = _SequencedGlycanStructure.Find(delegate(GlycanStructure t1) { return t1.IUPACString == newGT.IUPACString; });
        //                         if (ExistTree.Score < newGT.Score)
        //                         {
        //                             if (_isDebug == true)
        //                             {
        //                                 DebugSW.WriteLine(newGT.IUPACString);
        //                             }
        //                             _SequencedGlycanStructure.Remove(ExistTree);

        //                             if (_NGlycanData && newGT.isObyeNLinkedCore())
        //                             {
        //                                 _SequencedGlycanStructure.Add(newGT);
        //                             }
        //                             else if (_NGlycanData == false)
        //                             {
        //                                 _SequencedGlycanStructure.Add(newGT);
        //                             }
        //                         }
        //                     }

        //                 }
        //                 #region Add Missing Peak Structure

        //                 for (int NextGlycanIdx = 0; NextGlycanIdx < _glycanBuildingblock.Count; NextGlycanIdx++)
        //                 {
        //                     Glycan NextGlycan = _glycanBuildingblock[NextGlycanIdx];
        //                     float nextpeak = _y1mass + glycan.Mz + NextGlycan.Mz;
        //                     if (MassUtility.GetMassPPM(nextpeak, FindClosedPeakIdx(nextpeak)) <= _MS2torelance) //Peak found and will add in next round
        //                     {
        //                         continue;
        //                     }
        //                     else
        //                     {
        //                         if (NextGlycan.GlycanType == Glycan.Type.DeHex && newGT.Root.GlycanType != Glycan.Type.HexNAc)  //Fuc only connect to HexNAc
        //                         {
        //                             continue;
        //                         }
        //                         float targetMz = newGT.GlycanMZ + NextGlycan.Mz + _peptideMz;
        //                         float targetIntensity = 0.0f;
        //                         if (MassUtility.GetMassPPM(_scan.MSPeaks[MassUtility.GetClosestMassIdx(_scan.MSPeaks, targetMz)].MonoMass, targetMz) <= _torelance)
        //                         {
        //                             targetIntensity = _scan.MSPeaks[MassUtility.GetClosestMassIdx(_scan.MSPeaks, targetMz)].MonoIntensity / MaxIntensityInScan;
        //                         }



        //                         //List<GlycanTree> AddedTrees = (AddGlycanToGlycanTree(newGT, NextGlycan, targetMz, targetIntensity)); //Add all posibble structures into candidate list (allow missing peak
        //                         List<GlycanStructure> AddedTrees = (AddGlycanToGlycanTree(newGT, NextGlycan, targetMz, targetIntensity)); //Add all posibble structures into candidate list (allow missing peak
        //                         foreach (GlycanStructure iterGT in AddedTrees)
        //                         {
        //                             if (!_SequencedGlycanStructure.Contains(iterGT))
        //                             {
        //                                 if (_NGlycanData && iterGT.isObyeNLinkedCore())
        //                                 {
        //                                     _SequencedGlycanStructure.Add(iterGT);
        //                                 }
        //                                 else if (_NGlycanData == false)
        //                                 {
        //                                     _SequencedGlycanStructure.Add(iterGT);
        //                                 }
        //                                 if (_isDebug == true)
        //                                 {
        //                                     DebugSW.WriteLine((_SequencedGlycanStructure.Count).ToString() + "-Missed-" + iterGT.IUPACString + ";" + iterGT.NoOfHex + "," + iterGT.NoOfHexNac + "," + iterGT.NoOfDeHex + "," + iterGT.NoOfNeuAc);
        //                                 }
        //                             }
        //                         }
        //                     }
        //                 }
        //                 #endregion
        //                 continue;
        //             }

        //             TreeLeftMass = pk.Mass - _y1mass - glycan.Mz;

        //             if (TreeLeftMass < 0)
        //             {
        //                 continue;
        //             }

        //             for (int i = 0; i < _SequencedGlycanStructure.Count; i++)
        //             {
        //                 GlycanStructure GT = _SequencedGlycanStructure[i];
        //                 if (MassUtility.GetMassPPM(GT.GlycanMZ + glycan.Mz + _y1mass - GlycanMass.GetGlycanMasswithCharge(Glycan.Type.HexNAc, _y1charge), pk.Mass) > _torelance || GT.Charge != glycan.Charge)
        //                 {
        //                     continue;
        //                 }
        //                 else //Add One monosaccharide to Tree
        //                 {
        //                     if (glycan.GlycanType == Glycan.Type.DeHex && GT.Root.GlycanType != Glycan.Type.HexNAc) // DeHex only connect to HexNac
        //                     {
        //                         continue;
        //                     }
        //                     List<GlycanStructure> AddedTrees = (AddGlycanToGlycanTree(GT, glycan, pk.Mass, pk.Intensity));
        //                     foreach (GlycanStructure iterGT in AddedTrees)
        //                     {
        //                         if (!_SequencedGlycanStructure.Contains(iterGT))
        //                         {

        //                             if (_NGlycanData && iterGT.isObyeNLinkedCore())
        //                             {
        //                                 _SequencedGlycanStructure.Add(iterGT);
        //                             }
        //                             else if (_NGlycanData == false)
        //                             {
        //                                 _SequencedGlycanStructure.Add(iterGT);
        //                             }
        //                             if (_isDebug == true)
        //                             {
        //                                 DebugSW.WriteLine((_SequencedGlycanStructure.Count).ToString() + "-*" + iterGT.Charge.ToString() + "-" + iterGT.IUPACString + ";" + iterGT.NoOfHex + "," + iterGT.NoOfHexNac + "," + iterGT.NoOfDeHex + "," + iterGT.NoOfNeuAc);
        //                             }

        //                         }
        //                     }
        //                 }
        //             }
        //         }
        //     }

        //     //Extra structure check
        //     List<GlycanStructure> NewlyAdded = new List<GlycanStructure>();
        //     float ParentMonoMW = _scan.ParentMonoMW;
        //     foreach (GlycanStructure gt in _SequencedGlycanStructure)
        //     {
        //         if (gt.Root.SubTree1 == null || gt.Root.SubTree1.SubTree1 == null)
        //         {
        //             continue;
        //         }
        //         GlycanTreeNode BrancePoint = gt.Root.SubTree1.SubTree1;

        //         //Check symatric structure
        //         if (BrancePoint.SubTree1 != null && BrancePoint.SubTree2 == null)
        //         {
        //             GlycanStructure NewTree = (GlycanStructure)gt.Clone();
        //             //NewTree.Root.SubTree1.SubTree1.SubTree2 = (GlycanTree)BrancePoint.SubTree1.Clone();
        //             NewTree.AddGlycanToStructure((GlycanTreeNode)BrancePoint.SubTree1.Clone(), NewTree.Root.SubTree1.SubTree1.NodeID);

        //             if (NewTree.GlycanMonoMass + _peptideMass < ParentMonoMW)
        //             {
        //                 if (_NGlycanData && !NewTree.isObyeNLinkedCore())
        //                 {
        //                     continue;
        //                 }
        //                 NewlyAdded.Add(NewTree);
        //             }
        //         }
        //         else if (BrancePoint.SubTree1 != null && BrancePoint.SubTree2 != null && BrancePoint.SubTree1.NoOfTotalGlycan > 1 && BrancePoint.SubTree2.NoOfTotalGlycan == 1)
        //         {
        //             GlycanStructure NewTree = (GlycanStructure)gt.Clone();
        //            // NewTree.Root.SubTree1.SubTree1.SubTree2 = (GlycanTree)gt.Root.SubTree1.SubTree1.SubTree1.Clone();
        //             NewTree.AddGlycanToStructure((GlycanTreeNode)gt.Root.SubTree1.SubTree1.SubTree1.Clone(), NewTree.Root.SubTree1.SubTree1.NodeID);
        //             if (NewTree.GlycanMonoMass + _peptideMass < ParentMonoMW)
        //             {
        //                 if (_NGlycanData && !NewTree.isObyeNLinkedCore())
        //                 {
        //                     continue;
        //                 }
        //                 NewlyAdded.Add(NewTree);
        //             }
        //         }
        //     }
        //     foreach (GlycanStructure t in NewlyAdded)
        //     {
        //         _SequencedGlycanStructure.Add(t);
        //     }

        //     NewlyAdded.Clear();

        //     //Use parent mass to complete the structure
        //     foreach (GlycanStructure gt in _SequencedGlycanStructure)
        //     {
        //         //Miss last one glycan
        //         for (int i = 0; i < _glycanBuildingblock.Count; i++)
        //         {
        //             Glycan NextGlycan = new Glycan(_glycanBuildingblock[i].GlycanType, _y1charge);
        //             if (MassUtility.GetMassPPM(gt.GlycanMonoMass + GlycanMass.GetGlycanMasswithCharge(NextGlycan.GlycanType, 1), _glycanMonoMass) <= _torelance)
        //             {
        //                 //Find Peaks
        //                 float TargetMz = ((gt.GlycanMonoMass + _peptideMass + GlycanMass.GetGlycanMasswithCharge(NextGlycan.GlycanType, 1)) + _y1charge * Atoms.ProtonMass) / (float)_y1charge;
        //                 int PeakIdx = MassUtility.GetClosestMassIdx( _scan.MSPeaks,TargetMz);
        //                 float ParentMz = (ParentMonoMW + _y1charge * Atoms.ProtonMass) / (float)_y1charge;
        //                 float TargetIntensity = 0.0f;
        //                 if (MassUtility.GetMassPPM(_scan.MSPeaks[PeakIdx].MonoMass, ParentMz) <= _torelance)
        //                 {
        //                     TargetIntensity = _scan.MSPeaks[PeakIdx].MonoIntensity;
        //                 }

        //                 List<GlycanStructure> AddedTrees = (AddGlycanToGlycanTree(gt, NextGlycan, TargetMz, TargetIntensity)); //Add all posibble structures into candidate list (allow missing peak)
        //                 foreach (GlycanStructure iterGT in AddedTrees)
        //                 {
        //                     if (!_SequencedGlycanStructure.Contains(iterGT))
        //                     {
        //                         if (_NGlycanData && !iterGT.isObyeNLinkedCore() && !iterGT.HasNGlycanCore())
        //                         {
        //                             continue;
        //                         }
        //                         NewlyAdded.Add(iterGT);
        //                     }
        //                 }
        //             }
        //         }

        //         //Check symatric structure  for 3rd node (HexNac-HexNac-Hex-Hex)
        //         if (gt.Root.SubTree1 != null && gt.Root.SubTree1.SubTree1 != null && gt.Root.SubTree1.SubTree1.SubTree1 != null && gt.Root.SubTree1.SubTree1.SubTree2 == null)
        //         {
        //             GlycanStructure NewTree = (GlycanStructure)gt.Clone();
        //             //NewTree.SubTree1.SubTree1.SubTree2 = (GlycanTree)gt.Root.SubTree1.SubTree1.SubTree1.Clone();
        //             NewTree.AddGlycanToStructure((GlycanTreeNode)gt.Root.SubTree1.SubTree1.SubTree1.Clone(), NewTree.Root.SubTree1.SubTree1.NodeID);
        //             if (Math.Abs(MassUtility.GetMassPPM(NewTree.GlycanMonoMass, _glycanMonoMass)) <= _torelance)
        //             {
        //                 NewlyAdded.Add(NewTree);
        //             }
        //         }
        //         //Check symatric structure  for 4th node  (HexNac-HexNac-Hex-Hex(-Hex)-Hex)
        //         if (gt.Root.SubTree1 != null && gt.Root.SubTree1.SubTree1 != null && gt.Root.SubTree1.SubTree1.SubTree1 != null && gt.Root.SubTree1.SubTree1.SubTree2 != null
        //             && gt.Root.SubTree1.SubTree1.SubTree1.SubTree1 != null && gt.Root.SubTree1.SubTree1.SubTree2.SubTree1 == null)
        //         {
        //             GlycanStructure NewTree = (GlycanStructure)gt.Clone();
        //             //NewTree.SubTree1.SubTree1.SubTree2.SubTree1 = (GlycanTree)gt.SubTree1.SubTree1.SubTree1.SubTree1.Clone();
        //             NewTree.AddGlycanToStructure((GlycanTreeNode)gt.Root.SubTree1.SubTree1.SubTree1.SubTree1.Clone(), NewTree.Root.SubTree1.SubTree1.SubTree2.NodeID);
        //             if (Math.Abs(MassUtility.GetMassPPM(NewTree.GlycanMonoMass, _glycanMonoMass)) <= _torelance)
        //             {
        //                 NewlyAdded.Add(NewTree);
        //             }
        //         }
        //     }

        //     foreach (GlycanStructure t in NewlyAdded)
        //     {
        //         _SequencedGlycanStructure.Add(t);
        //     }



        //     //Remove duplicted structure

        //     List<String> _UniqueStructureIUPAC = new List<string>();
        //     List<GlycanStructure> UniqueTrees = new List<GlycanStructure>();
        //     foreach (GlycanStructure t in _SequencedGlycanStructure)
        //     {
        //         if (!_UniqueStructureIUPAC.Contains(t.IUPACString))
        //         {
        //             _UniqueStructureIUPAC.Add(t.IUPACString);
        //             // t.UpdateDistance(-1);
        //             UniqueTrees.Add(t);
        //         }
        //     }
        //     _SequencedGlycanStructure = UniqueTrees;
        //     List<GlycanStructure> PassStructureCheckStructure = new List<GlycanStructure>();

        //     foreach (GlycanStructure t in _SequencedGlycanStructure)
        //     {
        //         if (GlycanSequencing.IsStructureObeyAllRules(t, _structureRules))
        //         {
        //             PassStructureCheckStructure.Add(t);
        //         }
        //     }

        //     _SequencedGlycanStructure = PassStructureCheckStructure;

        //     if (_isDebug)
        //     {
        //         DebugSW.Close();
        //     }
        // }
        // */
        ////public static float GetMassPPM(float argExactMass, float argMeasureMass)
        ////{
        ////    return Math.Abs(Convert.ToSingle(((argMeasureMass - argExactMass) / argExactMass) * Math.Pow(10.0, 6.0)));
        ////}
        //private static bool IsStructureObeyAllRules(GlycanStructure argStructure, List<StructureRule> argRules)
        //{
        //    bool IsObey = true;
        //    foreach (StructureRule SR in argRules)
        //    {
        //        List<string> Locations = argStructure.Root.Contain(SR.Structure);
        //        if (SR.TypeOfRule == StructureRule.FiltereTypes.Required) //at least once
        //        {
        //            bool found = false;
        //            if (Locations.Count < 1)
        //            {
        //                return false;
        //            }
        //            foreach (string loc in Locations)
        //            {
        //                if (StructureLocationFound(SR.DistanceOperator, SR.DistanceToRoot, loc))
        //                {
        //                    found = true;
        //                }
        //            }
        //            if (!found)
        //            {
        //                IsObey = false;
        //            }
        //        }
        //        else if (SR.TypeOfRule == StructureRule.FiltereTypes.Denied)
        //        {
        //            if (Locations.Count > 1)
        //            {
        //                if (SR.DistanceOperator == ">")
        //                {
        //                    foreach (string s in Locations)
        //                    {
        //                        if (Convert.ToInt32(s.Split('-')[0]) > SR.DistanceToRoot)
        //                        {
        //                            IsObey = false;
        //                        }
        //                    }
        //                }
        //                else if (SR.DistanceOperator == "<")
        //                {
        //                    foreach (string s in Locations)
        //                    {
        //                        if (Convert.ToInt32(s.Split('-')[0]) < SR.DistanceToRoot)
        //                        {
        //                            IsObey = false;
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    return IsObey;
        //}
        //private static bool StructureLocationFound(string argOperator, int argDistanceToRoot, string NodeID)
        //{
        //    int StructureRoot = Convert.ToInt32(NodeID.Split('-')[0]);
        //    bool found = false;
        //    if (argOperator == "=")
        //    {
        //        if (argDistanceToRoot == StructureRoot)
        //        {
        //            found = true;
        //        }
        //    }
        //    else if (argOperator == "<")
        //    {
        //        if (StructureRoot < argDistanceToRoot)
        //        {
        //            found = true;
        //        }
        //    }
        //    else if (argOperator == ">")
        //    {
        //        if (StructureRoot > argDistanceToRoot)
        //        {
        //            found = true;
        //        }
        //    }
        //    return found;
        //}
        ////private void ReScoreGlycanTree(GlycanStructure argTree)
        ////{
        ////    List<GlycanTreeNode> fragments = argTree.TheoreticalFragment;
        ////    foreach (GlycanTreeNode fragment in fragments)
        ////    {
        ////        //Find Peak
        ////        int PeakIdx = MassUtility.GetClosestMassIdx( _scan.MSPeaks,GlycanMass.GetGlycanMasswithCharge( fragment.GlycanType,argTree.Charge));
        ////        if (MassUtility.GetMassPPM(_scan.MSPeaks[PeakIdx].MonoMass, GlycanMass.GetGlycanMasswithCharge(fragment.GlycanType, argTree.Charge)) <= _torelance) //Peak Found
        ////        {
        ////            fragment.IDPeak(_scan.MSPeaks[PeakIdx].MonoMass, _scan.MSPeaks[PeakIdx].MonoIntensity);
        ////        }
        ////        else
        ////        {
        ////            fragment.IDPeak(_scan.MSPeaks[PeakIdx].MonoMass, 0.0f);
        ////        }
        ////    }
        ////}

        //private List<GlycanStructure> AddGlycanToOGlycanTree(GlycanStructure argParentTree, Glycan argGlycan, float argMZ, float argIntensity)
        //{
        //    List<GlycanStructure> addedGT = new List<GlycanStructure>();
        //    List<GlycanTreeNode> AllGlycanNodeInParent = argParentTree.Root.FetchAllGlycanNode();
        //    List<int> AddPosition = new List<int>(); //AddPosition using GlycanNodeID start with 1;

        //    #region DeHex
        //    if (argGlycan.GlycanType == Glycan.Type.DeHex) // DeHex can place at the 1st core HexNac, and Non-reducing end HexNac  [RULE 8]
        //    {
        //        foreach (GlycanTreeNode T in AllGlycanNodeInParent)
        //        {
        //            if (T.GlycanType == Glycan.Type.HexNAc && (T.Subtrees == null || T.Subtrees.Count < 4) && T.NoOfDeHex == 0)
        //            {
        //                AddPosition.Add(T.NodeID);
        //            }
        //        }
        //    }
        //    #endregion
        //    #region HexNAc
        //    else if (argGlycan.GlycanType == Glycan.Type.HexNAc)
        //    {
        //        foreach (GlycanTreeNode T in AllGlycanNodeInParent)
        //        {
        //            if (T.Subtrees == null || T.Subtrees.Count < 4)
        //            {
        //                AddPosition.Add(T.NodeID);
        //            }
        //        }
        //    }
        //    #endregion HexNAc
        //    #region Hex
        //    else if (argGlycan.GlycanType == Glycan.Type.Hex)
        //    {
        //        foreach (GlycanTreeNode T in AllGlycanNodeInParent)
        //        {
        //            if (T.Subtrees == null || T.Subtrees.Count < 4)
        //            {
        //                AddPosition.Add(T.NodeID);
        //            }
        //        }
        //    }
        //    #endregion Hex
        //    #region NeuAc
        //    else //NeuAc,NeuGc
        //    {
        //        foreach (GlycanTreeNode T in AllGlycanNodeInParent) //Sialic acid only attach to terminal Hex(Gal) [RULE 6]
        //        {
        //            if (T.Subtrees == null || T.Subtrees.Count < 4)
        //            {
        //                AddPosition.Add(T.NodeID);
        //            }
        //        }
        //    }
        //    #endregion NeuAc
        //    for (int i = 0; i < AddPosition.Count; i++)
        //    {
        //        int AddedIndex = AddPosition[i];
        //        GlycanStructure newTree = (GlycanStructure)argParentTree.Clone();
        //        GlycanTreeNode ToBeAddGlycanTree = new GlycanTreeNode(argGlycan.GlycanType, newTree.NextID);
        //        ToBeAddGlycanTree.IDPeak(argMZ, argIntensity);
        //        ToBeAddGlycanTree.Parent = newTree.GetGlycanTreeByID(AddedIndex);

        //        if (ToBeAddGlycanTree.Parent.GlycanType != Glycan.Type.DeHex &&
        //            ToBeAddGlycanTree.Parent.GlycanType != Glycan.Type.NeuAc &&
        //            ToBeAddGlycanTree.Parent.GlycanType != Glycan.Type.NeuGc &&
        //            (ToBeAddGlycanTree.Parent.Subtrees == null || ToBeAddGlycanTree.Parent.Subtrees.Count < 4))  //DeHex NeuAc NeuGc can't have child
        //        {
        //            newTree.AddGlycanToStructure(ToBeAddGlycanTree, ToBeAddGlycanTree.Parent.NodeID);
        //            newTree.Root.SortSubTree();
        //            addedGT.Add(newTree);
        //        }
        //    }
        //    return addedGT;
        //}
       
        ///// <summary>
        ///// Add glycan to possible place
        ///// </summary>
        ///// <param name="argParentTree">Tree need to be added</param>
        ///// <param name="argGlycan">Glycan for add</param>
        ///// <param name="argMZ">m/z for structure </param>
        ///// <param name="argIntensity">intensity for structure</param>
        ///// <returns></returns>
        //private List<GlycanStructure> AddGlycanToGlycanTree(GlycanStructure argParentTree, Glycan argGlycan, float argMZ, float argIntensity)
        //{
        //    List<GlycanStructure> addedGT = new List<GlycanStructure>();
        //    List<GlycanTreeNode> AllGlycanNodeInParent = argParentTree.Root.FetchAllGlycanNode();
        //    List<int> AddPosition = new List<int>();

        //    for (int i = 0; i < AllGlycanNodeInParent.Count; i++)
        //    {
        //        GlycanTreeNode GT = AllGlycanNodeInParent[i];
        //        if (GT.SubTree1 == null) //leaf
        //        {
        //            AddPosition.Add(i);
        //            //find parent
        //            if (i >= 1)
        //            {
        //                //Can Branch
        //                if (AllGlycanNodeInParent[i - 1].SubTree2 == null)
        //                {
        //                    AddPosition.Add(i - 1);
        //                }
        //            }
        //        }
        //    }

        //    for (int i = 0; i < AddPosition.Count; i++)
        //    {
        //        int AddedIndex = AddPosition[i];
        //        GlycanStructure newTree = (GlycanStructure)argParentTree.Clone();
        //        List<GlycanTreeNode> tmpPreAddGTs = newTree.Root.FetchAllGlycanNode();
        //        GlycanTreeNode ToBeAddGlycanTree = new GlycanTreeNode(argGlycan.GlycanType, newTree.NextID);
        //        ToBeAddGlycanTree.IDPeak(argMZ, argIntensity);
        //        ToBeAddGlycanTree.Parent = tmpPreAddGTs[AddedIndex];
        //        if (tmpPreAddGTs[AddedIndex].GlycanType != Glycan.Type.HexNAc && ToBeAddGlycanTree.GlycanType == Glycan.Type.DeHex)
        //        {
        //            continue;
        //        }

        //        if (tmpPreAddGTs[AddedIndex].GlycanType != Glycan.Type.DeHex &&
        //            tmpPreAddGTs[AddedIndex].GlycanType != Glycan.Type.NeuAc &&
        //            tmpPreAddGTs[AddedIndex].GlycanType != Glycan.Type.NeuGc &&
        //            tmpPreAddGTs[AddedIndex].SubTree4 == null)
        //        {
        //            //tmpPreAddGTs[AddedIndex].AddGlycanSubTree(ToBeAddGlycanTree, argGT.NextID);
        //            newTree.AddGlycanToStructure(ToBeAddGlycanTree, tmpPreAddGTs[AddedIndex].NodeID);
        //            //newTree.Root.UpdateGlycans();
        //            newTree.Root.SortSubTree();
        //            addedGT.Add(newTree);
        //        }
        //    }
        //    return addedGT;
        //}
     
    }
}
