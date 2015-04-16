using System;
using System.Collections.Generic;
using BlueSheep.Data.Pathfinding.Positions;
using BlueSheep.Data.D2p;
using BlueSheep.Core.Fight;
using BlueSheep.Engine.Types;
using BlueSheep.Common.Types;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Core.Map;

namespace BlueSheep.Data.Pathfinding
{
    public class SimplePathfinder
    {
        // Methods
        public SimplePathfinder(MapData Map)
        {
            this.MapData = Map;
            if (Map.Id == 2561)
            {
                this.ListCellIdFighters.Add(53);
            }
        }

        public MovementPath FindPath(int FromCell, int ToCell)
        {
            SimpleCellInfo class3 = null;
            this.MapPoint_FromCell = new MapPoint(FromCell);
            this.MapPoint_ToCell = new MapPoint(ToCell);
            SimpleCellInfo item = new SimpleCellInfo(this.MapPoint_FromCell);
            this.list_0.Add(item);
        Label_00BF:
            class3 = this.method_0();
            if (class3 != null)
            {
                this.list_0.Remove(class3);
                this.list_1.Add(class3);
                if (class3.v_OriginPoint.CellId == ToCell)
                {
                    return this.method_3(class3);
                }
                //INSTANT C# NOTE: Commented this declaration since looping variables in 'foreach' loops are declared in the 'foreach' header in C#:
                //				MapPoint point = null;
                foreach (MapPoint point in class3.method_0(this.IsInFight))
                {
                    if (Convert.ToBoolean(this.method_2(point.CellId)))
                    {
                        this.method_1(new SimpleCellInfo(this.MapData, point, class3, this.MapPoint_ToCell));
                    }
                }
                goto Label_00BF;
            }
            return null;
        }

        private SimpleCellInfo method_0()
        {
            SimpleCellInfo class2 = null;
            //INSTANT C# NOTE: Commented this declaration since looping variables in 'foreach' loops are declared in the 'foreach' header in C#:
            //			SimpleCellInfo class3 = null;
            foreach (SimpleCellInfo class3 in this.list_0)
            {
                if ((class2 == null) || ((class2.int_0 + class2.int_1) > (class3.int_0 + class3.int_1)))
                {
                    class2 = class3;
                }
            }
            return class2;
        }

        private void method_1(SimpleCellInfo class13_0)
        {
            //INSTANT C# NOTE: Commented this declaration since looping variables in 'foreach' loops are declared in the 'foreach' header in C#:
            //			SimpleCellInfo class2 = null;
            foreach (SimpleCellInfo class2 in this.list_0)
            {
                if ((class2.v_OriginPoint.CellId == class13_0.v_OriginPoint.CellId) && ((class2.int_0 + class2.int_1) <= (class13_0.int_0 + class13_0.int_1)))
                {
                    return;
                }
            }
            //INSTANT C# NOTE: Commented this declaration since looping variables in 'foreach' loops are declared in the 'foreach' header in C#:
            //			SimpleCellInfo class3 = null;
            foreach (SimpleCellInfo class3 in this.list_1)
            {
                if ((class3.v_OriginPoint.CellId == class13_0.v_OriginPoint.CellId) && ((class3.int_0 + class3.int_1) <= (class13_0.int_0 + class13_0.int_1)))
                {
                    return;
                }
            }
            this.list_0.Add(class13_0);
        }

        private object method_2(int int_1)
        {
            if ((this.MapPoint_FromCell.CellId == int_1) || (this.MapPoint_ToCell.CellId == int_1))
            {
                return true;
            }
            if (this.ListCellIdFighters.Contains(int_1))
            {
                return false;
            }
            return this.MapData.Data.Cells[int_1].Mov();
        }

        private MovementPath method_3(SimpleCellInfo class13_0)
        {
            List<MapPoint> range = new List<MapPoint>();
            SimpleCellInfo class2 = class13_0;
            while (class2.v_OriginPoint.CellId != this.MapPoint_FromCell.CellId)
            {
                class2 = class2.class13_0;
                range.Add(class2.v_OriginPoint);
            }
            range.Reverse();
            range.Add(class13_0.v_OriginPoint);
            if (this.v_MouvementPoints != -1)
            {
                range = range.GetRange(0, (((this.v_MouvementPoints + 1) > range.Count) ? range.Count : (this.v_MouvementPoints + 1)));
            }
            MovementPath path = new MovementPath { CellStart = this.MapPoint_FromCell, CellEnd = range[(range.Count - 1)] };
            int num = (range.Count - 2);
            int i = 0;
            while (i <= num)
            {
                PathElement item = new PathElement { Cell = range[i], Orientation = range[i].OrientationTo(range[(i + 1)]) };
                path.Cells.Add(item);
                i += 1;
            }
            path.Compress();
            return path;
        }

        public void SetFight(List<BFighter> Fighters, int MovementPoints)
        {
            //INSTANT C# NOTE: Commented this declaration since looping variables in 'foreach' loops are declared in the 'foreach' header in C#:
            //			GameFightFighterInformations informations = null;
            foreach (BFighter fighter in Fighters)
            {
                if (fighter.IsAlive)
                    this.ListCellIdFighters.Add(fighter.CellId);
            }
            this.v_MouvementPoints = MovementPoints;
            this.IsInFight = true;
        }

        // Fields
        private bool IsInFight = false;
        private int v_MouvementPoints = -1;
        private List<SimpleCellInfo> list_0 = new List<SimpleCellInfo>();
        private List<SimpleCellInfo> list_1 = new List<SimpleCellInfo>();
        private List<int> ListCellIdFighters = new List<int>();
        private MapData MapData;
        private MapPoint MapPoint_FromCell;
        private MapPoint MapPoint_ToCell;
    }
}