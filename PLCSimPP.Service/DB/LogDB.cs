using CommonServiceLocator;
using Dapper;
using PLCSimPP.Comm.Interfaces.Services;
using PLCSimPP.Service.Log;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCSimPP.Service.DB
{
    public class LogDB : IDisposable
    {
        private IDbConnection mConn;

        private static LogDB sCurrentDb;

        /// <summary>
        /// Singleton
        /// </summary>
        public static LogDB Current
        {
            get
            {
                if (sCurrentDb == null)
                    LogDB.sCurrentDb = new LogDB();
                return LogDB.sCurrentDb;
            }
        }

        public void Dispose()
        {
            if (mConn == null)
                return;

            mConn.Dispose();
            mConn = null;
        }

        private LogDB()
        {
            IConfigService configServ = ServiceLocator.Current.GetInstance<IConfigService>();
            var config = configServ.ReadSysConfig();
            mConn = new SqlConnection(config.ConnectionString);
            //mConn = new SqlConnection("data source=.\\SQLEXPRESS;initial catalog=PLCSimPP;integrated security=SSPI;");
            //mConn = new SqlConnection(ConfigurationManager.AppSettings[DbConst.SQL_CONNECTION_KEY]);

        }

        public bool Insert(LogContent msgLog)
        {
            //string sqlString = string.Format("INSERT INTO {0}({1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}) VALUES({9}, {10}, {11},{12},{13},{14},{15},{16})",
            //    DbConst.MSGLOG_TABLE_NAME,
            //    DbConst.MSGLOG_COLUMN_TIME,
            //    DbConst.MSGLOG_COLUMN_DIRECTION,
            //    DbConst.MSGLOG_COLUMN_ADDRESS,
            //    DbConst.MSGLOG_COLUMN_CONTROL,
            //    DbConst.MSGLOG_COLUMN_STATUS,
            //    DbConst.MSGLOG_COLUMN_COMMAND,
            //    DbConst.MSGLOG_COLUMN_MESSAGE,
            //    DbConst.MSGLOG_COLUMN_TOKEN,
            //    DbConst.MSGLOG_PARAM_TIME,
            //    DbConst.MSGLOG_PARAM_DIRECTION,
            //    DbConst.MSGLOG_PARAM_ADDRESS,
            //    DbConst.MSGLOG_PARAM_CONTROL,
            //    DbConst.MSGLOG_PARAM_STATUS,
            //    DbConst.MSGLOG_PARAM_COMMAND,
            //    DbConst.MSGLOG_PARAM_MESSAGE,
            //    DbConst.MSGLOG_PARAM_TOKEN);

            //var result = mConn.Execute(sqlString, new
            //{
            //    @time = msgLog.Time,
            //    @direction = msgLog.Direction,
            //    @address = msgLog.Address,
            //    @control = msgLog.Control,
            //    @status = msgLog.Status,
            //    @command = msgLog.Command,
            //    @message = msgLog.Message,
            //    @token = msgLog.Token
            //});

            //return result > 0;
            return true;
        }

        public IEnumerable<LogGroup> QueryExecResult()
        {
            string querystring = $"SELECT [{DbConst.MSGLOG_COLUMN_TOKEN}], COUNT(*) AS [{DbConst.MSGLOG_AS_MSGCOUNT}], MIN([{DbConst.MSGLOG_COLUMN_TIME}]) AS [{DbConst.MSGLOG_AS_STARTTIME}] FROM {DbConst.MSGLOG_TABLE_NAME} GROUP BY {DbConst.MSGLOG_COLUMN_TOKEN}";
            var result = mConn.Query<LogGroup>(querystring);

            return result.OrderBy(t => t.StartTime);
        }

        public PageData<T> GetPageData<T>(PageCriteria criteria) where T : BindableBase
        {
            var p = new DynamicParameters();
            string proName = DbConst.PROC_PAGE_NAME;
            p.Add(DbConst.PROC_PAGE_PARAM_TABLENAME, criteria.TableName);
            p.Add(DbConst.PROC_PAGE_PARAM_PRIMARYKEY, criteria.PrimaryKey);
            p.Add(DbConst.PROC_PAGE_PARAM_FIELDS, criteria.Fields);
            p.Add(DbConst.PROC_PAGE_PARAM_CONDITION, criteria.Condition);
            p.Add(DbConst.PROC_PAGE_PARAM_CURRENTPAGE, criteria.CurrentPage);
            p.Add(DbConst.PROC_PAGE_PARAM_PAGESIZE, criteria.PageSize);
            p.Add(DbConst.PROC_PAGE_PARAM_SORT, criteria.Sort);
            p.Add(DbConst.PROC_PAGE_PARAM_RECORDCOUNT, dbType: DbType.Int32, direction: ParameterDirection.Output);

            PageData<T> pageData = new PageData<T>
            {
                Items = new ObservableCollection<T>(mConn.Query<T>(proName, p, commandType: CommandType.StoredProcedure)),
                TotalNum = p.Get<int>(DbConst.PROC_PAGE_PARAM_RECORDCOUNT)
            };

            pageData.TotalPageCount = Convert.ToInt32(Math.Ceiling(pageData.TotalNum * 1.0 / criteria.PageSize));
            pageData.CurrentPage = criteria.CurrentPage > pageData.TotalPageCount ? pageData.TotalPageCount : criteria.CurrentPage;
            return pageData;
        }

        public IEnumerable<LogContent> QueryLogContents(DateTime tmStart, DateTime tmEnd, string address, string param)
        {
            string querystring = $"SELECT * FROM {DbConst.MSGLOG_TABLE_NAME} WHERE [Time] <= '{tmEnd}' and [Time] >= '{tmStart}'";

            if (!string.IsNullOrEmpty(address))
                querystring += $" and [Address] = '{param}'";

            if (!string.IsNullOrEmpty(param))
                querystring += $" and [Details] like '%{param}%'";
            //if (!string.IsNullOrEmpty(sampleId))
            //{
            //    querystring += $" AND [{DbConst.MSGLOG_COLUMN_MESSAGE}] like CONCAT('%',{DbConst.MSGLOG_PARAM_SAMPLEID},'%')";
            //}

            var result = mConn.Query<LogContent>(querystring, new
            {
                tmStart,
                tmEnd,
                address,
                param
            });

            return result;
        }

        public IEnumerable<LogContent> QueryLogContents()
        {
            string querystring = $"SELECT * FROM {DbConst.MSGLOG_TABLE_NAME}";

            var result = mConn.Query<LogContent>(querystring, null, null, false, 10000);

            return result;
        }

        public void DeleteLog(string token)
        {
            string sql = $"DELETE [{DbConst.MSGLOG_TABLE_NAME}] WHERE [{DbConst.MSGLOG_COLUMN_TOKEN}] = '{token}' ";
            mConn.Execute(sql, null, null, 1000);
        }

        public void ClearDB()
        {
            string sql = $"DELETE [{DbConst.MSGLOG_TABLE_NAME}] ";
            mConn.Execute(sql, null, null, 1000);
        }

        public void BackupDb(string path, string name)
        {
            string sql = $"BACKUP DATABASE {DbConst.DB_NAME} TO DISK = '{path}{name}' ";
            mConn.Execute(sql, null, null, 1000);
        }

        public DateTime GetStartTime(string token)
        {
            string sql = $"SELECT top(1) [{DbConst.MSGLOG_COLUMN_TIME}] FROM {DbConst.MSGLOG_TABLE_NAME} WHERE [{DbConst.MSGLOG_COLUMN_TOKEN}]={DbConst.MSGLOG_PARAM_TOKEN} ORDER BY [{DbConst.MSGLOG_COLUMN_TIME}] ASC";

            try
            {
                var result = mConn.QuerySingle<DateTime>(sql, new
                {
                    token
                });

                return result;
            }
            catch (Exception ex)
            {
                //var logger = LogManager.GetLogger(LoggerConst.LOGGER_NAME_SYS);
                //logger.Info("GetStartTime return 0 rows");
                return new DateTime();
            }
        }

        public DateTime GetEndTime(string token)
        {
            string sql = $"SELECT top(1) [{DbConst.MSGLOG_COLUMN_TIME}] FROM {DbConst.MSGLOG_TABLE_NAME} WHERE [{DbConst.MSGLOG_COLUMN_TOKEN}]={DbConst.MSGLOG_PARAM_TOKEN} ORDER BY [{DbConst.MSGLOG_COLUMN_TIME}] DESC";

            try
            {
                var result = mConn.QuerySingle<DateTime>(sql, new
                {
                    token
                });

                return result;
            }
            catch (Exception ex)
            {
                //var logger = LogManager.GetLogger(LoggerConst.LOGGER_NAME_SYS);
                //logger.Info("GetEndTime return 0 rows");
                return new DateTime();
            }
        }


    }
}
