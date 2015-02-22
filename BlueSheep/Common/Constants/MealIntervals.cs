namespace BlueSheep.Common.Constants
{
    class MealIntervals
    {
        #region Public methods
        public static int GetMealIntervals(string petName)
        {
            switch (petName)
            {
                case "Bworky":
                    return 5;
                case "Péki":
                    return 3;
                case "Chacha":
                    return 5;
                case "Fotome":
                    return 5;
                case "Vilain Petit Corbac":
                    return 5;
                case "Dragoune Rose":
                    return 5;
                case "Atouin":
                    return 5;
                case "Pioute bleu":
                    return 5;
                case "Pioute jaune":
                    return 5;
                case "Pioute rose":
                    return 5;
                case "Pioute rouge":
                    return 5;
                case "Pioute vert":
                    return 5;
                case "Pioute violet":
                    return 5;
                case "Koalak Sanguin":
                    return 5;
                case "Croum":
                    return 5;
                case "Bwak d'Air":
                    return 11;
                case "Bwak de Feu":
                    return 11;
                case "Bwak d'Eau":
                    return 11;
                case "Bwak de Terre":
                    return 11;
                default:
                    return 0;
            }
        }
        #endregion
    }
}
