Squirrel - "Every squirrel has its nuts"

Squirrel provides service based application configuration using Microsoft
.NET 4.0, REST based WCF and MongoDb.

Each piece of application configuration (connection string, service end point, 
configuration values) is considered a "nut".  Application configuration is considered
a collection of "nuts".
 
How you arrange, and access, your nuts is up to you.  Nuts are returned as 
xml documents.

See /squirrel/help for a list of operations.

Uri for nuts can be composed like:

/{account}/{container}/{name}?key=value&key=value

{account} - the application name, or account (may eventually add authentication token which will remove
the requirement for this).

{container} - the configuration bucket.  

{name} - the name of the nut.  This is not necessarily the id, or key, of the nut as
some configuration will have different attributes (for example, environments).

GET /{account}/{container}/{name} 
GET /myapp/appconfig/batchsize

#insert sample returned nut

To return all nuts for an application and container:

GET /myapp/appconfig

Arranging by well known types:

GET /myapp/connectionstrings
GET /myapp/services
GET /myapp/config

Its entirely up to you.


squirrel - the REST based WCF service.  Stores application configuration, a collection of "nuts"

TODO
-- refactor
-- add authentication requirement for requests
-- add application configuration isolation using new databases for each
account.
-- add ability to export a nut or collection of nuts from powershell
