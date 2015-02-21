namespace BlueSheep.Common.Data.DataCenter.Items
{
   public  class Item : IDataCenter
    {
        #region Attributs
        private readonly GameDataFileAccessor m_GameDataFileAccessor;
        #endregion

        #region Propriétés
        public string Module
        {
            get { return "Items"; }
        }

        public int ID { get; private set; }

        public int NameID { get; private set; }

        public int TypeID { get; private set; }

        public int RealWeight { get; private set; }

        public string Name
        {
            get { return I18N.GetText(NameID); }
        }

        public object Type
        {
            get { return ItemType.GetItemTypeByID(m_GameDataFileAccessor, TypeID); }
        }
        #endregion
        
        #region Constructeurs
        public Item()
        {
        }

        public Item(GameDataFileAccessor gameDataFileAccessor, int id, int nameID, int typeID, int realWeight)
        {
            m_GameDataFileAccessor = gameDataFileAccessor;
            ID = id;
            TypeID = typeID;
            NameID = nameID;
            RealWeight = realWeight;
        }
        #endregion
    }
}