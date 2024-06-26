using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using ArmBracketDesignLibrary.Helpers;
using ArmBracketDesignLibrary.StructureComponents;
using ArmBracketDesignLibrary.StructureComponents.Arms;

namespace ArmBracketDesignLibrary.BracketAnalysis
{
    public class BracketLoadCalcs
    {

        private readonly TubularArm _arm;

        private ThruPlateConnection _conn;

        private readonly SaddleBracket _bracket;

        private readonly Dictionary<string, List<ArmLoad>> _loadsAtPoleFace;

        private bool _isOverstressed = false;

        public BracketLoadCalcs(TubularArm arm, SaddleBracket bracket) //   ArmProject project)
        {
            //_project = project;
            //_arm = project.TubularDavitArm;
            ////_attach = _project.
            //_bracket = project.SaddleBracket; // (SaddleBracket)attachment;

            _arm = arm;
            _bracket = bracket;

            _conn = (ThruPlateConnection)_bracket.Parent;

            //_conn = (ThruPlateConnection)connection; // TODO Is this connection correct?

            // ** Get the arm connection design loads and translate the loads
            // ** from the face of the pole to the bracket bolt center line
            //_bracketLoads = GetArmConnectionDesignLoads(_arm, _conn, _attach, out _loadsAtPoleFace);
            BracketLoadItems = GetArmConnectionDesignLoads(_arm, _conn, _bracket, out _loadsAtPoleFace);
            //BracketLoadItems = GetArmConnectionDesignLoads(_bracket, out _loadsAtPoleFace);

            // ** Added this check to handle a problem we found with an arm that is to be attached
            // ** to the face of the pole, and attached by the middle of the arm, not the base.
            if (BracketLoadItems.Count == 0)
            {
                return;
            }

        }

        /// <summary>
        /// This bracket design works for the given loads (ie not overstressed and W/t within limits).
        /// Look in ControllingLoadCase to find out which one.
        /// </summary>
        public bool DesignWorked { get { return !_isOverstressed && BracketLoadItems.Count > 0; } }

        /// <summary>
        /// The controlling load case by BracketStressType.
        /// </summary>
        public Dictionary<BracketStressType, BracketControllingResult> BracketAnalysisResults { get; set; }

        /// <summary>
        /// List of unique controlling load cases for this bracket.
        /// </summary>
        public List<BracketLoad> ControllingLoadcaseList { get; set; }


        public Dictionary<string, List<BracketLoad>> BracketLoadItems { get; }

        public void DoBracketAnalysis()
        {
            if (BracketLoadItems.Count <= 0)
            {
                return;  // nothing to analyze
            }


            int lc = -1;
            // ** setup the bracket bolts for each load case:
            // **   bolt location, shear, and moment.
            foreach (var kvp in BracketLoadItems)
            {
                lc++;
                string key = kvp.Key;
                int i = -1;
                foreach (var bktLoad in kvp.Value)
                {
                    i++;
                    ArmLoad atPoleFace = _loadsAtPoleFace[key][i];
                    bktLoad.LoadsAtPoleFace = atPoleFace;

                    bktLoad.SetupBracketBolts();
                }
            }

            // *********************************************************************
            // ** determine the controlling load case for each bracket stress type.
            ControllingLCase();

            // ** Reference the bracket analysis Results in the bracket
            _bracket.BracketAnalysisLoads = this;

        }

        /// <summary>
        /// Get a list of load case results for all arms at this connection.
        /// </summary>
        /// <param name="conn">The connection under consideration.</param>
        /// <returns>Return a list of load case results for all arms at this connection.</returns>
        private Dictionary<string, List<ArmLoad>> AggregateResults(ArmConnection conn)
        {
            Dictionary<string, List<ArmLoad>> results = new Dictionary<string, List<ArmLoad>>();

            // !! TODO Need to define _project
            //ArmsProject _project = null; // conn.Parent.Parent.Parent;

            //foreach (var armId in conn.Parent.ArmIds)
            //{
            //TubularArm arm = _project.GetArm(armId.Value);
            //TubularArm arm = _project.TubularDavitArm;
            TubularArm arm = _arm;

            Dictionary<string, List<ArmLoad>> currentResults = arm.ArmLoads;

            foreach (string loadCase in currentResults.Keys)
            {
                if (!results.ContainsKey(loadCase))
                {
                    results.Add(loadCase, new List<ArmLoad>());
                }

                foreach (ArmLoad result in currentResults[loadCase])
                {
                    results[loadCase].Add(result);
                }
            }

            //}

            return results;

        }

