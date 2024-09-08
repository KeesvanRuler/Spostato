namespace SpostatoBL.Helper
{
    public class RNG
    {
        public enum SuccesFailOrJail
        {
            Succes,
            Fail,
            Jail
        }
        public static SuccesFailOrJail IsSuccesFailOrJail(int SuccesPercentage, int FailPercentage) 
        {
            Random rnd = new();
            int num = rnd.Next(1, 101);
            if (num <= SuccesPercentage)
                return SuccesFailOrJail.Succes;
            else if (num <= SuccesPercentage + FailPercentage)
                return SuccesFailOrJail.Fail;
            return SuccesFailOrJail.Jail;
        }

        public static bool IsSuccesful(int SuccesPercentage)
        {
            Random rnd = new();
            int num = rnd.Next(1, 101);
            return num <= SuccesPercentage;
        }

        public static int GetRandomNumber(int MinValue, int MaxValue)
        {
            Random random = new();
            return random.Next(MinValue, MaxValue + 1);
        }
    }
}
