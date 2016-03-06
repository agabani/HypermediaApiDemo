using System;
using Api.Siren;
using Api.ViewModels;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Http.Extensions;

namespace Api.Modules
{
    public class BasketModule : Module
    {
        public BasketModule(HttpRequest request) : base(request)
        {
        }

        public Entity BuildEntity(BasketAddModel model)
        {
            var href = new Uri(Request.GetDisplayUrl());

            return new Entity
            {
                Class = new[] {"basket", "collection"},
                Links = new[]
                {
                    new Link
                    {
                        Rel = new[] {"self"},
                        Href = href
                    }
                },
                Entities = new[]
                {
                    new ItemModule(Request, "items", model.Id).BuildEntity()
                }
            };
        }
    }
}