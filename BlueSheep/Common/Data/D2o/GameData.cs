using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace BlueSheep.Common.Data.D2o
{
    public class GameData
    {
        // Methods
        /// <summary>Récupère les données d'un élément du jeu grâce au nom du conteneur et à l'identifiant de l'élément</summary>
        /// <param name="ModuleName">Nom du conteneur (sans le .D2o)</param>
        /// <param name="Id">Identifiant de l'élément</param>
        /// <returns>Données de l'élément</returns>
        public static DataClass GetDataObject(D2oFileEnum ModuleName, int Id)
        {
            string File = ModuleName.ToString();
            lock (GameData.CheckLock)
            {
                if (GameData.FileName_Data.ContainsKey(File))
                {
                    return GameData.FileName_Data[File].DataObject(File, Id);
                }
                return null;
            }
        }

        /// <summary>Récupère toutes les données du conteneur</summary>
        /// <param name="ModuleName">Nom du conteneur (sans le .D2o)</param>
        /// <returns>Les données du conteneur</returns>
        public static DataClass[] GetDataObjects(D2oFileEnum ModuleName)
        {
            string File = ModuleName.ToString();
            lock (GameData.CheckLock)
            {
                return GameData.FileName_Data[File].DataObjects(File);
            }
        }

        /// <summary>Initialise les données</summary>
        /// <param name="DirectoryInit">Chemin vers le répertoire contenant les .d2o</param>
        /// <remarks>Cette methode est executée automatiquement au lancement de Rebirth</remarks>
        public static void Init(string DirectoryInit)
		{
			GameData.FileName_Data = new Dictionary<string, D2oData>();
			foreach (string fichier_loopVariable in Directory.GetFiles(DirectoryInit)) {
				string fichier = fichier_loopVariable;
				FileInfo info = new FileInfo(fichier);
				if ((info.Extension.ToUpper() == ".D2O")) {
					D2oData D2oData = new D2oData(fichier);
					GameData.FileName_Data.Add(Path.GetFileNameWithoutExtension(fichier), D2oData);
				}
			}
		}

        // Fields
        static internal Dictionary<string, D2oData> FileName_Data;
        private static object CheckLock = RuntimeHelpers.GetObjectValue(new object());
    }
}