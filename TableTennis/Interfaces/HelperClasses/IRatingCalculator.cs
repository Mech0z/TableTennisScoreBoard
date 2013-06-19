using TableTennis.HelperClasses;

namespace TableTennis.Interfaces.HelperClasses
{
    public interface IRatingCalculator
    {
        void RecalculateSingleRatings(Game game);
        void RecalculateDoubleRatings(Game game);
    }
}