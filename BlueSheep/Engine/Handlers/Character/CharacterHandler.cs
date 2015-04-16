using BlueSheep.Common.Data;
using BlueSheep.Common.Data.D2o;
using BlueSheep.Common.IO;
using BlueSheep.Common.Protocol.Messages;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.Types;
using BlueSheep.Engine.Types;
using BlueSheep.Interface;
using BlueSheep.Interface.Text;
using System;

namespace BlueSheep.Engine.Handlers.Character
{
    class CharacterHandler
    {
        #region Public methods
        [MessageHandler(typeof(CharactersListMessage))]
        public static void CharactersListMessageTreatment(Message message, byte[] packetDatas, AccountUC account)
        {
            CharactersListMessage charactersListMessage = (CharactersListMessage)message;

            using (BigEndianReader reader = new BigEndianReader(packetDatas))
            {
                charactersListMessage.Deserialize(reader);
            }

            account.CharacterBaseInformations = charactersListMessage.characters[0];
        
            //MainForm.ActualMainForm.ActualizeAccountInformations();

            if (!account.IsMITM)
            {
                CharacterSelectionMessage characterSelectionMessage = new CharacterSelectionMessage(account.CharacterBaseInformations.id);
                account.SocketManager.Send(characterSelectionMessage);
            }
            
        }

        [MessageHandler(typeof(CharacterSelectedSuccessMessage))]
        public static void CharacterSelectedSuccessMessageTreatment(Message message, byte[] packetDatas, AccountUC account)
        {
            CharacterSelectedSuccessMessage characterSelectedSuccessMessage = (CharacterSelectedSuccessMessage)message;

            using (BigEndianReader reader = new BigEndianReader(packetDatas))
            {
                characterSelectedSuccessMessage.Deserialize(reader);
            }

            account.CharacterBaseInformations = characterSelectedSuccessMessage.infos;

            account.Log(new BotTextInformation(account.CharacterBaseInformations.name + " de niveau "+ account.CharacterBaseInformations.level + " est connecté."),1);
            account.ModifBar(7,0,0, account.AccountName + " - " + account.CharacterBaseInformations.name);
            account.ModifBar(8, 0, 0, Convert.ToString(account.CharacterBaseInformations.level));
            
            //MainForm.ActualMainForm.ActualizeAccountInformations();
        }

        [MessageHandler(typeof(CharacterSelectedErrorMessage))]
        public static void CharacterSelectedErrorMessageTreatment(Message message, byte[] packetDatas, AccountUC account)
        {
            account.Log(new ConnectionTextInformation("Erreur lors de la sélection du personnage."),0);
            account.TryReconnect(30);
        }

        [MessageHandler(typeof(CharacterStatsListMessage))]
        public static void CharacterStatsListMessageTreatment(Message message, byte[] packetDatas, AccountUC account)
        {
            CharacterStatsListMessage msg = (CharacterStatsListMessage)message;
            using (BigEndianReader reader = new BigEndianReader(packetDatas))
            {
                msg.Deserialize(reader);
            }
            if (!account.ConfigManager.Restored)
                account.ConfigManager.RecoverConfig();
            account.CharacterStats = msg.stats;
            account.CaracUC.Init();
            if (account.MyGroup != null)
                ((GroupForm)account.ParentForm).AddMember(account.CharacterBaseInformations.name);
            int percent = (msg.stats.lifePoints / msg.stats.maxLifePoints) * 100;
            string text = msg.stats.lifePoints + "/" + msg.stats.maxLifePoints + "(" + percent + "%)";
            account.ModifBar(2, (int)msg.stats.maxLifePoints, (int)msg.stats.lifePoints, "Vitalité");
            double i = msg.stats.experience - msg.stats.experienceLevelFloor;
            double j = msg.stats.experienceNextLevelFloor - msg.stats.experienceLevelFloor;
            try
            {
                int xppercent = (int)(Math.Round(i / j,2) * 100);
            }
            catch (Exception ex)
            {

            }
            account.ModifBar(1, (int)msg.stats.experienceNextLevelFloor - (int)msg.stats.experienceLevelFloor, (int)msg.stats.experience - (int)msg.stats.experienceLevelFloor, "Experience");
            account.ModifBar(4, 0, 0, msg.stats.kamas.ToString());
        }
        
        [MessageHandler(typeof(SpellListMessage))]
        public static void SpellListMessageTreatment(Message message, byte[] packetDatas, AccountUC account)
        {
            SpellListMessage msg = (SpellListMessage)message;
            using (BigEndianReader reader = new BigEndianReader(packetDatas))
            {
                msg.Deserialize(reader);
            }

            account.Spells.Clear();
            foreach (SpellItem spell in msg.spells)
                account.Spells.Add(new Spell(spell.spellId, spell.spellLevel, spell.position));
        }

