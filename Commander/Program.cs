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

            /// <summary>
            /// Returns whether this parameter is required.
            /// </summary>
            public bool Required { get; set; }

            public static Parameter Parse(string name, object defaultValue, bool optional) {

                var par = new Parameter() {

                    Name = name,
                    DefaultValue = defaultValue,
                    Optional = optional

                };

                return par;

            }

            public sealed override string ToString() {

                return

                    Optional ?      
                        "[" + Name + "]"

                    : Required ?    
                        "(" + Name + ")"

                    :               
                        "<" + Name + ">"

                    ;

            }

        }

        /// <summary>
        /// Returns the name of this command.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Returns the description of this command.
        /// </summary>
        public abstract string Description { get; }

        /// <summary>
        /// Returns the parameters of this command.
        /// </summary>
        /// <returns></returns>
        public abstract Parameter[] Parameters { get; }

        /// <summary>
        /// Returns the minimum amount of arguments that can be passed into this command.
        /// </summary>
        /// <returns></returns>
        public abstract int MinArguments { get; }

        /// <summary>
        /// Returns the maximum amount of arguments that can be passed into this command.
        /// </summary>
        /// <returns></returns>
        public abstract int MaxArguments { get; }

        /// <summary>
        /// Runs this command with the specified
        /// </summary>
        /// <param name="args"></param>
        public abstract void Run(string[] args = null);

        public sealed override string ToString() {

            string value = "!" + Name + " ";

            for (int i = 0; i < Parameters.Length; i++) {

                Parameter e = Parameters[i];

                if (i < Parameters.Length - 1) {
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

    public class CommandCmdHelp : Command {
        public override string Name {   
            get {
                return "help";
            }
        }

        public override string Description {
            get {
                return "Shows either information about a specific command, or a list of all commands.";
            }
        }

        public override Parameter[] Parameters {
            get {
                return new Parameter[] {Parameter.Parse("cmd", null, true)};
            }        
        }

        public override int MinArguments {
            get {
               return 0; 
            }
        }

        public override void Run(string[] args = null) {

            if (args != null && args.Length == 1) {

                // TODO: Check if the command is within the list.
                if (Program.HasCommand(args[0]));
                    var cmd = Program.GetCommand(args[0]);
                    Console.WriteLine(cmd.Name + ": " + cmd.ToString());
               
            } else {

                for (int i = 0; i < Program.listCommands.Count; i++) {

                    var cmd = Program.listCommands[i];
                    Run(new string[] {cmd.Name});

                }

            }

        }

        public override int MaxArguments {
            get {
               return 1; 
            }       
        }

    }

    class Program {

        public readonly static List<Command> listCommands = new List<Command>(); 

        static void Main(string[] args) {

            int loop = 0;

            InitializeCommands();

            while (true) {

                string cmdInput = Console.ReadLine();
                string name = "";
                string[] pars = null;

                cmdInput = cmdInput.Trim();

                if (cmdInput.Contains(" ")) {
                
                    // TODO: There are arguments to take care of.

                    string[] parts = cmdInput.Split(' ');

                    name = parts[0].Substring(1, parts[0].Length - 1);

                    if (name.Length == 0 || String.IsNullOrEmpty(name)) {
                        throw new Exception("The name of the command '" + name + "' can not be empty.");
                    }

                    cmdInput = cmdInput.Remove(0, name.Length + 1).Trim();

                    if (cmdInput.Length == 0 || String.IsNullOrEmpty(cmdInput)) {
                        throw new Exception("The arguments of the command '" + name + "' can not be empty.");
                    }

                    pars = cmdInput.Split(' ');

                } else {

                    name = cmdInput.Substring(1, cmdInput.Length - 1);
                    pars = null;

                }

                Command cmd = GetCommand(name);
                Console.WriteLine();

                if (pars != null && cmd.Parameters != null) {

                    // TODO: We have arguments to care of.

                    if (!(pars.Length >= cmd.MinArguments)) {

                        Console.WriteLine("Too few arguments were passed to the command '" + cmd.Name + "' (" + pars.Length + "/" + cmd.MinArguments + ").");
                        Console.ReadLine();

                        return;

                    }

                    if (!(pars.Length <= cmd.MaxArguments)) {

                        Console.WriteLine("Too many arguments were passed to the command '" + cmd.Name + "' (" + pars.Length + "/" + cmd.MaxArguments + ").");
                        Console.ReadLine();

                        return;

                    }

                    cmd.Run(pars);

                } else {

                    // TODO: We don't have any arguments to take care of.

                    if (cmd.MinArguments == 0)
                        cmd.Run();
                    else
                        Console.WriteLine("The Command.MinArguments has to be set to (0).");

                }

                Console.WriteLine();
                loop++; // Loops every loop. (duh)

            }

        }

        public static void AddCommand(Command cmd) {
            if (!HasCommand(cmd.Name)) {
                listCommands.Add(cmd);
            } else {
                Console.WriteLine("Internal error: Can not add the specified command (already exists).");  
            }
        }

        public static Command GetCommand(string name) {
            for (var i = 0; i < listCommands.Count; i++) {
                var e = listCommands[i];
                if (e.Name == name)
                    return e;
            }
            return null;
        }

        public static bool HasCommand(string name) {
            return GetCommand(name) != null;
        }

        private static void InitializeCommands() {
            listCommands.Add(new CommandExample());
            listCommands.Add(new CommandCmdHelp());
        }

    }

}
