using SpostatoBL.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpostatoBL.Helper
{
    public static class RankHelper
    {
        public static readonly Dictionary<RankEnum, int> RankBullets = new()
        {
            { RankEnum.Straatschoffie, 1000 },
            { RankEnum.Zakkenroller, 2000 },
            { RankEnum.Kruimeldief, 3500 },
            { RankEnum.Inbreker, 5000 },
            { RankEnum.Heler, 7500 },
            { RankEnum.Oplichter, 10000 },
            { RankEnum.Afperser, 15000 },
            { RankEnum.Smokkelaar, 20000 },
            { RankEnum.Witwasser, 30000 },
            { RankEnum.Bodyguard, 40000 },
            { RankEnum.Kluiskraker, 55000 },
            { RankEnum.Huurmoordenaar, 70000 },
            { RankEnum.Drugsdealer, 85000 },
            { RankEnum.Bendelidmaat, 100000 },
            { RankEnum.Bendeleider, 120000 },
            { RankEnum.Peetvader, 135000 },
            { RankEnum.Maffiabaas, 150000 }
        };

        public static int GetSuccessPercentage(RankEnum rank, int baseSuccessRate)
        {
            return Math.Min(95, baseSuccessRate + (int)rank * 3); // Cap at 95% success rate
        }

        public static int GetJailPercentage(RankEnum rank, int baseJailRate)
        {
            return Math.Max(1, baseJailRate - (int)rank); // Minimum 1% jail chance
        }

        public static int GetFailPercentage(RankEnum rank, int baseSuccessRate, int baseJailRate)
        {
            int successRate = GetSuccessPercentage(rank, baseSuccessRate);
            int jailRate = GetJailPercentage(rank, baseJailRate);
            return 100 - successRate - jailRate;
        }

        public static double GetProgressionForCrime(RankEnum rank, double baseProgression)
        {
            double diminishingFactor = Math.Max(0.1, 1 - ((int)rank * 0.05)); // Diminishing returns
            return Math.Round(baseProgression * diminishingFactor, 2);
        }

        public static (RankEnum newRank, double newProgression) UpdateRankAndProgression(RankEnum currentRank, double currentProgression, double progressionGain)
        {
            double newProgression = currentProgression + progressionGain;
            RankEnum newRank = currentRank;

            while (newProgression >= 100 && newRank < RankEnum.Maffiabaas)
            {
                newProgression -= 100;
                newRank = (RankEnum)((int)newRank + 1);
            }

            if (newRank == RankEnum.Maffiabaas)
            {
                newProgression = Math.Min(newProgression, 100);
            }

            return (newRank, Math.Round(newProgression, 2));
        }

        public static int CalculateJailTime(RankEnum rank, int baseJailTime)
        {
            return (int)(baseJailTime * (1 + (int)rank * 0.1));
        }
    }
}
