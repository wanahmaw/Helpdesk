using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Helpdesk.Models;
using Helpdesk.Services;
using CryptoHelper;

namespace Helpdesk.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly HelpDeskContext _context;
        private readonly ILoginService _userService;

        public LoginController(HelpDeskContext context, ILoginService userService)
        {
            _context = context;
            _userService = userService;
        }

        // TODO: Fix login
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<Identity>> PostLogin(Login login)
        {
            // https://stackoverflow.com/questions/17693918/system-web-helpers-crypto-wheres-the-salt

            // Verify user
            var user = await _context.User.FirstOrDefaultAsync(u => u.UserName == login.UserName);
            if (user == null)
            {
                return NotFound("User doesn't exist");
            }

            // Retrieve the saved hashed password from database
            string savedHashedPW = user.UserPassword;

            // Verify password
            if (Crypto.VerifyHashedPassword(savedHashedPW, login.Password) == false)
            {
                return Unauthorized("Invalid password");
            }

            // Create identity
            Identity identity = Factory.CreateIdentity(user.Id, _context);

            // Append token to identity
            _userService.CreateToken(ref identity);

            return CreatedAtAction("PostLogin", identity);
        }
    }
}