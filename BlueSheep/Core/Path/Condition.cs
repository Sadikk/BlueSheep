using BlueSheep.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlueSheep.Core.Path
{
    class Condition
    {
        #region Fields
        ConditionEnum m_Cond;
        object m_delta;
        char m_operateur;
        AccountUC account;
        #endregion

        #region Constructors
        public Condition(ConditionEnum condition, object delta, char operateur, AccountUC Account)
        {
            m_Cond = condition;
            m_delta = delta;
            m_operateur = operateur;
            account = Account;
        }
        #endregion

        #region Public Methods
        public bool CheckCondition()
        {
            switch (m_Cond)
            {
                case ConditionEnum.Null:
                    return true;
                case ConditionEnum.LastMapId:
                    if (account.Map.LastMapId == Convert.ToInt32(m_delta))
                        return true;
                    else
                        return false;
                case ConditionEnum.Level:
                    switch (m_operateur)
                    {
                        case '<':
                            if (account.CharacterBaseInformations.level < Convert.ToInt32(m_delta))
                                return true;
                            else
                                return false;
                        case '>':
                            if ((int)account.CharacterBaseInformations.level > Convert.ToInt32(m_delta))
                                return true;
                            else
                                return false;
                        case '=':
                            if (account.CharacterBaseInformations.level == Convert.ToInt32(m_delta))
                                return true;
                            else
                                return false;
                    }
                    return false;
                case ConditionEnum.Pods:
                    switch (m_operateur)
                    {
                        case '<':
                            if (account.Inventory.weight < Convert.ToInt32(m_delta))
                                return true;
                            else
                                return false;
                        case '>':
                            if (account.Inventory.weight > Convert.ToInt32(m_delta))
                                return true;
                            else
                                return false;
                        case '=':
                            if (account.Inventory.weight == Convert.ToInt32(m_delta))
                                return true;
                            else
                                return false;
                    }
                    return false;
                case ConditionEnum.PodsPercent:
                    if (m_delta == "%PODS%")
                        m_delta = account.GestItemsUC.NUDBank.Value;
                    switch (m_operateur)
                    {
                        case '<':
                            if (account.Inventory.weightPercent < Convert.ToInt32(m_delta))
                                return true;
                            else
                                return false;
                        case '>':
                            if (account.Inventory.weightPercent > Convert.ToInt32(m_delta))
                                return true;
                            else
                                return false;
                        case '=':
                            if (account.Inventory.weightPercent == Convert.ToInt32(m_delta))
                                return true;
                            else
                                return false;
                    }
                    return false;


            }
            return false;
        }
        #endregion


    }
}
