using DataAccess.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace WebApi.Auth
{
    public class TokenAuthenticationSchemeOptions
        : AuthenticationSchemeOptions
    { 
    }

    public class AuthenticationSchemeHandler
        : AuthenticationHandler<TokenAuthenticationSchemeOptions>
    {
        private readonly ILoginRepository _repository;

        public AuthenticationSchemeHandler(
            IOptionsMonitor<TokenAuthenticationSchemeOptions> options, 
            ILoggerFactory logger,
            UrlEncoder encoder, 
            ISystemClock clock,
            ILoginRepository repository) 
            : base(options, logger, encoder, clock)
        {
            _repository = repository;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey(HeaderNames.Authorization))
            {
                return AuthenticateResult.Fail("Header Not Found.");
            }

            var header = Request.Headers[HeaderNames.Authorization].ToString()?.Replace("Bearer ", string.Empty);

            if(!Guid.TryParse(header, out var tokenId))
            {
                return AuthenticateResult.Fail("Header is not guid token.");
            }

            var model = await _repository.GetAuthUserInfo(tokenId, Context.RequestAborted);
            if (model == null)
            {
                return AuthenticateResult.Fail("Token doesn't link with any user");
            }


            var claims = new[] {
                new Claim(ClaimTypes.NameIdentifier, model.UserId.ToString()),
                new Claim(ClaimTypes.Name, model.UserName), 
            };

            // generate claimsIdentity on the name of the class
            var claimsIdentity = new ClaimsIdentity(claims, nameof(AuthenticationSchemeHandler));

            // generate AuthenticationTicket from the Identity
            // and current authentication scheme
            var ticket = new AuthenticationTicket(new ClaimsPrincipal(claimsIdentity), this.Scheme.Name);
            return AuthenticateResult.Success(ticket);
        }
    }
}
