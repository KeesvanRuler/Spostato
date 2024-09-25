using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace BlazorWebApp.Components.Pages.Auth
{
    public partial class Logout
    {
        private bool isLoggedOut = false;

        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            isLoggedOut = !authState.User.Identity.IsAuthenticated;
        }

        private async Task HandleLogout()
        {
            await SignInManager.SignOutAsync();
            isLoggedOut = true;
            NavigationManager.NavigateTo("/", forceLoad: true);
        }
    }
}