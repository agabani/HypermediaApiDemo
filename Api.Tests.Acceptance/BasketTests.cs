using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Api.Tests.Acceptance.Siren;
using NUnit.Framework;

namespace Api.Tests.Acceptance
{
    [TestFixture]
    internal class BasketTests : Tests
    {
        [SetUp]
        public void SetUp()
        {
            _sirenJourney = new SirenJourney(new SirenHttpClient(new HttpClient {BaseAddress = BaseAddress}));
        }

        [TearDown]
        public void TearDown()
        {
            _sirenJourney.Dispose();
            _sirenJourney = null;
        }

        private SirenJourney _sirenJourney;

        [Test]
        public void Add_to_basket_and_provides_anonymous_account()
        {
            var entity = _sirenJourney
                .FollowLink(l => l.Rel.Contains("items"))
                .FollowEntityLink(e => e.Properties.Contains(new KeyValuePair<string, dynamic>("id", "A")))
                .FollowAction(a => a.Name.Equals("basket-add"))
                .Travel();

            Assert.That(entity.Class.Contains("basket"));
            Assert.That(entity.Class.Contains("collection"));

            Assert.That(entity
                .Links.Single(link => link.Rel.Contains("self"))
                .Href,
                Is.EqualTo(new Uri(BaseAddress, "basket")));

            Assert.That(entity
                .Entities.Count(e => e.Class.Contains("item")), Is.EqualTo(1));

            string authorizationHeaderValue = entity
                .Entities.Single(e => e.Class.Contains("http"))
                .Properties.Single(p => p.Key == "authorization").Value;

            Assert.That(authorizationHeaderValue.Split(' ').First(), Is.EqualTo("Basic"));
            Assert.That(Guid.Parse(authorizationHeaderValue.Split(' ').Last()), Is.Not.EqualTo(default(Guid)));
        }

        [Test]
        public void Authenticated_request_should_not_show_http_class()
        {
            var entity = _sirenJourney
                .FollowLink(l => l.Rel.Contains("items"))
                .FollowEntityLink(e => e.Properties.Contains(new KeyValuePair<string, dynamic>("id", "A")))
                .FollowAction(a => a.Name.Equals("basket-add"))
                .FollowLink(link => link.Rel.Contains("self"))
                .Travel();

            Assert.That(entity.Class.Contains("basket"));
            Assert.That(entity.Class.Contains("collection"));

            Assert.That(entity
                .Entities.Count(e => e.Class.Contains("item")),
                Is.EqualTo(1));

            Assert.That(entity
                .Entities.Any(e => e.Class.Contains("http")),
                Is.False);
        }

        [Test]
        [TestCase("A", 50d)]
        [TestCase("AB", 80d)]
        [TestCase("CDBA", 115d)]
        [TestCase("AA", 100d)]
        [TestCase("AAA", 130d)]
        [TestCase("AAABB", 175d)]
        public void Basket_has_price(string items, double expectedPrice)
        {
            foreach (var item in items)
            {
                _sirenJourney
                    .FollowLink(l => l.Rel.Contains("items"))
                    .FollowEntityLink(e => e.Properties.Contains(new KeyValuePair<string, dynamic>("id", item.ToString())))
                    .FollowAction(a => a.Name.Equals("basket-add"));
            }

            var entity = _sirenJourney.Travel();

            Assert.That(entity.Properties["price"], Is.EqualTo(expectedPrice));
        }
    }
}