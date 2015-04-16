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
            account.SocketManager.Send(leaveDialogRequestMessage);
            account.LastPacketID.Clear();

        }
        #endregion
    }
}
