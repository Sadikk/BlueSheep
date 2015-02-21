using System;

namespace BlueSheep.Engine.Handlers
{
    class MessageHandler : Attribute
    {
        #region Propriétés
        public Type MessageType { get; set; }
        #endregion

        #region Constructeurs
        public MessageHandler()
        {
        }

        public MessageHandler(Type messageType)
        {
            MessageType = messageType;
        }
        #endregion
    }
}
