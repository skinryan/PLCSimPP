using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCI.PLCSimPP.Service.DB
{
    public class PageCriteria
    {
        public string TableName { get; set; }
        public string Fields { get; set; } = DbConst.PAGE_DEFAULT_VALUE_FIELDS;
        public int PageSize { get; set; } = DbConst.PAGE_DEFAULT_VALUE_PAGESIZE;
        public string PrimaryKey { get; set; } = DbConst.PAGE_DEFAULT_VALUE_PRIMARYKEY;
        public int CurrentPage { get; set; } = 1;
        public string Sort { get; set; } = string.Empty;
        public string Condition { get; set; } = string.Empty;
        public int RecordCount { get; set; }
    }
}
