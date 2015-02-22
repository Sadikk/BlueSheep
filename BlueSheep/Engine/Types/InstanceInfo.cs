using System;
using System.Reflection;

namespace BlueSheep.Engine.Types
{
    class InstanceInfo
    {
        #region Properties
        public uint ProtocolID { get; set; }

        public Type MessageType { get; set; }

        public MethodInfo Method { get; set; }
        #endregion

        #region Constructeurs
        public InstanceInfo(uint protocolID, Type messageType, MethodInfo method)
        {
            ProtocolID = protocolID;
            MessageType = messageType;
            Method = method;
        }
        #endregion
    }
}
