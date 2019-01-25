using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using MobileMapViewer.Services;
using Protection.Shared.Services;

namespace MobileMapViewer.ViewModels
{
    public class ViewModelLocator
    {
        public MainPageViewModel MainPageViewModel => SimpleIoc.Default.GetInstance<MainPageViewModel>();
        public LoginViewModel LoginViewModel => SimpleIoc.Default.GetInstance<LoginViewModel>();
        public PortalViewViewModel  PortalViewViewModel => SimpleIoc.Default.GetInstance<PortalViewViewModel>();

        public ViewModelLocator()
        {
            SimpleIoc.Default.Register<MainPageViewModel>(); 
            SimpleIoc.Default.Register<PortalViewViewModel>();
            SimpleIoc.Default.Register<IAuthService>(() => new TokenAuthenticationService("https://www.arcgis.com/sharing/rest"));
        }
    }
}
