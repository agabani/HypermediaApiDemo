using System;

namespace Api.Tests.Acceptance.Siren.Pocos
{
    internal class Link
    {
        public string[] Rel { get; set; }
        public Uri Href { get; set; }
    }
}