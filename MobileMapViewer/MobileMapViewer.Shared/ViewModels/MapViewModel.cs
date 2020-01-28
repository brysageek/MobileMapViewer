using Esri.ArcGISRuntime.Mapping;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;

namespace MobileMapViewer.Shared.ViewModels
{
    public class MapViewModel : ViewModelBase
    {
        private Map _mainMap;

        public Map MainMap
        {
            get => _mainMap;
            set
            {
                _mainMap = value;
                RaisePropertyChanged(()=> MainMap);
            }
        }

        [PreferredConstructor]
        public MapViewModel()
        {
            MainMap = new Map(Basemap.CreateImagery());
        }
    }
}
