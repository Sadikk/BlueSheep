using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlueSheep.Common.Types
{
    [Serializable()]
    public class AccountConfig
    {
        public string PathDownBtText; //Strip down bt trajet
        public decimal NUDVerboseValue; //réglage de la verbosité

        #region Fight
        public decimal RegenChoiceValue; //réglage de la regen
        public bool WithItemsSetBoxChecked; //avec/sans equip rapide
        public decimal PresetStartUpDValue; //équipement rapide entrée en combat
        public decimal PresetEndUpDValue; //équipement rapide sortie de combat
        public decimal nudMinMonstersNumberValue; // nombre mini de mobs
        public decimal nudMaxMonstersNumberValue; //nombre maxi de mobs
        public decimal nudMinMonstersLevelValue; //level mini des mobs
        public decimal nudMaxMonstersLevelValue; //level maxi des mobs

        public int xpDay1; // Sauvegarde de l'xp
        public int xpDay2;
        public int xpDay3;
        public int xpDay4;
        public int xpDay5;
        public int xpDay6;
        public int xpDay7;
        #endregion

        #region House
        public decimal MaxPriceValue; //prix maxi de la maison
        public string PhraseADireText; //phrase à dire après l'achat
        #endregion

        #region Flood
        public string FloodContentRBoxText; // phrase à flooder
        public bool IsRandomingSmileyBoxChecked; //rajout de smiley ?
        public bool IsRandomingNumberBoxChecked; //rajout de nombres ?
        public decimal NUDFloodValue; //intervalle de flood
        public bool CommerceBoxChecked; //canaux de flood
        public bool RecrutementBoxChecked;
        public bool GeneralBoxChecked;
        public bool PrivateEnterBoxChecked;
        #endregion

        public AccountConfig()
        { }

    }
}