        /// <summary>
        /// Get the arm design loads.  Translate the PLS loads to the
        /// center line of the bracket bolts.
        /// </summary>
        /// <param name="arm">The tubular arm </param>
        /// <param name="connection">The Thruplate connection</param>
        /// <param name="attachment">The arm bracket under investigation</param>
        /// <param name="loadsAtPoleFace">The PLS loads translated to the pole face.</param>
        /// <returns></returns>
        private Dictionary<string, List<BracketLoad>> GetArmConnectionDesignLoads(TubularArm arm, ThruPlateConnection connection,
                                                                                  TubularArmAttachmentPoint attachment,
                                                                                  out Dictionary<string, List<ArmLoad>> loadsAtPoleFace)
        {
            bool useArmPoleOffsets = arm.UseArmPoleOffsets;

            double poleDiameter = arm.PoleDiameterIn;

            // !! TODO define arm loads
            Dictionary<string, List<ArmLoad>> results = AggregateResults(connection);

            Dictionary<string, List<BracketLoad>> baseresults = new Dictionary<string, List<BracketLoad>>();

            string armLabel = arm.Label;

            loadsAtPoleFace = new Dictionary<string, List<ArmLoad>>();

            if (results == null)
            {
                return baseresults;
            }

            foreach (string key in results.Keys)
            {
                foreach (ArmLoad result in results[key])
                {
                    // ** Get ArmJoints and NonUserArmJoints.
                    List<ArmJoint> allArmJoints =
                        (from j in arm.ArmJoints where j.HorizontalOffset <= 0 select j).ToList();

                    if (arm.NonUserArmJoints.Count > 0)
                    {
                        allArmJoints.AddRange(arm.NonUserArmJoints);
                    }

                    //now we must match result joint to arm joint
                    foreach (var joint in allArmJoints)
                    {
                        if (joint.HorizontalOffset > 0)
                        {
                            continue;
                        }

                        // !! TODO translate PLS arm results !!??!!
                        if (joint.HorizontalOffset.AreEqual(result.RelDist) && joint.VerticalOffset.AreEqual(result.VerticalOffset))
                        {
                            double offset = 0;

                            if (!useArmPoleOffsets)
                            {
                                offset -= poleDiameter.InchesToFeet() / 2;
                            }

                            offset -= connection.GetConnectionOffsetFromPoleFace(attachment); //, bendType);

                            SaddleBracket bracket = (SaddleBracket)attachment;

                            BracketLoad translated = (BracketLoad)TranslateArmResult(result, offset, bracket.ConnectionType);

                            if (!baseresults.ContainsKey(key))
                            {
                                baseresults.Add(key, new List<BracketLoad>());
                                loadsAtPoleFace.Add(key, new List<ArmLoad>());
                            }

                            baseresults[key].Add(translated);
                            loadsAtPoleFace[key].Add(result);
                        }
                    }
                }
            }

            return baseresults;
        }

        /// <summary>
        /// Translate the loads from the pole face to the center line of the bolts.
        /// Where Hocks's spreadsheet defines loads as follows:
        ///     Fv = Axial Force
        ///     ML = Horizontal moment
        ///     FL = Horizontal shear
        ///     Mv = Vertical moment
        ///     FT = Vertical shear
        ///     MT = Torsion
        /// </summary>
        /// <param name="original">The original arm load.</param>
        /// <param name="offset">The offset from the pole face to the center line of bracket bolts.</param>
        /// <param name="connectionType">The connection type: fixed or pinned.</param>
        /// <returns></returns>
        private BracketLoad TranslateArmResult(ArmLoad original, double offset, ArmConnectionMethod connectionType)
        {
            BracketLoad translated = new BracketLoad(original.Id, _arm, _bracket);

            translated.JointLabel = original.JointLabel;
            translated.Label = original.Label;
            translated.LoadCase = original.LoadCase;
            translated.HorizontalShear = Math.Abs(original.HorizontalShear);
            translated.HorizontalMoment = Math.Abs(original.HorizontalMoment) +
                                            (Math.Abs(original.HorizontalShear) * offset);

            if (connectionType == ArmConnectionMethod.Pinned)
            {
                translated.HorizontalMoment = 0;
            }

            translated.LongitudinalOffset = original.LongitudinalOffset;

            translated.RelDist = original.RelDist;
            translated.Torsion = Math.Abs(original.Torsion);
            translated.VerticalShear = Math.Abs(original.VerticalShear);

            translated.AxialForce = Math.Abs(original.AxialForce);
            translated.VerticalMoment = Math.Abs(original.VerticalMoment) + (Math.Abs(original.AxialForce) * offset);

            return translated;
        }

