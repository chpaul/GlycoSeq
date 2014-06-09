using System;
using System.Collections.Generic;
using System.Text;
using GlypID;
using COL.MassLib;
namespace COL.GlycoSequence
{
    public class HCDInfo
    {
        GlypID.enmGlycanType _gType;
        double _hcd_score;
        public HCDInfo(GlypID.Readers.clsRawData argReader, int argScanNo)
        {
            int ParentScan = argReader.GetParentScan(argScanNo);
            // Scorers and transforms
            GlypID.HCDScoring.clsHCDScoring HCDScoring = new GlypID.HCDScoring.clsHCDScoring();
            GlypID.HornTransform.clsHornTransform Transform = new GlypID.HornTransform.clsHornTransform();

            // mzs , intensities
            float[] hcd_mzs = null;
            float[] hcd_intensities = null;
            float[] parent_mzs = null;
            float[] parent_intensities = null;

            // Peaks
            GlypID.Peaks.clsPeak[] parent_peaks;
            GlypID.Peaks.clsPeak[] hcd_peaks;

            // Peak Processors 
            GlypID.Peaks.clsPeakProcessor hcdPeakProcessor = new GlypID.Peaks.clsPeakProcessor();
            GlypID.Peaks.clsPeakProcessor parentPeakProcessor = new GlypID.Peaks.clsPeakProcessor();

            // Results
            GlypID.HCDScoring.clsHCDScoringScanResults[] hcd_scoring_results;
            GlypID.HornTransform.clsHornTransformResults[] transform_results;

            // Params
            GlypID.Scoring.clsScoringParameters scoring_parameters = new GlypID.Scoring.clsScoringParameters();
            GlypID.HornTransform.clsHornTransformParameters transform_parameters = new GlypID.HornTransform.clsHornTransformParameters();
            scoring_parameters.MinNumPeaksToConsider = 2;
            scoring_parameters.PPMTolerance = 10;
            scoring_parameters.UsePPM = true;

            // Init
            Transform.TransformParameters = transform_parameters;

            // Loading parent
            int parent_scan = argReader.GetParentScan(argScanNo);
            double parent_mz = argReader.GetParentMz(argScanNo);
            int scan_level = argReader.GetMSLevel(argScanNo);
            int parent_level = argReader.GetMSLevel(parent_scan);
            argReader.GetSpectrum(parent_scan, ref parent_mzs, ref parent_intensities);


             // Parent processing
            parent_peaks = new GlypID.Peaks.clsPeak[1];
            parentPeakProcessor.ProfileType = GlypID.enmProfileType.PROFILE;
            parentPeakProcessor.DiscoverPeaks(ref parent_mzs, ref parent_intensities, ref parent_peaks,
                        Convert.ToSingle(transform_parameters.MinMZ), Convert.ToSingle(transform_parameters.MaxMZ), true);
            double bkg_intensity = parentPeakProcessor.GetBackgroundIntensity(ref parent_intensities);
            double min_peptide_intensity = bkg_intensity * transform_parameters.PeptideMinBackgroundRatio;
       

            transform_results = new GlypID.HornTransform.clsHornTransformResults[1];
            bool found = Transform.FindPrecursorTransform(Convert.ToSingle(bkg_intensity), Convert.ToSingle(min_peptide_intensity), ref parent_mzs, ref parent_intensities, ref parent_peaks, Convert.ToSingle(parent_mz), ref transform_results);
            if (!found && (argReader.GetMonoChargeFromHeader(ParentScan) > 0))
            {
                found = true;
                double mono_mz = argReader.GetMonoMzFromHeader(ParentScan);
                if (mono_mz == 0)
                    mono_mz = parent_mz;

                short[] charges = new short[1];
                charges[0] = argReader.GetMonoChargeFromHeader(ParentScan);
                Transform.AllocateValuesToTransform(Convert.ToSingle(mono_mz), 0, ref charges, ref transform_results); // Change abundance value from 0 to parent_intensity if you wish
            }
            if (found && transform_results.Length == 1)
            {
                // Score HCD scan first
                argReader.GetSpectrum(argScanNo, ref hcd_mzs, ref hcd_intensities);
                double hcd_background_intensity = GlypID.Utils.GetAverage(ref hcd_intensities, ref  hcd_mzs, Convert.ToSingle(scoring_parameters.MinHCDMz), Convert.ToSingle(scoring_parameters.MaxHCDMz));
                hcdPeakProcessor.SetPeakIntensityThreshold(hcd_background_intensity);
                hcd_peaks = new GlypID.Peaks.clsPeak[1];

                //Check Header
                string Header = argReader.GetScanDescription(argScanNo);
                hcdPeakProcessor.ProfileType = GlypID.enmProfileType.PROFILE;
                if (Header.Substring(Header.IndexOf("+") + 1).Trim().StartsWith("c"))
                {
                    hcdPeakProcessor.ProfileType = GlypID.enmProfileType.CENTROIDED;
                }
               
                hcdPeakProcessor.DiscoverPeaks(ref hcd_mzs, ref hcd_intensities, ref hcd_peaks, Convert.ToSingle
                    (scoring_parameters.MinHCDMz), Convert.ToSingle(scoring_parameters.MaxHCDMz), false);
                hcdPeakProcessor.InitializeUnprocessedData();

                hcd_scoring_results = new GlypID.HCDScoring.clsHCDScoringScanResults[1];

                HCDScoring.ScoringParameters = scoring_parameters;
                _hcd_score = HCDScoring.ScoreHCDSpectra(ref hcd_peaks, ref hcd_mzs, ref hcd_intensities, ref transform_results, ref hcd_scoring_results);
                 _gType = (GlypID.enmGlycanType)hcd_scoring_results[0].menm_glycan_type;
            }

        }
        public GlypID.enmGlycanType GlycanType
        {
            get { return _gType; }
        }
        public double HCDSCore
        {
            get { return _hcd_score; }
        }
    }
}
