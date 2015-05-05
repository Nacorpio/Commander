using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commander.Classes {
    public abstract class Command {

        public class Parameter {

            /// <summary>
            /// Returns the name of this parameter.
            /// </summary>
            public string Name {
                get;
                set;
            }

            /// <summary>
            /// Returns the default value of this parameter.<br>
            /// Null, if there isn't a default value.
            /// </summary>
            public object DefaultValue {
                get;
                set;
            }

            /// <summary>
            /// Returns whether this parameter is optional.
            /// </summary>
            public bool Optional {
                get;
                set;
            }

            /// <summary>
            /// Returns whether this parameter is required.
            /// </summary>
            public bool Required {
                get;
                set;
            }

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
        public abstract string Name {
            get;
        }

        /// <summary>
        /// Returns the description of this command.
        /// </summary>
        public abstract string Description {
            get;
        }

        /// <summary>
        /// Returns the parameters of this command.
        /// </summary>
        /// <returns></returns>
        public abstract Parameter[] Parameters {
            get;
        }

        /// <summary>
        /// Returns the minimum amount of arguments that can be passed into this command.
        /// </summary>
        /// <returns></returns>
        public abstract int MinArguments {
            get;
        }

        /// <summary>
        /// Returns the maximum amount of arguments that can be passed into this command.
        /// </summary>
        /// <returns></returns>
        public abstract int MaxArguments {
            get;
        }

        /// <summary>
        /// Runs this command with the specified
        /// </summary>
        /// <param name="args"></param>
        public abstract bool Run(string[] args = null);

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

            return value + " // " + Description;

        }

    }
}
