using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using ArmBracketAnalysisLib.Materials;
using ArmBracketAnalysisLib.StructureComponents.Arms;
using ArmBracketAnalysisLib.Vectors;
//using System.Reflection;
//using LUtility.LMath;

//using Sts.Materials;
//using Sts.UtilityStructure.Calculators;
//using Sts.UtilityStructure.StructureComponents.Arms;
//using Sts.UtilityStructure.StructureComponents.Arms.Connections;
//using Microsoft.Office.Interop.Excel;
//using Sts.UtilityStructure.StructureComponents.Attachments;
//using Sts.UtilityStructure.StructureComponents.Pole;
//using Sts.UtilityStructure.StructureComponents.UsageSummary;
using tqGlobals = TQGlobals;
//using BaseFlangePlateLibrary.Vectors;
using NLog;
//using Utils;

namespace ArmBracketAnalysisLib.StructureComponents
{
    public class Structure
    {
        #region Delegates

        public delegate void DataChangedHandler();

        #endregion


        private static Logger logger = LogManager.GetCurrentClassLogger();

        private readonly List<PlsConnectionPoint> _connectionPoints = new List<PlsConnectionPoint>();
        private readonly Dictionary<Guid, LoadCase> _loadCases = new Dictionary<Guid, LoadCase>();
        private readonly Dictionary<Guid, Pole.Pole> _poles = new Dictionary<Guid, Pole.Pole>();
        private readonly Dictionary<Guid, TubularArm> _tubularArms = new Dictionary<Guid, TubularArm>();
        private readonly Dictionary<string, List<PointLoad>> _pointLoads = new Dictionary<string, List<PointLoad>>();
        private readonly List<PostInsulator> _postInsulators = new List<PostInsulator>();
        private readonly List<SuspensionInsulator> _suspensionInsulators = new List<SuspensionInsulator>();
        private readonly List<TwoPartInsulator> _twoPartInsulators = new List<TwoPartInsulator>();

        private List<SteelPoleLoadCaseUsage> _poleSummaryUsages = new List<SteelPoleLoadCaseUsage>();
        private List<TubularArmLoadCaseUsage> _davitSummaryUsages = new List<TubularArmLoadCaseUsage>();
        private List<TubularArmLoadCaseUsage> _tubularXArmSummaryUsages = new List<TubularArmLoadCaseUsage>();
        private readonly List<Vang> _vangs = new List<Vang>();
        private readonly List<StructureCable> _structureCables = new List<StructureCable>();
        private readonly Dictionary<string, CableProperty> _cableProperties = new Dictionary<string, CableProperty>();
        private readonly Dictionary<string, List<StructureCableLoadCaseUsage>> _structureCableUsages = new Dictionary<string, List<StructureCableLoadCaseUsage>>();
        private readonly List<Brace> _braces = new List<Brace>();
        private readonly Dictionary<string, List<BraceLoadCaseUsage>> _braceUsages = new Dictionary<string, List<BraceLoadCaseUsage>>();
        private readonly List<Guy> _guys = new List<Guy>();
        private readonly Dictionary<string, List<GuyLoadCaseUsage>> _guyUsages = new Dictionary<string, List<GuyLoadCaseUsage>>();
        private readonly Dictionary<Guid, XArm> _xArms = new Dictionary<Guid, XArm>();

        private long _dbIdx;
        // private QtInstance _qtInstance = null;
        private PlateFinish _finish;
        private Guid _guid;
        private string _notes = string.Empty;
        private bool _useArmPoleOffsets = true;
        private Dictionary<string, List<EquilibriumJointPositionRotation>> _equilibriumJointPositionRotations = new Dictionary<string, List<EquilibriumJointPositionRotation>>();

        public Structure(Guid guid)
        {
            _guid = guid;

            JobOptionNumber = string.Empty;
            JobRevisionNumber = string.Empty;
            JobNumber = string.Empty;
            BidOptionNumber = string.Empty;
            BidRevisionNumber = string.Empty;
            BidNumber = string.Empty;

        }

        public Structure()
            : this(Guid.NewGuid())
        {
        }

        /// <summary>
        /// The QT Number of the pole
        /// </summary>

        public Guid Id
        {
            get { return _guid; }
        }

        public string PolFilenamePath { get; set; }

