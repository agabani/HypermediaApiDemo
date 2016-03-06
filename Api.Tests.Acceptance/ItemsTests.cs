using System;
using System.Collections.Generic;
using System.Linq;
using Api.Tests.Acceptance.Siren;
using NUnit.Framework;

namespace Api.Tests.Acceptance
{
    internal class ItemsTests : Tests
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

        [Test]
        [TestCase("A", 50d)]
        [TestCase("B", 30d)]
        [TestCase("C", 20d)]
        [TestCase("D", 15d)]
        public void Items_should_contain_item_collection(string id, double value)
        {
            Assert.That(_entity.Entities, Is.Not.Empty);

            var item = _entity
                .Entities.Single(entity => entity.Properties.Contains(new KeyValuePair<string, dynamic>("id", id)));

            Assert.That(item.Class.Contains("item"));

            Assert.That(item
                .Links.Single(link => link.Rel.Contains("self"))
                .Href,
                Is.EqualTo(new Uri(BaseAddress, $"items/{id}")));

            Assert.That(item.Properties["value"], Is.EqualTo(value));
        }
    }
}