namespace TableTennis.Models
{
    public class EloRating
    {
        public double Point1 { get; set; }
        public double Point2 { get; set; }

        public double FinalResult1 { get; set; }
        public double FinalResult2 { get; set; }

        public EloRating(double CurrentRating1, double CurrentRating2, double Score1, double Score2)
        {
            const double medium = 20;
            const double diversification = 10;
            const double minRating = medium - diversification;
            const double maxRating = medium + diversification;

            double diff = 0;

            if (Score1 > Score2)
            {
                diff = CurrentRating1 - CurrentRating2;
            }

            if (Score1 < Score2)
            {
                diff = CurrentRating2 - CurrentRating1;
            }

            var result = (medium * diversification - diff) / medium + minRating;

            if (result > maxRating)
                result = maxRating;

            else if (result < minRating)
                result = minRating;

            if (Score1 > Score2)
            {
                FinalResult1 = CurrentRating1 + result;
                FinalResult2 = CurrentRating2 - result;
            }

            else
            {
                FinalResult1 = CurrentRating1 - result;
                FinalResult2 = CurrentRating2 + result;
            }

            Point1 = FinalResult1 - CurrentRating1;
            Point2 = FinalResult2 - CurrentRating2;
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