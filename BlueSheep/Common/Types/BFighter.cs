using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueSheep.Common.Protocol.Types;

namespace BlueSheep.Common.Types
{
    public class BFighter : BEntity
    {
        public BFighter(int id, int cellId, int actionPoints, GameFightMinimalStats gameFightMinimalStats, bool isAlive, int lifePoints, int maxLifePoints, int movementPoints, uint teamId, int creatureId) : base(id, cellId)
        {
            ActionPoints = actionPoints;
            GameFightMinimalStats = gameFightMinimalStats;
            IsAlive = isAlive;
            LifePoints = lifePoints;
            MaxLifePoints = maxLifePoints;
            MovementPoints = movementPoints;
            TeamId = teamId;
            CreatureGenericId = creatureId;
        }

        public int ActionPoints { get; internal set; }
        public GameFightMinimalStats GameFightMinimalStats { get; private set; }
        public bool IsAlive { get; internal set; }
        public int LifePoints { get; internal set; }
        public int MaxLifePoints { get; private set; }
        public int MovementPoints { get; internal set; }
        public uint TeamId { get; private set; }
        public int CreatureGenericId { get; private set; }
    }
}
