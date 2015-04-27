using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COL.ProtLib;
using CSMSL.IO;

namespace COL.GlycoSequence
{
    public class GlycanSeqParameters
    {
 

        private string _rawFilePath;
        private List<TargetPeptide> _TargetPeptide;

        private int _StartScan = 0;
        private int _EndScan = 0;
        private float _MSMSTol = 0.0f;
        private float _PrecursorTol = 0.0f;
        private bool _NGlycan = true;
        private bool _Human;

        private int _NoHexNAc;
        private int _NoHex;
        private int _NoDeHex;
        private int _NoSia;
        private List<Protease.Type> _ProteaseType;
        private int _MissCLeavage;
        private bool _AverageMass;
        private bool _UseGlycanList;
        private string _exportFolder;
        private string _fastaFile;
        private string _glycanFile;
        private int _GetTopRank;
        private bool _UseHCD;
        private bool _SeqHCD;
        private bool _CompletedOnly;
        private float _CompletedReward;
        private int DividePartNum = 0;
        private enumPeptideMutation _PeptideMutation;
        private bool _ExportIndividualSpectrum = false;
        private List<int> _PeakParameters = new List<int>(); 

        public string RawFilePath
        {
            get { return _rawFilePath; }
            set { _rawFilePath = value; }
        }
        public List<TargetPeptide> TargetPeptides
        {
            get { return _TargetPeptide; }
            set { _TargetPeptide = value; }
        }
        public int StartScan
        {
            get { return _StartScan; }
            set { _StartScan = value; }
        }
        public int EndScan
        {
            get { return _EndScan; }
            set { _EndScan = value; }
        }
        public float MSMSTol
        {
            get { return _MSMSTol; }
            set { _MSMSTol = value; }
        }
        public float PrecursorTol
        {
            get { return _PrecursorTol; }
            set { _PrecursorTol = value; }
        }

        public List<int> PeaksParameters
        {
            get { return _PeakParameters; }
            set { _PeakParameters = value;} 
        }
        public bool IsNGlycan
        {
            get { return _NGlycan; }
            set { _NGlycan = value; }
        }

        public bool IsHuman
        {
            get { return _Human; }
            set { _Human = value; }
        }
        public int NoHexNAc
        {
            get { return _NoHexNAc; }
            set { _NoHexNAc = value; }
        }
        public int NoHex
        {
            get { return _NoHex; }
            set { _NoHex = value; }
        }
        public int NoDeHex
        {
            get { return _NoDeHex; }
            set { _NoDeHex = value; }
        }
        public int NoSia
        {
            get { return _NoSia; }
            set { _NoSia = value; }
        }

        public List<Protease.Type> ProteaseType
        {
            get { return _ProteaseType; }
            set { _ProteaseType = value; }
        }

        public int MissCLeavage
        {
            get { return _MissCLeavage; }
            set { _MissCLeavage = value; }
        }

        public bool AverageMass
        {
            get { return _AverageMass; }
            set { _AverageMass = value; }
        }

        public bool UseGlycanList
        {
            get { return _UseGlycanList; }
            set { _UseGlycanList = value; }
        }

        public string ExportFolder
        {
            get { return _exportFolder; }
            set { _exportFolder = value; }
        }

        public string FastaFile
        {
            get { return _fastaFile; }
            set { _fastaFile = value; }
        }

        public string GlycanFile
        {
            get { return _glycanFile; }
            set { _glycanFile = value; }
        }

        public int GetTopRank
        {
            get { return _GetTopRank; }
            set { _GetTopRank = value; }
        }

        public bool UseHCD
        {
            get { return _UseHCD; }
            set { _UseHCD = value; }
        }

        public bool SeqHCD
        {
            get { return _SeqHCD; }
            set { _SeqHCD = value; }
        }

        public bool CompletedOnly
        {
            get { return _CompletedOnly; }
            set { _CompletedOnly = value; }
        }

        public float CompletedReward
        {
            get { return _CompletedReward; }
            set { _CompletedReward = value; }
        }

        public enumPeptideMutation PeptideMutation
        {
            get { return _PeptideMutation; }
            set { _PeptideMutation = value; }
         }

        public bool ExportIndividualSpectrum
        {
            get { return _ExportIndividualSpectrum; }
            set { _ExportIndividualSpectrum = value; }
        }

        public GlycanSeqParameters()
        {
        }

    
    }
}
