using System;
using System.Net.Http;
using Api.Tests.Acceptance.Siren;
using NUnit.Framework;

namespace Api.Tests.Acceptance
{
    internal abstract class Tests
    {
        protected Uri BaseAddress;
        protected SirenHttpClient Client;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            BaseAddress = new Uri("http://localhost:5000");
            Client = new SirenHttpClient(new HttpClient
            {
                BaseAddress = BaseAddress
            });
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            Client.Dispose();
            Client = null;
        }
    }
}