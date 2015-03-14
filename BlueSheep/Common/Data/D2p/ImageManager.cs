using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace BlueSheep.Data.D2p
{
    public class ImageManager
    {
        private static FileStream mystream { get; set; }
        private static object mDofusPath { get; set; }

        public static Dictionary<string, int[]> DictionnaryItemGFX = new Dictionary<string, int[]>();
        public static void Init(string DofusPath)
        {
            mDofusPath = DofusPath;
            string directoryPath = Path.Combine (DofusPath, "content", "gfx", "items");
            foreach (string File in Directory.GetFiles(directoryPath))
            {
                if (File.Contains("bitmap"))
                {
                    mystream = new FileStream(File, FileMode.Open, FileAccess.Read);
                    byte num = Convert.ToByte(mystream.ReadByte() + mystream.ReadByte());
                    if (num == 3)
                    {
                        mystream.Position = mystream.Length - 0x18L;
                        int num2 = Convert.ToInt32(readUInt());
                        readUInt();
                        int num3 = Convert.ToInt32(readUInt());
                        int num4 = Convert.ToInt32(readUInt());
                        int num1 = Convert.ToInt32(readUInt());
                        int num10 = Convert.ToInt32(readUInt());
                        mystream.Position = num3;
                        int num5 = num4;
                        for (int i = 1; i <= num5; i++)
                        {
                            string key = readString();
                            int num7 = (int)(readUInt() + num2);
                            int num8 = (int)(readUInt());
                            DictionnaryItemGFX.Add(key, new int[] {
								num7,
								num8
							});
                        }
                        mystream.Close();
                    }
                }
            }
        }

        private static short readShort()
        {
            return BitConverter.ToInt16(ImageManager._InverseArray(ImageManager.readBytes(2)), 0);
        }

        private static string readString()
        {
            int lenght = ImageManager.readShort();
            byte[] bytes = ImageManager.readBytes(lenght);
            return Encoding.UTF8.GetString(bytes);
        }

        private static byte[] readBytes(int lenght)
        {
            byte[] destinationArray = new byte[lenght];
            for (int i = 0; i <= lenght - 1; i++)
            {
                destinationArray[i] = (byte)mystream.ReadByte();
            }
            return destinationArray;
        }

        private static UInt32 readUInt()
        {
            return BitConverter.ToUInt32(ImageManager._InverseArray(ImageManager.readBytes(4)), 0);
        }

        private static byte[] _InverseArray(byte[] source)
        {
            byte[] buffer = new byte[source.Length];
            int i = 0;
            for (i = 0; i <= source.Length - 1; i++)
            {
                buffer[i] = source[((source.Length - 1) - i)];
            }
            return buffer;
        }

        public static Image GetImage(int IconId)
        {
            for (int i = 0; i <= 1; i++)
            {
                try
                {
                    Image GFXItem = null;
                    string directoryPath = Path.Combine ((string)mDofusPath, "content", "gfx", "items", "bitmap" + i + ".d2p");
                    mystream = new FileStream(directoryPath, FileMode.Open, FileAccess.Read);
                    int[] numArray = DictionnaryItemGFX[IconId.ToString() + ".png"];
                    mystream.Position = numArray[0];
                    byte[] buffer = readBytes(numArray[1]);
                    MemoryStream stream = new MemoryStream(buffer, 0, buffer.Length);
                    mystream.Close();
                    GFXItem = Image.FromStream(stream);
                    return GFXItem;
                }
                catch
                {
                }
            }
            return null;
        }
    }
}
