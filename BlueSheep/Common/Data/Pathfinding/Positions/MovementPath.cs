using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

namespace BlueSheep.Data.Pathfinding.Positions
{
    public class MovementPath
    {
        // Methods
        internal void Compress()
        {
            if ((this.Cells.Count > 0))
            {
                int i = (this.Cells.Count - 1);
                while ((i > 0))
                {
                    if ((this.Cells[i].Orientation == this.Cells[(i - 1)].Orientation))
                    {
                        this.Cells.RemoveAt(i);
                        i -= 1;
                    }
                    i -= 1;
                }
            }
        }

        // Fields
        public MapPoint CellEnd;
        public List<PathElement> Cells = new List<PathElement>();
        public MapPoint CellStart;
    }
}