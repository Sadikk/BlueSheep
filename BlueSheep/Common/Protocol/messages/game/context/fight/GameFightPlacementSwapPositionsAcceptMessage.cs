using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.messages.game.context.fight
{
    public class GameFightPlacementSwapPositionsAcceptMessage : Message
    {
        public new const int ID = 6547;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        public int requestId;

        public GameFightPlacementSwapPositionsAcceptMessage()
        {
        }

        public GameFightPlacementSwapPositionsAcceptMessage(int requestId)
        {
            this.requestId = requestId;
        }

        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(requestId);
        }

        public override void Deserialize(BigEndianReader reader)
        {
            requestId = reader.ReadInt();
        }
    }
}
