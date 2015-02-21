


















// Generated on 12/11/2014 19:02:07
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.IO;


namespace BlueSheep.Common.Protocol.Types
{

public class JobExperience
{

public new const short ID = 98;
public virtual short TypeId
{
    get { return ID; }
}

public sbyte jobId;
        public sbyte jobLevel;
        public double jobXP;
        public double jobXpLevelFloor;
        public double jobXpNextLevelFloor;
        

public JobExperience()
{
}

public JobExperience(sbyte jobId, sbyte jobLevel, double jobXP, double jobXpLevelFloor, double jobXpNextLevelFloor)
        {
            this.jobId = jobId;
            this.jobLevel = jobLevel;
            this.jobXP = jobXP;
            this.jobXpLevelFloor = jobXpLevelFloor;
            this.jobXpNextLevelFloor = jobXpNextLevelFloor;
        }
        

public virtual void Serialize(BigEndianWriter writer)
{

writer.WriteSByte(jobId);
            writer.WriteSByte(jobLevel);
            writer.WriteVarLong(jobXP);
            writer.WriteVarLong(jobXpLevelFloor);
            writer.WriteVarLong(jobXpNextLevelFloor);
            

}

public virtual void Deserialize(BigEndianReader reader)
{

jobId = reader.ReadSByte();
            if (jobId < 0)
                throw new Exception("Forbidden value on jobId = " + jobId + ", it doesn't respect the following condition : jobId < 0");
            jobLevel = reader.ReadSByte();
            if (jobLevel < 0)
                throw new Exception("Forbidden value on jobLevel = " + jobLevel + ", it doesn't respect the following condition : jobLevel < 0");
            jobXP = reader.ReadVarUhLong();
            if (jobXP < 0 || jobXP > 9.007199254740992E15)
                throw new Exception("Forbidden value on jobXP = " + jobXP + ", it doesn't respect the following condition : jobXP < 0 || jobXP > 9.007199254740992E15");
            jobXpLevelFloor = reader.ReadVarUhLong();
            if (jobXpLevelFloor < 0 || jobXpLevelFloor > 9.007199254740992E15)
                throw new Exception("Forbidden value on jobXpLevelFloor = " + jobXpLevelFloor + ", it doesn't respect the following condition : jobXpLevelFloor < 0 || jobXpLevelFloor > 9.007199254740992E15");
            jobXpNextLevelFloor = reader.ReadVarUhLong();
            if (jobXpNextLevelFloor < 0 || jobXpNextLevelFloor > 9.007199254740992E15)
                throw new Exception("Forbidden value on jobXpNextLevelFloor = " + jobXpNextLevelFloor + ", it doesn't respect the following condition : jobXpNextLevelFloor < 0 || jobXpNextLevelFloor > 9.007199254740992E15");
            

}


}


}