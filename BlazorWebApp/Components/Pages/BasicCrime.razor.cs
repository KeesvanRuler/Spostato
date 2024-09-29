using Blazored.Modal;
using BlazorWebApp.Components.Pages.Comps;
using Microsoft.AspNetCore.Components.Authorization;
using SpostatoBL.Enum;
using SpostatoBL.Helper;
using SpostatoDAL.Models;

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

        private enum CrimeType
        {
            ShootingRange,
            RobGrandma,
            RobJuwelryStore
        }

        private async Task CommitCrime(CrimeType crimeType)
        {
            int baseSuccessRate, baseJailRate, baseJailTime;
            double baseProgression;
            int baseLoot;
            string modalTitle, successMessage, failMessage, jailMessage;
            double shootingSkillGain = 0, drivingSkillGain = 0, breakoutSkillGain = 0;

            switch (crimeType)
            {
                case CrimeType.ShootingRange:
                    baseSuccessRate = 70;
                    baseJailRate = 5;
                    baseJailTime = 20;
                    baseProgression = 0.5;
                    baseLoot = 0;
                    modalTitle = "Schietbaan";
                    successMessage = "Je schoot in de roos, je schietervaring is verbeterd!";
                    failMessage = "Je schoot mis, volgende keer beter!";
                    jailMessage = "Je schoot per ongeluk in de voet van de instructeur, de politie heeft je meegenomen.";
                    shootingSkillGain = 1.0;
                    break;
                case CrimeType.RobGrandma:
                    baseSuccessRate = 60;
                    baseJailRate = 10;
                    baseJailTime = 30;
                    baseProgression = 1.0;
                    baseLoot = 50;
                    modalTitle = "Beroving";
                    successMessage = "Alles verliep volgens plan, je hebt {0} euro uit de handtas gehaald!";
                    failMessage = "Het omaatje sloeg je volluit in je gezicht met haar tasje, je bent weggerend.";
                    jailMessage = "Er reed precies op het moment van de overval een politie auto voorbij, je bent opgepakt door de politie!";
                    drivingSkillGain = 0.5;
                    breakoutSkillGain = 0.5;
                    break;
                case CrimeType.RobJuwelryStore:
                    baseSuccessRate = 40;
                    baseJailRate = 20;
                    baseJailTime = 40;
                    baseProgression = 2.0;
                    baseLoot = 500;
                    modalTitle = "Overval";
                    successMessage = "De overval lukte, je hebt {0} euro buitgemaakt!";
                    failMessage = "De medewerker drukte op de alarm knop, je bent snel weggerend en weggekomen.";
                    jailMessage = "De politie achtervolgde je na de overval en hebben je een paar straten verder kunnen arresteren.";
                    shootingSkillGain = 0.5;
                    drivingSkillGain = 1.0;
                    breakoutSkillGain = 1.0;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(crimeType));
            }

            var result = RNG.IsSuccesFailOrJail(
                RankHelper.GetSuccessPercentage(currentGangster.Rank, baseSuccessRate),
                RankHelper.GetFailPercentage(currentGangster.Rank, baseSuccessRate, baseJailRate)
            );

            double progressionGained = 0;
            int moneyGained = 0;
            string resultMessage;

            switch (result)
            {
                case RNG.SuccesFailOrJail.Succes:
                    progressionGained = RankHelper.GetProgressionForCrime(currentGangster.Rank, baseProgression);
                    moneyGained = baseLoot + (int)(baseLoot * ((int)currentGangster.Rank * 0.1)); // Loot increases with rank
                    resultMessage = string.Format(successMessage, moneyGained);

                    currentGangster.RankProgression += progressionGained;
                    currentGangster.AmountOfMoneyInPocket += moneyGained;
                    currentGangster.ShootingSkill = Math.Min(100, currentGangster.ShootingSkill + shootingSkillGain);
                    currentGangster.DrivingSkill = Math.Min(100, currentGangster.DrivingSkill + drivingSkillGain);
                    currentGangster.BreakoutSkill = Math.Min(100, currentGangster.BreakoutSkill + breakoutSkillGain);

                    // Check for rank up
                    while (currentGangster.RankProgression >= 100 && currentGangster.Rank < RankEnum.Maffiabaas)
                    {
                        currentGangster.RankProgression -= 100;
                        currentGangster.Rank++;
                    }
                    break;
                case RNG.SuccesFailOrJail.Fail:
                    resultMessage = failMessage;
                    break;
                case RNG.SuccesFailOrJail.Jail:
                    progressionGained = -RankHelper.GetProgressionForCrime(currentGangster.Rank, baseProgression / 2); // Lose half progression when jailed
                    resultMessage = jailMessage;
                    currentGangster.RankProgression = Math.Max(0, currentGangster.RankProgression + progressionGained);
                    currentGangster.InPrisonUntill = DateTime.Now.AddMinutes(RankHelper.CalculateJailTime(currentGangster.Rank, baseJailTime));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            // Save changes to the database
            await GangsterService.UpdateGangsterAsync(currentGangster);

            var parameters = new ModalParameters();
            parameters.Add(nameof(CrimeResultModal.ResultMessage), resultMessage);
            parameters.Add(nameof(CrimeResultModal.ProgressionGained), progressionGained);
            parameters.Add(nameof(CrimeResultModal.MoneyGained), moneyGained);
            parameters.Add(nameof(CrimeResultModal.ShootingSkillGained), shootingSkillGain);
            parameters.Add(nameof(CrimeResultModal.DrivingSkillGained), drivingSkillGain);
            parameters.Add(nameof(CrimeResultModal.BreakoutSkillGained), breakoutSkillGain);

            var modal = Modal.Show<CrimeResultModal>(modalTitle, parameters);
            await modal.Result;
        }
    }
}