        /// <summary>
        /// Setup the Controlling load case dictionary with the
        /// defined BracketStressTypes.
        /// </summary>
        private void SetupControllingLCDictionary()
        {
            // ** Get the BracketStressTypes
            Array stressTypes = Enum.GetValues(typeof(BracketStressType));

            if (BracketAnalysisResults == null)
            {
                BracketAnalysisResults = new Dictionary<BracketStressType, BracketControllingResult>();
            }
            else
            {
                BracketAnalysisResults.Clear();
            }

            foreach (var typ in stressTypes)
            {
                BracketStressType stressType = (BracketStressType)typ;
                if (stressType == BracketStressType.Unknown)
                {
                    continue;
                }

                BracketAnalysisResults.Add(stressType, new BracketControllingResult());

            }
        }

        /// <summary>
        /// Determine the controlling load case for each bracket stress type.
        /// It is assumed the ControllingLoadCase dictionary is setup properly
        /// with the current BracketStressType(s).
        /// </summary>
        private void ControllingLCase()
        {
            SetupControllingLCDictionary();

            foreach (var kvp in BracketLoadItems)
            {
                string LCdesc = kvp.Key;
                List<BracketLoad> bktLoads = kvp.Value;

                foreach (var load in bktLoads)
                {
                    string loadLabel = load.Label;
                    Guid loadId = load.Id;
                    BracketProperties bracketProperties = load.GetBracketProperties();

                    // ** look at each result to find the controlling result for each stress type.
                    foreach (var par in bracketProperties.BracketResults)
                    {
                        BracketStressType stressType = par.Key;
                        BracketControllingResult ctrlResult = BracketAnalysisResults[stressType];
                        double thisIR = par.Value.InteractionRatio;
                        double ctrlIR = ctrlResult.InteractionRatio;

                        if ((decimal)thisIR >= (decimal)ctrlIR)
                        {
                            BracketControllingResult bktResult = new BracketControllingResult(par.Value, LCdesc, loadId, load);
                            BracketAnalysisResults[stressType] = bktResult;

                            if (!_isOverstressed && bktResult.IsOverstressed)
                            {
                                _isOverstressed = true;
                            }
                        }

                    }

                }
            }

            SetupControllingLoadcaseList();
        }

        /// <summary>
        /// Set up the unique controlling load case list.
        /// </summary>
        private void SetupControllingLoadcaseList()
        {
            if (ControllingLoadcaseList == null)
            {
                ControllingLoadcaseList = new List<BracketLoad>();
            }
            else
            {
                ControllingLoadcaseList.Clear();
            }


            // ** define controlling stress type in each result
            foreach (var kvp in BracketAnalysisResults)
            {
                BracketStressType stressType = kvp.Key;
                BracketControllingResult controllingResult = kvp.Value;
                BracketLoad bracketLoad = controllingResult.ControllingBracketLoad;

                bracketLoad.ControllingStressTypeResults[stressType] = true;

            }

            // ** setup list of results for Bracket Analysis Report
            foreach (var kvp in BracketAnalysisResults)
            {
                BracketStressType stressType = kvp.Key;
                BracketControllingResult controllingResult = kvp.Value;
                BracketLoad bracketLoad = controllingResult.ControllingBracketLoad;

                if (!ControllingLoadcaseList.Contains(bracketLoad))
                {
                    ControllingLoadcaseList.Add(bracketLoad);
                }
            }

        }

        /// <summary>
        /// Get the bracket load list containing each load for this arm..
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, List<BracketLoad>> GetBracketLoads()
        {
            return BracketLoadItems;
        }

        public override string ToString()
        {
            string design = (DesignWorked ? "worked." : "is overstressed!");
            string s = (BracketLoadItems.Count != 1 ? "s" : string.Empty);
            string msg = $"{BracketLoadItems.Count} Bracket Load{s}: Design {design}";

            return msg;
        }

    }
}
