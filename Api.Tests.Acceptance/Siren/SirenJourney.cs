using System;
using System.Collections.Generic;
using System.Linq;

namespace Api.Tests.Acceptance.Siren
{
    internal class SirenJourney : IDisposable
    {
        private readonly List<Journey> _links = new List<Journey>();
        private SirenHttpClient _client;

        public SirenJourney(SirenHttpClient client)
        {
            _client = client;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public SirenJourney FollowAction(string relation)
        {
            return this;
        }

        public SirenJourney FollowLink(Func<Link, bool> predicate)
        {
            _links.Add(new JourneyLink(predicate));
            return this;
        }

        public SirenJourney FollowEntityLink(Func<Entity, bool> predicate)
        {
            _links.Add(new JourneyEntityLink(predicate));
            return this;
        }

        public Entity Travel()
        {
            var entity = _client.Get();

            foreach (var journeyLink in _links)
            {
                entity = journeyLink.Travel(_client, entity);
            }

            return entity;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_client != null)
                {
                    _client.Dispose();
                    _client = null;
                }
            }
        }

        private abstract class Journey
        {
            public abstract Entity Travel(SirenHttpClient client, Entity entity);
        }

        private class JourneyLink : Journey
        {
            private readonly Func<Link, bool> _predicate;

            public JourneyLink(Func<Link, bool> predicate)
            {
                _predicate = predicate;
            }

            public override Entity Travel(SirenHttpClient client, Entity entity)
            {
                return client.Get(entity.Links.Single(_predicate).Href);
            }
        }

        private class JourneyEntityLink : Journey
        {
            private readonly Func<Entity, bool> _predicate;

            public JourneyEntityLink(Func<Entity, bool> predicate)
            {
                _predicate = predicate;
            }

            public override Entity Travel(SirenHttpClient client, Entity entity)
            {
                var href = entity.Entities.Single(_predicate)
                    .Links.Single(link => link.Rel.Contains("self"))
                    .Href;

                return client.Get(href);
            }
        }
    }
}