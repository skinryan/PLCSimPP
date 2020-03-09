using System;
using System.Collections.Generic;
using System.Text;
using PLCSimPP.Comm.Interfaces.Services;
using PLCSimPP.Communication.Models;

namespace PLCSimPP.Service.DB
{
    public class DbService : IDbService
    {
        public DbService()
        {

        }

        public int Connection
        {
            get => default;
            set
            {
            }
        }

        public List<CmdMsg> QueryMsgByFilter(string orderby)
        {
            return null;
        }

        public void QueryMsgCount()
        {
            throw new System.NotImplementedException();
        }
    }
}
