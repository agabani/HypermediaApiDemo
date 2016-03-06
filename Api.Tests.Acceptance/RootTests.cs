using System;
using System.Linq;
using Api.Tests.Acceptance.Siren;
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
            _entity = Client.Get();
        }

        [Test]
        public void Root_class()
        {
            Assert.That(_entity.Class.Contains("root"));
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
    }
}