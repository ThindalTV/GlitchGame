using GlitchGame.Shared;

namespace GlitchGame.Web.Repositories
{
    public class PointsRepository
    {
        public Dictionary<string, int> UserPoints { get; set; } = new Dictionary<string, int>();
        public Dictionary<Districts, int> DistrictPoints { get; set; } = new Dictionary<Districts, int>();

        public void LoadPoints()
        {
            // Load points from json

            // If user file exists, load it
            var userFile = "user_points.json";
            if (File.Exists(userFile))
            {
                var fileContent = File.ReadAllText(userFile);

                UserPoints = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, int>>(fileContent) ?? new Dictionary<string, int>();
            }

            // If district file exists, load it
            var districtFile = "district_points.json";
            if(File.Exists(districtFile))
            {
                var fileContent = File.ReadAllText(districtFile);
                var districtPoints = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, int>>(fileContent) ?? new Dictionary<string, int>();
                // Convert string keys to Districts enum

                // Go through the districts and get the points if they exist
                foreach(var district in Enum.GetValues(typeof(Districts)).Cast<Districts>())
                {
                    if (districtPoints.TryGetValue(district.ToString(), out int points))
                    {
                        DistrictPoints[district] = points;
                    } else
                    {
                        DistrictPoints[district] = 0; // Default to 0 if not found
                    }
                }
            }
        }

        public void UpdatePoints()
        {
            var userFile = "user_points.json";
            var districtFile = "district_points.json";

            var userJson = System.Text.Json.JsonSerializer.Serialize(UserPoints);
            var districtJson = System.Text.Json.JsonSerializer.Serialize(DistrictPoints.ToDictionary(kvp => kvp.Key.ToString(), kvp => kvp.Value));

            File.WriteAllText(userFile, userJson);
            File.WriteAllText(districtFile, districtJson);
        }
    }
}
