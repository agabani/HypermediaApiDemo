using System;
using Api.Siren;
using Api.ValueObjects;
using Action = Api.Siren.Action;

namespace Api.Factories
{
    public class ActionFactory
    {
        private readonly Uri _baseAddress;

        public ActionFactory(Uri baseAddress)
        {
            _baseAddress = baseAddress;
        }

        public Action Create(string entity, string action, object @object)
        {
            switch (entity)
            {
                case "basket":
                    return Create(action, @object);
                default:
                    throw new NotImplementedException();
            }
        }

        private Action Create(string action, object @object)
        {
            switch (action)
            {
                case "post":
                    return new Action
                    {
                        Name = "basket-add",
                        Href = new Uri(_baseAddress, "/basket"),
                        Method = "POST",
                        Type = "application/x-www-form-urlencoded",
                        Fields = new[]
                        {
                            new Field
                            {
                                Type = "hidden",
                                Name = "id",
                                Value = ((Item) @object).Id
                            }
                        },
                        Title = "Add to basket"
                    };
                default:
                    throw new NotImplementedException();
            }
        }
    }
}