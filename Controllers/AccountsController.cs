using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using coursesSystem.Models;
using coursesSystem.Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using coursesSystem.repository;
using AutoMapper;


namespace coursesSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private IConfiguration configuration;
        private IAccountRepository AccountRepository;
        private IAttendancesRepository attendancesRepository;

        public AccountsController( IAccountRepository accReposy , IAttendancesRepository attreposy,
        UserManager<AppUser> usermgr, IConfiguration config , RoleManager<IdentityRole> rol)
        {
            attendancesRepository = attreposy;
            configuration = config;
            AccountRepository = accReposy;
        }
        //-------------------------------------------------------------------------------------------------

        [Authorize(Roles = UserRoles.Professor)]
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] DetailsFormUser newUserDetails)
        {
            if (ModelState.IsValid)
            {
                IdentityResult result = await AccountRepository.CreateUserAndGetResultAsync(newUserDetails);
                if (result.Succeeded)
                {
                    return Ok(new Message(newUserDetails.name + " created successfuly!"));
                }
                foreach (IdentityError err in result.Errors)
                {
                    ModelState.AddModelError("errorArr", err.Description);
                }
            }
                return ValidationProblem();
        }
        //-------------------------------------------------------------------------------------------------

        [Authorize(Roles = UserRoles.Professor)]
        [HttpPost("createProfessor")]
        public async Task<IActionResult> CreateProfessorUser([FromBody] DetailsFormUser newUserDetails)
        {
            if (ModelState.IsValid)
            {
                IdentityResult result = await AccountRepository.CreateUserAndGetResultAsync(newUserDetails);
                if (result.Succeeded)
                {
                    AppUser newUser = await AccountRepository.GetUserByNameAsync(newUserDetails.name);

                    if (!await AccountRepository.IsRoleExistsAsync(UserRoles.Professor))
                        await AccountRepository.CreateRoleAsync(new IdentityRole(UserRoles.Professor));
                    if (!await AccountRepository.IsRoleExistsAsync(UserRoles.Student))
                        await AccountRepository.CreateRoleAsync(new IdentityRole(UserRoles.Student));
                    if (await AccountRepository.IsRoleExistsAsync(UserRoles.Professor))
                    {
                        await AccountRepository.AddToRoleAsync(newUser, UserRoles.Professor);
                    }
                    return Ok(new Message("Professor acount created successfuly!"));
                }
                foreach (IdentityError err in result.Errors)
                {
                    ModelState.AddModelError("errorArr", err.Description);
                }
            }
            return ValidationProblem();
        }
        //-------------------------------------------------------------------------------------------------

        [Authorize(Roles = UserRoles.Professor)]
        [HttpPatch]
        public async Task<IActionResult> ChangePassword([FromBody] managePassword model)
        {
            AppUser user = await AccountRepository.GetUserByNameAsync(model.studentName);
            if (user != null)
            {
                await AccountRepository.RemoveUserPasswordAsync(user);
                IdentityResult result = await AccountRepository.AddPasswordAsync(user, model.newPassword);
                if (result.Succeeded)
                {
                    return Ok(user);
                }
                foreach (IdentityError err in result.Errors)
                {
                    ModelState.AddModelError("errorArr", err.Description);
                }
                return ValidationProblem();
            }
            return Content("Somthing went wrong, please try again");
        }
        //-------------------------------------------------------------------------------------------------

        [Authorize]
        [HttpPatch("ChangePassword")]
        public async Task<IActionResult> ChangeYourOwnPassword([FromBody] managePassword model)
        {

            var currentUser = await AccountRepository.GetCurrentLoggedInUserAsync(User);   

            if (await AccountRepository.CheckPasswordAsync(currentUser , model.oldPassword))
            {
                if(model.newPassword != model.verifyNewPassword)
                {
                    return BadRequest("Please make sure your new password and verify password are the same");
                }

                await AccountRepository.RemoveUserPasswordAsync(currentUser);
                IdentityResult result = await AccountRepository.AddPasswordAsync(currentUser, model.newPassword);
                if (result.Succeeded)
                {
                    var message = new Message("success");
                    return Ok(message);
                }
                else
                {
                    foreach (IdentityError err in result.Errors)
                    {
                        ModelState.AddModelError("errorArr", err.Description);
                    }
                    return ValidationProblem();
                }
            }
            return Unauthorized();
        }
        //-------------------------------------------------------------------------------------------------

        [Authorize(Roles = UserRoles.Professor)]
        [HttpGet]
        public List<AppUserDto> GetUsers()
        {
            List<AppUserDto> users = AccountRepository.GetAllUsers();
            
            return users;
        }
        //-------------------------------------------------------------------------------------------------

        [Authorize(Roles = UserRoles.Professor)]
        [HttpDelete]
        public async Task<IActionResult> DeleteUser([FromBody] AppUserDto model)
        {
            AppUser user = await AccountRepository.GetUserByIdAsync(model.id);
            if(user == null || user.UserName != model.userName)
            {
                return Ok(new Message("The user was not found, please make sure the user id and name is correct."));
            }
            IdentityResult result = await AccountRepository.DeleteUserAndGetResult(user);
            if (result.Succeeded)
            {
                
                return Ok(new Message("success"));
            }
            return BadRequest();
        }
        //-------------------------------------------------------------------------------------------------

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await AccountRepository.GetUserByNameAsync(model.UserName);

            if (user != null && await AccountRepository.CheckPasswordAsync(user, model.Password))
            {
                var userRoles = await AccountRepository.GetRolesAsync(user);
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name , user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti , Guid.NewGuid().ToString())
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var authSignKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]));
                var token = new JwtSecurityToken(
                    issuer: configuration["JWT:ValidIssuer"],
                    audience: configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddMinutes(30),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSignKey, SecurityAlgorithms.HmacSha256)
                    );
                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    userId = user.Id,
                    expireDate = DateTime.Now.AddMinutes(30),
                    isProfessor = user.isProfessor,
                    email = user.Email
                }) ;
            }
            return Unauthorized();
        }
        //-------------------------------------------------------------------------------------------------

    }
}
