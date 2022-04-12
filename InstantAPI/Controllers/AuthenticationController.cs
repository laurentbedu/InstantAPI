using InstantAPI.Helpers;
using InstantAPI.Models;
using Microsoft.AspNetCore.Mvc;
using static InstantAPI.Helpers.DtoRecordsHelper;

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
        public async Task<IActionResult> Register(UserDto userDto)
        {
            AppUser? storedUser = _context.AppUsers.SingleOrDefault(user => user.Login == userDto.Login);

            if (storedUser != null)
            {
                return BadRequest("User already exists");
            }

            AppUser appUser = new AppUser() { Login = userDto.Login, Password = userDto.Password };
            SecurityHelper.HashAppUserPassword(ref appUser);

            _context.AppUsers.Add(appUser);
            int rowCount = await _context.SaveChangesAsync();

            if (rowCount == 1 && appUser.Id > 0)
            {
                return CreatedAtAction("Register", new { id = appUser.Id }, new { login = appUser.Login });
            }

            return BadRequest();

        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(UserDto userDto)
        {
            AppUser? storedUser = _context.AppUsers.Single(user => user.Login == userDto.Login);

            _context.Entry(storedUser).Reference("IdRoleNavigation").Load();

            if (storedUser == null)
            {
                return BadRequest("Invalid Credentials");
            }

            AppUser appUser = new AppUser() { Login = userDto.Login, Password = userDto.Password };
            if (!SecurityHelper.VerifyAppUserPassword(storedUser, appUser))
            {
                return BadRequest("Invalid Credentials");
            }

            return Ok(new { login = storedUser.Login, role = storedUser.IdRoleNavigation.Name });

        }

    }
}
