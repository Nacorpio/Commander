using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Commander
{

    public abstract class Command
    {

        public class Parameter {
            /// <summary>
            /// Returns the name of this parameter.
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// Returns the default value of this parameter.<br>
            /// Null, if there isn't a default value.
            /// </summary>
            public object DefaultValue { get; set; }

            /// <summary>
            /// Returns whether this parameter is optional.
            /// </summary>
            public bool Optional { get; set; }

            public bool Required { get; set; }

            public static Parameter Parse(string name, object defaultValue, bool optional) {

                Parameter par = new Parameter();

                par.Name = name;
                par.DefaultValue = defaultValue;
                par.Optional = optional;

                return par;

            }

            public sealed override string ToString() {
                return Optional ? "[" + Name + "]" : Required ? "(" + Name + ")" : "<" + Name + ">";
            }
        }

        public abstract string Name();

        public abstract Parameter[] Parameters();

        public abstract int MinArguments();

        public abstract int MaxArguments();

        public abstract void Run(string[] args = null);

        public sealed override string ToString() {

            string value = "!" + Name() + " ";

            for (int i = 0; i < Parameters().Length; i++) {

                Parameter e = Parameters()[i];

                if (i < Parameters().Length - 1) {
                    value += e.ToString() + " ";
                } else {
                    value += e.ToString();
                }

            }

            return value;

        }

    }

    public class CommandExample : Command {
        public override void Run(string[] args = null) {
            if (args == null) {
                Console.WriteLine("This is an example, bitch!");
            } else if (args.Length == 1) {
                Console.WriteLine("An argument was passed!");
            }
        }

        public override int MaxArguments() {
            return 1;
        }

        public override int MinArguments() {
            return 1;
        }

        public override Parameter[] Parameters() {
            return new Parameter[] {Parameter.Parse("arg1", null, true)};
        }

        public override string Name() {
            return "example";
        }
    }

    public class CommandCmdHelp : Command {
        public override string Name() {
            return "help";
        }

        public override Parameter[] Parameters() {
            return new Parameter[] {Parameter.Parse("cmd", null, true)};
        }

        public override int MinArguments() {
            return 0;
        }

        public override void Run(string[] args = null) {

            if (args != null && args.Length == 1) {

                // TODO: Check if the command is within the list.
                if (Program.HasCommand(args[0])) {

                    Command cmd = Program.GetCommand(args[0]);
                    Console.WriteLine(cmd.Name() + ": " + cmd.ToString());

                }
               
            } else {

                for (int i = 0; i < Program.commands.Count; i++) {

                    Command cmd = Program.commands[i];
                    Run(new string[] {cmd.Name()});

                }

            }

        }

        public override int MaxArguments() {
            return 1;
        }
    }

    class Program {

        public static List<Command> commands = new List<Command>(); 

        static void Main(string[] args) {

            int loop = 0;

            InitializeCommands();

            while (true) {

                Console.WriteLine("Enter a command: ");
                string input = Console.ReadLine();

                string name = "";
                string[] pars = null;

                input = input.Trim();

                if (input.Contains(" ")) {
                
                    // TODO: There are arguments to take care of.
                    string[] parts = input.Split(' ');

                    name = parts[0].Substring(1, parts[0].Length - 1);

                    if (name.Length == 0 || String.IsNullOrEmpty(name)) {
                        throw new Exception("The name of the command '" + name + "' can not be empty.");
                    }

                    input = input.Remove(0, name.Length + 1).Trim();

                    if (input.Length == 0 || String.IsNullOrEmpty(input)) {
                        throw new Exception("The arguments of the command '" + name + "' can not be empty.");
                    }

                    pars = input.Split(' ');

                } else {

                    name = input.Substring(1, input.Length - 1);
                    pars = null;

                }

                Command cmd = GetCommand(name);
                if (pars != null && cmd.Parameters() != null) {

                    // TODO: We have arguments to care of.
                    if (!(pars.Length >= cmd.MinArguments())) {
                        Console.WriteLine("Too few arguments were passed to this command (" + pars.Length + "/" + cmd.MinArguments() + ").");
                        Console.ReadLine();
                        return;
                    }

                    if (!(pars.Length <= cmd.MaxArguments())) {
                        Console.WriteLine("Too many arguments were passed to this command (" + pars.Length + "/" + cmd.MaxArguments() + ").");
                        Console.ReadLine();
                        return;
                    }

                    cmd.Run(pars);

                } else {

                    // TODO: We don't have any arguments to take care of.
                    if (cmd.MinArguments() == 0)
                        cmd.Run();
                    else
                        Console.WriteLine("The Command.MinArguments has to be set to (0).");

                }

                loop++; // Loops every loop. (duh)

            }

        }

        public static Command GetCommand(string name) {
            for (int i = 0; i < commands.Count; i++) {
                Command e = commands[i];
                if (e.Name() == name)
                    return e;
            }
            return null;
        }

        public static bool HasCommand(string name) {
            return GetCommand(name) != null;
        }

        private static void InitializeCommands() {
            commands.Add(new CommandExample());
            commands.Add(new CommandCmdHelp());
        }

    }

}
