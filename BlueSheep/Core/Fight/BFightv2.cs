using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueSheep.Common.IO;
using BlueSheep.Interface;
using BlueSheep.Interface.Text;
using BlueSheep.Engine.Enums;
using BlueSheep.Common.Types;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Data.Pathfinding.Positions;
using BlueSheep.Engine.Types;
using BlueSheep.Common.Protocol.Messages;
using BlueSheep.Data.Pathfinding;
using System.Collections;
using BlueSheep.Common.Data.D2o;
using System.Diagnostics;

namespace BlueSheep.Core.Fight
{
    public class BFightv2
    {
        // Dictionary
        public Dictionary<string, int> winLoseDic;
        public Dictionary<DateTime, int> xpWon;
        public Dictionary<int, int> TotalLaunchBySpell = new Dictionary<int, int>();
        public Dictionary<int, int> LastTurnLaunchBySpell = new Dictionary<int, int>();
        public Dictionary<int, Dictionary<int, int>> TotalLaunchByCellBySpell = new Dictionary<int, Dictionary<int, int>>();

        // List
        public List<BSpell> Spells;
        public List<BFighter> Fighters = new List<BFighter>();

        // Configs
        public AccountUC Account;
        public FightConfig Conf;

        //Property
        public BFighter Fighter
        {
            get { return GetFighter(Account.CharacterBaseInformations.id); }
        }

        /// <summary>
        /// Constructeur (override)
        /// </summary>
        /// <param name="account">Compte associé</param>
        public BFightv2(AccountUC account)
        {
            Account = account;
        }

        /// <summary>
        /// Constructeur (override)
        /// </summary>
        /// <param name="myconf">Configuration de fight</param>
        /// <param name="myspells">Liste des sorts</param>
        /// <param name="account">Compte associé</param>
        public BFightv2(FightConfig myconf, List<BSpell> myspells, AccountUC account)
        {
            Account = account;
            Conf = myconf;
            Spells = myspells;
            winLoseDic = new Dictionary<string, int>();
            winLoseDic.Add("Gagné", 0);
            winLoseDic.Add("Perdu", 0);
            xpWon = new Dictionary<DateTime, int>();
            xpWon.Add(DateTime.Today, 0);
        }

        /// <summary>
        /// CanLaunchSpell() vérifie si le sort peut être lancé. (override)
        /// </summary>
        /// <param name="spellId">ID du sort</param>
        /// <returns>SpellInabilityReasons: Unknown, ActionPoints, TooManyLaunch, Cooldown, TooManyInvocations, None  </returns>
        public SpellInabilityReason CanLaunchSpell(int spellId)
        {
            DataClass spellData = GameData.GetDataObject(D2oFileEnum.Spells, spellId);
            ArrayList ids = ((ArrayList)spellData.Fields["spellLevels"]);
            int level = Account.Spells.FirstOrDefault(Spell => Spell.Id == spellId).Level;
            int id = Convert.ToInt32(ids[level - 1]);
            DataClass spellLevelsData = GameData.GetDataObject(D2oFileEnum.SpellLevels, id);
            if (spellLevelsData == null)
                return SpellInabilityReason.Unknown;
          

            if ((spellId != 0 && Fighter.ActionPoints < (int)spellLevelsData.Fields["apCost"])) //|| (weaponData != null && Fighter.ActionPoints < weaponData.apCost))
                return SpellInabilityReason.ActionPoints;

            if (TotalLaunchBySpell.ContainsKey(spellId) && TotalLaunchBySpell[spellId] >= (int)spellLevelsData.Fields["maxCastPerTurn"] && (int)spellLevelsData.Fields["maxCastPerTurn"] > 0)
                return SpellInabilityReason.TooManyLaunch;
            if (this.LastTurnLaunchBySpell.ContainsKey(spellId))
                return SpellInabilityReason.Cooldown;

            ArrayList listEffects = (ArrayList)spellLevelsData.Fields["effects"];
            //EffectInstanceDice
            if (((listEffects != null) && (listEffects.Count > 0)) && ((int)((DataClass)listEffects[0]).Fields["effectId"]) == 181)
            {
                CharacterCharacteristicsInformations stats = Account.CharacterStats;
                int total = stats.summonableCreaturesBoost.@base + stats.summonableCreaturesBoost.objectsAndMountBonus + stats.summonableCreaturesBoost.alignGiftBonus + stats.summonableCreaturesBoost.contextModif;
                if (this.GetInvokationNumber() >= total)
                    return SpellInabilityReason.TooManyInvocations;
            }
            
            return SpellInabilityReason.None;
        }

