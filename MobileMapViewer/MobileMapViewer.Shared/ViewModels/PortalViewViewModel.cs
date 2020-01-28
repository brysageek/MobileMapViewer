using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Esri.ArcGISRuntime.Portal;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using MobileMapViewer.Shared.Services;

namespace MobileMapViewer.Shared.ViewModels
{
    public class PortalViewViewModel : ViewModelBase
    {
        private ArcGISPortal _portal;
        private IAuthService _authService;

        private ObservableCollection<PortalItem> _mobileMapsCollection;

        public ObservableCollection<PortalItem> MobileMapsCollection
        {
            get => _mobileMapsCollection;
            set
            {
                _mobileMapsCollection = value;
                RaisePropertyChanged(() => MobileMapsCollection);
            }
        }


        private readonly RelayCommand _getMaps;
        public ICommand GetMaps => _getMaps;


        [PreferredConstructor]
        public PortalViewViewModel(IAuthService authService)
        {
            _authService = authService;
            _mobileMapsCollection = new ObservableCollection<PortalItem>();
            GetContents();

            _getMaps = new RelayCommand(execute: async () => { });
        }

        private async void GetContents()
        {
            _portal = await ArcGISPortal.CreateAsync(new Uri("https://www.arcgis.com/sharing/rest"));
            var test = await _portal.User.GetContentAsync();

            foreach (var mmpk in test.Items.Where(pi => pi.Type == PortalItemType.MobileMapPackage))
            {
                MobileMapsCollection.Add(mmpk);
            }
        }
    }
}