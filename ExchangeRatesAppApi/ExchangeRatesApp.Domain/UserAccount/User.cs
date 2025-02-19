using Microsoft.AspNetCore.Identity;
using System;

namespace ExchangeRatesApp.Domain.UserAccount
{
    public class User
    {
        protected User() { }

        public Guid Id { get; private set; }
        public string Login { get; private set; }
        public string PasswordHash { get; private set; }
        public DateTime? LastLoginDate { get; private set; }
        public DateTime? LastFailedLoginDate { get; private set; }

        public static User Create(string login, string password, IPasswordHasher<User> passwordHasher)
        {
            var user = new User { Id = Guid.NewGuid(), Login = login };
            user.PasswordHash = passwordHasher.HashPassword(user, password);
            return user;
        }

        public void OnSuccessfulLogin()
        {
            LastLoginDate = DateTimeProvider.Now;
        }
    }
}
