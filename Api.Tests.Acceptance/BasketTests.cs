using System;
using System.Collections.Generic;
using System.Linq;
using Api.Tests.Acceptance.Siren;
using NUnit.Framework;

namespace Api.Tests.Acceptance
{
    internal class BasketTests : Tests
    {
        private Entity _itemAEntity;

        [OneTimeSetUp]
        public new void OneTimeSetUp()
        {
            var rootEntity = Client.Get();
            var itemsHref = rootEntity.Links.Single(link => link.Rel.Contains("items")).Href;
            var itemsEntity = Client.Get(itemsHref);
            var itemAHref = itemsEntity
                .Entities.Single(entity => entity.Properties.Contains(new KeyValuePair<string, dynamic>("id", "A")))
                .Links.Single(link => link.Rel.Contains("self"))
                .Href;
            _itemAEntity = Client.Get(itemAHref);
        }

        [Test]
        public void Should_add_to_basket()
        {
            var action = _itemAEntity
                .Actions.Single(a => a.Name.Equals("basket-add"));

            var entity = Client.Post(action.Href, action
                .Fields.ToDictionary(field => field.Name, field => field.Value));

            Assert.That(entity.Class.Contains("basket"));
            Assert.That(entity.Class.Contains("collection"));

            Assert.That(entity
                .Links.Single(link => link.Rel.Contains("self"))
                .Href,
                Is.EqualTo(new Uri(BaseAddress, "basket")));

            var itemEntity = entity
                .Entities.Single(e => e.Class.Contains("item"));

            Assert.That(itemEntity
                .Properties.Contains(new KeyValuePair<string, dynamic>("id", "A")));

            var authorization = ((string)entity
                .Entities.Single(e => e.Class.Contains("http"))
                .Properties.Single(pair => pair.Key == "authorization")
                .Value)
                .Split(' ');

            Assert.That(authorization.First(), Is.EqualTo("Basic"));
            Assert.That(Guid.Parse(authorization.Last()), Is.Not.EqualTo(default(Guid)));
        }
    }
}