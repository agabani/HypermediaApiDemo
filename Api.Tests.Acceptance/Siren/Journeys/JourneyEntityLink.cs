using System;
using System.Linq;
using Api.Tests.Acceptance.Siren.Pocos;

namespace Api.Tests.Acceptance.Siren.Journeys
{
    internal sealed class JourneyEntityLink : IJourney
    {
        private readonly Func<Entity, bool> _predicate;

        public JourneyEntityLink(Func<Entity, bool> predicate)
        {
            _predicate = predicate;
        }

        public Entity Travel(SirenHttpClient client, Entity entity)
        {
            var href = entity.Entities.Single(_predicate)
                .Links.Single(link => link.Rel.Contains("self"))
                .Href;

            return client.Get(href);
        }
    }
}