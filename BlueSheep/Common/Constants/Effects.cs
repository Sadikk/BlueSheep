using System.Collections.Generic;

namespace BlueSheep.Common.Constants
{
    static class Effects
    {
        #region Public methods
        public static List<int> GetEffects(string petName)
        {
            switch (petName)
            {
                case "Bworky":
                    return new List<int> { 158 };
                case "Péki":
                    return new List<int> { 125 };
                case "Chacha":
                    return new List<int> { 126, 119, 118, 123, 125, 214 };
                case "Fotome":
                    return new List<int> { 125 };
                case "Vilain Petit Corbac":
                    return new List<int> { 176 };
                case "Dragoune Rose":
                    return new List<int> { 124 };
                case "Atouin":
                    return new List<int> { 178, 112 };
                case "Pioute bleu":
                    return new List<int> { 123 };
                case "Pioute jaune":
                    return new List<int> { 119 };
                case "Pioute rose":
                    return new List<int> { 178 };
                case "Pioute rouge":
                    return new List<int> { 126 };
                case "Pioute vert":
                    return new List<int> { 118 };
                case "Pioute violet":
                    return new List<int> { 112 };
                case "Koalak Sanguin":
                    return new List<int> { 124 };
                case "Croum":
                    return new List<int> {212, 211, 210, 213, 214};
                default:
                    return null;
            }
        }
        #endregion
    }
}
