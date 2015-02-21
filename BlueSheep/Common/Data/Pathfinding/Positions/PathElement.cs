using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

namespace BlueSheep.Data.Pathfinding.Positions
{
    public class PathElement
    {
        // Fields
        public MapPoint Cell;
        public int Orientation = -1;
    }
}