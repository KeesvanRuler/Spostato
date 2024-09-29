using BlazorWebApp.Services;
using Microsoft.AspNetCore.Components;
using SpostatoBL.Enum;
using SpostatoDAL.Models;

namespace BlazorWebApp.Components.Pages
{
    public partial class GangsterOverview : ComponentBase
    {
        private Gangster gangster = new Gangster(); // In a real application, you'd get this from your database or state management

        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            if (user.Identity.IsAuthenticated)
            {
                gangster = await GangsterService.GetCurrentAliveGangsterAsync();
            }
        }

        private async Task BuyOutOfPrison()
        {
            if (gangster.Prison != null && gangster.AmountOfMoneyInPocket >= gangster.BuyOutAmount)
            {
                gangster.AmountOfMoneyInPocket -= gangster.BuyOutAmount.Value;
                gangster.Prison = null;
                gangster.InPrisonUntill = DateTime.Now;
                gangster.BuyOutAmount = null;
                await GangsterService.UpdateGangsterAsync(gangster);
            }
        }
    }
}