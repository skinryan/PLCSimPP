using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using PLCSimPP.Comm.Configuration;
using PLCSimPP.Comm.Interfaces;
using PLCSimPP.Comm.Interfaces.Services;
using PLCSimPP.Comm.Models;

namespace PLCSimPP.Service.Config
{
    public class ConifgService : IConfigService
    {
        private string mSitemapPath;
        private int mSysCfgPath;

        public ConifgService()
        {
            mSitemapPath = AppConfig.Configuration["Layout:Path"];
        }

        public IEnumerable<T> ReadFile<T>() where T : UnitBase
        {
            using (FileStream fs = new FileStream(mSitemapPath, FileMode.Open))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    string fileContent = sr.ReadToEnd();

                    return XmlConverter.Deserialize<List<T>>(fileContent);
                }
            }
        }

        public List<IUnit> ReadSiteMap()
        {
            throw new NotImplementedException();
        }

        public void SaveSiteMap()
        {
            throw new NotImplementedException();
        }

        public void Write<T>(List<T> unitCollection) where T : UnitBase
        {
            var path = mSitemapPath.Substring(0, mSitemapPath.LastIndexOf('/'));

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            using (FileStream fs = new FileStream(mSitemapPath, FileMode.OpenOrCreate))
            {
                using (StreamWriter sr = new StreamWriter(fs))
                {
                    var content = XmlConverter.Serialize<List<T>>(unitCollection);
                    sr.Write(sr);
                }
            }
        }

        public void ReadSysConfig()
        {
            throw new System.NotImplementedException();
        }

        public void SaveSysConfig()
        {
            throw new System.NotImplementedException();
        }

       
    }
}
