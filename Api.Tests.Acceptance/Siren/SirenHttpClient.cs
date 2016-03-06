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
            return ToEntity(_httpClient.GetAsync(_httpClient.BaseAddress).Result);
        }

        public Entity Get(Uri uri)
        {
            return ToEntity(_httpClient.GetAsync(uri).Result);
        }

        public Entity Post(Uri uri, IDictionary<string, dynamic> form)
        {
            return ToEntity(_httpClient.PostAsync(uri,
                new FormUrlEncodedContent(form
                    .Select(kvp => new KeyValuePair<string, string>(kvp.Key, kvp.Value.ToString())))).Result);
        }

        private Entity ToEntity(HttpResponseMessage httpResponseMessage)
        {
            var entity = JsonConvert.DeserializeObject<Entity>(httpResponseMessage
                .Content.ReadAsStringAsync().Result);

            ApplyHttpInstructions(entity);

            return entity;
        }

        private void ApplyHttpInstructions(Entity entity)
        {
            var httpEntity = entity.Class.Contains("http")
                ? entity
                : entity.Entities.FirstOrDefault(e => e.Class.Contains("http"));

            if (httpEntity == null)
            {
                return;
            }

            foreach (var property in httpEntity.Properties)
            {
                _httpClient.DefaultRequestHeaders.Add(property.Key, property.Value);
            }
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