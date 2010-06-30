using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management.Automation;
using squirrel.contracts;


namespace squirrel.powershell
{
    [Cmdlet("Get", "Config")]
    public class GetConfig : PSCmdlet
    {

        [Parameter(Mandatory=true, HelpMessage="The uri of the configration resource")]
        public string Uri { get; set; }


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

                var cs = squirrel.Get(table, name);

                if (cs != null)
                {
                    WriteObject(cs.Value);
                }
            }
            catch (Exception eax)
            {            
                WriteError( new ErrorRecord(eax, "", ErrorCategory.InvalidOperation, this));
            }
        }
    }
}
