using System;
using System.Collections.Generic;
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
            var items = Client.Get(itemsHref);
            var itemAHref = items
                .Entities.Single(entity => entity.Properties.Contains(new KeyValuePair<string, dynamic>("id", "A")))
                .Links.Single(link => link.Rel.Contains("self"))
                .Href;
            _entity = Client.Get(itemAHref);
        }

        [Test]
        public void Item_class()
        {
            Assert.That(_entity.Class.Contains("item"));
        }

        [Test]
        public void Item_links_to_self()
        {
            Assert.That(_entity
                .Links.Single(link => link.Rel.Contains("self"))
                .Href,
                Is.EqualTo(new Uri(BaseAddress, "items/A")));
        }

        [Test]
        public void Item_has_properties()
        {
            Assert.That(_entity.Properties.Single(property => property.Key == "id").Value, Is.EqualTo("A"));
            Assert.That(_entity.Properties.Single(property => property.Key == "value").Value, Is.EqualTo(50d));
        }
    }
}