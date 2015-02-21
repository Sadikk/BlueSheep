using BlueSheep.Common.IO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BlueSheep.Common.Data.D2o
{
    internal sealed class Class17
    {
        // Methods
        internal Class17(string string_1, BigEndianReader dofusReader_0)
        {
            this.string_0 = string_1;
            int num = dofusReader_0.ReadInt();
            this.delegate0_0 = this.method_0(num, dofusReader_0);
        }

        private Delegate0 method_0(int int_0, BigEndianReader dofusReader_0)
        {
            switch (int_0)
            {
                case -1:
                    return new Delegate0(this.method_9);
                case -2:
                    return new Delegate0(this.method_10);
                case -3:
                    return new Delegate0(this.method_5);
                case -4:
                    return new Delegate0(this.method_11);
                case -5:
                    return new Delegate0(this.method_12);
                case -6:
                    return new Delegate0(this.method_13);
                case -99:
                    string str = dofusReader_0.ReadUTF();
                    this.class17_0 = new Class17(str, dofusReader_0);
                    return new Delegate0(this.method_1);
            }
            if ((int_0 > 0))
            {
                return new Delegate0(this.method_2);
            }
            return null;
        }

        private ArrayList method_1(string string_1, BigEndianReader dofusReader_0)
        {
            ArrayList list = new ArrayList();
            int num = dofusReader_0.ReadInt();
            int i = 1;
            while ((i <= num))
            {
                list.Add(RuntimeHelpers.GetObjectValue(this.class17_0.delegate0_0.Invoke(string_1, dofusReader_0)));
                i += 1;
            }
            return list;
        }

        [CompilerGenerated()]
        private object method_10(string string_1, BigEndianReader dofusReader_0)
        {
            return this.method_4(string_1, dofusReader_0);
        }

        [CompilerGenerated()]
        private object method_11(string string_1, BigEndianReader dofusReader_0)
        {
            return this.method_6(string_1, dofusReader_0);
        }

        [CompilerGenerated()]
        private object method_12(string string_1, BigEndianReader dofusReader_0)
        {
            return this.method_7(string_1, dofusReader_0);
        }

        [CompilerGenerated()]
        private object method_13(string string_1, BigEndianReader dofusReader_0)
        {
            return this.method_8(string_1, dofusReader_0);
        }

        private object method_2(string string_1, BigEndianReader dofusReader_0)
        {
            int num = dofusReader_0.ReadInt();
            return GameData.FileName_Data[string_1].dictionary_0[num].method_0(string_1, dofusReader_0);
        }

        private int method_3(string string_1, BigEndianReader dofusReader_0)
        {
            return dofusReader_0.ReadInt();
        }

        private bool method_4(string string_1, BigEndianReader dofusReader_0)
        {
            return dofusReader_0.ReadBoolean();
        }

        private string method_5(string string_1, BigEndianReader dofusReader_0)
        {
            return dofusReader_0.ReadUTF();
        }

        private double method_6(string string_1, BigEndianReader dofusReader_0)
        {
            return dofusReader_0.ReadDouble();
        }

        private int method_7(string string_1, BigEndianReader dofusReader_0)
        {
            return dofusReader_0.ReadInt();
        }

        private UInt32 method_8(string string_1, BigEndianReader dofusReader_0)
        {
            return dofusReader_0.ReadUInt();
        }

        [CompilerGenerated()]
        private object method_9(string string_1, BigEndianReader dofusReader_0)
        {
            return this.method_3(string_1, dofusReader_0);
        }


        // Fields
        private Class17 class17_0;
        internal Delegate0 delegate0_0;

        internal string string_0 = "";
        // Nested Types
        internal delegate object Delegate0(string ModuleName, BigEndianReader Reader);
    }

}
