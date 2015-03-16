using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlueSheep.Common.Types
{
    public class Lot
    {
        public int GID;
        public string Name = "Unknown";
        public int DefaultPrice;
        public int MinimalPrice;
        public int Quantity;

        public Lot(int _GID, string _Nom, int _PrixMini, int _PrixDefault, int _Quantity)
        {
            this.GID = _GID;
            this.Name = _Nom;
            this.DefaultPrice = _PrixMini;
            this.MinimalPrice = _PrixDefault;
            this.Quantity = _Quantity;
        }
    }
}
