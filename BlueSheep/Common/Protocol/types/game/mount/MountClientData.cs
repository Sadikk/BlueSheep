


















// Generated on 12/11/2014 19:02:11
using System;
using System.Collections.Generic;
using System.Linq;
using BlueSheep.Common.IO;


namespace BlueSheep.Common.Protocol.Types
{

public class MountClientData
{

public new const short ID = 178;
public virtual short TypeId
{
    get { return ID; }
}

public double id;
        public int model;
        public int[] ancestor;
        public int[] behaviors;
        public string name;
        public int ownerId;
        public double experience;
        public double experienceForLevel;
        public double experienceForNextLevel;
        public sbyte level;
        public int maxPods;
        public int stamina;
        public int staminaMax;
        public int maturity;
        public int maturityForAdult;
        public int energy;
        public int energyMax;
        public int serenity;
        public int aggressivityMax;
        public int serenityMax;
        public int love;
        public int loveMax;
        public int fecondationTime;
        public int boostLimiter;
        public double boostMax;
        public int reproductionCount;
        public int reproductionCountMax;
        public Types.ObjectEffectInteger[] effectList;
    public bool sex;
    public bool isRideable;
    public bool isWild;
    public bool isFecondationReady;
        

public MountClientData()
{
}

public MountClientData(double id, int model, int[] ancestor, int[] behaviors, string name, int ownerId, double experience, double experienceForLevel, double experienceForNextLevel, sbyte level, int maxPods, int stamina, int staminaMax, int maturity, int maturityForAdult, int energy, int energyMax, int serenity, int aggressivityMax, int serenityMax, int love, int loveMax, int fecondationTime, int boostLimiter, double boostMax, int reproductionCount, int reproductionCountMax, Types.ObjectEffectInteger[] effectList)
        {
            this.id = id;
            this.model = model;
            this.ancestor = ancestor;
            this.behaviors = behaviors;
            this.name = name;
            this.ownerId = ownerId;
            this.experience = experience;
            this.experienceForLevel = experienceForLevel;
            this.experienceForNextLevel = experienceForNextLevel;
            this.level = level;
            this.maxPods = maxPods;
            this.stamina = stamina;
            this.staminaMax = staminaMax;
            this.maturity = maturity;
            this.maturityForAdult = maturityForAdult;
            this.energy = energy;
            this.energyMax = energyMax;
            this.serenity = serenity;
            this.aggressivityMax = aggressivityMax;
            this.serenityMax = serenityMax;
            this.love = love;
            this.loveMax = loveMax;
            this.fecondationTime = fecondationTime;
            this.boostLimiter = boostLimiter;
            this.boostMax = boostMax;
            this.reproductionCount = reproductionCount;
            this.reproductionCountMax = reproductionCountMax;
            this.effectList = effectList;
        }
        

public virtual void Serialize(BigEndianWriter writer)
{

writer.WriteDouble(id);
            writer.WriteVarInt(model);
            writer.WriteUShort((ushort)ancestor.Length);
            foreach (var entry in ancestor)
            {
                 writer.WriteInt(entry);
            }
            writer.WriteUShort((ushort)behaviors.Length);
            foreach (var entry in behaviors)
            {
                 writer.WriteInt(entry);
            }
            writer.WriteUTF(name);
            writer.WriteInt(ownerId);
            writer.WriteVarLong(experience);
            writer.WriteVarLong(experienceForLevel);
            writer.WriteDouble(experienceForNextLevel);
            writer.WriteSByte(level);
            writer.WriteVarInt(maxPods);
            writer.WriteVarInt(stamina);
            writer.WriteVarInt(staminaMax);
            writer.WriteVarInt(maturity);
            writer.WriteVarInt(maturityForAdult);
            writer.WriteVarInt(energy);
            writer.WriteVarInt(energyMax);
            writer.WriteInt(serenity);
            writer.WriteInt(aggressivityMax);
            writer.WriteVarInt(serenityMax);
            writer.WriteVarInt(love);
            writer.WriteVarInt(loveMax);
            writer.WriteInt(fecondationTime);
            writer.WriteInt(boostLimiter);
            writer.WriteDouble(boostMax);
            writer.WriteInt(reproductionCount);
            writer.WriteVarInt(reproductionCountMax);
            writer.WriteUShort((ushort)effectList.Length);
            foreach (var entry in effectList)
            {
                 entry.Serialize(writer);
            }
            

}

public virtual void Deserialize(BigEndianReader reader)
{
             byte b = reader.ReadByte();
         this.sex = BooleanByteWrapper.GetFlag(b,0);
         this.isRideable = BooleanByteWrapper.GetFlag(b,1);
         this.isWild = BooleanByteWrapper.GetFlag(b,2);
         this.isFecondationReady = BooleanByteWrapper.GetFlag(b,3);
id = reader.ReadDouble();
            if (id < -9.007199254740992E15 || id > 9.007199254740992E15)
                throw new Exception("Forbidden value on id = " + id + ", it doesn't respect the following condition : id < -9.007199254740992E15 || id > 9.007199254740992E15");
            model = reader.ReadVarInt();
            if (model < 0)
                throw new Exception("Forbidden value on model = " + model + ", it doesn't respect the following condition : model < 0");
            var limit = reader.ReadUShort();
            ancestor = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                 ancestor[i] = reader.ReadInt();
            }
            limit = reader.ReadUShort();
            behaviors = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                 behaviors[i] = reader.ReadInt();
            }
            name = reader.ReadUTF();
            ownerId = reader.ReadInt();
            if (ownerId < 0)
                throw new Exception("Forbidden value on ownerId = " + ownerId + ", it doesn't respect the following condition : ownerId < 0");
            experience = reader.ReadVarUhLong();
            if (experience < 0 || experience > 9.007199254740992E15)
                throw new Exception("Forbidden value on experience = " + experience + ", it doesn't respect the following condition : experience < 0 || experience > 9.007199254740992E15");
            experienceForLevel = reader.ReadVarUhLong();
            if (experienceForLevel < 0 || experienceForLevel > 9.007199254740992E15)
                throw new Exception("Forbidden value on experienceForLevel = " + experienceForLevel + ", it doesn't respect the following condition : experienceForLevel < 0 || experienceForLevel > 9.007199254740992E15");
            experienceForNextLevel = reader.ReadDouble();
            if (experienceForNextLevel < -9.007199254740992E15 || experienceForNextLevel > 9.007199254740992E15)
                throw new Exception("Forbidden value on experienceForNextLevel = " + experienceForNextLevel + ", it doesn't respect the following condition : experienceForNextLevel < -9.007199254740992E15 || experienceForNextLevel > 9.007199254740992E15");
            level = reader.ReadSByte();
            if (level < 0)
                throw new Exception("Forbidden value on level = " + level + ", it doesn't respect the following condition : level < 0");
            maxPods = reader.ReadVarInt();
            if (maxPods < 0)
                throw new Exception("Forbidden value on maxPods = " + maxPods + ", it doesn't respect the following condition : maxPods < 0");
            stamina = reader.ReadVarInt();
            if (stamina < 0)
                throw new Exception("Forbidden value on stamina = " + stamina + ", it doesn't respect the following condition : stamina < 0");
            staminaMax = reader.ReadVarInt();
            if (staminaMax < 0)
                throw new Exception("Forbidden value on staminaMax = " + staminaMax + ", it doesn't respect the following condition : staminaMax < 0");
            maturity = reader.ReadVarInt();
            if (maturity < 0)
                throw new Exception("Forbidden value on maturity = " + maturity + ", it doesn't respect the following condition : maturity < 0");
            maturityForAdult = reader.ReadVarInt();
            if (maturityForAdult < 0)
                throw new Exception("Forbidden value on maturityForAdult = " + maturityForAdult + ", it doesn't respect the following condition : maturityForAdult < 0");
            energy = reader.ReadVarInt();
            if (energy < 0)
                throw new Exception("Forbidden value on energy = " + energy + ", it doesn't respect the following condition : energy < 0");
            energyMax = reader.ReadVarInt();
            if (energyMax < 0)
                throw new Exception("Forbidden value on energyMax = " + energyMax + ", it doesn't respect the following condition : energyMax < 0");
            serenity = reader.ReadInt();
            aggressivityMax = reader.ReadInt();
            serenityMax = reader.ReadVarInt();
            if (serenityMax < 0)
                throw new Exception("Forbidden value on serenityMax = " + serenityMax + ", it doesn't respect the following condition : serenityMax < 0");
            love = reader.ReadVarInt();
            if (love < 0)
                throw new Exception("Forbidden value on love = " + love + ", it doesn't respect the following condition : love < 0");
            loveMax = reader.ReadVarInt();
            if (loveMax < 0)
                throw new Exception("Forbidden value on loveMax = " + loveMax + ", it doesn't respect the following condition : loveMax < 0");
            fecondationTime = reader.ReadInt();
            boostLimiter = reader.ReadInt();
            if (boostLimiter < 0)
                throw new Exception("Forbidden value on boostLimiter = " + boostLimiter + ", it doesn't respect the following condition : boostLimiter < 0");
            boostMax = reader.ReadDouble();
            if (boostMax < -9.007199254740992E15 || boostMax > 9.007199254740992E15)
                throw new Exception("Forbidden value on boostMax = " + boostMax + ", it doesn't respect the following condition : boostMax < -9.007199254740992E15 || boostMax > 9.007199254740992E15");
            reproductionCount = reader.ReadInt();
            reproductionCountMax = reader.ReadVarInt();
            if (reproductionCountMax < 0)
                throw new Exception("Forbidden value on reproductionCountMax = " + reproductionCountMax + ", it doesn't respect the following condition : reproductionCountMax < 0");
            limit = reader.ReadUShort();
            effectList = new Types.ObjectEffectInteger[limit];
            for (int i = 0; i < limit; i++)
            {
                 effectList[i] = new Types.ObjectEffectInteger();
                 effectList[i].Deserialize(reader);
            }
            

}


}


}