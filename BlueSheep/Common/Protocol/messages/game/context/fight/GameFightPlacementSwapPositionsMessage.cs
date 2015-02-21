









// Generated on 12/11/2014 19:01:28
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class GameFightPlacementSwapPositionsMessage : Message
    {
        public new const uint ID =6544;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public Types.IdentifiedEntityDispositionInformations[] dispositions;
        
        public GameFightPlacementSwapPositionsMessage()
        {
        }
        
        public GameFightPlacementSwapPositionsMessage(Types.IdentifiedEntityDispositionInformations[] dispositions)
        {
            this.dispositions = dispositions;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
          // writer.WriteUShort((ushort)dispositions.Length);
            foreach (var entry in dispositions)
            {
                 entry.Serialize(writer);
            }
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            var limit = reader.ReadUShort();
            dispositions = new Types.IdentifiedEntityDispositionInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 dispositions[i] = new Types.IdentifiedEntityDispositionInformations();
                 dispositions[i].Deserialize(reader);
            }
        }
        
    }
    
}