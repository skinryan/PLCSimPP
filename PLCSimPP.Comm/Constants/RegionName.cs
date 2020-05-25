using System;
using System.Collections.Generic;
using System.Text;

namespace BCI.PLCSimPP.Comm.Constant
{
    public class RegionName
    {
        public const string CONTENTREGION = "ContentRegion";

        public const string MENUREGION = "MenuRegion";

        public const string LAYOUTREGION = "LayoutRegion";

        public static Dictionary<string, string> ViewName = new Dictionary<string, string>
        {
            { "LogViewer","Communication Log" },
            { "Monitor","Monitor" },
            { "About","About" },
            { "Configuration","Settings" },
            { "SiteMapEditer","SiteMap Edit" },
            { "DeviceLayout","Layout" },
        };
    }


}
