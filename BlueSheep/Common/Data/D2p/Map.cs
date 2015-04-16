using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BlueSheep.Common.IO;
using System.IO;

namespace BlueSheep.Data.D2p
{
    public class Map
    {
        // Methods
        public bool IsLineOfSight(int cellId)
        {
            return (((cellId >= 0) && (cellId < this.CellsCount)) && this.Cells[cellId].Los());
        }

        public bool IsWalkable(int cellId)
        {
            return (((cellId >= 0) && (cellId < this.CellsCount)) && this.Cells[cellId].Mov());
        }

        internal void Init(BigEndianReader reader)
        {
            reader.ReadByte();
            this.MapVersion = reader.ReadByte();
            this.Id = (int)reader.ReadUInt();
            if (this.MapVersion >= 7)
            {
                this.Encrypted = reader.ReadBoolean();
                this.EncryptedVersion = reader.ReadByte();
                int count = reader.ReadInt();
                if (this.Encrypted)
                {
                    byte[] buffer = CustomHex.ToArray(CustomHex.FromString("649ae451ca33ec53bbcbcc33becf15f4", false));
                    byte[] buffer2 = reader.ReadBytes(count);
                    int num7 = (buffer2.Length - 1);
                    int n = 0;
                    while (n <= num7)
                    {
                        buffer2[n] = Convert.ToByte((buffer2[n] ^ buffer[(n % buffer.Length)]));
                        n += 1;
                    }
                    reader = new BigEndianReader(buffer2);
                }
            }
            this.RelativeId = (int)reader.ReadUInt();
            this.MapType = reader.ReadByte();
            this.SubAreaId = reader.ReadInt();
            this.TopNeighbourId = reader.ReadInt();
            this.BottomNeighbourId = reader.ReadInt();
            this.LeftNeighbourId = reader.ReadInt();
            this.RightNeighbourId = reader.ReadInt();
            this.ShadowBonusOnEntities = reader.ReadInt();
            if (this.MapVersion >= 3)
            {
                this.BackgroundRed = reader.ReadByte();
                this.BackgroundGreen = reader.ReadByte();
                this.BackgroundBlue = reader.ReadByte();
            }
            if (this.MapVersion >= 4)
            {
                this.ZoomScale = (Convert.ToDouble(reader.ReadUShort()) / 100);
                this.ZoomOffsetX = reader.ReadShort();
                this.ZoomOffsetY = reader.ReadShort();
            }
            this.UseLowPassFilter = reader.ReadBoolean();
            this.UseReverb = reader.ReadBoolean();
            if (this.UseReverb)
            {
                this.PresetId = reader.ReadInt();
            }
            this.BackgroundsCount = reader.ReadByte();
            int backgroundsCount = this.BackgroundsCount;
            int i = 1;
            while (i <= backgroundsCount)
            {
                Fixture item = new Fixture();
                item.Init(reader);
                this.BackgroundFixtures.Add(item);
                i += 1;
            }
            this.ForegroundsCount = reader.ReadByte();
            int foregroundsCount = this.ForegroundsCount;
            int j = 1;
            while (j <= foregroundsCount)
            {
                Fixture fixture2 = new Fixture();
                fixture2.Init(reader);
                this.ForegroundFixtures.Add(fixture2);
                j += 1;
            }
            this.CellsCount = 560;
            reader.ReadInt();
            this.GroundCRC = reader.ReadInt().ToString();
            this.LayersCount = reader.ReadByte();
            int layersCount = this.LayersCount;
            int k = 1;
            while (k <= layersCount)
            {
                Layer layer = new Layer();
                layer.Init(reader, this.MapVersion);
                this.Layers.Add(layer);
                k += 1;
            }
            int cellsCount = this.CellsCount;
            int m = 1;
            uint oldMvtSys = 0;
            while (m <= cellsCount)
            {
                CellData data = new CellData();
                data.Init(reader, this.MapVersion);
                if (oldMvtSys == 0)
                {
                    oldMvtSys = (uint)data.MoveZone;
                }
                if (data.MoveZone != oldMvtSys)
                {
                    IsUsingNewMovementSystem = true;
                }
                this.Cells.Add(data);
                m += 1;
            }
        }

        // Fields
        public int BackgroundBlue;
        public List<Fixture> BackgroundFixtures = new List<Fixture>();
        public int BackgroundGreen;
        public int BackgroundRed;
        public int BackgroundsCount;
        public int BottomNeighbourId;
        public List<CellData> Cells = new List<CellData>();
        public int CellsCount;
        public bool Encrypted;
        public int EncryptedVersion;
        public List<Fixture> ForegroundFixtures = new List<Fixture>();
        public int ForegroundsCount;
        public string GroundCRC;
        public int Id;
        public bool IsUsingNewMovementSystem;
        public List<Layer> Layers = new List<Layer>();
        public int LayersCount;
        public int LeftNeighbourId;
        public int MapType;
        public int MapVersion;
        public int PresetId;
        public int RelativeId;
        public int RightNeighbourId;
        public int ShadowBonusOnEntities;
        public int SubAreaId;
        public int TopNeighbourId;
        public bool UseLowPassFilter;
        public bool UseReverb;
        public int ZoomOffsetX;
        public int ZoomOffsetY;
        public double ZoomScale;
    }
}