        public int StructureNumber { get; set; }
        public string BidNumber { get; set; }
        public string BidRevisionNumber { get; set; }
        public string BidOptionNumber { get; set; }
        public string JobNumber { get; set; }
        public string JobRevisionNumber { get; set; }
        public string JobOptionNumber { get; set; }

        //hack to identify structure type - no enum implemented this is a end-of-life application
        public string StructureType { get; set; }

        public bool UseArmPoleOffsets
        {
            get { return _useArmPoleOffsets; }
            set { _useArmPoleOffsets = value; }
        }

        public long DbIdx
        {
            get { return _dbIdx; }
            set { _dbIdx = value; }
        }

        public string Notes
        {
            get { return _notes; }
            set { _notes = value; }
        }

        public string Model { get; set; }
        public string Label { get; set; }

        public ArmsProject Parent { get; set; }

        public List<TubularArm> TubularArms
        {
            get { return _tubularArms.Values.ToList(); }
        }

        public List<XArm> XArms
        {
            get { return _xArms.Values.ToList(); }
        }

        public List<PostInsulator> PostInsulators
        {
            get { return _postInsulators; }
        }

        public PlateFinish Finish
        {
            get { return _finish == PlateFinish.None ? PlateFinish.Galvanized : _finish; }
            set { _finish = value; }
        }

        public List<Pole.Pole> Poles
        {
            get
            {
                if (_poles == null)
                {
                    return null;
                }
                return _poles.Values.ToList();
            }
        }

        public List<PlsConnectionPoint> PlsConnectionPoints
        {
            get { return _connectionPoints; }
        }

        public Dictionary<string, List<PointLoad>> PointLoads
        {
            get { return _pointLoads; }
        }

        public List<SteelPoleLoadCaseUsage> PoleSummaryUsages
        {
            get { return _poleSummaryUsages; }
        }

        public List<TubularArmLoadCaseUsage> DavitSummaryUsages
        {
            get { return _davitSummaryUsages; }
        }

        public List<TubularArmLoadCaseUsage> TubularXArmSummaryUsages
        {
            get { return _tubularXArmSummaryUsages; }
        }

        public List<Guid> ConnectionPointIds
        {
            get { return PlsConnectionPoints.Select(point => point.Id).ToList(); }
        }

        public List<EquilibriumJointPositionRotation> GetJointPositionRotations(string loadCase)
        {
            if (_equilibriumJointPositionRotations != null && _equilibriumJointPositionRotations.Count > 0)
            {
                return _equilibriumJointPositionRotations[loadCase].ToList();
            }
            return new List<EquilibriumJointPositionRotation>();
        }

        public void AddEquilibriumJointPositionRotation(EquilibriumJointPositionRotation jointPosRot)
        {
            if (!_equilibriumJointPositionRotations.ContainsKey(jointPosRot.LoadCase))
            {
                _equilibriumJointPositionRotations.Add(jointPosRot.LoadCase, new List<EquilibriumJointPositionRotation>());
            }
            _equilibriumJointPositionRotations[jointPosRot.LoadCase].Add(jointPosRot);
        }

        public void AddTubularArm(TubularArm arm)
        {
            RemoveArm(arm.Id);
            arm.Parent = this;
            _tubularArms.Add(arm.Id, arm);
        }

        public void AddXArm(XArm arm)
        {
            RemoveArm(arm.Id);
            arm.Parent = this;
            _xArms.Add(arm.Id, arm);
        }

        public void RemoveArm(Guid id)
        {
            if (_tubularArms.ContainsKey(id))
            {
                _tubularArms.Remove(id);
            }
        }

        public void AddVang(Vang vang)
        {
            _vangs.Add(vang);
        }

        public List<Vang> Vangs
        {
            get { return _vangs.ToList(); }
        }

        public void AddStructureCable(StructureCable cable)
        {
            _structureCables.Add(cable);
        }

        //public bool QtLinked
        //{
        //    get { return _qtInstance != null; }
        //}

        //public QtInstance LinkedQt
        //{
        //    get { return _qtInstance; }
        //}

        //public void LinkQt(string filename)
        //{
        //    _qtInstance = new QtInstance(filename);

        //}

        //public void ClearQtLink()
        //{
        //    if (_qtInstance != null)
        //    {
        //        _qtInstance.CloseQt(false);
        //        _qtInstance = null;
        //    }
        //}

