using Microsoft.AspNetCore.Components;
using SpostatoDAL.Models;
using System.ComponentModel.DataAnnotations;

namespace BlazorWebApp.Components.Pages.Auth
{
    public partial class Register
    {
        private InputModel input = new InputModel();
        private string errorMessage;

        private class InputModel
        {
            [Required(ErrorMessage = "Naam is verplicht")]
            [StringLength(100, ErrorMessage = "De {0} moet tussen {2} en {1} tekens lang zijn.", MinimumLength = 2)]
            [Display(Name = "Naam")]
            public string Name { get; set; }

            [Required(ErrorMessage = "E-mailadres is verplicht")]
            [EmailAddress(ErrorMessage = "Ongeldig e-mailadres")]
            [Display(Name = "E-mailadres")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Wachtwoord is verplicht")]
            [StringLength(100, ErrorMessage = "Het {0} moet minstens {2} en maximaal {1} tekens lang zijn.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Wachtwoord")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Bevestig wachtwoord")]
            [Compare("Password", ErrorMessage = "Het wachtwoord en het bevestigingswachtwoord komen niet overeen.")]
            public string ConfirmPassword { get; set; }
        }

        private async Task HandleRegistration()
        {
            errorMessage = null;
            var user = new ApplicationUser { UserName = input.Email, Email = input.Email, Name = input.Name };
            var result = await UserManager.CreateAsync(user, input.Password);

            if (result.Succeeded)
            {
                var token = await UserManager.GenerateEmailConfirmationTokenAsync(user);
                var confirmationLink = NavigationManager.ToAbsoluteUri($"/confirmemail?userId={user.Id}&token={token}").ToString();

                var emailBody = $@"
                <h2>Hallo {input.Name},</h2>
                <p>Bedankt voor uw registratie. Klik op de onderstaande link om uw account te bevestigen:</p>
                <p><a href='{confirmationLink}'>Bevestig mijn account</a></p>
                <p>Als de link niet werkt, kopieer dan de volgende URL en plak deze in uw browser:</p>
                <p>{confirmationLink}</p>
                <p>Bedankt,<br>Het Spostato Team</p>";

                await EmailService.SendEmailAsync(input.Email, "Bevestig uw e-mailadres", emailBody);

                NavigationManager.NavigateTo("/registerconfirmation");
            }
            else
            {
                errorMessage = string.Join(", ", result.Errors.Select(e => e.Description));
            }
        }
    }
}