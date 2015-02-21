using BlueSheep.Common.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlueSheep.Data.D2p.Elements
{
    public class GraphicalElement : BasicElement
    {
        // Methods
        internal override void Init(BigEndianReader Reader, int MapVersion)
        {
            this.ElementId = Convert.ToInt32(Reader.ReadUInt());
            this.Hue = new ColorMultiplicator(Reader.ReadSByte(), Reader.ReadSByte(), Reader.ReadSByte());
            this.Shadow = new ColorMultiplicator(Reader.ReadSByte(), Reader.ReadSByte(), Reader.ReadSByte());
            if ((MapVersion <= 4))
            {
                this.OffsetX = Reader.ReadSByte();
                this.OffsetY = Reader.ReadSByte();
                this.PixelOffsetX = (this.OffsetX * 43);
                this.PixelOffsetY = (this.OffsetY * 21.5);
            }
            else
            {
                this.PixelOffsetX = Reader.ReadShort();
                this.PixelOffsetY = Reader.ReadShort();
                this.OffsetX = (this.PixelOffsetX / 43);
                this.OffsetY = (this.PixelOffsetY / 21.5);
            }
            this.Altitude = Reader.ReadSByte();
            this.Identifier = Convert.ToInt32(Reader.ReadUInt());
        }


        // Fields
        public int Altitude;
        public int ElementId;
        public ColorMultiplicator FinalTeint;
        public ColorMultiplicator Hue;
        public int Identifier;
        public double OffsetX;
        public double OffsetY;
        public double PixelOffsetX;
        public double PixelOffsetY;
        public ColorMultiplicator Shadow;
    }
}
