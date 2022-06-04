using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using coursesSystem.Models;
using coursesSystem.Dtos;
using AutoMapper;

namespace coursesSystem.repository
{
    public class AccountRepository : IAccountRepository
    {
        public AccountRepository(Context ctx, UserManager<AppUser> usrmngr , RoleManager<IdentityRole> rol, IMapper _mapper)
        {
            context = ctx;
            userManager = usrmngr;
            roleManager = rol;
            mapper = _mapper;
        }
        private IMapper mapper;
        private UserManager<AppUser> userManager;
        private Context context;
        private RoleManager<IdentityRole> roleManager;

        public async Task<AppUser> GetUserByNameAsync(string name)
        {
            AppUser user = await userManager.FindByNameAsync(name);
            return user;
        }

        public async Task<AppUser> GetUserByIdAsync(string id)
        {
            AppUser user = await userManager.FindByIdAsync(id);
            return user;
        }

        public async Task<AppUser> GetCurrentLoggedInUserAsync(ClaimsPrincipal User)
        {
            var currentUser = await userManager.GetUserAsync(User);
            return currentUser;
        }
        public async Task<IdentityResult> CreateUserAndGetResultAsync(DetailsFormUser newUserDetails)
        {
            AppUser user = new AppUser
            { UserName = newUserDetails.name, SecurityStamp = Guid.NewGuid().ToString(), Email = newUserDetails.email, isProfessor = newUserDetails.isProfessor };
            IdentityResult result = await userManager.CreateAsync(user, newUserDetails.password);
            return result;
        }
        public async Task RemoveUserPasswordAsync(AppUser user)
        {
            await userManager.RemovePasswordAsync(user);
        }
        public async Task<IdentityResult> AddPasswordAsync(AppUser user, string password)
        {
            IdentityResult result = await userManager.AddPasswordAsync(user, password);
            return result;
        }
        public async Task<Boolean> CheckPasswordAsync(AppUser user, string password)
        {
            Boolean isCorrectPassword = await userManager.CheckPasswordAsync(user, password);
            return isCorrectPassword;
        }
        public List<AppUserDto> GetAllUsers()
        {
            List<AppUser> users = userManager.Users.OrderBy(u => u.UserName).ToList();
            List<AppUserDto> userDtos = new List<AppUserDto>();
            foreach (AppUser user in users)
            {
                AppUserDto userDto = mapper.Map<AppUserDto>(user);
                userDtos.Add(userDto);
            }
            return userDtos;
        }
        public async Task<IdentityResult> DeleteUserAndGetResult(AppUser user)
        {
            IdentityResult result = await userManager.DeleteAsync(user);
            return result;
        }
        public async Task<bool> IsRoleExistsAsync(string role)
        {
            bool result = await roleManager.RoleExistsAsync(role);
            return result;
        }
        public async Task AddToRoleAsync(AppUser user, string role)
        {
            await userManager.AddToRoleAsync(user, role);
        }
        public async Task CreateRoleAsync(IdentityRole role)
        {
            await roleManager.CreateAsync(role);
        }
        public async Task<IList<string>> GetRolesAsync(AppUser user)
        {
            var roles = await userManager.GetRolesAsync(user);
            return roles;
        }
    }
}
