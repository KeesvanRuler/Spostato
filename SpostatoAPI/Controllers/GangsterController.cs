using CrimeGameBlazor_DataAccess;
using CrimeGameBlazor_DataAccess.Data;
using CrimeGameBlazor_DataAccess.Migrations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SpostatoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GangsterController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public GangsterController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }


        // GET api/<GangsterController>/5
        [Authorize]
        [HttpGet]
        public async Task<Gangster?> Get()
        {
            ApplicationUser CurrentUser = await _userManager.FindByNameAsync(HttpContext.User.Identity!.Name);
            return await _db.Gangsters.FindAsync(CurrentUser.GangsterId);
        }

        // POST api/<GangsterController>
        [Authorize]
        [HttpPost]
        public async Task<bool> Post([FromBody] string Name)
        {
            try
            {
                ApplicationUser CurrentUser = await _userManager.FindByNameAsync(HttpContext.User.Identity!.Name);
                Gangster? CurrentGangster = CurrentUser.CurrentGangster;
                if (CurrentGangster == null)
                {
                    Gangster NewGangster = new()
                    {
                        User = CurrentUser,
                        Name = Name,
                    };
                    _db.Gangsters.Add(NewGangster);
                    await _db.SaveChangesAsync();
                    CurrentUser.CurrentGangster = NewGangster;
                    CurrentUser.GangsterId = NewGangster.Id;
                    await _db.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
