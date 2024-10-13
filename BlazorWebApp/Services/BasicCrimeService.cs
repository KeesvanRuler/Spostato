using SpostatoBL.Enum;
using SpostatoBL.Helper;
using SpostatoBL.Models;
using SpostatoDAL.Models;

namespace BlazorWebApp.Services
{
    public interface IBasicCrimeService
    {
        Task<CrimeResult> CommitCrime(Gangster gangster, BasicCrimeType crimeType);
    }

    public class BasicCrimeService : IBasicCrimeService
    {
        private readonly IGangsterService _gangsterService;

        public BasicCrimeService(IGangsterService gangsterService)
        {
            _gangsterService = gangsterService;
        }

        public async Task<CrimeResult> CommitCrime(Gangster gangster, BasicCrimeType crimeType)
        {
            var crimeDetails = GetCrimeDetails(crimeType);
            var result = RNG.IsSuccesFailOrJail(
                RankHelper.GetSuccessPercentage(gangster.Rank, crimeDetails.BaseSuccessRate),
                RankHelper.GetFailPercentage(gangster.Rank, crimeDetails.BaseSuccessRate, crimeDetails.BaseJailRate)
            );

            var crimeResult = new CrimeResult();

            switch (result)
            {
                case RNG.SuccesFailOrJail.Succes:
                    HandleSuccessResult(gangster, crimeDetails, crimeResult);
                    break;
                case RNG.SuccesFailOrJail.Fail:
                    crimeResult.ResultMessage = crimeDetails.FailMessage;
                    break;
                case RNG.SuccesFailOrJail.Jail:
                    HandleJailResult(gangster, crimeDetails, crimeResult);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            await _gangsterService.UpdateGangsterAsync(gangster);

            return crimeResult;
        }

        private void HandleSuccessResult(Gangster gangster, CrimeDetails crimeDetails, CrimeResult crimeResult)
        {
            crimeResult.ProgressionGained = RankHelper.GetProgressionForCrime(gangster.Rank, crimeDetails.BaseProgression);
            crimeResult.MoneyGained = crimeDetails.BaseLoot + (int)(crimeDetails.BaseLoot * ((int)gangster.Rank * 0.1));
            crimeResult.ResultMessage = string.Format(crimeDetails.SuccessMessage, crimeResult.MoneyGained);

            gangster.RankProgression += crimeResult.ProgressionGained;
            gangster.AmountOfMoneyInPocket += crimeResult.MoneyGained;
            gangster.ShootingSkill = Math.Min(100, gangster.ShootingSkill + crimeDetails.ShootingSkillGain);
            gangster.DrivingSkill = Math.Min(100, gangster.DrivingSkill + crimeDetails.DrivingSkillGain);
            gangster.BreakoutSkill = Math.Min(100, gangster.BreakoutSkill + crimeDetails.BreakoutSkillGain);

            while (gangster.RankProgression >= 100 && gangster.Rank < RankEnum.Maffiabaas)
            {
                gangster.RankProgression -= 100;
                gangster.Rank++;
            }

            crimeResult.ShootingSkillGained = crimeDetails.ShootingSkillGain;
            crimeResult.DrivingSkillGained = crimeDetails.DrivingSkillGain;
            crimeResult.BreakoutSkillGained = crimeDetails.BreakoutSkillGain;
        }

        private void HandleJailResult(Gangster gangster, CrimeDetails crimeDetails, CrimeResult crimeResult)
        {
            crimeResult.ProgressionGained = -RankHelper.GetProgressionForCrime(gangster.Rank, crimeDetails.BaseProgression / 2);
            crimeResult.ResultMessage = crimeDetails.JailMessage;
            gangster.RankProgression = Math.Max(0, gangster.RankProgression + crimeResult.ProgressionGained);
            gangster.InPrisonUntill = DateTime.Now.AddMinutes(RankHelper.CalculateJailTime(gangster.Rank, crimeDetails.BaseJailTime));
            gangster.BreakoutAttemptsLeft = 5;
        }

        private CrimeDetails GetCrimeDetails(BasicCrimeType crimeType)
        {
            return crimeType switch
            {
                BasicCrimeType.ShootingRange => new CrimeDetails(70, 5, 20, 0.5, 0, "Je schoot in de roos, je schietervaring is verbeterd!", "Je schoot mis, volgende keer beter!", "Je schoot per ongeluk in de voet van de instructeur, de politie heeft je meegenomen.", 1.0, 0, 0),
                BasicCrimeType.RobGrandma => new CrimeDetails(60, 10, 30, 1.0, 50, "Alles verliep volgens plan, je hebt {0} euro uit de handtas gehaald!", "Het omaatje sloeg je volluit in je gezicht met haar tasje, je bent weggerend.", "Er reed precies op het moment van de overval een politie auto voorbij, je bent opgepakt door de politie!", 0, 0.5, 0.5),
                BasicCrimeType.RobJuwelryStore => new CrimeDetails(40, 20, 40, 2.0, 500, "De overval lukte, je hebt {0} euro buitgemaakt!", "De medewerker drukte op de alarm knop, je bent snel weggerend en weggekomen.", "De politie achtervolgde je na de overval en hebben je een paar straten verder kunnen arresteren.", 0.5, 1.0, 1.0),
                _ => throw new ArgumentOutOfRangeException(nameof(crimeType))
            };
        }
    }

    public record CrimeDetails(
        int BaseSuccessRate, int BaseJailRate, int BaseJailTime, double BaseProgression, int BaseLoot,
        string SuccessMessage, string FailMessage, string JailMessage,
        double ShootingSkillGain, double DrivingSkillGain, double BreakoutSkillGain);

    public enum BasicCrimeType
    {
        ShootingRange,
        RobGrandma,
        RobJuwelryStore
    }
}