using Blazored.Modal;
using BlazorWebApp.Components.Pages.Comps;
using Microsoft.AspNetCore.Components.Authorization;
using SpostatoDAL.Models;
using BlazorWebApp.Services;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using System.IO;

namespace BlazorWebApp.Components.Pages
{
    public partial class BasicCrime
    {
        private Gangster currentGangster;

        [Inject] private AuthenticationStateProvider AuthenticationStateProvider { get; set; }
        [Inject] private IGangsterService GangsterService { get; set; }
        [Inject] private IBasicCrimeService BasicCrimeService { get; set; }
        [Inject] private IModalService Modal { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            if (user.Identity.IsAuthenticated)
            {
                currentGangster = await GangsterService.GetCurrentAliveGangsterAsync();
            }
        }

        private async Task CommitCrime(BasicCrimeType crimeType)
        {
            if (currentGangster.InPrisonUntill > DateTime.Now)
                return;

            var crimeResult = await BasicCrimeService.CommitCrime(currentGangster, crimeType);

            var parameters = new ModalParameters();
            parameters.Add(nameof(CrimeResultModal.Result), crimeResult);

            var modalTitle = GetModalTitle(crimeType);
            var modal = Modal.Show<CrimeResultModal>(modalTitle, parameters);
            await modal.Result;

            currentGangster = await GangsterService.GetCurrentAliveGangsterAsync();
            StateHasChanged();
        }

        private string GetModalTitle(BasicCrimeType crimeType) =>
            crimeType switch
            {
                BasicCrimeType.ShootingRange => "Schietbaan",
                BasicCrimeType.RobGrandma => "Beroving",
                BasicCrimeType.RobJuwelryStore => "Juwelier Overval",
                _ => throw new ArgumentOutOfRangeException(nameof(crimeType))
            };
    }
}
