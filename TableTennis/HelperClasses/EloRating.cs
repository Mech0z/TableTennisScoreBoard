using TableTennis.Interfaces.HelperClasses;

namespace TableTennis.HelperClasses
{
    public class EloRating : IRating
    {
        public double CalculateRating(int player1, int player2, bool playerOneWin)
        {
            const double medium = 20;
            const double diversification = 20;
            const double minRating = medium - diversification;
            const double maxRating = medium + diversification;

            double diff;

            if (playerOneWin)
            {
                diff = player1 - player2;
            }
            else
            {
                diff = player2 - player1;
            }

            var result = (medium * diversification - diff) / medium + minRating;

            if (result > maxRating)
                result = maxRating;

            else if (result < minRating)
                result = minRating;

            return result;
        }


        //public EloRating(double CurrentRating1, double CurrentRating2, double Score1, double Score2)
        //{
        //    double E = 25;
        //    double K = 400;

        //    if (Score1 != Score2)
        //    {
        //        if (Score1 > Score2)
        //        {
        //            E = 120 - Math.Round(1 / (1 + Math.Pow(10, ((CurrentRating2 - CurrentRating1) / K))) * 120);
        //            FinalResult1 = CurrentRating1 + E;
        //            FinalResult2 = CurrentRating2 - E;
        //        }
        //        else
        //        {
        //            E = 120 - Math.Round(1 / (1 + Math.Pow(10, ((CurrentRating1 - CurrentRating2) / K))) * 120);
        //            FinalResult1 = CurrentRating1 - E;
        //            FinalResult2 = CurrentRating2 + E;
        //        }
        //    }
        //    else
        //    {
        //        if (CurrentRating1 == CurrentRating2)
        //        {
        //            FinalResult1 = CurrentRating1;
        //            FinalResult2 = CurrentRating2;
        //        }
        //        else
        //        {
        //            if (CurrentRating1 > CurrentRating2)
        //            {
        //                E = (120 - Math.Round(1 / (1 + Math.Pow(10, ((CurrentRating1 - CurrentRating2) / K))) * 120)) - (120 - Math.Round(1 / (1 + Math.Pow(10, ((CurrentRating2 - CurrentRating1) / K))) * 120));
        //                FinalResult1 = CurrentRating1 - E;
        //                FinalResult2 = CurrentRating2 + E;
        //            }
        //            else
        //            {
        //                E = (120 - Math.Round(1 / (1 + Math.Pow(10, ((CurrentRating2 - CurrentRating1) / K))) * 120)) - (120 - Math.Round(1 / (1 + Math.Pow(10, ((CurrentRating1 - CurrentRating2) / K))) * 120));
        //                FinalResult1 = CurrentRating1 + E;
        //                FinalResult2 = CurrentRating2 - E;
        //            }
        //        }
        //    }
        //    Point1 = FinalResult1 - CurrentRating1;
        //    Point2 = FinalResult2 - CurrentRating2;

        //}
    }
}