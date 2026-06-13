using GlitchGame.Shared;
using GlitchGame.Web.Repositories;

namespace GlitchGame.Web.Services;

public class PointsTracker
{
    private Dictionary<string, int> _userPoints = new Dictionary<string, int>(); // username, points
    private Dictionary<Districts, int> _districtPoints = new Dictionary<Districts, int>() {
        { Districts.Forge, 0},
        { Districts.Mirage, 0},
        { Districts.Neon, 0},
        { Districts.Pink, 0 },
        { Districts.Tokyo, 0 },
        { Districts.Tilted, 0 }
    };

    public event Action<string, int>? OnUserPointsChanged;
    public event Action<Districts, int>? OnDistrictPointsChanged;

    private readonly PointsRepository _pointsRepository;
    public PointsTracker(PointsRepository pointsRepository)
    {
        _pointsRepository = pointsRepository;

        _pointsRepository.LoadPoints();

        _userPoints = _pointsRepository.UserPoints;
        _districtPoints = _pointsRepository.DistrictPoints;
    }

    public void RegisterUserAction(string user, Districts districs, EventType action)
    {
        int points = action switch { 
            EventType.UserJoined => 5,
            EventType.GoodMessage => 10,
            EventType.BadMessage => -15,
            EventType.VeryBadMessage => -25,
            EventType.HadMessageRedacted => -50,
            EventType.RightAnswer => 20,
            EventType.WrongAnswer => 5,
            _ => 0
        };

        _userPoints[user] = _userPoints.GetValueOrDefault(user) + points;
        _districtPoints[districs] = _districtPoints.GetValueOrDefault(districs) + points;

        OnUserPointsChanged?.Invoke(user, _userPoints[user]);
        OnDistrictPointsChanged?.Invoke(districs, _districtPoints[districs]);

        _pointsRepository.DistrictPoints = _districtPoints;
        _pointsRepository.UserPoints = _userPoints;
        _pointsRepository.UpdatePoints();
    }

    public Dictionary<Districts, int> GetDistrictPoints() => _districtPoints;
    public Dictionary<string, int> GetUserPoints() => _userPoints;
}

public enum EventType
{
    UserJoined,
    GoodMessage,
    BadMessage,
    VeryBadMessage,
    HadMessageRedacted, // MASSIVE POINT LOSS
    RightAnswer,
    WrongAnswer // Still worths points, just less
}
