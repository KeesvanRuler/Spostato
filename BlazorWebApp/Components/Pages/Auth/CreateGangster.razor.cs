using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using SpostatoBL.Enum;
using SpostatoDAL.Models;
using System.ComponentModel.DataAnnotations;

namespace BlazorWebApp.Components.Pages.Auth
{
    public partial class CreateGangster
    {
        private GangsterModel model = new GangsterModel();
        private string errorMessage;
        private ApplicationUser currentUser;
        private List<City> cities;

        protected override async Task OnInitializedAsync()
        {
            var httpContext = HttpContextAccessor.HttpContext;
            if (httpContext != null)
            {
                var user = await UserManager.GetUserAsync(httpContext.User);
                if (user != null)
                {
                    currentUser = await DbContext.Users
                        .Include(u => u.CurrentGangster)
                        .FirstOrDefaultAsync(u => u.Id == user.Id);
                }
            }

            cities = await DbContext.Cities.ToListAsync();
        }

        private class GangsterModel
        {
            [Required(ErrorMessage = "Naam is verplicht")]
            [StringLength(50, ErrorMessage = "Naam moet tussen 3 en 50 tekens lang zijn", MinimumLength = 3)]
            public string Name { get; set; }

            [Required(ErrorMessage = "Startstad is verplicht")]
            public int CityId { get; set; }
        }

        private async Task CreateNewGangster()
        {
            if (currentUser == null || (currentUser.CurrentGangster != null && currentUser.CurrentGangster.IsAlive))
            {
                errorMessage = "Je kunt geen nieuwe gangster aanmaken op dit moment.";
                return;
            }

            var city = await DbContext.Cities.FindAsync(model.CityId);
            if (city == null)
            {
                errorMessage = "Ongeldige stad geselecteerd.";
                return;
            }

            var gangster = new Gangster
            {
                User = currentUser,
                Name = model.Name,
                CurrentCity = city,
                IsAlive = true,
                CreatedAt = DateTime.Now,
                AmountOfMoneyInPocket = 1000, // Starting money
                HP = 100,
                Rank = RankEnum.Straatschoffie
            };

            DbContext.Gangsters.Add(gangster);
            currentUser.CurrentGangster = gangster;
            currentUser.Gangsters.Add(gangster);

            await DbContext.SaveChangesAsync();

            NavigationManager.NavigateTo("/");
        }
    }
}