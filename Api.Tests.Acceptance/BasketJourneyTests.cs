using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Api.Tests.Acceptance.Siren;
using NUnit.Framework;

namespace Api.Tests.Acceptance
{
    [TestFixture]
    internal class BasketJourneyTests : JourneyTests
    {
        [SetUp]
        public void SetUp()
        {
            _sirenJourney = new SirenJourney(new SirenHttpClient(new HttpClient
            {
                BaseAddress = BaseAddress
            }))
                .FollowLink(link => link.Rel.Contains("items"))
                .FollowEntityLink(entity => entity.Properties.Contains(new KeyValuePair<string, dynamic>("id", "A")))
                .FollowAction(action => action.Name.Equals("basket-add"));
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
    }
}