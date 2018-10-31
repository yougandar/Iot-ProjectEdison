using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Edison.Mobile.Common.Logging;
using Microsoft.Identity.Client;

namespace Edison.Mobile.Common.Auth
{
    public class AuthService
    {
        readonly IPlatformAuthService platformAuthService;
        readonly IPublicClientApplication publicClientApplication;
        readonly ILogger logger;

        public event EventHandler<AuthChangedEventArgs> OnAuthChanged;

        public AuthenticationResult AuthenticationResult { get; set; }
        public string Email { get; set; }

        public AuthService(IPlatformAuthService platformAuthService, IPublicClientApplication publicClientApplication, ILogger logger)
        {
            this.platformAuthService = platformAuthService;
            this.publicClientApplication = publicClientApplication;
            this.logger = logger;
        }

        public async Task<bool> AcquireToken()
        {
            try
            {
                AuthenticationResult = await platformAuthService.AcquireTokenAsync();

                if (AuthenticationResult != null && AuthenticationResult.IdToken != null)
                {
                    HandleTokenAcquisition(false);
                }
            }
            catch (Exception ex)
            {
                logger.Log(ex, "Error acquiring B2C token");
            }

            return AuthenticationResult != null;
        }

        public async Task<bool> AcquireTokenSilently()
        {
            try
            {
                var accounts = await publicClientApplication.GetAccountsAsync();
                var firstAccount = accounts.FirstOrDefault();
                var authenticationResult = await publicClientApplication.AcquireTokenSilentAsync(AuthConfig.Scopes, firstAccount);

                AuthenticationResult = authenticationResult;

                HandleTokenAcquisition(authenticationResult != null);

                return authenticationResult != null;
            }
            catch (Exception ex)
            {
                logger.Log(ex, "Error acquiring B2C token silently");
            }

            return false;
        }

        public async Task SignOut() 
        {
            var accounts = await publicClientApplication.GetAccountsAsync();
            foreach (var account in accounts) 
            {
                await publicClientApplication.RemoveAsync(account);
            }
        }

        void HandleTokenAcquisition(bool wasAcquiredSilently) 
        {
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(AuthenticationResult.IdToken);
            var emailsClaim = token.Claims.First(c => c.Type == "emails");
            if (emailsClaim != null)
            {
                var email = emailsClaim.Value;
                Email = email;
            }

            OnAuthChanged?.Invoke(this, new AuthChangedEventArgs
            {
                IsLoggedIn = AuthenticationResult != null && Email != null,
                WasTokenAcquiredSilently = wasAcquiredSilently,
            });
        }
    }
}
