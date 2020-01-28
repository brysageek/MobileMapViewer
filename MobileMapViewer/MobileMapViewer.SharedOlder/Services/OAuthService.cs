using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Esri.ArcGISRuntime.Security;

#if __IOS__
using Xamarin.Auth;
using Xamarin.Forms.Platform.iOS;
using UIKit;
#endif

#if __ANDROID__
using Android.App;
using Xamarin.Auth;
using System.IO;
#endif


namespace MobileMapViewer.Shared.Services
{
    public class OAuthService : IAuthService
    {
        private readonly string _serverUrl;
        private readonly string _appClientId;
        private readonly string _clientSecret;
        private readonly string _oathClientRedirect;


        public OAuthService(string serverUrl, string appClientId, string clientSecret, string oathClientRedirect=@"urn:ietf:wg:oauth:2.0:oob")
        {
            _serverUrl = serverUrl;
            _appClientId = appClientId;
            _clientSecret = clientSecret;
            _oathClientRedirect = oathClientRedirect;
        }

        public Task<bool> Authenticate(string username, string password)
        {
            SetOauthInfo();
            return Task.FromResult(true);
        }

        private async void SetOauthInfo()
        {
            var serverInfo = new ServerInfo
            {
                ServerUri = new Uri(_serverUrl),
                TokenAuthenticationType = TokenAuthenticationType.OAuthImplicit,
                OAuthClientInfo = new OAuthClientInfo
                {
                    ClientId = _appClientId,
                    RedirectUri = new Uri(_oathClientRedirect)
                }
            };

            if (!string.IsNullOrEmpty(_clientSecret))
            {
                // Use OAuthAuthorizationCode if you need a refresh token (and have specified a valid client secret).
                serverInfo.TokenAuthenticationType = TokenAuthenticationType.OAuthAuthorizationCode;
                serverInfo.OAuthClientInfo.ClientSecret = _clientSecret;
            }

       

#if __ANDROID__ || __IOS__
            AuthenticationManager.Current.OAuthAuthorizeHandler = this;
#endif

            var challengeRequest = new CredentialRequestInfo
            {
                GenerateTokenOptions = new GenerateTokenOptions
                {
                    TokenAuthenticationType = TokenAuthenticationType.OAuthImplicit
                },
                ServiceUri = new Uri(_serverUrl)
            };

            AuthenticationManager.Current.RegisterServer(serverInfo);

            AuthenticationManager.Current.ChallengeHandler = new ChallengeHandler(CreateCredentialAsync);
            var cred = await AuthenticationManager.Current.GetCredentialAsync(challengeRequest, false);

        }

        public void RemoveAuthentication()
        {
            throw new System.NotImplementedException();
        }

        private async Task<Credential> CreateCredentialAsync(CredentialRequestInfo info)
        {
            Credential credential = null;

            try
            {
                // IOAuthAuthorizeHandler will challenge the user for OAuth credentials.
                credential = await AuthenticationManager.Current.GenerateCredentialAsync(info.ServiceUri);
            }
            catch (TaskCanceledException)
            {
                return credential;
            }
            catch (Exception)
            {
                // Exception will be reported in calling function.
                throw;
            }

            return credential;
        }

        #region IOAuthAuthorizationHandler implementation

        // Use a TaskCompletionSource to track the completion of the authorization.
        private TaskCompletionSource<IDictionary<string, string>> _taskCompletionSource;

        // IOAuthAuthorizeHandler.AuthorizeAsync implementation.
        public Task<IDictionary<string, string>> AuthorizeAsync(Uri serviceUri, Uri authorizeUri, Uri callbackUri)
        {
            // If the TaskCompletionSource is not null, authorization may already be in progress and should be canceled.
            if (_taskCompletionSource != null)
            {
                // Try to cancel any existing authentication task.
                _taskCompletionSource.TrySetCanceled();
            }

            // Create a task completion source.
            _taskCompletionSource = new TaskCompletionSource<IDictionary<string, string>>();
#if __ANDROID__ || __IOS__
#if __ANDROID__
            // Get the current Android Activity
            var activity = Xamarin.Forms.Forms.Context as Activity; 
#endif
#if __IOS__
            // Get the current iOS ViewController.
            UIViewController viewController = null;
            Device.BeginInvokeOnMainThread(() =>
            {
                viewController = UIApplication.SharedApplication.KeyWindow.RootViewController;
            });
#endif
            // Create a new Xamarin.Auth.OAuth2Authenticator using the information passed in.
            Xamarin.Auth.OAuth2Authenticator authenticator = new Xamarin.Auth.OAuth2Authenticator(
                clientId: AppClientId,
                scope: "",
                authorizeUrl: authorizeUri,
                redirectUrl: callbackUri)
            {
                ShowErrors = false
            };

            // Allow the user to cancel the OAuth attempt.
            authenticator.AllowCancel = true;

            // Define a handler for the OAuth2Authenticator.Completed event.
            authenticator.Completed += (sender, authArgs) =>
            {
                try
                {
#if __IOS__
                    // Dismiss the OAuth UI when complete.
                    viewController.DismissViewController(true, null);
#endif

                    // Check if the user is authenticated.
                    if (authArgs.IsAuthenticated)
                    {
                        // If authorization was successful, get the user's account.
                        Xamarin.Auth.Account authenticatedAccount = authArgs.Account;

                        // Set the result (Credential) for the TaskCompletionSource.
                        _taskCompletionSource.SetResult(authenticatedAccount.Properties);
                    }
                    else
                    {
                        throw new Exception("Unable to authenticate user.");
                    }
                }
                catch (Exception ex)
                {
                    // If authentication failed, set the exception on the TaskCompletionSource.
                    _taskCompletionSource.TrySetException(ex);

                    // Cancel authentication.
                    authenticator.OnCancelled();
                }
                finally
                {
                    // Dismiss the OAuth login.
#if __ANDROID__ 
                    activity.FinishActivity(99);
#endif
                }
            };

            // If an error was encountered when authenticating, set the exception on the TaskCompletionSource.
            authenticator.Error += (sndr, errArgs) =>
            {
                // If the user cancels, the Error event is raised but there is no exception ... best to check first.
                if (errArgs.Exception != null)
                {
                    _taskCompletionSource.TrySetException(errArgs.Exception);
                }
                else
                {
                    // Login canceled: dismiss the OAuth login.
                    if (_taskCompletionSource != null)
                    {
                        _taskCompletionSource.TrySetCanceled();
#if __ANDROID__ 
                        activity.FinishActivity(99);
#endif
                    }
                }

                // Cancel authentication.
                authenticator.OnCancelled();
            };

            // Present the OAuth UI so the user can enter user name and password.
#if __ANDROID__
            var intent = authenticator.GetUI(activity);
            activity.StartActivityForResult(intent, 99);
#endif
#if __IOS__
            // Present the OAuth UI (on the app's UI thread) so the user can enter user name and password.
            Device.BeginInvokeOnMainThread(() =>
            {
                viewController.PresentViewController(authenticator.GetUI(), true, null);
            });
#endif

#endif // (If Android or iOS)
            // Return completion source task so the caller can await completion.
            return _taskCompletionSource.Task;
        }
        #endregion
    }
}