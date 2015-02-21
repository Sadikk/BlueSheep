namespace BlueSheep.Common.Data.DataCenter.Effects
{
    class Effect : IDataCenter
    {
        #region Propriétés
        public string Module
        {
            get { return "Effects"; }
        }

        public int ID { get; private set; }

        public int DescriptionID { get; private set; }

        public string Operator { get; private set; }

        public string Description
        {
            get { return I18N.GetText(DescriptionID); }
        }
        #endregion

        #region Constructeurs
        public Effect()
        {
        }

        public Effect(GameDataFileAccessor gameDataFileAccessor, int id, int descriptionID, string operatoR)
        {
            ID = id;
            DescriptionID = descriptionID;
            Operator = operatoR;
        }
        #endregion
    }
}
