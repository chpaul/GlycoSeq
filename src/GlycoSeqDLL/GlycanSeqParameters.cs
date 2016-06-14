using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using COL.ProtLib;
using CSMSL.IO;
using COL.GlycoLib;
namespace COL.GlycoSequence
{
    public class GlycanSeqParameters
    {
       
        // private float _CompletedReward;
        public bool IsHuman { get; set; }
        public int NoHexNAc { get; set; }
        public int NoHex { get; set; }
        public int NoDeHex { get; set; }
        public int NoSia { get; set; }
        public List<Protease.Type> ProteaseType { get; set; }
        public bool AverageMass { get; set; }
        public bool UseGlycanList { get; set; }
        public string ExportFolder { get; set; }
        public string FastaFile { get; set; }
        public string GlycanFile { get; set; }
        public int GetTopRank { get; set; }
        public bool UseHCD { get; set; }
        public bool SeqHCD { get; set; }
        public bool CompletedOnly { get; set; }
        public enumPeptideMutation PeptideMutation { get; set; }
        public string RawFilePath { get; set; }
        public List<TargetPeptide> TargetPeptides { get; set; }
        public bool UnknownPeptideSearch { get; set; }
        public bool PeptideFromMascotResult { get; set; }
        public int GetTopY1_i { get; set; }
        public int GetTopCore_k { get; set; }
        public int GetBranch_l { get; set; }
        public int MaxPanddingGlycan { get; set; }
        public int MissedCleavages { get; set; }
        public float MascotTimeShift { get; set; }
        public float MascotTimeTolerance { get; set; }
        public List<int> PeaksParameters { get; set; }

        private int _StartScan = 0;
        private int _EndScan = 0;
        private float _MSMSTol = 0.0f;
        private float _PrecursorTol = 0.0f;
        private bool _NGlycan = true;
        private bool _ExportIndividualSpectrum = false;

        private float _scoreAlpha = 1.0f;
        private float _scoreBeta = 1.0f;

        public float ScoreAlpha
        {
            get { return _scoreAlpha; }
            set { _scoreAlpha = value; }
        }

        public float ScoreBeta
        {
            get {return _scoreBeta;}
            set { _scoreBeta = value; }
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
        public bool IsNGlycan
        {
            get { return _NGlycan; }
            set { _NGlycan = value; }
        }
        public bool ExportIndividualSpectrum
        {
            get { return _ExportIndividualSpectrum; }
            set { _ExportIndividualSpectrum = value; }
        }
        public string Enzyme
        {
            get
            {
                string tmpEzme = "";
                foreach (Protease.Type p in ProteaseType)
                {
                    tmpEzme += p.ToString() + ";";
                }
                return tmpEzme;
            }
        }
        public GlycanSeqParameters()
        {
            PeaksParameters = new List<int>();
        }

      
        //public float CompletedReward
        //{
        //    get { return _CompletedReward; }
        //    set { _CompletedReward = value; }
        //}
        
    }
}
