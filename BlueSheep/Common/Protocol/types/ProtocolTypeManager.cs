using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.Serialization;
using System.Text;

namespace BlueSheep.Common.Protocol.Types
{
    public static class ProtocolTypeManager
    {
        private static readonly Dictionary<short, Type> m_types = new Dictionary<short, Type>(200);
        private static readonly Dictionary<short, Func<object>> m_typesConstructors = new Dictionary<short, Func<object>>(200);

        static ProtocolTypeManager()
        {
            Assembly asm = Assembly.GetAssembly(typeof(ProtocolTypeManager));

            foreach (Type type in asm.GetTypes())
            {
                if (type.Namespace == null || !type.Namespace.StartsWith(typeof(ProtocolTypeManager).Namespace))
                    continue;

                FieldInfo field = type.GetField("ID");

                if (field != null)
                {
                    // le cast uint est obligatoire car l'objet n'a pas de type
                    short id = (short)(field.GetValue(type));

                    m_types.Add(id, type);

                    ConstructorInfo ctor = type.GetConstructor(Type.EmptyTypes);

                    if (ctor == null)
                        throw new Exception(string.Format("'{0}' doesn't implemented a parameterless constructor", type));

                    //m_typesConstructors.Add(id, ConstructorHelper.CreateDelegate(ctor, type)<Func<object>>();
                    m_typesConstructors.Add(id, ctor.CreateDelegate<Func<object>>());
                }
            }
        }

        public static T CreateDelegate<T>(this ConstructorInfo ctor)
        {
            var parameters = ctor.GetParameters().Select(param => Expression.Parameter(param.ParameterType)).ToList();

            var lamba = Expression.Lambda<T>(Expression.New(ctor, parameters), parameters);

            return lamba.Compile();
        }

        /// <summary>
        ///   Gets instance of the type defined by id.
        /// </summary>
        /// <typeparam name = "T">Type.</typeparam>
        /// <param name = "id">id.</param>
        /// <returns></returns>
        public static T GetInstance<T>(short id) where T : class
        {
            if (!m_types.ContainsKey(id))
            {
                throw new ProtocolTypeNotFoundException(string.Format("Type <id:{0}> doesn't exist", id));
            }

            return m_typesConstructors[id]() as T;
        }

        [Serializable]
        public class ProtocolTypeNotFoundException : Exception
        {
            public ProtocolTypeNotFoundException()
            {
            }

            public ProtocolTypeNotFoundException(string message)
                : base(message)
            {
            }

            public ProtocolTypeNotFoundException(string message, Exception inner)
                : base(message, inner)
            {
            }

            protected ProtocolTypeNotFoundException(
                SerializationInfo info,
                StreamingContext context)
                : base(info, context)
            {
            }
        }
        //public static object GetInstance(short id)
        //{
        //    object type;
        //    switch ((int)id)
        //    {