        public void AddCableProperty(CableProperty property)
        {
            if (_cableProperties.ContainsKey(property.Label)) _cableProperties.Remove(property.Label);
            _cableProperties.Add(property.Label, property);
        }

        public List<StructureCable> StructureCables
        {
            get { return _structureCables; }
        }

        public List<StructureCableLoadCaseUsage> GetStructureCableUsages(string loadCase)
        {
            return !_structureCableUsages.ContainsKey(loadCase) ? new List<StructureCableLoadCaseUsage>() : _structureCableUsages[loadCase].ToList();
        }

        public void AddBrace(Brace brace)
        {
            _braces.Add(brace);
        }

        public void AddGuy(Guy guy)
        {
            _guys.Add(guy);
        }

        public void AddGuyUsageSummary(GuyLoadCaseUsage usage)
        {
            if (!_guyUsages.ContainsKey(usage.LoadCase))
            {
                _guyUsages.Add(usage.LoadCase, new List<GuyLoadCaseUsage>());
            }
            _guyUsages[usage.LoadCase].Add(usage);
        }

        public List<GuyLoadCaseUsage> GetGuyUsages(string loadCase)
        {
            if (!_guyUsages.ContainsKey(loadCase)) return new List<GuyLoadCaseUsage>();
            return _guyUsages[loadCase];
        }

        public List<Guy> Guys
        {
            get { return _guys; }
        }

        public List<BraceLoadCaseUsage> GetBraceUsages(string loadCase)
        {
            return _braceUsages.ContainsKey(loadCase) ? _braceUsages[loadCase] : new List<BraceLoadCaseUsage>();
        }

        public List<CableProperty> CableProperties
        {
            get { return _cableProperties.Values.ToList(); }
        }

        public List<Brace> Braces
        {
            get { return _braces.ToList(); }
        }

        public void AddBraceUsage(BraceLoadCaseUsage usage)
        {
            if (!_braceUsages.ContainsKey(usage.LoadCase))
            {
                _braceUsages.Add(usage.LoadCase, new List<BraceLoadCaseUsage>());
            }

            _braceUsages[usage.LoadCase].Add(usage);
        }

        public void AddStructureCableUsage(StructureCableLoadCaseUsage usage)
        {
            if (!_structureCableUsages.ContainsKey(usage.LoadCase))
            {
                _structureCableUsages.Add(usage.LoadCase, new List<StructureCableLoadCaseUsage>());
            }

            if (_structureCableUsages[usage.LoadCase] == null)
            {
                _structureCableUsages[usage.LoadCase] = new List<StructureCableLoadCaseUsage>();
            }

            _structureCableUsages[usage.LoadCase].Add(usage);
        }

        public void AddPoleSummaryUsage(SteelPoleLoadCaseUsage usage)
        {
            _poleSummaryUsages.Add(usage);
        }

        public void AddSuspensionInsulator(SuspensionInsulator insulator)
        {
            _suspensionInsulators.Add(insulator);
        }

        public List<SuspensionInsulator> SuspensionInsulators
        {
            get { return _suspensionInsulators.ToList(); }
        }

        public void AddTwoPartInsulator(TwoPartInsulator insulator)
        {
            _twoPartInsulators.Add(insulator);
        }

        public List<TwoPartInsulator> TwoPartInsulators
        {
            get { return _twoPartInsulators; }
        }

        public void AddDavitSummaryUsage(TubularArmLoadCaseUsage usage)
        {
            _davitSummaryUsages.Add(usage);
        }


        public void AddTubularXArmSummaryUsage(TubularArmLoadCaseUsage usage)
        {
            _tubularXArmSummaryUsages.Add(usage);
        }


        public TubularArm GetConnectionPointArm(Guid connectionPointId, double azimuth)
        {
            foreach (TubularArm arm in GetConnectionPointArms(connectionPointId))
            {
                if (DoubleUtil.areEqual(arm.Azimuth, azimuth))
                {
                    return arm;
                }
            }

            return null;
        }

        public PlsConnectionPoint GetPlsConnectionPointByArmId(Guid armId)
        {
            return PlsConnectionPoints.Find(o => o.ArmIds.Values.Contains(armId));
        }

        public void AddPole(Pole.Pole pole)
        {
            pole.DataChanged += PoleDataChanged;
            pole.Parent = this;
            _poles.Add(pole.Id, pole);
        }

