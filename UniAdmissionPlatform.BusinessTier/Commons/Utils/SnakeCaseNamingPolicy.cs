using System.Text.Json;

namespace UniAdmissionPlatform.BusinessTier.Commons.Utils
{
    public class SnakeCaseNamingPolicy : JsonNamingPolicy
    {
        public override string ConvertName(string name)
        {
            return name.ToSnakeCase();
        }
    }
}