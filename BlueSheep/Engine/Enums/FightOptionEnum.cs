using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueSheep.Engine.Enums
{
        public enum FightOptionEnum
        {
            /// <summary>Demande de l'aide</summary>
            AskForHelp = 3,
            /// <summary>Refuse les nouveaux membres</summary>
            Closed = 2,
            /// <summary>Réservé aux membres du groupe</summary>
            PartyOnly = 1,
            /// <summary>Mode spectateur désactivé</summary>
            Secret = 0
        }
    
}
