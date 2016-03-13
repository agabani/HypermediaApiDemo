using System;

namespace Api.Tests.Acceptance.Siren.Pocos
{
    internal class Action
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public string Method { get; set; }
        public Uri Href { get; set; }
        public string Type { get; set; }
        public Field[] Fields { get; set; }
    }
}