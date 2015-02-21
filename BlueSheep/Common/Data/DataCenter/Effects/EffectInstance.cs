using System;
namespace BlueSheep.Common.Data.DataCenter.Effects
{
    class EffectInstance : IDataCenter
    {
        #region Propriétés
        public string Module
        {
            get { return "Effects"; }
        }
        #endregion

        #region Constructeurs
        public EffectInstance()
        {
        }

        public EffectInstance(GameDataFileAccessor gameDataFileAccessor)
        {
        }

        public uint effectId;
        public int targetId;
        public String targetMask;
        public int duration;
        public int delay;
        public int random;
        public int group;
        public int modificator;
        public Boolean trigger;
        public String triggers;
        public Boolean hidden;
        public int order;
        public Object zoneSize;
        public uint zoneShape;
        public Object zoneMinSize;
        public Object zoneEfficiencyPercent;
        public Object zoneMaxEfficiency;
        public Boolean rawZoneInit;
        public String rawZone;
        #endregion
    }
}