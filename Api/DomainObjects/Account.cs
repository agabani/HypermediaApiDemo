using System;

namespace Api.DomainObjects
{
    public class Account
    {
        public Account()
        {
            Id = Guid.NewGuid();
            Token = Guid.NewGuid();
        }

        public Guid Id { get; }
        public Guid Token { get; }
    }
}