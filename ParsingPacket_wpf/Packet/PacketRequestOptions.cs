using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ParsingPacket_wpf.Packet
{
    class PacketRequestOptions : PacketBase
    {
        private UInt64 Packet_Time;	// Time create packet
        private byte[] CCID = new byte[20];       // CCID of device

        public PacketRequestOptions(List<byte> data)
        {
            if (Parsing(ref data) == false)
            {
                MessageBox.Show("Not correct data", "Warning", MessageBoxButton.OK);
                return;
            }

            Packet_Time = GetUint64(ref data);
            for (int i = 0; i < CCID.Length; i++)
            {
                CCID[i] = GetByte(ref data);
            }
            CRC32 = GetUInt32(ref data);
        }

        public List<Parameter> GetListParam()
        {
            SetBaseParam();

            Parameter p;

            p = TimestampToDate(Packet_Time);
            list.Add(p);

            return list;
        }
    }
}