        public Pole.Pole GetPole(Guid id)
        {
            return !_poles.ContainsKey(id) ? null : _poles[id];
        }

        /// <summary>
        /// Checks each arm tube connected to the structure, ensuring that every arm 
        /// has an arm connection with attachments to support it.
        /// </summary>
        public void CleanupArmConnections()
        {
            //Normalize all the arm azimuths and assign material properties.
            foreach (TubularArm arm in TubularArms)
            {
                arm.Azimuth = Trigonometry.NormalizeAngleD(arm.Azimuth);
                if ((decimal)arm.Azimuth == 360) arm.Azimuth = 0;

                // ** Not here!! Adjust arm length after bracket selected or designed
                // ** in ThruPlateDesignCalcs.DesignConnections
                // ** NOTE: DavitArm and TubularArm do NOT calculate arm lengths the same.
                // ModifyArmToMatchQt(arm);  
            }

            Pole.Pole p = Poles[0];

            foreach (PlsConnectionPoint point in _connectionPoints)
            {
                // ** Used for structures with multiple poles: HFrame, AFrames, etc
                // ** only want the first pole in the structure (ie at position 0)
                if (point.PoleId != p.Id)
                {
                    continue;
                }

                foreach (Guid armId in point.ArmIds.Values)
                {
                    // 2012-08-03 First check to see if two arms on same dft are not 180 degrees apart
                    // and throw error if so
                    //TubularArm arm = GetArm(armId);
                    TubularArm arm = DefineThisArm(this, armId, point);
                    bool armPlaced = false;
                    bool validArm = true;

                    foreach (Guid armId2 in point.ArmIds.Values)
                    {
                        //TubularArm arm2 = GetArm(armId2);
                        TubularArm arm2 = DefineThisArm(this, armId2, point);
                        if (DoubleUtil.areEqual(arm.Dft, arm2.Dft) && !DoubleUtil.areEqual(arm.Azimuth, arm2.Azimuth))
                        {
                            if (!DoubleUtil.areEqual(Math.Abs(arm.Azimuth - arm2.Azimuth), 180d))
                            {
                                double diff = Math.Abs(arm.Azimuth - arm2.Azimuth);
                                string msg = $"Arms at {arm.Dft:f2}' are not 180 degrees apart\r\n";
                                msg += $"\tarm 1 @ {arm.Azimuth} deg && arm 2 @ {arm2.Azimuth} deg (delta = {diff} deg)\r\n";
                                msg += "\tThis is not supported in this version of Edge.\r\n";
                                //throw new Exception(msg);
                                //throw new Exception("Arms at same elevation that are not 180 degrees apart are not supported in this version of Edge.\r\n");
                                logger.Error(msg);
                                Results.Instance().ResultCode = RunResultCode.Fail;
                                Results.Instance().ResultNotes.AppendLine(msg);
                                Results.Instance().WriteResultNotesToFile();
                                Results.Instance().ShowResults();
                                validArm = false;
                                break;
                            }
                        }
                    }

                    // ** If this is not a valid arm, go to the next arm and check it.
                    // ** Do not just 'throw exception' and jump out of the method if one arm fails.
                    if (!validArm)
                    {
                        continue;
                    }

                    foreach (ArmConnection connection in point.ArmConnections)
                    {
                        foreach (TubularArmAttachmentPoint attach in connection.Attachments)
                        {
                            if (DoubleUtil.areEqual(attach.Azimuth, arm.Azimuth))
                            {
                                //if (attach.ArmId != null && attach.ArmId != Guid.Empty && attach.ArmId != arm.Id)
                                //{
                                //    throw new Exception("Unable to reconcile arm attachments.");
                                //}

                                attach.ArmId = arm.Id;
                                armPlaced = true;
                            }
                        }
                    }

                    if (!armPlaced)
                    {
                        //Assume thru plate connection for the time being
                        ThruPlateConnection tpc = new ThruPlateConnection();
                        tpc.Parent = point;

                        SaddleBracket bracket = new SaddleBracket(tpc);
                        bracket.Azimuth = arm.Azimuth;
                        bracket.ArmId = arm.Id;
                        tpc.Attachments.Add(bracket);

                        bracket = new SaddleBracket(tpc);
                        bracket.Azimuth = Trigonometry.NormalizeAngleD(arm.Azimuth + 180);
                        if ((decimal)bracket.Azimuth == 360) bracket.Azimuth = 0;

                        tpc.Attachments.Add(bracket);

                        AssignQtValuesToArmAssembly(arm, tpc);

                        AssignQtBracketThruPlateMaterial(arm, tpc);

                        point.ArmConnections.Add(tpc);
                    }
                }
            }

            foreach (PlsConnectionPoint point in _connectionPoints)
            {
                foreach (ArmConnection connection in point.ArmConnections)
                {
                    connection.CullUnusedAttachments();
                }
            }

        }

