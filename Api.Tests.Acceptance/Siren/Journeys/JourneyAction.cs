using System;
using System.Linq;
using Api.Tests.Acceptance.Siren.Pocos;
using Action = Api.Tests.Acceptance.Siren.Pocos.Action;

namespace Api.Tests.Acceptance.Siren.Journeys
{
    internal sealed class JourneyAction : IJourney
    {
        private readonly Func<Action, bool> _predicate;

        public JourneyAction(Func<Action, bool> predicate)
        {
            _predicate = predicate;
        }

        public Entity Travel(SirenHttpClient client, Entity entity)
        {
            var action = entity.Actions.Single(_predicate);

            return client.Post(action.Href, action
                .Fields.ToDictionary(field => field.Name, field => field.Value));
        }
    }
}