using CRM.Models;
using CRM.View_Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CRM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration _configuration;

        public UserController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            _configuration = configuration;
        }

        [HttpGet]
        [Route("getUserInfo")]
        public async Task<IActionResult> getUserInfo()
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            User user = await userManager.FindByIdAsync(userId);
            if(user != null)
            {
                IList<string> userRole = await userManager.GetRolesAsync(user);

                UserInfo response = new UserInfo()
                {
                    Id = user.Id,
                    Username = user.UserName,
                    Email = user.Email,
                    Role = userRole[0]
                };

                return Ok(response);
            } else
            {
                return NotFound(new Response { Status = "Failed", Message = "User Not Found!"});
            }
            
        }
    }
}
