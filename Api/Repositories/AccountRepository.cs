using System;
using System.Collections.Generic;
using System.Linq;

namespace Api.Repositories
{
    public class AccountRepository
    {
        private static readonly List<Account> Accounts = new List<Account>();

        public Account CreateAnonymous()
        {
            var account = new Account
            {
                Id = Guid.NewGuid(),
                Token = Guid.NewGuid()
            };

            Accounts.Add(account);

            return account;
        }

        public Account GetByToken(Guid guid)
        {
            return Accounts.Single(account => account.Token == guid);
        }

        public class Account
        {
            public Guid Id { get; set; }
            public Guid Token { get; set; }
        }
    }
}