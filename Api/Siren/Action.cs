using System;

namespace Api.Siren
{
    public class Action
    {
        public string Name { get; set; }
        public string[] Class { get; set; }
        public string Method { get; set; }
        public Uri Href { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public Field[] Fields { get; set; }
    }
}