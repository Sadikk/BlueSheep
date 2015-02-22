using System;
using System.Collections.Generic;
using BlueSheep.Common.Constants;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Interface;
using BlueSheep.Common.Data.D2o;

namespace BlueSheep.Common.Types
{
    public class Pet
    {
        #region Properties
        public Core.Inventory.Item Informations { get; set; }

        public DataClass Datas { get; set; }

        public AccountUC account { get; set; }

        public List<Food> FoodList { get; set; }

        public int MealInterval { get; set; }

        public DateTime NextMeal { get; set; }

        public string Effect { get; set; }

        public bool NonFeededForMissingFood { get; set; }
        #endregion

        #region Constructeurs
        public Pet(Core.Inventory.Item informations, DataClass datas, AccountUC uc)
        {
            FoodList = new List<Food>();
            Informations = informations;
            Datas = datas;
            account = uc;
            Set();
        }
        #endregion

        #region Public methods
        public void Set()
        {
            MealInterval = MealIntervals.GetMealIntervals(Datas.Name);

            SetFood();
            SetNextMeal();
            SetEffect();
        }

        public void SetFood()
        {
            FoodList.Clear();
            string name = BlueSheep.Common.Data.I18N.GetText((int)Datas.Fields["nameId"]);
            List<int> foodIndex = Foods.GetFoods(name);
            if (foodIndex == null)
                foodIndex = Foods.GetFoods(Informations.GID);

            foreach (Core.Inventory.Item item in account.Inventory.Items)
            {
                if (foodIndex != null && foodIndex.Contains(item.GID))
                {
                    FoodList.Add(new Food(item,GameData.GetDataObject(D2oFileEnum.Items, item.GID)));
                }
            }
        }

        public void SetNextMeal()
        {
            NextMeal = new DateTime();

            foreach (ObjectEffect effect in Informations.Effects)
            {
                if (effect.actionId == 808)
                {
                    ObjectEffectDate effectDate = (ObjectEffectDate)effect;

                    int month;
                    int year = 0;

                    if (effectDate.month == 12)
                    {
                        month = 1;
                        year = effectDate.year + 1;
                    }
                    else
                    {
                        month = effectDate.month + 1;
                        year = effectDate.year;
                    }

                    year += 1370;

                    NextMeal = new DateTime(year, month, effectDate.day, effectDate.hour,
                        effectDate.minute, 0);

                    break;
                }
            }

            NextMeal = NextMeal.AddHours(MealInterval);
        }

        public void SetEffect()
        {
            List<int> effectIndex = Effects.GetEffects(BlueSheep.Common.Data.I18N.GetText((int)Datas.Fields["nameId"]));
            Effect = 0.ToString();

            foreach (ObjectEffect effect in Informations.Effects)
            {
                if (effectIndex != null && effectIndex.Contains(effect.actionId))
                {
                    if (effect is ObjectEffectInteger)
                    {
                        ObjectEffectInteger effectInteger = (ObjectEffectInteger) effect;
                        DataClass effectObject = GameData.GetDataObject(D2oFileEnum.Effects, effect.actionId);
                        //Effect effectEffect = (Effect) effectObject;
                        string effectString = BlueSheep.Common.Data.I18N.GetText((int)effectObject.Fields["descriptionId"]);
                        effectString = effectString.TrimStart('#');
                        int index = effectString.IndexOf("#", StringComparison.Ordinal);
                        index += 2;
                        effectString = effectString.Remove(0, index);
                        Effect = (string)effectObject.Fields["operator"] + effectInteger.value + effectString;
                        break;
                    }
                }
            }
        }
        #endregion
    }
}
