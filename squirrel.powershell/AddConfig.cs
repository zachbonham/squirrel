using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management.Automation;
using squirrel.contracts;


namespace squirrel.powershell
{
    [Cmdlet("Add", "Config")]
    public class AddConfig : PSCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "The uri of the configration resource")]
        public string Uri { get; set; }

        [Parameter(Mandatory=true, HelpMessage="This value of the config resource")]
        public string Value { get; set; }

        protected override void ProcessRecord()
        {

            try
            {

                var uri = Uri;

                var tokens = uri.Split('/');

                var account = tokens[1];
                var table = tokens[0];
                var name = tokens[2];


                var squirrel = new SquirrelClient(new Uri("http://localhost/squirrel"), account);
                var nut = new Nut() { Key = name, Table = table, Value = Value };

                squirrel.Add(nut);
                WriteObject("Nut added");
            }
            catch (Exception eax)
            {
                WriteError(new ErrorRecord(eax, "", ErrorCategory.InvalidOperation, this));
            }

        }
    }
}
