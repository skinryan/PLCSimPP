using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCI.PLCSimPP.Service.DB
{
    public class DbConst
    {
       
        public const string MSGLOG_TABLE_NAME = "MsgLog";

        public const string MSGLOG_COLUMN_TIME = "Time";
        public const string MSGLOG_COLUMN_TOKEN = "Token";
        public const string MSGLOG_PARAM_TOKEN = "@token";
       
        public const string PROC_PAGE_NAME = "ProcGetPageData";
        public const string PROC_PAGE_PARAM_TABLENAME = "TableName";
        public const string PROC_PAGE_PARAM_PRIMARYKEY = "PrimaryKey";
        public const string PROC_PAGE_PARAM_FIELDS = "Fields";
        public const string PROC_PAGE_PARAM_CONDITION = "Condition";
        public const string PROC_PAGE_PARAM_CURRENTPAGE = "CurrentPage";
        public const string PROC_PAGE_PARAM_PAGESIZE = "PageSize";
        public const string PROC_PAGE_PARAM_SORT = "Sort";
        public const string PROC_PAGE_PARAM_RECORDCOUNT = "RecordCount";

        public const string PAGE_DEFAULT_VALUE_PRIMARYKEY = "ID";
        public const string PAGE_DEFAULT_VALUE_FIELDS = "*";
        public const int PAGE_DEFAULT_VALUE_PAGESIZE = 50;
    }
}
