using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api_back.Data;
using api_back.Models;
using Hasher = BCrypt.Net.BCrypt;

namespace api_back.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ApiDbContext _context;

        public UsersController(ApiDbContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> RegisterUser(RegisterRequest request)
        {
            string passwordHash = Hasher.HashPassword(request.Password);

            var newUser = new User
            {
                Email = request.Email,
                PasswordHash = passwordHash,
                Role = request.Role
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUserById), new { id = newUser.Id }, newUser);
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserResponse>> Login(LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest("Email e senha são obrigatórios");
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            
            if (user == null || !Hasher.Verify(request.Password, user.PasswordHash))
            {
                return Unauthorized("Email ou senha inválidos");
            }

            return Ok(new UserResponse
            {
                Id = user.Id,
                Email = user.Email,
                Role = user.Role
            });
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserResponse>>> GetUsers()
        {
            return await _context.Users
                .Select(user => new UserResponse
                {
                    Id = user.Id,
                    Email = user.Email,
                    Role = user.Role
                })
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserResponse>> GetUserById(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            var userResponse = new UserResponse
            {
                Id = user.Id,
                Email = user.Email,
                Role = user.Role
            };

            return userResponse;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, RegisterRequest request)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            user.Email = request.Email;
            user.Role = request.Role;
            if (!string.IsNullOrEmpty(request.Password))
            {
                user.PasswordHash = Hasher.HashPassword(request.Password);
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}