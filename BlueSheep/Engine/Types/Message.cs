using BlueSheep.Common.IO;

namespace BlueSheep.Engine.Types
{
    public abstract class Message
    {
        #region Propriétés
        public abstract uint ProtocolID { get; }
        #endregion

        #region Méthodes publiques
        public abstract void Serialize(BigEndianWriter writer);

        public abstract void Deserialize(BigEndianReader reader);
        #endregion
    }
}
