using BlueSheep.Common.Data.D2o;
using BlueSheep.Common.IO;
using BlueSheep.Common.Protocol.Messages;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Engine.Types;
using BlueSheep.Interface;
using BlueSheep.Interface.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueSheep.Core.Inventory
{
    public class Inventory
    {
        #region Fields
        public int kamas;
        public int maxWeight;
        public int weight;
        public AccountUC Account;
        public List<Item> Items;
        public int weightPercent
        {
            get
            {
                double per = (double)this.weight / (double)this.maxWeight;
                return (int)(per * 100);
            }

        }
        #endregion

        #region Constructors
        public Inventory(AccountUC account)
        {
            Account = account;

            Items = new List<Item>();
            kamas = 0;
            maxWeight = 0;
            weight = 0;
        }
        #endregion

        #region Public Methods
        public bool HasFishingRod
        {
            get
            {
                    Item weapon = Items.FirstOrDefault(i => i.Position == (int)InventoryPositionEnum.Weapon);
                    if (weapon == null)
                        return false;

                    DataClass fishingRod = GameData.GetDataObject(D2oFileEnum.Items, weapon.GID);
                    return (int)fishingRod.Fields["typeId"] == 20  && (int)fishingRod.Fields["useAnimationId"] == 18 ? true : false;
            }
        }

        public int WeaponRange
        {
            get
            {
                Item weapon = Items.FirstOrDefault(i => i.Position == (int)InventoryPositionEnum.Weapon);
                return weapon != null ? (int)GameData.GetDataObject(D2oFileEnum.Items,weapon.GID).Fields["range"] : 1;
            }
        }

        public Item Weapon
        {
            get
            {
                Item weapon = Items.FirstOrDefault(i => i.Position == (int)InventoryPositionEnum.Weapon);
                return weapon;
            }
        }

        public void DeleteItem(int uid, int quantity)
        {
            if (ItemExists(uid) && ItemQuantity(uid) > 0)
            {
                ObjectDeleteMessage msg = new ObjectDeleteMessage(uid, quantity);
                Account.SocketManager.Send(msg);
                Account.Log(new ActionTextInformation("Suppression de " + GetItemFromUID(uid).Name + "(x" + quantity + ")."), 2);
            }
            
        }

        public void SendItemToShop(int uid, int quantity, int price)
        {
            if (ItemExists(uid) && ItemQuantity(uid) > 0)
            {
                ExchangeRequestOnShopStockMessage packetshop = new ExchangeRequestOnShopStockMessage();
                Account.SocketManager.Send(packetshop);
                ExchangeObjectMovePricedMessage msg = new ExchangeObjectMovePricedMessage(uid, quantity, price);
                Account.SocketManager.Send(msg);
                Account.Log(new ActionTextInformation("Ajout de " + Account.Inventory.GetItemFromUID(uid).Name + "(x " + quantity + ") dans le magasin magasin au prix de : " + price + " Kamas"), 2);
                LeaveDialogRequestMessage packetleave = new LeaveDialogRequestMessage();
                Account.SocketManager.Send(packetleave);
            }

        }

        public void DropItem(int uid, int quantity)
        {
            if (ItemExists(uid) && ItemQuantity(uid) > 0)
            {
                    ObjectDropMessage msg = new ObjectDropMessage(uid, quantity);
                    Account.SocketManager.Send(msg);
                    Account.Log(new ActionTextInformation("Jet de " + GetItemFromUID(uid).Name + "(x" + quantity + ")."), 2);               
            }
        }

        public void EquipItem(int uid)
        {
            if (ItemExists(uid) && ItemQuantity(uid) > 0)
            {
                ObjectSetPositionMessage msg = new ObjectSetPositionMessage(uid, (byte)GetPosition(GetItemFromUID(uid).Type), 1);
                Account.SocketManager.Send(msg);
                Account.Log(new ActionTextInformation(GetItemFromUID(uid).Name + " équipé."), 2);

            }
        }

        public Item GetItemFromName(string name)
        {
            return Items.FirstOrDefault(i => i.Name == name);
        }

        public Item GetItemFromGID(int gid)
        {
                return Items.FirstOrDefault(i => i.GID == gid);
        }

        public Item GetItemFromUID(int uid)
        {
            return Items.FirstOrDefault(i => i.UID == uid);
        }

        public void UseItem(int uid)
        {
            if (!ItemExists(uid))
                return;

            ObjectUseMessage msg = new ObjectUseMessage(uid);
            Account.SocketManager.Send(msg);
            Account.Log(new BotTextInformation("Utilisation de : " + GetItemFromUID(uid).Name), 3);
        }

        public void TransferItems(List<int> items)
        {
            foreach (int i in items)
            {
                Account.Log(new ActionTextInformation("Objet transféré : " + GetItemFromUID(i).Name + " (x" + GetItemFromUID(i).Quantity + ")."), 2);
            }
            ExchangeObjectTransfertListFromInvMessage msg = new ExchangeObjectTransfertListFromInvMessage(items.ToArray());
            Account.SocketManager.Send(msg);
            Account.Log(new BotTextInformation("Trajet : Tous les objets transférés."), 3);
            //Account.Npc.CloseDialog();
            //Don't need, dialog closed with the GetItems() method.
            //Account.Busy = false;
        }

        public void GetItems(List<int> items)
        {
            if (items.Count > 0)
            {
                foreach (int i in items)
                {
                    Account.Log(new ActionTextInformation("Objet pris du coffre : " + GetItemFromUID(i).Name + " (x" + GetItemFromUID(i).Quantity + ")."), 2);
                }
                ExchangeObjectTransfertListToInvMessage msg = new ExchangeObjectTransfertListToInvMessage(items.ToArray());
                Account.SocketManager.Send(msg);
                Account.Log(new BotTextInformation("Trajet : Tous les objets pris du coffre."), 3);
            }
            Account.Npc.CloseDialog();   
        }

        public void ExchangeReady()
        {
            Account.SocketManager.Send(new ExchangeReadyMessage(true, 1));
            Account.Log(new ActionTextInformation("Echange prêt"), 5);
        }

        public void RequestExchange(string name)
        {
            int targetId = Account.MapData.Players.Find(p=> p.name == name).contextualId;
            if (targetId != 0)
            {
                Account.SocketManager.Send(new ExchangePlayerRequestMessage(1, targetId));
                Account.Log(new ActionTextInformation("Demande d'échange"), 5);
            }
            else
            {
                Account.Log(new ErrorTextInformation("Le joueur " + name + " ne semble pas être sur la map."), 5);
            }
        }

        public void AcceptExchange()
        {
            Account.SocketManager.Send(new ExchangeAcceptMessage());
            Account.Log(new ActionTextInformation("Echange accepté"), 5);
        }
#endregion

        #region Private Methods 
        private bool ItemExists(int uid)
        {
                return Items.FirstOrDefault(i => i.UID == uid) != null ? true : false;
        }

        private int ItemQuantity(int uid)
        {
                return Items.FirstOrDefault(i => i.UID == uid) != null ? Items.First(i => i.UID == uid).Quantity : 0;
        }

        private int GetPosition(string TypeName)
        {
            switch (TypeName)
            {
                case "Chapeau":
                    return (int)InventoryPositionEnum.Hat;
                case "Cape":
                    return (int)InventoryPositionEnum.Cape;
                case "Amulette":
                    return (int)InventoryPositionEnum.Amulet;
                case "Ceinture":
                    return (int)InventoryPositionEnum.Belt;
                case "Anneau":
                    return (int)InventoryPositionEnum.RingRight;
                case "Bottes":
                    return (int)InventoryPositionEnum.Boots;
                case "Familier":
                    return (int)InventoryPositionEnum.Pets;
                case "Dofus":
                case "Trophée":
                    return (int)InventoryPositionEnum.Dofus6;
                case "Marteau":
                case "Bâton":
                case "Baguette":
                case "Epée":
                case "Hache":
                case "Faux":
                case "Pioche":
                    return (int)InventoryPositionEnum.Weapon;
                case "Bouclier":
                    return (int)InventoryPositionEnum.Shield;         
                default:
                    return 0;
            }
        }
        #endregion
    }
}
