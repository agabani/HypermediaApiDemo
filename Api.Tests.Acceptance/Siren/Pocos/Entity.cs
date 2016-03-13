using System.Collections.Generic;

namespace Api.Tests.Acceptance.Siren.Pocos
{
    internal class Entity
    {
        public string[] Class { get; set; }
        public Dictionary<string, dynamic> Properties { get; set; }
        public Entity[] Entities { get; set; }
        public Action[] Actions { get; set; }
        public Link[] Links { get; set; }
    }
}