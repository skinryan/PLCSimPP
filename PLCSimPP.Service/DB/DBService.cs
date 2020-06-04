using CommonServiceLocator;
using Dapper;
using BCI.PLCSimPP.Comm.Interfaces.Services;
using BCI.PLCSimPP.Service.Log;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCI.PLCSimPP.Service.DB
{
    public class DBService : IDisposable
    {
        private IDbConnection mConn;

        private static DBService sCurrentDb;

        /// <summary>
        /// Singleton
        /// </summary>
        public static DBService Current
        {
            get
            {
                if (sCurrentDb == null)
                    DBService.sCurrentDb = new DBService();
                return DBService.sCurrentDb;
            }
        }

        public void Dispose()
        {
            if (mConn == null)
                return;

            mConn.Dispose();
            mConn = null;
        }

        private DBService()
        {
            IConfigService configServ = ServiceLocator.Current.GetInstance<IConfigService>();
            var config = configServ.ReadSysConfig();
            mConn = new SqlConnection(config.ConnectionString);
        }

        public DBService(string connString)
        {
            mConn = new SqlConnection(connString);
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
            string querystring = $"SELECT * FROM [{DbConst.MSGLOG_TABLE_NAME}] WHERE [Time] <= '{tmEnd}' and [Time] >= '{tmStart}'";

            if (!string.IsNullOrEmpty(address))
                querystring += $" and [Address] = '{param}'";

            if (!string.IsNullOrEmpty(param))
                querystring += $" and [Details] like '%{param}%'";

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

        public bool TruncateTable()
        {
            string querystring = $"TRUNCATE TABLE {DbConst.MSGLOG_TABLE_NAME}";

            var result = mConn.Execute(querystring);

            return result > 1;
        }

        public DateTime GetStartTime(string token)
        {
            string sql = $"SELECT top(1) [{DbConst.MSGLOG_COLUMN_TIME}] FROM [{DbConst.MSGLOG_TABLE_NAME}] WHERE [{DbConst.MSGLOG_COLUMN_TOKEN}]={DbConst.MSGLOG_PARAM_TOKEN} ORDER BY [{DbConst.MSGLOG_COLUMN_TIME}] ASC";

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
            string sql = $"SELECT top(1) [{DbConst.MSGLOG_COLUMN_TIME}] FROM [{DbConst.MSGLOG_TABLE_NAME}] WHERE [{DbConst.MSGLOG_COLUMN_TOKEN}]={DbConst.MSGLOG_PARAM_TOKEN} ORDER BY [{DbConst.MSGLOG_COLUMN_TIME}] DESC";

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
