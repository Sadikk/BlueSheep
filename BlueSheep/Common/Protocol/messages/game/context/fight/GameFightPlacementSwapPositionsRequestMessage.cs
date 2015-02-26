using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.messages.game.context.fight
{
    public class GameFightPlacementSwapPositionsRequestMessage : Message
    {
        public new const uint ID = 6541;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        public int requestId;
        
        public GameFightPlacementSwapPositionsRequestMessage()
        {

        }
        public GameFightPlacementSwapPositionsRequestMessage(int param1, int requestId)
        {
           // il manquerait ça:
            // GameFightPlacementPositionRequestMessage(param1);
            this.requestId = requestId;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            //GameFightPlacementPositionRequestMessage.Serialize()
            writer.WriteInt(requestId);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            // GameFightPlacementPositionRequestMessage.Deserialize()
            requestId = reader.ReadInt();
        }
    }
}
