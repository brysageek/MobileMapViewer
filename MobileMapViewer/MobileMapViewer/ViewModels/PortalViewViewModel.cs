using System;
using System.Collections.ObjectModel;
using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.Portal;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;

namespace MobileMapViewer.ViewModels
{
    public class PortalViewViewModel : ViewModelBase
    {
        private ObservableCollection<Map> _mobileMapsCollection;


        public ObservableCollection<Map> MobileMapsCollection
        {
            get => _mobileMapsCollection;
            set
            {
                _mobileMapsCollection = value;
                RaisePropertyChanged(() => MobileMapsCollection);
            }
        }

        [PreferredConstructor]
        public PortalViewViewModel()
        {
            _mobileMapsCollection = new ObservableCollection<Map>();
            
            
        }


        private async void TestMobile()
        {
            var test = await MobileMapPackage.OpenAsync(":");
        }

        public PortalViewViewModel(ArcGISPortal arcgisPortal)
        {
            throw new Exception("Not Implemented");
        }
    }
}