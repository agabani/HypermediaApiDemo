using System;
using Api.Siren;

namespace Api.Factories
{
    public class LinkFactory
    {
        private readonly Uri _baseAddress;

        public LinkFactory(Uri baseAddress)
        {
            _baseAddress = baseAddress;
        }

        public Link Create(string entity, bool self)
        {
            switch (entity)
            {
                case "root":
                    return new Link
                    {
                        Rel = Relation("root", self),
                        Href = new Uri(_baseAddress, "/")
                    };
                case "status":
                    return new Link
                    {
                        Rel = Relation("status", self),
                        Href = new Uri(_baseAddress, "/status")
                    };
                case "items":
                    return new Link
                    {
                        Rel = Relation("items", self),
                        Href = new Uri(_baseAddress, "/items")
                    };
                case "basket":
                    return new Link
                    {
                        Rel = Relation("basket", self),
                        Href = new Uri(_baseAddress, "/basket")
                    };
                default:
                    throw new NotImplementedException();
            }
        }


        public Link Create(string entity, string id, bool self)
        {
            switch (entity)
            {
                case "item":
                    return new Link
                    {
                        Rel = Relation("item", self),
                        Href = new Uri(_baseAddress, $"/items/{id}")
                    };
                default:
                    throw new NotImplementedException();
            }
        }

        private static string[] Relation(string relation, bool self)
        {
            return self ? new[] {"self"} : new[] {relation};
        }
    }
}