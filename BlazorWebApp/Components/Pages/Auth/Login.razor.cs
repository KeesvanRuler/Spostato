using Microsoft.AspNetCore.Components;
using System.ComponentModel.DataAnnotations;

namespace BlazorWebApp.Components.Pages.Auth
{
    public partial class Login
    {
        private LoginModel loginModel = new LoginModel();
        private string errorMessage;

        private class LoginModel
        {
            [Required(ErrorMessage = "E-mailadres is verplicht")]
            [EmailAddress(ErrorMessage = "Ongeldig e-mailadres")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Wachtwoord is verplicht")]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }

        private async Task HandleLogin()
        {
            errorMessage = null;
            var user = await UserManager.FindByNameAsync(loginModel.Email);
            if (user != null)
            {
                var result = await SignInManager.PasswordSignInAsync(user, loginModel.Password, isPersistent: false, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    NavigationManager.NavigateTo("/");
                    return;
                }
            }

            errorMessage = "Ongeldige inloggegevens";
        }
    }
}