        /// <summary>
        /// Assign arm properties and
        /// Assign Bracket and Thru Plate information only if the Bracket Method Specified is OVRD (Override)
        /// </summary>
        /// <remarks>
        /// All others will either be Standard brackets or the bracket will be designed if a standard couldn't
        /// support the loads or a standard bracket could not be found.
        /// </remarks>
        /// <param name="plsArm">PLS arm</param>
        /// <param name="tpc">Thru plate connection</param>
        private void AssignQtValuesToArmAssembly(TubularArm plsArm, ThruPlateConnection tpc)
        {
            tqGlobals.DavitArm qtArm = GetQtArm(plsArm);

            if (qtArm == null || qtArm.BracketMethodSpecified != Enums.BracketDesignMethod.OVRD)
            {
                return;  // nothing to process
            }

            foreach (var bkt in tpc.Attachments)
            {
                bkt.DesignMethodSpecified = qtArm.BracketMethodSpecified;
                bkt.DesignMethodUsed = qtArm.BracketMethodSpecified;

                bkt.BracketThick = qtArm.Bracket.Thickness;
                bkt.BracketOpening = qtArm.Bracket.SpanInside;
                bkt.Height = qtArm.Bracket.Height;

                bkt.BoltPattern = BracketBoltPattern.SingleRow;
                bkt.BoltQty = qtArm.Bracket.BoltQty / 2;
                bkt.BoltDiameter = qtArm.Bracket.BoltDia;
                bkt.BoltLength = qtArm.Bracket.BoltLength;

                bkt.ThruPlateWidth = qtArm.Bracket.SpanInside;

                bkt.StiffenerWidth = 3.0 * Math.Max(0.75, qtArm.Bracket.Thickness);
                bkt.StiffenerThick = qtArm.Bracket.SideGussetThickness;
                if (bkt.StiffenerThick > 0)
                {
                    bkt.StiffenerQty = qtArm.Bracket.StiffenerQty > 0 ? qtArm.Bracket.StiffenerQty : 2;  // was = 1;
                }
            }

            // ** Thru Plate values
            tpc.ThruPlateOpening = qtArm.Bracket.SpanInside - 0.125;
            tpc.Thickness = qtArm.Tab.Thickness;
            tpc.ThruPlateLength = qtArm.Tab.Depth;
        }

        /// <summary>
        /// Assign Bracket and Thru Plate material information for DavitArm and XArm connections.
        /// </summary>
        /// <remarks>
        /// The arm material is being assigned in PlsData, so use the same material type
        /// for both the thru plates and brackets.
        /// </remarks>
        /// <param name="plsArm">PLS arm</param>
        /// <param name="tpc">Thru plate connection</param>
        private void AssignQtBracketThruPlateMaterial(TubularArm plsArm, ThruPlateConnection tpc)
        {
            PlateFinish pltFinish = plsArm.Finish;

            foreach (var bkt in tpc.Attachments)
            {
                bkt.BracketMaterialSpec = PlateMaterial.GetStandardPlateMaterialSpecification(pltFinish, bkt.BracketThick);
            }

            tpc.MaterialSpecification = PlateMaterial.GetStandardPlateMaterialSpecification(pltFinish, tpc.Thickness);
            tpc.Finish = pltFinish;

        }

        /// <summary>
        /// Modify the PLS arm length to match the QT in order 
        /// to get the arm weight closer to the QT arm wt.
        /// </summary>
        /// <param name="plsArm"></param>
        private void ModifyArmToMatchQt(TubularArm plsArm)
        {
            tqGlobals.DavitArm qtArm = GetQtArm(plsArm);

            if (qtArm == null)
            {
                return;  // nothing to process
            }

            // ** Arm values
            plsArm.LengthFt = qtArm.CenterLineLength / 12;
        }

