using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Commander.Classes;

namespace Commander
{

    

    

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
