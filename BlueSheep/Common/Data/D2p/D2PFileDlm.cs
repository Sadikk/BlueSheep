using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using BlueSheep.Common.IO;

namespace BlueSheep.Data.D2p
{
    internal sealed class D2PFileDlm
    {
        // Methods
        internal D2PFileDlm(string D2pFilePath)
        {
            this.D2pFileStream = new FileStream(D2pFilePath, FileMode.Open, FileAccess.Read);
            this.Reader = new BigEndianReader(this.D2pFileStream);
            this.FilenameDataDictionnary = new Dictionary<string, int[]>();
            this.CheckLock = RuntimeHelpers.GetObjectValue(new object());
            byte num = Convert.ToByte((this.Reader.ReadByte() + this.Reader.ReadByte()));
            if ((num == 3))
            {
                this.D2pFileStream.Position = (this.D2pFileStream.Length - 24);
                int num2 = Convert.ToInt32(this.Reader.ReadUInt());
                this.Reader.ReadUInt();
                int num3 = Convert.ToInt32(this.Reader.ReadUInt());
                int num4 = Convert.ToInt32(this.Reader.ReadUInt());
                int num1 = Convert.ToInt32(this.Reader.ReadUInt());
                int num9 = Convert.ToInt32(this.Reader.ReadUInt());
                this.D2pFileStream.Position = num3;
                int num5 = num4;
                int i = 1;
                while ((i <= num5))
                {
                    string key = this.Reader.ReadUTF();
                    int num7 = (this.Reader.ReadInt() + num2);
                    int num8 = this.Reader.ReadInt();
                    this.FilenameDataDictionnary.Add(key, new int[] {
					num7,
					num8
				});
                    i += 1;
                }
            }
        }

        internal bool ExistsDlm(string DlmName)
        {
            return this.FilenameDataDictionnary.ContainsKey(DlmName);
        }

        internal byte[] ReadFile(string fileName)
        {
            lock (this.CheckLock)
            {
                int[] numArray = this.FilenameDataDictionnary[fileName];
                this.D2pFileStream.Position = numArray[0];
                return this.Reader.ReadBytes(numArray[1]);
            }
        }


        // Fields
        private Dictionary<string, int[]> FilenameDataDictionnary;
        private BigEndianReader Reader;
        private FileStream D2pFileStream;
        private object CheckLock;
    }
}
