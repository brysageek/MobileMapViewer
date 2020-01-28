using GalaSoft.MvvmLight.Ioc;
using MobileMapViewer.Shared.Services;

namespace MobileMapViewer.Shared.ViewModels
{
    public class ViewModelLocator
    {
        public MainPageViewModel MainPageViewModel => SimpleIoc.Default.GetInstance<MainPageViewModel>();
        public LoginViewModel LoginViewModel => SimpleIoc.Default.GetInstance<LoginViewModel>();
        public PortalViewViewModel  PortalViewViewModel => SimpleIoc.Default.GetInstance<PortalViewViewModel>();

        public MapViewModel MapViewModel => SimpleIoc.Default.GetInstance<MapViewModel>();
        public ViewModelLocator()
        {
            SimpleIoc.Default.Register<MainPageViewModel>(); 
            SimpleIoc.Default.Register<PortalViewViewModel>();
            SimpleIoc.Default.Register<MapViewModel>();
            SimpleIoc.Default.Register<IAuthService>(()=> new OAuthService("https://www.arcgis.com/sharing/rest)", "esXAtZqh4vJB29qq", "f794ea3f9f4e48e7892f056c78c5f9e4", "https://arcgis.com/sharing/rest"));
            //SimpleIoc.Default.Register<IAuthService>(() => new TokenAuthenticationService("https://www.arcgis.com/sharing/rest"));
        }
    }
}
