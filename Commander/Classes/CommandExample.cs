using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commander.Classes {

    public class CommandExample : Command {

        public override void Run(string[] args = null) {
            if (args == null) {
                Console.WriteLine("This is an example, bitch!");
            } else if (args.Length == 1) {
                Console.WriteLine("An argument was passed!");
            }
        }

        public override int MaxArguments {
            get {
                return 1;
            }
        }

        public override int MinArguments {
            get {
                return 0;
            }
        }

        public override Parameter[] Parameters {
            get {
                return new Parameter[] {
                    Parameter.Parse("arg1", null, true)
                };
            }
        }

        public override string Name {
            get {
                return "example";
            }
        }

        public override string Description {
            get {
                return "This is just an example command.";
            }
        }
    }

}
