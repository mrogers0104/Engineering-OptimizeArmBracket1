using System;
using System.Collections.Generic;
using ArmBracketDesignLibrary.Helpers;
using ArmBracketDesignLibrary.Materials;
using ArmBracketDesignLibrary.StructureComponents.Pole;
using Newtonsoft.Json;

//using Sts.UtilityStructure.Calculators;
//using Sts.UtilityStructure.StructureComponents.Arms.Connections.Loads;
//using Sts.UtilityStructure.StructureComponents.Arms.Loads;
//using Sts.UtilityStructure.StructureComponents.Pole;
//using LUtility.LMath;

namespace ArmBracketDesignLibrary.StructureComponents.Arms
{
    public abstract class ArmConnection
    {
        protected readonly List<TubularArmAttachmentPoint> _attachments = new List<TubularArmAttachmentPoint>();

        #region Constructors

        protected ArmConnection(Guid guid)
        {
            Id = guid;
        }

        protected ArmConnection() : this(Guid.NewGuid())
        {
        }
            

        #endregion  // Constructors

        #region Properties

        


        //public abstract List<double> AttachedArmAzimuths { get; }
        [JsonIgnore]
        public List<TubularArmAttachmentPoint> Attachments
        {
            get { return _attachments; }
        }

        public abstract ConnectionType ConnectionType { get; }

        public virtual string ConnectionTypeLabel
        {
            get { return "None"; }
        }

        public PlateFinish PlateFinish { get; set; }


        public Guid Id { get; }

        public string Label { get; set; } = string.Empty;


        public virtual double Thickness { get; set; }

        #endregion  // Properties

        #region Methods

        //public void InitializeDimensions(ArmsProject project)
        //{
        //    //bool allowArmCoping 
        //    //PoleSection poleSection = Parent.Parent.GetConnectedPoleSection(Parent.Id);
        //    //double poleThickness = poleSection.MaterialThicknessInches;

        //    InitializeDimensions(project);

        //}

        public void AddAttachment(SaddleBracket attachment)
        {
            attachment.Parent = this;
            SaddleBracket t = null;


            if (Attachments.Count > 0)
            {
                t = (SaddleBracket)Attachments.Find(o => o.Azimuth.AreEqual(attachment.Azimuth));
                //t = (SaddleBracket)Attachments.Find(o => o.Azimuth == attachment.Azimuth);
            }

            if (t != null)
            {
                Attachments.Remove(t);

            }

            Attachments.Add(attachment);

            //LinkArmsWithAttachments();

        }

        public abstract double GetConnectionOffsetFromPoleFace(TubularArmAttachmentPoint attachment);

        //public abstract void InitializeDimensions(double poleDiameter, double poleThickness, bool allowArmCoping);

        public abstract void FindWorkingStdBracket(ArmProject project); //, bool allowArmCoping);

        #endregion  // Methods

    }
}