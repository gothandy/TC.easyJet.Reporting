using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.IO;

namespace Config
{
    public class UserConfig
    {
        private Configuration exeConfig;
        private Configuration userConfig;

        public UserConfig(string fileName)
        {
            exeConfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            var file = new FileInfo(exeConfig.FilePath);
            var folder = file.Directory.Parent.Parent;


            var userFilePath = String.Concat(folder.FullName, "\\", fileName);

            ExeConfigurationFileMap configMap = new ExeConfigurationFileMap();

            configMap.ExeConfigFilename = userFilePath;

            userConfig = ConfigurationManager.OpenMappedExeConfiguration(configMap, ConfigurationUserLevel.None);
        }

        public string GetUserIfNull(string key)
        {
            var exeSetting = exeConfig.AppSettings.Settings[key];

            if (exeSetting != null) return exeSetting.Value;

            var userSetting = userConfig.AppSettings.Settings[key];

            return userSetting.Value;
        }
    }
}
