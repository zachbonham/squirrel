using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace squirrel.contracts
{

    [DataContract(Name = "Nut", Namespace = "urn://squirrel/20100608")]
    public class Nut
    {

        [DataMember]
        public string Uri { get; set; }

        [DataMember]
        public string Version { get; set; }

        [DataMember]
        public string Database { get; set; }
        
        [DataMember]
        public string Table { get; set; }
        
        [DataMember]
        public string Key { get; set; }
        
        [DataMember]
        public string Value { get; set; }
        
        [DataMember]
        public IDictionary<string, string> Properties { get; set; }


    }
}
