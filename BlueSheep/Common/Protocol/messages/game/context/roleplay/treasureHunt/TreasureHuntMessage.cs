









// Generated on 12/11/2014 19:01:42
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class TreasureHuntMessage : Message
    {
        public new const uint ID =6486;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public sbyte questType;
        public int startMapId;
        public Types.TreasureHuntStep[] knownStepsList;
        public sbyte totalStepCount;
        public int checkPointCurrent;
        public int checkPointTotal;
        public int availableRetryCount;
        public Types.TreasureHuntFlag[] flags;
        
        public TreasureHuntMessage()
        {
        }
        
        public TreasureHuntMessage(sbyte questType, int startMapId, Types.TreasureHuntStep[] knownStepsList, sbyte totalStepCount, int checkPointCurrent, int checkPointTotal, int availableRetryCount, Types.TreasureHuntFlag[] flags)
        {
            this.questType = questType;
            this.startMapId = startMapId;
            this.knownStepsList = knownStepsList;
            this.totalStepCount = totalStepCount;
            this.checkPointCurrent = checkPointCurrent;
            this.checkPointTotal = checkPointTotal;
            this.availableRetryCount = availableRetryCount;
            this.flags = flags;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteSByte(questType);
            writer.WriteInt(startMapId);
            writer.WriteUShort((ushort)knownStepsList.Length);
            foreach (var entry in knownStepsList)
            {
                 writer.WriteShort(entry.TypeId);
                 entry.Serialize(writer);
            }
            writer.WriteSByte(totalStepCount);
            writer.WriteVarInt(checkPointCurrent);
            writer.WriteVarInt(checkPointTotal);
            writer.WriteInt(availableRetryCount);
            writer.WriteUShort((ushort)flags.Length);
            foreach (var entry in flags)
            {
                 entry.Serialize(writer);
            }
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            questType = reader.ReadSByte();
            if (questType < 0)
                throw new Exception("Forbidden value on questType = " + questType + ", it doesn't respect the following condition : questType < 0");
            startMapId = reader.ReadInt();
            var limit = reader.ReadUShort();
            knownStepsList = new Types.TreasureHuntStep[limit];
            for (int i = 0; i < limit; i++)
            {
                 knownStepsList[i] = Types.ProtocolTypeManager.GetInstance<Types.TreasureHuntStep>(reader.ReadShort());
                 knownStepsList[i].Deserialize(reader);
            }
            totalStepCount = reader.ReadSByte();
            if (totalStepCount < 0)
                throw new Exception("Forbidden value on totalStepCount = " + totalStepCount + ", it doesn't respect the following condition : totalStepCount < 0");
            checkPointCurrent = reader.ReadVarInt();
            if (checkPointCurrent < 0)
                throw new Exception("Forbidden value on checkPointCurrent = " + checkPointCurrent + ", it doesn't respect the following condition : checkPointCurrent < 0");
            checkPointTotal = reader.ReadVarInt();
            if (checkPointTotal < 0)
                throw new Exception("Forbidden value on checkPointTotal = " + checkPointTotal + ", it doesn't respect the following condition : checkPointTotal < 0");
            availableRetryCount = reader.ReadInt();
            limit = reader.ReadUShort();
            flags = new Types.TreasureHuntFlag[limit];
            for (int i = 0; i < limit; i++)
            {
                 flags[i] = new Types.TreasureHuntFlag();
                 flags[i].Deserialize(reader);
            }
        }
        
    }
    
}