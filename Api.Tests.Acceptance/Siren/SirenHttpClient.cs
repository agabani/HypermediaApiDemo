using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json;

namespace Api.Tests.Acceptance.Siren
{
    internal class SirenHttpClient : IDisposable
    {
        private HttpClient _httpClient;

        public SirenHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public Entity Get()
        {
            return JsonConvert.DeserializeObject<Entity>(_httpClient
                .GetAsync(_httpClient.BaseAddress).Result
                .Content.ReadAsStringAsync().Result);
        }

        public Entity Get(Uri uri)
        {
            return JsonConvert.DeserializeObject<Entity>(_httpClient
                .GetAsync(uri).Result
                .Content.ReadAsStringAsync().Result);
        }

        public Entity Post(Uri uri, IDictionary<string, dynamic> form)
        {
            var nameValueCollection = form
                .Select(kvp => new KeyValuePair<string, string>(kvp.Key, kvp.Value.ToString()));

            return JsonConvert.DeserializeObject<Entity>(_httpClient
                .PostAsync(uri, new FormUrlEncodedContent(nameValueCollection)).Result
                .Content.ReadAsStringAsync().Result);
        }

        protected virtual void Dispose(bool dispoing)
        {
            if (dispoing)
            {
                if (_httpClient != null)
                {
                    _httpClient.Dispose();
                    _httpClient = null;
                }
            }
        }
    }
}