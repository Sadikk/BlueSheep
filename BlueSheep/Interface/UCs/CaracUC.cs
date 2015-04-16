using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BlueSheep.Common.Protocol.Messages;
using BlueSheep.Interface.Text;
using BlueSheep.Common.Data.D2o;
using System.Collections;

namespace BlueSheep.Interface.UCs
{
    public partial class CaracUC : MetroFramework.Controls.MetroUserControl
    {
        #region Fields
        AccountUC account;
        private int m_count;
        #endregion

        private delegate void DelegLabel(string text, Label lab);

        #region Constructors
        public CaracUC(AccountUC Account)
        {
            InitializeComponent();
            account = Account;
        }
        #endregion

        #region Public Methods
        public void Init()
        {
            Invoke(new DelegLabel(ModLabel), Convert.ToString(account.CharacterStats.vitality.@base + account.CharacterStats.vitality.additionnal + account.CharacterStats.vitality.objectsAndMountBonus), VitaLb);
            Invoke(new DelegLabel(ModLabel), Convert.ToString(account.CharacterStats.wisdom.@base + account.CharacterStats.wisdom.additionnal + account.CharacterStats.wisdom.objectsAndMountBonus), WisdomLb);
            Invoke(new DelegLabel(ModLabel), Convert.ToString(account.CharacterStats.strength.@base + account.CharacterStats.strength.additionnal + account.CharacterStats.strength.objectsAndMountBonus), StreLb);
            Invoke(new DelegLabel(ModLabel), Convert.ToString(account.CharacterStats.intelligence.@base + account.CharacterStats.intelligence.additionnal + account.CharacterStats.intelligence.objectsAndMountBonus), InteLb);
            Invoke(new DelegLabel(ModLabel), Convert.ToString(account.CharacterStats.chance.@base + account.CharacterStats.chance.additionnal + account.CharacterStats.chance.objectsAndMountBonus), LuckLb);
            Invoke(new DelegLabel(ModLabel),  Convert.ToString(account.CharacterStats.agility.@base + account.CharacterStats.agility.additionnal + account.CharacterStats.agility.objectsAndMountBonus), AgiLb);
            Invoke(new DelegLabel(ModLabel), Convert.ToString(account.CharacterStats.actionPoints.@base + account.CharacterStats.actionPoints.additionnal + account.CharacterStats.actionPoints.objectsAndMountBonus), APLb);
            Invoke(new DelegLabel(ModLabel), Convert.ToString(account.CharacterStats.movementPoints.@base + account.CharacterStats.movementPoints.additionnal + account.CharacterStats.movementPoints.objectsAndMountBonus),MpLb);
            Invoke(new DelegLabel(ModLabel), Convert.ToString(account.CharacterStats.statsPoints), AvailabPtLb);
            //GetBoost(10);
        }

        

        public void UpAuto()
        {
            if (VitaRb.Checked)
            {
                while (Convert.ToInt32(AvailabPtLb.Text) != 0)
                {
                    UpStat(11, 1);
                }
            }
            else
                account.Log(new ErrorTextInformation("L'auto-up ne gère pas les paliers et a donc été désactivé pour des raisons de sécurité (ban 2h sinon)."), 0);
            //else if (WisRb.Checked)
            //{
            //    while (Convert.ToInt32(AvailabPtLb.Text) % 3 == 0)
            //        UpStat(12, 3);
            //}
            //else if (StreRb.Checked)
            //{
            //    while (Convert.ToInt32(AvailabPtLb.Text) != 0)
            //        UpStat(10, 1);
            //}
            //else if (InteRb.Checked)
            //{
            //    while (Convert.ToInt32(AvailabPtLb.Text) != 0)
            //        UpStat(15, 1);
            //}
            //else if (LuckRb.Checked)
            //{
            //    while (Convert.ToInt32(AvailabPtLb.Text) != 0)
            //        UpStat(13, 1);
            //}
            //else if (AgiRb.Checked)
            //{
            //    while (Convert.ToInt32(AvailabPtLb.Text) != 0)
            //        UpStat(14, 1);
            //}
                

        }

        public void DecreaseAvailablePoints(int n)
        {
            AvailabPtLb.Text = Convert.ToString(Convert.ToInt32(AvailabPtLb.Text) - n);
        }
        #endregion

        #region Interface Methods
        private void sadikButton1_Click(object sender, EventArgs e)
        {
            UpStat(11,1);
        }

        private void sadikButton2_Click(object sender, EventArgs e)
        {
            UpStat(12,3);
        }

        private void sadikButton3_Click(object sender, EventArgs e)
        {
            UpStat(10,1);
        }

        private void sadikButton4_Click(object sender, EventArgs e)
        {
            UpStat(15,1);
        }

        private void sadikButton5_Click(object sender, EventArgs e)
        {
            UpStat(13, 1);
        }

        private void sadikButton6_Click(object sender, EventArgs e)
        {
            UpStat(14, 1);
        }
        #endregion

        #region Private Methods
        private void UpStat(int statId, int boost)
        {
            StatsUpgradeRequestMessage msg = new StatsUpgradeRequestMessage(false, (sbyte)statId, (short)boost);
            account.SocketManager.Send(msg);          
        }

        private void ModLabel(string content, Label lab)
        {
            lab.Text = content;
        }

        public int GetBoost(int statId)
        {
            DataClass d = GameData.GetDataObject(D2oFileEnum.Breeds, account.CharacterBaseInformations.breed);
            switch (statId)
            {
                case 10:
                    ArrayList o =(ArrayList)d.Fields["statsPointsForStrength"];
                    break;
                case 11:
                    break;
                case 12:
                    break;
                case 13:
                    break;
                case 14:
                    break;
                case 15:
                    break;
            }

            return 1;
        }

        #endregion

        
    }
}
