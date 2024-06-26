using ArmBracketDesignLibrary.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArmBracketDesignLibrary.BracketAnalysis;
using ArmBracketDesignLibrary.StructureComponents.Arms;

namespace ArmBracketDesignLibrary.ArmBracketDesignEngine
{
    public class SaddleBracketDTO
    {
        public SaddleBracketDTO()
        {

        }

        public SaddleBracketDTO(StructureComponents.SaddleBracket sb)
        {
            if (sb != null)
            {

                BlackWeight = sb.BlackWeight;

                foreach (var v in sb.BoltLocations)
                {
                    BoltLocations.Add(new Vector2DTO(new Vectors.Vector2(v.X, v.Y)));
                }
                BoltThreadsInShearPlane = sb.BoltThreadsInShearPlane;
                BoltTopEdgeDistanceInch = sb.BoltTopEdgeDistance;
                IsStandardBracket = sb.IsStandardBracket;
                BoltWeight = sb.BoltWeight;
                BracketSideWidth = sb.BracketSideWidth;
                GalvWeight = sb.GalvWeight;
                MaximumBoltOffset = sb.MaximumBoltOffset;
                BracketAnalysisLoads = new BracketLoadCalcsDTO(sb.BracketAnalysisLoads);

                BracketMaterialSpecification = sb.BracketMaterialSpecification.ToString();
                BracketFinish = sb.Finish.ToString();
                //BracketMaterialSpec = isGalv ? PlateMaterialSpecification.A572_65 : PlateMaterialSpecification.A871_65;
                ThruPlateSpec = BracketMaterialSpecification;
                BracketThick = sb.BracketThick;
                BracketOpening = sb.BracketOpening;

                // ((ThruPlateConnection)sb.Parent).ThruPlateOpening = sb.BracketOpening - 0.125;
                //((ThruPlateConnection)sb.Parent).Thickness = ((StructureComponents.SaddleBracket) sb).ThruPlateThick;

                ThruPlateThick = sb.ThruPlateThick;

                ThruPlateWidth = sb.ThruPlateWidth;
                BracketHeight = sb.Height;
                //ThruPlateBlackWeight = 2 * (decimal)(ThruPlateThick * BracketHeight * ThruPlateWidth * Globals.SteelDensityPerCuFt / 1728d);
                ThruPlateBlackWeight = (decimal) MyExtensions.ThruPlateBlackWt(ThruPlateThick, ThruPlateWidth, BracketHeight);

                StiffenerQty = sb.StiffenerQty;
                StiffenerThick = sb.StiffenerThick;
                StiffenerVertSpacing = sb.StiffenerVertSpacing;
                StiffenerLength = sb.StiffenerLength;
                StiffenerWidth = sb.StiffenerWidth;
                StiffenerWeightLbs = sb.StiffenerWeightLbs;

                BoltQty = sb.BoltQty * 2; // Std Bracket divides the bolt qty by 2.
                BoltSpec = sb.BoltSpec.ToString();
                BoltDiameter = sb.BoltDiameter;
                BoltLength = sb.BoltLength;
                StdBracketID = sb.StdBracketID; // (IsStandardBracket ? sb.StdBracketID : "");

                BoltSpacing = sb.BoltSpacing;

                BracketBoltCenterSpace = sb.GetBktBoltCenterSpace();
            }
        }

        public List<Message> AnalysisWarningMessages { get; set; }
        public double BlackWeight { get; set; }

        // custom
        public double BoltDiameter { get; set; }
        // custom
        public double BoltLength { get; set; }

        public List<Vector2DTO> BoltLocations { get; set; } = new List<Vector2DTO>();

        // custom
        public int BoltQty { get; set; }
        public double BoltSpacing { get; set; }
        public string BoltSpec { get; set; }
        public bool BoltThreadsInShearPlane { get; set; }

        public double BoltTopEdgeDistanceInch { get; set; }

        public double BoltWeight { get; set; }
        public BracketLoadCalcsDTO BracketAnalysisLoads { get; set; }
        // ** From TubularArmAttachmentPoint
        public double BracketBoltCenterSpace { get; set; }
        public string BracketFinish { get; set; }
        public string BracketMaterialSpecification { get; set; }

        // custom
        public double BracketOpening { get; set; }
        public double BracketSideWidth { get; set; }

        // custom
        public double BracketThick { get; set; }
        //public string DesignMethodSpecified { get; set; }
        //public string DesignMethodUsed { get; set; }
        public double GalvWeight { get; set; }

        // custom
        public double BracketHeight { get; set; }
        public bool IsStandardBracket { get; set; }
        public double MaximumBoltOffset { get; set; }
        public string StdBracketID { get; set; }

        // custom
        public int StiffenerQty { get; set; }
        // custom
        public double StiffenerThick { get; set; }

        public double StiffenerVertSpacing { get; set; }

        public double StiffenerLength { get; set; }

        public double StiffenerWidth { get; set; }

        /// <summary>
        /// The computed stiffener black weight in lbs.
        /// </summary>
        public virtual double StiffenerWeightLbs { get; set; }

        public string ThruPlateSpec { get; set; }

        public double ThruPlateWidth { get; set; }
        public double ThruPlateThick { get; set; }

        public decimal ThruPlateBlackWeight { get; set; }

        public override string ToString()
        {
            string bktId = (string.IsNullOrEmpty(StdBracketID) ? "Custom" : StdBracketID);
            return $"{bktId}: {BracketHeight}\"X{BracketThick}\" (side width: {BracketSideWidth:f3}\"";
        }
    }
}
