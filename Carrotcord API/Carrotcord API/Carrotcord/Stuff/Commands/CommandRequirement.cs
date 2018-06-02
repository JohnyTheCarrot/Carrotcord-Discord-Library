using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carrotcord_API.Carrotcord.Stuff.Commands
{
    public class CommandRequirement
    {
        public int ID { get; internal set; }
        public string reason = "No reason provided";
        
        public virtual bool checkCondition(CommandContext context) { return true; }
    }
}
