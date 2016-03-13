using System;
using System.Collections.Generic;
using System.Linq;
using Api.Siren;
using Action = Api.Siren.Action;

namespace Api.Builders
{
    public class EntityBuilder
    {
        private readonly List<System.Func<Action>> _actions = new List<System.Func<Action>>();
        private readonly List<string> _class = new List<string>();
        private readonly List<Func<Entity>> _entityBuilders = new List<Func<Entity>>();
        private readonly List<Func<Link>> _links = new List<Func<Link>>();
        private readonly Dictionary<string, dynamic> _properties = new Dictionary<string, dynamic>();

        public Entity Build()
        {
            return new Entity
            {
                Class = _class.ToArray(),
                Properties = _properties.ToDictionary(kvp => kvp.Key, kvp => kvp.Value),
                Entities = _entityBuilders.Select(entity => entity()).ToArray(),
                Links = _links.Select(link => link()).ToArray(),
                Actions = _actions.Select(action => action()).ToArray()
            };
        }

        public EntityBuilder WithClass(string @class)
        {
            _class.Add(@class);
            return this;
        }

        public EntityBuilder WithProperty(string key, dynamic value)
        {
            _properties[key] = value;
            return this;
        }

        public EntityBuilder WithEntity(Func<Entity> entity)
        {
            _entityBuilders.Add(entity);
            return this;
        }

        public EntityBuilder WithEntity(IEnumerable<Func<Entity>> entities)
        {
            _entityBuilders.AddRange(entities);
            return this;
        }

        public EntityBuilder WithLink(Func<Link> link)
        {
            _links.Add(link);
            return this;
        }

        public EntityBuilder WithAction(Func<Action> action)
        {
            _actions.Add(action);
            return this;
        }

        public EntityBuilder WithAction(IEnumerable<Func<Action>> actions)
        {
            _actions.AddRange(actions);
            return this;
        }
    }
}