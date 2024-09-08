using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SpostatoDAL.Models;
using SpostatoBL.Service;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Pages.Auth
{
    public class RegisterModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailService _emailService;

        public RegisterModel(UserManager<ApplicationUser> userManager, IEmailService emailService)
        {
            _userManager = userManager;
            _emailService = emailService;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
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

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = Input.Email, Email = Input.Email, Name = Input.Name };
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var confirmationLink = Url.Page(
                        "/ConfirmEmail",
                        pageHandler: null,
                        values: new { userId = user.Id, token },
                        protocol: Request.Scheme);

                    var emailBody = $@"
                        <h2>Hallo {Input.Name},</h2>
                        <p>Bedankt voor uw registratie. Klik op de onderstaande link om uw account te bevestigen:</p>
                        <p><a href='{confirmationLink}'>Bevestig mijn account</a></p>
                        <p>Als de link niet werkt, kopieer dan de volgende URL en plak deze in uw browser:</p>
                        <p>{confirmationLink}</p>
                        <p>Bedankt,<br>Het Spostato Team</p>";

                    await _emailService.SendEmailAsync(Input.Email, "Bevestig uw e-mailadres", emailBody);

                    return RedirectToPage("/RegisterConfirmation");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return Page();
        }
    }
}