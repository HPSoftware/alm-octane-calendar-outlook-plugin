using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Script.Serialization;
using Hpe.Nga.Api.UI.Core.Configuration;
using Hpe.Nga.Api.UI.Core.Crypto;

namespace Hpe.Nga.Api.UI.Core.Configuration
{
    public class ConfigurationJsonConverter : JavaScriptConverter
    {
        String PASSWORD = "my encrypted properties";

        public override IEnumerable<Type> SupportedTypes
        {
            get
            {

                HashSet<Type> types = new HashSet<Type>();
                //types.Add(typeof(LoginConfiguration));

                var baseEntityType = typeof(BaseConfiguration);
                IEnumerable<Type> typesEnumeration = Assembly.GetAssembly(baseEntityType).GetTypes().Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(baseEntityType));
                foreach (Type type in typesEnumeration)
                {
                    types.Add(type);
                }


                return types;
            }
        }

        public override IDictionary<string, object> Serialize(object obj, JavaScriptSerializer serializer)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            if (obj == null) return result;
            LoginConfiguration entity = ((LoginConfiguration)obj);
            IDictionary<string, object> properties = entity.GetProperties();

            //encrypt secret properties
            IEnumerable<String> cryptoProperties = GetCryptoProperties(obj.GetType());
            foreach (String property in cryptoProperties)
            {
                if (properties.ContainsKey(property))
                {
                    String value = properties[property].ToString();
                    String encryptedValue = StringCipher.Encrypt(value, PASSWORD);
                    properties[property] = encryptedValue;
                }

            }

            return properties;
        }

        private IEnumerable<String> GetCryptoProperties(Type t)
        {
            CryptoPropertiesAttribute attribute = (CryptoPropertiesAttribute)Attribute.GetCustomAttribute(t, typeof(CryptoPropertiesAttribute));

            if (attribute == null)
            {
                return Enumerable.Empty<String>();
            }
            else
            {
                return attribute.Names;
            }
        }

        public override object Deserialize(IDictionary<string, object> dictionary, Type type, JavaScriptSerializer serializer)
        {
            BaseConfiguration configuration = (BaseConfiguration)Activator.CreateInstance(type);
            configuration.SetProperties(dictionary);

            //decrypt secret properties
            IEnumerable<String> cryptoProperties = GetCryptoProperties(type);
            foreach (String property in cryptoProperties)
            {
                if (configuration.Contains(property))
                {
                    String value = configuration.GetStringValue(property);
                    String decryptedValue = StringCipher.Decrypt(value, PASSWORD);
                    configuration.SetValue(property, decryptedValue);
                }
            }

            return configuration;
        }
    }
}
