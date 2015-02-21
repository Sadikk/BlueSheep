









// Generated on 12/11/2014 19:01:41
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class StatsUpgradeRequestMessage : Message
    {
        public new const uint ID =5610;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public bool useAdditionnal;
        public sbyte statId;
        public short boostPoint;
        
        public StatsUpgradeRequestMessage()
        {
        }
        
        public StatsUpgradeRequestMessage(bool useAdditionnal, sbyte statId, short boostPoint)
        {
            this.useAdditionnal = useAdditionnal;
            this.statId = statId;
            this.boostPoint = boostPoint;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteBoolean(useAdditionnal);
            writer.WriteSByte(statId);
            writer.WriteVarShort(boostPoint);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            useAdditionnal = reader.ReadBoolean();
            statId = reader.ReadSByte();
            if (statId < 0)
                throw new Exception("Forbidden value on statId = " + statId + ", it doesn't respect the following condition : statId < 0");
            boostPoint = reader.ReadVarShort();
            if (boostPoint < 0)
                throw new Exception("Forbidden value on boostPoint = " + boostPoint + ", it doesn't respect the following condition : boostPoint < 0");
        }
        
    }
    
}