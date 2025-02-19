using ExchangeRatesApp.Application.TechnicalInterfaces;
using ExchangeRatesApp.Domain;
using ExchangeRatesApp.Domain.Exceptions;
using ExchangeRatesApp.Domain.UserAccount;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ExchangeRatesApp.Application.UserAccount
{
    public class LoginToUserAccountCommandHandler : IRequestHandler<LoginToUserAccountCommand, LoginToUserAccountResponse>
    {
        private readonly IBaseRepository<User> _userBaseRepo;
        private readonly IAppSettings _appConfig;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IAuthCookieService _authCookieService;

        public LoginToUserAccountCommandHandler(IBaseRepository<User> userBaseRepo, IAppSettings appSettings, IPasswordHasher<User> passwordHasher,
             IAuthCookieService authCookieService)
        {
            _userBaseRepo = userBaseRepo;
            _appConfig = appSettings;
            _passwordHasher = passwordHasher;
            this._authCookieService = authCookieService;
        }

        public async Task<LoginToUserAccountResponse> Handle(LoginToUserAccountCommand request, CancellationToken cancellationToken)
        {
            var user = await _userBaseRepo.QueryableReadonly()
                                        .FirstOrDefaultAsync(x => x.Login == request.Username.Trim().ToLowerInvariant())
                                            ?? throw new UserException("Invalid login or password.");

            if (_passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password) == PasswordVerificationResult.Failed)
            {
                throw new UserException("Invalid login or password.");
            }

            user.OnSuccessfulLogin();

            var claims = new List<Claim>
                            {
                                new Claim(ClaimTypes.Name, user.Login)
                            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appConfig.AuthSettings.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(_appConfig.AuthSettings.ExpireMinutes),
                signingCredentials: creds);

            _authCookieService.SetAuthTokenCookie(token);

            return new LoginToUserAccountResponse { Username = user.Login };
        }
    }

    public class LoginToUserAccountCommand : IRequest<LoginToUserAccountResponse>
    {
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
    }

    public class LoginToUserAccountResponse
    {
        public string Username { get; set; } = null!;
    }

    public class LoginToUserAccountCommandValidator : AbstractValidator<LoginToUserAccountCommand>
    {
        public LoginToUserAccountCommandValidator()
        {
            RuleFor(x => x.Username).Must(x => !string.IsNullOrWhiteSpace(x))
                .WithMessage("Login is required");
            RuleFor(x => x.Password).Must(x => !string.IsNullOrWhiteSpace(x))
                .WithMessage("Password is required");
        }
    }
}
