using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using BlueSheep.Data.Pathfinding.Positions;

namespace BlueSheep.Data.Pathfinding
{
    public class MapMovementAdapter
    {
        // Methods
        public static MovementPath GetClientMovement(List<UInt32> Keys)
        {
            MovementPath path = new MovementPath();
            PathElement element = null;
            int num = 0;
            int num2 = 0;
            foreach (int num2_loopVariable in Keys)
            {
                num2 = num2_loopVariable;
                MapPoint point = new MapPoint((num2 & 4095));
                PathElement item = new PathElement { Cell = point };
                if ((num == 0))
                {
                    path.CellStart = point;
                }
                else
                {
                    element.Orientation = element.Cell.OrientationTo(item.Cell);
                }
                if ((num == (Keys.Count - 1)))
                {
                    path.CellEnd = point;
                }
                path.Cells.Add(item);
                element = item;
                num += 1;
            }
            return path;
        }

        public static List<uint> GetServerMovement(MovementPath Path)
		{
			List<uint> list = new List<uint>();
			int orientation = 0;
			foreach (PathElement element in Path.Cells) {
				orientation = element.Orientation;
                list.Add(Convert.ToUInt32(((orientation & 7) << 12) | (element.Cell.CellId & 4095)));
			}
			list.Add(Convert.ToUInt32(((orientation & 7) << 12) | (Path.CellEnd.CellId & 4095)));
			return list;
		}
    }
}