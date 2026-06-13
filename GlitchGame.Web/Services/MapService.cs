using GlitchGame.Shared;
using GlitchGame.Shared.Dto;

namespace GlitchGame.Web.Services
{
    public class MapService
    {
        private PointsTracker _pointsTracker;
        public MapService(PointsTracker pointsTracker)
        {
            _pointsTracker = pointsTracker;
            _pointsTracker.OnDistrictPointsChanged += OnDistrictPointsChanged;
        }

        private void OnDistrictPointsChanged(Districts district, int newPoints)
        {
            var e = new MapUpdate() { District = district, NewPoints = newPoints };
            OnMapUpdated?.Invoke(e);
        }

   
        public event Action<MapUpdate>? OnMapUpdated;
    }
}
