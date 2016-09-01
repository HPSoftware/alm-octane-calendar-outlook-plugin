using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using System.IO;

namespace Hpe.Nga.Api.UI.Core.Configuration
{
    public class ConfigurationPersistService
    {
        private JavaScriptSerializer m_jsonSerializer;


        public ConfigurationPersistService()
        {
            m_jsonSerializer = new JavaScriptSerializer();
            m_jsonSerializer.RegisterConverters(new JavaScriptConverter[] { new ConfigurationJsonConverter() });

            //Set default values
            ConfigurationFilePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\HPE\NGA\";
            ConfigurationFileName = "Configuration.json";
        }

        public String ConfigurationFileName { get; set; }

        public String ConfigurationFilePath { get; set; }

        public void Save(LoginConfiguration configuration)
        {
            String data = m_jsonSerializer.Serialize(configuration);
            File.WriteAllText(GetFullPath(), data);
        }

        public T Load<T>() 
            where T:BaseConfiguration
        {
            String path = GetFullPath();
            if (File.Exists(path))
            {
                String data = File.ReadAllText(path);
                T conf = m_jsonSerializer.Deserialize<T>(data);
                return conf;
            }
            return null;

        }

        private String GetFullPath()
        {
            if (!Directory.Exists(ConfigurationFilePath))
            {
                Directory.CreateDirectory(ConfigurationFilePath);
            }

            String path = ConfigurationFilePath + ConfigurationFileName;

            return path;
        }
    }
}
