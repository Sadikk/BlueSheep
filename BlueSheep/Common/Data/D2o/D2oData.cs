using BlueSheep.Common.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueSheep.Common.Data.D2o
{
    internal sealed class D2oData
    {
        // Methods
        internal D2oData(string D2oFile)
        {
            this.D2oFileStream = new FileStream(D2oFile, FileMode.Open, FileAccess.Read);
            this.dictionary_1 = new Dictionary<int, int>();
            this.dictionary_0 = new Dictionary<int, Class16>();
            this.Reader = new BigEndianReader(this.D2oFileStream);
            Encoding.Default.GetString(this.Reader.ReadBytes(3));
            this.D2oFileStream.Position = this.Reader.ReadInt();
            int num = this.Reader.ReadInt();
            int i = 1;
            while ((i <= num))
            {
                this.dictionary_1.Add(this.Reader.ReadInt(), this.Reader.ReadInt());
                this.int_0 += 1;
                i = (i + 8);
            }
            int num3 = this.Reader.ReadInt();
            int j = 1;
            while ((j <= num3))
            {
                this.method_2(this.Reader.ReadInt());
                j += 1;
            }
        }

        internal DataClass DataObject(string File, int Id)
        {
            if (!this.Id_Data.ContainsKey(Id))
            {
                if (!this.dictionary_1.ContainsKey(Id))
                {
                    return null;
                }
                this.D2oFileStream.Position = Convert.ToInt64(this.dictionary_1[Id]);
                int key = this.Reader.ReadInt();
                if (this.dictionary_0.ContainsKey(key))
                {
                    this.Id_Data.Add(Id, this.dictionary_0[key].method_0(File, this.Reader));
                }
                else
                {
                    this.Id_Data.Add(Id, null);
                }
            }
            return this.Id_Data[Id];
        }

        internal DataClass[] DataObjects(string string_0)
        {
            List<DataClass> list = new List<DataClass>();
            int num = 0;
            foreach (int num_loopVariable in this.dictionary_1.Keys)
            {
                num = num_loopVariable;
                this.D2oFileStream.Position = Convert.ToInt64(this.dictionary_1[num]);
                int key = this.Reader.ReadInt();
                if (this.dictionary_0.ContainsKey(key))
                {
                    list.Add(this.dictionary_0[key].method_0(string_0, this.Reader));
                }
            }
            return list.ToArray();
        }

        private void method_2(int int_1)
        {
            Class16 class2 = new Class16(this.Reader.ReadUTF(), this.Reader.ReadUTF());
            int num2 = this.Reader.ReadInt();
            int i = 1;
            while ((i <= num2))
            {
                class2.method_1(this.Reader.ReadUTF(), this.Reader);
                i += 1;
            }
            this.dictionary_0.Add(int_1, class2);
        }


        // Fields
        internal Dictionary<int, Class16> dictionary_0;
        private Dictionary<int, int> dictionary_1;
        private Dictionary<int, DataClass> Id_Data = new Dictionary<int, DataClass>();
        private BigEndianReader Reader;
        private FileStream D2oFileStream;
        private int int_0 = 0;
    }
}
