using System;

namespace BlueSheep.Engine.Handlers
{
    class MessageHandler : Attribute
    {
        #region Properties
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
