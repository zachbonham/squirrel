using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Http;
using System.Runtime.Serialization;
using System.IO;
using System.Xml.Linq;


namespace squirrel.contracts
{
    public class SquirrelClient
    {
        public string Account { get; set; }
        public Uri Address { get; set; }
        
        public SquirrelClient(Uri address, string account)
        {
            Address = address;
            Account = account;
        }


        public Nut Get(string table, string key)
        {

            Nut response = null;

            string uri = string.Format("{0}/{1}/{2}/{3}", Address.AbsoluteUri.ToLower(), Account.ToLower(), table.ToLower(), key.ToLower());


            var client = new HttpClient(uri);

            
            HttpResponseMessage r = client.Get();
            r.EnsureStatusIsSuccessful();

            //if (r.StatusCode == System.Net.HttpStatusCode.OK)
            {
                response = r.Content.ReadAsDataContract<Nut>();
            }
           
            return response;

        }

        public void Add(Nut nut)
        {
            Nut response = null;

            string uri = string.Format("{0}/{1}/{2}", Address.AbsoluteUri.ToLower(), Account.ToLower(), nut.Table.ToLower());


            var client = new HttpClient(uri);

            MemoryStream ms = new MemoryStream();

            DataContractSerializer s = new DataContractSerializer(typeof(Nut));
            s.WriteObject(ms, nut);
           
            ms.Position = 0;
            HttpResponseMessage r = client.Post(uri, HttpContent.Create(ms.ToArray(), "application/xml"));

            ms.Close();

            r.EnsureStatusIsSuccessful();
   
            if (r.StatusCode == System.Net.HttpStatusCode.OK)
            {
                nut.Uri = uri;
                
            }
            
        }

    }
}
