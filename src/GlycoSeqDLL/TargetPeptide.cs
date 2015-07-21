﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using COL.MassLib;
using COL.ProtLib;
namespace COL.GlycoSequence
{
    public class TargetPeptide
    {
        private string _PeptideSeq;
        private float _StartTime = -1; //in Mins 
        private float _EndTime = 9999; //in Mins
        private string _ProteinName;
        private float _PeptideMass;
        private string _AminoAcidAfter;
        private string _AminoAcidBefore;
        public string PeptideSequence
        {
            get { return _PeptideSeq; }
            set { _PeptideSeq = value; }
        }
        public string AminoAcidAfter
        {
            get { return _AminoAcidAfter; }
            set { _AminoAcidAfter = value; }
        }
        public string AminoAcidBefore
        {
            get { return _AminoAcidBefore; }
            set { _AminoAcidBefore = value; }
        }
        public float StartTime
        {
            get { return _StartTime; }
            set { _StartTime = value; }
        }

        public float EndTime
        {
            get { return _EndTime; }
            set { _EndTime = value; }
        }

        public string ProteinName
        {
            get { return _ProteinName; }
            set { _ProteinName = value; }
        }

        public float PeptideMass
        {
            get
            {
                if (_PeptideMass == 0)
                {
                    AminoAcidMass AAMass = new AminoAcidMass();
                    _PeptideMass = AAMass.GetMonoMW(_PeptideSeq, true);
                }
                return _PeptideMass;
            }
            set { _PeptideMass = value; }
        }
   
        public TargetPeptide(string argPeptide) : this(argPeptide, "No_data", 0,-1, 9999){}
        public TargetPeptide(string argPeptide, string argProteinName, float argStartTime, float argEndTime) : this(argPeptide, argProteinName, 0, -1, 9999) { }
        public TargetPeptide(string argPeptide,string argProteinName, float argPeptideMass,float argStartTime,float argEndTime)
        {
            _PeptideSeq = argPeptide;
            _ProteinName = argProteinName;
            if (argPeptideMass == 0)
            {
                AminoAcidMass  AAMS = new AminoAcidMass();
                _PeptideMass = AAMS.GetMonoMW(_PeptideSeq, true);
            }
            else
            {
                _PeptideMass = argPeptideMass;    
            }
            _StartTime = argStartTime;
            _EndTime = argEndTime;
        }
    }
}
