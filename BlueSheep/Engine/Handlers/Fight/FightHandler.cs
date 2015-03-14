using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueSheep.Common.Protocol.Messages;
using BlueSheep.Interface;
using BlueSheep.Interface.Text;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;
using BlueSheep.Common.Types;
using BlueSheep.Engine.Enums;
using BlueSheep.Data.Pathfinding.Positions;
using BlueSheep.Data.Pathfinding;
using BlueSheep.Common.Data.D2o;
using System.Collections;
namespace BlueSheep.Engine.Handlers.Fight
{
    class FightHandler
    {
        #region Public methods
        [MessageHandler(typeof(GameActionFightDeathMessage))]
        public static void GameActionFightDeathMessageTreatment(Message message, byte[] packetDatas, AccountUC account)
        {
            GameActionFightDeathMessage msg = (GameActionFightDeathMessage)message;
            using (BigEndianReader reader = new BigEndianReader(packetDatas))
            {
                msg.Deserialize(reader);
            }
            BFighter fighter = account.Fight.GetFighter(msg.targetId);
            if (fighter != null)
            {
                fighter.IsAlive = false;
                fighter.LifePoints = 0;
                if (fighter.Id == account.Fight.Fighter.Id)
                {
                    account.Log(new ErrorTextInformation("Personnage mort :'("), 0);
                }
                account.Fight.Fighters.RemoveAt(account.Fight.Fighters.IndexOf(account.Fight.GetFighter(msg.targetId)));
            }
            if (fighter.CreatureGenericId != 0)
            {
                account.Log(new ActionTextInformation(BlueSheep.Common.Data.I18N.GetText((int)GameData.GetDataObject(D2oFileEnum.Monsters, fighter.CreatureGenericId).Fields["nameId"]) + "est mort ! "), 5);
            }
            account.Fight.DeadEnnemiInTurn += 1;
        }
        [MessageHandler(typeof(GameActionFightDispellableEffectMessage))]
        public static void GameActionFightDispellableEffectMessageTreatment(Message message, byte[] packetDatas, AccountUC account)
        {
            GameActionFightDispellableEffectMessage msg = (GameActionFightDispellableEffectMessage)message;
            using (BigEndianReader reader = new BigEndianReader(packetDatas))
            {
                msg.Deserialize(reader);
            }
            if (msg.effect is FightTemporaryBoostStateEffect)
            {
                FightTemporaryBoostStateEffect effect = (FightTemporaryBoostStateEffect)msg.effect;
                if (effect.targetId == account.Fight.Fighter.Id)
                {
                    if (account.Fight.DurationByEffect.ContainsKey(effect.stateId))
                        account.Fight.DurationByEffect.Remove(effect.stateId);
                    account.Fight.DurationByEffect.Add(effect.stateId, effect.turnDuration);
                }
            }
            else if (msg.effect is FightTemporaryBoostEffect)
            {
                FightTemporaryBoostEffect effect = (FightTemporaryBoostEffect)msg.effect;
                if (msg.actionId == 168)
                    ((BFighter)account.Fight.Fighter).ActionPoints = account.Fight.Fighter.ActionPoints - effect.delta;
                else if (msg.actionId == 169)
                    ((BFighter)account.Fight.Fighter).MovementPoints = account.Fight.Fighter.MovementPoints - effect.delta;
            }
        }
        [MessageHandler(typeof(GameActionFightPointsVariationMessage))]
        public static void GameActionFightPointsVariationMessageTreatment(Message message, byte[] packetDatas, AccountUC account)
        {
            GameActionFightPointsVariationMessage msg = (GameActionFightPointsVariationMessage)message;
            using (BigEndianReader reader = new BigEndianReader(packetDatas))
            {
                msg.Deserialize(reader);
            }
            BFighter fighter = (BFighter)account.Fight.GetFighter(msg.targetId);
            if (fighter != null)
            {
                switch (msg.actionId)
                {
                    case 101:
                    case 102:
                    case 120:
                        fighter.ActionPoints = (fighter.ActionPoints + msg.delta);
                        break;
                    case 78:
                    case 127:
                    case 129:
                        fighter.MovementPoints = (fighter.MovementPoints + msg.delta);
                        break;
                }
            }
        }
        [MessageHandler(typeof(GameActionFightSlideMessage))]
        public static void GameActionFightSlideMessageTreatment(Message message, byte[] packetDatas, AccountUC account)
        {
            GameActionFightSlideMessage msg = (GameActionFightSlideMessage)message;
            using (BigEndianReader reader = new BigEndianReader(packetDatas))
            {
                msg.Deserialize(reader);
            }
            BFighter fighter = account.Fight.GetFighter(msg.targetId);
            if (fighter != null)
            {
                //account.Log(new BotTextInformation("Ancienne cellid of " + fighter.Id + " = " + fighter.CellId));
                fighter.CellId = msg.endCellId;
                //account.Log(new BotTextInformation("Nouvelle cellid of " + fighter.Id+ " = " + fighter.CellId));
            }
        }
        [MessageHandler(typeof(GameActionFightSpellCastMessage))]
        public static void GameActionFightSpellCastMessageTreatment(Message message, byte[] packetDatas, AccountUC account)
        {
            GameActionFightSpellCastMessage msg = (GameActionFightSpellCastMessage)message;
            using (BigEndianReader reader = new BigEndianReader(packetDatas))
            {
                msg.Deserialize(reader);
            }
            BFighter fighter = (BFighter)account.Fight.GetFighter(msg.targetId);
            if (fighter != null && account.Fight.Fighter != null && fighter.Id == account.Fight.Fighter.Id)
            {
                int spellLevel = -1;
                BlueSheep.Common.Types.Spell spell = account.Spells.FirstOrDefault(s => s.Id == msg.spellId);
                if (spell != null)
                    spellLevel = spell.Level;
                if (spellLevel != -1)
                {
                    DataClass spellData = GameData.GetDataObject(D2oFileEnum.Spells, msg.spellId);
                    if (spellData != null)
                    {
                        uint spellLevelId = (uint)((ArrayList)spellData.Fields["spellLevels"])[spellLevel - 1];
                        DataClass spellLevelData = GameData.GetDataObject(D2oFileEnum.SpellLevels, (int)spellLevelId);
                        if (spellLevelData != null)
                        {
                            if ((int)spellLevelData.Fields["minCastInterval"] > 0 && !(account.Fight.LastTurnLaunchBySpell.ContainsKey(msg.spellId)))
                                account.Fight.LastTurnLaunchBySpell.Add(msg.spellId, (int)spellLevelData.Fields["minCastInterval"]);
                            if (account.Fight.TotalLaunchBySpell.ContainsKey(msg.spellId)) //Si on a déjà utilisé ce sort ce tour
                                account.Fight.TotalLaunchBySpell[msg.spellId] += 1;
                            else
                                account.Fight.TotalLaunchBySpell.Add(msg.spellId, 1);
                            if (account.Fight.TotalLaunchByCellBySpell.ContainsKey(msg.spellId)) //Si on a déjà utilisé ce sort ce tour
                            {
                                if (account.Fight.TotalLaunchByCellBySpell[msg.spellId].ContainsKey(msg.destinationCellId)) //Si on a déjà utilisé ce sort sur cette case
                                    account.Fight.TotalLaunchByCellBySpell[msg.spellId][msg.destinationCellId] += 1;
                                else
                                    account.Fight.TotalLaunchByCellBySpell[msg.spellId].Add(msg.destinationCellId, 1);
                            }
                            else
                            {
                                Dictionary<int, int> tempdico = new Dictionary<int, int>();
                                tempdico.Add(msg.destinationCellId, 1);
                                account.Fight.TotalLaunchByCellBySpell.Add(msg.spellId, tempdico);
                            }
                        }
                    }
                }
            }
        }
        [MessageHandler(typeof(GameActionFightSummonMessage))]
        public static void GameActionFightSummonMessageTreatment(Message message, byte[] packetDatas, AccountUC account)
        {
            GameActionFightSummonMessage msg = (GameActionFightSummonMessage)message;
            using (BigEndianReader reader = new BigEndianReader(packetDatas))
            {
                msg.Deserialize(reader);
            }
            account.Fight.Fighters.Add(new BFighter(msg.summon.contextualId, msg.summon.disposition.cellId, msg.summon.stats.actionPoints, msg.summon.stats, msg.summon.alive, msg.summon.stats.lifePoints, msg.summon.stats.maxLifePoints, msg.summon.stats.movementPoints, (uint)msg.summon.teamId, 0));
        }
        [MessageHandler(typeof(GameActionFightTeleportOnSameMapMessage))]
        public static void GameActionFightTeleportOnSameMapMessageTreatment(Message message, byte[] packetDatas, AccountUC account)
        {
            GameActionFightTeleportOnSameMapMessage msg = (GameActionFightTeleportOnSameMapMessage)message;
            using (BigEndianReader reader = new BigEndianReader(packetDatas))
            {
                msg.Deserialize(reader);
            }
            BFighter fighter = (BFighter)account.Fight.GetFighter(msg.targetId);
            if (fighter != null)
                fighter.CellId = msg.cellId;
        }
        [MessageHandler(typeof(GameEntitiesDispositionMessage))]
        public static void GameEntitiesDispositionMessageTreatment(Message message, byte[] packetDatas, AccountUC account)
        {
            GameEntitiesDispositionMessage msg = (GameEntitiesDispositionMessage)message;
            using (BigEndianReader reader = new BigEndianReader(packetDatas))
            {
                msg.Deserialize(reader);
            }
            if (account.Fight != null)
            {
                msg.dispositions.ToList().ForEach(d =>
                {
                    var fighter = account.Fight.GetFighter(d.id);
                    if (fighter != null)
                        ((BFighter)fighter).CellId = d.cellId;
                });
            }
            account.SetStatus(Status.Fighting);
        }
        [MessageHandler(typeof(GameFightEndMessage))]
        public static void GameFightEndMessageTreatment(Message message, byte[] packetDatas, AccountUC account)
        {
            GameFightEndMessage msg = (GameFightEndMessage)message;
            using (BigEndianReader reader = new BigEndianReader(packetDatas))
            {
                msg.Deserialize(reader);
            }
        }
        [MessageHandler(typeof(GameFightHumanReadyStateMessage))]
        public static void GameFightHumanReadyStateMessageTreatment(Message message, byte[] packetDatas, AccountUC account)
        {
            GameFightHumanReadyStateMessage msg = (GameFightHumanReadyStateMessage)message;
            using (BigEndianReader reader = new BigEndianReader(packetDatas))
            {
                msg.Deserialize(reader);
            }
            if (msg.characterId == account.CharacterBaseInformations.id)
                account.Fight.WaitForReady = !msg.isReady;
        }
        [MessageHandler(typeof(GameFightJoinMessage))]
        public static void GameFightJoinMessageTreatment(Message message, byte[] packetDatas, AccountUC account)
        {
            GameFightJoinMessage msg = (GameFightJoinMessage)message;
            using (BigEndianReader reader = new BigEndianReader(packetDatas))
            {
                msg.Deserialize(reader);
            }
            if (account.Fight != null)
            {
                account.Fight.Fighters.Clear();
                account.Fight.Options.Clear();
                account.Fight.TotalLaunchBySpell.Clear();
                account.Fight.LastTurnLaunchBySpell.Clear();
                account.Fight.TotalLaunchByCellBySpell.Clear();
                account.Fight.DurationByEffect.Clear();
                account.Fight.IsFightStarted = msg.isFightStarted;
                account.Fight.WaitForReady = (!msg.isFightStarted && msg.canSayReady);
                if (account.IsLockingFight.Checked)
                    account.Fight.LockFight();
                account.Fight.followinggroup = null;
            }
            if (account.Path != null)
            {
                account.Path.Stop = true;
            }
        }
        [MessageHandler(typeof(GameFightLeaveMessage))]
        public static void GameFightLeaveMessageTreatment(Message message, byte[] packetDatas, AccountUC account)
        {
            GameFightLeaveMessage msg = (GameFightLeaveMessage)message;
            using (BigEndianReader reader = new BigEndianReader(packetDatas))
            {
                msg.Deserialize(reader);
            }
            if (msg.charId == account.CharacterBaseInformations.id)
            {
                account.Fight.IsFightStarted = false;
                account.Fight.WaitForReady = false;
            }
            else
            {
                BFighter fighter = account.Fight.GetFighter(msg.charId);
                if (fighter != null)
                    account.Fight.Fighters.Remove(fighter);
            }
        }
        [MessageHandler(typeof(GameFightOptionStateUpdateMessage))]
        public static void GameFightOptionStateUpdateMessageTreatment(Message message, byte[] packetDatas, AccountUC account)
        {
            GameFightOptionStateUpdateMessage msg = (GameFightOptionStateUpdateMessage)message;
            using (BigEndianReader reader = new BigEndianReader(packetDatas))
            {
                msg.Deserialize(reader);
            }
            if (account.Fight == null)
                return;
            if (!msg.state && account.Fight.Options.Contains((FightOptionEnum)msg.option))
                account.Fight.Options.Remove((FightOptionEnum)msg.option);
            if (msg.state && !account.Fight.Options.Contains((FightOptionEnum)msg.option))
                account.Fight.Options.Add((FightOptionEnum)msg.option);
        }
        [MessageHandler(typeof(GameFightShowFighterMessage))]
        public static void GameFightShowFighterMessageTreatment(Message message, byte[] packetDatas, AccountUC account)
        {
            GameFightShowFighterMessage msg = (GameFightShowFighterMessage)message;
            using (BigEndianReader reader = new BigEndianReader(packetDatas))
            {
                msg.Deserialize(reader);
            }
            if (account.Fight == null)
                return;
            if (msg.informations is GameFightMonsterInformations)
            {
                GameFightMonsterInformations infos = (GameFightMonsterInformations)msg.informations;
                account.Fight.Fighters.Add(new BFighter(msg.informations.contextualId, msg.informations.disposition.cellId, msg.informations.stats.actionPoints, msg.informations.stats, msg.informations.alive, msg.informations.stats.lifePoints, msg.informations.stats.maxLifePoints, msg.informations.stats.movementPoints, (uint)msg.informations.teamId, infos.creatureGenericId));
            }
            else
            {
                account.Fight.Fighters.Add(new BFighter(msg.informations.contextualId, msg.informations.disposition.cellId, msg.informations.stats.actionPoints, msg.informations.stats, msg.informations.alive, msg.informations.stats.lifePoints, msg.informations.stats.maxLifePoints, msg.informations.stats.movementPoints, (uint)msg.informations.teamId, 0));
            }
        }
        [MessageHandler(typeof(GameFightShowFighterRandomStaticPoseMessage))]
        public static void GameFightShowFighterRandomStaticPoseMessageTreatment(Message message, byte[] packetDatas, AccountUC account)
        {
            GameFightShowFighterRandomStaticPoseMessage msg = (GameFightShowFighterRandomStaticPoseMessage)message;
            using (BigEndianReader reader = new BigEndianReader(packetDatas))
            {
                msg.Deserialize(reader);
            }
            if (msg.informations is GameFightMonsterInformations)
            {
                GameFightMonsterInformations infos = (GameFightMonsterInformations)msg.informations;
                account.Fight.Fighters.Add(new BFighter(msg.informations.contextualId, msg.informations.disposition.cellId, msg.informations.stats.actionPoints, msg.informations.stats, msg.informations.alive, msg.informations.stats.lifePoints, msg.informations.stats.maxLifePoints, msg.informations.stats.movementPoints, (uint)msg.informations.teamId, infos.creatureGenericId));
            }
            else
            {
                account.Fight.Fighters.Add(new BFighter(msg.informations.contextualId, msg.informations.disposition.cellId, msg.informations.stats.actionPoints, msg.informations.stats, msg.informations.alive, msg.informations.stats.lifePoints, msg.informations.stats.maxLifePoints, msg.informations.stats.movementPoints, (uint)msg.informations.teamId, 0));
            }
        }
        [MessageHandler(typeof(GameFightStartMessage))]
        public static void GameFightStartMessageTreatment(Message message, byte[] packetDatas, AccountUC account)
        {
            GameFightStartMessage msg = (GameFightStartMessage)message;
            using (BigEndianReader reader = new BigEndianReader(packetDatas))
            {
                msg.Deserialize(reader);
            }
            account.Fight.WaitForReady = false;
            account.Fight.IsFightStarted = true;
            account.Log(new ActionTextInformation("Début du combat"), 2);
            account.Fight.watch.Restart();
        }
        [MessageHandler(typeof(GameFightSynchronizeMessage))]
        public static void GameFightSynchronizeMessageTreatment(Message message, byte[] packetDatas, AccountUC account)
        {
            GameFightSynchronizeMessage msg = (GameFightSynchronizeMessage)message;
            using (BigEndianReader reader = new BigEndianReader(packetDatas))
            {
                msg.Deserialize(reader);
            }
            if (account.Fight != null)
            {
                account.Fight.Fighters.Clear();
                account.Fight.Fighters.AddRange(
                msg.fighters.Select(f => new BFighter(f.contextualId, f.disposition.cellId, f.stats.actionPoints, f.stats, f.alive, f.stats.lifePoints, f.stats.maxLifePoints, f.stats.movementPoints, (uint)f.teamId, 0)));
            }
        }
        [MessageHandler(typeof(GameFightTurnEndMessage))]
        public static void GameFightTurnEndMessageTreatment(Message message, byte[] packetDatas, AccountUC account)
        {
            GameFightTurnEndMessage msg = (GameFightTurnEndMessage)message;
            using (BigEndianReader reader = new BigEndianReader(packetDatas))
            {
                msg.Deserialize(reader);
            }
            if (msg.id == account.CharacterBaseInformations.id)
            {
                int num4 = 0;
                List<int> list = new List<int>();
                account.Fight.IsFighterTurn = false;
                account.Fight.TotalLaunchBySpell.Clear(); //Nettoyage des variables de vérification de lancement d'un sort
                account.Fight.TotalLaunchByCellBySpell.Clear(); //Nettoyage des variables de vérification de lancement d'un sort
                for (int i = 0; i < account.Fight.DurationByEffect.Keys.Count; i++)
                {
                    Dictionary<int, int> durationPerEffect = account.Fight.DurationByEffect;
                    num4 = Enumerable.ElementAtOrDefault<int>(account.Fight.DurationByEffect.Keys, i);
                    durationPerEffect[num4] = (durationPerEffect[num4] - 1);
                    if (account.Fight.DurationByEffect[Enumerable.ElementAtOrDefault<int>(account.Fight.DurationByEffect.Keys, i)] <= 0)
                        list.Add(Enumerable.ElementAtOrDefault<int>(account.Fight.DurationByEffect.Keys, i));
                }
                while (list.Count > 0)
                {
                    account.Fight.DurationByEffect.Remove(list[0]);
                    list.RemoveAt(0);
                }
                for (int i = 0; i < account.Fight.LastTurnLaunchBySpell.Keys.Count; i++)
                {
                    Dictionary<int, int> dictionary = account.Fight.LastTurnLaunchBySpell;
                    num4 = Enumerable.ElementAtOrDefault<int>(account.Fight.LastTurnLaunchBySpell.Keys, i);
                    dictionary[num4] = (dictionary[num4] - 1);
                    if (account.Fight.LastTurnLaunchBySpell[Enumerable.ElementAtOrDefault<int>(account.Fight.LastTurnLaunchBySpell.Keys, i)] <= 0)
                        list.Add(Enumerable.ElementAtOrDefault<int>(account.Fight.LastTurnLaunchBySpell.Keys, i));
                }
                while (list.Count > 0)
                {
                    account.Fight.LastTurnLaunchBySpell.Remove(list[0]);
                    list.RemoveAt(0);
                }
                account.Log(new BotTextInformation("Fin du tour"), 5);
            }
            BFighter fighter = (BFighter)account.Fight.GetFighter(msg.id);
            if (fighter != null)
            {
                fighter.ActionPoints = fighter.GameFightMinimalStats.maxActionPoints;
                fighter.MovementPoints = fighter.GameFightMinimalStats.maxMovementPoints;
            }
        }
        [MessageHandler(typeof(GameFightTurnStartMessage))]
        public static void GameFightTurnStartMessageTreatment(Message message, byte[] packetDatas, AccountUC account)
        {
            GameFightTurnStartMessage msg = (GameFightTurnStartMessage)message;
            using (BigEndianReader reader = new BigEndianReader(packetDatas))
            {
                msg.Deserialize(reader);
            }
            if (!account.Fight.IsFightStarted)
                account.Fight.IsFightStarted = true;
            if (msg.id == account.CharacterBaseInformations.id)
            {
                account.Fight.IsFighterTurn = true;
            }
            else
                account.Fight.IsFighterTurn = false;
            account.Fight.FighterTurnId = msg.id;
            //TODO Perform Turn
        }
        [MessageHandler(typeof(GameMapMovementMessage))]
        public static void GameMapMovementMessageTreatment(Message message, byte[] packetDatas, AccountUC account)
        {
            GameMapMovementMessage msg = (GameMapMovementMessage)message;
            using (BigEndianReader reader = new BigEndianReader(packetDatas))
            {
                msg.Deserialize(reader);
            }
            MovementPath clientMovement = MapMovementAdapter.GetClientMovement(msg.keyMovements.Select<short, uint>(k => (uint)k).ToList());
            if (account.state == Enums.Status.Fighting)
            {
                BFighter fighter = account.Fight.GetFighter(msg.actorId);
                if (fighter != null)
                {
                    //account.Log(new BotTextInformation("GameMap Ancienne cellid of " + fighter.Id + " = " + fighter.CellId));
                    fighter.CellId = clientMovement.CellEnd.CellId;
                    //account.Log(new BotTextInformation("GameMap Nouvelle cellid of " + fighter.Id + " = " + fighter.CellId));
                }
            }
        }
        [MessageHandler(typeof(GameFightNewRoundMessage))]
        public static void GameFightNewRoundMessageTreatment(Message message, byte[] packetDatas, AccountUC account)
        {
            GameFightNewRoundMessage msg = (GameFightNewRoundMessage)message;
            using (BigEndianReader reader = new BigEndianReader(packetDatas))
            {
                msg.Deserialize(reader);
            }
            account.Fight.TurnId = msg.roundNumber;
        }
        [MessageHandler(typeof(GameFightTurnStartPlayingMessage))]
        public static void GameFightTurnStartPlayingMessageTreatment(Message message, byte[] packetDatas, AccountUC account)
        {
            GameFightTurnStartPlayingMessage msg = (GameFightTurnStartPlayingMessage)message;
            using (BigEndianReader reader = new BigEndianReader(packetDatas))
            {
                msg.Deserialize(reader);
            }
            account.Fight.PerformAutoTimeoutFight(100);
            account.Fight.FightTurn();
        }
        [MessageHandler(typeof(GameFightPlacementPossiblePositionsMessage))]
        public static void GameFightPlacementPossiblePositionsMessageTreatment(Message message, byte[] packetDatas, AccountUC account)
        {
            GameFightPlacementPossiblePositionsMessage msg = (GameFightPlacementPossiblePositionsMessage)message;
            using (BigEndianReader reader = new BigEndianReader(packetDatas))
            {
                msg.Deserialize(reader);
            }
            account.SetStatus(Status.Fighting);
            account.Fight.PlacementCells = msg.positionsForChallengers.ToList();
            account.Fight.TurnId = 0;
            if (account.Fight.m_Conf.Tactic != BlueSheep.Core.Fight.TacticEnum.Immobile)
                account.Fight.PlaceCharacter();
            //account.Fight.PerformAutoTimeoutFight(3000);
            //if (account.IsMITM)
            // account.Fight.PerformAutoTimeoutFight(3000);
            if (account.WithItemSetBox.Checked == true)
            {
                sbyte id = (sbyte)account.PresetStartUpD.Value;
                InventoryPresetUseMessage msg2 = new InventoryPresetUseMessage((sbyte)(id - 1));
                account.SocketManager.Send(msg2);
                account.Log(new ActionTextInformation("Equipement rapide numero " + Convert.ToString(id)), 5);
                account.Fight.PerformAutoTimeoutFight(500);
            }
            //LaunchWatch()
            GameFightReadyMessage nmsg = new GameFightReadyMessage(true);
            account.SocketManager.Send(nmsg);
            account.Log(new BotTextInformation("Send Ready !"), 5);
        }
        [MessageHandler(typeof(GameFightTurnReadyRequestMessage))]
        public static void GameFightTurnReadyRequestMessageTreatment(Message message, byte[] packetDatas, AccountUC account)
        {
            GameFightTurnReadyRequestMessage msg = (GameFightTurnReadyRequestMessage)message;
            using (BigEndianReader reader = new BigEndianReader(packetDatas))
            {
                msg.Deserialize(reader);
            }
            GameFightTurnReadyMessage msg2 = new GameFightTurnReadyMessage(true);
            account.SocketManager.Send(msg2);
        }
        [MessageHandler(typeof(SequenceEndMessage))]
        public static void SequenceEndMessageTreatment(Message message, byte[] packetDatas, AccountUC account)
        {
            SequenceEndMessage msg = (SequenceEndMessage)message;
            using (BigEndianReader reader = new BigEndianReader(packetDatas))
            {
                msg.Deserialize(reader);
            }
            if ((account.Fight.Fighter != null) && (account.Fight.Fighter.Id == msg.authorId) && (!account.IsMITM))
            {
                GameActionAcknowledgementMessage msg2 = new GameActionAcknowledgementMessage(true, msg.sequenceType);
                account.SocketManager.Send(msg2);
            }
        }
        [MessageHandler(typeof(LifePointsRegenBeginMessage))]
        public static void LifePointsRegenBeginMessageTreatment(Message message, byte[] packetDatas, AccountUC account)
        {
            LifePointsRegenBeginMessage msg = (LifePointsRegenBeginMessage)message;
            using (BigEndianReader reader = new BigEndianReader(packetDatas))
            {
                msg.Deserialize(reader);
            }
            if (account.Fight != null && account.state == Enums.Status.Fighting)
            {
                account.Fight.watch.Stop();
                account.Fight.WaitForReady = false;
                account.Fight.IsFighterTurn = false;
                account.Fight.IsFightStarted = false;
                account.Log(new ActionTextInformation("Combat fini ! (" + account.Fight.watch.Elapsed.Minutes + " min, " + account.Fight.watch.Elapsed.Seconds + " sec)"), 0);
                account.Fight.watch.Reset();
                account.Fight.PerformAutoTimeoutFight(2000);
                if (account.WithItemSetBox.Checked == true)
                {
                    sbyte id = (sbyte)account.PresetEndUpD.Value;
                    InventoryPresetUseMessage msg2 = new InventoryPresetUseMessage((sbyte)(id - 1));
                    account.SocketManager.Send(msg2);
                    account.Log(new ActionTextInformation("Equipement rapide numero " + Convert.ToString(id)), 5);
                }
                account.Fight.PerformAutoTimeoutFight(2000);
                account.Fight.PulseRegen();
                account.SetStatus(Status.None);
            }
            //account.Path.Stop = false;
        }
        [MessageHandler(typeof(LifePointsRegenEndMessage))]
        public static void LifePointsRegenEndMessageTreatment(Message message, byte[] packetDatas, AccountUC account)
        {
            LifePointsRegenEndMessage msg = (LifePointsRegenEndMessage)message;
            using (BigEndianReader reader = new BigEndianReader(packetDatas))
            {
                msg.Deserialize(reader);
            }
            int percent = (msg.lifePoints / msg.maxLifePoints) * 100;
            account.Log(new BotTextInformation("Fin de la régénération. + " + msg.lifePointsGained + " points de vie"), 2);
            //string text = msg.lifePoints + "/" + msg.maxLifePoints + "(" + percent + "%)";
            account.ModifBar(2, (int)msg.maxLifePoints, (int)msg.lifePoints, "Vitalité");
        }
        [MessageHandler(typeof(GameFightSpectatorJoinMessage))]
        public static void GameFightSpectatorJoinMessageTreatment(Message message, byte[] packetDatas, AccountUC account)
        {
            GameFightSpectatorJoinMessage msg = (GameFightSpectatorJoinMessage)message;
            using (BigEndianReader reader = new BigEndianReader(packetDatas))
            {
                msg.Deserialize(reader);
            }
        }
        #endregion
    }
}