        [MessageHandler(typeof(CharacterSelectedForceMessage))]
        public static void CharacterSelectedForceMessageTreatment(Message message, byte[] packetDatas, AccountUC account)
        {
            CharacterSelectedForceMessage msg = (CharacterSelectedForceMessage)message;
            using (BigEndianReader reader = new BigEndianReader(packetDatas))
            {
                msg.Deserialize(reader);
            }
                CharacterSelectedForceReadyMessage nmsg = new CharacterSelectedForceReadyMessage();
                account.SocketManager.Send(nmsg);
            
        }

        [MessageHandler(typeof(CharacterLevelUpMessage))]
        public static void CharacterLevelUpMessageTreatment(Message message, byte[] packetDatas, AccountUC account)
        {
            CharacterLevelUpMessage msg = (CharacterLevelUpMessage)message;
            using (BigEndianReader reader = new BigEndianReader(packetDatas))
            {
                msg.Deserialize(reader);
            }
            account.ModifBar(8, 0, 0, Convert.ToString(msg.newLevel));
            account.Log(new BotTextInformation("Level up ! New level : " + Convert.ToString(msg.newLevel)), 3);
            account.CaracUC.UpAuto();
        }

        [MessageHandler(typeof(AchievementFinishedMessage))]
        public static void AchievementFinishedTreatment(Message message, byte[] packetDatas, AccountUC account)
        {
            AchievementFinishedMessage msg = (AchievementFinishedMessage)message;
            using (BigEndianReader reader = new BigEndianReader(packetDatas))
            {
                msg.Deserialize(reader);
            }
            DataClass d = GameData.GetDataObject(D2oFileEnum.Achievements, msg.id);
            account.Log(new ActionTextInformation("Succès débloqué : " + I18N.GetText((int)d.Fields["nameId"])),3);
                AchievementRewardRequestMessage nmsg = new AchievementRewardRequestMessage(-1);
                account.SocketManager.Send(nmsg);
            
        }

        [MessageHandler(typeof(CharacterExperienceGainMessage))]
        public static void CharacterExperienceGainMessageTreatment(Message message, byte[] packetDatas, AccountUC account)
        {
            CharacterExperienceGainMessage msg = (CharacterExperienceGainMessage)message;
            using (BigEndianReader reader = new BigEndianReader(packetDatas))
            {
                msg.Deserialize(reader);
            }
            account.Log(new ActionTextInformation("Experience gagnée : + " + msg.experienceCharacter + " points d'expérience"), 4);
            account.CharacterStats.experience += msg.experienceCharacter;
            double i = account.CharacterStats.experience - account.CharacterStats.experienceLevelFloor;
            double j = account.CharacterStats.experienceNextLevelFloor - account.CharacterStats.experienceLevelFloor;
            try
            {
                int xppercent = (int)((i / j) * 100);
            }
            catch (Exception ex)
            {

            }
            account.ModifBar(1, (int)account.CharacterStats.experienceNextLevelFloor - (int)account.CharacterStats.experienceLevelFloor, (int)account.CharacterStats.experience - (int)account.CharacterStats.experienceLevelFloor, "Experience");
            if (account.Fight != null)
            {
                account.FightData.xpWon[DateTime.Today] += (int)msg.experienceCharacter;
            }

        }

        [MessageHandler(typeof(StatsUpgradeResultMessage))]
        public static void StatsUpgradeResultMessageTreatment(Message message, byte[] packetDatas, AccountUC account)
        {
            StatsUpgradeResultMessage msg = (StatsUpgradeResultMessage)message;
            using (BigEndianReader reader = new BigEndianReader(packetDatas))
            {
                msg.Deserialize(reader);
            }
            //if (msg.result == 1)
            //{
            //    //account.CaracUC.DecreaseAvailablePoints(msg.nbCharacBoost);
            //    account.Log(new BotTextInformation("Caractéristique augmentée."),0);
            //}
            //else
            //    account.Log(new ErrorTextInformation("Echec de l'up de caractéristique."), 0);
                
        }

        [MessageHandler(typeof(PlayerStatusUpdateMessage))]
        public static void PlayerStatusUpdateMessageTreatment(Message message, byte[] packetDatas, AccountUC account)
        {
            PlayerStatusUpdateMessage msg = (PlayerStatusUpdateMessage)message;
            using (BigEndianReader reader = new BigEndianReader(packetDatas))
            {
                msg.Deserialize(reader);
            }
            if (msg.playerId == account.CharacterBaseInformations.id)
            {
                switch (msg.status.statusId)
                {
                    case 10:
                        account.Log(new ActionTextInformation("Statut disponible activé."), 3);
                        break;
                    case 20:
                        account.Log(new ActionTextInformation("Statut absent activé."), 3);
                        PlayerStatusUpdateRequestMessage nmsg = new PlayerStatusUpdateRequestMessage(new PlayerStatus(10));
                        account.SocketManager.Send(nmsg);
                        break;
                    case  40:
                        account.Log(new ActionTextInformation("Statut solo activé."), 3);
                        break;
                    case 30:
                        account.Log(new ActionTextInformation("Statut privé activé."), 3);
                        break;

                }
            }
        }
        
        #endregion
    }
}
