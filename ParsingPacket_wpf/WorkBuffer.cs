using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ParsingPacket_wpf
{
    public static class WorkBuffer
    {
        public static float GetFloat(ref List<byte> list)
        {
            float data = 0;
            byte[] bytes = new byte[sizeof(UInt32)];

            for (int i = 0; i < sizeof(UInt32); i++)
                bytes[i] = GetByte(ref list);

            data = BitConverter.ToSingle(bytes, 0);

            return data;
        }

        public static byte GetByte(ref List<byte> list)
        {
            byte data = list[0];
            list.RemoveAt(0);
            return data;
        }

        public static UInt16 GetUInt16(ref List<byte> list)
        {
            UInt16 data = 0;
            byte[] bytes = new byte[sizeof(UInt16)];

            for (int i = 0; i < sizeof(UInt16); i++)
                bytes[i] = GetByte(ref list);

            data = BitConverter.ToUInt16(bytes, 0);

            return data;
        }

        public static UInt32 GetUInt32(ref List<byte> list)
        {
            UInt32 data = 0;
            byte[] bytes = new byte[sizeof(UInt32)];

            for (int i = 0; i < sizeof(UInt32); i++)
                bytes[i] = GetByte(ref list);

            data = BitConverter.ToUInt32(bytes, 0);
            return data;
        }

        public static UInt64 GetUInt64(ref List<byte> list)
        {
            UInt64 data = 0;
            byte[] bytes = new byte[sizeof(UInt64)];

            for (int i = 0; i < sizeof(UInt64); i++)
                bytes[i] = GetByte(ref list);

            data = BitConverter.ToUInt64(bytes, 0);
            return data;
        }
    }
}
