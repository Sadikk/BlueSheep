









// Generated on 12/11/2014 19:01:26
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class GameContextCreateMessage : Message
    {
        public new const uint ID =200;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public sbyte context;
        
        public GameContextCreateMessage()
        {
        }
        
        public GameContextCreateMessage(sbyte context)
        {
            this.context = context;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteSByte(context);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            context = reader.ReadSByte();
            if (context < 0)
                throw new Exception("Forbidden value on context = " + context + ", it doesn't respect the following condition : context < 0");
        }
        
    }
    
}