        /// <summary>
        /// CanLaunchSpell() vérifie si le sort peut être lancé. (override)
        /// </summary>
        /// <param name="spellId">ID du sort</param>
        /// <param name="characterCellId">CellId du personnage</param>
        /// <param name="cellId">CellId cible</param>
        /// <returns>SpellInabilityReasons: Unknown, ActionPoints, TooManyLaunch, Cooldown, TooManyInvocations, None </returns>
        public SpellInabilityReason CanLaunchSpell(int spellId, int characterCellId, int cellId)
        {
          
            DataClass spellData = GameData.GetDataObject(D2oFileEnum.Spells, spellId);
            ArrayList ids = (ArrayList)spellData.Fields["spellLevels"];
            int level = Account.Spells.FirstOrDefault(Spell => Spell.Id == spellId).Level;
            int id = Convert.ToInt32(ids[level - 1]);
            DataClass spellLevelsData = GameData.GetDataObject(D2oFileEnum.SpellLevels, id);
            if (spellLevelsData == null)
                return SpellInabilityReason.Unknown;

            MapPoint characterPoint = new MapPoint(characterCellId);
            MapPoint targetPoint = new MapPoint(cellId);
            int distanceToTarget = characterPoint.DistanceToCell(targetPoint);
            int minRange = (spellId != 0) ? (int)spellLevelsData.Fields["minRange"] : 0; //weaponData.minRange;
            if ((spellId != 0 && (bool)spellLevelsData.Fields["castInDiagonal"]))// || (weaponData != null && weaponData.castInDiagonal))
                minRange = (minRange * 2);
            if (minRange < 0)
                minRange = 0;
            int maxRange = (spellId != 0) ? (int)((int)spellLevelsData.Fields["range"] + ((bool)spellLevelsData.Fields["rangeCanBeBoosted"] ? Account.CharacterStats.range.objectsAndMountBonus : 0)) : (int)spellLevelsData.Fields["range"];
            if ((spellId != 0 && (bool)spellLevelsData.Fields["castInDiagonal"]))// || (weaponData != null && weaponData.castInDiagonal))
                maxRange = (maxRange * 2);
            if (maxRange < 0)
                maxRange = 0;
            if (distanceToTarget < minRange && distanceToTarget > 0)
                return SpellInabilityReason.MinRange;
            if (distanceToTarget > maxRange)
                return SpellInabilityReason.MaxRange;
            if (((spellId != 0 && (bool)spellLevelsData.Fields["castInLine"]))// || (weaponData != null && weaponData.castInDiagonal)) &&
               && characterPoint.X != targetPoint.X &&
                characterPoint.Y != targetPoint.Y)
                return SpellInabilityReason.NotInLine;
            if ((spellId != 0 && (bool)spellLevelsData.Fields["castInDiagonal"]))// || (weaponData != null && weaponData.castInDiagonal))
            {
                ArrayList list = Dofus1Line.GetLine(characterPoint.X, characterPoint.Y, targetPoint.X, targetPoint.Y);

                int i = 0;
                while (i < list.Count - 1)
                {
                    Dofus1Line.Point actualPoint = (Dofus1Line.Point)list[i];
                    Dofus1Line.Point nextPoint = (Dofus1Line.Point)list[i + 1];
                    i += 1;
                    if (actualPoint.X == nextPoint.X + 1 && actualPoint.Y == nextPoint.Y + 1)
                        continue;
                    else if (actualPoint.X == nextPoint.X - 1 && actualPoint.Y == nextPoint.Y - 1)
                        continue;
                    else if (actualPoint.X == nextPoint.X + 1 && actualPoint.Y == nextPoint.Y - 1)
                        continue;
                    else if (actualPoint.X == nextPoint.X - 1 && actualPoint.Y == nextPoint.Y + 1)
                        continue;
                    return SpellInabilityReason.NotInDiagonal;
                }
            }
            if (((spellId != 0 && (bool)spellLevelsData.Fields["castTestLos"] && distanceToTarget > 1)))// || (weaponData != null && weaponData.castTestLos)) && distanceToTarget > 1)
            {
                ArrayList list = Dofus1Line.GetLine(characterPoint.X, characterPoint.Y, targetPoint.X, targetPoint.Y);
                int i = 0;
                while (i < list.Count - 1)
                {
                    Dofus1Line.Point point3 = (Dofus1Line.Point)list[i];
                    MapPoint point4 = new MapPoint((int)Math.Round(Math.Floor(point3.X)), (int)Math.Round(Math.Floor(point3.Y)));
                    if (!(IsFreeCell(point4.CellId)) || !(Account.Map.Data.IsLineOfSight(point4.CellId)))
                        return SpellInabilityReason.LineOfSight;
                    i += 1;
                }
            }
            if ((TotalLaunchByCellBySpell.ContainsKey(spellId) && TotalLaunchByCellBySpell[spellId].ContainsKey(targetPoint.CellId)) && this.TotalLaunchByCellBySpell[spellId][targetPoint.CellId] >= (int)spellLevelsData.Fields["maxCastPerTarget"] && (int)spellLevelsData.Fields["maxCastPerTarget"] > 0)
                return SpellInabilityReason.TooManyLaunchOnCell;
            if (IsFreeCell(cellId))
            {
                if ((bool)spellLevelsData.Fields["needTakenCell"])
                    return SpellInabilityReason.NeedTakenCell;
            }
            else if ((bool)spellLevelsData.Fields["needFreeCell"])
                return SpellInabilityReason.NeedFreeCell;
            return SpellInabilityReason.None;
        }

