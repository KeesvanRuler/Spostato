using Microsoft.AspNetCore.Identity;
using SpostatoDAL.Models;
using SpostatoDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BlazorWebApp.Services
{
    public interface IGangsterService
    {
        Task<Gangster> GetCurrentAliveGangsterAsync();
        Task UpdateGangsterAsync(Gangster gangster);
    }

    public class GangsterService : IGangsterService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GangsterService(ApplicationDbContext context,
                               UserManager<ApplicationUser> userManager,
                               IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Gangster> GetCurrentAliveGangsterAsync()
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            if (user == null)
            {
                return null;
            }

            return await _context.Gangsters
                .FirstOrDefaultAsync(g => g.User.Id == user.Id && g.IsAlive);
        }

        public async Task UpdateGangsterAsync(Gangster gangster)
        {
            _context.Update(gangster);
            await _context.SaveChangesAsync();
        }
    }
}
