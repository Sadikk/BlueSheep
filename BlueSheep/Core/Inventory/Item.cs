using BlueSheep.Common.Data;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueSheep.Common.Data.D2o;

namespace BlueSheep.Core.Inventory
{
    public class Item : BlueSheep.Common.Protocol.Types.Item
    {
        public Item(List<ObjectEffect> effects, int gid, int position, int quantity, int uid, AccountUC account)
        {
            this.Effects = effects;
            this.GID = gid;
            this.Position = position;
            this.Quantity = quantity;
            this.UID = uid;
        }

        public string Description
        {
            get { return I18N.GetText((int)GameData.GetDataObject(D2oFileEnum.Items, GID).Fields["descriptionId"]); }
        }
        public List<ObjectEffect> Effects { get; private set; }
        public int GID { get; private set; }
        public string Name
        {
            get { return I18N.GetText((int)GameData.GetDataObject(D2oFileEnum.Items, GID).Fields["nameId"]); }
        }
        public string Type
        {
            get { return I18N.GetText((int)GameData.GetDataObject(D2oFileEnum.ItemTypes, typeId).Fields["nameId"]); }
        }
        public int typeId
        {
            get
            {
                DataClass d = GameData.GetDataObject(D2oFileEnum.Items, GID);
                return (int)GameData.GetDataObject(D2oFileEnum.Items, GID).Fields["typeId"];
            }
        }
        public double Price
        {
            get 
            {
                DataClass d = GameData.GetDataObject(D2oFileEnum.Items, GID);
                if (d.Fields.Keys.Contains("price"))
                    return (double)GameData.GetDataObject(D2oFileEnum.Items, GID).Fields["price"] * Quantity; 
                else
                    return 0;
            }
        }
        public int Position { get; internal set; }
        public int Quantity { get; internal set; }
        public int UID { get; private set; }
        public AccountUC Account;
    }
}
