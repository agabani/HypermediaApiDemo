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
            var account = new Account();

            Accounts.Add(account);

            return account;
        }

        public Account GetByToken(Guid token)
        {
            return Accounts.Single(account => account.Token == token);
        }

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
}