        //        case 163:
        //            type = new CharacterMinimalPlusLookInformations();
        //            return type;
        //        case 45:
        //            type = new CharacterBaseInformations();
        //            return type;
        //        case 391:
        //            type = new PartyMemberArenaInformations();
        //            return type;
        //        case 376:
        //            type = new PartyInvitationMemberInformations();
        //            return type;
        //        case 474:
        //            type = new CharacterHardcoreOrEpicInformations();
        //            return type;
        //        case 445:
        //            type = new CharacterMinimalGuildInformations();
        //            return type;
        //        case 444:
        //            type = new CharacterMinimalAllianceInformations();
        //            return type;
        //        case 193:
        //            type = new CharacterMinimalPlusLookAndGradeInformations();
        //            return type;
        //        case 90:
        //            type = new PartyMemberInformations();
        //            return type;
        //        case 60:
        //            type = new EntityDispositionInformations();
        //            return type;
        //        case 107:
        //            type = new IdentifiedEntityDispositionInformations();
        //            return type;
        //        case 217:
        //            type = new FightEntityDispositionInformations();
        //            return type;
        //        case 416:
        //            type = new AbstractSocialGroupInfos();
        //            return type;
        //        case 419:
        //            type = new BasicAllianceInformations();
        //            return type;
        //        case 418:
        //            type = new BasicNamedAllianceInformations();
        //            return type;
        //        case 417:
        //            type = new AllianceInformations();
        //            return type;
        //        case 421:
        //            type = new AllianceFactSheetInformations();
        //            return type;
        //        case 365:
        //            type = new BasicGuildInformations();
        //            return type;
        //        case 127:
        //            type = new GuildInformations();
        //            return type;
        //        case 424:
        //            type = new GuildFactSheetInformations();
        //            return type;
        //        case 423:
        //            type = new GuildInsiderFactSheetInformations();
        //            return type;
        //        case 422:
        //            type = new AlliancedGuildFactSheetInformations();
        //            return type;
        //        case 420:
        //            type = new GuildInAllianceInformations();
        //            return type;
        //        case 435:
        //            type = new GuildVersatileInformations();
        //            return type;
        //        case 437:
        //            type = new GuildInAllianceVersatileInformations();
        //            return type;
        //        case 438:
        //            type = new PrismSubareaEmptyInfo();
        //            return type;
        //        case 434:
        //            type = new PrismGeolocalizedInformation();
        //            return type;
        //        case 428:
        //            type = new PrismInformation();
        //            return type;
        //        case 431:
        //            type = new AllianceInsiderPrismInformation();
        //            return type;
        //        case 427:
        //            type = new AlliancePrismInformation();
        //            return type;
        //        case 44:
        //            type = new FightTeamMemberInformations();
        //            return type;
        //        case 13:
        //            type = new FightTeamMemberCharacterInformations();
        //            return type;
        //        case 426:
        //            type = new FightTeamMemberWithAllianceCharacterInformations();
        //            return type;
        //        case 451:
        //            type = new FightTeamMemberCompanionInformations();
        //            return type;
        //        case 6:
        //            type = new FightTeamMemberMonsterInformations();
        //            return type;
        //        case 177:
        //            type = new FightTeamMemberTaxCollectorInformations();
        //            return type;
        //        case 33:
        //            type = new FightTeamInformations();
        //            return type;
        //        case 439:
        //            type = new FightAllianceTeamInformations();
        //            return type;
        //        case 31:
        //            type = new GameFightMinimalStats();
        //            return type;
        //        case 360:
        //            type = new GameFightMinimalStatsPreparation();
        //            return type;
        //        case 16:
        //            type = new FightResultListEntry();
        //            return type;
        //        case 189:
        //            type = new FightResultFighterListEntry();
        //            return type;
        //        case 216:
        //            type = new FightResultMutantListEntry();
        //            return type;
        //        case 24:
        //            type = new FightResultPlayerListEntry();
        //            return type;
        //        case 84:
        //            type = new FightResultTaxCollectorListEntry();
        //            return type;
        //        case 191:
        //            type = new FightResultAdditionalData();
        //            return type;
        //        case 190:
        //            type = new FightResultPvpData();
        //            return type;
        //        case 192:
        //            type = new FightResultExperienceData();
        //            return type;
        //        case 206:
        //            type = new AbstractFightDispellableEffect();
        //            return type;
        //        case 209:
        //            type = new FightTemporaryBoostEffect();
        //            return type;
        //        case 214:
        //            type = new FightTemporaryBoostStateEffect();
        //            return type;
        //        case 211:
        //            type = new FightTemporaryBoostWeaponDamagesEffect();
        //            return type;
        //        case 207:
        //            type = new FightTemporarySpellBoostEffect();
        //            return type;
        //        case 366:
        //            type = new FightTemporarySpellImmunityEffect();
        //            return type;
        //        case 210:
        //            type = new FightTriggeredEffect();
        //            return type;
        //        case 76:
        //            type = new ObjectEffect();
        //            return type;
        //        case 70:
        //            type = new ObjectEffectInteger();
        //            return type;
        //        case 71:
        //            type = new ObjectEffectCreature();
        //            return type;
        //        case 81:
        //            type = new ObjectEffectLadder();
        //            return type;
        //        case 75:
        //            type = new ObjectEffectDuration();
        //            return type;
        //        case 73:
        //            type = new ObjectEffectDice();
        //            return type;
        //        case 82:
        //            type = new ObjectEffectMinMax();
        //            return type;
        //        case 74:
        //            type = new ObjectEffectString();
        //            return type;
        //        case 179:
        //            type = new ObjectEffectMount();
        //            return type;
        //        case 72:
        //            type = new ObjectEffectDate();
        //            return type;
        //        case 356:
        //            type = new UpdateMountBoost();
        //            return type;
        //        case 357:
        //            type = new UpdateMountIntBoost();
        //            return type;
        //        case 369:
        //            type = new Shortcut();
        //            return type;
        //        case 367:
        //            type = new ShortcutObject();
        //            return type;
        //        case 371:
        //            type = new ShortcutObjectItem();
        //            return type;
        //        case 370:
        //            type = new ShortcutObjectPreset();
        //            return type;
        //        case 368:
        //            type = new ShortcutSpell();
        //            return type;
        //        case 389:
        //            type = new ShortcutEmote();
        //            return type;
        //        case 388:
        //            type = new ShortcutSmiley();
        //            return type;
        //        case 106:
        //            type = new IgnoredInformations();
        //            return type;
        //        case 105:
        //            type = new IgnoredOnlineInformations();
        //            return type;
        //        case 78:
        //            type = new FriendInformations();
        //            return type;
        //        case 92:
        //            type = new FriendOnlineInformations();
        //            return type;
        //        case 77:
        //            type = new FriendSpouseInformations();
        //            return type;
        //        case 93:
        //            type = new FriendSpouseOnlineInformations();
        //            return type;
        //        case 219:
        //            type = new InteractiveElementSkill();
        //            return type;
        //        case 220:
        //            type = new InteractiveElementNamedSkill();
        //            return type;
        //        case 80:
        //            type = new InteractiveElement();
        //            return type;
        //        case 398:
        //            type = new InteractiveElementWithAgeBonus();
        //            return type;
        //        case 102:
        //            type = new SkillActionDescription();
        //            return type;
        //        case 103:
        //            type = new SkillActionDescriptionTimed();
        //            return type;
        //        case 99:
        //            type = new SkillActionDescriptionCollect();
        //            return type;
        //        case 100:
        //            type = new SkillActionDescriptionCraft();
        //            return type;
        //        case 111:
        //            type = new HouseInformations();
        //            return type;
        //        case 112:
        //            type = new HouseInformationsExtended();
        //            return type;
        //        case 132:
        //            type = new PaddockInformations();
        //            return type;
        //        case 130:
        //            type = new PaddockBuyableInformations();
        //            return type;
        //        case 133:
        //            type = new PaddockAbandonnedInformations();
        //            return type;
        //        case 131:
        //            type = new PaddockPrivateInformations();
        //            return type;
        //        case 183:
        //            type = new PaddockContentInformations();
        //            return type;
        //        case 150:
        //            type = new GameContextActorInformations();
        //            return type;
        //        case 143:
        //            type = new GameFightFighterInformations();
        //            return type;
        //        case 151:
        //            type = new GameFightAIInformations();
        //            return type;
        //        case 29:
        //            type = new GameFightMonsterInformations();
        //            return type;
        //        case 203:
        //            type = new GameFightMonsterWithAlignmentInformations();
        //            return type;
        //        case 48:
        //            type = new GameFightTaxCollectorInformations();
        //            return type;
        //        case 158:
        //            type = new GameFightFighterNamedInformations();
        //            return type;
        //        case 50:
        //            type = new GameFightMutantInformations();
        //            return type;
        //        case 46:
        //            type = new GameFightCharacterInformations();
        //            return type;
        //        case 450:
        //            type = new GameFightCompanionInformations();
        //            return type;
        //        case 141:
        //            type = new GameRolePlayActorInformations();
        //            return type;
        //        case 154:
        //            type = new GameRolePlayNamedActorInformations();
        //            return type;
        //        case 159:
        //            type = new GameRolePlayHumanoidInformations();
        //            return type;
        //        case 3:
        //            type = new GameRolePlayMutantInformations();
        //            return type;
        //        case 36:
        //            type = new GameRolePlayCharacterInformations();
        //            return type;
        //        case 129:
        //            type = new GameRolePlayMerchantInformations();
        //            return type;
        //        case 180:
        //            type = new GameRolePlayMountInformations();
        //            return type;
        //        case 156:
        //            type = new GameRolePlayNpcInformations();
        //            return type;
        //        case 383:
        //            type = new GameRolePlayNpcWithQuestInformations();
        //            return type;
        //        case 160:
        //            type = new GameRolePlayGroupMonsterInformations();
        //            return type;
        //        case 464:
        //            type = new GameRolePlayGroupMonsterWaveInformations();
        //            return type;
        //        case 148:
        //            type = new GameRolePlayTaxCollectorInformations();
        //            return type;
        //        case 161:
        //            type = new GameRolePlayPrismInformations();
        //            return type;
        //        case 467:
        //            type = new GameRolePlayPortalInformations();
        //            return type;
        //        case 471:
        //            type = new GameRolePlayTreasureHintInformations();
        //            return type;
        //        case 157:
        //            type = new HumanInformations();
        //            return type;
        //        case 425:
        //            type = new HumanOptionAlliance();
        //            return type;
        //        case 411:
        //            type = new HumanOptionOrnament();
        //            return type;
        //        case 410:
        //            type = new HumanOptionFollowers();
        //            return type;
        //        case 449:
        //            type = new HumanOptionObjectUse();
        //            return type;
        //        case 409:
        //            type = new HumanOptionGuild();
        //            return type;
        //        case 408:
        //            type = new HumanOptionTitle();
        //            return type;
        //        case 407:
        //            type = new HumanOptionEmote();
        //            return type;
        //        case 147:
        //            type = new TaxCollectorStaticInformations();
        //            return type;
        //        case 440:
        //            type = new TaxCollectorStaticExtendedInformations();
        //            return type;
        //        case 167:
        //            type = new TaxCollectorInformations();
        //            return type;
        //        case 448:
        //            type = new TaxCollectorComplementaryInformations();
        //            return type;
        //        case 372:
        //            type = new TaxCollectorLootInformations();
        //            return type;
        //        case 447:
        //            type = new TaxCollectorWaitingForHelpInformations();
        //            return type;
        //        case 446:
        //            type = new TaxCollectorGuildInformations();
        //            return type;
        //        case 140:
        //            type = new GroupMonsterStaticInformations();
        //            return type;
        //        case 396:
        //            type = new GroupMonsterStaticInformationsWithAlternatives();
        //            return type;
        //        case 381:
        //            type = new QuestActiveInformations();
        //            return type;
        //        case 382:
        //            type = new QuestActiveDetailedInformations();
        //            return type;
        //        case 385:
        //            type = new QuestObjectiveInformations();
        //            return type;
        //        case 386:
        //            type = new QuestObjectiveInformationsWithCompletion();
        //            return type;
        //        case 413:
        //            type = new GameFightFighterLightInformations();
        //            return type;
        //        case 456:
        //            type = new GameFightFighterNamedLightInformations();
        //            return type;
        //        case 455:
        //            type = new GameFightFighterMonsterLightInformations();
        //            return type;
        //        case 454:
        //            type = new GameFightFighterCompanionLightInformations();
        //            return type;
        //        case 457:
        //            type = new GameFightFighterTaxCollectorLightInformations();
        //            return type;
        //        case 174:
        //            type = new MapCoordinates();
        //            return type;
        //        case 392:
        //            type = new MapCoordinatesAndId();
        //            return type;
        //        case 176:
        //            type = new MapCoordinatesExtended();
        //            return type;
        //        case 463:
        //            type = new TreasureHuntStep();
        //            return type;
        //        case 462:
        //            type = new TreasureHuntStepFight();
        //            return type;
        //        case 468:
        //            type = new TreasureHuntStepFollowDirection();
        //            return type;
        //        case 465:
        //            type = new TreasureHuntStepDig();
        //            return type;
        //        case 461:
        //            type = new TreasureHuntStepFollowDirectionToPOI();
        //            return type;
        //        case 472:
        //            type = new TreasureHuntStepFollowDirectionToHint();
        //            return type;
        //        case 466:
        //            type = new PortalInformation();
        //            return type;
        //        case 415:
        //            type = new PlayerStatus();
        //            return type;
        //        case 414:
        //            type = new PlayerStatusExtended();
        //            return type;
        //        case 430:
        //            type = new ServerSessionConstant();
        //            return type;
        //        case 436:
        //            type = new ServerSessionConstantString();
        //            return type;
        //        case 433:
        //            type = new ServerSessionConstantInteger();
        //            return type;
        //        case 429:
        //            type = new ServerSessionConstantLong();
        //            return type;
        //    }
        //    return null;
        //}
    }

}
