using System;
using System.Collections.Generic;
using Api.Extensions;
using Api.Repositories;
using Api.Siren;
using Api.ValueObjects;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Http.Extensions;
using Action = Api.Siren.Action;

namespace Api.Modules
{
    public class ItemModule : Module
    {
        private static readonly ItemRepository ItemRepository = new ItemRepository();
        protected readonly string Id;
        protected readonly Uri Href;

        public ItemModule(HttpRequest request, string id) : base(request)
        {
            Href = new Uri(Request.GetDisplayUrl());
            Id = id;
        }

        public Entity BuildEntity()
        {
            var item = ItemRepository.Get(Id);

            return new Entity
            {
                Class = BuildClass(),
                Properties = BuildProperties(item),
                Entities = BuildEntities(),
                Links = BuildLinks(),
                Actions = BuildActions(item)
            };
        }

        private static string[] BuildClass()
        {
            return new[] {"item"};
        }

        private static Dictionary<string, dynamic> BuildProperties(Item item)
        {
            return new Dictionary<string, dynamic>
            {
                {"id", item.Id},
                {"value", item.Value.Units}
            };
        }

        private static Entity[] BuildEntities()
        {
            return new Entity[] {};
        }

        protected virtual Action[] BuildActions(Item item)
        {
            return new[]
            {
                new Action
                {
                    Name = "basket-add",
                    Href = new Uri(Href, "/basket"),
                    Method = "POST",
                    Type = "application/x-www-form-urlencoded",
                    Fields = new[]
                    {
                        new Field
                        {
                            Type = "text",
                            Name = "id",
                            Value = item.Id
                        }
                    },
                    Title = "Add to basket"
                }
            };
        }

        protected virtual Link[] BuildLinks()
        {
            return new[]
            {
                new Link
                {
                    Rel = new[] {"self"},
                    Href = Href
                },
                new Link
                {
                    Rel = new[] {"items"},
                    Href = new Uri(Request.GetBaseAddress(), "items")
                },
                new Link
                {
                    Rel = new[] {"basket"},
                    Href = new Uri(Request.GetBaseAddress(), "basket")
                }
            };
        }
    }
}