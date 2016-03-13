using System;
using System.Collections.Generic;
using Api.Tests.Acceptance.Siren.Journeys;
using Api.Tests.Acceptance.Siren.Pocos;
using Action = Api.Tests.Acceptance.Siren.Pocos.Action;

namespace Api.Tests.Acceptance.Siren
{
    internal class SirenHttpJourney : IDisposable
    {
        private readonly List<IJourney> _links = new List<IJourney>();
        private SirenHttpClient _client;

        public SirenHttpJourney(SirenHttpClient client)
        {
            _client = client;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public SirenHttpJourney FollowAction(Func<Action, bool> predicate)
        {
            _links.Add(new JourneyAction(predicate));
            return this;
        }

        public SirenHttpJourney FollowLink(Func<Link, bool> predicate)
        {
            _links.Add(new JourneyLink(predicate));
            return this;
        }

        public SirenHttpJourney FollowEntityAction(Func<Entity, bool> predicateEntity, Func<Action, bool> predicateAction)
        {
            _links.Add(new JourneyEntityAction(predicateEntity, predicateAction));
            return this;
        }

        public SirenHttpJourney FollowEntityLink(Func<Entity, bool> predicate)
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
    }
}