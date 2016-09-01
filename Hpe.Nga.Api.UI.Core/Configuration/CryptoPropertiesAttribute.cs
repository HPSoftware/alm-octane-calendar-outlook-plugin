using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hpe.Nga.Api.UI.Core.Configuration
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class CryptoPropertiesAttribute : Attribute
    {
        public String[] Names { get; set; }

        public CryptoPropertiesAttribute(params  String[] names)
        {
            this.Names = names;
        }
    }



}
