using System;
using System.Linq;
using System.Net.Http;
using Api.Tests.Acceptance.Siren;
using Api.Tests.Acceptance.Siren.Pocos;
using NUnit.Framework;

namespace Api.Tests.Acceptance
{
    [TestFixture]
    internal class RootTests : Tests
    {
        private Entity _entity;

        [OneTimeSetUp]
        public new void OneTimeSetUp()
        {
            var sirenJourney = new SirenHttpJourney(new SirenHttpClient(new HttpClient
            {
                BaseAddress = BaseAddress
            }));

            _entity = sirenJourney
                .Travel();
        }

        [Test]
        public void Root_class()
        {
            Assert.That(_entity.Class.Contains("root"));
        }

        [Test]
        public void Root_contains_no_properties()
        {
            Assert.That(_entity.Properties, Is.Empty);
        }

        [Test]
        public void Root_links_to_self()
        {
            Assert.That(_entity
                .Links.Single(l => l.Rel.Contains("self"))
                .Href,
                Is.EqualTo(BaseAddress));
        }

        [Test]
        public void Root_links_to_status()
        {
            Assert.That(_entity
                .Links.Single(l => l.Rel.Contains("status"))
                .Href,
                Is.EqualTo(new Uri(BaseAddress, "status")));
        }

        [Test]
        public void Root_links_to_items()
        {
            Assert.That(_entity
                .Links.Single(link => link.Rel.Contains("items"))
                .Href,
                Is.EqualTo(new Uri(BaseAddress, "items")));
        }

        [Test]
        public void Root_links_to_basket()
        {
            Assert.That(_entity
                .Links.Single(link => link.Rel.Contains("basket"))
                .Href,
                Is.EqualTo(new Uri(BaseAddress, "basket")));
        }
    }
}