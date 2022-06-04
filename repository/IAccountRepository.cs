using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using coursesSystem.Models;
using coursesSystem.Dtos;

namespace coursesSystem.repository
{
    public interface IAccountRepository
    {
       Task<AppUser> GetUserByNameAsync(string name);
       Task<AppUser> GetUserByIdAsync(string id);
       Task<AppUser> GetCurrentLoggedInUserAsync(ClaimsPrincipal User);
       Task<IdentityResult> CreateUserAndGetResultAsync(DetailsFormUser newUserDetails);
       Task RemoveUserPasswordAsync(AppUser user);
       Task<IdentityResult> AddPasswordAsync(AppUser user, string password);
       Task<Boolean> CheckPasswordAsync(AppUser user, string password);
       List<AppUserDto> GetAllUsers();
       Task<IdentityResult> DeleteUserAndGetResult(AppUser user);
       Task<bool> IsRoleExistsAsync(string role);
       Task AddToRoleAsync(AppUser user, string role);
       Task CreateRoleAsync(IdentityRole role);
       Task<IList<string>> GetRolesAsync(AppUser user);
    }
}
