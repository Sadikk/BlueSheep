namespace BlueSheep.Common.Data.DataCenter.Items
{
    class ItemType : IDataCenter
    {
        #region Propriétés
        public string Module
        {
            get { return "ItemTypes"; }
        }

        public int ID { get; private set; }

        public int NameID { get; private set; }

        public string Name
        {
            get { return I18N.GetText(NameID); }
        }
        #endregion
        
        #region Constructeurs
        public ItemType()
        {
        }

        public ItemType(GameDataFileAccessor gameDataFileAccessor, int id, int nameID)
        {
            ID = id;
            NameID = nameID;
        }
        #endregion

        #region Méthodes public
        public static ItemType GetItemTypeByID(GameDataFileAccessor gameDataFileAccessor, int id)
        {
            return (ItemType) gameDataFileAccessor.GetObject("ItemTypes", id);
        }
        #endregion
    }
}