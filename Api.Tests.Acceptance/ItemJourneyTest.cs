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
        public new void OneTimeSetUp()
        {
            using (var sirenJourney = new SirenJourney(new SirenHttpClient(new HttpClient
            {
                BaseAddress = BaseAddress
            })))
            {
                _entity = sirenJourney
                    .FollowLink(link => link.Rel.Contains("items"))
                    .FollowEntityLink(e => e.Properties.Contains(new KeyValuePair<string, dynamic>("id", "A")))
                    .Travel();
            }
        }

        private Entity _entity;

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

        [Test]
        public void Item_has_basket_actions()
        {
            var action = _entity
                .Actions.Single(a => a.Name.Equals("basket-add"));

            Assert.That(action.Href, Is.EqualTo(new Uri(BaseAddress, "basket")));
            Assert.That(action.Fields.Single().Name, Is.EqualTo("id"));
            Assert.That(action.Fields.Single().Type, Is.EqualTo("text"));
            Assert.That(action.Fields.Single().Value, Is.EqualTo("A"));
            Assert.That(action.Method, Is.EqualTo("POST"));
            Assert.That(action.Name, Is.EqualTo("basket-add"));
            Assert.That(action.Title, Is.EqualTo("Add to basket"));
            Assert.That(action.Type, Is.EqualTo("application/x-www-form-urlencoded"));
        }
    }
}