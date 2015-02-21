









// Generated on 12/11/2014 19:01:43
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class IgnoredAddedMessage : Message
    {
        public new const uint ID =5678;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public Types.IgnoredInformations ignoreAdded;
        public bool session;
        
        public IgnoredAddedMessage()
        {
        }
        
        public IgnoredAddedMessage(Types.IgnoredInformations ignoreAdded, bool session)
        {
            this.ignoreAdded = ignoreAdded;
            this.session = session;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteShort(ignoreAdded.TypeId);
            ignoreAdded.Serialize(writer);
            writer.WriteBoolean(session);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            ignoreAdded = Types.ProtocolTypeManager.GetInstance<Types.IgnoredInformations>(reader.ReadShort());
            ignoreAdded.Deserialize(reader);
            session = reader.ReadBoolean();
        }
        
    }
    
}