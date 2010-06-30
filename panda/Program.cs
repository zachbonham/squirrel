using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using squirrel.contracts;


namespace panda
{
    class Program
    {
        static void Main(string[] args)
        {

            if (args.Count() == 0)
            {
                PrintHelp();
            }
            else
            {
                switch(args[0])
                {
                    case "config:add":
                        ConfigAdd(args);
                        break;
                    case "config:get":
                        ConfigGet(args);
                        break;
                    default:
                        Console.WriteLine("command " + args[0] + " not found.");
                        break;
                }
            }
            
        }

        static void ConfigAdd(string[] args)
        {

            var squirrel = new SquirrelClient(new Uri("http://localhost:49622/squirrel"), "crm");

            var nut = new Nut { Table="ConnectionStrings", Key = "test", Value = "<endpoint xmlns='testing'><b>testing</b></endpoint>" };
            squirrel.Add(nut);


        }

        static void ConfigGet(string[] args)
        {

            var uri = args[1];
            var tokens = uri.Split('/');


            var account = tokens[1];
            var table = tokens[0];
            var name = tokens[2];


            var squirrel = new SquirrelClient(new Uri("http://localhost/squirrel"), account);

            var cs = squirrel.Get(table, name);

            if (cs != null)
            {
                Console.WriteLine(cs.Value);
            }
        }

        static void PrintHelp()
        {
            Console.WriteLine("## General Help");
            Console.WriteLine("");
            Console.WriteLine(@"config:add %table%/%account%/%name% [base64 encoded string]   # add a configuration item");
            Console.WriteLine(@"config:get %table%/%account%/%name%?[k=v]&[...]               # get a configuration item");
        }
    }
}
