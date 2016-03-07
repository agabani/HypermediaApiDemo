using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Api.Tests.Acceptance.Siren;
using NUnit.Framework;

namespace Api.Tests.Acceptance
{
    [TestFixture]
    internal class ItemJourneyTest : JourneyTests
    {
        [SetUp]
        public void SetUp()
        {
            _sirenJourney = new SirenJourney(new SirenHttpClient(new HttpClient
            {
                BaseAddress = BaseAddress
            }))
                .FollowLink(link => link.Rel.Contains("items"));
        }

        [TearDown]
        public void TearDown()
        {
            _sirenJourney.Dispose();
            _sirenJourney = null;
        }

        private Entity _entity;
        private SirenJourney _sirenJourney;

        [Test]
        [TestCase("A", 50d)]
        [TestCase("B", 30d)]
        [TestCase("C", 20d)]
        [TestCase("D", 15d)]
        public void Item_Test(string id, double value)
        {
            _entity = _sirenJourney
                .FollowEntityLink(entity => entity.Properties.Contains(new KeyValuePair<string, dynamic>("id", id)))
                .Travel();

            Has_class();
            Has_link_to_self(id);
            Has_properties(id, value);
            Has_basket_actions(id);
        }

        private void Has_class()
        {
            Assert.That(_entity.Class.Contains("item"));
        }

        private void Has_link_to_self(string id)
        {
            Assert.That(_entity
                .Links.Single(link => link.Rel.Contains("self"))
                .Href,
                Is.EqualTo(new Uri(BaseAddress, $"items/{id}")));
        }

        private void Has_properties(string id, double value)
        {
            Assert.That(_entity.Properties.Single(property => property.Key == "id").Value, Is.EqualTo(id));
            Assert.That(_entity.Properties.Single(property => property.Key == "value").Value, Is.EqualTo(value));
        }

        private void Has_basket_actions(string id)
        {
            var action = _entity
                .Actions.Single(a => a.Name.Equals("basket-add"));

            Assert.That(action.Href, Is.EqualTo(new Uri(BaseAddress, "basket")));
            Assert.That(action.Fields.Single().Name, Is.EqualTo("id"));
            Assert.That(action.Fields.Single().Type, Is.EqualTo("text"));
            Assert.That(action.Fields.Single().Value, Is.EqualTo(id));
            Assert.That(action.Method, Is.EqualTo("POST"));
            Assert.That(action.Name, Is.EqualTo("basket-add"));
            Assert.That(action.Title, Is.EqualTo("Add to basket"));
            Assert.That(action.Type, Is.EqualTo("application/x-www-form-urlencoded"));
        }
    }
}