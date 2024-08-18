using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using CrimeGameBlazor_Client.Services.IServices;

namespace SpostatoClient.Pages.Auth
{
    public partial class RedirectToLogin : ComponentBase
    {
        [CascadingParameter]
        public Task<AuthenticationState> _authState { get; set; }
        [Inject]
        public NavigationManager _navigationManager { get; set; }

        public bool NotAuthorized { get; set; } = false;

        protected override async Task OnInitializedAsync()
        {
            var authState = await _authState;

            if (authState?.User?.Identity is null || !authState.User.Identity.IsAuthenticated)
            {
                var returnUrl = _navigationManager.ToBaseRelativePath(_navigationManager.Uri);
                if (string.IsNullOrEmpty(returnUrl))
                {
                    _navigationManager.NavigateTo("login");
                }
                else
                {
                    _navigationManager.NavigateTo($"login?returnUrl={returnUrl}");
                }
            }
            else
            {
                NotAuthorized = true;
            }

        }
    }
}