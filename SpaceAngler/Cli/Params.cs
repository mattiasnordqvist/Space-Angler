using System;

namespace Cli
{
    public class Params
    {
        private string[] args;

        public Params(string[] args)
        {
            this.args = args;
        }

        internal string Get(string description, string shortcut)
        {
            var index = Array.IndexOf(args, shortcut);
            string value = "";
            if(index > -1)
            {
                value = args[index + 1];
            }
            else
            {
                while(value == "")
                { 
                    Console.WriteLine("Please enter " + description);
                    value = Console.ReadLine();
                }
            }

            Console.WriteLine("Using " + value + " as " + description);
            return value;
        }
    }
}