namespace BlueSheep.Common.Data.DataCenter.Effects.Instances
{
    class EffectInstanceDice : EffectInstanceInteger
    {
        #region Constructeurs
        public EffectInstanceDice()
        {
        }

        public EffectInstanceDice(GameDataFileAccessor gameDataFileAccessor)
        {
        }
        #endregion

        public uint diceNum;
        public uint diceSide;

    }
}
