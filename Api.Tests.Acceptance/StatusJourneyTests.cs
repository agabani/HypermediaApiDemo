using System;
using System.Linq;
using System.Net.Http;
using Api.Tests.Acceptance.Siren;
using NUnit.Framework;

namespace Api.Tests.Acceptance
{
    [TestFixture]
    internal class StatusJourneyTests : JourneyTests
    {
        private Entity _entity;

        [OneTimeSetUp]
        public new void OneTimeSetUp()
        {
            var sirenJourney = new SirenJourney(new SirenHttpClient(new HttpClient
            {
                BaseAddress = BaseAddress
            }));

            _entity = sirenJourney
                .FollowLink(link => link.Rel.Contains("status"))
                .Travel();
        }

        [Test]
        public void Status_class()
        {
            Assert.That(_entity.Class.Contains("status"));
        }

        [Test]
        public void Status_links_to_self()
        {
            Assert.That(_entity
                .Links.Single(link => link.Rel.Contains("self"))
                .Href,
                Is.EqualTo(new Uri(BaseAddress, "status")));
        }

        [Test]
        public void Status_shows_current_utc_time()
        {
            Assert.That(_entity
                .Properties["currentUtcTime"],
                Is.GreaterThan(DateTime.UtcNow.AddSeconds(-1)));
        }
    }
}