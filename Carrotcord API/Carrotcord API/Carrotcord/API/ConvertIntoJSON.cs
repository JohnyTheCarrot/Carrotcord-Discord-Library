using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carrotcord_API.Carrotcord.API
{
    class ConvertIntoJSON
    {
        public static string Convert(Dictionary<string, object> objects)
        {
            string JSON = "{";
            foreach(string obj in objects.Keys)
            {
                object value;
                if(objects.TryGetValue(obj, out value))
                {
                    JSON += $"\"{obj}\":\"{value}\",\n";
                }
            }
            JSON = JSON.Substring(JSON.Length - 3);
            return JSON += "}";
        }
    }
}
