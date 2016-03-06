using System;
using System.Collections.Generic;
using System.Linq;
using Api.Extensions;
using Api.Repositories;
using Api.Siren;
using Api.ViewModels;
using Microsoft.AspNet.Http;
using Microsoft.Extensions.Primitives;

namespace Api.Modules
{
    public class BasketModule : Module
    {
        private readonly AccountRepository _accountRepository = new AccountRepository();
        private readonly BasketRepository _basketRepository = new BasketRepository();

        public BasketModule(HttpRequest request) : base(request)
        {
        }

        public Entity BuildEntity(BasketAddModel model)
        {
            StringValues stringValues;

            var account = Request.Headers.TryGetValue("authorization", out stringValues)
                ? _accountRepository.GetByToken(Guid.Parse(stringValues.First().Split(' ').Last()))
                : _accountRepository.CreateAnonymous();

            _basketRepository.AddItem(account.Id, model.Id);

            var basket = _basketRepository.GetBasket(account.Id);

            var entities = basket.Select(s => new ItemModule(Request, "items", s).BuildEntity()).ToArray();

            var httpEntity = new Entity
            {
                Class = new[] {"http"},
                Properties = new Dictionary<string, dynamic>
                {
                    {"authorization", $"Basic {account.Token}"}
                }
            };

            return new Entity
            {
                Class = new[] {"basket", "collection"},
                Links = new[]
                {
                    new Link
                    {
                        Rel = new[] {"self"},
                        Href = Request.GetAbsoluteAddress()
                    }
                },
                Entities = stringValues == default(StringValues) ? new[] {httpEntity}.Concat(entities).ToArray() : entities
            };
        }
    }
}