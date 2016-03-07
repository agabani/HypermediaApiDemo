using System;
using System.Collections.Generic;
using System.Linq;

namespace Api.Tests.Acceptance.Siren
{
    internal class SirenJourney : IDisposable
    {
        private SirenHttpClient _client;
        private readonly List<JourneyLink> _links = new List<JourneyLink>();

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

        public SirenJourney FollowLink(string relation)
        {
            _links.Add(new JourneyLink(relation));
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

        public class JourneyLink
        {
            private readonly string _relation;

            public JourneyLink(string relation)
            {
                _relation = relation;
            }

            public Entity Travel(SirenHttpClient client, Entity entity)
            {
                return client.Get(entity.Links.Single(link => link.Rel.Contains(_relation)).Href);
            }
        }
    }
}