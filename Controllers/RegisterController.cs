using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Helpdesk.Models;
using Helpdesk.Services;
using CryptoHelper;

namespace Helpdesk.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class RegisterController : ControllerBase
    {
        private readonly HelpDeskContext _context;
        private readonly ILoginService _userService;

        public RegisterController(HelpDeskContext context, ILoginService userService)
        {
            _context = context;
            _userService = userService;
        }

        [HttpPost]
        public async Task<ActionResult<Identity>> PostRegister(Login login)
        {
            // Very user exists
            var existingUser = await _context.User.FirstOrDefaultAsync(u => u.UserName == login.UserName);
            if (existingUser != null)
            {
                return BadRequest("Username already exists");
            }

            // Hash the password
            string hashedPw = Crypto.HashPassword(login.Password);

            // Create user
            User newUser = await Factory.CreateUser(login, hashedPw, _context);

            // Create user role
            UserRoles newUserRole = await Factory.CreateUserRoleAssociation(newUser.Id, RoleTitle.User, _context);

            // Create identity
            Identity identity = Factory.CreateIdentity(newUser.Id, _context);

            // Append token
            _userService.CreateToken(ref identity);

            return CreatedAtAction("PostRegister", identity);
        }
    }
}