using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using MobileMapViewer.Services;
using MobileMapViewer.ViewModels;
using MobileMapViewer.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

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
            SimpleIoc.Default.Register<INavigationService>(() => nav);
            SimpleIoc.Default.Register(() => new LoginViewModel(
                SimpleIoc.Default.GetInstance<IAuthService>(),
                SimpleIoc.Default.GetInstance<INavigationService>(), "PortalViewPage"));

            var mainPage = new NavigationPage(new LoginPage());
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
