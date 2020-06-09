using System;
using System.Collections.Generic;
using System.Text;

namespace BCI.PLCSimPP.Comm.Constants
{
    public class RegionName
    {
        public const string CONTENT_REGION = "ContentRegion";

        public const string MENU_REGION = "MenuRegion";

        public const string LAYOUT_REGION = "LayoutRegion";

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
