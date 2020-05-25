using System.ComponentModel;

namespace BCI.PLCSimPP.Comm
{
    public enum SortingOrder
    {
        LaneThrough,
        Sorting,
        SortEmptyCarriers,
    }

    public enum Direction
    {
        Forward,
        Backward
    }

    public enum UnitType
    {
        [Description("Aliquoter")]
        Aliquoter,
        [Description("Centrifuge")]
        Centrifuge,
        [Description("DxC")]
        DxC,
        [Description("Dynamic Inlet")]
        DynamicInlet,
        [Description("Error Lane")]
        ErrorLane,
        [Description("GC")]
        GC,
        [Description("H Lane")]
        HLane,
        [Description("HM Outlet")]
        HMOutlet,
        [Description("I Lane")]
        ILane,
        [Description("Labeler")]
        Labeler,
        [Description("Level Detector")]
        LevelDetector,
        [Description("Outlet")]
        Outlet,
        [Description("Storage Unit")]
        Stocker
    }

    public enum RackType
    {
        [Description("Unrecognized")]
        Unrecognized = 0,
        [Description("Priority")]
        Priority = 11,
        [Description("Normal")]
        Normal = 12,
        [Description("Centrifuge Bypass")]
        Bypass = 13,
        [Description("Remapping")]
        Remap = 14,
        //[Description("RMAP(Ambient)")]
        //A_RM,
        //[Description("BULK")]
        //BULK,
    }

    public enum DcAnalyzerType
    {
        GC = 0,
        DxI = 1,
        AU = 2,
        DxH = 3
    }

    public enum DxCAnalyzerType
    {
        None = 0,
        LX20 = 1,
        DxC = 2
    }

    ///// <summary>
    ///// priority rack
    ///// </summary>
    //public const string RACK_PRIORITY = "11";

    ///// <summary>
    ///// normal rack
    ///// </summary>
    //public const string RACK_NORMAL = "12";

    ///// <summary>
    ///// Centrifuge Bypass Rack
    ///// </summary>
    //public const string RACK_CENTBYPASS = "13";

    ///// <summary>
    ///// Remapping Rack
    ///// </summary>
    //public const string RACK_REMAP = "14";
}



