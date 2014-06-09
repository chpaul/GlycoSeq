using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;
using COL.GlycoLib;
using COL.MassLib;
using COL.ProtLib;
namespace COL.GlycoSequence
{
    public class GlycanSequencing
    {

        private bool _isDebug = false;
        private bool _NGlycanData = true;
        private bool _allowSiaConnectToHexNac = false;
        private bool _createPrecursor = false;
        private float  _rewardForCompleteStructure = 0.0f;
        StreamWriter DebugSW;
        private string _debugFolderPath = "c:\\temp";
        float _MS2torelance = 0.8f; //dalton
        //float _torelance = 500.0f; // old one
        float _precursorTorelance = 50.0f;
        private float _y1mass = 0.0f;
        private int _y1charge = 0;
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
        private float _peptideMonoMass = 0.0f;
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
        private bool _isCYS_CAM=true;
        private string _status = "";
        private enumGlycanType _glycanType;
        public GlycanSequencing(MSScan argScan, float argY1, int argY1Charge)
        {
            _y1mass = argY1;
            _y1charge = argY1Charge;
            _scan = argScan;
            //_PrecusorMono = _scan.ParentAVGMonoMW;            
            //_peptideMonoMass = (float)(_y1mass - GlycanMass.GetGlycanAVGMasswithCharge(Glycan.Type.HexNAc, _y1charge)) * _y1charge - MassLib.Atoms.ProtonMass;
            //_glycanMonoMass = (float)(_scan.ParentAVGMonoMW - _peptideMonoMass);
            //_peptideMz = _y1mass - GlycanMass.GetGlycanAVGMasswithCharge(Glycan.Type.HexNAc, argY1Charge);
            GetAllPeaks();
            _structureRules = ReadFilterRules.ReadFilterRule();
        }
        public GlycanSequencing(MSScan argScan, float argY1, int argY1Charge, int argNoHex, int argNoHexNAc, int argNoDeHex, int argNoNeuAc, int argNoNeuGc, string argOutput, bool argNGlycanData, float argPeakTol, float argPrecursorTol)
        {
            _NGlycanData = argNGlycanData;
            _y1mass = argY1;
            _y1charge = argY1Charge;
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
            GetAllPeaks();
            _structureRules = ReadFilterRules.ReadFilterRule();
        }
        /// <summary>
        /// Use peptide sequence and number of compostion to get the best result
        /// </summary>
        /// <param name="argScan"></param>
        /// <param name="argPeptideSequence"></param>
        /// <param name="argIsCYS_CAM"></param>
        /// <param name="argY1Charge"></param>
        /// <param name="argNoHex"></param>
        /// <param name="argNoHexNAc"></param>
        /// <param name="argNoDeHex"></param>
        /// <param name="argNoNeuAc"></param>
        /// <param name="argNoNeuGc"></param>
        /// <param name="argOutput"></param>
        /// <param name="argNGlycanData"></param>
        /// <param name="argPeakTol"></param>
        /// <param name="argPrecursorTol"></param>
        /// <param name="argAVGMass"></param>
        public GlycanSequencing(MSScan argScan, string  argPeptideSequence, bool argIsCYS_CAM, int argY1Charge, int argNoHex, int argNoHexNAc, int argNoDeHex, int argNoNeuAc, int argNoNeuGc, string argOutput, bool argNGlycanData, float argPeakTol, float argPrecursorTol)
        {
            _NGlycanData = argNGlycanData;
            _peptideStr = argPeptideSequence;
            _isCYS_CAM = argIsCYS_CAM;
            AminoAcidMass MW = new AminoAcidMass();
            //if (argAVGMass)
            //{
                _y1mass = (MW.GetAVGMonoMW(argPeptideSequence, argIsCYS_CAM) + GlycanMass.GetGlycanAVGMass(Glycan.Type.HexNAc) + Atoms.ProtonMass * argY1Charge) / argY1Charge;
            //}
            //else
            //{
            //    _y1mass = (MW.GetMonoMW(argPeptideSequence, argIsCYS_CAM) + GlycanMass.GetGlycanMass(Glycan.Type.HexNAc) + Atoms.ProtonMass * argY1Charge) / argY1Charge;
            //}            
            _y1charge = argY1Charge;
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
            GetAllPeaks();
            _structureRules = ReadFilterRules.ReadFilterRule();
        }
        /*public GlycanSequencing(MSScan argScan, float argY1, int argY1Charge, string argOutput, bool argNGlycanData, float argPeakTol, float argPrecursorTol)
        {
            _NGlycanData = argNGlycanData;
            _y1mass = argY1;
            _y1charge = argY1Charge;
            _scan = argScan;
            _torelance = argPeakTol;
            _precursorTorelance = argPrecursorTol;
            _OutputLocation = argOutput;
            _PrecusorMono = _scan.ParentMonoMW;
            _peptideMonoMass = (float)((_y1mass - 1.0073) * argY1Charge - GlycanMass.GetGlycanMass(Glycan.Type.HexNAc));
            _glycanMonoMass = (float)(_scan.ParentMonoMW - _peptideMonoMass);
            _peptideMz = _y1mass - GlycanMass.GetGlycanMasswithCharge(Glycan.Type.HexNAc, argY1Charge);
            GetAllPeaks();
            //GenerateGlycanMass(_y1charge, out _glycanBuildingblock);
            ReadFilterRules();
        }*/
        /*public GlycanSequencing(MSScan argScan, float argY1, int argY1Charge, string argOutput, float argPeakTol, float argPrecursorTol)
        {
            _y1mass = argY1;
            _y1charge = argY1Charge;
            _scan = argScan;
            _torelance = argPeakTol;
            _precursorTorelance = argPrecursorTol;
            _OutputLocation = argOutput;
            _PrecusorMono = _scan.ParentMonoMW;
            _peptideMonoMass = (float)((_y1mass - 1.0073) * argY1Charge - GlycanMass.GetGlycanMasswithCharge(Glycan.Type.HexNAc, 1));
            _glycanMonoMass = (float)(_scan.ParentMonoMW - _peptideMonoMass);
            _peptideMz = _y1mass - GlycanMass.GetGlycanMasswithCharge(Glycan.Type.HexNAc, argY1Charge);
            GetAllPeaks();
            //GenerateGlycanMass(_y1charge, out _glycanBuildingblock); 
            ReadFilterRules();
        }*/
        /*public GlycanSequencing(MSScan argScan, float argY1, int argY1Charge, bool argIsHex, bool argIsHexNAc, bool argIsDeHex, bool argIsNeuAc, bool argIsNeuGc, string argOutput, bool argNGlycanData, float argPeakTol, float argPrecursorTol)
        {
            _NGlycanData = argNGlycanData;
            _y1mass = argY1;
            _y1charge = argY1Charge;
            _scan = argScan;
            if (argIsHex == false)
            {
                _NoHex = 0;
            }
            if (argIsDeHex == false)
            {
                _NoDeHex = 0;
            }
            if (argIsHexNAc == false)
            {
                _NoHexNac = 0;
            }
            if (argIsNeuAc == false)
            {
                _NoNeuAc = 0;
            }
            if (argIsNeuGc == false)
            {
                _NoNeuGc = 0;
            }
            
            _OutputLocation = argOutput;
            _PrecusorMono = _scan.ParentMonoMW;
            _peptideMonoMass = (float)((_y1mass - 1.0073) * argY1Charge - GlycanMass.GetGlycanMasswithCharge(Glycan.Type.HexNAc, 1));
            _glycanMonoMass = (float)(_scan.ParentMonoMW - _peptideMonoMass);
            _peptideMz = _y1mass - GlycanMass.GetGlycanMasswithCharge(Glycan.Type.HexNAc, argY1Charge);
            _torelance = argPeakTol;
            _precursorTorelance = argPrecursorTol;
            GetAllPeaks();
            //GenerateGlycanMass(_y1charge, out _glycanBuildingblock);
            ReadFilterRules();
        }*/
    
        /*public GlycanSequencing(MSScan argScan, float argY1, int argY1Charge, bool argIsHex, bool argIsHexNAc, bool argIsDeHex, bool argIsNeuAc, bool argIsNeuGc, string argOutput)
        {
            _y1mass = argY1;
            _y1charge = argY1Charge;
            _scan = argScan;
            if (argIsHex == false)
            {
                _NoHex = 0;
            }
            if (argIsDeHex == false)
            {
                _NoDeHex = 0;
            }
            if (argIsHexNAc == false)
            {
                _NoHexNac = 0;
            }
            if (argIsNeuAc == false)
            {
                _NoNeuAc = 0;
            }
            if (argIsNeuGc == false)
            {
                _NoNeuGc = 0;
            }
            _OutputLocation = argOutput;
            _PrecusorMono = _scan.ParentMonoMW;
            _peptideMonoMass = (float)((_y1mass - 1.0073) * argY1Charge - GlycanMass.GetGlycanMasswithCharge(Glycan.Type.HexNAc, 1));
            _glycanMonoMass = (float)(_scan.ParentMonoMW - _peptideMonoMass);

            GetAllPeaks();
            //GenerateGlycanMass(_y1charge, out _glycanBuildingblock);
            ReadFilterRules();


        }*/
        public enumGlycanType GlycanType
        {
            get { return _glycanType; }
            set { _glycanType = value; }
        }

