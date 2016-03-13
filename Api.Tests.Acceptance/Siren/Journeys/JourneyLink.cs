using System;
using System.Linq;
using Api.Tests.Acceptance.Siren.Pocos;

namespace Api.Tests.Acceptance.Siren.Journeys
{
    internal sealed class JourneyLink : IJourney
    {
        private readonly Func<Link, bool> _predicate;

        public JourneyLink(Func<Link, bool> predicate)
        {
            _predicate = predicate;
        }

        public Entity Travel(SirenHttpClient client, Entity entity)
        {
            return client.Get(entity.Links.Single(_predicate).Href);
        }
    }
}