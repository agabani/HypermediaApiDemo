using System;
using System.Linq;
using Api.Tests.Acceptance.Siren;
using NUnit.Framework;

namespace Api.Tests.Acceptance
{
    [TestFixture]
    internal class StatusTests : Tests
    {
        private Entity _entity;

        [OneTimeSetUp]
        public new void OneTimeSetUp()
        {
            var root = Client.Get();
            var statusHref = root.Links.Single(link => link.Rel.Contains("status")).Href;
            _entity = Client.Get(statusHref);
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