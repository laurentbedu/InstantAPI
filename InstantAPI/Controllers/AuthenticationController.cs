using InstantAPI.Helpers;
using InstantAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace InstantAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AuthenticationController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(AppUser appUser)
        {
            AppUser? user = _context.AppUsers.SingleOrDefault(user => user.Login == appUser.Login);

            if (user != null)
            {
                return BadRequest("User already exists");
            }

            SecurityHelper.HashAppUserPassword(ref appUser);

            _context.AppUsers.Add(appUser);
            int rowCount = await _context.SaveChangesAsync();

            if (rowCount == 1 && appUser.Id > 0)
            {
                return CreatedAtAction("Register", new { id = appUser.Id }, new { login = appUser.Login });
            }

            return BadRequest();

        }

    }
}
