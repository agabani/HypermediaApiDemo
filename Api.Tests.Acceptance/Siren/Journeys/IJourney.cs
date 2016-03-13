using Api.Tests.Acceptance.Siren.Pocos;

namespace Api.Tests.Acceptance.Siren.Journeys
{
    internal interface IJourney
    {
        Entity Travel(SirenHttpClient client, Entity entity);
    }
}