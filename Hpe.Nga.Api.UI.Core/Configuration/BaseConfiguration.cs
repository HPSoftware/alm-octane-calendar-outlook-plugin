using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hpe.Nga.Api.Core.Services.Core;

namespace Hpe.Nga.Api.UI.Core.Configuration
{
    public class BaseConfiguration : DictionaryBasedEntity
    {
        public BaseConfiguration()
            : base()
        {
        }

        public BaseConfiguration(IDictionary<string, object> properties)
            : base(properties)
        {
        }



    }
}
