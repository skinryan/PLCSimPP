using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace BCI.PLCSimPP.Comm.Configuration
{
    public class AppConfig
    {
        public const string SETTING_FILE_NAME = "appsettings.json";
        public static IConfiguration Configuration { get; set; }
        static AppConfig()
        {
            //ReloadOnChange = true 当appsettings.json被修改时重新加载            
            Configuration = new ConfigurationBuilder()
            .Add(new JsonConfigurationSource { Path = SETTING_FILE_NAME, ReloadOnChange = true })
            .Build();
        }
    }
}
