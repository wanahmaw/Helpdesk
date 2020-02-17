using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Helpdesk.Models;
using CryptoHelper;

namespace Helpdesk.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class LoginController : ControllerBase
    {
        private readonly HelpDeskContext _context;

        public LoginController(HelpDeskContext context)
        {
            _context = context;
        }

        // TODO: Fix login
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

            // Create identity for front-end
            Identity identity = Factory.CreateIdentity(user.Id, _context);

            return CreatedAtAction("PostLogin", identity);
        }
    }
}