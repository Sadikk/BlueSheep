namespace BlueSheep.Common.Types
{
    public class Pods
    {
        #region Properties
        public int Weight { get; set; }

        public int WeightMax { get; set; }

        #endregion

        #region Constructeurs
        public Pods()
        {
        }

        public Pods(int weight, int weightMax)
        {
            Weight = weight;
            WeightMax = weightMax;
        }
        #endregion
    }
}
