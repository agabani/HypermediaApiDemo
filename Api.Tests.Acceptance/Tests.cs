using System;
using System.Net.Http;
using Api.Tests.Acceptance.Siren;
using NUnit.Framework;

namespace Api.Tests.Acceptance
{
    [TestFixture]
    public class Tests
    {
        [SetUp]
        public void SetUp()
        {
            _client = new SirenHttpClient(new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5000")
            });
        }

        [TearDown]
        public void TearDown()
        {
            _client.Dispose();
            _client = null;
        }

        private SirenHttpClient _client;

        [Test]
        public void Should_be_able_to_connect()
        {
            _client.Get();
        }
    }
}