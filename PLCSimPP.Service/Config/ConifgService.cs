using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PLCSimPP.Comm.Configuration;
using PLCSimPP.Comm.Interfaces;
using PLCSimPP.Comm.Interfaces.Services;
using PLCSimPP.Comm.Models;

namespace PLCSimPP.Service.Config
{
    public class ConifgService : IConfigService
    {
        private readonly ILogService mLogger;

        public ConifgService(ILogService logger)
        {
            mLogger = logger;
        }

        public IEnumerable<IUnit> ReadSiteMap()
        {
            try
            {
                var mSitemapPath = AppConfig.Configuration["SiteMapPath"];
                using (FileStream fs = new FileStream(mSitemapPath, FileMode.Open))
                {
                    using (StreamReader sr = new StreamReader(fs))
                    {
                        string fileContent = sr.ReadToEnd();

                        return XmlConverter.DeserializeIUnit(fileContent);
                    }
                }
            }
            catch (Exception ex)
            {
                mLogger.LogSys("Invoke ReadSiteMap() method error.", ex);
                return new List<IUnit>();
            }
        }

        public SystemInfo ReadSysConfig()
        {
            try
            {
                var path = AppDomain.CurrentDomain.BaseDirectory + "\\appsettings.json";
                string josnString = File.ReadAllText(path, Encoding.Default);
                SystemInfo sys = JsonConvert.DeserializeObject<SystemInfo>(josnString);

                return sys;
            }
            catch (Exception ex)
            {
                var result = new SystemInfo();
                result.MsgSendInterval = 100;

                result.SiteMapPath = "./Layout/Setting1.txt";
                return result;
            }

        }

        public void SaveSysConfig(SystemInfo info)
        {
            //AppConfig.Configuration["System:ReceiveInterval"] = info.MsgReceiveInterval.ToString();
            //AppConfig.Configuration["System:SiteMapPath"] = info.SiteMapPath;

            var path = AppDomain.CurrentDomain.BaseDirectory + "\\appsettings.json";
            string josnString = File.ReadAllText(path, Encoding.Default);

            SystemInfo sys = JsonConvert.DeserializeObject<SystemInfo>(josnString);
            sys.MsgSendInterval = info.MsgSendInterval;
            sys.SiteMapPath = info.SiteMapPath;
            sys.DcSimLocation = info.DcSimLocation;
            sys.DcInstruments = info.DcInstruments;
            sys.DxCSimLocation = info.DxCSimLocation;
            sys.DxCInstruments = info.DxCInstruments;

            var convertString = JsonConvert.SerializeObject(sys);
            File.WriteAllText(path, convertString, Encoding.UTF8);
        }

        public void SaveSiteMap(string path, IEnumerable<IUnit> unitCollection)
        {
            var dir = path.Substring(0, path.LastIndexOf('\\'));

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            if (File.Exists(path))
            {
                File.Delete(path);
            }

            FileInfo myFile = new FileInfo(path);

            using (StreamWriter sw = myFile.CreateText())
            {
                var content = XmlConverter.SerializeIUnit(unitCollection);
                sw.Write(content);
            }
        }
    }
}
