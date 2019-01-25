using System;
using System.Threading.Tasks;
using Esri.ArcGISRuntime.Http;
using Esri.ArcGISRuntime.Security;
using MobileMapViewer.Services;

namespace Protection.Shared.Services
{
    public class TokenAuthenticationService : IAuthService
    {
        private readonly ServerInfo _serverInfo;
        private Credential _serverCredential;

        public Task<bool> Authenticate()
        {
            try
            {
                AuthenticationManager.Current.AddCredential(_serverCredential);
                return Task.FromResult(true);
            }
            catch (ArcGISWebException exception)
            {
                throw new Exception("Unable to authenticate user", exception);
            }
        }

        public async Task<bool> Authenticate(string username, string password)
        {
            try
            {
                _serverCredential = await AuthenticationManager.Current.GenerateCredentialAsync(_serverInfo.ServerUri, username, password);
                AuthenticationManager.Current.AddCredential(_serverCredential);
                return true;
            }
            catch (ArcGISWebException exception)
            {
                throw new Exception("Unable to authenticate user", exception);
            }
        }

        public void RemoveAuthentication()
        {
            AuthenticationManager.Current.RemoveCredential(_serverCredential);
        }

        public TokenAuthenticationService(string serverUrl)
        {
            _serverInfo = new ServerInfo
            {
                ServerUri = new Uri(serverUrl),
                TokenAuthenticationType = TokenAuthenticationType.ArcGISToken
            };

            AuthenticationManager.Current.RegisterServer(_serverInfo);
        }
    }
}
