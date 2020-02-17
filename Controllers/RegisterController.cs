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

    public class RegisterController : ControllerBase
    {
        private readonly HelpDeskContext _context;

        public RegisterController(HelpDeskContext context)
        {
            _context = context;
        }

        // TODO: Fix register
        [HttpPost]
        public async Task<ActionResult<Identity>> PostRegister(Login login)
        {
            // Very user doesn't exist
            var existingUser = await _context.User.FirstOrDefaultAsync(u => u.UserName == login.UserName);
            if (existingUser != null)
            {
                return BadRequest("Username already exists");
            }

            // Hash the password from register form in client
            string hashedPw = Crypto.HashPassword(login.Password);

            // Create user
            User newUser = await Factory.CreateUser(login, hashedPw, _context);

            // Create user role. 1 = USER, 2 = TEAM MEMBER
            UserRoles newUserRole = await Factory.CreateUserRoleAssociation(newUser.Id, 1, _context);

            // Create identity for front-end
            Identity identity = Factory.CreateIdentity(newUser.Id, _context);

            return CreatedAtAction("PostRegister", identity);
        }
    }
}