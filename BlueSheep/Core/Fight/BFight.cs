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
    public class BFight
    {
        #region Fields
        #region Dictionary
        public Dictionary<DateTime, int> xpWon;
        public Dictionary<string, int> winLoseDic;
        public Dictionary<int, int> DurationByEffect = new Dictionary<int, int>();
        public Dictionary<int, int> LastTurnLaunchBySpell = new Dictionary<int, int>();
        public Dictionary<int, int> TotalLaunchBySpell = new Dictionary<int, int>();
        public Dictionary<int, Dictionary<int, int>> TotalLaunchByCellBySpell = new Dictionary<int, Dictionary<int, int>>();
        #endregion

        #region Public Fields
        public AccountUC m_Account;
        public List<BSpell> m_Spells;
        public FightConfig m_Conf;
        List<BSpell> spells;
       

        public BFighter NearMonster;
        public List<BFighter> Fighters = new List<BFighter>();

        public int DeadEnnemiInTurn;
        public int TurnId;
        public int TimeoutMin = 0;
        public int TimeoutMax = 0;
        public int Relaunch;

        public bool AutoTimeout = false;
        public bool IsFighterTurn = false;
        public bool IsFightStarted = false;
        public bool WaitForReady = false;

        public List<FightOptionEnum> Options = new List<FightOptionEnum>();
        
        public List<short> PlacementCells;
        public MonsterGroup followinggroup;
        public Stopwatch watch = new Stopwatch();
        #endregion

        #region Private Fields
        private Object clock = new Object();
        #endregion
        #endregion

        #region Properties
        public BFighter Fighter
        {
            get { return GetFighter(m_Account.CharacterBaseInformations.id); }
        }
        #endregion

        #region Constructeurs
        public BFight(AccountUC account)
        {
            m_Account = account;
            spells = new List<BSpell>();
        }

        public BFight(FightConfig myconf, List<BSpell> myspells, AccountUC account)
        {
            m_Account = account;
            m_Conf = myconf;
            m_Spells = myspells;
            winLoseDic = new Dictionary<string, int>();
            winLoseDic.Add("Gagné", 0);
            winLoseDic.Add("Perdu", 0);
            spells = new List<BSpell>();
            xpWon = new Dictionary<DateTime, int>();
            xpWon.Add(DateTime.Today, 0);
        }
        #endregion

        #region Methodes publiques
        public SpellInabilityReason CanLaunchSpell(int spellId)
        {
            //IItem weapon = Account.Game.Character.Inventory.Items.FirstOrDefault(i => (InventoryPositionEnum)i.Position == InventoryPositionEnum.Weapon);
            //Weapon weaponData = null;
            DataClass spellData = GameData.GetDataObject(D2oFileEnum.Spells, spellId);
            ArrayList ids = ((ArrayList)spellData.Fields["spellLevels"]);
            int level = m_Account.Spells.FirstOrDefault(Spell => Spell.Id == spellId).Level;
            int id = Convert.ToInt32(ids[level - 1]);
            DataClass spellLevelsData = GameData.GetDataObject(D2oFileEnum.SpellLevels, id);
            if (spellLevelsData == null)
                return SpellInabilityReason.Unknown;
            //if (spellId == 0 && weapon != null)
            //    weaponData = ObjectDataManager.Instance.Get<Weapon>(weapon.GID);

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
                CharacterCharacteristicsInformations stats = m_Account.CharacterStats;
                int total = stats.summonableCreaturesBoost.@base + stats.summonableCreaturesBoost.objectsAndMountBonus + stats.summonableCreaturesBoost.alignGiftBonus + stats.summonableCreaturesBoost.contextModif;
                if (this.GetInvokationNumber() >= total)
                    return SpellInabilityReason.TooManyInvocations;
            }
            //List<int> listOfStates = (List<int>)spellLevelsData.Fields["statesRequired"];
            //foreach (var state in listOfStates)
            //{
            //    if (!(DurationByEffect.ContainsKey(state)))
            //        return SpellInabilityReason.RequiredState;
            //}
            //listOfStates = (List<int>)spellLevelsData.Fields["statesForbidden"];
            //foreach (var state in listOfStates)
            //{
            //    if (DurationByEffect.ContainsKey(state))
            //        return SpellInabilityReason.ForbiddenState;
            //}
            return SpellInabilityReason.None;
        }

        public SpellInabilityReason CanLaunchSpell(int spellId, int characterCellId, int cellId)
        {
            //IItem weapon = Account.Game.Character.Inventory.Items.FirstOrDefault(i => (InventoryPositionEnum)i.Position == InventoryPositionEnum.Weapon);
            //Weapon weaponData = null;
            //SpellLevel spellLevelsData = (SpellLevel)m_Account.GameDataFileAccessor.GetObject("SpellLevels", spellId);
            DataClass spellData = GameData.GetDataObject(D2oFileEnum.Spells, spellId);
            ArrayList ids = (ArrayList)spellData.Fields["spellLevels"];
            int level = m_Account.Spells.FirstOrDefault(Spell => Spell.Id == spellId).Level;
            int id = Convert.ToInt32(ids[level - 1]);
            DataClass spellLevelsData = GameData.GetDataObject(D2oFileEnum.SpellLevels, id);
            if (spellLevelsData == null)
                return SpellInabilityReason.Unknown;

            //if (spellId == 0 && weapon != null)
            //    weaponData = ObjectDataManager.Instance.Get<Weapon>(weapon.GID);

            MapPoint characterPoint = new MapPoint(characterCellId);
            MapPoint targetPoint = new MapPoint(cellId);
            int distanceToTarget = characterPoint.DistanceToCell(targetPoint);
            int minRange = (spellId != 0) ? (int)spellLevelsData.Fields["minRange"] : 0; //weaponData.minRange;
            if ((spellId != 0 && (bool)spellLevelsData.Fields["castInDiagonal"]))// || (weaponData != null && weaponData.castInDiagonal))
                minRange = (minRange * 2);
            if (minRange < 0)
                minRange = 0;
            int maxRange = (spellId != 0) ? (int)((int)spellLevelsData.Fields["range"] + ((bool)spellLevelsData.Fields["rangeCanBeBoosted"] ? m_Account.CharacterStats.range.objectsAndMountBonus : 0)) : (int)spellLevelsData.Fields["range"];
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
                    if (!(IsFreeCell(point4.CellId)) || !(m_Account.Map.Data.IsLineOfSight(point4.CellId)))
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

        public bool PerformSpellsStack()
        {
            m_Account.Wait(200, 400);
            Relaunch--;              /* On baisse le relaunch */
            if (Relaunch == 0)       /* Si on l'a lancé le nombre de fois qu'il fallait, on passe au sort suivant */
                spells.RemoveAt(0);
            if (spells.Count() == 0)
            {                           /* Si il n'y a plus de sorts restants, on renvoie false pour terminer le tour. */
                PerformMove();
                return false;
            }
            BFighter target = null;
            if (spells[0].TargetName == "Nom du monstre") /* On récup la cible */
            {
                target = GetTarget(spells[0].Target);
            }
            else
            {
                target = GetTarget(spells[0].TargetName);
            }
           
            //if (spells[0].SpellId == 413)
            //{
            //    if (target.LifePoints / target.MaxLifePoints * 100 <= spells[0].TargetLife && GetFighters(true).Count() < 2)
            //    {
            //        m_Account.Log(new BotTextInformation("It's time to capture ! POKEBALL GOOOOOOOOOO"), 5);
            //        LaunchSpell(spells[0].SpellId, Fighter.CellId);
            //        spells[0].LastTurn = TurnId;
            //        PerformAutoTimeoutFight(1000);
            //    }
            //}
            if (target != null && target.LifePoints / target.MaxLifePoints * 100 <= spells[0].TargetLife)
            {
                //ComputeDamage(spells[0].SpellId, target);
                m_Account.Log(new BotTextInformation("Cible en vue à la cellule " + target.CellId + " !"), 5);
                if (m_Conf.Tactic == TacticEnum.Immobile)
                {
                    if (CanLaunchSpell(spells[0].SpellId) == SpellInabilityReason.None && CanLaunchSpell(spells[0].SpellId, Fighter.CellId, target.CellId) == SpellInabilityReason.None)
                    {
                        m_Account.Log(new BotTextInformation("CanLaunchSpell !"), 5);
                        LaunchSpell(spells[0].SpellId, target.CellId);
                        spells[0].LastTurn = TurnId;
                    }
                    else
                    {
                        Relaunch = 1;           /* Si on peut pas lancer ce sort, on passe au suivant en mettant le relaunch comme fini */
                        return PerformSpellsStack();
                    }
                }
                else if (CanUseSpell(spells[0], target))
                {
                    m_Account.Log(new BotTextInformation("CanUseSpell ! "), 5);
                    LaunchSpell(spells[0].SpellId, target.CellId);
                    spells[0].LastTurn = TurnId;
                }
                else
                {
                    Relaunch = 1;              /* Si on peut pas lancer ce sort, on passe au suivant en mettant le relaunch comme fini */
                    return PerformSpellsStack();
                }
            }
            else
            {
                Relaunch = 1;
                return PerformSpellsStack();
            }
                

            if (Relaunch == 0)                /* Si on a réussi a lancé le sort et que le relaunch n'a pas été mis, on le définit. */
                Relaunch = spells[0].Relaunch;
            return true;
        }

        public void PerformMove()
        {
           
                NearMonster = NearestMonster();

                if (NearMonster == null)
                {
                    EndTurn();
                    return;
                }

                // EndMove
                if (Fighter.MovementPoints > 0 && m_Conf.Tactic != TacticEnum.Immobile)
                {
                    int distance = new MapPoint(Fighter.CellId).DistanceToCell(new MapPoint(NearMonster.CellId));
                    if (m_Conf.Tactic == TacticEnum.Fuyard)
                    {
                        if (distance > m_Conf.FarCells)
                        {
                            MoveToCell(NearCell());
                        }
                        else
                        {
                            MoveToCell(FarCell());
                        }
                    }
                    else if (m_Conf.Tactic == TacticEnum.Barbare && !IsHandToHand())
                        MoveToCell(NearCell());

                    m_Account.Log(new BotTextInformation("EndMove"), 5);
                }

                EndTurn();
            
        }

        public void FightTurn()
        {
            this.IsFighterTurn = true;
            m_Account.Log(new BotTextInformation("FightTurn"), 5);
            spells = new List<BSpell>();
            try
            {
                if (GetFighters(true).Count > 0)
                {
                    // Spells
                    foreach (BSpell spell in m_Spells)
                    {
                        if (spell.Turns != TurnId & spell.Turns != -1)
                        {
                            continue;
                        }
                        else
                            spells.Add(spell);
                    }
                    spells = spells.OrderBy(o => o.Turns).ToList();
                    spells.Reverse();
                    PerformSpellsStack();
                }



                //    m_Account.Log(new BotTextInformation("Sort validé"), 5);
                //    //Relaunch
                //    //for (int i = 0; i <= spell.Relaunch - 1; i++)
                //    //{
                //    if (GetFighters(true).Count > 0)
                //    {
                //        //if (DeadEnnemiInTurn > 0)
                //        //{
                //        //    PerformAutoTimeoutFight(Convert.ToInt32((DeadEnnemiInTurn + 1.5) * 500));
                //        //    DeadEnnemiInTurn = 0;
                //        //}
                //        BFighter target = null;
                //        if (spell.TargetName == "Nom du monstre")
                //        {
                //            target = GetTarget(spell.Target);
                //        }
                //        else
                //        {
                //            target = GetTarget(spell.TargetName);
                //        }
                //        m_Account.Log(new BotTextInformation("Cible en vue à la cellule " + target.CellId + " !"), 5);
                //        //target = GetTarget(spell.Target);
                //        if (spell.SpellId == 413)
                //        {
                //            if (target.LifePoints / target.MaxLifePoints * 100 <= spell.TargetLife && GetFighters(true).Count() < 2)
                //            {
                //                m_Account.Log(new BotTextInformation("It's time to capture ! POKEBALL GOOOOOOOOOO"), 5);
                //                LaunchSpell(spell.SpellId, Fighter.CellId);
                //                spell.LastTurn = TurnId;
                //                PerformAutoTimeoutFight(1000);
                //                EndTurn();
                //                return;
                //            }
                //            else
                //                break;
                //        }
                //        if (Relaunch == -1)
                //            Relaunch = spell.Relaunch;
                //        if (target.LifePoints / target.MaxLifePoints * 100 <= spell.TargetLife)
                //        {
                //            if (m_Conf.Tactic == TacticEnum.Immobile)
                //            {
                //                if (CanLaunchSpell(spell.SpellId) == SpellInabilityReason.None && CanLaunchSpell(spell.SpellId, Fighter.CellId, target.CellId) == SpellInabilityReason.None)
                //                {
                //                    m_Account.Log(new BotTextInformation("CanLaunchSpell ! "), 5);
                //                    LaunchSpell(spell.SpellId, target.CellId);
                //                    spell.LastTurn = TurnId;
                //                }
                //                else
                //                {
                //                    break;
                //                }
                //            }
                //            else if (CanUseSpell(spell, target))
                //            {
                //                m_Account.Log(new BotTextInformation("CanUseSpell ! "), 5);
                //                LaunchSpell(spell.SpellId, target.CellId);
                //                spell.LastTurn = TurnId;
                //            }
                //            else
                //            {
                //                break;
                //            }

                //        }
                //    }
                //    //}
                //}
                
                    //else if (!IsHandToHand())
                    //{
                    //    Account.Game.Fight.MoveToCell(NearCell());
                    //}
                    //}
                    //EndTurn();
                //}
            }
            catch (Exception ex)
            {
                m_Account.Log(new ErrorTextInformation(ex.Message), 0);
            }
        }

        public int ComputeDamage(int spellId, BFighter target)
        {
            DataClass spellData = GameData.GetDataObject(D2oFileEnum.Spells, spellId);
            ArrayList ids = (ArrayList)spellData.Fields["spellLevels"];
            int level = m_Account.Spells.FirstOrDefault(Spell => Spell.Id == spellId).Level;
            int id = Convert.ToInt32(ids[level - 1]);
            DataClass spellLevelsData = GameData.GetDataObject(D2oFileEnum.SpellLevels, id);
            ArrayList effects = (ArrayList)spellLevelsData.Fields["effects"];
            int minDam = 0;
            int maxDam= 0;
            foreach (var effect in effects)
            {
                DataClass data = (DataClass)effect;
                minDam += (int)data.Fields["diceNum"];
                maxDam += (int)data.Fields["diceSide"];
                maxDam = minDam >= maxDam ? minDam : maxDam;
            }
            int baseDam = (maxDam + minDam) / 2;
           // int totalDam = baseDam + (baseDam * m_Account.CharacterStats.permanentDamagePercent.@base) + m_Account.CharacterStats.allDamagesBonus;
            // TODO check is this is puissance.
            // totaux - res fixes  - (totaux * %res/100)
            return 0;
        }

        public void EndTurn()
        {
            m_Account.Log(new BotTextInformation("ATTENTION ENTRER METHODE ENDTURN"), 4);
            GameFightTurnFinishMessage msg = new GameFightTurnFinishMessage();
            m_Account.SocketManager.Send(msg);
            IsFighterTurn = false;
        }

        public bool IsHandToHand(int cell = 0)
        {
            if (cell == 0)
                cell = Fighter.CellId;
            MapPoint characterPoint = new MapPoint(cell);
            MapPoint targetPoint = new MapPoint(NearestMonster().CellId);

            if (characterPoint.DistanceToCell(targetPoint) <= 1)
                return true;

            return false;
        }

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
                //if (spell.IsHandToHand && !IsHandToHand())
                //{
                //    MapPoint characterPoint = new MapPoint(Account.Game.Fight.Fighter.CellId);
                //    int tempDistance = characterPoint.DistanceToCell(new MapPoint(NearestMonster().CellId));
                //    if (tempDistance - 1 > Account.Game.Fight.Fighter.MovementPoints)
                //    {
                //        return false;
                //    }
                //}
                //else if (spell.IsHandToHand && IsHandToHand())
                //{
                //    Account.Log("No need to move maggle", LogType.DEBUG);
                //    return true;
                //}
                //else if (!spell.IsHandToHand)
                //{
                m_Account.Log(new BotTextInformation("No need to move maggle"),5);
                return true;
                //}
            }

            // Try with move
            int moveCell = -1;
            int distance = -1;
            foreach (int cell in GetReachableCells())
            {
                //if (spell.IsHandToHand)
                //{
                //    MapPoint characterPoint = new MapPoint(cell);
                //    int tempDistance = characterPoint.DistanceToCell(new MapPoint(target.CellId));

                //    if (IsHandToHand(cell) && (tempDistance < distance || distance == -1))
                //    {
                //        distance = tempDistance;
                //        moveCell = cell;
                //    }
                //}
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
                m_Account.Log(new BotTextInformation("CanUseSpellWithMove!"),5);
                return true;
            }

            // Can't use
            m_Account.Log(new ErrorTextInformation("CantUseSpell"),5);
            return false;
        }

        public BFighter GetTarget(TeamEnum teamEnum__1)
        {
            BFighter target = null;
            switch (teamEnum__1)
            {
                case TeamEnum.Ally:
                    target = null;
                    break;
                case TeamEnum.EmptyCase:
                    target = new BFighter(0,NearCell(),0,null,false,0,0,0,0,0);
                    break;
                case TeamEnum.Enemy:
                    target = NearestMonster();
                    break;
                case TeamEnum.Self:
                    target = Fighter;
                    break;
            }
            return target;
        }

        public BFighter GetTarget(string monstername)
        {
            foreach (BFighter fighter in Fighters)
            {
                if (BlueSheep.Common.Data.I18N.GetText(fighter.CreatureGenericId) == monstername)
                    return fighter;
            }
            return null;
        }

        public BFighter GetFighter(int fighterId)
        {
            return Fighters.FirstOrDefault(f => f.Id == fighterId);
        }

        public bool SearchFight()
        {
            //m_Account.Path.Stop = true;
            int minNumber = (int)m_Account.nudMinMonstersNumber.Value;
            int maxNumber = (int)m_Account.nudMaxMonstersNumber.Value;
            int minLevel = (int)m_Account.nudMinMonstersLevel.Value;
            int maxLevel = (int)m_Account.nudMaxMonstersLevel.Value;
            lock(this.clock)
            {
                foreach (MonsterGroup monsters in m_Account.Map.List)
                {

                    if (monsters.monstersCount < minNumber || monsters.monstersCount > maxNumber)
                    {
                        continue;
                    }

                    if (monsters.monstersLevel < minLevel || monsters.monstersLevel > maxLevel)
                    {
                        continue;
                    }
                    if (m_Account.VerifGroup(monsters.NameList()) == false)
                        continue;

                    this.followinggroup = monsters;
                    if (m_Account.Map.MoveToCell(monsters.m_cellId))
                    {
                        m_Account.SetStatus(Status.Moving);
                        //m_Account.Path.Stop = true;
                        m_Account.Log(new ActionTextInformation(string.Format("Lancement d'un combat contre {0} monstres de niveau {1} ({2})", monsters.monstersCount, monsters.monstersLevel, monsters.monstersName(true))),1);
                        return true;
                    }

                }
            }
            return false;
}

        public bool IsFollowingGroup()
        {
            if (this.followinggroup != null)
                return true;
            else
            {
                return false;
            }
        }

        public void LaunchFight(int id)
        {
            /* Ajout temporaire (?) pour résoudre le bug du lancement de combat qui ne se faisait pas
               parce que le groupe de mob bougeait */
            MovementPath path = (new Pathfinder(m_Account.Map.Data, m_Account.Map)).FindPath(m_Account.Map.Character.CellId, followinggroup.m_cellId);
            if (!(path == null))
            {
                using (BigEndianWriter writer = new BigEndianWriter())
                {
                    GameRolePlayAttackMonsterRequestMessage msg = new GameRolePlayAttackMonsterRequestMessage(id);
                    m_Account.SocketManager.Send(msg);
                    m_Account.Log(new ActionTextInformation("Launch Fight !"), 1);
                }
            }
            else
            {
                m_Account.Log(new ActionTextInformation("Search Fight: le groupe de mob n'est pas au même emplacement.."), 1);
                this.SearchFight();
            }
        }

        public void PlaceCharacter()
        {
            m_Account.Log(new BotTextInformation("Placement du personnage"),5);
            try
            {
                int distance = -1;
                int cell = -1;
                Dictionary<int, int> cells = new Dictionary<int, int>();
                foreach (uint tempCell in PlacementCells)
                {
                    int tempDistance = 0;
                    MapPoint cellPoint = new MapPoint(Convert.ToInt32(tempCell));
                    foreach (BFighter fighter in Fighters)
                    {
                        MapPoint fighterPoint = new MapPoint(fighter.CellId);
                        tempDistance += cellPoint.DistanceToCell(fighterPoint);
                    }

                    if (m_Conf.Placement == PlacementEnum.Eloigné)
                    {
                        if (distance < tempDistance || distance == -1)
                        {
                            distance = tempDistance;
                            cell = Convert.ToInt32(tempCell);
                        }
                    }
                    else if (m_Conf.Placement == PlacementEnum.Rapproché)
                    {
                        if (distance > tempDistance || distance == -1 || tempDistance == 1)
                        {
                            distance = tempDistance;
                            cell = Convert.ToInt32(tempCell);
                        }
                    }
                }


                if (cell != -1)
                {
                        GameFightPlacementPositionRequestMessage msg = new GameFightPlacementPositionRequestMessage((short)cell);
                        m_Account.SocketManager.Send(msg);
                }
            }
            catch (Exception ex)
            {
                m_Account.Log(new ErrorTextInformation(ex.Message),0);
            }
        }

        public void PulseRegen()
        {
            if ((m_Account.CharacterStats.lifePoints / m_Account.CharacterStats.maxLifePoints) < m_Account.RegenChoice.Value)
            {
                m_Account.SetStatus(Status.Regenerating);
                int maxLife = Convert.ToInt32(m_Account.CharacterStats.maxLifePoints);
                int life = Convert.ToInt32(m_Account.CharacterStats.lifePoints);
                int time = Convert.ToInt32(Math.Round(Convert.ToDecimal(maxLife - life) / 2));
                using (BigEndianWriter writer = new BigEndianWriter())
                {
                    EmotePlayRequestMessage msg2 = new EmotePlayRequestMessage(1);
                    msg2.Serialize(writer);
                    MessagePackaging pack = new MessagePackaging(writer);
                    pack.Pack((int)msg2.ProtocolID);
                    m_Account.SocketManager.Send(pack.Writer.Content);
                }
                m_Account.Log(new GeneralTextInformation(String.Format("Régénération pendant {0} secondes.", time)), 2);
                m_Account.Wait(time * 1000, (time + 1) * 1000);
            }
            // TODO : Continue path after this.
        }

        public void LockFight()
        {
            GameFightOptionToggleMessage msg = new GameFightOptionToggleMessage(2);
            m_Account.SocketManager.Send(msg);
            m_Account.Log(new ActionTextInformation("Fermeture du combat"), 4);
        }

        public void PerformAutoTimeoutFight(int originalTime)
        {
            m_Account.Wait(Convert.ToInt32(originalTime * m_Account.NUDTimeoutFight.Value), Convert.ToInt32(originalTime * m_Account.NUDTimeoutFight.Value));
        }
        #endregion

        #region Methodes Privées

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

        private bool IsFreeCell(int cellId)
        {
            return !Fighters.Any(f => f != null && f.CellId == cellId);
        }

        private void KickPlayer(int id)
        {
            GameContextKickMessage msg = new GameContextKickMessage(id);
            m_Account.SocketManager.Send(msg);
        }

        private void LaunchSpell(int spellId, int cellId)
        {

            //foreach (BFighter fighter in Fighters)
            //{
            //    if (fighter.CellId == cellId)
            //    {
            //        GameActionFightCastOnTargetRequestMessage msg = new GameActionFightCastOnTargetRequestMessage((short)spellId, fighter.Id);
            //        m_Account.SocketManager.Send(msg);
            //        m_Account.Log(new ActionTextInformation("Lancement d'un sort en " + cellId), 5);
            //        return;
            //    }
            //}
            using (BigEndianWriter writer = new BigEndianWriter())
            {
                GameActionFightCastRequestMessage msg = new GameActionFightCastRequestMessage((short)spellId, (short)cellId);
                msg.Serialize(writer);
                writer.Content = m_Account.HumanCheck.hash_function(writer.Content);
                MessagePackaging pack = new MessagePackaging(writer);
                pack.Pack((int)msg.ProtocolID);
                m_Account.SocketManager.Send(pack.Writer.Content);
                m_Account.Log(new ActionTextInformation("Lancement d'un sort en " + cellId), 5);
                if (m_Account.DebugMode.Checked)
                    m_Account.Log(new BotTextInformation("[SND] 1005 (GameActionFightCastRequestMessage)"), 0);
            }
        }

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
                        direction += 2;
                        if (direction > 7)
                        {
                            if (num == -1)
                                return false;
                            cellId = num;
                            break;
                        }
                    }
                }
                SimplePathfinder pathfinder = new SimplePathfinder((BlueSheep.Data.D2p.Map)m_Account.Map.Data);
                pathfinder.SetFight(Fighters, Fighter.MovementPoints);
                MovementPath path = pathfinder.FindPath(Fighter.CellId, cellId);
                if (path != null)
                {
                    List<UInt32> serverMovement = MapMovementAdapter.GetServerMovement(path);
                    //Account.Network.SendToServer(new GameMapMovementRequestMessage(serverMovement.ToList().Select<uint, short>(ui => (short)ui).ToArray(), Account.Game.Map.Id));
                    using (BigEndianWriter writer = new BigEndianWriter())
                    {
                        GameMapMovementRequestMessage msg = new GameMapMovementRequestMessage(serverMovement.ToList().Select<uint, short>(ui => (short)ui).ToArray(), m_Account.Map.Id);
                        msg.Serialize(writer);
                        writer.Content = m_Account.HumanCheck.hash_function(writer.Content);
                        MessagePackaging pack = new MessagePackaging(writer);
                        pack.Pack((int)msg.ProtocolID);
                        m_Account.SocketManager.Send(pack.Writer.Content);
                        if (m_Account.DebugMode.Checked)
                            m_Account.Log(new BotTextInformation("[SND] 950 (GameMapMovementRequestMessage)"), 0);
                    }
                    return true;
                }
            }
            return false;
        }

        private List<BFighter> GetFighters(bool ennemies)
        {
            // TODO : Rework with LINQ
            List<BFighter> fighters = new List<BFighter>();
            if (Fighter != null)
            {
                uint TeamIdAllies = Fighter.TeamId;

                foreach (var fighter in Fighters)
                {
                    if (fighter.IsAlive & fighter.Id != m_Account.CharacterBaseInformations.id)
                    {
                        if ((ennemies == true && TeamIdAllies != fighter.TeamId) || (!ennemies && TeamIdAllies == fighter.TeamId))
                            fighters.Add(fighter);
                    }
                }
            }
            return fighters;
        }

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

        private bool IsCellWalkable(int cellId)
        {
            BlueSheep.Data.D2p.Map MapData = (BlueSheep.Data.D2p.Map)m_Account.Map.Data;
            if (m_Account.Map.Data.IsWalkable(cellId))
            {
                var selectedFighter = Fighters.FirstOrDefault((f) => f.CellId == cellId || MapData.Cells[cellId].NonWalkableDuringFight());
                if (selectedFighter != null)
                    return false;
                else
                    return true;
            }
            return false;
        }

        private BFighter NearestMonster()
        {
            MapPoint CharacterPoint = new MapPoint(this.Fighter.CellId);
            BFighter Fighterr = null;
            int SavDistance = -1;
            foreach (BFighter TestFighter in Fighters)
            {
                if (TestFighter.TeamId == Fighter.TeamId || TestFighter.IsAlive == false)
                    continue;
                MapPoint TestFighterPoint = new MapPoint(TestFighter.CellId);
                int dist = new SimplePathfinder(m_Account.Map.Data).FindPath(CharacterPoint.CellId, TestFighterPoint.CellId).Cells.Count();
                dist += CharacterPoint.DistanceToCell(TestFighterPoint);
                if (((dist < SavDistance) || (SavDistance == -1)) && TestFighter != this.Fighter)
                {
                    SavDistance = dist;
                    Fighterr = TestFighter;
                }
            }
            if (Fighterr == null)
            {
                return null;
            }
            return Fighterr;
        }

        private int FarCell()
        {
            List<int> ReachableCells = GetReachableCells();
            int CellId = -1;
            int SavDistance = -1;
            foreach (int ReachableCell in ReachableCells)
            {
                MapPoint ReachableCellPoint = new MapPoint(ReachableCell);
                int Distance = 0;
                Distance = (Distance + ReachableCellPoint.DistanceToCell(new MapPoint(NearMonster.CellId)));
                if (((SavDistance == -1) || (Distance > SavDistance)))
                {
                    CellId = ReachableCell;
                    SavDistance = Distance;
                }
            }
            return CellId;
        }

        private int NearCell()
        {
            List<int> ReachableCells = GetReachableCells();
            int CellId = -1;
            int SavDistance = -1;
            foreach (int ReachableCell in ReachableCells)
            {
                MapPoint ReachableCellPoint = new MapPoint(ReachableCell);
                int Distance = 0;
                Distance = (Distance + ReachableCellPoint.DistanceToCell(new MapPoint(NearMonster.CellId)));
                if (((SavDistance == -1) || (Distance < SavDistance)))
                {
                    CellId = ReachableCell;
                    SavDistance = Distance;
                }
            }
            return CellId;
        }
        #endregion
    }
}
