using BlueSheep.Common.IO;
using BlueSheep.Common.Protocol.Messages;
using BlueSheep.Engine.Types;
using BlueSheep.Interface;

namespace BlueSheep.Core.Storage
{
    public class LeavingDialog
    {
        #region Public methods
        public void Init(AccountUC account)
        {
            LeaveDialogRequestMessage leaveDialogRequestMessage = new LeaveDialogRequestMessage();

            using (BigEndianWriter writer = new BigEndianWriter())
            {
                leaveDialogRequestMessage.Serialize(writer);

                MessagePackaging messagePackaging = new MessagePackaging(writer);

                messagePackaging.Pack((int)leaveDialogRequestMessage.ProtocolID);

                account.SocketManager.Send(messagePackaging.Writer.Content);
                account.LastPacketID.Clear();
            }
        }
        #endregion
    }
}
