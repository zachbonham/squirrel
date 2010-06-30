using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using MongoDB.Driver;

using squirrel.contracts;

namespace squirrel
{
    // Start the service and browse to http://<machine_name>:<port>/Service1/help to view the service's generated help page
    // NOTE: By default, a new instance of the service is created for each call; change the InstanceContextMode to Single if you want
    // a single instance of the service to process all calls.	
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    // NOTE: If the service is renamed, remember to update the global.asax.cs file
    public class ConfigurationService
    {

        [WebGet(UriTemplate = "{account}/{table}")]
        public List<Nut> GetCollection(string account, string table)
        {

            List<Nut> nuts = new List<Nut>();


            Mongo mongo = new Mongo();
            mongo.Connect();

            var db = mongo.GetDatabase(WellKnownDb.AppConfiguration);

            var collection = db.GetCollection(table);

            var query = new Document();

            query["account"] = account;


            var querystring = WebOperationContext.Current.IncomingRequest.UriTemplateMatch.QueryParameters;

            foreach (string k in querystring.AllKeys)
            {

                var t = querystring[k];

                if (k == "value")
                {
                    query["value"] = t as string;
                }
                else
                {
                    query[k] = querystring[k];

                }
            }


            var result = collection.Find(query);

        
            foreach (var r in result.Documents)
            {

                var nut = new Nut { Database = WellKnownDb.AppConfiguration, Table = table, Key = r["name"] as string, Properties = new Dictionary<string, string>() };

                nut.Value = r["value"] as string;
                nut.Uri = string.Format("/{0}/{1}/{2}", account, table, r["name"]);

                foreach (var j in r.Keys)
                {
                    nut.Properties.Add(j, r[j] as string);
                }

                nuts.Add(nut);
            }

            return nuts; 

        }

        [WebInvoke(UriTemplate = "{account}/{table}", Method = "POST")]
        public void Create(string account, string table, Nut instance)
        {

            Mongo mongo = new Mongo();
            mongo.Connect();

            var db = mongo.GetDatabase(WellKnownDb.AppConfiguration);

            var collection = db.GetCollection(table);
            
            var doc = new Document();

            doc["account"] = account;
            doc["name"] = instance.Key;
            doc["value"] = instance.Value;


            if (instance.Properties != null)
            {
                foreach (var k in instance.Properties.Keys)
                {
                    doc.Add(k, instance.Properties[k]);
                }
            }

            bool alreadyExists = collection.FindOne(doc) != null;

            if (alreadyExists)
            {
                throw new WebFaultException<string>("Nut already exists", System.Net.HttpStatusCode.BadRequest);
            }
            else
            {
                collection.Insert(doc);
            }

            
            WebOperationContext.Current.OutgoingResponse.SetStatusAsCreated(CreateUri(account, table, instance));
            
        }

        private Uri CreateUri(string account, string table, Nut instance)
        {
            string t = string.Format("{0}/{1}/{2}/{3}", WebOperationContext.Current.IncomingRequest.UriTemplateMatch.BaseUri, account, table, instance.Key);

           
            return new Uri(t);
        }

        [WebGet(UriTemplate = @"{account}/{table}/{name}")]
        public Nut Get(string account, string table, string name)
        {

            Mongo mongo = new Mongo();
            mongo.Connect();

            var db = mongo.GetDatabase(WellKnownDb.AppConfiguration);

            var collection = db.GetCollection(table);

            var query = new Document();

            query["name"] = name;
            query["account"] = account;


            var querystring = WebOperationContext.Current.IncomingRequest.UriTemplateMatch.QueryParameters;

            foreach (string k in querystring.AllKeys)
            {

                var t = querystring[k];

                if (k == "value")
                {
                    query["value"] = t as string;
                }
                else
                {
                    query[k] = querystring[k];
                    
                }
            }

            
            var result = collection.FindOne(query);

            Nut nut = null;

            if (result != null)
            {

                nut = new Nut { Database = WellKnownDb.AppConfiguration, Table = table, Key = name, Properties = new Dictionary<string,string>() };

                nut.Value = result["value"] as string;
                nut.Uri = string.Format("/{0}/{1}/{2}", account, table, name);

                foreach (var j in result.Keys)
                {
                    nut.Properties.Add(j, result[j] as string);
                }
                                
            }
            else
            {
                throw new WebFaultException<string>(string.Format("{0}/{1} not found", table, name), System.Net.HttpStatusCode.NotFound);
            }

            return nut; 
        }
        
        [WebInvoke(UriTemplate = "{account}/{table}/{name}", Method = "PUT")]
        public SampleItem Update(string account, string table, string name, Nut instance)
        {
            // TODO: Update the given instance of SampleItem in the collection
            throw new NotImplementedException();
        }


       
        [WebInvoke(UriTemplate = "{account}/{table}/{name}", Method = "DELETE")]
        public void Delete(string account, string table, string name)
        {
            // TODO: Remove the instance of SampleItem with the given id from the collection
            throw new NotImplementedException();
        }

    }
}
