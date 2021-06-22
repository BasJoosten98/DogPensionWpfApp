using DogPensionWpfApp.EntityFramework;
using DogPensionWpfApp.Models.Database;
using DogPensionWpfApp.Services.Repositories;
using System;
using System.Collections.Generic;
using Microsoft.AspNet.Identity;
using System.Text;
using System.Threading.Tasks;
using System.Security;
using System.Runtime.InteropServices;
using DogPensionWpfApp.Extensions;
using System.ComponentModel.DataAnnotations;

namespace DogPensionWpfApp.Services
{
    public class AccountService
    {
        private readonly DogPensionDbContextFactory factory;
        private readonly IPasswordHasher passwordHasher;

        public AccountService(DogPensionDbContextFactory factory,
            IPasswordHasher passwordHasher)
        {
            this.factory = factory;
            this.passwordHasher = passwordHasher;
        }

        public async Task<Account> Login(string name, SecureString password)
        {
            using(AccountRepository repo = new AccountRepository(factory.CreateDbContext()))
            {
                if(password == null)
                {
                    throw new Exception("Password can not be empty");
                }
                Account account = await repo.GetByName(name);
                if(account == null)
                {
                    throw new Exception("No account with this name");
                }
                PasswordVerificationResult result = (!string.IsNullOrEmpty(password.ToUnsecureString())) ? passwordHasher.VerifyHashedPassword(account.Password, password.ToUnsecureString()) : PasswordVerificationResult.Failed;
                if(result == PasswordVerificationResult.Success)
                {
                    return account;
                }
                else
                {
                    throw new Exception("Wrong password");
                }
            }
        }

        public async Task<Account> Register(string name, string email, SecureString password)
        {
            using (AccountRepository repo = new AccountRepository(factory.CreateDbContext()))
            {
                if (password == null)
                {
                    throw new Exception("Password can not be empty");
                }
                Account account = await repo.GetByName(name);
                if (account != null)
                {
                    throw new Exception("Account with this name exists");
                }
                account = await repo.GetByEmail(email);
                if (account != null)
                {
                    throw new Exception("Account with this email exists");
                }

                //create account
                string hashedPassword = passwordHasher.HashPassword(password.ToUnsecureString());
                account = new Account
                {
                    Name = name,
                    Email = email,
                    Password = hashedPassword
                };
                var ctx = new ValidationContext(account);

                // A list to hold the validation result.
                var results = new List<ValidationResult>();

                if (!Validator.TryValidateObject(account, ctx, results, true))
                {
                    //has errors
                }
                if(results.Count > 0) { throw new Exception(results[0].ErrorMessage); }

                repo.Add(account);
                await repo.SaveAsync();
                return account;
            }
        }
    }
}
