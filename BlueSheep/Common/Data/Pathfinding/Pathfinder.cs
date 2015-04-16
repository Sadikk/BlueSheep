using BlueSheep.Data.Pathfinding.Positions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BlueSheep.Core.Fight;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Core.Map;
using BlueSheep.Engine.Types;
using BlueSheep.Common.Types;

namespace BlueSheep.Data.Pathfinding
{
    public class Pathfinder
    {
        // Methods
        public Pathfinder(MapData map)
        {
            this.MapData = map;
            if (MapData.Id == 2561)
            {
                this.ListCellIdFighters.Add(53);
            }
        }


        public MovementPath FindPath(int FromCell, int ToCell, bool ate)
        {
            return FindPath(FromCell, ToCell, ate, true);
        }

        public MovementPath FindPath(int FromCell, int ToCell)
        {
            return FindPath(FromCell, ToCell, true, true);
        }

        public MovementPath FindPath(int FromCell, int ToCell, bool ate, bool adc)
        {
            MovPath = new MovementPath();
            MovPath.CellStart = new MapPoint(FromCell);
            MovPath.CellEnd = new MapPoint(ToCell);
            AllowDiag = adc;
            AllowDiagCornering = adc;
            AllowTroughEntity = ate;

            StartPathfinding(new MapPoint(FromCell), new MapPoint(ToCell));
            ProcessPathfinding();

            return MovPath;
        }

        public void StartPathfinding(MapPoint startP, MapPoint endP)
        {
            this.Start = startP;
            this.End = endP;
            this.StartX = startP.X;
            this.StartY = startP.Y;
            this.EndX = endP.X;
            this.EndY = endP.Y;

            this.StartPoint = new MapPoint(startP.X, startP.Y);
            this.EndPoint = new MapPoint(endP.X, endP.Y);

            this.AuxEndPoint = this.StartPoint;
            this.AuxEndX = this.StartPoint.X;
            this.AuxEndY = this.StartPoint.Y;

            this.DistanceToEnd = this.StartPoint.DistanceToCell(this.EndPoint);

            for (int y = -19; y <= this.MaxY; y++)
            {
                for (int x = 0; x <= this.MaxX; x++)
                {
                    this.MapStatus.Add(new CellInfo(0, null, false, false, x, y));
                }
            }
            OpenList = new List<OpenSquare>();
            OpenSquare(this.StartY, this.StartX, null, 0, 0, false);
        }

