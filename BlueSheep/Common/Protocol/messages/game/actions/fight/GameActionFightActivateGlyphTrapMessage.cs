









// Generated on 12/11/2014 19:01:16
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class GameActionFightActivateGlyphTrapMessage : AbstractGameActionMessage
    {
        public new const uint ID =6545;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public short markId;
        public bool active;
        
        public GameActionFightActivateGlyphTrapMessage()
        {
        }
        
        public GameActionFightActivateGlyphTrapMessage(short actionId, int sourceId, short markId, bool active)
         : base(actionId, sourceId)
        {
            this.markId = markId;
            this.active = active;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            base.Serialize(writer);
            writer.WriteShort(markId);
            writer.WriteBoolean(active);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            base.Deserialize(reader);
            markId = reader.ReadShort();
            active = reader.ReadBoolean();
        }
        
    }
    
}