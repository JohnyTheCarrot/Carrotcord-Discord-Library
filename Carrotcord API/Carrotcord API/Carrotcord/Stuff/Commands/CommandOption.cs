using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carrotcord_API.Carrotcord.Stuff.Commands
{
    public class CommandOption
    {
        [Flags]
        public enum Option
        {
            DMONLY,
            NSFWONLY,
            GUILDONLY,
            EVERYWHERE
        }

        public static Option calculatePermission(Option[] options)
        {
            //Permission permission = permissions[0] | permissions[1] | permissions[2];
            Option option = options[0];
            for (int i = 0; i < options.Length; i++)
            {
                if (i != 0)
                {
                    option = option | options[i];
                }
            }
            return option;
        }

        public static List<Option> getPermissions(Option value)
        {
            List<Option> options = new List<Option>();
            foreach (Option option in Enum.GetValues(typeof(Option)))
            {
                if (value.HasFlag(option)) options.Add(option);
            }
            return options;
        }

        public CommandOption()
        {

        }

    }
}
