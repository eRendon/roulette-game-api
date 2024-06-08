using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RouletteGame.DB;
using RouletteGame.Models;

namespace RouletteGame.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Método para obtener el saldo de un usuario por su nombre
        [HttpGet("{name}")]
        public async Task<IActionResult> GetUserBalance(string name)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Name.ToLower() == name.ToLower());
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        // Método para actualizar el saldo de un usuario
        [HttpPost("{name}/update-balance")]
        public async Task<IActionResult> UpdateUserBalance(string name, [FromBody] BalanceUpdateRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Name.ToLower() == name.ToLower());
            if (user == null)
            {
                user = new User { Name = name, Balance = request.Amount };
                _context.Users.Add(user);
            }
            else
            {
                user.Balance += request.Amount;
            }
            await _context.SaveChangesAsync();
            return Ok(user);
        }
    }
}
