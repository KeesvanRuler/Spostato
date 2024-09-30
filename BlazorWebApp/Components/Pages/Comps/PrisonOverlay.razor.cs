using BlazorWebApp.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SpostatoBL.Helper;
using SpostatoDAL.Models;
using System.Timers;

namespace BlazorWebApp.Components.Pages.Comps
{
    public partial class PrisonOverlay
    {
        [Inject] private IGangsterService GangsterService { get; set; }
        [Parameter] public Gangster Gangster { get; set; }

        private System.Timers.Timer refreshTimer;
        private Gangster currentGangster;
        private string message;
        private bool IsInPrison => currentGangster?.InPrisonUntill > DateTime.Now;

        protected override void OnInitialized()
        {
            currentGangster = Gangster;
            refreshTimer = new System.Timers.Timer(1000);
            refreshTimer.Elapsed += (sender, e) => InvokeAsync(CheckPrisonStatus);
            refreshTimer.Start();
        }

        protected override void OnParametersSet()
        {
            currentGangster = Gangster;
        }

        private async Task CheckPrisonStatus()
        {
            var wasInPrison = IsInPrison;
            currentGangster = await GangsterService.GetCurrentAliveGangsterAsync();

            if (wasInPrison != IsInPrison)
            {
                await InvokeAsync(StateHasChanged);
            }
        }

        private async Task BuyOut()
        {
            if (currentGangster.BuyOutAmount.HasValue &&
                currentGangster.AmountOfMoneyInPocket >= currentGangster.BuyOutAmount.Value)
            {
                currentGangster.AmountOfMoneyInPocket -= currentGangster.BuyOutAmount.Value;
                currentGangster.InPrisonUntill = DateTime.Now;
                await GangsterService.UpdateGangsterAsync(currentGangster);
                SetMessage("Je hebt jezelf vrijgekocht!");
            }
            else
            {
                SetMessage("Je hebt niet genoeg geld om jezelf vrij te kopen!");
            }
        }

        private async Task AttemptBreakout()
        {
            if (currentGangster.BreakoutAttemptsLeft <= 0)
            {
                SetMessage("Je hebt geen uitbraakpogingen meer over!");
                return;
            }

            currentGangster.BreakoutAttemptsLeft--;

            // Calculate success percentage (10% minimum, 100% maximum)
            int successPercentage = Math.Min(100, Math.Max(10, (int)(currentGangster.BreakoutSkill)));

            bool isSuccessful = RNG.IsSuccesful(successPercentage);

            if (isSuccessful)
            {
                currentGangster.InPrisonUntill = DateTime.Now;
                currentGangster.BreakoutSkill += 1;
                await GangsterService.UpdateGangsterAsync(currentGangster);
                SetMessage("Je bent succesvol uitgebroken!");
            }
            else
            {
                currentGangster.BuyOutAmount = (int)(currentGangster.BuyOutAmount! * 1.5);
                // 10% chance of extended sentence on failure
                if (RNG.IsSuccesful(10))
                {
                    currentGangster.InPrisonUntill = currentGangster.InPrisonUntill.AddHours(2);
                    await GangsterService.UpdateGangsterAsync(currentGangster);
                    SetMessage("Je uitbraakpoging is mislukt en je straf is verlengd met 2 uur!");
                }
                else
                {
                    SetMessage($"Je uitbraakpoging is mislukt. Je hebt nog {currentGangster.BreakoutAttemptsLeft} poging(en) over.");
                }
            }

            await GangsterService.UpdateGangsterAsync(currentGangster);
        }

        private void SetMessage(string newMessage)
        {
            message = newMessage;
            StateHasChanged();
        }

        public void Dispose()
        {
            refreshTimer?.Dispose();
        }
    }
}