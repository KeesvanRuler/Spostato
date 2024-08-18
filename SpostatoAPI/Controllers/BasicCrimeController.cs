using CrimeGameBlazor_DataAccess.Data;
using CrimeGameBlazor_DataAccess;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CrimeGameBlazor_Models;
using static CrimeGameBlazor_API.Helper.RNG;

namespace SpostatoAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BasicCrimeController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public BasicCrimeController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }


        [Authorize]
        [HttpGet]
        public async Task<IActionResult> ShootingRangeAsync()
        {
            ApplicationUser CurrentUser = await _userManager.FindByNameAsync(HttpContext.User.Identity!.Name);
            Gangster? Gangster = await _db.Gangsters.FindAsync(CurrentUser.GangsterId);
            if (Gangster == null || Gangster.InPrisonUntill > DateTime.Now ||  Gangster.NextBasicCrimeAt > DateTime.Now)
                return BadRequest();
            if (Gangster.ShootingSkill == 100.0)
                return Ok("You are already at the maximum shooting skill level.");
            SuccesFailOrJail result = IsSuccesFailOrJail(25, 50);
            if (result == SuccesFailOrJail.Succes)
                Gangster.ShootingSkill += 0.1;
            else if (result == SuccesFailOrJail.Jail)
            {
                Gangster.InPrisonUntill = DateTime.Now.AddSeconds(GetRandomNumber(60, 120));
                //Gangster.PrisonId = 1;
            }
            Gangster.NextBasicCrimeAt = DateTime.Now.AddSeconds(GetRandomNumber(60, 69));
            await _db.SaveChangesAsync();
            return Ok(new BasicCrimeResultDTO()
            {
                SuccesFailOrJail = result.ToString()
            });
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> RobJuwelryStoreAsync()
        {
            ApplicationUser CurrentUser = await _userManager.FindByNameAsync(HttpContext.User.Identity!.Name);
            Gangster? Gangster = await _db.Gangsters.FindAsync(CurrentUser.GangsterId);
            if (Gangster == null || Gangster.InPrisonUntill > DateTime.Now || Gangster.NextBasicCrimeAt > DateTime.Now)
                return BadRequest();
            SuccesFailOrJail result = IsSuccesFailOrJail(50, 25);
            if (result == SuccesFailOrJail.Succes)
            {
                int loot = GetRandomNumber(1000, 10000);
                Gangster.AmountOfMoneyInPocket += loot;
                Gangster.NextBasicCrimeAt = DateTime.Now.AddSeconds(GetRandomNumber(60, 69));
                await _db.SaveChangesAsync();
                return Ok(new BasicCrimeResultDTO()
                {
                    SuccesFailOrJail = result.ToString(),
                    Loot = loot
                });
            }
            else if (result == SuccesFailOrJail.Jail)
            {
                Gangster.InPrisonUntill = DateTime.Now.AddSeconds(GetRandomNumber(60, 120));
                //Gangster.PrisonId = 1;
            }
            Gangster.NextBasicCrimeAt = DateTime.Now.AddSeconds(GetRandomNumber(60, 69));
            await _db.SaveChangesAsync();
            return Ok(new BasicCrimeResultDTO()
            {
                SuccesFailOrJail = result.ToString()
            });
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> RobGrandmaAsync()
        {
            ApplicationUser CurrentUser = await _userManager.FindByNameAsync(HttpContext.User.Identity!.Name);
            Gangster? Gangster = await _db.Gangsters.FindAsync(CurrentUser.GangsterId);
            if (Gangster == null || Gangster.InPrisonUntill > DateTime.Now || Gangster.NextBasicCrimeAt > DateTime.Now)
                return BadRequest();
            SuccesFailOrJail result = IsSuccesFailOrJail(75, 15);
            if (result == SuccesFailOrJail.Succes)
            {
                int loot = GetRandomNumber(50, 500);
                Gangster.AmountOfMoneyInPocket += loot;
                Gangster.NextBasicCrimeAt = DateTime.Now.AddSeconds(GetRandomNumber(60, 69));
                await _db.SaveChangesAsync();
                return Ok(new BasicCrimeResultDTO()
                {
                    SuccesFailOrJail = result.ToString(),
                    Loot = loot
                });
            }
            if (result == SuccesFailOrJail.Jail)
            {
                Gangster.InPrisonUntill = DateTime.Now.AddSeconds(GetRandomNumber(60, 120));
                //Gangster.PrisonId = 1;
            }
            Gangster.NextBasicCrimeAt = DateTime.Now.AddSeconds(GetRandomNumber(60, 69));
            await _db.SaveChangesAsync();
            return Ok(new BasicCrimeResultDTO()
            {
                SuccesFailOrJail = result.ToString()
            });
        }
    }
}
