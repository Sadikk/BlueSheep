using BlueSheep.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlueSheep.Core.Path
{
    class PathCondition
    {
        #region Fields
        PathConditionEnum m_Cond;
        object m_delta;
        char m_operateur;
        AccountUC account;
        #endregion

        #region Constructors
        public PathCondition(PathConditionEnum condition, object delta, char operateur, AccountUC Account)
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
                case PathConditionEnum.Null:
                    return true;
                case PathConditionEnum.LastMapId:
                    if (account.MapData.LastMapId == Convert.ToInt32(m_delta))
                        return true;
                    else
                        return false;
                case PathConditionEnum.Level:
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
                        default:
                            return false;
                    }
                    return false;
                case PathConditionEnum.Pods:
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
                        default:
                            return false;
                    }
                    return false;
                case PathConditionEnum.Alive:
                    switch (m_operateur)
                    {
                        case '=':
                            if ((account.CharacterStats.energyPoints <= 0) == Convert.ToBoolean(m_delta))
                                return true;
                            else
                                return false;
                        default:
                            return false;
                    }
                case PathConditionEnum.PodsPercent:
                    if (Convert.ToString(m_delta) == "%PODS%")
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
                        default:
                            return false;
                    }
                    return false;


            }
            return false;
        }
        #endregion
    }
}
