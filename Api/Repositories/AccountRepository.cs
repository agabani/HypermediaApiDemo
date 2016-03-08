using System;
using System.Collections.Generic;
using System.Linq;
using Api.DomainObjects;

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

        
    }
}