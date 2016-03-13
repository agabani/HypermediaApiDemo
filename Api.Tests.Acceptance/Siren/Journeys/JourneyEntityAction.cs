using System;
using System.Linq;
using Api.Tests.Acceptance.Siren.Pocos;
using Action = Api.Tests.Acceptance.Siren.Pocos.Action;

namespace Api.Tests.Acceptance.Siren.Journeys
{
    internal sealed class JourneyEntityAction : IJourney
    {
        private readonly Func<Entity, bool> _predicateEntity;
        private readonly Func<Action, bool> _predicateAction;

        public JourneyEntityAction(Func<Entity, bool> predicateEntity, Func<Action, bool> predicateAction)
        {
            _predicateEntity = predicateEntity;
            _predicateAction = predicateAction;
        }

        public Entity Travel(SirenHttpClient client, Entity entity)
        {
            var action = entity.Entities.Single(_predicateEntity)
                .Actions.Single(_predicateAction);

            switch (action.Method)
            {
                case "POST":
                    return client.Post(action.Href, action
                        .Fields.ToDictionary(field => field.Name, field => field.Value));
                case "DELETE":
                    return client.Delete(action.Href);
                default:
                    throw new NotImplementedException();
            }
        }
    }
}