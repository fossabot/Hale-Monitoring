using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace Hale.Core.Utils
{
    public class IPAdressYamlTypeConverter : IYamlTypeConverter
    {
        public bool Accepts(Type type) 
            => type == typeof(IPAddress);

        public object ReadYaml(IParser parser, Type type)
        {
            var ip = IPAddress.Parse(((Scalar)(parser.Current)).Value);
            parser.MoveNext();
            return ip;
        }

        public void WriteYaml(IEmitter emitter, object value, Type type)
        {
            var val = value as IPAddress;
            emitter.Emit(new Scalar(val?.ToString() ?? "0.0.0.0"));
        }
    }
}
