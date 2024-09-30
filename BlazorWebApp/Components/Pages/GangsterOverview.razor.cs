using BlazorWebApp.Services;
using Microsoft.AspNetCore.Components;
using SpostatoBL.Enum;
using SpostatoDAL.Models;

namespace BlazorWebApp.Components.Pages
{
    public partial class GangsterOverview : ComponentBase
    {
        private Gangster? gangster { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            if (user.Identity.IsAuthenticated)
            {
                gangster = await GangsterService.GetCurrentAliveGangsterAsync();
            }
        }
    }
}