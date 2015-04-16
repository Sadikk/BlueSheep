using BlueSheep.Common.IO;
using BlueSheep.Common.Protocol.Messages;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Engine.Types;
using BlueSheep.Interface;

namespace BlueSheep.Core.Storage
{
    class Opening
    {
        #region Public methods
        public void Init(AccountUC account)
        {
            int skillUID = 0;

            foreach (InteractiveElementSkill skill in account.Safe.enabledSkills)
            {
                if (skill.skillId == 104)
                {
                    skillUID = skill.skillInstanceUid;

                    break;
                }
            }

            InteractiveUseRequestMessage interactiveUseRequestMessage = new InteractiveUseRequestMessage(
                account.Safe.elementId,
                skillUID);

            using (BigEndianWriter writer = new BigEndianWriter())
            {
                interactiveUseRequestMessage.Serialize(writer);
                writer.Content = account.HumanCheck.hash_function(writer.Content);
                MessagePackaging messagePackaging = new MessagePackaging(writer);

                messagePackaging.Pack((int)interactiveUseRequestMessage.ProtocolID);

                account.SocketManager.Send(messagePackaging.Writer.Content);
                account.LastPacketID.Clear();
                if (account.DebugMode.Checked)
                    account.Log(new BlueSheep.Interface.Text.DebugTextInformation("[SND] 5001 (InteractiveUseRequestMessage)"), 0);
            }
        }
        #endregion
    }
}
