using Microsoft.AspNetCore.Components;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;

namespace BlazorWebApp.Components.Pages.Auth
{
    public partial class ForgotPassword
    {
        private InputModel input = new InputModel();

        private class InputModel
        {
            [Required(ErrorMessage = "E-mailadres is verplicht")]
            [EmailAddress(ErrorMessage = "Ongeldig e-mailadres")]
            [Display(Name = "E-mailadres")]
            public string Email { get; set; }
        }

        private async Task HandleForgotPassword()
        {
            var user = await UserManager.FindByEmailAsync(input.Email);
            if (user != null && await UserManager.IsEmailConfirmedAsync(user))
            {
                var code = await UserManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = NavigationManager.ToAbsoluteUri($"/resetpassword?code={code}").ToString();

                await EmailService.SendEmailAsync(
                    input.Email,
                    "Reset uw wachtwoord",
                    $"Reset uw wachtwoord door <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>hier te klikken</a>.");
            }

            // Always redirect to confirmation page, even if user doesn't exist (for security)
            NavigationManager.NavigateTo("/forgotpasswordconfirmation");
        }
    }
}