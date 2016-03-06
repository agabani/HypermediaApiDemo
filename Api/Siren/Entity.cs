using System.Collections.Generic;

namespace Api.Siren
{
    public class Entity
    {
        public string[] Class { get; set; }
        public Dictionary<string, dynamic> Properties { get; set; }
        public Link[] Links { get; set; }
    }
}