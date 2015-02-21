using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueSheep.Engine.Enums
{
        public enum SpellInabilityReason
        {
            /// <summary>Les points d'action sont insuffisants</summary>
            ActionPoints = 3,
            /// <summary>Le temps de recharge du sort est actif</summary>
            Cooldown = 2,
            /// <summary>L'état n'est pas autorisé</summary>
            ForbiddenState = 13,
            /// <summary>La ligne de vue n'est pas dégagée</summary>
            LineOfSight = 8,
            /// <summary>La distance maximum du sort a été dépassée</summary>
            MaxRange = 5,
            /// <summary>La distance minimum du sort a été dépassée</summary>
            MinRange = 6,
            /// <summary>La cellule doit être vide</summary>
            NeedFreeCell = 10,
            /// <summary>La cellule doit être prise</summary>
            NeedTakenCell = 11,
            /// <summary>Le sort peut être lancé</summary>
            None = 0,
            /// <summary>Le sort n'est pas lancé en ligne alors qu'il le devrait</summary>
            NotInLine = 7,
            /// <summary>L'état requis est manquant</summary>
            RequiredState = 12,
            /// <summary>Le nombre d'invoquation est dépassé</summary>
            TooManyInvocations = 9,
            /// <summary>Le sort a été lancé trop de fois ce tour</summary>
            TooManyLaunch = 1,
            /// <summary>Le sort a été lancé trop de fois sur cette cellule</summary>
            TooManyLaunchOnCell = 4,
            /// <summary>Le sort ne peut pas être lancé pour une raison inconnue</summary>
            Unknown = 16,
            /// <summary>Le personnage ne possède pas le sort indiqué</summary>
            UnknownSpell = 15,
            /// <summary>Le sort n'est pas lancé en diagonale alors qu'il le devrait</summary>
            NotInDiagonal = 14
        
    }
}
