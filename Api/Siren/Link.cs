using System;

namespace Api.Siren
{
    public class Link
    {
        public Link(string rel, Uri href) : this(new[] {rel}, href)
        {
        }

        public Link(string[] rel, Uri href)
        {
            Rel = rel;
            Href = href;
        }

        public string[] Rel { get; private set; }
        public Uri Href { get; private set; }
    }
}