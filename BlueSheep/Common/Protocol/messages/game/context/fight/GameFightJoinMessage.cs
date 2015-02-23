









// Generated on 12/11/2014 19:01:27
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class GameFightJoinMessage : Message
    {
        public new const uint ID =702;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public bool canBeCancelled;
        public bool canSayReady;
        public bool isFightStarted;
        public int timeMaxBeforeFightStart;
        public sbyte fightType;
        
        public GameFightJoinMessage()
        {
        }
        
        public GameFightJoinMessage(bool canBeCancelled, bool canSayReady, bool isFightStarted, int timeMaxBeforeFightStart, sbyte fightType)
        {
            this.canBeCancelled = canBeCancelled;
            this.canSayReady = canSayReady;
            this.isFightStarted = isFightStarted;
            this.timeMaxBeforeFightStart = timeMaxBeforeFightStart;
            this.fightType = fightType;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            byte flag1 = 0;
            flag1 = BooleanByteWrapper.SetFlag(flag1, 0, canBeCancelled);
            flag1 = BooleanByteWrapper.SetFlag(flag1, 1, canSayReady);
            flag1 = BooleanByteWrapper.SetFlag(flag1, 2, isFightStarted);
            writer.WriteByte(flag1);
            //writer.WriteInt(timeMaxBeforeFightStart);
            writer.WriteShort((short)timeMaxBeforeFightStart);
            writer.WriteSByte(fightType);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            byte flag1 = reader.ReadByte();
            canBeCancelled = BooleanByteWrapper.GetFlag(flag1, 0);
            canSayReady = BooleanByteWrapper.GetFlag(flag1, 1);
            isFightStarted = BooleanByteWrapper.GetFlag(flag1, 2);
            //timeMaxBeforeFightStart = reader.ReadInt();
            timeMaxBeforeFightStart = reader.ReadShort();
            if (timeMaxBeforeFightStart < 0)
                throw new Exception("Forbidden value on timeMaxBeforeFightStart = " + timeMaxBeforeFightStart + ", it doesn't respect the following condition : timeMaxBeforeFightStart < 0");
            fightType = reader.ReadSByte();
            if (fightType < 0)
                throw new Exception("Forbidden value on fightType = " + fightType + ", it doesn't respect the following condition : fightType < 0");
        }
        
    }
    
}