        public string PeptideSeq
        {
            get { return _peptideStr; }
        }
        public MSScan ScanInfo
        {
            set { _scan = value; }
            get { return _scan; }
        }
        public float Y1
        {
            set { _y1mass = value; }
            get { return  _y1mass; }
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
            set { _UseAVGMass = value; 
                    if(_UseAVGMass ==false)
                    {
                        AminoAcidMass MW = new AminoAcidMass();
                        _y1mass = (MW.GetMonoMW(_peptideStr, _isCYS_CAM) + GlycanMass.GetGlycanMass(Glycan.Type.HexNAc) + Atoms.ProtonMass * _y1charge) / _y1charge;
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
            get { return _peptideMonoMass; }
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
        private void UpdateStatus(string argStatue)
        {
            _status = _status + argStatue + ";";
        }
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
            GetCandidatePeaks(_NumbersOfPeaksForSeq);
            if (_potentialPeaks.Count <= 20)
            {
                return -2;
            }
            if (_y1mass > 2000.0f )
            {
                return -1;
            }
            //if (_potentialPeaks.Count < _NumbersOfPeaksForSeq)
            //{
            //    return -2;
            //}
            if (_NGlycanData)
            {
                FindNLinkedStructure();
            }
            else
            {
                FindOLinkedStructure();
            }
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

        private void GetAllPeaks()
        {
            _allPeaks = new List<MSPoint>();
            for (int i = 0; i < _scan.MSPeaks.Count; i++)
            {

                    _allPeaks.Add(new MSPoint(_scan.MSPeaks[i].MonoMass, _scan.MSPeaks[i].MonoIntensity));
                    if (_scan.MSPeaks[i].MonoIntensity > MaxIntensityInScan)
                    {
                        MaxIntensityInScan = (float)_scan.MSPeaks[i].MonoIntensity;
                    } 
            }
        }

        private void GenerateGlycanMass(int argCharge, out List<Glycan> argBuildingBlocks)
        {
            argBuildingBlocks = new List<Glycan>();
            if (_NoNeuAc!=0)
            {
                argBuildingBlocks.Add(new Glycan(Glycan.Type.NeuAc, argCharge));
            }
            if (_NoNeuGc!=0)
            {
                argBuildingBlocks.Add(new Glycan(Glycan.Type.NeuGc, argCharge));
            }

            if (_NoHexNac!=0)
            {
                argBuildingBlocks.Add(new Glycan(Glycan.Type.HexNAc, argCharge));
            }
            if (_NoHex !=0)
            {
                argBuildingBlocks.Add(new Glycan(Glycan.Type.Hex, argCharge));
            }
            if (_NoDeHex!=0)
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
                if (_allPeaks[i].Mass >= _y1mass - 2.0f)
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
        private void FindOLinkedStructure()
        {
            #region AVG Mass
            if (_UseAVGMass)
            {
                if (_scan.ParentAVGMonoMW != 0.0f)
                {
                    UpdateStatus("Parent avg mono found");
                    _PrecusorMono = _scan.ParentAVGMonoMW;
                }
                else
                {

                    // try to recover PrecusorMono
                    if (_peptideStr != "")
                    {
                        UpdateStatus("Parent avg mono not found use peptide and composition");
                        AminoAcidMass MW = new AminoAcidMass();
                        _PrecusorMono = MW.GetAVGMonoMW(_peptideStr, _isCYS_CAM) +
                                                         GlycanMass.GetGlycanAVGMass(Glycan.Type.HexNAc) * _NoHexNac +
                                                         GlycanMass.GetGlycanAVGMass(Glycan.Type.Hex) * _NoHex +
                                                         GlycanMass.GetGlycanAVGMass(Glycan.Type.DeHex) * _NoDeHex +
                                                         GlycanMass.GetGlycanAVGMass(Glycan.Type.NeuAc) * _NoNeuAc +
                                                         GlycanMass.GetGlycanAVGMass(Glycan.Type.NeuGc) * _NoNeuGc;
                        _y1mass = (MW.GetAVGMonoMW(_peptideStr, _isCYS_CAM) + GlycanMass.GetGlycanAVGMass(Glycan.Type.HexNAc) + Atoms.ProtonMass * _y1charge) / _y1charge;
                    }
                    else if (_scan.ParentMonoMW != 0.0)
                    {
                        UpdateStatus("Parent avg mono not found use parent mono");
                        _PrecusorMono = _scan.ParentMonoMW;
                    }
                    else
                    {
                        UpdateStatus("Parent avg mono not found use header mz");
                        _PrecusorMono = (_scan.ParentMZ - Atoms.ProtonMass) * _scan.ParentMZ;
                    }
                } //End  of Recovery Precursor mono


                if (_y1mass == 0.0f)
                {
                    AminoAcidMass MW = new AminoAcidMass();
                    _y1mass = (MW.GetAVGMonoMW(_peptideStr, _isCYS_CAM) + GlycanMass.GetGlycanAVGMass(Glycan.Type.HexNAc) + Atoms.ProtonMass * _y1charge) / _y1charge;
                }
                _peptideMonoMass = (float)(_y1mass - GlycanMass.GetGlycanAVGMasswithCharge(Glycan.Type.HexNAc, _y1charge)) * _y1charge - MassLib.Atoms.ProtonMass;
                _glycanMonoMass = (float)(_PrecusorMono - _peptideMonoMass);
                //_peptideMz = _y1mass - GlycanMass.GetGlycanAVGMasswithCharge(Glycan.Type.HexNAc, _y1charge);

            }
            #endregion 
            #region Use Mono Mass
            else // Use mono mass
            {
                if (_scan.ParentMonoMW != 0.0f)
                {
                    _PrecusorMono = _scan.ParentMonoMW;
                }
                else
                {
                    // try to recover PrecusorMono
                    if (_peptideStr != "")
                    {
                        AminoAcidMass MW = new AminoAcidMass();
                        _PrecusorMono = MW.GetMonoMW(_peptideStr, _isCYS_CAM) +
                                                         GlycanMass.GetGlycanMass(Glycan.Type.HexNAc) * _NoHexNac +
                                                         GlycanMass.GetGlycanMass(Glycan.Type.Hex) * _NoHex +
                                                         GlycanMass.GetGlycanMass(Glycan.Type.DeHex) * _NoDeHex +
                                                         GlycanMass.GetGlycanMass(Glycan.Type.NeuAc) * _NoNeuAc +
                                                         GlycanMass.GetGlycanMass(Glycan.Type.NeuGc) * _NoNeuGc;
                        _y1mass = (MW.GetMonoMW(_peptideStr, _isCYS_CAM) + GlycanMass.GetGlycanMass(Glycan.Type.HexNAc) + Atoms.ProtonMass * _y1charge) / _y1charge;
                    }
                    else
                    {
                        _PrecusorMono = (_scan.ParentMZ - Atoms.ProtonMass) * _scan.ParentMZ;
                    }
                }
                if (_y1mass == 0.0f)
                {
                    AminoAcidMass MW = new AminoAcidMass();
                    _y1mass = (MW.GetMonoMW(_peptideStr, _isCYS_CAM) + GlycanMass.GetGlycanMass(Glycan.Type.HexNAc) + Atoms.ProtonMass * _y1charge) / _y1charge;
                }
                _peptideMonoMass = (float)(_y1mass - GlycanMass.GetGlycanMasswithCharge(Glycan.Type.HexNAc, _y1charge)) * _y1charge - MassLib.Atoms.ProtonMass;
                _glycanMonoMass = (float)(_PrecusorMono - _peptideMonoMass);
               // _peptideMz = _y1mass - GlycanMass.GetGlycanMasswithCharge(Glycan.Type.HexNAc, _y1charge);
            }
            #endregion

            //-------Hard code part--------
            //_PrecusorMono = 5407.23229f;
            //_peptideMonoMass = 2399.187f;
            //_glycanMonoMass = (float)(_PrecusorMono - _peptideMonoMass);
            //_y1mass = (float)(_peptideMonoMass + GlycanMass.GetGlycanMass(Glycan.Type.HexNAc) + COL.MassLib.Atoms.ProtonMass * 3) / 3;
            //_peptideMz = _y1mass - GlycanMass.GetGlycanMasswithCharge(Glycan.Type.HexNAc, _y1charge);
            //---------------

            _SequencedGlycanStructure.Add(new GlycanStructure(new Glycan(Glycan.Type.HexNAc, _y1charge), _y1mass)); //Add Y1 into Candidate;
            UpdateStatus("Y1=" + _y1mass.ToString("0.0000") + "  Y1z=" + _y1charge.ToString());
            //Find Y1
            int CloseY1Idx = MassUtility.GetClosestMassIdx(_potentialPeaks, _y1mass);
            if (Math.Abs(_y1mass - _potentialPeaks[CloseY1Idx].Mass) <= _MS2torelance)
            {
                _SequencedGlycanStructure[0].Root.IDPeak(_potentialPeaks[CloseY1Idx].Mass, _potentialPeaks[CloseY1Idx].Intensity);
            }

            if (_isDebug)
            {
                DebugSW = new StreamWriter(_debugFolderPath + "\\StructureListByMass_" + _scan.ScanNo + ".txt");
            }
            //charge
            for (int chargePlus = 0; chargePlus <= 1; chargePlus++)
            {
                int CurrentCharge = _y1charge + chargePlus;
                float CurrentY1Mz = Convert.ToSingle((_y1mass * _y1charge + 1.0079 * chargePlus) / CurrentCharge);
                GenerateGlycanMass(CurrentCharge, out _glycanBuildingblock);
                float PeptideMone = 0.0f;
                if (_UseAVGMass)
                {
                    PeptideMone = (CurrentY1Mz - GlycanMass.GetGlycanAVGMasswithCharge(Glycan.Type.HexNAc, CurrentCharge)) * CurrentCharge - MassLib.Atoms.ProtonMass;
                }
                else
                {
                    PeptideMone = (CurrentY1Mz - GlycanMass.GetGlycanMasswithCharge(Glycan.Type.HexNAc, CurrentCharge)) * CurrentCharge - MassLib.Atoms.ProtonMass;
                }
                foreach (MSPoint pk in _potentialPeaks)
                {
                    for (int i = 0; i < _SequencedGlycanStructure.Count; i++)
                    {
                        GlycanStructure CandidateTree = (GlycanStructure)_SequencedGlycanStructure[i];
                        foreach (GlycanTreeNode Node in CandidateTree.Root.TravelGlycanTreeBFS()) //Convert All Internal node to current charge
                        {
                            Node.Charge = CurrentCharge;
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



                            /*if (Math.Abs((float)(CandidateTree.GlycanAVGMonoMass + MassLib.Atoms.ProtonMass * CurrentCharge) 
                                    / (float)CurrentCharge + ExtraGlycan.AVGMz + CurrentY1Mz - GlycanMass.GetGlycanAVGMasswithCharge(Glycan.Type.HexNAc, CurrentCharge)
                                    - pk.Mass) 
                                    > _MS2torelance)*/
                            if (_UseAVGMass)
                            {
                                if (Math.Abs(((CandidateTree.GlycanAVGMonoMass + PeptideMone + ExtraGlycan.AVGMass + MassLib.Atoms.ProtonMass * CurrentCharge) / CurrentCharge) - pk.Mass) > _MS2torelance)
                                {
                                    continue;
                                }
                                else //Add One monosaccharide to Tree
                                {
                                    if (ExtraGlycan.GlycanType == Glycan.Type.DeHex && CandidateTree.Root.GlycanType != Glycan.Type.HexNAc) // DeHex only connect to HexNac
                                    {
                                        continue;
                                    }
                                    GlycanStructure CloneTree = (GlycanStructure)CandidateTree.Clone();


                                    List<GlycanStructure> AddedTrees = AddGlycanToOGlycanTree(CloneTree, ExtraGlycan, pk.Mass, pk.Intensity);

                                    if (_isDebug)
                                    {
                                        string tmp = pk.Mass.ToString("0.000") + "," + CloneTree.IUPACString + "," + ExtraGlycan.GlycanType.ToString() + ",";
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
                                        iterGT.Root.SortSubTree();
                                        iterGT.Charge = CurrentCharge;
                                        if (!_SequencedGlycanStructure.Contains(iterGT))
                                        {
                                            _SequencedGlycanStructure.Add(iterGT);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (Math.Abs(((CandidateTree.GlycanMonoMass + PeptideMone + ExtraGlycan.Mass + MassLib.Atoms.ProtonMass * CurrentCharge) / CurrentCharge) - pk.Mass) > _MS2torelance)
                                {
                                    continue;
                                }
                                else //Add One monosaccharide to Tree
                                {
                                    if (ExtraGlycan.GlycanType == Glycan.Type.DeHex && CandidateTree.Root.GlycanType != Glycan.Type.HexNAc) // DeHex only connect to HexNac
                                    {
                                        continue;
                                    }
                                    GlycanStructure CloneTree = (GlycanStructure)CandidateTree.Clone();

                                    List<GlycanStructure> AddedTrees = AddGlycanToOGlycanTree(CloneTree, ExtraGlycan, pk.Mass, pk.Intensity);

                                    if (_isDebug)
                                    {
                                        string tmp = CloneTree.IUPACString + "," + ExtraGlycan.GlycanType.ToString() + ",";
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
                                        iterGT.Root.SortSubTree();
                                        iterGT.Charge = CurrentCharge;
                                        if (!_SequencedGlycanStructure.Contains(iterGT))
                                        {
                                            _SequencedGlycanStructure.Add(iterGT);
                                        }
                                    }
                                }
                            }


                        }//iterate Glycan
                    } //iterate CandidateGlycan
                }//iterate Peaks


                //Extra structure check
                //List<GlycanStructure> NewlyAdded = new List<GlycanStructure>();
                //float ParentMonoMW = _scan.ParentMonoMW;

                //foreach (GlycanStructure t in NewlyAdded)
                //{
                //    t.Root.SortSubTree();
                //    _SequencedGlycanStructure.Add(t);
                //}

                //NewlyAdded.Clear();      

                //foreach (GlycanStructure t in NewlyAdded)
                //{
                //    t.Root.SortSubTree();
                //    _SequencedGlycanStructure.Add(t);
                //}                
            }
            _SequencedGlycanStructure = CleanOLinkedStructure(_SequencedGlycanStructure);

            //Complete the structure (add last glycan) using percursor average or first mono mass             

            foreach (GlycanStructure t in _SequencedGlycanStructure)
            {
                for (int i = 0; i < _glycanBuildingblock.Count; i++)
                {
                    Glycan NextGlycan = new Glycan(_glycanBuildingblock[i].GlycanType, 1);
                    if (_UseAVGMass)
                    {
                        if (PrecusorAVGMonoMass != 0.0f)
                        {
                            if (MassUtility.GetMassPPM(t.GlycanAVGMonoMass + PeptideMonoMass + NextGlycan.AVGMass, PrecusorAVGMonoMass) < _precursorTorelance)
                            {
                                List<GlycanStructure> AddedTrees = AddGlycanToNGlycanTree(t, NextGlycan, _PrecusorMono, 0.0f);
                                _FullSequencedGlycanStructure.AddRange(AddedTrees);
                                continue;
                            }
                            if (_createPrecursor)
                            {
                                //Create Precursor m/z
                                for (int j = t.Charge; j <= _scan.ParentCharge; j++)
                                {
                                    float PrecusorMZ = (_PrecusorMono + Atoms.ProtonMass * j) / (float)j;
                                    float GlycoPeptidMZ = (t.GlycanAVGMonoMass + PeptideMonoMass + NextGlycan.AVGMass + Atoms.ProtonMass * j) / (float)j;

                                    if (Math.Abs(PrecusorMZ - GlycoPeptidMZ) <= _MS2torelance)
                                    {
                                        List<GlycanStructure> AddedTrees = AddGlycanToNGlycanTree(t, NextGlycan, _PrecusorMono, 0.0f);
                                        _FullSequencedGlycanStructure.AddRange(AddedTrees);
                                    }
                                }
                            }
                        }
                        else //Precursor AVG Mono not found
                        {
                            if (MassUtility.GetMassPPM(t.GlycanAVGMonoMass + PeptideMonoMass + NextGlycan.AVGMass, PrecusorMonoMass) < _precursorTorelance * 3)
                            {
                                List<GlycanStructure> AddedTrees = AddGlycanToNGlycanTree(t, NextGlycan, _PrecusorMono, 0.0f);
                                _FullSequencedGlycanStructure.AddRange(AddedTrees);
                                continue;
                            }
                            if (_createPrecursor)
                            {
                                //Create Precursor m/z
                                for (int j = t.Charge - 1; j <= _scan.ParentCharge; j++)
                                {
                                    float PrecusorMZ = (_PrecusorMono + Atoms.ProtonMass * j) / (float)j;
                                    float GlycoPeptidMZ = (t.GlycanAVGMonoMass + PeptideMonoMass + NextGlycan.AVGMass + Atoms.ProtonMass * j) / (float)j;

                                    if (Math.Abs(PrecusorMZ - GlycoPeptidMZ) <= _MS2torelance)
                                    {
                                        List<GlycanStructure> AddedTrees = AddGlycanToNGlycanTree(t, NextGlycan, _PrecusorMono, 0.0f);
                                        _FullSequencedGlycanStructure.AddRange(AddedTrees);
                                    }
                                }
                            }
                        }
                    }
                    else //non-AVG Mass
                    {
                        if (MassUtility.GetMassPPM(t.GlycanMonoMass + PeptideMonoMass + NextGlycan.Mass, PrecusorMonoMass) < _precursorTorelance)
                        {
                            List<GlycanStructure> AddedTrees = AddGlycanToNGlycanTree(t, NextGlycan, _PrecusorMono, 0.0f);
                            _FullSequencedGlycanStructure.AddRange(AddedTrees);
                            continue;
                        }
                        if (_createPrecursor)
                        {
                            //Create Precursor m/z
                            for (int j = t.Charge; j <= _scan.ParentCharge; j++)
                            {
                                float PrecusorMZ = (_PrecusorMono + Atoms.ProtonMass * j) / (float)j;
                                float GlycoPeptidMZ = (t.GlycanMonoMass + PeptideMonoMass + NextGlycan.Mass + Atoms.ProtonMass * j) / (float)j;

                                if (Math.Abs(PrecusorMZ - GlycoPeptidMZ) <= _MS2torelance)
                                {
                                    List<GlycanStructure> AddedTrees = AddGlycanToNGlycanTree(t, NextGlycan, _PrecusorMono, 0.0f);
                                    _FullSequencedGlycanStructure.AddRange(AddedTrees);
                                }
                            }
                        }
                    }
                }
            }

            //Use number of compostion to complete structure
            if (_FullSequencedGlycanStructure.Count == 0 && _NumCompostionSet == true)
            {
                foreach (GlycanStructure GS in _SequencedGlycanStructure)
                {
                    int LeftGlycan = 0;
                    LeftGlycan = _NoHex - GS.NoOfHex;
                    LeftGlycan = LeftGlycan + (_NoHexNac - GS.NoOfHexNac);
                    LeftGlycan = LeftGlycan + (_NoDeHex - GS.NoOfDeHex);
                    LeftGlycan = LeftGlycan + (_NoNeuAc - GS.NoOfNeuAc);
                    LeftGlycan = LeftGlycan + (_NoNeuGc - GS.NoOfNeuGc);

                    List<Glycan> ExtraGlycan = new List<Glycan>();
                    if (LeftGlycan <= 2)
                    {
                        for (int i = 0; i < _NoHex - GS.NoOfHex; i++)
                        {
                            ExtraGlycan.Add(new Glycan(Glycan.Type.Hex, GS.Charge));
                        }
                        for (int i = 0; i < (_NoHexNac - GS.NoOfHexNac); i++)
                        {
                            ExtraGlycan.Add(new Glycan(Glycan.Type.HexNAc, GS.Charge));
                        }
                        for (int i = 0; i < (_NoDeHex - GS.NoOfDeHex); i++)
                        {
                            ExtraGlycan.Add(new Glycan(Glycan.Type.DeHex, GS.Charge));
                        }
                        for (int i = 0; i < (_NoNeuAc - GS.NoOfNeuAc); i++)
                        {
                            ExtraGlycan.Add(new Glycan(Glycan.Type.NeuAc, GS.Charge));
                        }
                        for (int i = 0; i < (_NoNeuGc - GS.NoOfNeuGc); i++)
                        {
                            ExtraGlycan.Add(new Glycan(Glycan.Type.NeuGc, GS.Charge));
                        }
                    }
                }
            }

            if (_FullSequencedGlycanStructure.Count != 0)
            {
                _FullSequencedGlycanStructure = CleanOLinkedStructure(_FullSequencedGlycanStructure);
                //Reware for complete structure;
                if (_rewardForCompleteStructure != 0.0f)
                {
                    foreach (GlycanStructure g in _FullSequencedGlycanStructure)
                    {
                        g.Score = g.Score + _rewardForCompleteStructure;
                    }
                }
            }
            if (_isDebug)
            {
                DebugSW.Close();
            }
        }
        private void FindNLinkedStructure()
        {           
            AminoAcidMass MW = new AminoAcidMass();
            //if (_UseAVGMass)
            //{
            //    if(_scan.ParentAVGMonoMW != 0.0f)
            //    {
            //        UpdateStatus("Parent avg mono found");
            //        //_PrecusorMono = _scan.ParentAVGMonoMW;
            //        _PrecusorMono = _scan.ParentMonoMW;
            //    }
            //    else
            //    {                   
            //            // try to recover PrecusorMono
            //           if(_peptideStr!="")
            //            {
            //                UpdateStatus("Parent avg mono not found use peptide and composition");                           
            //               _PrecusorMono = MW.GetAVGMonoMW(_peptideStr, _isCYS_CAM)+
            //                                                GlycanMass.GetGlycanAVGMass(Glycan.Type.HexNAc) * _NoHexNac +
            //                                                GlycanMass.GetGlycanAVGMass(Glycan.Type.Hex) * _NoHex +
            //                                                GlycanMass.GetGlycanAVGMass(Glycan.Type.DeHex) * _NoDeHex +
            //                                                GlycanMass.GetGlycanAVGMass(Glycan.Type.NeuAc) * _NoNeuAc +
            //                                                GlycanMass.GetGlycanAVGMass(Glycan.Type.NeuGc) *_NoNeuGc ;                           
            //               _y1mass = (MW.GetAVGMonoMW(_peptideStr, _isCYS_CAM) +  GlycanMass.GetGlycanAVGMass(Glycan.Type.HexNAc)  + Atoms.ProtonMass * _y1charge) / _y1charge;
            //            }
            //           else if ( _scan.ParentMonoMW!=0.0)
            //           {
            //               UpdateStatus("Parent avg mono not found use parent mono");
            //               _PrecusorMono = _scan.ParentMonoMW;
            //           }
            //           else 
            //            {
            //                UpdateStatus("Parent avg mono not found use header mz");
            //                _PrecusorMono = (_scan.ParentMZ  - Atoms.ProtonMass)* _scan.ParentMZ;
            //            }                    
            //    }
            //    if (_y1mass == 0.0f)
            //    {                  
            //        _y1mass = (MW.GetAVGMonoMW(_peptideStr, _isCYS_CAM) + GlycanMass.GetGlycanAVGMass(Glycan.Type.HexNAc) + Atoms.ProtonMass * _y1charge) / _y1charge;
            //    }
            //    _peptideMonoMass = (float)(_y1mass - GlycanMass.GetGlycanAVGMasswithCharge(Glycan.Type.HexNAc, _y1charge)) * _y1charge - MassLib.Atoms.ProtonMass;                
            //    _glycanMonoMass = (float)(_PrecusorMono - _peptideMonoMass);
            //    //_peptideMz = _y1mass - GlycanMass.GetGlycanAVGMasswithCharge(Glycan.Type.HexNAc, _y1charge);         

            //}
            //else //Not average mass
            //{
            //    if(_scan.ParentMonoMW != 0.0f)
            //    {
            //        _PrecusorMono = _scan.ParentMonoMW;
            //    }
            //    else
            //    {
            //            // try to recover PrecusorMono
            //           if(_peptideStr!="")
            //            {                            
            //               _PrecusorMono = MW.GetMonoMW(_peptideStr, _isCYS_CAM)+
            //                                                GlycanMass.GetGlycanMass(Glycan.Type.HexNAc) * _NoHexNac +
            //                                                GlycanMass.GetGlycanMass(Glycan.Type.Hex) * _NoHex +
            //                                                GlycanMass.GetGlycanMass(Glycan.Type.DeHex) * _NoDeHex +
            //                                                GlycanMass.GetGlycanMass(Glycan.Type.NeuAc) * _NoNeuAc +
            //                                                GlycanMass.GetGlycanMass(Glycan.Type.NeuGc) *_NoNeuGc ;                           
            //               _y1mass = (MW.GetMonoMW(_peptideStr, _isCYS_CAM) +  GlycanMass.GetGlycanMass(Glycan.Type.HexNAc)  + Atoms.ProtonMass * _y1charge) / _y1charge;
            //            }
            //           else 
            //            {
            //                _PrecusorMono = (_scan.ParentMZ  - Atoms.ProtonMass)* _scan.ParentMZ;
            //            }                    
            //    }
            //    if (_y1mass == 0.0f)
            //    {
                    
            //        _y1mass = (MW.GetMonoMW(_peptideStr, _isCYS_CAM) + GlycanMass.GetGlycanMass(Glycan.Type.HexNAc) + Atoms.ProtonMass * _y1charge) / _y1charge;
            //    }
            //    _peptideMonoMass = (float)MW.GetMonoMW(_peptideStr, _isCYS_CAM) ;
            //    _glycanMonoMass = (float)(_PrecusorMono - _peptideMonoMass);
            //    //_peptideMz = _y1mass - GlycanMass.GetGlycanMasswithCharge(Glycan.Type.HexNAc, _y1charge);
            //}


            //-------Hard code part--------
            //_PrecusorMono = 5407.23229f;
            //_peptideMonoMass = 2399.187f;
            //_glycanMonoMass = (float)(_PrecusorMono - _peptideMonoMass);
            //_y1mass = (float)(_peptideMonoMass + GlycanMass.GetGlycanMass(Glycan.Type.HexNAc) + COL.MassLib.Atoms.ProtonMass * 3) / 3;
            //_peptideMz = _y1mass - GlycanMass.GetGlycanMasswithCharge(Glycan.Type.HexNAc, _y1charge);
            //---------------

            //_y1mass = (MW.GetAVGMonoMW(_peptideStr, _isCYS_CAM) + GlycanMass.GetGlycanAVGMass(Glycan.Type.HexNAc)) / _y1charge + Atoms.ProtonMass;

            _SequencedGlycanStructure.Add(new GlycanStructure(new Glycan(Glycan.Type.HexNAc, _y1charge), _y1mass)); //Add Y1 into Candidate;
            UpdateStatus("Y1="+ _y1mass.ToString("0.0000")+"  Y1z=" + _y1charge.ToString());
            //Find Y1
            int CloseY1Idx =   MassUtility.GetClosestMassIdx( _potentialPeaks,_y1mass);
            if (Math.Abs(_y1mass - _potentialPeaks[CloseY1Idx].Mass) <= _MS2torelance)
            {
                _SequencedGlycanStructure[0].Root.IDPeak(_potentialPeaks[CloseY1Idx].Mass, _potentialPeaks[CloseY1Idx].Intensity);
            }

            if (_isDebug)
            {
                DebugSW = new StreamWriter(_debugFolderPath + "\\StructureListByMass_" + _scan.ScanNo + ".txt");
            }
            //charge
            for (int chargePlus = 0; chargePlus <=1; chargePlus++)
            {
                int CurrentCharge = _y1charge + chargePlus;
                float CurrentY1Mz = Convert.ToSingle((_y1mass *_y1charge + 1.0079 * chargePlus) / CurrentCharge);
                GenerateGlycanMass(CurrentCharge, out _glycanBuildingblock);
                float PeptideMono = 0.0f;
                if (_UseAVGMass)
                {
                    PeptideMono = (CurrentY1Mz - GlycanMass.GetGlycanAVGMasswithCharge(Glycan.Type.HexNAc, CurrentCharge)) * CurrentCharge - MassLib.Atoms.ProtonMass;
                }
                else
                {
                    PeptideMono = (CurrentY1Mz - GlycanMass.GetGlycanMasswithCharge(Glycan.Type.HexNAc, CurrentCharge)) * CurrentCharge - MassLib.Atoms.ProtonMass;
                }
                foreach (MSPoint pk in _potentialPeaks)
                {
                    for (int i = 0; i < _SequencedGlycanStructure.Count; i++)
                    {
                        GlycanStructure CandidateTree =(GlycanStructure) _SequencedGlycanStructure[i];
                        foreach (GlycanTreeNode Node in CandidateTree.Root.TravelGlycanTreeBFS()) //Convert All Internal node to current charge
                        {
                            Node.Charge = CurrentCharge;
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
                            if ( (ExtraGlycan.GlycanType == Glycan.Type.Hex ||
                                    ExtraGlycan.GlycanType == Glycan.Type.Gal ||
                                    ExtraGlycan.GlycanType == Glycan.Type.Man
                                )
                                &&
                                _NoHex - CandidateTree.NoOfHex <= 0)
                            {
                                continue;
                            }

                     

                            /*if (Math.Abs((float)(CandidateTree.GlycanAVGMonoMass + MassLib.Atoms.ProtonMass * CurrentCharge) 
                                    / (float)CurrentCharge + ExtraGlycan.AVGMz + CurrentY1Mz - GlycanMass.GetGlycanAVGMasswithCharge(Glycan.Type.HexNAc, CurrentCharge)
                                    - pk.Mass) 
                                    > _MS2torelance)*/
                            if (_UseAVGMass)
                            {
                                if (Math.Abs(((CandidateTree.GlycanAVGMonoMass + PeptideMono + ExtraGlycan.AVGMass + MassLib.Atoms.ProtonMass * CurrentCharge) / CurrentCharge) - pk.Mass) > _MS2torelance)
                                //if (Math.Abs((_y1mass + CandidateTree.GlycanAVGMZ + ExtraGlycan.AVGMz - GlycanMass.GetGlycanAVGMasswithCharge(Glycan.Type.HexNAc, CurrentCharge)) - pk.Mass) > _MS2torelance)
                                {
                                    continue;
                                }
                                else //Add One monosaccharide to Tree
                                {
                                    if (ExtraGlycan.GlycanType == Glycan.Type.DeHex && CandidateTree.Root.GlycanType != Glycan.Type.HexNAc) // DeHex only connect to HexNac
                                    {
                                        continue;
                                    }
                                    GlycanStructure CloneTree = (GlycanStructure)CandidateTree.Clone();


                                    List<GlycanStructure> AddedTrees = AddGlycanToNGlycanTree(CloneTree, ExtraGlycan, pk.Mass, pk.Intensity);

                                    if (_isDebug)
                                    {
                                        string tmp = pk.Mass.ToString("0.000")+","+CloneTree.IUPACString + "," + ExtraGlycan.GlycanType.ToString() + ",";
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
                                        iterGT.Root.SortSubTree();
                                        iterGT.Charge = CurrentCharge;
                                        if (!_SequencedGlycanStructure.Contains(iterGT))
                                        {
                                            _SequencedGlycanStructure.Add(iterGT);
                                        }
                                    }
                                }
                            }
                            //else
                            //{
                            //    if (Math.Abs(((CandidateTree.GlycanMonoMass + PeptideMono + ExtraGlycan.Mass + MassLib.Atoms.ProtonMass * CurrentCharge) / CurrentCharge) - pk.Mass) > _MS2torelance)
                            //   // if (Math.Abs((CandidateTree.GlycanMZ + _y1mass + ExtraGlycan.Mz - GlycanMass.GetGlycanAVGMasswithCharge(Glycan.Type.HexNAc, CurrentCharge)) - pk.Mass) > _MS2torelance)
                            //    {
                            //        continue;
                            //    }
                            //    else //Add One monosaccharide to Tree
                            //    {
                            //        if (ExtraGlycan.GlycanType == Glycan.Type.DeHex && CandidateTree.Root.GlycanType != Glycan.Type.HexNAc) // DeHex only connect to HexNac
                            //        {
                            //            continue;
                            //        }
                            //        GlycanStructure CloneTree = (GlycanStructure)CandidateTree.Clone();

                            //        List<GlycanStructure> AddedTrees = AddGlycanToNGlycanTree(CloneTree, ExtraGlycan, pk.Mass, pk.Intensity);

                            //        if (_isDebug)
                            //        {
                            //            string tmp = CloneTree.IUPACString + "," + ExtraGlycan.GlycanType.ToString() + ",";
                            //            List<string> Structure = new List<string>();
                            //            foreach (GlycanStructure T in AddedTrees)
                            //            {
                            //                if (!Structure.Contains(T.IUPACString))
                            //                {
                            //                    Structure.Add(T.IUPACString);
                            //                }
                            //            }
                            //            foreach (string str in Structure)
                            //            {
                            //                tmp = tmp + str + ",";
                            //            }
                            //            DebugSW.WriteLine(tmp.Substring(0, tmp.Length - 1));
                            //        }

                            //        foreach (GlycanStructure iterGT in AddedTrees)
                            //        {
                            //            iterGT.Root.SortSubTree();
                            //            iterGT.Charge = CurrentCharge;
                            //            if (!_SequencedGlycanStructure.Contains(iterGT))
                            //            {
                            //                _SequencedGlycanStructure.Add(iterGT);
                            //            }
                            //        }
                            //    }
                            //}


                        }//iterate Glycan
                    } //iterate CandidateGlycan
                }//iterate Peaks


                //Extra structure check
                //List<GlycanStructure> NewlyAdded = new List<GlycanStructure>();
                //float ParentMonoMW = _scan.ParentMonoMW;
 
                //foreach (GlycanStructure t in NewlyAdded)
                //{
                //    t.Root.SortSubTree();
                //    _SequencedGlycanStructure.Add(t);
                //}

                //NewlyAdded.Clear();      

                //foreach (GlycanStructure t in NewlyAdded)
                //{
                //    t.Root.SortSubTree();
                //    _SequencedGlycanStructure.Add(t);
                //}                
            }
            _SequencedGlycanStructure = CleanNLinkedStructure(_SequencedGlycanStructure);        

            //Complete the structure (add last glycan) using percursor average or first mono mass            
         
            foreach (GlycanStructure t in _SequencedGlycanStructure)
            {
                for (int i = 0; i < _glycanBuildingblock.Count; i++)
                {
                    Glycan NextGlycan = new Glycan(_glycanBuildingblock[i].GlycanType, 1);
                    if(MassUtility.GetMassPPM( (t.GlycanMonoMass+NextGlycan.Mass + MW.GetMonoMW(_peptideStr, _isCYS_CAM) )/ _scan.ParentCharge + Atoms.ProtonMass ,_scan.ParentMZ)<_precursorTorelance)
                    {
                        List<GlycanStructure> AddedTrees = AddGlycanToNGlycanTree(t, NextGlycan, _scan.ParentMonoMW, 0.0f);
                        _FullSequencedGlycanStructure.AddRange(AddedTrees);
                    }
                    //if (_UseAVGMass)
                    //{
                    //    if (PrecusorAVGMonoMass != 0.0f)
                    //    {
                    //        if (MassUtility.GetMassPPM(t.GlycanAVGMonoMass + PeptideMonoMass + NextGlycan.AVGMass, PrecusorAVGMonoMass) < _precursorTorelance)
                    //        {
                    //            List<GlycanStructure> AddedTrees = AddGlycanToNGlycanTree(t, NextGlycan, _PrecusorMono, 0.0f);
                    //            _FullSequencedGlycanStructure.AddRange(AddedTrees);
                    //            continue;
                    //        }
                    //        if (_createPrecursor)
                    //        {
                    //            //Create Precursor m/z
                    //            for (int j = t.Charge; j <= _scan.ParentCharge; j++)
                    //            {
                    //                float  PrecusorMZ= (_PrecusorMono + Atoms.ProtonMass * j ) / (float)j;
                    //                float GlycoPeptidMZ = (t.GlycanAVGMonoMass + PeptideMonoMass + NextGlycan.AVGMass + Atoms.ProtonMass * j) / (float)j;

                    //                if (Math.Abs(PrecusorMZ-GlycoPeptidMZ) <= _MS2torelance)
                    //                {
                    //                    List<GlycanStructure> AddedTrees = AddGlycanToNGlycanTree(t, NextGlycan, _PrecusorMono, 0.0f);
                    //                    _FullSequencedGlycanStructure.AddRange(AddedTrees);
                    //                }
                    //            }
                    //        }
                    //    }
                    //    else //Precursor AVG Mono not found
                    //    {
                    //        if (MassUtility.GetMassPPM(t.GlycanAVGMonoMass + PeptideMonoMass + NextGlycan.AVGMass, PrecusorMonoMass) < _precursorTorelance*3)
                    //        {
                    //            List<GlycanStructure> AddedTrees = AddGlycanToNGlycanTree(t, NextGlycan, _PrecusorMono, 0.0f);
                    //            _FullSequencedGlycanStructure.AddRange(AddedTrees);
                    //            continue;
                    //        }
                    //        if (_createPrecursor)
                    //        {
                    //            //Create Precursor m/z
                    //            for (int j = t.Charge-1; j <= _scan.ParentCharge; j++)
                    //            {
                    //                float PrecusorMZ = (_PrecusorMono + Atoms.ProtonMass * j) / (float)j;
                    //                float GlycoPeptidMZ = (t.GlycanAVGMonoMass + PeptideMonoMass + NextGlycan.AVGMass + Atoms.ProtonMass * j) / (float)j;

                    //                if (Math.Abs(PrecusorMZ - GlycoPeptidMZ) <= _MS2torelance)
                    //                {
                    //                    List<GlycanStructure> AddedTrees = AddGlycanToNGlycanTree(t, NextGlycan, _PrecusorMono, 0.0f);
                    //                    _FullSequencedGlycanStructure.AddRange(AddedTrees);
                    //                }
                    //            }
                    //        }
                    //    }
                    //}
                    //else //non-AVG Mass
                    //{
                    //    if (MassUtility.GetMassPPM(t.GlycanMonoMass + PeptideMonoMass + NextGlycan.Mass, PrecusorMonoMass) < _precursorTorelance)
                    //    {
                    //        List<GlycanStructure> AddedTrees = AddGlycanToNGlycanTree(t, NextGlycan, _PrecusorMono, 0.0f);
                    //        _FullSequencedGlycanStructure.AddRange(AddedTrees);
                    //        continue;
                    //    }
                    //    if (_createPrecursor)
                    //    {
                    //        //Create Precursor m/z
                    //        for (int j = t.Charge; j <= _scan.ParentCharge; j++)
                    //        {
                    //            float PrecusorMZ = (_PrecusorMono + Atoms.ProtonMass * j) / (float)j;
                    //            float GlycoPeptidMZ = (t.GlycanMonoMass + PeptideMonoMass + NextGlycan.Mass + Atoms.ProtonMass * j) / (float)j;

                    //            if (Math.Abs(PrecusorMZ - GlycoPeptidMZ) <= _MS2torelance)
                    //            {
                    //                List<GlycanStructure> AddedTrees = AddGlycanToNGlycanTree(t, NextGlycan, _PrecusorMono, 0.0f);
                    //                _FullSequencedGlycanStructure.AddRange(AddedTrees);
                    //            }
                    //        }
                    //    }
                    //}
                }
            }

            //TO:Do
            //Use number of compostion to complete structure
            //if (_FullSequencedGlycanStructure.Count == 0 && _NumCompostionSet == true)
            //{               
            //    foreach (GlycanStructure GS in _SequencedGlycanStructure)
            //    {
            //        int LeftGlycan = 0;
            //        LeftGlycan = _NoHex - GS.NoOfHex;
            //        LeftGlycan = LeftGlycan + (_NoHexNac - GS.NoOfHexNac);
            //        LeftGlycan = LeftGlycan + (_NoDeHex - GS.NoOfDeHex);
            //        LeftGlycan = LeftGlycan + (_NoNeuAc - GS.NoOfNeuAc);
            //        LeftGlycan = LeftGlycan + (_NoNeuGc - GS.NoOfNeuGc);

            //        List<Glycan> ExtraGlycan = new List<Glycan>();
            //        if (LeftGlycan <= 2)
            //        {
            //            for (int i = 0; i < _NoHex - GS.NoOfHex; i++)
            //            {
            //                ExtraGlycan.Add(new Glycan(Glycan.Type.Hex,GS.Charge));
            //            }
            //            for (int i = 0; i <(_NoHexNac - GS.NoOfHexNac); i++)
            //            {
            //                ExtraGlycan.Add(new Glycan(Glycan.Type.HexNAc,GS.Charge));
            //            }
            //            for (int i = 0; i < (_NoDeHex - GS.NoOfDeHex); i++)
            //            {
            //                ExtraGlycan.Add(new Glycan(Glycan.Type.DeHex,GS.Charge));
            //            }
            //            for (int i = 0; i <(_NoNeuAc - GS.NoOfNeuAc); i++)
            //            {
            //                ExtraGlycan.Add(new Glycan(Glycan.Type.NeuAc,GS.Charge));
            //            }
            //            for (int i = 0; i <(_NoNeuGc - GS.NoOfNeuGc); i++)
            //            {
            //                ExtraGlycan.Add(new Glycan(Glycan.Type.NeuGc,GS.Charge));
            //            }
            //        }
            //    }
            //}

            if (_FullSequencedGlycanStructure.Count != 0)
            {
                List<GlycanStructure> tmpStructures = new List<GlycanStructure>();
                _FullSequencedGlycanStructure = CleanNLinkedStructure(_FullSequencedGlycanStructure);                               

                //Filter with mono mass
                foreach (GlycanStructure g in _FullSequencedGlycanStructure)
                {
                    if (MassUtility.GetMassPPM(g.GlycanMonoMass + MW.GetMonoMW(_peptideStr, true), _scan.ParentMonoMW) < _precursorTorelance)
                    {
                        tmpStructures.Add((GlycanStructure)g.Clone());
                        //Reware for complete structure;
                        if (_rewardForCompleteStructure != 0.0f)
                        {
                            tmpStructures[tmpStructures.Count - 1].Score = tmpStructures[tmpStructures.Count - 1].Score + _rewardForCompleteStructure;                   
                        }
                    }
                }
                _FullSequencedGlycanStructure = tmpStructures;

            }
            if (_isDebug)
            {
                DebugSW.Close();
            }
        }
        private List<GlycanStructure> CleanNLinkedStructure(List<GlycanStructure> argStructures)
        {
            //Remove duplicate
            List<String> _UniqueStructureIUPAC = new List<string>();
            List<GlycanStructure> UniqueTrees = new List<GlycanStructure>();
            foreach (GlycanStructure t in argStructures)
            {
                if (!_UniqueStructureIUPAC.Contains(t.IUPACString) && t.GlycanAVGMonoMass + _peptideMonoMass<= PrecusorMonoMass+10.0f)
                {
                    _UniqueStructureIUPAC.Add(t.IUPACString);
                    // t.UpdateDistance(-1);
                    UniqueTrees.Add(t);
                }
            }
            argStructures = UniqueTrees;

            List<GlycanStructure> PassStructureCheckStructure = new List<GlycanStructure>();
            foreach (GlycanStructure t in argStructures)
            {
                if (GlycanSequencing.IsStructureObeyAllRules(t, _structureRules) && HasNCore(t))
                {
                    PassStructureCheckStructure.Add(t);
                }
            }
            argStructures = PassStructureCheckStructure;

            return argStructures;
        }
        private bool HasNCore(GlycanStructure argStructure)
        {
            bool HasCore = false;
            if (argStructure.Root.GlycanType == Glycan.Type.HexNAc &&
                argStructure.Root.NoOfHexNacInChild == 1)  //Root
            {
                GlycanTreeNode TreeNode; //Glycan2
                if(argStructure.Root.SubTree1.GlycanType == Glycan.Type.HexNAc)
                {
                    TreeNode= argStructure.Root.SubTree1;
                }
                else
                {
                     TreeNode= argStructure.Root.SubTree2;
                }
                if (TreeNode.NoOfHexInChild == 1 &&
                    TreeNode.SubTree1.NoOfHexInChild == 2)
                {
                        HasCore = true;                    
                }
            }
            return HasCore;
        }
        private List<GlycanStructure> CleanOLinkedStructure(List<GlycanStructure> argStructures)
        {
            //Remove duplicate
            List<String> _UniqueStructureIUPAC = new List<string>();
            List<GlycanStructure> UniqueTrees = new List<GlycanStructure>();
            foreach (GlycanStructure t in argStructures)
            {
                if (!_UniqueStructureIUPAC.Contains(t.IUPACString) && t.GlycanAVGMonoMass + _peptideMonoMass <= PrecusorMonoMass + 10.0f)
                {
                    _UniqueStructureIUPAC.Add(t.IUPACString);
                    // t.UpdateDistance(-1);
                    UniqueTrees.Add(t);
                }
            }
            argStructures = UniqueTrees;

            return argStructures;
        }
       /* private void FindStructure()
        {
            //#region Find Structure 
            double TreeLeftMass = 0.0;

            DebugSW = null; //For debug
            if (_isDebug)
            {
                DebugSW = new StreamWriter(_debugFolderPath + "\\StructureListByMass_" + _scan.ScanNo + ".txt");
            }

            _SequencedGlycanStructure.Add(new GlycanStructure(new Glycan(Glycan.Type.HexNAc, _y1charge), _y1mass));
            foreach (MSPoint pk in _potentialPeaks)
            {
                if (_isDebug)
                {
                    DebugSW.WriteLine("-------" + pk.Mass.ToString() + "---------");
                }
                for (int GlycanIdx = 0; GlycanIdx < _glycanBuildingblock.Count; GlycanIdx++)
                {

                    Glycan glycan = _glycanBuildingblock[GlycanIdx];
                    if (MassUtility.GetMassPPM(_y1mass + glycan.Mz, pk.Mass) < _MS2torelance) // One monosaccharide match
                    {
                        GlycanStructure newGT = (GlycanStructure)_SequencedGlycanStructure[0].Clone();
                        //newGT.SubTree1 = new GlycanTree(glycan);
                        newGT.AddGlycanToStructure(new GlycanTreeNode(glycan.GlycanType, newGT.NextID), newGT.Root.NodeID);
                        newGT.GetGlycanTreeByID(newGT.NextID-1).IDPeak(pk.Mass, pk.Intensity);

                        if (newGT.Score > 0)
                        {
                            if (!_SequencedGlycanStructure.Contains(newGT))//(delegate(GlycanTree t1) { return (t1.GetIUPACString() == newGT.GetIUPACString() && t1.Charge); }))
                            {
                                if (_isDebug == true)
                                {
                                    DebugSW.WriteLine((_SequencedGlycanStructure.Count).ToString() + "-" + newGT.Charge.ToString() + "-" + newGT.IUPACString);
                                }

                                if (_NGlycanData && newGT.isObyeNLinkedCore())
                                {
                                    _SequencedGlycanStructure.Add(newGT);
                                }
                                else if (_NGlycanData == false)
                                {
                                    _SequencedGlycanStructure.Add(newGT);
                                }
                                // _SequencedGlycanStructure.Sort(delegate(GlycanTree t1, GlycanTree t2) { return Comparer<float>.Default.Compare(t2.TreeMass, t1.TreeMass); });
                            }
                            else
                            {
                                GlycanStructure ExistTree = _SequencedGlycanStructure.Find(delegate(GlycanStructure t1) { return t1.IUPACString == newGT.IUPACString; });
                                if (ExistTree.Score < newGT.Score)
                                {
                                    if (_isDebug == true)
                                    {
                                        DebugSW.WriteLine(newGT.IUPACString);
                                    }
                                    _SequencedGlycanStructure.Remove(ExistTree);

                                    if (_NGlycanData && newGT.isObyeNLinkedCore())
                                    {
                                        _SequencedGlycanStructure.Add(newGT);
                                    }
                                    else if (_NGlycanData == false)
                                    {
                                        _SequencedGlycanStructure.Add(newGT);
                                    }
                                }
                            }

                        }
                        #region Add Missing Peak Structure

                        for (int NextGlycanIdx = 0; NextGlycanIdx < _glycanBuildingblock.Count; NextGlycanIdx++)
                        {
                            Glycan NextGlycan = _glycanBuildingblock[NextGlycanIdx];
                            float nextpeak = _y1mass + glycan.Mz + NextGlycan.Mz;
                            if (MassUtility.GetMassPPM(nextpeak, FindClosedPeakIdx(nextpeak)) <= _MS2torelance) //Peak found and will add in next round
                            {
                                continue;
                            }
                            else
                            {
                                if (NextGlycan.GlycanType == Glycan.Type.DeHex && newGT.Root.GlycanType != Glycan.Type.HexNAc)  //Fuc only connect to HexNAc
                                {
                                    continue;
                                }
                                float targetMz = newGT.GlycanMZ + NextGlycan.Mz + _peptideMz;
                                float targetIntensity = 0.0f;
                                if (MassUtility.GetMassPPM(_scan.MSPeaks[MassUtility.GetClosestMassIdx(_scan.MSPeaks, targetMz)].MonoMass, targetMz) <= _torelance)
                                {
                                    targetIntensity = _scan.MSPeaks[MassUtility.GetClosestMassIdx(_scan.MSPeaks, targetMz)].MonoIntensity / MaxIntensityInScan;
                                }



                                //List<GlycanTree> AddedTrees = (AddGlycanToGlycanTree(newGT, NextGlycan, targetMz, targetIntensity)); //Add all posibble structures into candidate list (allow missing peak
                                List<GlycanStructure> AddedTrees = (AddGlycanToGlycanTree(newGT, NextGlycan, targetMz, targetIntensity)); //Add all posibble structures into candidate list (allow missing peak
                                foreach (GlycanStructure iterGT in AddedTrees)
                                {
                                    if (!_SequencedGlycanStructure.Contains(iterGT))
                                    {
                                        if (_NGlycanData && iterGT.isObyeNLinkedCore())
                                        {
                                            _SequencedGlycanStructure.Add(iterGT);
                                        }
                                        else if (_NGlycanData == false)
                                        {
                                            _SequencedGlycanStructure.Add(iterGT);
                                        }
                                        if (_isDebug == true)
                                        {
                                            DebugSW.WriteLine((_SequencedGlycanStructure.Count).ToString() + "-Missed-" + iterGT.IUPACString + ";" + iterGT.NoOfHex + "," + iterGT.NoOfHexNac + "," + iterGT.NoOfDeHex + "," + iterGT.NoOfNeuAc);
                                        }
                                    }
                                }
                            }
                        }
                        #endregion
                        continue;
                    }

                    TreeLeftMass = pk.Mass - _y1mass - glycan.Mz;

                    if (TreeLeftMass < 0)
                    {
                        continue;
                    }

                    for (int i = 0; i < _SequencedGlycanStructure.Count; i++)
                    {
                        GlycanStructure GT = _SequencedGlycanStructure[i];
                        if (MassUtility.GetMassPPM(GT.GlycanMZ + glycan.Mz + _y1mass - GlycanMass.GetGlycanMasswithCharge(Glycan.Type.HexNAc, _y1charge), pk.Mass) > _torelance || GT.Charge != glycan.Charge)
                        {
                            continue;
                        }
                        else //Add One monosaccharide to Tree
                        {
                            if (glycan.GlycanType == Glycan.Type.DeHex && GT.Root.GlycanType != Glycan.Type.HexNAc) // DeHex only connect to HexNac
                            {
                                continue;
                            }
                            List<GlycanStructure> AddedTrees = (AddGlycanToGlycanTree(GT, glycan, pk.Mass, pk.Intensity));
                            foreach (GlycanStructure iterGT in AddedTrees)
                            {
                                if (!_SequencedGlycanStructure.Contains(iterGT))
                                {

                                    if (_NGlycanData && iterGT.isObyeNLinkedCore())
                                    {
                                        _SequencedGlycanStructure.Add(iterGT);
                                    }
                                    else if (_NGlycanData == false)
                                    {
                                        _SequencedGlycanStructure.Add(iterGT);
                                    }
                                    if (_isDebug == true)
                                    {
                                        DebugSW.WriteLine((_SequencedGlycanStructure.Count).ToString() + "-*" + iterGT.Charge.ToString() + "-" + iterGT.IUPACString + ";" + iterGT.NoOfHex + "," + iterGT.NoOfHexNac + "," + iterGT.NoOfDeHex + "," + iterGT.NoOfNeuAc);
                                    }

                                }
                            }
                        }
                    }
                }
            }

            //Extra structure check
            List<GlycanStructure> NewlyAdded = new List<GlycanStructure>();
            float ParentMonoMW = _scan.ParentMonoMW;
            foreach (GlycanStructure gt in _SequencedGlycanStructure)
            {
                if (gt.Root.SubTree1 == null || gt.Root.SubTree1.SubTree1 == null)
                {
                    continue;
                }
                GlycanTreeNode BrancePoint = gt.Root.SubTree1.SubTree1;

                //Check symatric structure
                if (BrancePoint.SubTree1 != null && BrancePoint.SubTree2 == null)
                {
                    GlycanStructure NewTree = (GlycanStructure)gt.Clone();
                    //NewTree.Root.SubTree1.SubTree1.SubTree2 = (GlycanTree)BrancePoint.SubTree1.Clone();
                    NewTree.AddGlycanToStructure((GlycanTreeNode)BrancePoint.SubTree1.Clone(), NewTree.Root.SubTree1.SubTree1.NodeID);

                    if (NewTree.GlycanMonoMass + _peptideMonoMass < ParentMonoMW)
                    {
                        if (_NGlycanData && !NewTree.isObyeNLinkedCore())
                        {
                            continue;
                        }
                        NewlyAdded.Add(NewTree);
                    }
                }
                else if (BrancePoint.SubTree1 != null && BrancePoint.SubTree2 != null && BrancePoint.SubTree1.NoOfTotalGlycan > 1 && BrancePoint.SubTree2.NoOfTotalGlycan == 1)
                {
                    GlycanStructure NewTree = (GlycanStructure)gt.Clone();
                   // NewTree.Root.SubTree1.SubTree1.SubTree2 = (GlycanTree)gt.Root.SubTree1.SubTree1.SubTree1.Clone();
                    NewTree.AddGlycanToStructure((GlycanTreeNode)gt.Root.SubTree1.SubTree1.SubTree1.Clone(), NewTree.Root.SubTree1.SubTree1.NodeID);
                    if (NewTree.GlycanMonoMass + _peptideMonoMass < ParentMonoMW)
                    {
                        if (_NGlycanData && !NewTree.isObyeNLinkedCore())
                        {
                            continue;
                        }
                        NewlyAdded.Add(NewTree);
                    }
                }
            }
            foreach (GlycanStructure t in NewlyAdded)
            {
                _SequencedGlycanStructure.Add(t);
            }

            NewlyAdded.Clear();

            //Use parent mass to complete the structure
            foreach (GlycanStructure gt in _SequencedGlycanStructure)
            {
                //Miss last one glycan
                for (int i = 0; i < _glycanBuildingblock.Count; i++)
                {
                    Glycan NextGlycan = new Glycan(_glycanBuildingblock[i].GlycanType, _y1charge);
                    if (MassUtility.GetMassPPM(gt.GlycanMonoMass + GlycanMass.GetGlycanMasswithCharge(NextGlycan.GlycanType, 1), _glycanMonoMass) <= _torelance)
                    {
                        //Find Peaks
                        float TargetMz = ((gt.GlycanMonoMass + _peptideMonoMass + GlycanMass.GetGlycanMasswithCharge(NextGlycan.GlycanType, 1)) + _y1charge * Atoms.ProtonMass) / (float)_y1charge;
                        int PeakIdx = MassUtility.GetClosestMassIdx( _scan.MSPeaks,TargetMz);
                        float ParentMz = (ParentMonoMW + _y1charge * Atoms.ProtonMass) / (float)_y1charge;
                        float TargetIntensity = 0.0f;
                        if (MassUtility.GetMassPPM(_scan.MSPeaks[PeakIdx].MonoMass, ParentMz) <= _torelance)
                        {
                            TargetIntensity = _scan.MSPeaks[PeakIdx].MonoIntensity;
                        }

                        List<GlycanStructure> AddedTrees = (AddGlycanToGlycanTree(gt, NextGlycan, TargetMz, TargetIntensity)); //Add all posibble structures into candidate list (allow missing peak)
                        foreach (GlycanStructure iterGT in AddedTrees)
                        {
                            if (!_SequencedGlycanStructure.Contains(iterGT))
                            {
                                if (_NGlycanData && !iterGT.isObyeNLinkedCore() && !iterGT.HasNGlycanCore())
                                {
                                    continue;
                                }
                                NewlyAdded.Add(iterGT);
                            }
                        }
                    }
                }

                //Check symatric structure  for 3rd node (HexNac-HexNac-Hex-Hex)
                if (gt.Root.SubTree1 != null && gt.Root.SubTree1.SubTree1 != null && gt.Root.SubTree1.SubTree1.SubTree1 != null && gt.Root.SubTree1.SubTree1.SubTree2 == null)
                {
                    GlycanStructure NewTree = (GlycanStructure)gt.Clone();
                    //NewTree.SubTree1.SubTree1.SubTree2 = (GlycanTree)gt.Root.SubTree1.SubTree1.SubTree1.Clone();
                    NewTree.AddGlycanToStructure((GlycanTreeNode)gt.Root.SubTree1.SubTree1.SubTree1.Clone(), NewTree.Root.SubTree1.SubTree1.NodeID);
                    if (Math.Abs(MassUtility.GetMassPPM(NewTree.GlycanMonoMass, _glycanMonoMass)) <= _torelance)
                    {
                        NewlyAdded.Add(NewTree);
                    }
                }
                //Check symatric structure  for 4th node  (HexNac-HexNac-Hex-Hex(-Hex)-Hex)
                if (gt.Root.SubTree1 != null && gt.Root.SubTree1.SubTree1 != null && gt.Root.SubTree1.SubTree1.SubTree1 != null && gt.Root.SubTree1.SubTree1.SubTree2 != null
                    && gt.Root.SubTree1.SubTree1.SubTree1.SubTree1 != null && gt.Root.SubTree1.SubTree1.SubTree2.SubTree1 == null)
                {
                    GlycanStructure NewTree = (GlycanStructure)gt.Clone();
                    //NewTree.SubTree1.SubTree1.SubTree2.SubTree1 = (GlycanTree)gt.SubTree1.SubTree1.SubTree1.SubTree1.Clone();
                    NewTree.AddGlycanToStructure((GlycanTreeNode)gt.Root.SubTree1.SubTree1.SubTree1.SubTree1.Clone(), NewTree.Root.SubTree1.SubTree1.SubTree2.NodeID);
                    if (Math.Abs(MassUtility.GetMassPPM(NewTree.GlycanMonoMass, _glycanMonoMass)) <= _torelance)
                    {
                        NewlyAdded.Add(NewTree);
                    }
                }
            }

            foreach (GlycanStructure t in NewlyAdded)
            {
                _SequencedGlycanStructure.Add(t);
            }



            //Remove duplicted structure

            List<String> _UniqueStructureIUPAC = new List<string>();
            List<GlycanStructure> UniqueTrees = new List<GlycanStructure>();
            foreach (GlycanStructure t in _SequencedGlycanStructure)
            {
                if (!_UniqueStructureIUPAC.Contains(t.IUPACString))
                {
                    _UniqueStructureIUPAC.Add(t.IUPACString);
                    // t.UpdateDistance(-1);
                    UniqueTrees.Add(t);
                }
            }
            _SequencedGlycanStructure = UniqueTrees;
            List<GlycanStructure> PassStructureCheckStructure = new List<GlycanStructure>();

            foreach (GlycanStructure t in _SequencedGlycanStructure)
            {
                if (GlycanSequencing.IsStructureObeyAllRules(t, _structureRules))
                {
                    PassStructureCheckStructure.Add(t);
                }
            }

            _SequencedGlycanStructure = PassStructureCheckStructure;

            if (_isDebug)
            {
                DebugSW.Close();
            }
        }
        */
        //public static float GetMassPPM(float argExactMass, float argMeasureMass)
        //{
        //    return Math.Abs(Convert.ToSingle(((argMeasureMass - argExactMass) / argExactMass) * Math.Pow(10.0, 6.0)));
        //}
        private static bool IsStructureObeyAllRules(GlycanStructure argStructure, List<StructureRule> argRules)
        {
            bool IsObey = true;
            foreach (StructureRule SR in argRules)
            {
                List<string> Locations = argStructure.Root.Contain(SR.Structure);
                if (SR.TypeOfRule == StructureRule.FiltereTypes.Required) //at least once
                {             
                    bool found = false;
                    if (Locations.Count < 1)
                    {
                        return false;
                    }                    
                    foreach(string loc in Locations)
                    {                       
                        if (StructureLocationFound(SR.DistanceOperator,SR.DistanceToRoot,loc))
                        {
                            found = true;
                        }
                    }
                    if (!found)
                    {
                        IsObey = false;
                    }
                }
                else if (SR.TypeOfRule == StructureRule.FiltereTypes.Denied)
                {                    
                    if (Locations.Count > 1)
                    {
                        if (SR.DistanceOperator == ">")
                        {
                            foreach (string s in Locations)
                            {
                                if (Convert.ToInt32(s.Split('-')[0]) > SR.DistanceToRoot)
                                {
                                    IsObey = false;
                                }
                            }
                        }
                        else if (SR.DistanceOperator == "<")
                        {
                             foreach (string s in Locations)
                            {
                                if (Convert.ToInt32(s.Split('-')[0]) < SR.DistanceToRoot)
                                {
                                    IsObey = false;
                                }
                            }
                        }
                    }
                }
            }
            return IsObey;
        }
        private static bool StructureLocationFound(string argOperator, int argDistanceToRoot, string NodeID)
        {
            int StructureRoot = Convert.ToInt32( NodeID.Split('-')[0]);
            bool found = false;
            if (argOperator == "=")
            {
                if (argDistanceToRoot == StructureRoot)
                {
                    found = true;
                }
            }
            else if (argOperator == "<")
            {
                if (StructureRoot < argDistanceToRoot)
                {
                    found = true;
                }
            }
            else if (argOperator == ">")
            {
                if (StructureRoot > argDistanceToRoot)
                {
                    found = true;
                }
            }
            return found;
        }
        //private void ReScoreGlycanTree(GlycanStructure argTree)
        //{
        //    List<GlycanTreeNode> fragments = argTree.TheoreticalFragment;
        //    foreach (GlycanTreeNode fragment in fragments)
        //    {
        //        //Find Peak
        //        int PeakIdx = MassUtility.GetClosestMassIdx( _scan.MSPeaks,GlycanMass.GetGlycanMasswithCharge( fragment.GlycanType,argTree.Charge));
        //        if (MassUtility.GetMassPPM(_scan.MSPeaks[PeakIdx].MonoMass, GlycanMass.GetGlycanMasswithCharge(fragment.GlycanType, argTree.Charge)) <= _torelance) //Peak Found
        //        {
        //            fragment.IDPeak(_scan.MSPeaks[PeakIdx].MonoMass, _scan.MSPeaks[PeakIdx].MonoIntensity);
        //        }
        //        else
        //        {
        //            fragment.IDPeak(_scan.MSPeaks[PeakIdx].MonoMass, 0.0f);
        //        }
        //    }
        //}

        private List<GlycanStructure> AddGlycanToOGlycanTree(GlycanStructure argParentTree, Glycan argGlycan, float argMZ, float argIntensity)
        {
            List<GlycanStructure> addedGT = new List<GlycanStructure>();
            List<GlycanTreeNode> AllGlycanNodeInParent = argParentTree.Root.FetchAllGlycanNode();
            List<int> AddPosition = new List<int>(); //AddPosition using GlycanNodeID start with 1;

            #region DeHex
            if (argGlycan.GlycanType == Glycan.Type.DeHex) // DeHex can place at the 1st core HexNac, and Non-reducing end HexNac  [RULE 8]
            {
                foreach (GlycanTreeNode T in AllGlycanNodeInParent)
                {
                    if (T.GlycanType == Glycan.Type.HexNAc && (T.Subtrees == null || T.Subtrees.Count < 4) && T.NoOfDeHex == 0)
                    {
                        AddPosition.Add(T.NodeID);
                    }
                }
            }
            #endregion 
            #region HexNAc
            else if (argGlycan.GlycanType == Glycan.Type.HexNAc)
            {
                    foreach (GlycanTreeNode T in AllGlycanNodeInParent)
                    {
                        if (T.Subtrees==null || T.Subtrees.Count<4)
                        {
                                AddPosition.Add(T.NodeID);
                        }                    
                    }  
            }
            #endregion HexNAc
            #region Hex
            else if (argGlycan.GlycanType == Glycan.Type.Hex)
            {
                    foreach (GlycanTreeNode T in AllGlycanNodeInParent)
                    {
                        if (T.Subtrees == null || T.Subtrees.Count < 4)
                        {
                             AddPosition.Add(T.NodeID);                            
                        }
                    }              
            }
            #endregion Hex
            #region NeuAc
            else //NeuAc,NeuGc
            {
                foreach (GlycanTreeNode T in AllGlycanNodeInParent) //Sialic acid only attach to terminal Hex(Gal) [RULE 6]
                {
                    if (T.Subtrees == null || T.Subtrees.Count < 4)
                    {                        
                        AddPosition.Add(T.NodeID);                        
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
                    (ToBeAddGlycanTree.Parent.Subtrees == null || ToBeAddGlycanTree.Parent.Subtrees.Count < 4))  //DeHex NeuAc NeuGc can't have child
                {
                    newTree.AddGlycanToStructure(ToBeAddGlycanTree, ToBeAddGlycanTree.Parent.NodeID);
                    newTree.Root.SortSubTree();
                    addedGT.Add(newTree);
                }
            }
            return addedGT;
        }
        private List<GlycanStructure> AddGlycanToNGlycanTree(GlycanStructure argParentTree, Glycan argGlycan, float argMZ, float argIntensity)
        {
            List<GlycanStructure> addedGT = new List<GlycanStructure>();
            List<GlycanTreeNode> AllGlycanNodeInParent = argParentTree.Root.FetchAllGlycanNode();
            List<int> AddPosition = new List<int>(); //AddPosition using GlycanNodeID start with 1;

            if (argParentTree.IUPACString == "NeuAc-Hex-HexNAc-Hex-(Hex-Hex-)Hex-HexNAc-HexNAc" && argGlycan.GlycanType == Glycan.Type.HexNAc)
            {
            }
            #region DeHex
            if (argGlycan.GlycanType == Glycan.Type.DeHex) // DeHex can place at the 1st core HexNac, and Non-reducing end HexNac  [RULE 8]
            {
                foreach (GlycanTreeNode T in AllGlycanNodeInParent)
                {
                    if (T.GlycanType == Glycan.Type.HexNAc && T.DistanceRoot != 3 && (T.Subtrees == null || T.Subtrees.Count < 4) &&T.DistanceRoot!=1 && T.NoOfDeHex == 0)
                    {
                        AddPosition.Add(T.NodeID);                        
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
                            if ((T.DistanceRoot == 2 || T.DistanceRoot == 3) && T.GlycanType == Glycan.Type.Hex)
                            {
                                AddPosition.Add(T.NodeID);
                            }
                        }
                    }
                     else if (NoOfHexRemain > NoOfHexNACRemain)//  Hybrid Structure[RULE 9]  HexNAC-Hex reapeat [RULE 10]
                    {
                        foreach (GlycanTreeNode T in AllGlycanNodeInParent)
                        {                            
                            if (T.DistanceRoot == 2 && T.NoOfHexNacInChild == 0) 
                            {
                                AddPosition.Add(T.NodeID); //bisecting
                            }
                            else //Add to branch (none high mannos branch)
                            {
                                if (T.DistanceRoot>=3 && T.DistanceRoot%2 ==1 && T.GlycanType == Glycan.Type.Hex && T.Subtrees==null  )
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
                            if (T.DistanceRoot >=3  && T.GlycanType == Glycan.Type.Hex)
                            {
                                if (T.Subtrees == null && T.Parent.GlycanType!= Glycan.Type.Hex)
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
                            if (T.DistanceRoot == 2 && T.NoOfHexNacInChild == 0)
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
                                if (T.NoOfHexNacInChild== 0)
                                {
                                    AddPosition.Add(T.NodeID); //Add to First HexNac; HexNAc-(DeHex)-HexNAc
                                }
                            }
                        }
                        if (T.DistanceRoot == 2 && T.NoOfHexNacInChild == 0) //Bisecting 
                        {
                            AddPosition.Add(T.NodeID);
                        }
                        if (T.DistanceRoot == 3 && T.GlycanType == Glycan.Type.Hex && (T.Subtrees==null || T.Subtrees.Count<=1)) //Branching 
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
                    if(NoOfHexNACRemain ==0)
                    {
                        //AttachToAntenna [RULE 3]
                        foreach (GlycanTreeNode T in AllGlycanNodeInParent)
                        {
                            if (T.GlycanType == Glycan.Type.Hex && T.Subtrees == null && T.DistanceRoot>=3)  //Add to leaf Hex
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
                            if  ((T.DistanceRoot>3 && T.Subtrees == null && T.GlycanType == Glycan.Type.Hex &&T.Parent.GlycanType != Glycan.Type.HexNAc) || //High Man
                                (T.DistanceRoot > 3 && T.Subtrees == null && T.GlycanType == Glycan.Type.HexNAc) ||//Add to leaf Hex //Complex
                                (T.DistanceRoot ==3 && T.GlycanType == Glycan.Type.Hex && (T.Subtrees ==null || T.Subtrees.Count<=1) )                               
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
                            if ((T.DistanceRoot == 2 && T.GlycanType == Glycan.Type.Hex && T.NoOfHexInChild <=1 )|| //core
                                (T.DistanceRoot ==3 && T.GlycanType  == Glycan.Type.Hex && (T.Subtrees == null ||T.Subtrees.Count == 1)))
                            {
                                AddPosition.Add(T.NodeID);
                            }
                            else if (T.DistanceRoot > 3 && T.GlycanType == Glycan.Type.HexNAc && T.Subtrees==null)
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
                    (ToBeAddGlycanTree.Parent.Subtrees == null || ToBeAddGlycanTree.Parent.Subtrees.Count<4 ))
                {                    
                    newTree.AddGlycanToStructure(ToBeAddGlycanTree, ToBeAddGlycanTree.Parent.NodeID);
                    newTree.Root.SortSubTree();
                    addedGT.Add(newTree);
                }
            }
            return addedGT;
        }
        /// <summary>
        /// Add glycan to possible place
        /// </summary>
        /// <param name="argParentTree">Tree need to be added</param>
        /// <param name="argGlycan">Glycan for add</param>
        /// <param name="argMZ">m/z for structure </param>
        /// <param name="argIntensity">intensity for structure</param>
        /// <returns></returns>
        private List<GlycanStructure> AddGlycanToGlycanTree(GlycanStructure argParentTree, Glycan argGlycan, float argMZ, float argIntensity)
        {
            List<GlycanStructure> addedGT = new List<GlycanStructure>();
            List<GlycanTreeNode> AllGlycanNodeInParent = argParentTree.Root.FetchAllGlycanNode();
            List<int> AddPosition = new List<int>();

            for (int i = 0; i < AllGlycanNodeInParent.Count; i++)
            {
                GlycanTreeNode GT = AllGlycanNodeInParent[i];
                if (GT.SubTree1 == null) //leaf
                {
                    AddPosition.Add(i);
                    //find parent
                    if (i >= 1)
                    {
                        //Can Branch
                        if (AllGlycanNodeInParent[i - 1].SubTree2 == null)
                        {
                            AddPosition.Add(i - 1);
                        }
                    }
                }
            }

            for (int i = 0; i < AddPosition.Count; i++)
            {
                int AddedIndex = AddPosition[i];
                GlycanStructure newTree = (GlycanStructure)argParentTree.Clone();
                List<GlycanTreeNode> tmpPreAddGTs = newTree.Root.FetchAllGlycanNode();
                GlycanTreeNode ToBeAddGlycanTree = new GlycanTreeNode(argGlycan.GlycanType, newTree.NextID);
                ToBeAddGlycanTree.IDPeak(argMZ, argIntensity);
                ToBeAddGlycanTree.Parent = tmpPreAddGTs[AddedIndex];
                if (tmpPreAddGTs[AddedIndex].GlycanType != Glycan.Type.HexNAc && ToBeAddGlycanTree.GlycanType == Glycan.Type.DeHex)
                {
                    continue;
                }

                if (tmpPreAddGTs[AddedIndex].GlycanType != Glycan.Type.DeHex &&
                    tmpPreAddGTs[AddedIndex].GlycanType != Glycan.Type.NeuAc &&
                    tmpPreAddGTs[AddedIndex].GlycanType != Glycan.Type.NeuGc &&
                    tmpPreAddGTs[AddedIndex].SubTree4 == null)
                {
                    //tmpPreAddGTs[AddedIndex].AddGlycanSubTree(ToBeAddGlycanTree, argGT.NextID);
                    newTree.AddGlycanToStructure(ToBeAddGlycanTree, tmpPreAddGTs[AddedIndex].NodeID);
                    //newTree.Root.UpdateGlycans();
                    newTree.Root.SortSubTree();
                    addedGT.Add(newTree);
                }
            }
            return addedGT;
        }
        
        public void ExporToFolder(string argFolder)
        {
            //GenerateHtmlReport.ExporToFolder(argFolder, this);
        }
       
    }
}
