using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Collections.Specialized;

namespace squirrel
{
    public class SquirrelAuthorization : ServiceAuthorizationManager
    {
        public const string AUTHKEY = "Authorization";
        public override bool CheckAccess(OperationContext operationContext)
        {

            return IsValidAPIKey(operationContext);
        }

        protected bool IsValidAPIKey(OperationContext operationContext)
        {
            var authorizationKey = GetAPIKey(operationContext);

            
            return true;
        }

        public string GetAPIKey(OperationContext operationContext)
        {
            // Get the request message
            var request = operationContext.RequestContext.RequestMessage;

            // Get the HTTP Request
            var requestProp = (HttpRequestMessageProperty)request.Properties[HttpRequestMessageProperty.Name];

            // Get the query string
            NameValueCollection queryParams = HttpUtility.ParseQueryString(requestProp.QueryString);

            // Return the API key (if present, null if not)
            return queryParams[AUTHKEY];
        }

    }
}