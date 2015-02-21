









// Generated on 12/11/2014 19:01:44
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
    public class GuildInformationsGeneralMessage : Message
    {
        public new const uint ID =5557;
        public override uint ProtocolID
        {
            get { return ID; }
        }
        
        public bool enabled;
        public bool abandonnedPaddock;
        public byte level;
        public double expLevelFloor;
        public double experience;
        public double expNextLevelFloor;
        public int creationDate;
        public short nbTotalMembers;
        public short nbConnectedMembers;
        
        public GuildInformationsGeneralMessage()
        {
        }

        public GuildInformationsGeneralMessage(bool enabled, bool abandonnedPaddock, byte level, double expLevelFloor, double experience, double expNextLevelFloor, int creationDate, short nbTotalMembers, short nbConnectedMembers)
        {
            this.enabled = enabled;
            this.abandonnedPaddock = abandonnedPaddock;
            this.level = level;
            this.expLevelFloor = expLevelFloor;
            this.experience = experience;
            this.expNextLevelFloor = expNextLevelFloor;
            this.creationDate = creationDate;
            this.nbTotalMembers = nbTotalMembers;
            this.nbConnectedMembers = nbConnectedMembers;
        }
        
        public override void Serialize(BigEndianWriter writer)
        {
            byte flag1 = 0;
            flag1 = BooleanByteWrapper.SetFlag(flag1, 0, enabled);
            flag1 = BooleanByteWrapper.SetFlag(flag1, 1, abandonnedPaddock);
            writer.WriteByte(flag1);
            writer.WriteByte(level);
            writer.WriteVarLong(expLevelFloor);
            writer.WriteVarLong(experience);
            writer.WriteVarLong(expNextLevelFloor);
            writer.WriteInt(creationDate);
            writer.WriteVarShort(nbTotalMembers);
            writer.WriteVarShort(nbConnectedMembers);
        }
        
        public override void Deserialize(BigEndianReader reader)
        {
            byte flag1 = reader.ReadByte();
            enabled = BooleanByteWrapper.GetFlag(flag1, 0);
            abandonnedPaddock = BooleanByteWrapper.GetFlag(flag1, 1);
            level = reader.ReadByte();
            if (level < 0 || level > 255)
                throw new Exception("Forbidden value on level = " + level + ", it doesn't respect the following condition : level < 0 || level > 255");
            expLevelFloor = reader.ReadVarUhLong();
            if (expLevelFloor < 0 || expLevelFloor > 9.007199254740992E15)
                throw new Exception("Forbidden value on expLevelFloor = " + expLevelFloor + ", it doesn't respect the following condition : expLevelFloor < 0 || expLevelFloor > 9.007199254740992E15");
            experience = reader.ReadVarUhLong();
            if (experience < 0 || experience > 9.007199254740992E15)
                throw new Exception("Forbidden value on experience = " + experience + ", it doesn't respect the following condition : experience < 0 || experience > 9.007199254740992E15");
            expNextLevelFloor = reader.ReadVarUhLong();
            if (expNextLevelFloor < 0 || expNextLevelFloor > 9.007199254740992E15)
                throw new Exception("Forbidden value on expNextLevelFloor = " + expNextLevelFloor + ", it doesn't respect the following condition : expNextLevelFloor < 0 || expNextLevelFloor > 9.007199254740992E15");
            creationDate = reader.ReadInt();
            if (creationDate < 0)
                throw new Exception("Forbidden value on creationDate = " + creationDate + ", it doesn't respect the following condition : creationDate < 0");
            nbTotalMembers = reader.ReadVarShort();
            if (nbTotalMembers < 0)
                throw new Exception("Forbidden value on nbTotalMembers = " + nbTotalMembers + ", it doesn't respect the following condition : nbTotalMembers < 0");
            nbConnectedMembers = reader.ReadVarShort();
            if (nbConnectedMembers < 0)
                throw new Exception("Forbidden value on nbConnectedMembers = " + nbConnectedMembers + ", it doesn't respect the following condition : nbConnectedMembers < 0");
        }
        
    }
    
}