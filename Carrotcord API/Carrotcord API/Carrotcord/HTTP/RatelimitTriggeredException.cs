using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carrotcord_API.Carrotcord.HTTP
{
    public class RatelimitTriggeredException : Exception
    {
        public RatelimitTriggeredException()
        {
        }

        public RatelimitTriggeredException(string message) : base(message)
        {
        }
    }
}
