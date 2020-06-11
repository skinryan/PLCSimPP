﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using BCI.PLCSimPP.Comm.Configuration;
using BCI.PLCSimPP.Comm.Interfaces;
using BCI.PLCSimPP.Comm.Interfaces.Services;
using BCI.PLCSimPP.Comm.Models;

namespace BCI.PLCSimPP.Service.Config
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
                var siteMapPath = AppConfig.Configuration["SiteMapPath"];
                using (FileStream fs = new FileStream(siteMapPath, FileMode.Open))
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
                var path = AppDomain.CurrentDomain.BaseDirectory + "\\"+ AppConfig.SETTING_FILE_NAME;
                string josnString = File.ReadAllText(path, Encoding.Default);
                SystemInfo sys = JsonConvert.DeserializeObject<SystemInfo>(josnString);

                return sys;
            }
            catch (Exception ex)
            {
                var result = new SystemInfo();
                result.SendInterval = 100;

                result.SiteMapPath = "./Layout/Setting1.txt";
                return result;
            }

        }

        public void SaveSysConfig(SystemInfo info)
        {
            //AppConfig.Configuration["System:ReceiveInterval"] = info.MsgReceiveInterval.ToString();
            //AppConfig.Configuration["System:SiteMapPath"] = info.SiteMapPath;

            var path = AppDomain.CurrentDomain.BaseDirectory + "\\" + AppConfig.SETTING_FILE_NAME;
            string josnString = File.ReadAllText(path, Encoding.Default);

            SystemInfo sys = JsonConvert.DeserializeObject<SystemInfo>(josnString);
            sys.SendInterval = info.SendInterval;
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
