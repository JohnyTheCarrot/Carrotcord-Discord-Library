using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carrotcord_API.Carrotcord.API
{
    class Payload
    {
        [JsonProperty("op")]
        public int OP;
        [JsonProperty("t", NullValueHandling = NullValueHandling.Ignore)]
        public string type;
        [JsonProperty("s", NullValueHandling = NullValueHandling.Ignore)]
        public int? sequence;
        [JsonProperty("d")]
        public object data;
    }
}
