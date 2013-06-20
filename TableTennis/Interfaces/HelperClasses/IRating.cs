namespace TableTennis.Interfaces.HelperClasses
{
    public interface IRating
    {
        double CalculateRating(int player1, int player2, bool playerOneWin);
    }
}