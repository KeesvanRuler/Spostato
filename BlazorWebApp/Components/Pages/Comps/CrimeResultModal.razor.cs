using Blazored.Modal;
using Microsoft.AspNetCore.Components;

namespace BlazorWebApp.Components.Pages.Comps
{
    public partial class CrimeResultModal
    {
        [CascadingParameter] BlazoredModalInstance BlazoredModal { get; set; } = default!;

        [Parameter] public string ResultMessage { get; set; } = "";
        [Parameter] public double ProgressionGained { get; set; }
        [Parameter] public int MoneyGained { get; set; }
        [Parameter] public double ShootingSkillGained { get; set; }
        [Parameter] public double DrivingSkillGained { get; set; }
        [Parameter] public double BreakoutSkillGained { get; set; }

        private async Task Close()
        {
            await BlazoredModal.CloseAsync();
            NavigationManager.NavigateTo("/overview");
        }
    }
}