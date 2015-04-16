using System.Collections.Generic;
using BlueSheep.Common.Constants;
using BlueSheep.Common.IO;
using BlueSheep.Common.Protocol.Messages;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Engine.Types;
using BlueSheep.Interface;
using BlueSheep.Common.Data.D2o;

namespace BlueSheep.Core.Storage
{
    class Getting
    {
        #region Fields
        public AccountUC account;
        #endregion

        #region Public methods

        public Getting(AccountUC accountform)
        {
            account = accountform;
        }

        public void Init()
        {
            account.Running.OnGetting = true;

            List<int> foodIndex = Foods.GetFoods(account.petsList[account.Running.CurrentPetIndex].Informations.GID);

            ObjectItem objectItem = null;

            foreach (ObjectItem item1 in account.SafeItems)
            {
                if (foodIndex.Contains(item1.ObjectGID))
                    objectItem = item1;
            }

            if (objectItem == null)
            {
                account.Running.NoFood();
                return;
            }

            int abbleWeight = account.Pods.WeightMax -
                              account.Pods.Weight - 1;
            //return;
            DataClass item = GameData.GetDataObject(D2oFileEnum.Items, objectItem.ObjectGID);
            int objectWeight = (int)item.Fields["realWeight"];
            int quantity = abbleWeight / objectWeight;

            if (objectItem.Quantity < quantity)
                quantity = objectItem.Quantity;

            ExchangeObjectMoveMessage exchangeObjectMoveMessage =
                new ExchangeObjectMoveMessage(objectItem.ObjectUID, -quantity);
            account.SocketManager.Send(exchangeObjectMoveMessage);
            account.LastPacketID.Clear();

        }
        #endregion
    }
}
