using BlueSheep.Common.IO;

namespace BlueSheep.Engine.Types
{
    public abstract class Message
    {
        #region Properties
        public abstract uint ProtocolID { get; }
        #endregion

        #region Public methods
        public abstract void Serialize(BigEndianWriter writer);

        public abstract void Deserialize(BigEndianReader reader);
        #endregion
    }
}
