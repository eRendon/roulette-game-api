using System.Text.Json.Serialization;

namespace RouletteGame.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Balance { get; set; }
    }

    public class BalanceUpdateRequest
    {
        public int Amount { get; set; }
    }
}