        public void ProcessPathfinding()
        {
            int actualX = 0;
            int actualY = 0;
            int speed = 0;
            int moveCost = 0;

            bool isDownRightEnd = false;
            bool isDownRightStart = false;
            bool isTopRightEnd = false;
            bool isTopRightStart = false;

            MapPoint actualPoint = null;
            int actualDistanceToEnd = 0;
            double heuristic = 0;
            uint square = 0;

            if (this.OpenList.Count > 0 && !(IsClosed(this.EndY, this.EndX)))
            {
                square = NearerSquare();
                this.NowY = this.OpenList[(int)square].Y;
                this.NowX = this.OpenList[(int)square].X;
                this.PreviousCellId = (new MapPoint(this.NowX, this.NowY)).CellId;
                CloseSquare(this.NowY, this.NowX);

                for (actualY = this.NowY - 1; actualY <= this.NowY + 1; actualY++)
                {
                    for (actualX = this.NowX - 1; actualX <= this.NowX + 1; actualX++)
                    {
                        if ((new MapPoint(actualX, actualY)).IsInMap())
                        {
                            if (actualY >= this.MinY && actualY < this.MaxY && actualX >= this.MinX && actualX < this.MaxX && !(actualY == this.NowY && actualX == this.NowX) && ((this.AllowDiag) || actualY == this.NowY || actualX == this.NowX && ((AllowDiagCornering) || actualY == this.NowY || actualX == this.NowX || (PointMov(this.NowX, this.NowY,  this.PreviousCellId, this.AllowTroughEntity)) || (PointMov(actualX, this.NowY, this.PreviousCellId, this.AllowTroughEntity)))))
                            {
                                if (!(!(PointMov(this.NowX, actualY, this.PreviousCellId, this.AllowTroughEntity)) && !(PointMov(actualX, this.NowY, this.PreviousCellId, this.AllowTroughEntity)) && !this.IsFighting && (this.AllowDiag)))
                                {
                                    if (PointMov(actualX, actualY, this.PreviousCellId, this.AllowTroughEntity))
                                    {
                                        if (!(IsClosed(actualY, actualX)))
                                        {
                                            if (actualX == this.EndX && actualY == this.EndY)
                                            {
                                                speed = 1;
                                            }
                                            else
                                            {
                                                speed = (int)GetCellSpeed((new MapPoint(actualX, actualY)).CellId, AllowTroughEntity);
                                            }

                                            moveCost = this.GetCellInfo(this.NowY, this.NowX).MovementCost + ((actualY == this.NowY || actualX == this.NowX) ? this.HVCost : this.DCost) * speed;

                                            if (AllowTroughEntity)
                                            {
                                                isDownRightEnd = actualX + actualY == this.EndX + this.EndY;
                                                isDownRightStart = actualX + actualY == this.StartX + this.StartY;
                                                isTopRightEnd = actualX - actualY == this.EndX - this.EndY;
                                                isTopRightStart = actualX - actualY == this.StartX - this.StartY;
                                                actualPoint = new MapPoint(actualX, actualY);

                                                if (!isDownRightEnd && !isTopRightEnd || !isDownRightStart && !isTopRightStart)
                                                {
                                                    moveCost = moveCost + actualPoint.DistanceToCell(this.EndPoint);
                                                    moveCost = moveCost + actualPoint.DistanceToCell(this.StartPoint);
                                                }

                                                if (actualX == this.EndX || actualY == this.EndY)
                                                {
                                                    moveCost = moveCost - 3;
                                                }
                                                if ((isDownRightEnd) || (isTopRightEnd) || actualX + actualY == this.NowX + this.NowY || actualX - actualY == this.NowX - this.NowY)
                                                {
                                                    moveCost = moveCost - 2;
                                                }
                                                if (actualX == this.StartX || actualY == this.StartY)
                                                {
                                                    moveCost = moveCost - 3;
                                                }
                                                if ((isDownRightStart) || (isTopRightStart))
                                                {
                                                    moveCost = moveCost - 2;
                                                }

                                                actualDistanceToEnd = actualPoint.DistanceToCell(this.EndPoint);
                                                if (actualDistanceToEnd < this.DistanceToEnd)
                                                {
                                                    if (actualX == this.EndX || actualY == this.EndY || actualX + actualY == this.EndX + this.EndY || actualX - actualY == this.EndX - this.EndY)
                                                    {
                                                        this.AuxEndPoint = actualPoint;
                                                        this.AuxEndX = actualX;
                                                        this.AuxEndY = actualY;
                                                        this.DistanceToEnd = actualDistanceToEnd;
                                                    }
                                                }
                                            }

                                            if (IsOpened(actualY, actualX))
                                            {
                                                if (moveCost < this.GetCellInfo(actualY, actualX).MovementCost)
                                                {
                                                    this.OpenSquare(actualY, actualX, new[] { this.NowY, this.NowX }, moveCost, 0, true);
                                                }
                                            }
                                            else
                                            {
                                                heuristic = Convert.ToDouble(this.HeuristicCost) * Math.Sqrt((this.EndY - actualY) * (this.EndY - actualY) + (this.EndX - actualX) * (this.EndX - actualX));
                                                OpenSquare(actualY, actualX, new[] { this.NowY, this.NowX }, moveCost, heuristic, false);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                ProcessPathfinding();
            }
            else
            {
                EndPathfinding();
            }
        }

        public void EndPathfinding()
        {
            List<MapPoint> mapsArray = new List<MapPoint>();
            int parentY = 0;
            int parentX = 0;
            MapPoint btwPoint = null;
            List<MapPoint> tempArray = new List<MapPoint>();
            int i = 0;
            int actualX = 0;
            int actualY = 0;
            int thirdX = 0;
            int thirdY = 0;
            int btwX = 0;
            int btwY = 0;
            bool endPointClosed = IsClosed(this.EndY, this.EndX);

            if (!endPointClosed)
            {
                this.EndPoint = this.AuxEndPoint;
                this.EndX = this.AuxEndX;
                this.EndY = this.AuxEndY;
                endPointClosed = true;
                MovPath.CellEnd = this.EndPoint;
            }
            this.PreviousCellId = -1;
            if (endPointClosed)
            {
                this.NowX = this.EndX;
                this.NowY = this.EndY;

                while (!(this.NowX == this.StartX) || !(this.NowY == this.StartY))
                {
                    mapsArray.Add(new MapPoint(this.NowX, this.NowY));
                    parentY = this.GetCellInfo(this.NowY, this.NowX).Parent[0];
                    parentX = this.GetCellInfo(this.NowY, this.NowX).Parent[1];
                    this.NowX = parentX;
                    this.NowY = parentY;
                }
                mapsArray.Add(this.StartPoint);
                if (AllowDiag)
                {
                    for (i = 0; i < mapsArray.Count; i++)
                    {
                        tempArray.Add(mapsArray[i]);
                        this.PreviousCellId = mapsArray[i].CellId;
                        if ((mapsArray.Count > i + 2 && !((mapsArray[i + 2] == null)) && (mapsArray[i].DistanceToCell(mapsArray[i + 2]) == 1)) && (!(IsChangeZone(mapsArray[i].CellId, mapsArray[i + 1].CellId))) && !(IsChangeZone(mapsArray[i + 1].CellId, mapsArray[i + 2].CellId)))
                        {
                            i += 1;
                        }
                        else
                        {
                            if ((mapsArray.Count > i + 3 && !((mapsArray[i + 3] == null))) && mapsArray[i].DistanceToCell(mapsArray[i + 3]) == 2)
                            {
                                actualX = mapsArray[i].X;
                                actualY = mapsArray[i].Y;
                                thirdX = mapsArray[i + 3].X;
                                thirdY = mapsArray[i + 3].Y;
                                btwX = actualX + (int)Math.Round((thirdX - actualX) / 2.0);
                                btwY = actualY + (int)Math.Round((thirdY - actualY) / 2.0);
                                btwPoint = new MapPoint(btwX, btwY);
                                if ((PointMov(btwX, btwY, this.PreviousCellId, true)) && GetCellSpeed(btwPoint.CellId, this.AllowTroughEntity) < 2)
                                {
                                    tempArray.Add(btwPoint);
                                    this.PreviousCellId = btwPoint.CellId;
                                    i += 2;
                                }
                            }
                            else
                            {
                                if (mapsArray.Count > i + 2 && !((mapsArray[i + 2] == null)) && mapsArray[i].DistanceToCell(mapsArray[i + 2]) == 2)
                                {
                                    actualX = mapsArray[i].X;
                                    actualY = mapsArray[i].Y;
                                    thirdX = mapsArray[i + 2].X;
                                    thirdY = mapsArray[i + 2].Y;
                                    btwX = mapsArray[i + 1].X;
                                    btwY = mapsArray[i + 1].Y;

                                    if (actualX + actualY == thirdX + thirdY && !(actualX - actualY == btwX - btwY) && !(IsChangeZone((new MapPoint(actualX, actualY)).CellId, (new MapPoint(btwX, btwY)).CellId)) && !(IsChangeZone((new MapPoint(btwX, btwY)).CellId, (new MapPoint(thirdX, thirdY)).CellId)))
                                    {
                                        i += 1;
                                    }
                                    else
                                    {
                                        if (actualX - actualY == thirdX - thirdY && !(actualX - actualY == btwX - btwY) && !(IsChangeZone((new MapPoint(actualX, actualY)).CellId, (new MapPoint(btwX, btwY)).CellId)) && !(IsChangeZone((new MapPoint(btwX, btwY)).CellId, (new MapPoint(thirdX, thirdY)).CellId)))
                                        {
                                            i += 1;
                                        }
                                        else
                                        {
                                            if (actualX == thirdX && !(actualX == btwX) && GetCellSpeed((new MapPoint(actualX, btwY)).CellId, this.AllowTroughEntity) < 2 && (PointMov(actualX, btwY, this.PreviousCellId, this.AllowTroughEntity)))
                                            {
                                                btwPoint = new MapPoint(actualX, btwY);
                                                tempArray.Add(btwPoint);
                                                this.PreviousCellId = btwPoint.CellId;
                                                i += 1;
                                            }
                                            else
                                            {
                                                if (actualY == thirdY && !(actualY == btwY) && GetCellSpeed((new MapPoint(btwX, actualY)).CellId, this.AllowTroughEntity) < 2 && (PointMov(btwX, actualY, this.PreviousCellId, this.AllowTroughEntity)))
                                                {
                                                    btwPoint = new MapPoint(btwX, actualY);
                                                    tempArray.Add(btwPoint);
                                                    this.PreviousCellId = btwPoint.CellId;
                                                    i += 1;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    mapsArray = tempArray;
                }
                if (mapsArray.Count == 1)
                {
                    mapsArray = new List<MapPoint>();
                }
                mapsArray.Reverse();
                MovementPathFromArray(mapsArray.ToArray());
            }
        }


        public bool PointMov(int x, int y, int cellId)
        {
            return PointMov(x, y, cellId, true);
        }

        public bool PointMov(int x, int y)
        {
            return PointMov(x, y, -1, true);
        }

        public bool PointMov(int x, int y, int cellId, bool troughtEntities)
        {
            bool isNewSystem = MapData.Data.IsUsingNewMovementSystem;
            MapPoint actualPoint = new MapPoint(x, y);
            BlueSheep.Data.D2p.CellData fCellData = null;
            BlueSheep.Data.D2p.CellData sCellData = null;
            bool mov = false;
            int floor = 0;

            if (actualPoint.IsInMap())
            {
                fCellData = MapData.Data.Cells[actualPoint.CellId];
                mov = ((fCellData.Mov()) && (!this.IsFighting || !fCellData.NonWalkableDuringFight()));

                if (!((mov == false)) && isNewSystem && cellId != -1 && cellId != actualPoint.CellId)
                {
                    sCellData = (MapData).Data.Cells[cellId];
                    floor = Math.Abs(Math.Abs(fCellData.Floor) - Math.Abs(sCellData.Floor));
                    if (!(sCellData.MoveZone == fCellData.MoveZone) && floor > 0 && sCellData.MoveZone == fCellData.MoveZone && fCellData.MoveZone == 0 && floor > 11)
                    {
                        mov = false;
                    }
                    if (!troughtEntities)
                    {
                        int count = 0;
                        count += MapData.Monsters.Where((e) => e.m_cellId == actualPoint.CellId).ToList().Count;
                        if (count > 0)
                        {
                        //    //ToDo Voir à travers les entity
                            return false;
                        }
                    }
                }
            }
            else
            {
                mov = false;
            }
            return mov;
        }

        public bool IsChangeZone(int fCell, int sCell)
        {
            BlueSheep.Data.D2p.Map data = MapData.Data;
            return data.Cells[fCell].MoveZone != data.Cells[sCell].MoveZone && System.Math.Abs(data.Cells[fCell].Floor) == System.Math.Abs(data.Cells[sCell].Floor);
        }

        private double GetCellSpeed(int cellId, bool throughEntities)
        {
            BlueSheep.Data.D2p.Map data = MapData.Data;
            var speed = data.Cells[cellId].Speed;
            MapPoint cell = new MapPoint(cellId);

            if (throughEntities)
            {
                if (!(MapData.NoEntitiesOnCell(cellId)))
                {
                    return 20;
                }

                if (speed >= 0)
                {
                    return 1 + (5 - speed);
                }

                return 1 + (11 + Math.Abs(speed));
            }

            var cost = 1.0D;
            MapPoint adjCell = null;

            if (!(MapData.NoEntitiesOnCell(cellId)))
            {
                cost += 0.3;
            }

            adjCell = new MapPoint(cell.X + 1, cell.Y);
            if (adjCell != null && !(MapData.NoEntitiesOnCell(adjCell.CellId)))
            {
                cost += 0.3;
            }

            adjCell = new MapPoint(cell.X, cell.Y + 1);
            if (adjCell != null && !(MapData.NoEntitiesOnCell(adjCell.CellId)))
            {
                cost += 0.3;
            }

            adjCell = new MapPoint(cell.X - 1, cell.Y);
            if (adjCell != null && !(MapData.NoEntitiesOnCell(adjCell.CellId)))
            {
                cost += 0.3;
            }

            adjCell = new MapPoint(cell.X, cell.Y - 1);
            if (adjCell != null && !(MapData.NoEntitiesOnCell(adjCell.CellId)))
            {
                cost += 0.3;
            }

            // todo
            //            if (m_context.IsCellMarked(cell))
            //                cost += 0.2;

            return cost;
        }

        public bool IsOpened(int y, int x)
        {
            return this.GetCellInfo(y, x).Opened;
        }

        public bool IsClosed(int y, int x)
        {
            CellInfo cellInfo = this.GetCellInfo(y, x);
            if ((cellInfo == null) || !cellInfo.Closed)
            {
                return false;
            }
            return true;
        }

        public uint NearerSquare()
        {
            uint y = 0;
            double distance = 9999999;
            double tempDistance = 0;

            for (int tempY = 0; tempY < this.OpenList.Count; tempY++)
            {
                tempDistance = this.GetCellInfo(this.OpenList[tempY].Y, this.OpenList[tempY].X).Heuristic + this.GetCellInfo(this.OpenList[tempY].Y, this.OpenList[tempY].X).MovementCost;
                if (tempDistance <= distance)
                {
                    distance = tempDistance;
                    y = Convert.ToUInt32(tempY);
                }
            }
            return y;
        }

        public void CloseSquare(int y, int x)
        {
            OpenList.RemoveAll((os) => os.X == x && os.Y == y);
            CellInfo cell = this.GetCellInfo(y, x);
            cell.Opened = false;
            cell.Closed = true;
        }

        public void OpenSquare(int y, int x, int[] parent, int moveCost, double heuristic, bool newSquare)
        {
            if (!newSquare)
            {
                foreach (OpenSquare op in this.OpenList)
                {
                    if (op.Y == y && op.X == x)
                    {
                        newSquare = true;
                        break;
                    }
                }
            }

            if (!newSquare)
            {
                this.OpenList.Add(new OpenSquare(y, x));
                this.MapStatus.RemoveAll((c) => c.X == x && c.Y == y);
                this.MapStatus.Add(new CellInfo(heuristic, null, true, false, x, y));
            }

            CellInfo cell = this.GetCellInfo(y, x);
            cell.Parent = parent;
            cell.MovementCost = moveCost;
        }

        public void MovementPathFromArray(MapPoint[] squares)
        {
            PathElement path = null;

            for (var i = 0; i <= squares.Length - 2; i++)
            {
                path = new PathElement();
                path.Cell = squares[i];
                path.Orientation = squares[i].OrientationTo(squares[i + 1]);
                this.MovPath.Cells.Add(path);
            }
            this.MovPath.Compress();
        }

        public void SetFight(List<BFighter> Fighters, int MovementPoints)
        {
            foreach (BFighter fighter in Fighters)
            {
                if (fighter.IsAlive)
                    this.ListCellIdFighters.Add(fighter.CellId);
            }
            this.MovePoint = MovementPoints;
            this.IsFighting = true;
        }

        public CellInfo GetCellInfo(int y, int x)
        {
            CellInfo cell = null;
            try
            {
                cell = MapStatus.First((ci) => ci.X == x && ci.Y == y);
            }
            catch
            {
                // TODO : Null cell
                cell = null;
            }
            return cell;
        }

        // Fields
        private bool IsFighting = false;
        private List<int> ListCellIdFighters = new List<int>();
        private int MovePoint = -1;

        private MapData MapData;
        //private Map Map;

        private List<CellInfo> MapStatus = new List<CellInfo>();
        private List<OpenSquare> OpenList = new List<OpenSquare>();

        private MapPoint Start;
        private MapPoint End;
        private MapPoint StartPoint;
        private MapPoint EndPoint;
        private MapPoint AuxEndPoint;
        private MovementPath MovPath;
        private int HVCost = 10;
        private int DCost = 20;
        private object HeuristicCost = 10;
        private bool AllowDiag;
        private bool AllowTroughEntity = true;
        private bool AllowDiagCornering = false;

        private int StartX;
        private int StartY;
        private int EndX;
        private int EndY;
        private int AuxEndX;
        private int AuxEndY;
        private int NowX;
        private int NowY;

        private int DistanceToEnd;
        private int PreviousCellId;

        private int MinX = 0;
        private int MaxX = 34;
        private int MinY = -19;
        private int MaxY = 14;
    }

}
