using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commander.Classes {
    public class CommandCmdHelp : Command {

        public override string Name {
            get {
                return "help";
            }
        }

        public override string Description {
            get {
                return "Shows command information.";
            }
        }

        public override Parameter[] Parameters {
            get {
                return new Parameter[] {
                    Parameter.Parse("cmd", null, true)
                };
            }
        }

        public override int MinArguments {
            get {
                return 0;
            }
        }

        public override bool Run(string[] args = null) {

            if (args != null && args.Length == 1) {

                // TODO: Check if the command is within the list.
                if (Program.HasCommand(args[0]));
                var cmd = Program.GetCommand(args[0]);
                Console.WriteLine(cmd.Name + ": " + cmd.ToString());

                return true;

            } else {

                for (var i = 0; i < Program.listCommands.Count; i++) {

                    var cmd = Program.listCommands[i];
                    Run(new string[] { cmd.Name });

                }

                return true;

            }

            return false;

        }

        public override int MaxArguments {
            get {
                return 1;
            }
        }

    }
}
