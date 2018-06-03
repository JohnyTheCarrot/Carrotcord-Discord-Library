using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carrotcord_API.Carrotcord.HTTP
{
    public class _204NoContent : Exception
    {
        public _204NoContent()
        {
        }

        public _204NoContent(string message) : base(message)
        {
        }
    }
}
