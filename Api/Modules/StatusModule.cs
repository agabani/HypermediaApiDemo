using System;
using Api.Builders;
using Api.Siren;
using Microsoft.AspNet.Http;

namespace Api.Modules
{
    public class StatusModule : Module
    {
        public StatusModule(HttpRequest request) : base(request)
        {
        }

        public Entity Handle()
        {
            return new EntityBuilder()
                .WithClass("status")
                .WithProperty("currentUtcTime", DateTime.UtcNow)
                .WithLink(() => LinkFactory.Create("status", true))
                .Build();
        }
    }
}