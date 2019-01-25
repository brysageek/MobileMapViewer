using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using MobileMapViewer.Services;
using MobileMapViewer.Views;
using Protection.Shared.Services;

namespace MobileMapViewer.ViewModels
{
    public class ViewModelLocator
    {
        public MainMapViewModel MapViewModel => SimpleIoc.Default.GetInstance<MainMapViewModel>();
        public LoginViewModel LoginViewModel => SimpleIoc.Default.GetInstance<LoginViewModel>();
        public PortalViewViewModel  PortalViewViewModel => SimpleIoc.Default.GetInstance<PortalViewViewModel>();
        public IAuthService TokenAuthService => SimpleIoc.Default.GetInstance<IAuthService>();
        public INavigationService NavigationService => SimpleIoc.Default.GetInstance<INavigationService>();

        public ViewModelLocator()
        {
            SimpleIoc.Default.Register<MainMapViewModel>(); 
            SimpleIoc.Default.Register<PortalViewViewModel>();
            SimpleIoc.Default.Register<IAuthService>(() => new TokenAuthenticationService("https://www.arcgis.com/sharing/rest"));

        
        }
    }
}
