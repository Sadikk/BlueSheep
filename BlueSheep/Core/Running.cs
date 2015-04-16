using System;
using System.Collections.Generic;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.Types;
using BlueSheep.Core.Meal;
using BlueSheep.Core.Storage;
using BlueSheep.Interface;
using BlueSheep.Interface.Text;
using BlueSheep.Common.Data.D2o;

namespace BlueSheep.Core
{
    public class Running
    {
        #region Fields
        private int m_CurrentPetIndex;
        private Opening m_Openning;
        private Leaving m_Leaving;
        private Getting m_Getting;
        private Feeding m_Feeding;
        private LeavingDialog m_LeavingDialog;
        private bool m_OnSafe;
        private AccountUC account;
        #endregion

        #region Properties
        public Leaving Leaving
        {
            get { return m_Leaving; }
            set { m_Leaving = value; }
        }

        public int CurrentPetIndex
        {
            get { return m_CurrentPetIndex; }
            set { m_CurrentPetIndex = value; }
        }

        public Feeding Feeding
        {
            get { return m_Feeding; }
            set { m_Feeding = value; }
        }

        public LeavingDialog LeavingDialog
        {
            get { return m_LeavingDialog; }
            set { m_LeavingDialog = value; }
        }

        public bool OnSafe
        {
            get { return m_OnSafe; }
            set { m_OnSafe = value; }
        }

        public bool OnGetting { get; set; }

        public bool OnLeaving { get; set; }
        #endregion

        #region Constructeurs
        public Running()
        {
            m_Openning = null;
            m_Leaving = null;
            m_Getting = null;
            OnSafe = false;
            m_CurrentPetIndex = 0;
        }

        public Running(AccountUC accountform)
        {
            account = accountform;
            m_Openning = null;
            m_Leaving = null;
            m_Getting = null;
            m_CurrentPetIndex = 0;
        }
        #endregion

        #region Public methods
        public void Init()
        {
            if (m_CurrentPetIndex == account.petsList.Count)
            {
                account.SetNextMeal();
                account.GetNextMeal();
                return;
            }

            if ((CheckTime(account.petsList[m_CurrentPetIndex])) ||
                ((m_Feeding != null) && (m_Feeding.SecondFeeding)))
            {
                if (
                    account.petsList[m_CurrentPetIndex].Informations.Position == 8)
                {
                    Console.WriteLine();
                }

                if (account.petsList[m_CurrentPetIndex].FoodList.Count == 0)
                {
                    if (account.Safe == null)
                    {
                        NoFood();
                        return;
                    }

                    if (!m_OnSafe)
                    {
                        m_OnSafe = true;
                        m_Openning = new Opening();
                        m_Openning.Init(account);
                        return;
                    }

                    LeavingFoodToSafe();
                    return;
                }

                m_Feeding = new Feeding(account);
                m_Feeding.Init(account.petsList[m_CurrentPetIndex]);
                account.Wait(500, 1000);
                m_CurrentPetIndex++;
                return;
            }

            m_CurrentPetIndex++;
            Init();
        }

        public void LeavingFoodToSafe()
        {
            if (m_Leaving == null)
                m_Leaving = new Leaving(account);

            m_Leaving.Init();
        }

        public void GettingFoodFromSafe()
        {
            if (m_Getting == null)
                m_Getting = new Getting(account);

            m_Getting.Init();
        }

        public void CheckStatisticsUp()
        {
            foreach (Pet petModified in account.PetsModifiedList)
            {
                if (m_CurrentPetIndex >= account.petsList.Count)
                    continue;
                if (petModified.Informations.UID ==
                    account.petsList[m_CurrentPetIndex].Informations.UID)
                {
                    Pet pet = account.petsList[m_CurrentPetIndex];

                    if (pet.Effect != petModified.Effect)
                    {
                       account.Log(new ActionTextInformation("Up de caractéristique, " + petModified.Datas.Name + " " + petModified.Informations.UID + "."),4);

                        m_Feeding.SecondFeeding = true;
                    }
                    else
                        m_Feeding.SecondFeeding = false;

                    break;
                }
            }

            account.petsList = new List<Pet>();


            foreach (Core.Inventory.Item item in account.Inventory.Items)
            {
                DataClass itemData = GameData.GetDataObject(D2oFileEnum.Items, item.GID);
                if ((int)itemData.Fields["typeId"] == 18)
                {
                    Pet petToAdd = new Pet(item, itemData, account);
                    account.petsList.Add(petToAdd);
                }
            }

            account.PetsModifiedList = null;

            Init();
        }

        public void LeavingDialogWithSafe()
        {
            if (m_LeavingDialog == null)
                m_LeavingDialog = new LeavingDialog();

            m_LeavingDialog.Init(account);
        }

        public void NoFood()
        {
           account.Log(new ActionTextInformation("Aucune nourriture disponible pour " +
                                                                        BlueSheep.Common.Data.I18N.GetText((int)account.petsList[m_CurrentPetIndex].Datas
                                                                          .Fields["nameId"]) +
                                                                      "."),0);

           account.petsList[m_CurrentPetIndex].NonFeededForMissingFood = true;

           account.petsList[m_CurrentPetIndex].NextMeal = new DateTime();
            m_CurrentPetIndex++;
            Init();
        }
        #endregion

        #region Private methods
        private static bool CheckTime(Pet pet)
        {
            DateTime nextMeal = new DateTime(pet.NextMeal.Year, pet.NextMeal.Month, pet.NextMeal.Day, pet.NextMeal.Hour,
                pet.NextMeal.Minute, 0);

            return nextMeal <= DateTime.Now;
        }
        #endregion
    }
}
