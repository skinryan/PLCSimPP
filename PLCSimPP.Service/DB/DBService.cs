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
using log4net.Core;

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

        /// <summary>
        /// constructor
        /// </summary>
        private DBService()
        {
            IConfigService configServ = ServiceLocator.Current.GetInstance<IConfigService>();
            var config = configServ.ReadSysConfig();
            mConn = new SqlConnection(config.ConnectionString);
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="connString"></param>
        public DBService(string connString)
        {
            mConn = new SqlConnection(connString);
        }

        /// <summary>
        /// get data by page
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="criteria"></param>
        /// <returns></returns>
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

        /// <summary>
        /// query log content
        /// </summary>
        /// <param name="tmStart">start time</param>
        /// <param name="tmEnd">end time</param>
        /// <param name="address">unit address</param>
        /// <param name="param">param</param>
        /// <returns></returns>
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

        /// <summary>
        /// query log content
        /// </summary>
        /// <returns></returns>
        public IEnumerable<LogContent> QueryLogContents()
        {
            string querystring = $"SELECT * FROM {DbConst.MSGLOG_TABLE_NAME}";

            var result = mConn.Query<LogContent>(querystring, null, null, false, 10000);

            return result;
        }

        /// <summary>
        /// clear table
        /// </summary>
        /// <returns></returns>
        public bool TruncateTable()
        {
            string querystring = $"TRUNCATE TABLE {DbConst.MSGLOG_TABLE_NAME}";

            var result = mConn.Execute(querystring);

            return result > 1;
        }

        /// <summary>
        /// get first log time
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
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
                
                return new DateTime();
            }
        }

        /// <summary>
        /// get last log time
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
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
                return new DateTime();
            }
        }


    }
}
