using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlueSheep.Data.Pathfinding
{
    public class CellInfo
    {
        // Methods
        internal CellInfo(double heuristic, int[] parent, bool opened, bool closed, int x, int y)
        {
            this.Heuristic = heuristic;
            this.Parent = parent;
            this.Opened = opened;
            this.Closed = closed;
            this.X = x;
            this.Y = y;
        }

        public double Heuristic { get; set; }
        public int[] Parent { get; set; }
        public bool Opened { get; set; }
        public bool Closed { get; set; }
        public int MovementCost { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

    }

}
