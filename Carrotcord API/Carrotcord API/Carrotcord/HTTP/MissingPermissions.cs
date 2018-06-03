using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carrotcord_API.Carrotcord.Exceptions
{
    public class MissingPermissions : Exception
    {
        public MissingPermissions(string message) : base(message)
        {
        }
    }
}
