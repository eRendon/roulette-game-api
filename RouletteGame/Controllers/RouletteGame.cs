using Microsoft.AspNetCore.Mvc;
using RouletteGame.DB;
using RouletteGame.Models;
using System;

namespace RouletteGame.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class RouletteController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private SpinResult _lastSpinResult;
        private static readonly Dictionary<int, string> numberColors = new Dictionary<int, string>
    {
        { 0, "green" },
        { 32, "red" }, { 19, "red" }, { 21, "red" }, { 25, "red" }, { 34, "red" },
        { 27, "red" }, { 36, "red" }, { 30, "red" }, { 23, "red" }, { 5, "red" },
        { 16, "red" }, { 1, "red" }, { 14, "red" }, { 9, "red" }, { 18, "red" },
        { 7, "red" }, { 12, "red" }, { 3, "red" },
        { 15, "black" }, { 4, "black" }, { 2, "black" }, { 17, "black" }, { 6, "black" },
        { 13, "black" }, { 11, "black" }, { 8, "black" }, { 10, "black" }, { 24, "black" },
        { 33, "black" }, { 20, "black" }, { 31, "black" }, { 22, "black" }, { 29, "black" },
        { 28, "black" }, { 35, "black" }, { 26, "black" }
    };

        public RouletteController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Método para generar un número aleatorio y un color
        [HttpGet("spin")]
        public IActionResult SpinRoulette()
        {
            var random = new Random();
            int number = random.Next(0, 37);
            string color = GetColor(number);
            _lastSpinResult = new SpinResult
            {
                Number = number,
                Color = color
            };
            return Ok(_lastSpinResult);
        }

        public static string GetColor(int number)
        {
            if (numberColors.TryGetValue(number, out var color))
            {
                return color;
            }
            else
            {
                throw new InvalidOperationException("Número inválido en la ruleta.");
            }
        }

        [HttpPost("bet")]
        public IActionResult PlaceBet([FromBody] BetRequest betRequest)
        {
            IActionResult spinResults = SpinRoulette();
            if (!(spinResults is OkObjectResult))
            {
                return spinResults;
            }
            SpinResult lastSpinResult = (SpinResult)((OkObjectResult) spinResults).Value;
            
            bool isWin = false;
            decimal winAmount = 0;

            // Verificar si el tipo de apuesta es "color" y si coincide con el color obtenido
            if (betRequest.Type == "color" && betRequest.Value.ToLower() == lastSpinResult.Color.ToLower())
            {
                isWin = true;
                winAmount = betRequest.Amount / 2;
            }
            // Verificar si el tipo de apuesta es "even" (pares) o "odd" (impares)
            else if ((betRequest.Type == "even" && lastSpinResult.Number % 2 == 0) || (betRequest.Type == "odd" && lastSpinResult.Number % 2 != 0))
            {
                isWin = true;
                winAmount = betRequest.Amount;
            }
            // Verificar si el tipo de apuesta es "number" y si coincide con el número obtenido y el color
            else if (betRequest.Type == "number" && betRequest.Value == $"{lastSpinResult.Number} {lastSpinResult.Color}")
            {
                isWin = true;
                winAmount = betRequest.Amount * 3;
            }

            // Devolver el resultado de la apuesta
            var betResponse = new BetResponse
            {
                IsWin = isWin,
                WinAmount = isWin ? winAmount : 0,
                Color = lastSpinResult.Color,
                Number = lastSpinResult.Number,
            };
            return Ok(betResponse);
        }
    }
}
