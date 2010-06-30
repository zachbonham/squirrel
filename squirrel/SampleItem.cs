using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace squirrel
{
    // TODO: Edit the SampleItem class
    public class SampleItem
    {
        public int Id { get; set; }
        public string StringValue { get; set; }
    }

    public class ConfigEntry
    {
        public string Key { get; set; }
        public string Value { get; set; }
        
        IDictionary<string, string> Properties { get; set; }

    }


}
