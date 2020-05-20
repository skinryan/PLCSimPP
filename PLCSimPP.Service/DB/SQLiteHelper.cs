using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCSimPP.Service.DB
{
    public class SQLiteHelper
    {
        private string _dbName = "";
        private SQLiteConnection _SQLiteConn = null;
        private SQLiteTransaction _SQLiteTrans = null;
        private bool _IsRunTrans = false;
        private string _SQLiteConnString = null;
        private bool _AutoCommit = false;

        public string SQLiteConnString
        {
            set { this._SQLiteConnString = value; }
            get { return this._SQLiteConnString; }
        }

        public SQLiteHelper(string dbPath)
        {
            this._dbName = dbPath;
            this._SQLiteConnString = "Data Source=" + dbPath;
        }

        /// <summary>
        /// create new db file
        /// </summary>
        /// <param name="dbPath">file path and file name</param>
        /// <returns>return true if success ,otherwise return false</returns>
        static public Boolean NewDbFile(string dbPath)
        {
            try
            {
                SQLiteConnection.CreateFile(dbPath);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Create DB file " + dbPath + " fail：" + ex.Message);
            }
        }


        /// <summary>
        /// create table
        /// </summary>
        /// <param name="dbPath">db path</param>
        /// <param name="tableName">table name</param>
        static public void NewTable(string dbPath)
        {

            SQLiteConnection sqliteConn = new SQLiteConnection("data source=" + dbPath);
            if (sqliteConn.State != System.Data.ConnectionState.Open)
            {
                sqliteConn.Open();
                SQLiteCommand cmd = new SQLiteCommand();
                cmd.Connection = sqliteConn;


                cmd.CommandText = @"DROP TABLE IF EXISTS `MsgLog`;
                                       CREATE TABLE `MsgLog` (
                                      `ID` integer NOT NULL PRIMARY KEY AUTOINCREMENT,
                                      `Time` text,
                                      `Direction` text,
                                      `Address` text,
                                      `Command` text,
                                      `Details` text,
                                      `Token` text); ";
                cmd.ExecuteNonQuery();

            }
            sqliteConn.Close();
        }

        /// <summary>
        /// open db connection
        /// </summary>
        /// <returns></returns>
        public Boolean OpenDb()
        {
            try
            {
                this._SQLiteConn = new SQLiteConnection(this._SQLiteConnString);
                this._SQLiteConn.Open();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Open DB ：" + _dbName + " fail：" + ex.Message);
            }
        }

        /// <summary>
        /// open db connection 
        /// </summary>
        /// <param name="dbPath">target file path</param>
        /// <returns></returns>
        public Boolean OpenDb(string dbPath)
        {
            try
            {
                string sqliteConnString = "Data Source=" + dbPath;

                this._SQLiteConn = new SQLiteConnection(sqliteConnString);
                this._dbName = dbPath;
                this._SQLiteConnString = sqliteConnString;
                this._SQLiteConn.Open();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Open DB：" + dbPath + " fail：" + ex.Message);
            }
        }

        /// <summary>
        /// close db connection
        /// </summary>
        public void CloseDb()
        {
            if (this._SQLiteConn != null && this._SQLiteConn.State != ConnectionState.Closed)
            {
                if (this._IsRunTrans && this._AutoCommit)
                {
                    this.Commit();
                }
                this._SQLiteConn.Close();
                this._SQLiteConn = null;
            }
        }

        /// <summary>
        /// begin transaction
        /// </summary>
        public void BeginTransaction()
        {
            this._SQLiteConn.BeginTransaction();
            this._IsRunTrans = true;
        }

        /// <summary>
        /// begin transaction
        /// </summary>
        /// <param name="isoLevel">transaction lock level</param>
        public void BeginTransaction(IsolationLevel isoLevel)
        {
            this._SQLiteConn.BeginTransaction(isoLevel);
            this._IsRunTrans = true;
        }

        /// <summary>
        /// submit transcation
        /// </summary>
        public void Commit()
        {
            if (this._IsRunTrans)
            {
                this._SQLiteTrans.Commit();
                this._IsRunTrans = false;
            }
        }
    }
}
