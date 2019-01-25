using System.Threading.Tasks;
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



namespace MobileMapViewer.Services
{
    public class OAuthService : IAuthService
    {
        public Task<bool> Authenticate(string username, string password)
        {
            return Task.FromResult(true);
        }

        public void RemoveAuthentication()
        {
            throw new System.NotImplementedException();
        }
    }
}
