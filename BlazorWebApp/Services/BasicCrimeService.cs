using SpostatoBL.Enum;
using SpostatoBL.Helper;
using SpostatoDAL.Models;

namespace BlazorWebApp.Services
{
    public interface IBasicCrimeService
    {
        Task<(string resultMessage, double progressionGained, int moneyGained, double shootingSkillGain, double drivingSkillGain, double breakoutSkillGain)> CommitCrime(Gangster gangster, BasicCrimeType crimeType);
    }

    public class BasicCrimeService : IBasicCrimeService
    {
        private readonly IGangsterService _gangsterService;

        public BasicCrimeService(IGangsterService gangsterService)
        {
            _gangsterService = gangsterService;
        }

        public async Task<(string resultMessage, double progressionGained, int moneyGained, double shootingSkillGain, double drivingSkillGain, double breakoutSkillGain)> CommitCrime(Gangster gangster, BasicCrimeType crimeType)
        {
            var (baseSuccessRate, baseJailRate, baseJailTime, baseProgression, baseLoot, successMessage, failMessage, jailMessage, shootingSkillGain, drivingSkillGain, breakoutSkillGain) = GetCrimeDetails(crimeType);

            var result = RNG.IsSuccesFailOrJail(
                RankHelper.GetSuccessPercentage(gangster.Rank, baseSuccessRate),
                RankHelper.GetFailPercentage(gangster.Rank, baseSuccessRate, baseJailRate)
            );

            double progressionGained = 0;
            int moneyGained = 0;
            string resultMessage;

            switch (result)
            {
                case RNG.SuccesFailOrJail.Succes:
                    progressionGained = RankHelper.GetProgressionForCrime(gangster.Rank, baseProgression);
                    moneyGained = baseLoot + (int)(baseLoot * ((int)gangster.Rank * 0.1));
                    resultMessage = string.Format(successMessage, moneyGained);

                    gangster.RankProgression += progressionGained;
                    gangster.AmountOfMoneyInPocket += moneyGained;
                    gangster.ShootingSkill = Math.Min(100, gangster.ShootingSkill + shootingSkillGain);
                    gangster.DrivingSkill = Math.Min(100, gangster.DrivingSkill + drivingSkillGain);
                    gangster.BreakoutSkill = Math.Min(100, gangster.BreakoutSkill + breakoutSkillGain);

                    while (gangster.RankProgression >= 100 && gangster.Rank < RankEnum.Maffiabaas)
                    {
                        gangster.RankProgression -= 100;
                        gangster.Rank++;
                    }
                    break;
                case RNG.SuccesFailOrJail.Fail:
                    resultMessage = failMessage;
                    break;
                case RNG.SuccesFailOrJail.Jail:
                    progressionGained = -RankHelper.GetProgressionForCrime(gangster.Rank, baseProgression / 2);
                    resultMessage = jailMessage;
                    gangster.RankProgression = Math.Max(0, gangster.RankProgression + progressionGained);
                    gangster.InPrisonUntill = DateTime.Now.AddMinutes(RankHelper.CalculateJailTime(gangster.Rank, baseJailTime));
                    gangster.BreakoutAttemptsLeft = 5;                   
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            await _gangsterService.UpdateGangsterAsync(gangster);

            return (resultMessage, progressionGained, moneyGained, shootingSkillGain, drivingSkillGain, breakoutSkillGain);
        }

        private (int baseSuccessRate, int baseJailRate, int baseJailTime, double baseProgression, int baseLoot, string successMessage, string failMessage, string jailMessage, double shootingSkillGain, double drivingSkillGain, double breakoutSkillGain) GetCrimeDetails(BasicCrimeType crimeType)
        {
            return crimeType switch
            {
                BasicCrimeType.ShootingRange => (70, 5, 20, 0.5, 0, "Je schoot in de roos, je schietervaring is verbeterd!", "Je schoot mis, volgende keer beter!", "Je schoot per ongeluk in de voet van de instructeur, de politie heeft je meegenomen.", 1.0, 0, 0),
                BasicCrimeType.RobGrandma => (60, 10, 30, 1.0, 50, "Alles verliep volgens plan, je hebt {0} euro uit de handtas gehaald!", "Het omaatje sloeg je volluit in je gezicht met haar tasje, je bent weggerend.", "Er reed precies op het moment van de overval een politie auto voorbij, je bent opgepakt door de politie!", 0, 0.5, 0.5),
                BasicCrimeType.RobJuwelryStore => (40, 20, 40, 2.0, 500, "De overval lukte, je hebt {0} euro buitgemaakt!", "De medewerker drukte op de alarm knop, je bent snel weggerend en weggekomen.", "De politie achtervolgde je na de overval en hebben je een paar straten verder kunnen arresteren.", 0.5, 1.0, 1.0),
                _ => throw new ArgumentOutOfRangeException(nameof(crimeType))
            };
        }
    }

    public enum BasicCrimeType
    {
        ShootingRange,
        RobGrandma,
        RobJuwelryStore
    }
}