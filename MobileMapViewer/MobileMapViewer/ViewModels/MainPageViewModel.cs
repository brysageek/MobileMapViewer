using Esri.ArcGISRuntime.Mapping;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;

namespace MobileMapViewer.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private Map _mainMap;

        public Map MainMap
        {
            get => _mainMap;
            set
            {
                _mainMap = value; 
                RaisePropertyChanged(()=>MainMap);
            }
        }

        [PreferredConstructor]
        public MainPageViewModel()
        {
           MainMap = new Map(Basemap.CreateImagery());
        }

        public MainPageViewModel(Map map)
        {
            MainMap = map;
        }
    }
}
