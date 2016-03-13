using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Api.Tests.Acceptance.Siren;
using Api.Tests.Acceptance.Siren.Pocos;
using NUnit.Framework;

namespace Api.Tests.Acceptance
{
    [TestFixture]
    internal class ItemsTests : Tests
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
                .FollowLink(link => link.Rel.Contains("items"))
                .Travel();
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
        public void Links_to_basket()
        {
            Assert.That(_entity
                .Links.Single(link => link.Rel.Contains("basket"))
                .Href,
                Is.EqualTo(new Uri(BaseAddress, "basket")));
        }

        [Test]
        public void Items_contains_no_properties()
        {
            Assert.That(_entity.Properties, Is.Empty);
        }

        [Test]
        public void Items_contains_no_actions()
        {
            Assert.That(_entity.Actions, Is.Empty);
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
            Assert.That(item.Entities, Is.Empty);

            var link = item.Links.Single();

            Assert.That(link.Rel.Contains("self"));
            Assert.That(link.Href, Is.EqualTo(new Uri(BaseAddress, $"items/{id}")));

            Assert.That(item.Properties["value"], Is.EqualTo(value));

            var action = item.Actions.Single();

            Assert.That(action.Href, Is.EqualTo(new Uri(BaseAddress, "basket")));
            Assert.That(action.Fields.Single().Name, Is.EqualTo("id"));
            Assert.That(action.Fields.Single().Type, Is.EqualTo("hidden"));
            Assert.That(action.Fields.Single().Value, Is.EqualTo(id));
            Assert.That(action.Method, Is.EqualTo("POST"));
            Assert.That(action.Name, Is.EqualTo("basket-add"));
            Assert.That(action.Title, Is.EqualTo("Add to basket"));
            Assert.That(action.Type, Is.EqualTo("application/x-www-form-urlencoded"));
        }
    }
}