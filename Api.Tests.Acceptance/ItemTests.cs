using System;
using System.Linq;
using Api.Tests.Acceptance.Siren;
using NUnit.Framework;

namespace Api.Tests.Acceptance
{
    internal class ItemTests : Tests
    {
        private Entity _entity;

        [OneTimeSetUp]
        public new void OneTimeSetUp()
        {
            var root = Client.Get();
            var itemsHref = root.Links.Single(link => link.Rel.Contains("items")).Href;
            _entity = Client.Get(itemsHref);
        }

        [Test]
        public void Items_class()
        {
            Assert.That(_entity.Class.Contains("items"));
            Assert.That(_entity.Class.Contains("collection"));
        }

        [Test]
        public void Items_links_to_self()
        {
            Assert.That(_entity
                .Links.Single(link => link.Rel.Contains("self"))
                .Href,
                Is.EqualTo(new Uri(BaseAddress, "items")));
        }
    }
}