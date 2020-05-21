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
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCSimPP.Service.DB
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
            mConn = new SQLiteConnection(config.SQLiteConnString);
        }

        public DBService(string connString)
        {
            mConn = new SQLiteConnection(connString);
        }

        public PageData<T> GetPageData<T>(PageCriteria criteria) where T : BindableBase
        {
            string querystring = $"SELECT * FROM {criteria.TableName} WHERE {criteria.Condition} LIMIT {criteria.PageSize} OFFSET {criteria.PageSize * (criteria.CurrentPage - 1)}";

            PageData<T> pageData = new PageData<T>()
            {
                Items = new ObservableCollection<T>(mConn.Query<T>(querystring)),
                TotalNum = mConn.ExecuteScalar<int>($"SELECT COUNT(1) FROM {criteria.TableName} WHERE {criteria.Condition}")
            };

            pageData.TotalPageCount = Convert.ToInt32(Math.Ceiling(pageData.TotalNum * 1.0 / criteria.PageSize));
            pageData.CurrentPage = criteria.CurrentPage > pageData.TotalPageCount ? pageData.TotalPageCount : criteria.CurrentPage;
            return pageData;
        }

        public IEnumerable<LogContent> QueryLogContents(DateTime tmStart, DateTime tmEnd, string address, string param)
        {
            string querystring = $"SELECT * FROM {DbConst.MSGLOG_TABLE_NAME} WHERE `Time` <= '{tmEnd.ToString("yyyy-MM-dd HH:mm:ss")}' and `Time` >= '{tmStart.ToString("yyyy-MM-dd HH:mm:ss")}'";

            if (!string.IsNullOrEmpty(address))
                querystring += $" and `Address` = '{param}'";

            if (!string.IsNullOrEmpty(param))
                querystring += $" and `Details` like '%{param}%'";

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

        public DateTime GetStartTime(string token)
        {
            string sql = $"SELECT top(1) `{DbConst.MSGLOG_COLUMN_TIME}` FROM {DbConst.MSGLOG_TABLE_NAME} WHERE `{DbConst.MSGLOG_COLUMN_TOKEN}`={DbConst.MSGLOG_PARAM_TOKEN} ORDER BY `{DbConst.MSGLOG_COLUMN_TIME}` ASC";

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
            string sql = $"SELECT top(1) `{DbConst.MSGLOG_COLUMN_TIME}` FROM {DbConst.MSGLOG_TABLE_NAME} WHERE `{DbConst.MSGLOG_COLUMN_TOKEN}`={DbConst.MSGLOG_PARAM_TOKEN} ORDER BY `{DbConst.MSGLOG_COLUMN_TIME}` DESC";

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
