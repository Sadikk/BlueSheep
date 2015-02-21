using BlueSheep.Common.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlueSheep.Data.D2p
{
    public class CellData
    {
        // Methods
        internal void Init(BigEndianReader Reader, int MapVersion)
        {
            this.Floor = (Reader.ReadSByte() * 10);
            if ((this.Floor != -1280))
            {
                this.LosMov = Reader.ReadSByte();
                this.Speed = Reader.ReadSByte();
                this.MapChangeData = Reader.ReadByte();
                if ((MapVersion > 5))
                {
                    this.MoveZone = Reader.ReadByte();
                }
                if ((MapVersion > 7))
                {
                    int tmp = Reader.ReadSByte();
                    Arrow = 15 & tmp;
                }
            }
        }

        public bool Blue()
        {
            return (((this.LosMov & 16) >> 4) == 1);
        }

        public bool FarmCell()
        {
            return (((this.LosMov & 32) >> 5) == 1);
        }

        public bool Los()
        {
            return (((this.LosMov & 2) >> 1) == 1);
        }

        public bool Mov()
        {
            return ((this.LosMov & 1) == 1);
        }

        public bool NonWalkableDuringFight()
        {
            return (((this.LosMov & 4) >> 2) == 1);
        }

        public bool Red()
        {
            return (((this.LosMov & 8) >> 3) == 1);
        }

        public bool Visible()
        {
            return (((this.LosMov & 64) >> 6) == 1);
        }

        public bool TopArrow()
        {
            return ((this.Arrow & 1) != 0);
        }

        public bool BottomArrow()
        {
            return ((this.Arrow & 2) != 0);
        }

        public bool RightArrow()
        {
            return ((this.Arrow & 4) != 0);
        }

        public bool LeftArrow()
        {
            return ((this.Arrow & 8) != 0);
        }

        // Fields
        public int Floor;
        public int LosMov;
        public int MapChangeData;
        public int MoveZone;
        public int Speed;
        public int Arrow = 0;
    }
}
