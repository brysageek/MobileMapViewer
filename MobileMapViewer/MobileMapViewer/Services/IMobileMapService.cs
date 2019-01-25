using System.Collections.Generic;
using Esri.ArcGISRuntime.Mapping;

namespace MobileMapViewer.Services
{
    interface IMobileMapService
    {
        IEnumerable<Map> GetMobileMaps();
    }
}
