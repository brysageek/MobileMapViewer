using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using MobileMapViewer.Shared.ViewModels;
using MobileMapViewer.Shared.Views;
using MobileMapViewer.Shared.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using NavigationService = MobileMapViewer.Shared.Services.NavigationService;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace MobileMapViewer
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            var nav = new NavigationService();
            nav.RegisterView("MainPage", typeof(MainPage));
            nav.RegisterView("LoginPage", typeof(LoginPage));
            nav.RegisterView("PortalViewPage", typeof(PortalViewPage));
            nav.RegisterView("MapPage", typeof(MapPage));
            //nav.RegisterView("MapPage", typeof(MapPage));
            SimpleIoc.Default.Register<INavigationService>(() => nav);
            SimpleIoc.Default.Register(() => new LoginViewModel(SimpleIoc.Default.GetInstance<IAuthService>(),SimpleIoc.Default.GetInstance<INavigationService>(), "PortalViewPage"));

            var mainPage = new NavigationPage(new MainPage());
            MainPage = mainPage;
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
