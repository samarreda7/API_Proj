using APIproject.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Projectapi.DTO;
using Projectapi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Projectapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration config;
        public AccountController(UserManager<ApplicationUser> UserManager , IConfiguration config)
        {
            userManager = UserManager;
            this.config = config;
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDTO userFromreq)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser();
                user.UserName = userFromreq.UserName;
                user.Email = userFromreq.Email;
                IdentityResult result =
                  await userManager.CreateAsync(user, userFromreq.Password);
                if (result.Succeeded)
                {
                    return Ok("create");
                }
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("passward Invalid",item.Description);
                }

            }
            return BadRequest(ModelState);

        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDTO userFromreq)
        {
            if (ModelState.IsValid)
            {

                ApplicationUser userfromDb = await userManager.FindByNameAsync(userFromreq.Username);
                if (userfromDb != null)
                {

                    bool found = await userManager.CheckPasswordAsync(userfromDb, userFromreq.Password);
                    if (found == true)
                    {
                        List<Claim> userClaim = new List<Claim>();
                        userClaim.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
                        userClaim.Add(new Claim(ClaimTypes.NameIdentifier, userfromDb.Id));
                        userClaim.Add(new Claim(ClaimTypes.Name, userfromDb.UserName));
                        var userRoles = await userManager.GetRolesAsync(userfromDb);
                        foreach (var role in userRoles)
                        {
                            userClaim.Add(new Claim(ClaimTypes.Role, role));
                        }
                        var signInKey =
                            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt : SecritKey"]));
                        SigningCredentials signCred = new SigningCredentials(signInKey, SecurityAlgorithms.HmacSha256);

                        JwtSecurityToken mytoken = new JwtSecurityToken(
                            audience: config["Jwt : AudienceIp"],
                            issuer: config["Jwt : IssuerIp"],
                            expires: DateTime.Now.AddHours(1),
                            claims: userClaim,
                            signingCredentials: signCred
                            );
                        return Ok(new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(mytoken),
                            expiration = DateTime.Now.AddHours(1)
                        });


                    }
                }
                ModelState.AddModelError("UserName", "UserName Or Password Invalid");
            }
            return BadRequest(ModelState);

        }
    }
}
