using Blazored.Modal;
using Microsoft.AspNetCore.Components;
using SpostatoBL.Models;

namespace BlazorWebApp.Components.Pages.Comps
{
    public partial class CrimeResultModal
    {
        [CascadingParameter] BlazoredModalInstance BlazoredModal { get; set; } = default!;
        [Parameter] public CrimeResult Result { get; set; } = new CrimeResult();
        [Inject] private NavigationManager NavigationManager { get; set; }

        private async Task Close()
        {
            await BlazoredModal.CloseAsync();
            NavigationManager.NavigateTo("/overview");
        }
    }
}