        /// <summary>
        /// Get the Davit arm information from the QT associated with the PLS arm.
        /// </summary>
        /// <param name="plsArm"></param>
        /// <returns>Returns the arm as defined in QtParser.</returns>
        private tqGlobals.DavitArm GetQtArm(TubularArm plsArm)
        {
            string attachLabel = plsArm.AttachLabel;
            string propertyLabel = plsArm.DavitPropertyLabel;
            foreach (var arm in tqGlobals.Globals.Instance.QtDavitArms)
            {
                if (arm.PlsAttachLabel != null && arm.PlsAttachLabel.Equals(attachLabel) && arm.PlsArmPropertyLabel.Equals(propertyLabel))
                {
                    //bkt.BoltDiameter = arm.Bracket.
                    //tda.DesignMethodSpecified = arm.BracketMethodSpecified;

                    logger.Info("Qt Arm found for attachLabel [{0}] and propertyLabel[{1}] :: BracketMethodSpecified: {2}",
                                                    attachLabel, propertyLabel, arm.BracketMethodSpecified.ToString());

                    return arm;     // all done: no need to look at any other arms.
                }
            }
            logger.Warn("Qt Arm not found for attachLabel [{0}] and propertyLabel[{1}] :: UseStdBracket NOT set.", attachLabel, propertyLabel);
            return null;
        }

        /// <summary>
        /// Define the material specification for the arm.
        /// </summary>
        /// <param name="plsArm"></param>
        /// <returns></returns>
        private PlateMaterialSpecification DefineArmMaterialSpec(TubularArm plsArm)
        {
            tqGlobals.DavitArm qtArm = GetQtArm(plsArm);

            if (qtArm == null)
            {
                return PlateMaterialSpecification.None;  // no arm
            }

            // ** setup the material specification for this arm and arm connections
            string spec = qtArm.ParentSection.RawPlate.Specification;
            string materialSpec = spec;
            PlateFinish pltFinish = spec.Contains("A871") ? PlateFinish.Weathering : PlateFinish.Galvanized;


            PlateMaterialSpecification pltSpec = PlateMaterial.GetStandardPlateMaterialSpecification(pltFinish, plsArm.MaterialThicknessInches);

            return pltSpec;
        }

        #region Define XArm direction

        /// <summary>
        /// Get the arm given the arm ID.
        /// If the arm is an Cross Arm, this method will compute the arm azimuth and define it.
        /// </summary>
        /// <param name="armId"></param>
        /// <param name="connPt"></param>
        /// <returns></returns>
        public static TubularArm DefineThisArm(Structure myStructure, Guid armId, PlsConnectionPoint connPt)
        {
            double armAzimuth = 0;
            TubularArm arm = myStructure.GetArm(armId);

            if (arm.GetType() != typeof(TubularXArm) || myStructure.Poles.Count <= 1)
            {
                return arm;
            }

            // ** Determine azimuth of XArm
            string fromPoint = connPt.Label;
            TubularXArm xArm = (TubularXArm)arm;
            string label = (from c in xArm.ConnectionTypes where c.Key == fromPoint select c.Key).First();
            if (xArm.ConnectionTypes.Count <= 1 || string.IsNullOrEmpty(label))
            {
                return arm;
            }

            armAzimuth = Structure.DetermineXArmDirection(myStructure, xArm.ConnectionTypes, fromPoint);
            xArm.Azimuth = armAzimuth;
            xArm.AttachLabel = fromPoint; // assign pole attachment label

            return xArm;

        }


