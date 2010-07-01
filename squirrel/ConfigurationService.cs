using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using MongoDB.Driver;

using squirrel.contracts;
using squirrel.businesslogic;
using System.Diagnostics;

namespace squirrel
{

    public static class NameValueCollectionExtensions
    {
        public static Dictionary<string, string> ToAttributes(this NameValueCollection collection)
        {
            Dictionary<string, string> properties = new Dictionary<string, string>();

            foreach (string k in collection.AllKeys)
            {

                var t = collection[k];

                if (k == "value")
                {
                    properties["value"] = t as string;
                }
                else
                {
                    properties[k] = collection[k];

                }
            }

            return properties;
        }
    }
    // Start the service and browse to http://<machine_name>:<port>/Service1/help to view the service's generated help page
    // NOTE: By default, a new instance of the service is created for each call; change the InstanceContextMode to Single if you want
    // a single instance of the service to process all calls.	
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class ConfigurationService
    {

        [WebGet(UriTemplate = "{account}/{container}")]
        public List<Nut> GetCollection(string account, string container)
        {

            List<Nut> nuts = new List<Nut>();


            try
            {

                var properties = WebOperationContext.Current.IncomingRequest.UriTemplateMatch.QueryParameters.ToAttributes();

                nuts = SquirrelBusinessLogic.FindAllInContainer(account, container, properties);
            }
            catch (Exception eax)
            {
                throw new WebFaultException<string>(eax.Message, System.Net.HttpStatusCode.InternalServerError);
            }
            
            return nuts; 

        }

        [WebInvoke(UriTemplate = "{account}/{container}", Method = "POST")]
        public void Create(string account, string container, Nut nut)
        {

            bool success = false;

            try
            {
                success = SquirrelBusinessLogic.CreateNut(account, container, nut); 
                
            }
            catch (Exception eax)
            {
                
                throw new WebFaultException<string>(eax.Message, System.Net.HttpStatusCode.InternalServerError);

            }
            finally
            {
                if (success)
                {
                    WebOperationContext.Current.OutgoingResponse.SetStatusAsCreated(CreateUri(account, container, nut));
                }
            }

        }

        private Uri CreateUri(string account, string table, Nut nut)
        {
            string t = string.Format("{0}/{1}/{2}/{3}", WebOperationContext.Current.IncomingRequest.UriTemplateMatch.BaseUri, account, table, nut.Key);

           
            return new Uri(t);
        }

        [WebGet(UriTemplate = @"{account}/{container}/{name}")]
        public Nut Get(string account, string container, string name)
        {

            Nut nut = null;

            try
            {
                
                var attributes = WebOperationContext.Current.IncomingRequest.UriTemplateMatch.QueryParameters.ToAttributes();

            
                nut = SquirrelBusinessLogic.FindNut(account, container, name, attributes);

            }
            catch (Exception eax)
            {

                throw new WebFaultException<string>(eax.Message, System.Net.HttpStatusCode.InternalServerError);

            }
            finally
            {
                if (nut == null)
                {
                    throw new WebFaultException<string>(string.Format("{0}/{1} not found", container, name), System.Net.HttpStatusCode.NotFound);                  
                }
                
            }

            return nut;

        }
        
   
        [WebInvoke(UriTemplate = "{account}/{container}/{name}", Method = "DELETE")]
        public void Delete(string account, string container, string name)
        {
            // TODO: Remove the instance of SampleItem with the given id from the collection
            throw new NotImplementedException();
        }

    }
}
