using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BlueSheep.Engine.Enums;
using BlueSheep.Interface.Text;
using BlueSheep.Core.Inventory;

namespace BlueSheep.Interface.UCs
{
    public partial class RegenUC : MetroFramework.Controls.MetroUserControl
    {
        #region Fields
        private AccountUC m_Account;
        #endregion

        #region Constructors
        public RegenUC(AccountUC Account)
        {
            InitializeComponent();
            m_Account = Account;
            Init();
        }
        #endregion

        #region Public Methods
        public void PulseRegen()
        {
            if (((m_Account.CharacterStats.lifePoints / m_Account.CharacterStats.maxLifePoints) * 100) < m_Account.RegenChoice.Value)
            {
                m_Account.SetStatus(Status.Regenerating);
                List<Item> items = GetRegenItems();
                if (items.Count > 0)
                {
                    if (UseItems(items))
                    {
                        m_Account.Wait(500, 1000);
                        PulseRegen();
                        return;
                    }
                }             
                int maxLife = Convert.ToInt32(m_Account.CharacterStats.maxLifePoints);
                int life = Convert.ToInt32(m_Account.CharacterStats.lifePoints);
                int time = Convert.ToInt32(Math.Round(Convert.ToDecimal(maxLife - life) / 2));
                BlueSheep.Common.Protocol.Messages.EmotePlayRequestMessage msg = new BlueSheep.Common.Protocol.Messages.EmotePlayRequestMessage(1);
                m_Account.SocketManager.Send(msg);
                m_Account.Log(new GeneralTextInformation(String.Format("Régénération pendant {0} secondes.", time)), 2);
                m_Account.Wait(time * 1000, (time + 1) * 1000);
                if (m_Account.Path != null && m_Account.Path.Launched)
                    m_Account.Path.ParsePath();
            }         
        }

        public void RefreshQuantity()
        {
            for (int i = 0; i < LVItems.Items.Count; i++)
            {
                LVItems.Items[i].SubItems[1].Text = m_Account.Inventory.GetItemFromName(LVItems.Items[i].SubItems[0].Text).Quantity.ToString();
                LVItems.Invalidate();
            }
        }
        #endregion

        #region Private methods
        private List<Item> GetRegenItems()
        {
            List<Item> result = new List<Item>();
            foreach (ListViewItem i in LVItems.Items)
            {
                if (int.Parse(i.SubItems[1].Text) > 0)
                    result.Add(m_Account.Inventory.GetItemFromName(i.SubItems[0].Text));
            }
            return result;
        }

        private void Init()
        {
            LVItems.Columns.Add("Nom", -2, HorizontalAlignment.Center);
            LVItems.Columns.Add("Quantité", -2, HorizontalAlignment.Center);
        }

        private bool UseItems(List<Item> items)
        {
            foreach (Item i in items)
            {
                if (i.Quantity <= 0)
                    continue;
                else
                {
                    m_Account.Inventory.UseItem(i.UID);
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region UI Methods
        private void LVGestItems_TouchPressed(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Delete)
            {
                for (int i = 0; i < LVItems.Items.Count; i++)
                {
                    if (LVItems.Items[i].Selected)
                        LVItems.Items.RemoveAt(i);
                }
            }
        }
        #endregion
    }
}