        /// <summary>
        /// Determine the direction of the XArm from the connection points.
        /// </summary>
        /// <param name="structure">The structure containing the poles.</param>
        /// <param name="XArmConnections">The list of XArm connections</param>
        /// <param name="fromLabel">The origin of the XArm on the pole.</param>
        /// <returns></returns>
        public static double DetermineXArmDirection(Structure myStructure, Dictionary<string, string> XArmConnections, string fromLabel)
        {
            if (XArmConnections.Count <= 1)
            {
                return 0;
            }

            // ** there should only be 2 connections in XArmConnections
            string fromConn = string.Empty;
            string toConn = string.Empty;
            foreach (var kv in XArmConnections)
            {
                if (string.IsNullOrEmpty(fromConn) && kv.Key.Equals(fromLabel))
                {
                    fromConn = kv.Key;
                }

                if (string.IsNullOrEmpty(toConn) && !kv.Key.Equals(fromLabel))
                {
                    toConn = kv.Key;
                }

            }

            Pole.Pole atOrigin = getPoleFromConnectionLabel(myStructure, fromConn);
            Pole.Pole atEnd = getPoleFromConnectionLabel(myStructure, toConn);

            // there is no direction if one of these is null!
            if (atOrigin == null || atEnd == null)
            {
                return 0;
            }

            //Vector3D from = new Vector3D(atOrigin.BaseXCoordinate, atOrigin.BaseYCoordinate, atOrigin.BaseZCoordinate);
            //Vector3D to = new Vector3D(atEnd.BaseXCoordinate, atEnd.BaseYCoordinate, atEnd.BaseZCoordinate);

            // ** The Z coordinate does not contribute to which side of the pole the cross arm is on.
            // ** it causes weird angles if one pole is at a different Z elevation.
            Vector3D from = new Vector3D(atOrigin.BaseXCoordinate, atOrigin.BaseYCoordinate, 0.0);
            Vector3D to = new Vector3D(atEnd.BaseXCoordinate, atEnd.BaseYCoordinate, 0.0);

            Vector3D direction = GetDirectionVector(from, to);

            double yAng = Math.Acos(direction.y) * 180 / Math.PI;

            //if (direction.x < 0 || direction.y < 0)
            //{
            //    return 180;
            //}

            //return 0;

            return yAng;

        }

        /// <summary>
        /// Get the pole at the poleLabel.
        /// </summary>
        /// <param name="poleLabel"></param>
        /// <returns></returns>
        public static Pole.Pole getPoleFromConnectionLabel(Structure myStructure, string poleLabel)
        {
            string poleLbl = poleLabel;

            if (poleLabel.Contains(":"))
            {
                string[] w = poleLabel.Split(':');
                poleLbl = w[0];
            }

            foreach (var pole in myStructure.Poles)
            {
                if (poleLbl.StartsWith(pole.Label))
                {
                    return pole;
                }
            }

            return null;
        }

        /// <summary>
        /// Get the direction cosine for the vectors.
        /// </summary>
        /// <param name="from">Starting point (Vector)</param>
        /// <param name="to">Ending point (Vector)</param>
        /// <returns></returns>
        public static Vector3D GetDirectionVector(Vector3D from, Vector3D to)
        {
            Line3D line = new Line3D(from, to);

            Vector3D direction = line.Direction;

            return direction;

        }

        #endregion // Define XArm direction


        ///// <summary>
        ///// Find the QT Davit Arm given the PLS Xml davit arm location.
        ///// </summary>
        ///// <param name="locator"></param>
        ///// <returns></returns>
        //private void AssignQtArmParameters(TubularArm arm)
        //{
        //    if (tqGlobals.Globals.Instance.QtDavitArms == null || tqGlobals.Globals.Instance.QtDavitArms.Count == 0)
        //    {
        //        return;
        //    }

        //    foreach (var qtArm in tqGlobals.Globals.Instance.QtDavitArms)
        //    {
        //        //bool sameDft = (qtArm.Locator.Offset.Equals(arm.Dft));
        //        //bool sameAngle = (qtArm.Locator.Angle.Equals(arm.Azimuth));
        //        bool sameOffset = (qtArm.Locator.Parent.Offset.Equals(arm.Dft));
        //        bool sameAngle = (qtArm.Locator.Angle.Equals(arm.Azimuth));

        //        if (sameOffset && sameAngle)
        //        {
        //            arm.UseStdBracket = qtArm.UseStdBracket;
        //            break;
        //        }

        //    }

        //}


        public void AddLoadCase(LoadCase loadCase)
        {
            _loadCases.Add(loadCase.Id, loadCase);
        }

        public List<LoadCase> LoadCases
        {
            get
            {
                if (_loadCases == null) return new List<LoadCase>();
                return _loadCases.Values.ToList();
            }
        }

        private void PoleDataChanged()
        {
            OnDataChanged();
        }

        public void AddPlsConnectionPoint(PlsConnectionPoint point)
        {
            foreach (PlsConnectionPoint cp in PlsConnectionPoints)
            {
                if (cp.Label == point.Label)
                {
                    return;
                }
            }

            point.Parent = this;
            PlsConnectionPoints.Add(point);
            OnDataChanged();
        }

