using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using BlueSheep.Data.Pathfinding.Positions;
using BlueSheep.Data.D2p;
using BlueSheep.Core.Map;

namespace BlueSheep.Data.Pathfinding
{
    internal sealed class SimpleCellInfo
    {
        // Methods
        internal SimpleCellInfo(MapPoint OriginPoint)
        {
            this.int_0 = 0;
            this.int_1 = 0;
            this.v_OriginPoint = OriginPoint;
            this.class13_0 = null;
            this.int_0 = 0;
        }

        internal SimpleCellInfo(MapData MapData, MapPoint OriginPoint, SimpleCellInfo class13_1, MapPoint mapPoint_2)
        {
            this.int_0 = 0;
            this.int_1 = 0;
            this.v_OriginPoint = OriginPoint;
            this.class13_0 = class13_1;
            this.int_1 = (mapPoint_2.DistanceToCell(OriginPoint) * 5);
            this.int_0 = (class13_1.int_0 + 10);
            if (((class13_1.class13_0 != null)))
            {
                int num2 = class13_1.class13_0.v_OriginPoint.OrientationTo(class13_1.v_OriginPoint);
                int num3 = class13_1.v_OriginPoint.OrientationTo(OriginPoint);
                int num = Math.Abs(Convert.ToInt32((num2 - num3)));
                if ((num != 0))
                {
                    this.int_0 = (this.int_0 + 5);
                    if (((num != 1) && (num != 7)))
                    {
                        this.int_0 = (this.int_0 + 50);
                    }
                }
                if ((MapData.Data.Cells[OriginPoint.CellId].Speed == 2))
                {
                    this.int_0 = (this.int_0 - 8);
                }
            }
        }

        internal List<MapPoint> method_0(bool bool_0)
        {
            List<MapPoint> list = new List<MapPoint>();
            int num = bool_0 ? 2 : 1;
            int i = bool_0 ? 1 : 0;
            while ((((num >> 31) ^ i) <= ((num >> 31) ^ 7)))
            {
                MapPoint nearestCellInDirection = this.v_OriginPoint.GetNearestCellInDirection(i, 1);
                if (((nearestCellInDirection.IsInMap() && (nearestCellInDirection.CellId < 560)) && (nearestCellInDirection.CellId > 0)))
                {
                    list.Add(nearestCellInDirection);
                }
                i = (i + num);
            }
            return list;
        }


        // Fields
        internal SimpleCellInfo class13_0;
        internal int int_0;
        internal int int_1;
        internal MapPoint v_OriginPoint;
    }
}