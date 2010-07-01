using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver;
using squirrel.contracts;

namespace squirrel.businesslogic
{
    public class SquirrelBusinessLogic
    {
        /// <summary>
        /// Create a Nut for a specific application account and in a specific configuration container.
        /// </summary>
        /// <param name="account">The application account we are creating the nut for.</param>
        /// <param name="container">The container we are inserting the nut into (e.g. connectionstrings, endpoints, appconfig, etc).</param>
        /// <param name="nut">Busta nut.</param>
        /// <returns>True if the nut was added and false if it already exists, and therefore was not added/updated.</returns>
        public static bool CreateNut(string account, string container, Nut nut)
        {
            
            bool nutAdded = false;

            Mongo mongo = new Mongo();
            mongo.Connect();

            var db = mongo.GetDatabase(WellKnownDb.AppConfiguration);

            var collection = db.GetCollection(container);

            var doc = new Document();

            doc["account"] = account;
            doc["name"] = nut.Key;
            doc["value"] = nut.Value;


            if (nut.Properties != null)
            {
                foreach (var k in nut.Properties.Keys)
                {
                    doc.Add(k, nut.Properties[k]);
                }
            }


            if (collection.FindOne(doc) == null)
            {
                collection.Insert(doc);
                nutAdded = true;
            }

            return nutAdded;
        }


        public static Nut FindNut(string account, string container, string name, IDictionary<string,string> properties)
        {

            Nut nut = null;


            Mongo mongo = new Mongo();
            mongo.Connect();

            var db = mongo.GetDatabase(WellKnownDb.AppConfiguration);

            var collection = db.GetCollection(container);

            var query = new Document();

            query["name"] = name;
            query["account"] = account;

            foreach (string k in properties.Keys)
            {
                query[k] = properties[k];
            }

       
            var result = collection.FindOne(query);

            if (result != null)
            {

                nut = new Nut { Database = WellKnownDb.AppConfiguration, Table = container, Key = name, Properties = new Dictionary<string, string>() };

                nut.Value = result["value"] as string;
                nut.Uri = string.Format("/{0}/{1}/{2}", account, container, name);

                foreach (var j in result.Keys)
                {
                    nut.Properties.Add(j, result[j] as string);
                }

            }

            return nut;
        }

        public static List<Nut> FindAllInContainer(string account, string container, IDictionary<string,string> properties)
        {

            List<Nut> nuts = new List<Nut>();


            Mongo mongo = new Mongo();
            mongo.Connect();

            var db = mongo.GetDatabase(WellKnownDb.AppConfiguration);

            var collection = db.GetCollection(container);

            var query = new Document();

            query["account"] = account;


            foreach (string k in properties.Keys)
            {

                var t = properties[k];

                if (k == "value")
                {
                    query["value"] = t as string;
                }
                else
                {
                    query[k] = properties[k];

                }
            }


            var result = collection.Find(query);


            foreach (var r in result.Documents)
            {

                var nut = new Nut { Database = WellKnownDb.AppConfiguration, Table = container, Key = r["name"] as string, Properties = new Dictionary<string, string>() };

                nut.Value = r["value"] as string;
                nut.Uri = string.Format("/{0}/{1}/{2}", account, container, r["name"]);

                foreach (var j in r.Keys)
                {
                    nut.Properties.Add(j, r[j] as string);
                }

                nuts.Add(nut);
            }

            return nuts;
        }
    }
}
