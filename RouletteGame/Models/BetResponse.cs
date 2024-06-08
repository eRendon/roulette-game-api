namespace RouletteGame.Models
{
    public class BetResponse : SpinResult
    {
        public bool IsWin { get; set; }
        public decimal WinAmount { get; set; }
    }
}