        /// <summary>
        /// CanUseSpell() vérifie si le sort peut être utilisé. 
        /// </summary>
        /// <param name="spell">Sort à lancé</param>
        /// <param name="target">Cible</param>
        /// <returns>False si non, true si oui. </returns>
        public bool CanUseSpell(BSpell spell, BFighter target)
        {
            // Principally AP
            if (CanLaunchSpell(spell.SpellId) != SpellInabilityReason.None)
            {
                return false;
            }

            // Use without move
            if (CanLaunchSpell(spell.SpellId, Fighter.CellId, target.CellId) == SpellInabilityReason.None)
            {
                Account.Log(new BotTextInformation("No need to move maggle"), 5);
                return true;
            }
            // Try with move
            int moveCell = -1;
            int distance = -1;
            foreach (int cell in GetReachableCells())
            {
                if (CanLaunchSpell(spell.SpellId, cell, target.CellId) == SpellInabilityReason.None)
                {
                    MapPoint characterPoint = new MapPoint(cell);
                    int tempDistance = characterPoint.DistanceToCell(new MapPoint(target.CellId));

                    if (tempDistance > distance || distance == -1)
                    {
                        distance = tempDistance;
                        moveCell = cell;
                    }
                }
            }

            if (moveCell != -1 && moveCell != Fighter.CellId)
            {
                MoveToCell(moveCell);
                Account.Log(new BotTextInformation("CanUseSpellWithMove!"), 5);
                return true;
            }

            // Can't use
            Account.Log(new ErrorTextInformation("CantUseSpell"), 5);
            return false;
        }

