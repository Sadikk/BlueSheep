using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BlueSheep.Common.IO;

namespace BlueSheep.Data.D2p
{
    public class Layer
    {
        // Methods
        internal void Init(BigEndianReader Reader, int MapVersion)
        {
            this.LayerId = Reader.ReadInt();
            this.CellsCount = Reader.ReadShort();
            int cellsCount = this.CellsCount;
            int i = 1;
            while ((i <= cellsCount))
            {
                Cell item = new Cell();
                item.Init(Reader, MapVersion);
                this.Cells.Add(item);
                i += 1;
            }
        }

        // Fields
        public List<Cell> Cells = new List<Cell>();
        public int CellsCount;
        public int LayerId;
    }
}
