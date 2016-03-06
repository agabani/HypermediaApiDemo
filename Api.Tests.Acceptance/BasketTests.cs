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
            var basketAddAction = _itemAEntity
                .Actions.Single(a => a.Name.Equals("basket-add"));

            var basketEntity = Client.Post(basketAddAction.Href, basketAddAction
                .Fields.ToDictionary(field => field.Name, field => field.Value));

            Assert.That(basketEntity.Class.Contains("basket"));
            Assert.That(basketEntity.Class.Contains("collection"));

            Assert.That(basketEntity
                .Links.Single(link => link.Rel.Contains("self"))
                .Href,
                Is.EqualTo(new Uri(BaseAddress, "basket")));

            var itemEntity = basketEntity
                .Entities.Single(e => e.Class.Contains("item"));

            Assert.That(itemEntity
                .Properties.Contains(new KeyValuePair<string, dynamic>("id", "A")));

            var httpEntity = basketEntity
                .Entities.Single(e => e.Class.Contains("http"));

            var authorization = ((string)httpEntity
                .Properties.Single(pair => pair.Key == "authorization")
                .Value)
                .Split(' ');

            Assert.That(authorization.First(), Is.EqualTo("Basic"));
            Assert.That(Guid.Parse(authorization.Last()), Is.Not.EqualTo(default(Guid)));
        }
    }
}