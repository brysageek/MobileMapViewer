using System.Collections.ObjectModel;
using System.Windows.Input;
using Esri.ArcGISRuntime.Mapping;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using MobileMapViewer.Shared.Services;
using MobileMapViewer.Shared.Utilities;

namespace MobileMapViewer.Shared.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private ObservableCollection<Map> _mobileMapCollection;

        public ObservableCollection<Map> MobileMapCollection
        {
            get => _mobileMapCollection;
            set
            {
                _mobileMapCollection = value;
                RaisePropertyChanged(() => MobileMapCollection);
            }
        }

        private readonly RelayCommand _signInCommand;
        public ICommand SignInCommand => _signInCommand;

        private readonly RelayCommand _getMapsCommand;
        public ICommand GetMapsCommand => _getMapsCommand;

        private readonly RelayCommand _loadMapCommand;
        public ICommand LoadMapCommand => _loadMapCommand;

        [PreferredConstructor]
        public MainPageViewModel()
        {
            _signInCommand = new RelayCommand(() =>
            {
                SimpleIoc.Default.GetInstance<IAuthService>().Authenticate("", "");
            });

            _getMapsCommand = new RelayCommand(() =>
            {
                SimpleIoc.Default.GetInstance<INavigationService>().NavigateTo("PortalViewPage");
            });

            _loadMapCommand = new RelayCommand(()=>
            {
                //SimpleIoc.Default.GetInstance<MapViewModel>().MainMap = map;
                SimpleIoc.Default.GetInstance<INavigationService>().NavigateTo("MapPage");
            });

            MobileMapCollection = new ObservableCollection<Map>();
            LoadMobileMaps();
        }

        private async void LoadMobileMaps()
        {
            foreach (var mmpkString in MobileMapPackageHelper.FindAll())
            {
                if (await MobileMapPackage.IsDirectReadSupportedAsync(mmpkString))
                {
                    var mobileMapPackage = await MobileMapPackage.OpenAsync(mmpkString);
                    foreach (var map in mobileMapPackage.Maps)
                    {
                        MobileMapCollection.Add(map);
                    }
                }
                else
                {
                    if (!MobileMapPackageHelper.IsUnpacked(mmpkString))
                    {
                        await MobileMapPackage.UnpackAsync(mmpkString,
                            MobileMapPackageHelper.GetUnpackedLocation(mmpkString));
                        MobileMapPackageHelper.Delete(mmpkString);
                    }

                    var unpackedMobileMapPackage =
                        await MobileMapPackage.OpenAsync(MobileMapPackageHelper.GetUnpackedLocation(mmpkString));

                    foreach (var map in unpackedMobileMapPackage.Maps)
                    {
                        MobileMapCollection.Add(map);
                    }
                }
            }
        }
    }
}