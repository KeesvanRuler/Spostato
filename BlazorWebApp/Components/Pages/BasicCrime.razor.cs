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
            var (resultMessage, progressionGained, moneyGained, shootingSkillGain, drivingSkillGain, breakoutSkillGain) =
                await BasicCrimeService.CommitCrime(currentGangster, crimeType);

            var parameters = new ModalParameters();
            parameters.Add(nameof(CrimeResultModal.ResultMessage), resultMessage);
            parameters.Add(nameof(CrimeResultModal.ProgressionGained), progressionGained);
            parameters.Add(nameof(CrimeResultModal.MoneyGained), moneyGained);
            parameters.Add(nameof(CrimeResultModal.ShootingSkillGained), shootingSkillGain);
            parameters.Add(nameof(CrimeResultModal.DrivingSkillGained), drivingSkillGain);
            parameters.Add(nameof(CrimeResultModal.BreakoutSkillGained), breakoutSkillGain);

            var modalTitle = crimeType switch
            {
                BasicCrimeType.ShootingRange => "Schietbaan",
                BasicCrimeType.RobGrandma => "Beroving",
                BasicCrimeType.RobJuwelryStore => "Overval",
                _ => throw new ArgumentOutOfRangeException(nameof(crimeType))
            };

            var modal = Modal.Show<CrimeResultModal>(modalTitle, parameters);
            await modal.Result;

            // Refresh the current gangster data after the crime
            currentGangster = await GangsterService.GetCurrentAliveGangsterAsync();
            StateHasChanged();
        }
    }
}