using BlueSheep.Common.Data.D2o;
using BlueSheep.Common.Protocol.Messages;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.Types;
using BlueSheep.Core.Inventory;
using BlueSheep.Data.Pathfinding;
using BlueSheep.Data.Pathfinding.Positions;
using BlueSheep.Engine.Enums;
using BlueSheep.Interface;
using BlueSheep.Interface.Text;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace BlueSheep.Core.Fight
{
    public class FightData
    {
        #region Fields
        #region Dictionary
        public  Dictionary<DateTime, int> xpWon;
        public  Dictionary<string, int> winLoseDic;
        public Dictionary<int, int> DurationByEffect = new Dictionary<int, int>();
        public Dictionary<int, int> LastTurnLaunchBySpell = new Dictionary<int, int>();
        public Dictionary<int, int> TotalLaunchBySpell = new Dictionary<int, int>();
        public Dictionary<int, Dictionary<int, int>> TotalLaunchByCellBySpell = new Dictionary<int, Dictionary<int, int>>();
        private Dictionary<int, List<BFighter>> m_Summons = new Dictionary<int, List<BFighter>>();
        #endregion

        #region Public Fields
        public AccountUC m_Account;
        public List<BFighter> Fighters = new List<BFighter>();

        public List<BFighter> DeadEnnemies = new List<BFighter>();
        public int TurnId;

        public bool IsFighterTurn = false;
        public bool IsFightStarted = false;
        public bool WaitForReady = false;
        public bool IsDead = false;

        public List<FightOptionEnum> Options = new List<FightOptionEnum>();
        public Stopwatch watch = new Stopwatch();
        public MonsterGroup followingGroup;
        private Dictionary<string, int> boss;
        #endregion
        #endregion

        #region Properties
        public BFighter Fighter
        {
            get { return GetFighter(m_Account.CharacterBaseInformations.id); }
        }

        public int MonsterNumber
        {
            get { return Fighters.FindAll(f => f.TeamId != Fighter.TeamId).ToList().Count; }
        }

        public bool IsFollowingGroup
        {
            get
            {
                if (this.followingGroup != null)
                    return true;
                else
                    return false;
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Constructeur (override)
        /// </summary>
        /// <param name="account">Compte associé</param>
        public FightData(AccountUC Account)
        {
            m_Account = Account;
            xpWon = new Dictionary<DateTime, int>();
            winLoseDic = new Dictionary<string, int>();
            winLoseDic.Add("Gagné", 0);
            winLoseDic.Add("Perdu", 0);
            xpWon.Add(DateTime.Today, 0);
            DataClass[] data = GameData.GetDataObjects(D2oFileEnum.Monsters);
            List<DataClass> b = data.ToList().FindAll(e => ((bool)e.Fields["isBoss"]) == true).ToList();
            boss = new Dictionary<string, int>();
            foreach (DataClass d in b)
            {
                boss.Add(BlueSheep.Common.Data.I18N.GetText((int)d.Fields["nameId"]), (int)d.Fields["id"]);
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Returns if we are currently hand to hand with the nearest monster or not.
        /// </summary>
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

        /// <summary>
        /// Check if the target is targetable by the specified spell (with and without moving).
        /// </summary>
        /// <param name="spell">Spell to launch</param>
        /// <param name="target">Target</param>
        /// <returns>The cellId we need to move to. -1 if we can't use. 0 if we don't need to move.</returns>
        public int CanUseSpell(BSpell spell, BFighter target)
        {
            if (CanLaunchSpell(spell.SpellId) != SpellInabilityReason.None)
            {
                return -1;
            }

            // Use without move
            if (CanLaunchSpellOn(spell.SpellId, Fighter.CellId, target.CellId) == SpellInabilityReason.None)
            {
                return 0;
            }

            // Try with move
            int moveCell = -1;
            int distance = -1;
            foreach (int cell in GetReachableCells())
            {
                if (CanLaunchSpellOn(spell.SpellId, cell, target.CellId, true) == SpellInabilityReason.None)
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
            return moveCell;
        }

        /// <summary>
        /// Add the summoner and its summon.
        /// </summary>
        public void AddSummon(int sourceId, BFighter summon)
        {
            Fighters.Add(summon);
            List<BFighter> summoned = new List<BFighter>();
            if (m_Summons.ContainsKey(sourceId))
            {
                m_Summons.TryGetValue(sourceId, out summoned);
                summoned.Add(summon);
                m_Summons.Remove(sourceId);
                m_Summons.Add(sourceId, summoned);
            }
            else
            {
                summoned.Add(summon);
                m_Summons.Add(sourceId, summoned);
            }
        }

        /// <summary>
        /// Get the summoner with the biggest number of summons.
        /// </summary>
        public BFighter GetSummoner()
        {
            Tuple<int,int> temp = new Tuple<int,int>(0, -1);
            foreach (KeyValuePair<int, List<BFighter>> pair in m_Summons)
            {
                if (pair.Value.Count > temp.Item2)
                {
                    temp = new Tuple<int, int>(pair.Key, pair.Value.Count);
                }
            }
            if (temp.Item1 != 0)
                return GetFighter(temp.Item1);
            else
                return null;
        }

        /// <summary>
        /// Removes the fighter from the fighters and add it to the dead ennemies' list.
        /// </summary>
        public void SetFighterDeath(int id)
        {
            BFighter fighter = GetFighter(id);
            DeadEnnemies.Add(fighter);
            if (!IsDead && fighter.Id == Fighter.Id)
            {
                m_Account.Log(new ErrorTextInformation("Personnage mort :'("), 0);
                IsDead = true;
            }
            else if (fighter.CreatureGenericId != 0)
            {
                m_Account.Log(new ActionTextInformation(fighter.Name + " est mort !"), 5);
            }
            Fighters.Remove(fighter);          
        }

        /// <summary>
        /// Set the specified effect.
        /// </summary>
        public void SetEffect(AbstractFightDispellableEffect effect, int actionId = -1)
        {
            if (effect is FightTemporaryBoostStateEffect)
            {
                FightTemporaryBoostStateEffect m_effect = (FightTemporaryBoostStateEffect)effect;
                if (!IsDead && m_effect.targetId == Fighter.Id)
                {
                    if (DurationByEffect.ContainsKey(m_effect.stateId))
                        DurationByEffect.Remove(m_effect.stateId);
                    DurationByEffect.Add(m_effect.stateId, effect.turnDuration);
                }
            }
            else if (effect is FightTemporaryBoostEffect)
            {
                FightTemporaryBoostEffect m_effect = (FightTemporaryBoostEffect)effect;
                if (actionId == 168)
                    Fighter.ActionPoints -= m_effect.delta;
                else if (actionId == 169)
                    Fighter.MovementPoints -= m_effect.delta;
                else if (!IsDead && actionId == 116 && m_effect.targetId == Fighter.Id)
                    m_Account.CharacterStats.range.contextModif -= m_effect.delta;

            }
        }

        /// <summary>
        /// Affect the specified variation on the specified target.
        /// </summary>
        public void SetPointsVariation(int targetId, int actionId, int delta)
        {
            BFighter fighter = GetFighter(targetId);
            if (fighter != null)
            {
                switch (actionId)
                {
                    case 101:
                    case 102:
                    case 120:
                        fighter.ActionPoints += delta;
                        break;
                    case 78:
                    case 127:
                    case 129:
                        fighter.MovementPoints += delta;
                        break;
                }
            }
        }

        /// <summary>
        /// Update the life points of the specified fighter.
        /// </summary>
        public void UpdateFighterLifePoints(int id, int delta)
        {
            BFighter fighter = GetFighter(id);
            if (fighter != null)
            {
                fighter.LifePoints += delta;
                if (fighter.Id == Fighter.Id)
                {
                    m_Account.ModifBar(2, m_Account.FightData.Fighter.MaxLifePoints, m_Account.FightData.Fighter.LifePoints, "Vitalité");
                }
                m_Account.Log(new ActionTextInformation(fighter.Name + ": " + delta + "PV."), 5);
            }
            
        }

        /// <summary>
        /// Set a spell casted by the player.
        /// </summary>
        public void SetSpellCasted(int id, int spellId, int destinationCellId)
        {
            BFighter fighter = GetFighter(id);
            if (fighter != null && fighter.Id == Fighter.Id)
            {
                int spellLevel = -1;
                BlueSheep.Common.Types.Spell spell = m_Account.Spells.FirstOrDefault(s => s.Id == spellId);
                if (spell != null)
                    spellLevel = spell.Level;
                if (spellLevel != -1)
                {
                    DataClass spellData = GameData.GetDataObject(D2oFileEnum.Spells, spellId);
                    if (spellData != null)
                    {
                        uint spellLevelId = (uint)((ArrayList)spellData.Fields["spellLevels"])[spellLevel - 1];
                        DataClass spellLevelData = GameData.GetDataObject(D2oFileEnum.SpellLevels, (int)spellLevelId);
                        if (spellLevelData != null)
                        {
                            if ((int)spellLevelData.Fields["minCastInterval"] > 0 && !(LastTurnLaunchBySpell.ContainsKey(spellId)))
                                LastTurnLaunchBySpell.Add(spellId, (int)spellLevelData.Fields["minCastInterval"]);
                            if (TotalLaunchBySpell.ContainsKey(spellId)) //Si on a déjà utilisé ce sort ce tour
                                TotalLaunchBySpell[spellId] += 1;
                            else
                                TotalLaunchBySpell.Add(spellId, 1);
                            if (TotalLaunchByCellBySpell.ContainsKey(spellId)) //Si on a déjà utilisé ce sort ce tour
                            {
                                if (TotalLaunchByCellBySpell[spellId].ContainsKey(destinationCellId)) //Si on a déjà utilisé ce sort sur cette case
                                    TotalLaunchByCellBySpell[spellId][destinationCellId] += 1;
                                else
                                    TotalLaunchByCellBySpell[spellId].Add(destinationCellId, 1);
                            }
                            else
                            {
                                Dictionary<int, int> tempdico = new Dictionary<int, int>();
                                tempdico.Add(destinationCellId, 1);
                               TotalLaunchByCellBySpell.Add(spellId, tempdico);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Update the cellId of the specified fighter.
        /// </summary>
        public void UpdateFighterCell(int id, int cell)
        {
            BFighter fighter = GetFighter(id);
            if (fighter != null)
            {
                fighter.CellId = cell;
            }
        }

        /// <summary>
        /// Update state of a specified option.
        /// </summary>
        public void SetOption(bool state, int id)
        {
            if (!state && Options.Contains((FightOptionEnum)id))
                Options.Remove((FightOptionEnum)id);
            if (state && !Options.Contains((FightOptionEnum)id))
                Options.Add((FightOptionEnum)id);
        }

        /// <summary>
        /// Add a fighter to the fighter's list.
        /// </summary>
        public void AddFighter(GameFightFighterInformations informations)
        {
            if (informations is GameFightMonsterInformations)
            {
                GameFightMonsterInformations infos = (GameFightMonsterInformations)informations;
                Fighters.Add(new BFighter(informations.contextualId, informations.disposition.cellId, informations.stats.actionPoints, informations.stats, informations.alive, informations.stats.lifePoints, informations.stats.maxLifePoints, informations.stats.movementPoints, (uint)informations.teamId, infos.creatureGenericId));
            }
            else
            {
                Fighters.Add(new BFighter(informations.contextualId, informations.disposition.cellId, informations.stats.actionPoints, informations.stats, informations.alive, informations.stats.lifePoints, informations.stats.maxLifePoints, informations.stats.movementPoints, (uint)informations.teamId, 0));
            }
            if (Fighter != null)
                Fighter.Name = m_Account.CharacterBaseInformations.name;
        }

        /// <summary>
        /// Set the fight as started.
        /// </summary>
        public void FightStart()
        {
            WaitForReady = false;
            IsFightStarted = true;
            m_Account.Log(new ActionTextInformation("Début du combat"), 2);
            watch.Restart();
        }

        /// <summary>
        /// Set the fight as stopped.
        /// </summary>
        public void FightStop()
        {
            watch.Stop();
            WaitForReady = false;
            IsFighterTurn = false;
            IsFightStarted = false;
            IsDead = false;
            m_Account.Log(new ActionTextInformation("Combat fini ! (" + watch.Elapsed.Minutes + " min, " + watch.Elapsed.Seconds + " sec)"), 0);
            watch.Reset();
            m_Account.SetStatus(Status.Busy);
            Reset();
            PerformAutoTimeoutFight(2000);
            if (m_Account.WithItemSetBox.Checked == true)
            {
                sbyte id = (sbyte)m_Account.PresetEndUpD.Value;
                InventoryPresetUseMessage msg2 = new InventoryPresetUseMessage((sbyte)(id - 1));
                m_Account.SocketManager.Send(msg2);
                m_Account.Log(new ActionTextInformation("Equipement rapide numero " + Convert.ToString(id)), 5);
            }
            PulseRegen();
        }

        /// <summary>
        /// Set turn as started.
        /// </summary>
        public void TurnStarted(int id = 0)
        {
            if (!IsFightStarted)
                IsFightStarted = true;
            //if (id == m_Account.CharacterBaseInformations.id)
            //{
            //    IsFighterTurn = true;
            //}
            //else
            //    IsFighterTurn = false;
            IsFighterTurn = true;
        }

        /// <summary>
        /// Set turn as ended.
        /// </summary>
        public void TurnEnded(int id)
        {
            if (id == m_Account.CharacterBaseInformations.id)
            {
                int num4 = 0;
                List<int> list = new List<int>();
                IsFighterTurn = false;
                TotalLaunchBySpell.Clear(); //Nettoyage des variables de vérification de lancement d'un sort
                TotalLaunchByCellBySpell.Clear(); //Nettoyage des variables de vérification de lancement d'un sort
                for (int i = 0; i < DurationByEffect.Keys.Count; i++)
                {
                    Dictionary<int, int> durationPerEffect = DurationByEffect;
                    num4 = Enumerable.ElementAtOrDefault<int>(DurationByEffect.Keys, i);
                    durationPerEffect[num4] = (durationPerEffect[num4] - 1);
                    if (DurationByEffect[Enumerable.ElementAtOrDefault<int>(DurationByEffect.Keys, i)] <= 0)
                        list.Add(Enumerable.ElementAtOrDefault<int>(DurationByEffect.Keys, i));
                }
                while (list.Count > 0)
                {
                    DurationByEffect.Remove(list[0]);
                    list.RemoveAt(0);
                }
                for (int i = 0; i < LastTurnLaunchBySpell.Keys.Count; i++)
                {
                    Dictionary<int, int> dictionary = LastTurnLaunchBySpell;
                    num4 = Enumerable.ElementAtOrDefault<int>(LastTurnLaunchBySpell.Keys, i);
                    dictionary[num4] = (dictionary[num4] - 1);
                    if (LastTurnLaunchBySpell[Enumerable.ElementAtOrDefault<int>(LastTurnLaunchBySpell.Keys, i)] <= 0)
                        list.Add(Enumerable.ElementAtOrDefault<int>(LastTurnLaunchBySpell.Keys, i));
                }
                while (list.Count > 0)
                {
                    LastTurnLaunchBySpell.Remove(list[0]);
                    list.RemoveAt(0);
                }
                m_Account.Log(new ActionTextInformation("Fin du tour"), 5);
            }

        }

        /// <summary>
        /// Update the turnId.
        /// </summary>
        public void UpdateTurn(int roundNumber)
        {
            TurnId = roundNumber;
        }

        /// <summary>
        /// Clear all the fields.
        /// </summary>
        public void Reset(bool isFightStarted = false, bool canSayReady = false)
        {
            m_Summons.Clear();
            Fighters.Clear();
            DeadEnnemies.Clear();
            Options.Clear();
            TotalLaunchBySpell.Clear();
            LastTurnLaunchBySpell.Clear();
            TotalLaunchByCellBySpell.Clear();
            DurationByEffect.Clear();
            IsFightStarted = isFightStarted;
            WaitForReady = (!isFightStarted && canSayReady);
            followingGroup = null;
        }

        /// <summary>
        /// Perform the auto-timeout.
        /// </summary>
        public void PerformAutoTimeoutFight(int originalTime)
        {
            m_Account.Wait(Convert.ToInt32(originalTime * m_Account.NUDTimeoutFight.Value), Convert.ToInt32(originalTime * m_Account.NUDTimeoutFight.Value));
        }

        /// <summary>
        /// Returns the nearest monster from our player.
        /// </summary>
        public BFighter NearestMonster()
        {
            BFighter Fighterr = null;
            int SavDistance = -1;
            foreach (BFighter TestFighter in Fighters)
            {
                if (TestFighter.TeamId == Fighter.TeamId || TestFighter.IsAlive == false)
                    continue;
                int dist = DistanceFrom(TestFighter);
                if (((dist < SavDistance) || (SavDistance == -1)) && TestFighter != Fighter)
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

        /// <summary>
        /// Returns the weakest monster in the fight.
        /// </summary>
        public BFighter WeakestMonster()
        {
            Tuple<int, BFighter> temp = new Tuple<int, BFighter>(0, null);
            foreach (BFighter f in Fighters)
            {
                if (temp.Item1 > f.LifePoints && f.TeamId != Fighter.TeamId)
                    temp = new Tuple<int, BFighter>(f.LifePoints, f);
            }
            return temp.Item2;
        }

        /// <summary>
        /// Returns the nearest monster from our player, in the specified list.
        /// </summary>
        public BFighter NearestMonster(List<BFighter> LFighters)
        {
            MapPoint CharacterPoint = new MapPoint(this.Fighter.CellId);
            BFighter Fighterr = null;
            int SavDistance = -1;
            foreach (BFighter TestFighter in LFighters)
            {
                if (TestFighter.TeamId == Fighter.TeamId || TestFighter.IsAlive == false)
                    continue;
                MapPoint TestFighterPoint = new MapPoint(TestFighter.CellId);
                int dist = new SimplePathfinder(m_Account.MapData).FindPath(CharacterPoint.CellId, TestFighterPoint.CellId).Cells.Count();
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

        /// <summary>
        /// Returns the nearest ally from our player.
        /// </summary>
        public BFighter NearestAlly()
        {
            BFighter Fighterr = null;
            int SavDistance = -1;
            foreach (BFighter TestFighter in Fighters)
            {
                if (TestFighter.TeamId != Fighter.TeamId || TestFighter.IsAlive == false)
                    continue;
                int dist = DistanceFrom(TestFighter);
                if (((dist < SavDistance) || (SavDistance == -1)) && TestFighter != Fighter)
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

        /// <summary>
        /// Returns the weakest ally in the fight.
        /// </summary>
        public BFighter WeakestAlly()
        {
            Tuple<int, BFighter> temp = new Tuple<int, BFighter>(0, null);
            foreach (BFighter f in Fighters)
            {
                if (temp.Item1 > f.LifePoints && f.TeamId == Fighter.TeamId)
                    temp = new Tuple<int, BFighter>(f.LifePoints, f);
            }
            return temp.Item2;
        }


        /// <summary>
        /// Returns the distance between our player and the specified fighter. Default is the nearest monster.
        /// </summary>
        public int DistanceFrom(BFighter fighter = null)
        {
            if (fighter == null)
                fighter = NearestMonster();
            MapPoint CharacterPoint = new MapPoint(Fighter.CellId);
            MapPoint TestFighterPoint = new MapPoint(fighter.CellId);
            int dist = new SimplePathfinder(m_Account.MapData).FindPath(fighter.CellId, TestFighterPoint.CellId).Cells.Count();
            dist += CharacterPoint.DistanceToCell(TestFighterPoint);
            return dist;
        }

        /// <summary>
        /// Returns the furthest cell from the specified fighter. Default is the nearest monster.
        /// </summary>
        public int FurthestCellFrom(BFighter fighter = null)
        {
            if (fighter == null)
                fighter = NearestMonster();
            List<int> ReachableCells = GetReachableCells();
            int CellId = -1;
            int SavDistance = -1;
            foreach (int ReachableCell in ReachableCells)
            {
                MapPoint ReachableCellPoint = new MapPoint(ReachableCell);
                int Distance = 0;
                Distance = (Distance + ReachableCellPoint.DistanceToCell(new MapPoint(fighter.CellId)));
                if (((SavDistance == -1) || (Distance > SavDistance)))
                {
                    CellId = ReachableCell;
                    SavDistance = Distance;
                }
            }
            return CellId;
        }

        /// <summary>
        /// Returns the nearest cell from the specified fighter. Default is the nearest monster.
        /// </summary>
        public int NearestCellFrom(BFighter fighter = null)
        {
            if (fighter == null)
                fighter = NearestMonster();
            List<int> ReachableCells = GetReachableCells();
            int CellId = -1;
            int SavDistance = -1;
            foreach (int ReachableCell in ReachableCells)
            {
                MapPoint ReachableCellPoint = new MapPoint(ReachableCell);
                int Distance = 0;
                Distance = (Distance + ReachableCellPoint.DistanceToCell(new MapPoint(fighter.CellId)));
                if (((SavDistance == -1) || (Distance < SavDistance)))
                {
                    CellId = ReachableCell;
                    SavDistance = Distance;
                }
            }
            return CellId;
        }

        /// <summary>
        /// Returns if a boss is in the fight or not.
        /// </summary>
        public bool BossInFight()
        {
            foreach (BFighter f in Fighters)
            {
                if (boss.ContainsKey(f.Name) || boss.ContainsValue(f.CreatureGenericId))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Returns if the specified cellId is walkable or not.
        /// </summary>
        public bool IsCellWalkable(int cellId)
        {
            if (m_Account.MapData.Data.IsWalkable(cellId))
            {
                var selectedFighter = Fighters.FirstOrDefault((f) => f.CellId == cellId || m_Account.MapData.Data.Cells[cellId].NonWalkableDuringFight());
                if (selectedFighter != null)
                    return false;
                else
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Clear the fighters list before synchronization with server.
        /// </summary>
        public void ClearFighters()
        {
            Fighters.Clear();
        }

        #endregion

        #region Private Methods
        /// <summary>
        /// Get the fighter by it's id.
        /// </summary>
        /// <param name="id">ID du fighter</param>
        /// <returns>BFighter fighter.</returns>
        private BFighter GetFighter(int id)
        {
            return Fighters.FirstOrDefault(f => f.Id == id);
        }

        /// <summary>
        /// Returns the number of summoned monsters by our character.
        /// </summary>
        private int GetInvokationNumber()
        {
            int num = 0;
            foreach (BFighter fighter in Fighters)
            {
                if (fighter.GameFightMinimalStats.summoner == Fighter.Id)
                    num++;
            }
            return num;
        }

        /// <summary>
        /// Returns if any of the fighters are currently on the cellId or not.
        /// </summary>
        private bool IsFreeCell(int cellId)
        {
            return !Fighters.Any(f => f != null && f.CellId == cellId);
        }

        /// <summary>
        /// Returns the cells reachable by the player.
        /// </summary>
        private List<int> GetReachableCells()
        {
            // TODO : Manage challenge (zombie)
            List<int> listWalkableCells = new List<int>();
            MapPoint point = new MapPoint(Fighter.CellId);
            int movementPoints = Fighter.MovementPoints;

            for (int i = 0; i < 600; i++)
            {
                if (IsCellWalkable(i))
                {
                    MapPoint cellPoint = new MapPoint(i);
                    if (cellPoint.DistanceToCell(point) <= movementPoints)
                        listWalkableCells.Add(i);
                }
            }
            if (listWalkableCells.Contains(point.CellId))
                listWalkableCells.Add(point.CellId);

            return listWalkableCells;
        }

        /// <summary>
        /// Returns if the spectator mode is locked or not.
        /// </summary>
        private bool IsSpectatorAvailable()
        {
            foreach (FightOptionEnum e in Options)
                if (e == FightOptionEnum.Secret)
                    return true;
            return false;
        }

        /// <summary>
        /// Returns if a spell is launchable on a specified spellId or not.
        /// </summary>
        /// <param name="spellId">ID du sort</param>
        /// <param name="characterCellId">CellId du personnage</param>
        /// <param name="cellId">CellId cible</param>
        /// <returns>SpellInabilityReasons: Unknown, ActionPoints, TooManyLaunch, Cooldown, TooManyInvocations, None </returns>
        private SpellInabilityReason CanLaunchSpellOn(int spellId, int characterCellId, int cellId, bool withMove = false)
        {
            if (!withMove)
            {
                SpellInabilityReason canLaunchSpell = CanLaunchSpell(spellId);
                if (canLaunchSpell != SpellInabilityReason.None)
                    return canLaunchSpell;
            }
            Inventory.Item weapon = m_Account.Inventory.Weapon;
            DataClass weaponData = null;

            DataClass spellData = GameData.GetDataObject(D2oFileEnum.Spells, spellId);
            ArrayList ids = (ArrayList)spellData.Fields["spellLevels"];
            int level = m_Account.Spells.FirstOrDefault(Spell => Spell.Id == spellId).Level;
            int id = Convert.ToInt32(ids[level - 1]);
            DataClass spellLevelsData = GameData.GetDataObject(D2oFileEnum.SpellLevels, id);

            if (spellLevelsData == null && spellId != -1) // spellId = -1 -> Use weapon.
                return SpellInabilityReason.Unknown;
            if (spellId == 0 && weapon != null)
                weaponData = GameData.GetDataObject(D2oFileEnum.Items, weapon.GID);


            MapPoint characterPoint = new MapPoint(characterCellId);
            MapPoint targetPoint = new MapPoint(cellId);
            int distanceToTarget = characterPoint.DistanceToCell(targetPoint);
            int minRange = (spellId != -1) ? (int)spellLevelsData.Fields["minRange"] : (int)weaponData.Fields["minRange"];
            if ((spellId != 0 && (bool)spellLevelsData.Fields["castInDiagonal"]) || (weaponData != null && !(bool)weaponData.Fields["castInLine"]))
                minRange = (minRange * 2);
            if (minRange < 0)
                minRange = 0;
            int maxRange = (spellId != 0) ? (int)((int)spellLevelsData.Fields["range"] + ((bool)spellLevelsData.Fields["rangeCanBeBoosted"] ? (m_Account.CharacterStats.range.objectsAndMountBonus + m_Account.CharacterStats.range.contextModif) : 0)) : (int)spellLevelsData.Fields["range"];
            if ((spellId != 0 && (bool)spellLevelsData.Fields["castInDiagonal"]) || (weaponData != null && !(bool)weaponData.Fields["castInLine"]))
                maxRange = (maxRange * 2);
            if (maxRange < 0)
                maxRange = 0;
            if (distanceToTarget < minRange && distanceToTarget > 0)
                return SpellInabilityReason.MinRange;
            if (distanceToTarget > maxRange)
                return SpellInabilityReason.MaxRange;
            if (((spellId != 0 && (bool)spellLevelsData.Fields["castInLine"]) || (weaponData != null && (bool)weaponData.Fields["castInLine"]))
               && characterPoint.X != targetPoint.X &&
                characterPoint.Y != targetPoint.Y)
                return SpellInabilityReason.NotInLine;
            if ((spellId != 0 && (bool)spellLevelsData.Fields["castInDiagonal"]) || (weaponData != null && !(bool)weaponData.Fields["castInLine"]))
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
            if (((spellId != 0 && (bool)spellLevelsData.Fields["castTestLos"] && distanceToTarget > 1)) || (weaponData != null && (bool)weaponData.Fields["castTestLos"]) && distanceToTarget > 1)
            {
                ArrayList list = Dofus1Line.GetLine(characterPoint.X, characterPoint.Y, targetPoint.X, targetPoint.Y);
                int i = 0;
                while (i < list.Count - 1)
                {
                    Dofus1Line.Point point3 = (Dofus1Line.Point)list[i];
                    MapPoint point4 = new MapPoint((int)Math.Round(Math.Floor(point3.X)), (int)Math.Round(Math.Floor(point3.Y)));
                    if (!(IsFreeCell(point4.CellId)) || !(m_Account.MapData.Data.IsLineOfSight(point4.CellId)))
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
        /// Returns if a spell is launchable or not. Don't take the target in count.
        /// </summary>
        /// <param name="spellId">ID du sort</param>
        /// <returns>SpellInabilityReasons: Unknown, ActionPoints, TooManyLaunch, Cooldown, TooManyInvocations, None  </returns>
        private SpellInabilityReason CanLaunchSpell(int spellId)
        {
            Inventory.Item weapon = m_Account.Inventory.Weapon;
            DataClass weaponData = null;

            DataClass spellData = GameData.GetDataObject(D2oFileEnum.Spells, spellId);
            ArrayList ids = ((ArrayList)spellData.Fields["spellLevels"]);
            int level = 0;
            try
            {
                level = m_Account.Spells.FirstOrDefault(Spell => Spell.Id == spellId).Level;
            }
            catch (NullReferenceException ex)
            {
                m_Account.Log(new ErrorTextInformation("Le sort spécifié n'existe pas dans votre liste de sorts."), 0);
                return SpellInabilityReason.UnknownSpell;
            }
            int id = Convert.ToInt32(ids[level - 1]);
            DataClass spellLevelsData = GameData.GetDataObject(D2oFileEnum.SpellLevels, id);

            if (spellLevelsData == null && spellId != -1) // spellId = -1 -> Use weapon.
                return SpellInabilityReason.Unknown;
            if (spellId == 0 && weapon != null)
                weaponData = GameData.GetDataObject(D2oFileEnum.Items, weapon.GID);

            if ((spellId != 0 && Fighter.ActionPoints < (int)spellLevelsData.Fields["apCost"]) || (weaponData != null && Fighter.ActionPoints < (int)weaponData.Fields["apCost"]))
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
            ArrayList listOfStates = (ArrayList)spellLevelsData.Fields["statesRequired"];
            foreach (var state in listOfStates)
            {
                if (!(DurationByEffect.ContainsKey((int)state)))
                    return SpellInabilityReason.RequiredState;
            }
            listOfStates = (ArrayList)spellLevelsData.Fields["statesForbidden"];
            foreach (var state in listOfStates)
            {
                if (DurationByEffect.ContainsKey((int)state))
                    return SpellInabilityReason.ForbiddenState;
            }
            return SpellInabilityReason.None;
        }

        /// <summary>
        /// Perform the regeneration if necessary.
        /// </summary>
        private void PulseRegen()
        {
            m_Account.RegenUC.PulseRegen();
        }
        #endregion
    }
}
