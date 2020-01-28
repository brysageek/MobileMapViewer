using System.Collections.Generic;
using Esri.ArcGISRuntime.Mapping;

namespace MobileMapViewer.Shared.Services
{
    interface IMobileMapService
    {
        IEnumerable<Map> GetMobileMaps();
    }
}
