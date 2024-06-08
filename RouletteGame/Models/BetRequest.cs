namespace RouletteGame.Models
{
    public class BetRequest
    {
        public decimal Amount { get; set; } // Monto de la apuesta
        public string Type { get; set; } // Tipo de apuesta: "color", "even" (pares), "odd" (impares), "number"
        public string Value { get; set; } // Valor de la apuesta: puede ser el color, "even" o "odd", o número y color específicos
    }
}
