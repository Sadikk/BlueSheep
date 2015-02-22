using BlueSheep.Common.Data.D2o;
using BlueSheep.Common.Protocol.Types;

namespace BlueSheep.Common.Types
{
    public class Food
    {
        #region Properties
        public Core.Inventory.Item Informations { get; set; }

        public DataClass Datas { get; set; }
        #endregion

        #region Constructeurs
        public Food(Core.Inventory.Item informations, DataClass datas)
        {
            Informations = informations;
            Datas = datas;
        }
        #endregion
    }
}
