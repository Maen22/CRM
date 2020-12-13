using CRM.Models;
using CRM.View_Models;
using Microsoft.AspNetCore.Http;
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
        private readonly IHttpContextAccessor _httpContextAccessor;
        

        public UserController(IHttpContextAccessor httpContextAccessor, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            this._httpContextAccessor = httpContextAccessor;
            this.userManager = userManager;
            this.roleManager = roleManager;
            _configuration = configuration;
        }

        [HttpGet]
        [Route("getUserInfo")]
        public async Task<IActionResult> getUserInfo()
        {
            //ClaimsPrincipal currentUser = this.User;
            var username = HttpContext.User.Identity.Name;
            Console.WriteLine(username);
            var user = await userManager.FindByNameAsync(username);

            if (user != null)
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