        /// <summary>
        /// MoveToCell() permet de se déplacer sur la cellId indiqué.
        /// </summary>
        /// <param name="cellId">CellID cible</param>
        /// <returns>True si déplacement, False si aucun déplacement</returns>
        private bool MoveToCell(int cellId)
        {
            if (cellId != Fighter.CellId)
            {
                if (!(IsCellWalkable(cellId)))
                {
                    int num = -1;
                    int num2 = 5000;
                    MapPoint point = new MapPoint(Fighter.CellId);
                    MapPoint point2 = new MapPoint(cellId);
                    int direction = 1;
                    while (true)
                    {
                        MapPoint nearestCellInDirection = point2.GetNearestCellInDirection(direction, 1);
                        if (IsCellWalkable(nearestCellInDirection.CellId))
                        {
                            int num4 = point.DistanceToCell(nearestCellInDirection);
                            if (num4 < num2)
                            {
                                num2 = num4;
                                num = nearestCellInDirection.CellId;
                            }
                        }
                        direction = (direction + 2);
                        if (direction > 7)
                        {
                            if (num == -1)
                                return false;
                            cellId = num;
                            break;
                        }
                    }
                }
                SimplePathfinder pathfinder = new SimplePathfinder((BlueSheep.Data.D2p.Map)Account.Map.Data);
                pathfinder.SetFight(Fighters, Fighter.MovementPoints);
                MovementPath path = pathfinder.FindPath(Fighter.CellId, cellId);
                if (path != null)
                {
                    List<UInt32> serverMovement = MapMovementAdapter.GetServerMovement(path);
                    //Account.Network.SendToServer(new GameMapMovementRequestMessage(serverMovement.ToList().Select<uint, short>(ui => (short)ui).ToArray(), Account.Game.Map.Id));
                    using (BigEndianWriter writer = new BigEndianWriter())
                    {
                        GameMapMovementRequestMessage msg = new GameMapMovementRequestMessage(serverMovement.ToList().Select<uint, short>(ui => (short)ui).ToArray(), Account.Map.Id);
                        msg.Serialize(writer);
                        writer.Content = Account.HumanCheck.hash_function(writer.Content);
                        MessagePackaging pack = new MessagePackaging(writer);
                        pack.Pack((int)msg.ProtocolID);
                        Account.SocketManager.Send(pack.Writer.Content);
                        if (Account.DebugMode.Checked)
                            Account.Log(new BotTextInformation("[SND] 950 (GameMapMovementRequestMessage)"), 0);
                    }
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// GetFighter() retourne le fighter correspondant au fighterId indiqué
        /// </summary>
        /// <param name="fighterId">ID du fighter</param>
        /// <returns>BFighter fighter. </returns>
        public BFighter GetFighter(int fighterId)
        {
            return Fighters.FirstOrDefault(f => f.Id == fighterId);
        }

        /// <summary>
        /// GetInvokationNumber() retourne le nombre d'invocations présente
        /// </summary>
        /// <returns>Nombre d'invocation</returns>
        private int GetInvokationNumber()
        {
            int num = 0;
            foreach (BFighter fighter in Fighters)
            {
                if (fighter.GameFightMinimalStats.summoner == Fighter.Id)
                    num += 1;
            }
            return num;
        }

        /// <summary>
        /// GetReachableCells() récupère les cellules que l'on peut atteindre.
        /// </summary>
        /// <returns>List INT contenant les cellIds que l'on peut atteindre</returns>
        private List<int> GetReachableCells()
        {
            // TODO : Generate Losange form Fighter Point
            List<int> listCellulesMarchables = new List<int>();
            MapPoint point = new MapPoint(Fighter.CellId);
            int movementPoints = Fighter.MovementPoints;

            for (int i = 0; i < 600; i++)
            {
                if (IsCellWalkable(i))
                {
                    MapPoint cellPoint = new MapPoint(i);
                    if (cellPoint.DistanceToCell(point) <= movementPoints)
                        listCellulesMarchables.Add(i);
                }
            }

            if (listCellulesMarchables.Contains(point.CellId))
                listCellulesMarchables.Add(point.CellId);

            return listCellulesMarchables;
        }

        /// <summary>
        /// IsCellWalkable: permet de savoir si on peut marcher sur une cellule indiqué
        /// </summary>
        /// <param name="cellId">Cellule sur laquelle marché</param>
        /// <returns>True si oui; False si non.</returns>
        private bool IsCellWalkable(int cellId)
        {
            BlueSheep.Data.D2p.Map MapData = (BlueSheep.Data.D2p.Map)Account.Map.Data;
            if (Account.Map.Data.IsWalkable(cellId))
            {
                var selectedFighter = Fighters.FirstOrDefault((f) => f.CellId == cellId || MapData.Cells[cellId].NonWalkableDuringFight());
                if (selectedFighter != null)
                    return false;
                else
                    return true;
            }
            return false;
        }

        /// <summary>
        /// IsFreeCell: permet de savoir si la cellId renseigner est libre
        /// </summary>
        /// <param name="cellId">Cellule cible</param>
        /// <returns>True si oui; False si non.</returns>
        private bool IsFreeCell(int cellId)
        {
            return !Fighters.Any(f => f != null && f.CellId == cellId);
        }
        /// <summary>
        /// LaunchSpell() permet de lancer un sort.
        /// </summary>
        /// <param name="spellId">ID du sort à lancer</param>
        /// <param name="cellId">Cell ID cible</param>
        private void LaunchSpell(int spellId, int cellId)
        {
            using (BigEndianWriter writer = new BigEndianWriter())
            {
                GameActionFightCastRequestMessage msg = new GameActionFightCastRequestMessage((short)spellId, (short)cellId);
                msg.Serialize(writer);
                writer.Content = Account.HumanCheck.hash_function(writer.Content);
                MessagePackaging pack = new MessagePackaging(writer);
                pack.Pack((int)msg.ProtocolID);
                Account.SocketManager.Send(pack.Writer.Content);
                Account.Log(new ActionTextInformation("Lancement d'un sort en " + cellId), 5);
                if (Account.DebugMode.Checked)
                    Account.Log(new BotTextInformation("[SND] 1005 (GameActionFightCastRequestMessage)"), 0);
            }
        }
    }
}