        public PlsConnectionPoint GetConnectionPoint(Guid id)
        {
            return _connectionPoints.FirstOrDefault(point => point.Id == id);
        }

        public PlsConnectionPoint GetConnectionPointByLabel(string label)
        {
            return _connectionPoints.Find(point => point.Label == label);
        }

        public bool ConnectionPointDisplayLabelExists(string label)
        {
            return _connectionPoints.Exists(point => point.DisplayLabel == label);
        }

        public List<TubularArm> GetConnectionPointArms(Guid connectionPointId)
        {
            List<TubularArm> arms = new List<TubularArm>();

            foreach (PlsConnectionPoint point in _connectionPoints)
            {
                if (point.Id == connectionPointId)
                {
                    foreach (Guid armId in point.ArmIds.Values)
                    {
                        if (_tubularArms.ContainsKey(armId))
                        {
                            arms.Add(_tubularArms[armId]);
                        }
                    }

                    break;
                }
            }

            return arms;
        }

        public double GetDavitArmConnectionOffset(Guid armId)
        {
            foreach (PlsConnectionPoint point in PlsConnectionPoints)
            {
                if (point.ArmIds.ContainsValue(armId))
                {
                    foreach (ArmConnection conn in point.ArmConnections)
                    {
                        foreach (TubularArmAttachmentPoint attachment in conn.Attachments)
                        {
                            if (attachment.ArmId == armId)
                            {
                                return UnitConversion.InchesToFeet(conn.GetArmBaseOffsetFromPoleFace(attachment, Parent.BendRadiusType));
                            }
                        }
                    }
                }
            }

            return 0;
        }

        public TubularArm GetArm(Guid id)
        {

            TubularArm t = null;
            foreach (TubularArm ta in TubularArms)
            {
                if (ta.Id == id)
                {
                    t = ta;
                    break;
                }
            }

            if (t == null)
            {
                bool found = false;
                foreach (Guid cpId in ConnectionPointIds)
                {
                    if (found) break;
                    List<TubularArm> tas = GetConnectionPointArms(cpId);
                    foreach (TubularArm ta in tas)
                    {
                        if (ta.Id == id)
                        {
                            t = ta;
                            found = true;
                            break;
                        }
                    }
                }
            }

            //if (t.GetType() == typeof(TubularXArm))
            //{
            //    t.Parent.Parent.DefineXArm(t);  // define the azimuth of the XArm
            //}

            return t;
        }

        public TubularArm GetArmByLabel(string label)
        {
            TubularArm t = null;
            foreach (TubularArm ta in TubularArms)
            {
                if (ta.Label == label)
                {
                    t = ta;
                }
            }
            return t;
        }

        public event DataChangedHandler DataChanged;

        public void OnDataChanged()
        {
            if (DataChanged != null)
            {
                DataChanged();
            }
        }

        public void RemovePole(Guid guid)
        {
            if (_poles.ContainsKey(guid))
            {
                _poles.Remove(guid);
            }
        }

        public void RemoveConnectionPoint(Guid guid)
        {
            for (int i = 0; i < _connectionPoints.Count; i++)
            {
                if (_connectionPoints[i].Id == guid)
                {
                    _connectionPoints.RemoveAt(i);
                    break;
                }
            }
        }

        public void RemoveArmTube(Guid guid)
        {
            if (_tubularArms.ContainsKey(guid))
            {
                double azimuth = _tubularArms[guid].Azimuth;

                foreach (PlsConnectionPoint point in _connectionPoints)
                {
                    if (point.ArmIds.ContainsValue(guid))
                    {
                        foreach (double key in point.ArmIds.Keys)
                        {
                            if (point.ArmIds[key] == guid)
                            {
                                point.ArmIds.Remove(key);
                                break;
                            }
                        }
                    }

                    if (point.GetArmConnection(azimuth) != null)
                    {
                        point.RemoveArmConnection(point.GetArmConnection(azimuth).Id, azimuth);
                    }


                }
                _tubularArms.Remove(guid);
            }
        }


        public PoleSection GetConnectedPoleSection(Guid connectionPointId)
        {
            foreach (PlsConnectionPoint point in PlsConnectionPoints)
            {
                if (point.Id != connectionPointId) continue;

                Pole.Pole pole = GetPole(point.PoleId);

                return PoleSectionCalculator.GetOutermostSectionAtLocation(point.Dft, pole);
            }

            return null;
        }
    }
}