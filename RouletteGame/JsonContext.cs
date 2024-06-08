using RouletteGame.Models;
using System.Text.Json.Serialization;

namespace RouletteGame
{
    [JsonSerializable(typeof(SpinResult))]
    [JsonSerializable(typeof(BetResponse))]
    public partial class RouletteGameJsonContext : JsonSerializerContext
    {
    }
}
