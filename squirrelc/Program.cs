using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Http;

using squirrel.contracts;


namespace squirrelc
{

    
    class Program
    {
        static void Main(string[] args)
        {




            var squirrel = new SquirrelClient(new Uri("http://localhost/squirrel"), "crm");

            Nut nut = new Nut() { Table="ConnectionStrings",  Key = "intranetdb", Value = "this is my development intranet db connectionstring", Properties = new Dictionary<string,string>() };

            nut.Properties.Add("environment", "Development");
            squirrel.Add(nut);

            var cs = squirrel.Get("ConnectionStrings", "aem");

            Console.WriteLine(cs.Value);

            


            
        }
    }
}
