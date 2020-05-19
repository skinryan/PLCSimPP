using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCSimPP.Service.DB
{
    public class DbConst
    {
        public const string DB_NAME = "PLCSimPP";

        public const string MSGLOG_TABLE_NAME = "MsgLog";

        public const string MSGLOG_COLUMN_TIME = "Time";
        public const string MSGLOG_COLUMN_DIRECTION = "Direction";
        public const string MSGLOG_COLUMN_ADDRESS = "Address";
        //public const string MSGLOG_COLUMN_CONTROL = "Control";
        //public const string MSGLOG_COLUMN_STATUS = "Status";
        public const string MSGLOG_COLUMN_COMMAND = "Command";
        //public const string MSGLOG_COLUMN_MESSAGE = "Message";
        public const string MSGLOG_COLUMN_DETAILS = "Details";
        public const string MSGLOG_COLUMN_TOKEN = "Token";

        public const string MSGLOG_PARAM_TIME = "@time";
        public const string MSGLOG_PARAM_DIRECTION = "@direction";
        public const string MSGLOG_PARAM_ADDRESS = "@address";
        //public const string MSGLOG_PARAM_CONTROL = "@control";
        //public const string MSGLOG_PARAM_STATUS = "@status";
        public const string MSGLOG_PARAM_COMMAND = "@command";
        //public const string MSGLOG_PARAM_MESSAGE = "@message";
        public const string MSGLOG_PARAM_DETAILS = "@details";
        public const string MSGLOG_PARAM_TOKEN = "@token";
        public const string MSGLOG_PARAM_SAMPLEID = "@sampleId";

        public const string MSGLOG_AS_MSGCOUNT = "MsgCount";
        public const string MSGLOG_AS_STARTTIME = "StartTime";

        public const string PROC_PAGE_NAME = "ProcGetPageData";
        public const string PROC_PAGE_PARAM_TABLENAME = "TableName";
        public const string PROC_PAGE_PARAM_PRIMARYKEY = "PrimaryKey";
        public const string PROC_PAGE_PARAM_FIELDS = "Fields";
        public const string PROC_PAGE_PARAM_CONDITION = "Condition";
        public const string PROC_PAGE_PARAM_CURRENTPAGE = "CurrentPage";
        public const string PROC_PAGE_PARAM_PAGESIZE = "PageSize";
        public const string PROC_PAGE_PARAM_SORT = "Sort";
        public const string PROC_PAGE_PARAM_RECORDCOUNT = "RecordCount";

        public const string SQL_CONNECTION_KEY = "SqlConnection";

        public const string PAGE_DEFAULT_VALUE_PRIMARYKEY = "ID";
        public const string PAGE_DEFAULT_VALUE_FIELDS = "*";
        public const int PAGE_DEFAULT_VALUE_PAGESIZE = 100;

    }
}
