using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carrotcord_API.Carrotcord.API
{
    public class BitWiseTest
    {
        [Flags]
        public enum test
        {
            None = 0x00,
            Durable = 0x01,
            // messages are saved to disk in case messaging crashes (value is 2)
            Persistent = 0x02,
            // messages are buffered at send/receive point so not blocking (value is 4)
            Buffered = 0x04
        }

        public static void getFlag(test value)
        {
            if(value.HasFlag(test.Durable))
            {
                Console.WriteLine("durable");
            }
            if(value.HasFlag(test.Persistent))
            {
                Console.WriteLine("persistent");
            }
        }

        public static test getBit(test test1, test test2)
        {
            return test1 | test2;
        }

    }
}
