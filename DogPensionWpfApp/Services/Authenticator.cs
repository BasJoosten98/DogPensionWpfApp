using DogPensionWpfApp.Extensions;
using DogPensionWpfApp.Models.Database;
using System;
using System.Collections.Generic;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace DogPensionWpfApp.Services
{
    public class Authenticator
    {
        private readonly AccountService accountService;

        private Account _account;

        public Account Account
        {
            get { return _account; }
            private set { _account = value; }
        }

        public event Action Account_Changed;

        public Authenticator(AccountService accountService)
        {
            this.accountService = accountService;
        }

        public async Task<bool> Login(string name, SecureString password)
        {
            Account temp = await accountService.Login(name, password);
            if(temp != null) { _account = temp; OnAccountChanged(); }
            return temp != null;
        }

        public async Task<bool> Register(string name, string email, SecureString password, SecureString comfirmPassword)
        {
            if (password == null)
            {
                throw new Exception("Password can not be empty");
            }
            if (comfirmPassword == null)
            {
                throw new Exception("Repeat password can not be empty");
            }
            if (password.ToUnsecureString() == comfirmPassword.ToUnsecureString())
            {
                Account temp = await accountService.Register(name, email, password);
                if(temp != null) { _account = temp; OnAccountChanged(); }
                return temp != null;
            }
            else
            {
                throw new Exception("Passwords do not match");
            }
        }

        public void Logout()
        {
            _account = null;
            OnAccountChanged();
        }

        private void OnAccountChanged() { Account_Changed?.Invoke(); }
    }
}
