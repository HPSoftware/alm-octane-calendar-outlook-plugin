// (c) Copyright 2016 Hewlett Packard Enterprise Development LP

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.

// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS,

// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.

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

        public void Save(Configuration configuration)
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
