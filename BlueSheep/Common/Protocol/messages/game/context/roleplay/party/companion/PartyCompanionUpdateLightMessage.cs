









// Generated on 12/11/2014 19:01:40
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class PartyCompanionUpdateLightMessage : PartyUpdateLightMessage
    {
        public new const uint ID =6472;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public sbyte indexId;
        
        public PartyCompanionUpdateLightMessage()
        {
        }
        
        public PartyCompanionUpdateLightMessage(int partyId, int id, int lifePoints, int maxLifePoints, short prospecting, byte regenRate, sbyte indexId)
         : base(partyId, id, lifePoints, maxLifePoints, prospecting, regenRate)
        {
            this.indexId = indexId;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            base.Serialize(writer);
            writer.WriteSByte(indexId);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            base.Deserialize(reader);
            indexId = reader.ReadSByte();
            if (indexId < 0)
                throw new Exception("Forbidden value on indexId = " + indexId + ", it doesn't respect the following condition : indexId < 0");
        }
        
    }
    
}